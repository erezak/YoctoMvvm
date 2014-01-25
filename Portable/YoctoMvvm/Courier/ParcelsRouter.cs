
namespace YoctoMvvm.Courier {
    /// <summary>
    /// Represents a parcel router. A router, can change the route of the parcel,
    /// change parts of the parcel, etc.
    /// </summary>
    public interface IParcelsRouter {
        void Deliver(IParcel parcel, IParcelTypeSubscription subscription);
    }

    /// <summary>
    /// Direct router - simply sends the parcel to the subscriber.
    /// 
    /// This is the default router.
    /// </summary>
    sealed class DirectParcelsRouter : IParcelsRouter {

        static DirectParcelsRouter() {
        }

        #region singleton
        private static readonly DirectParcelsRouter _Instance = new DirectParcelsRouter();
        public static DirectParcelsRouter Instance {
            get {
                return _Instance;
            }
        }
        private DirectParcelsRouter() {
        }
        #endregion singleton
        /// <summary>
        /// Direct router - send parcels to the subscribers
        /// </summary>
        /// <param name="parcel">The parcel to deliver</param>
        /// <param name="subscription">The subscription to use for delivery</param>
        public void Deliver(IParcel parcel, IParcelTypeSubscription subscription) {
            subscription.Deliver(parcel);
        }
    }
}
