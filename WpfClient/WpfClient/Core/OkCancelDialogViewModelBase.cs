using System.Windows.Input;
using JetBrains.Annotations;
using Ninject;
using WpfClient.Helpers;

namespace WpfClient.Core
{
    public abstract class OkCancelDialogViewModelBase<TView> : DialogViewModelBase<TView> where TView : IDialogView
    {
        public ICommand OkCommand { private set; get; }
        protected OkCancelDialogViewModelBase([NotNull]IKernel kernel, [NotNull]TView view) : base(kernel, view)
        {
            OkCommand = new RelayCommand(Ok, CanOkExecute);
        }

        protected virtual bool CanOkExecute(object o)
        {
            return !HasErrors && o is IDialogView;
        }

        protected virtual void Ok()
        {
            Validate();
        }
    }
}