using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoctoMvvm.Courier {
    public sealed class Courier {

        #region private and internal use members
        private readonly object _SyncLock = new object();

        #region private subscription tables
        private readonly Dictionary<Type, List<RoutableParcelTypeSubscription>> _Subscriptions = new Dictionary<Type, List<RoutableParcelTypeSubscription>>();
        private readonly Dictionary<Type, IParcel> _ParcelsWaitingForDelivery = new Dictionary<Type, IParcel>();
        #endregion private subscription tables

        #region ParcelTypeSubscription (weak and strong reference)
        private class ParcelTypeSubscription<TParcel> : IParcelTypeSubscription
            where TParcel : class, IParcel {
            protected ParcelTypeSubscriptionVoucher _SubscriptionVoucher;
            protected Action<TParcel> _DeliveryAction;
            protected Predicate<TParcel> _ParcelFilter;

            public ParcelTypeSubscriptionVoucher SubscriptionVoucher {
                get {
                    return _SubscriptionVoucher;
                }
            }

            public bool ShouldTryToDeliverParcel(IParcel parcel) {
                var filterResult = _ParcelFilter.Invoke(parcel as TParcel);
                return filterResult;
            }

            public void Deliver(IParcel message) {
                _DeliveryAction.Invoke(message as TParcel);
            }

            /// <summary>
            /// Constructs a ParcelTypeSubscription
            /// </summary>
            public ParcelTypeSubscription(ParcelTypeSubscriptionVoucher subscriptionVoucher,
                                             Action<TParcel> deliveryAction,
                                             Predicate<TParcel> parcelFilter) {
                _SubscriptionVoucher = subscriptionVoucher;
                _DeliveryAction = deliveryAction;
                _ParcelFilter = parcelFilter;
            }
        }

        private sealed class ParcelTypeSubscriptionWithWeakReferences<TParcel> : IParcelTypeSubscription
            where TParcel : class, IParcel {
            private ParcelTypeSubscriptionVoucher _SubscriptionVoucher;
            private WeakReference _DeliveryAction;
            private WeakReference _ParcelFilter;

            public ParcelTypeSubscriptionVoucher SubscriptionVoucher {
                get {
                    return _SubscriptionVoucher;
                }
            }

            public bool ShouldTryToDeliverParcel(IParcel parcel) {
                var filterResult = false;
                if (_ParcelFilter.IsAlive && _DeliveryAction.IsAlive) {
                    var filter = (Predicate<TParcel>)_ParcelFilter.Target;
                    filterResult = filter.Invoke(parcel as TParcel);
                }
                return filterResult;
            }

            public void Deliver(IParcel message) {
                var deliveryAction = (Action<TParcel>)_DeliveryAction.Target;
                deliveryAction.Invoke(message as TParcel);
            }

            /// <summary>
            /// Constructs a new ParcelTypeSubscriptionWithWeakReferences
            /// </summary>
            public ParcelTypeSubscriptionWithWeakReferences(ParcelTypeSubscriptionVoucher subscriptionVoucher,
                                              Action<TParcel> deliveryAction,
                                              Predicate<TParcel> parcelFilter) {
                _SubscriptionVoucher = subscriptionVoucher;
                _DeliveryAction = new WeakReference(deliveryAction);
                _ParcelFilter = new WeakReference(parcelFilter);
            }
        }


        #endregion ParcelTypeSubscription (weak and strong reference)

        /// <summary>
        /// A subscription that includes its router
        /// </summary>
        private class RoutableParcelTypeSubscription {
            public IParcelsRouter Router {
                get;
                private set;
            }
            public IParcelTypeSubscription ParcelTypeSubscription {
                get;
                private set;
            }
            public RoutableParcelTypeSubscription(IParcelsRouter router, IParcelTypeSubscription subscription) {
                Router = router;
                ParcelTypeSubscription = subscription;
            }
        }
        #endregion private and internal use members

        #region public pub/sub
        /// <summary>
        /// Deliver a parcel type to any subscriber that requested this type
        /// </summary>
        /// <typeparam name="TParcel">The type of parcel to deliver</typeparam>
        /// <param name="parcel">The parcel to deliver</param>
        /// <returns>True is there was at least one subscriber</returns>
        public bool Publish<TParcel>(TParcel parcel)
            where TParcel : class, IParcel {
            if (parcel == null) {
                throw new ArgumentNullException("parcel");
            }
            var wasSubscriberFound = false;
            List<RoutableParcelTypeSubscription> currentlySubscribed;
            lock (_SyncLock) {
                List<RoutableParcelTypeSubscription> currentSubscriptions;
                if (!_Subscriptions.TryGetValue(typeof(TParcel), out currentSubscriptions)) {
                    // No subscribers at all - no need to continue
                    return false;
                }
                currentlySubscribed = (from sub in currentSubscriptions
                                       where sub.ParcelTypeSubscription.ShouldTryToDeliverParcel(parcel)
                                       select sub).ToList();
            }

            foreach (var sub in currentlySubscribed) {
                wasSubscriberFound = true;
                try {
                    sub.Router.Deliver(parcel, sub.ParcelTypeSubscription);
                } catch (Exception) {
                    //Exceptions shouldn't arrive to the parcel publisher
                    //So there's nothing to do here
                }
            }
            return wasSubscriberFound;
        }

        /// <summary>
        /// Start a subscription for a parcel type and optionally retrieve a previously depositted parcel
        /// </summary>
        /// <typeparam name="TParcel">The parcel type</typeparam>
        /// <param name="actionToTriggerUponDelivery">An action used to notify about new parcels</param>
        /// <param name="router">An optional router</param>
        /// <param name="filter">An optional filter</param>
        /// <param name="useStrongReferences">Should the subscription keep strong references to the subscriber?</param>
        /// <returns>A voucher that can be used to cancel the subscription</returns>
        public ParcelTypeSubscriptionVoucher SubscribeForParcelType<TParcel>(
                                           Action<TParcel> actionToTriggerUponDelivery,
                                            IParcelsRouter router = null,
                                            Predicate<TParcel> filter = null,
                                           bool useStrongReferences = true)
                                                        where TParcel : class, IParcel {
            var routerToUse = router ?? DirectParcelsRouter.Instance;
            var filterToUse = filter ?? ((p) => true);
            if (actionToTriggerUponDelivery == null) {
                throw new ArgumentNullException("actionToTriggerUponDelivery");
            }

            lock (_SyncLock) {
                List<RoutableParcelTypeSubscription> existingSubscriptions;

                if (!_Subscriptions.TryGetValue(typeof(TParcel), out existingSubscriptions)) {
                    existingSubscriptions = new List<RoutableParcelTypeSubscription>();
                    _Subscriptions[typeof(TParcel)] = existingSubscriptions;
                }

                var subscriptionVoucher = new ParcelTypeSubscriptionVoucher(this, typeof(TParcel));

                IParcelTypeSubscription subscription;
                if (useStrongReferences) {
                    subscription = new ParcelTypeSubscription<TParcel>(subscriptionVoucher,
                                                                        actionToTriggerUponDelivery,
                                                                        filterToUse);
                } else {
                    subscription = new ParcelTypeSubscriptionWithWeakReferences<TParcel>(subscriptionVoucher,
                                                                        actionToTriggerUponDelivery,
                                                                        filterToUse);
                }


                var routableSubscription = new RoutableParcelTypeSubscription(routerToUse, subscription);
                existingSubscriptions.Add(routableSubscription);

                // Check if any waiting parcels exist
                if (_ParcelsWaitingForDelivery.ContainsKey(typeof(TParcel))) {
                    var parcel = _ParcelsWaitingForDelivery[typeof(TParcel)];
                    _ParcelsWaitingForDelivery.Remove(typeof(TParcel));
                    try {
                        routableSubscription.Router.Deliver(parcel, subscription);
                    } catch (Exception) {
                        // No need to destroy subscriptions becuase of a faulty parcel
                    }
                }

                return subscriptionVoucher;
            }
        }

        /// <summary>
        /// Removes a subscription is found
        /// </summary>
        /// <typeparam name="TParcel">Type of subscription</typeparam>
        /// <param name="subscriptionVoucher">voucher received in the subscribe stage</param>
        public void Unsubscribe<TParcel>(ParcelTypeSubscriptionVoucher subscriptionVoucher)
            where TParcel : class, IParcel {
            if (subscriptionVoucher == null) {
                throw new ArgumentNullException("subscriptionVoucher");
            }

            lock (_SyncLock) {
                List<RoutableParcelTypeSubscription> currentSubscriptions;
                if (!_Subscriptions.TryGetValue(typeof(TParcel), out currentSubscriptions))
                    return;

                var currentlySubscribed = (from sub in currentSubscriptions
                                           where object.ReferenceEquals(sub.ParcelTypeSubscription.SubscriptionVoucher, subscriptionVoucher)
                                           select sub).ToList();
                foreach (var subscription in currentlySubscribed) {
                    currentSubscriptions.Remove(subscription);
                }
            }
        }
        #endregion public pub/sub
        /// <summary>
        /// Deposits a parcel that will be consumed by the first subscriber to this type.
        /// </summary>
        /// <typeparam name="TParcel">Type of parcel</typeparam>
        /// <param name="parcel">The parcel to deposit</param>
        public void DepositInMailbox<TParcel>(TParcel parcel)
            where TParcel : class, IParcel {
            if (parcel == null) {
                throw new ArgumentNullException("parcel");
            }

            // If a parcel of this type was already depositted, discard it
            if (_ParcelsWaitingForDelivery.ContainsKey(typeof(TParcel))) {
                _ParcelsWaitingForDelivery.Remove(typeof(TParcel));
            }
            //Save the parcel - it will be collected later.
            _ParcelsWaitingForDelivery.Add(typeof(TParcel), parcel);
            return;
        }

    }
}
