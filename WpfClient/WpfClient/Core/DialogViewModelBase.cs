using JetBrains.Annotations;
using Ninject;

namespace WpfClient.Core
{
    public abstract class DialogViewModelBase<TView> : ValidationViewModelBase<TView>, IDialogViewModelBase<TView> where TView : IDialogView
    {
        public DialogViewModelBase([NotNull]IKernel container, [NotNull] TView view) : base(container, view)
        {

        }
        public TView DisplayedView { private set; get; }
        public void Show(object owner)
        {
            DisplayedView = View;
            DisplayedView.DataContext = this;
            DisplayedView.Show(owner);
        }

        public bool? ShowDialog(object owner)
        {
            DisplayedView = View;
            DisplayedView.DataContext = this;
            return DisplayedView.ShowDialog(owner);
        }

        public void Close()
        {
            DisplayedView?.Close();
        }
    }
}
