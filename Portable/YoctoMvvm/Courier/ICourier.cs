using System;
namespace YoctoMvvm.Courier {
    public interface ICourier {
        void DepositInMailbox<TParcel>(TParcel parcel) where TParcel : class, IParcel;
        bool Publish<TParcel>(TParcel parcel) where TParcel : class, IParcel;
        ParcelTypeSubscriptionVoucher SubscribeForParcelType<TParcel>(Action<TParcel> actionToTriggerUponDelivery, IParcelsRouter router = null, Predicate<TParcel> filter = null, bool useStrongReferences = true) where TParcel : class, IParcel;
        void Unsubscribe<TParcel>(ParcelTypeSubscriptionVoucher subscriptionVoucher) where TParcel : class, IParcel;
    }
}
