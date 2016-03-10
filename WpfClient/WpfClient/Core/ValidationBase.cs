using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace WpfClient.Core
{
    public abstract class  ValidationBase : NotificationBase, INotifyDataErrorInfo
    {
        private PropertyDescriptorCollection _properties;
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        [JsonIgnore]
        public bool HasErrors
        {
            get { return _errors.Any(propErrors => propErrors.Value != null && propErrors.Value.Count > 0); }
        }
        [JsonIgnore]
        public bool IsValid => !this.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (_errors.ContainsKey(propertyName) && (_errors[propertyName] != null) &&
                    _errors[propertyName].Count > 0)
                    return _errors[propertyName].ToList();
                return null;
            }
            return _errors.SelectMany(err => err.Value.ToList());
        }

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            var validationContext = new ValidationContext(this, null, null);
            validationContext.MemberName = propertyName;
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(value, validationContext, validationResults);
            if (_errors.ContainsKey(propertyName))
                _errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
            HandleValidationResults(validationResults);
        }

        public void ValidateProperty([CallerMemberName] string propertyName = null)
        {
            if (_properties == null)
                _properties = TypeDescriptor.GetProperties(this);
            var property = _properties.Find(propertyName, true);
            ValidateProperty(property.GetValue(this), propertyName);
        }

        /// <summary>
        /// Checks for errors
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            var validationContext = new ValidationContext(this, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, validationContext, validationResults, true);
            //clear all previous _errors  
            var propNames = _errors.Keys.ToList();
            _errors.Clear();
            propNames.ForEach(OnErrorsChanged);
            HandleValidationResults(validationResults);
            return IsValid;
        }

        private void HandleValidationResults(List<ValidationResult> validationResults)
        {
            var resultsByPropNames = from res in validationResults
                                     from mname in res.MemberNames
                                     group res by mname into g
                                     select g;
            foreach (var prop in resultsByPropNames)
            {
                var messages = prop.Select(r => r.ErrorMessage).ToList();
                _errors.Add(prop.Key, messages);
                OnErrorsChanged(prop.Key);
            }

        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            ValidateProperty(propertyName);
            base.OnPropertyChanged(propertyName);
        }

        public string GetStringErrors(string propertyName)
        {
            return GetErrors(propertyName).Cast<string>().Aggregate((s1, s2) => { return s1 + Environment.NewLine + s2; });
        }
    }
}
