using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Helpers
{
  public static class AggregateExceptionEx
    {
        public static string AggregateMessages(this AggregateException aggregateException)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var innerException in aggregateException.InnerExceptions)
            {
                sb.AppendLine(innerException.Message);
            }
            return sb.ToString();
        }
    }
}
