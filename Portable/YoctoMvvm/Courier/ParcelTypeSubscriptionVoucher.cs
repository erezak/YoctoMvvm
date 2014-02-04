using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YoctoMvvm.Courier {
    public sealed class ParcelTypeSubscriptionVoucher : IDisposable {
        private WeakReference _Courier;
        private Type _ParcelType;

        /// <summary>
        /// Create a new instance of the ParcelTypeSubscriptionVoucher class.
        /// </summary>
        public ParcelTypeSubscriptionVoucher(ICourier courier, Type parcelType) {
            if (courier == null) {
                throw new ArgumentNullException("courier");
            }

            if (!typeof(IParcel).GetTypeInfo().IsAssignableFrom(parcelType.GetTypeInfo())) {
                throw new ArgumentOutOfRangeException("parcelType");
            }

            _Courier = new WeakReference(courier);
            _ParcelType = parcelType;
        }

        public void Dispose() {
            if (_Courier.IsAlive) {
                var courier = _Courier.Target as ICourier;

                if (courier != null) {
                    var unsubscribeMethod = typeof(ICourier).GetRuntimeMethod("Unsubscribe", new Type[] { typeof(ParcelTypeSubscriptionVoucher) });
                    unsubscribeMethod = unsubscribeMethod.MakeGenericMethod(_ParcelType);
                    unsubscribeMethod.Invoke(courier, new object[] { this });
                }
            }

            GC.SuppressFinalize(this);
        }
    }
}
