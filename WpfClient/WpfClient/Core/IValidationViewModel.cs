using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfClient.Core
{
    public interface IValidationViewModel : INotifyDataErrorInfo
    {
        //bool IsValid { get; }
        //bool Validate();
        void ValidateProperty(object value, [CallerMemberName] string propertyName = null);
        void ValidateProperty([CallerMemberName] string propertyName = null);
    }
}
