using System.Windows;

namespace WpfClient.Core
{
    public interface IMessageBoxService
    {
        MessageBoxResult ShowError(string messageBoxText, string caption="Error");
    }
}