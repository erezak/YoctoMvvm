using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using YoctoMvvm.Platform;

namespace YoctoMvvmWindowsStore.Platform {
    public class CryptoProvider : ICryptoProvider {
        public byte[] GetSecureRandomNumber() {
            var number = new byte[32];
            var provider = CryptographicBuffer.GenerateRandom(32);
            CryptographicBuffer.CopyToByteArray(provider, out number);
            return number;
        }
        public byte[] GetHash(byte[] preHash) {
            var hashProvider = HashAlgorithmProvider.OpenAlgorithm("SHA256");
            var preHashData = CryptographicBuffer.CreateFromByteArray(preHash);
            var hashData = hashProvider.HashData(preHashData);
            var hash = new byte[hashData.Length];
            CryptographicBuffer.CopyToByteArray(hashData, out hash);
            return hash;
        }
    }
}
