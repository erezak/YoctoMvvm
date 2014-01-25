
namespace YoctoMvvm.Courier {
    /// <summary>
    /// Represents a subscription to a parcel type
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public interface IParcelTypeSubscription {
        /// <summary>
        /// Voucher for a subscription - should be saved by the subscriber 
        /// </summary>
        ParcelTypeSubscriptionVoucher SubscriptionVoucher { get; }

        bool ShouldTryToDeliverParcel(IParcel parcel);

        /// <summary>
        /// Deliver the parcel to the subscriber
        /// </summary>
        /// <param name="parcel">Parcel to deliver</param>
        void Deliver(IParcel parcel);
    }

}
