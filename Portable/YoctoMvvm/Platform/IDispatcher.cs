using System;
namespace YoctoMvvm.Platform {
    /// <summary>
    /// Represents a dispatcher that allows running on the UI thread.
    /// 
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public interface IDispatcher {
        /// <summary>
        /// Runs the supplied action on the UI thread.
        /// 
        /// The proper way to do this is determined by a platform specific implementation of the YoctoMvvm framework.
        /// </summary>
        /// <param name="action">The action to run</param>
        void RunOnUiThread(Action action);
    }
}
