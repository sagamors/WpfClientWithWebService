using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Helpers
{
    static class ValidateHelpers
    {
        public static void NotNull(object value,string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }
    }
}
