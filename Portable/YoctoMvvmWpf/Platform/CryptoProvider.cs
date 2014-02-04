using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YoctoMvvm.Platform;

namespace YoctoMvvmWpf.Platform {
    public class CryptoProvider : ICryptoProvider {
        public byte[] GetSecureRandomNumber() {
            var number = new byte[32];
            var provider = new RNGCryptoServiceProvider();
            provider.GetBytes(number);
            return number;
        }
        public byte[] GetHash(byte[] preHash) {
            var hashProvider = new SHA256Managed();
            var hash = hashProvider.ComputeHash(preHash);
            return hash;
        }
    }
}
