using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using JetBrains.Annotations;
using Ninject;
using Ninject.Parameters;
using RestProtocol;
using WpfClient.Api;
using WpfClient.Core;
using WpfClient.Helpers;
using WpfClient.Windows;

namespace WpfClient.ViewModels
{
    internal class LogonViewModel : OkCancelDialogViewModelBase<IDialogView>
    {
        private readonly IMessageBoxService _messageBoxService;
      
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public IEnumerable<User> Users { private set; get; }

        [Required(ErrorMessage = "Выберите пользователя")]
        public User SelectedUser { set; get; }

        public ICommand LoadUsersCommand { private set; get; }
        public LogonViewModel([NotNull] IKernel kernel, [NotNull] IDialogView view, [NotNull] IMessageBoxService messageBoxService) : base(kernel, view)
        {
            _messageBoxService = messageBoxService;
            ValidateHelpers.NotNull(messageBoxService, nameof(messageBoxService));
            Users = new ObservableCollection<User>();
            LoadUsersCommand = new RelayCommand(() => LoadUsersAsync(_cancellationTokenSource.Token), o => !IsBusy);
            LoadUsersAsync(_cancellationTokenSource.Token);
            SelectedUser = Users.FirstOrDefault();
        }

        protected override void Ok()
        {
            base.Ok();
            if (SelectedUser == null) return;
            var reports = Container.Get<ReportsViewModel>(new ConstructorArgument("user", SelectedUser));
            DisplayedView.Close();
            reports.ShowDialog(null);
        }

        protected override bool CanOkExecute(object o)
        {
            return SelectedUser!=null && !IsBusy && !ErrorDataLoading;
        }

        public void LoadUsersAsync(CancellationToken token)
        {
            SetBusyState();
            StartLoadUsersAsync(token);
        }

        private void StartLoadUsersAsync(CancellationToken token)
        {
            UsersProvider.GetUsersAsync(token).ContinueWith(task =>
            {
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        if (task.IsFaulted)
                        {
                            SetErrorState(task.Exception.AggregateMessages());
                            return;
                        }
                        if (task.IsCompleted)
                        {
                            ClearStates();
                            Users = task.Result;
                            SelectedUser = SelectedUser == null
                                ? Users.FirstOrDefault()
                                : Users.FirstOrDefault(user => SelectedUser.ID == user.ID);
                            CommandManager.InvalidateRequerySuggested();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                });
            return task.Result;
            }, token);
        }
    }
}
