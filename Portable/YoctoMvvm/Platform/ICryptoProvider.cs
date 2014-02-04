using System;
namespace YoctoMvvm.Platform {
    public interface ICryptoProvider {
        byte[] GetHash(byte[] preHash);
        byte[] GetSecureRandomNumber();
    }
}
