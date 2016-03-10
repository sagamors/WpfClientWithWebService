namespace WpfClient.Core
{
    public interface IDialog
    {
        bool? ShowDialog(object owner);
        void Show(object owner);
        void Close();
        void Ok();
    }
}
