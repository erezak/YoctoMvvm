using System;

namespace YoctoMvvm.Courier {
    /// <summary>
    /// Represents a parcel that should be wasSubscriberFound by the courier
    /// to anyone who is subscribed to this type
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public interface IParcel {
        /// <summary>
        /// Parcel origin
        /// </summary>
        object Sender { get; }
    }

    /// <summary>
    /// Parcel base class.
    /// </summary>
    public abstract class Parcel : IParcel {
        private WeakReference _Sender;
        public object Sender {
            get {
                if (_Sender == null) {
                    return null;
                }
                return _Sender.Target;
            }
        }
        /// <summary>
        /// Constructs a new parcel
        /// </summary>
        /// <param name="sender">Parcel originator</param>
        public Parcel(object sender) {
            if (sender != null) {
                _Sender = new WeakReference(sender);
            } else {
                _Sender = null;
            }
        }
    }

    /// <summary>
    /// Parcel with contents
    /// </summary>
    public class Parcel<TContents> : Parcel {
        /// <summary>
        /// Contents of the parcel
        /// </summary>
        public TContents Contents {
            get;
            protected set;
        }

        /// <summary>
        /// Constructs a new parcel
        /// </summary>
        /// <param name="sender">Parcel originator</param>
        /// <param name="contents">Parcel contents</param>
        public Parcel(object sender, TContents contents)
            : base(sender) {
            Contents = contents;
        }
    }

}
