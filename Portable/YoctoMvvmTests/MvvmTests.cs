using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoctoMvvm.Common;
using YoctoMvvm.Courier;
using System.Diagnostics.CodeAnalysis;

namespace YoctoMvvmTests {
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MvvmTests {
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Init_ShouldSucceeddInConstruction() {
            var a = new Courier();
            Assert.IsNotNull(a);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Construct_ShouldSucceedWhenNoSender() {
            var m = new Parcel<int>(null, 1);
            Assert.IsNotNull(m);
            Assert.IsNull(m.Sender);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Publish_ShouldSendParcel() {
            var courier = new Courier();
            var parcel = 0;
            object sender = null;
            var expected = 5;
            courier.SubscribeForParcelType<Parcel<string>>((b) => {
            });
            courier.SubscribeForParcelType<Parcel<int>>((b) => {
                parcel = b.Contents;
                sender = b.Sender;
            });
            courier.Publish<Parcel<int>>(new Parcel<int>(this, 5));
            Assert.AreEqual(expected, parcel);
            Assert.AreEqual(this, sender);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Publish_WithProxyShouldSendParcel() {
            var courier = new Courier();
            var parcel = 0;
            object sender = null;
            var expected = 5;
            courier.SubscribeForParcelType<Parcel<int>>(
                (b) => {
                    parcel = b.Contents;
                    sender = b.Sender;
                });
            courier.Publish<Parcel<int>>(new Parcel<int>(this, 5));
            Assert.AreEqual(expected, parcel);
            Assert.AreEqual(this, sender);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CourierSubscriptionVoucher_Init_ShouldThrowWhenNullCourier() {
            var a = new ParcelTypeSubscriptionVoucher(null, typeof(Parcel<int>));
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CourierSubscriptionVoucher_Init_ShouldThrowWhenNotParcel() {
            var courier = new Courier();
            var a = new ParcelTypeSubscriptionVoucher(courier, typeof(string));
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void CourierSubscriptionVoucher_Init_ShouldSucceed() {
            var courier = new Courier();
            using (var a = new ParcelTypeSubscriptionVoucher(courier, typeof(Parcel<int>))) {
                Assert.IsNotNull(a);
            };
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Unsubscribe_ShouldSucceed() {
            var courier = new Courier();
            var contents = 0;
            var voucher = courier.SubscribeForParcelType<Parcel<int>>(
                (a) => { contents = a.Contents; },
                useStrongReferences: false);
            courier.Unsubscribe<Parcel<int>>(voucher);
            courier.Publish<Parcel<int>>(new Parcel<int>(this, 5));
            Assert.AreEqual(0, contents);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Unsubscribe_ShouldSucceedWeak() {
            var courier = new Courier();
            var contents = 0;
            var voucher = courier.SubscribeForParcelType<Parcel<int>>(
                (a) => { contents = a.Contents; });
            courier.Unsubscribe<Parcel<int>>(voucher);
            courier.Publish<Parcel<int>>(new Parcel<int>(this, 5));
            Assert.AreEqual(0, contents);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Courier_Unsubscribe_ShouldThrowWhenNull() {
            var courier = new Courier();
            courier.Unsubscribe<Parcel<int>>(null);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Subscribe_ShouldSucceedWithFilter() {
            var courier = new Courier();
            var contents = 0;
            var filtered = false;
            var voucher = courier.SubscribeForParcelType<Parcel<int>>(
                (a) => { contents = a.Contents; },
                null,
                (m) => { filtered = true; return true; });
            courier.Publish<Parcel<int>>(new Parcel<int>(this, 5));
            Assert.AreEqual(5, contents);
            Assert.IsTrue(filtered);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Subscribe_ShouldStopWhenFiltered() {
            var courier = new Courier();
            var contents = 0;
            var filtered = false;
            var voucher = courier.SubscribeForParcelType<Parcel<int>>(
                (a) => { contents = a.Contents; },
                null,
                (m) => { filtered = true; return false; },
                false);
            courier.Publish<Parcel<int>>(new Parcel<int>(this, 5));
            Assert.AreEqual(0, contents);
            Assert.IsTrue(filtered);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Courier_Subscribe_ShouldThrowWhenNoAction() {
            var courier = new Courier();
            var contents = 0;
            var filtered = false;
            var voucher = courier.SubscribeForParcelType<Parcel<int>>(
                null,
                null,
                (m) => { filtered = true; return false; });
            courier.Publish<Parcel<int>>(new Parcel<int>(this, 5));
            Assert.AreEqual(0, contents);
            Assert.IsTrue(filtered);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Subscribe_ShouldNotThrowEvenWhenActionThrows() {
            var courier = new Courier();
            var contents = 0;
            var voucher = courier.SubscribeForParcelType<Parcel<int>>(
                (a) => { throw new Exception(); });
            courier.Publish<Parcel<int>>(new Parcel<int>(this, 5));
            Assert.AreEqual(0, contents);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Subscribe_ShouldFindLastDepositedParcel() {
            var courier = new Courier();
            courier.DepositInMailbox<Parcel<int>>(new Parcel<int>(this, 4));
            courier.DepositInMailbox<Parcel<int>>(new Parcel<int>(this, 5));
            var contents = 0;
            var voucher = courier.SubscribeForParcelType<Parcel<int>>(
                (a) => { contents = a.Contents; });
            Assert.AreEqual(5, contents);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Subscribe_ShouldNotFailEvenIfDeliveryFailsForDepositedParcel() {
            var courier = new Courier();
            courier.DepositInMailbox<Parcel<int>>(new Parcel<int>(this, 4));
            courier.DepositInMailbox<Parcel<int>>(new Parcel<int>(this, 5));
            var contents = 0;
            var voucher = courier.SubscribeForParcelType<Parcel<int>>(
                (a) => {
                    throw new Exception();
                });
            Assert.AreEqual(0, contents);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Courier_Deposit_ShouldThrowWhenNoParcel() {
            var courier = new Courier();
            courier.DepositInMailbox<Parcel<int>>(null);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Courier_Publish_ShouldThrowWhenNoParcel() {
            var courier = new Courier();
            courier.Publish<Parcel<int>>(null);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Publish_ShouldDeliverParcel() {
            var courier = new Courier();
            var contents = 0;
            courier.SubscribeForParcelType<Parcel<int>>(
                (a) => {
                    contents = a.Contents;
                });
            courier.SubscribeForParcelType<Parcel<string>>(
                (a) => {
                });
            courier.Publish<Parcel<int>>(new Parcel<int>(this, 5));
            Assert.AreEqual(5, contents);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Publish_ShouldDeliverParcelWeak() {
            var courier = new Courier();
            var contents = 0;
            courier.SubscribeForParcelType<Parcel<int>>(
                actionToTriggerUponDelivery:
                (a) => {
                    contents = a.Contents;
                },
                useStrongReferences: false);
            courier.SubscribeForParcelType<Parcel<string>>(
                (a) => {
                });
            courier.Publish<Parcel<int>>(new Parcel<int>(this, 5));
            Assert.AreEqual(5, contents);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Courier")]
        public void Courier_Publish_ShouldReturnWhenNoSubscriber() {
            var courier = new Courier();
            courier.Publish<Parcel<int>>(new Parcel<int>(this, 5));
        }
    }
}
