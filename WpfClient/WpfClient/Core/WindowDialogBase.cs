using System.Windows;

namespace WpfClient.Core
{
    public class WindowDialogBase : Window, IDialogView
    {
        public bool? ShowDialog(object owner)
        {
            var ownerWindow = owner as Window;
            Owner = ownerWindow;
            
            return this.ShowDialog();
        }
        public void Show(object owner)
        {
            var ownerWindow = owner as Window;
            Owner = ownerWindow;
            this.Show();
        }

        public void Ok()
        {
            DialogResult = true;
            Close();
        }
    }
}
