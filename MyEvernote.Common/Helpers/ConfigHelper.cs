using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Common.Helpers
{
    public class ConfigHelper
    {
        public static T Get<T>(string key)
        {        // Appsettingsten aldıgımız degeri bu metodu çağırırken verdiğim t tipine dönüştürüp geri döner.
            return (T) Convert.ChangeType(ConfigurationManager.AppSettings[key],typeof(T)); 
        }
    }
}
