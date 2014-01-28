
namespace YoctoMvvm.Commands {
    /// <summary>
    /// CancelYoctoCommandEventArgs is raised just
    /// before execution.
    /// It supplies the command parameter and allows the caller to indicate that the execution should be stopped
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public class CancelYoctoCommandEventArgs : YoctoCommandEventArgs {
        /// <summary>
        /// Should the command be canceled
        /// </summary>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// CancelYoctoCommandEventArgs is raised just before execution.
    /// It supplies the command parameter and allows the caller to indicate that the execution should be stopped
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public class CancelYoctoCommandEventArgs<TParameter> : YoctoCommandEventArgs<TParameter> {
        /// <summary>
        /// Should the command be canceled
        /// </summary>
        public bool Cancel { get; set; }
    }
}
