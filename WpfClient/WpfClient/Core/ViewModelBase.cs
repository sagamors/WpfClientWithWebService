using System;
using System.Windows.Threading;
using JetBrains.Annotations;
using Ninject;
using Ninject.Parameters;
using WpfClient.Helpers;

namespace WpfClient.Core
{
    public abstract class ViewModelBase<TView> : NotificationBase, IViewModel<TView> where TView : IView
    {
        public bool IsBusy { protected set; get; }
        public bool ErrorDataLoading { protected set; get; }
        public string ErrorDataLodingMessage { protected set; get; }
        private readonly TView _view;
        public Dispatcher Dispatcher { private set;get; }

        public ViewModelBase([NotNull]IKernel container, [NotNull]TView view)
        {
            ValidateHelpers.NotNull(container, nameof(view));
            ValidateHelpers.NotNull(view, nameof(view));
            _view = view;
            Container = container;
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public IKernel Container { get; private set; }

        public TView View
        {
            get
            {
                return _view;
            }
        }

        protected void SetBusyState()
        {
            IsBusy = true;
            ErrorDataLoading = false;
            ErrorDataLodingMessage = String.Empty;
        }

        protected void ClearStates()
        {
            IsBusy = false;
            ErrorDataLodingMessage = String.Empty;
            ErrorDataLoading = false;
        }

        protected void SetErrorState(string error)
        {
            IsBusy = false;
            ErrorDataLodingMessage = error;
            ErrorDataLoading = true;
        }
    }
}

