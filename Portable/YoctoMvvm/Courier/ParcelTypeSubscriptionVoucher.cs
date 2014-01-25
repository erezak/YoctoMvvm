using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YoctoMvvm.Courier {
    public sealed class ParcelTypeSubscriptionVoucher : IDisposable {
        private WeakReference _Hub;
        private Type _ParcelType;

        /// <summary>
        /// Create a new instance of the ParcelTypeSubscriptionVoucher class.
        /// </summary>
        public ParcelTypeSubscriptionVoucher(Courier courier, Type parcelType) {
            if (courier == null) {
                throw new ArgumentNullException("courier");
            }

            if (!typeof(IParcel).GetTypeInfo().IsAssignableFrom(parcelType.GetTypeInfo())) {
                throw new ArgumentOutOfRangeException("parcelType");
            }

            _Hub = new WeakReference(courier);
            _ParcelType = parcelType;
        }

        public void Dispose() {
            if (_Hub.IsAlive) {
                var hub = _Hub.Target as Courier;

                if (hub != null) {
                    var unsubscribeMethod = typeof(Courier).GetRuntimeMethod("Unsubscribe", new Type[] { typeof(ParcelTypeSubscriptionVoucher) });
                    unsubscribeMethod = unsubscribeMethod.MakeGenericMethod(_ParcelType);
                    unsubscribeMethod.Invoke(hub, new object[] { this });
                }
            }

            GC.SuppressFinalize(this);
        }
    }
}
