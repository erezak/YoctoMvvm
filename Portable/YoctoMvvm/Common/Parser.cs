using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
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
    }
}
