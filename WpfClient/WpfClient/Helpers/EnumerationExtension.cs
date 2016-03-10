using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Markup;

namespace WpfClient.Helpers
{
    public class EnumerationExtension : MarkupExtension
    {
        private Type _enumType;

        public EnumerationExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            EnumType = enumType;
        }

        public Type EnumType
        {
            get { return _enumType; }
            private set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                _enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);

            List<EnumerationMember> list = new List<EnumerationMember>();
            foreach (var enumValue in enumValues)
            {
                var displayAttribute = GetDisplayAttribute(enumValue);
                var member = new EnumerationMember();
                member.DisplayName = displayAttribute.Name;
                member.Description = displayAttribute.Description;
                member.Value = enumValue;
                list.Add(member);
            }
            return list.ToArray();
        }

        private DisplayAttribute GetDisplayAttribute(object enumValue)
        {
            var name = enumValue.ToString();
            var descriptionAttribute = EnumType
                .GetField(name)
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;
            if (descriptionAttribute == null)
            {
                descriptionAttribute= new DisplayAttribute() {Name = name };
            }
            return descriptionAttribute;
        }

        public class EnumerationMember
        {
            public string Description { get; set; }
            public string DisplayName { get; set; }
            public object Value { get; set; }
        }
    }
}
