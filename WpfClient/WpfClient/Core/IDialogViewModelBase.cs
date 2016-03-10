namespace WpfClient.Core
{
    public interface IDialogViewModelBase<TView> : IViewModel<TView> where TView : IDialogView
    {
        bool? ShowDialog(object owner);
        void Show(object owner);
    }

}
