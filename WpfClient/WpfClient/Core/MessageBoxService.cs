using System.Windows;
using WpfClient.Windows;

namespace WpfClient.Core
{
    public class MessageBoxService : IMessageBoxService
    {
        public MessageBoxResult ShowError(string messageBoxText,string caption)
        {
            return MessageBox.Show(messageBoxText, caption,MessageBoxButton.OK,MessageBoxImage.Error);
        }
    }
}
