using System;

namespace YoctoMvvm.Common {
    /// <summary>
    /// YoctoCommandEventArgs used for general command event handlers
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public class YoctoCommandEventArgs : EventArgs {
        /// <summary>
        /// Command parameter
        /// </summary>
        public object Parameter { get; set; }
    }

    /// <summary>
    /// Typed YoctoCommandEventArgs used for general command event handlers
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public class YoctoCommandEventArgs<TParameter> : EventArgs {
        /// <summary>
        /// Command parameter
        /// </summary>
        public TParameter Parameter { get; set; }
    }
}
