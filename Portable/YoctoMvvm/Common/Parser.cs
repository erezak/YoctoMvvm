using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YoctoMvvm.Common {
    public static class Parser {
        public static T ParseJson<T>(this string json) {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json))) {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(ms);
                return obj;
            }
        }

        public static string SerializeToJson<T>(this T objectToConvert) {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToConvert.GetType());
            using (MemoryStream ms = new MemoryStream()) {
                serializer.WriteObject(ms, objectToConvert);
                var msBytes = ms.ToArray();
                return Encoding.UTF8.GetString(msBytes, 0, msBytes.Length);
            }
        }

        public static bool IsValidEmail(this string email) {
            if (string.IsNullOrWhiteSpace(email)) {
                return false;
            }
            // hthttp://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
            var emailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                               + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                               + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var emailValidator = new Regex(emailPattern, RegexOptions.IgnoreCase);
            return emailValidator.IsMatch(email);

        }
    }
}
