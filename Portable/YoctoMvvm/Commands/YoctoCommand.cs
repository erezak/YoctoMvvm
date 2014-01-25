using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace YoctoMvvm.Common {
    /// <summary>
    /// An ICommand that triggers an action
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public class YoctoCommand : YoctoCommand<object> {
        public YoctoCommand(Action action, Func<bool> canExecuteMethod)
            : this(m => action(), canExecuteMethod) {
        }

        public YoctoCommand(Action<object> parameterizedAction, Func<bool> canExecuteMethod)
            : base(parameterizedAction, canExecuteMethod) {
        }
    }

    /// <summary>
    /// An ICommand that triggers an action
    /// This version of the YoctoCommand class invokes a function that takes TParameter as its parameter.
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public class YoctoCommand<TParameter> : Bindable, ICommand {
        /// <summary>
        /// Initializes a new instance of the <see cref="YoctoCommand"/> class.
        /// </summary>
        /// <param name="parameterizedAction">The parameterized action.</param>
        /// <param name="canExecute">if set to <c>true</c> the command can execute.</param>
        public YoctoCommand(Action<TParameter> parameterizedAction, Func<bool> canExecuteMethod) {
            //  Set the action.
            _InternalAction = parameterizedAction;
            _CanExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="param">The param.</param>
        public virtual void DoExecute(TParameter param) {
            // Raise the Executing event, allowing cancellation by the listener
            var executingEventArgs = new CancelYoctoCommandEventArgs<TParameter> {
                Parameter = param,
                Cancel = false
            };
            RaiseExecuting(executingEventArgs);

            // Only continue if the command has not been canceled.
            if (!executingEventArgs.Cancel) {
                param = executingEventArgs.Parameter;
                RunInternalAction(param);

                // Raise the Executed event
                RaiseExecuted(new YoctoCommandEventArgs<TParameter> { Parameter = param });
            }
        }

        /// <summary>
        /// Run the actual command action
        /// </summary>
        /// <param name="param">The param.</param>
        protected void RunInternalAction(TParameter param) {
            if (_InternalAction != null)
                _InternalAction(param);
        }

        /// <summary>
        /// Raise Executed event.
        /// </summary>
        /// <param name="args">Event parameters for handler</param>
        protected void RaiseExecuted(YoctoCommandEventArgs<TParameter> args) {
            if (Executed != null)
                Executed(this, args);
        }

        /// <summary>
        /// Raise Executing event.
        /// </summary>
        /// <param name="args">Event parameters for handler</param>
        protected void RaiseExecuting(CancelYoctoCommandEventArgs<TParameter> args) {
            if (Executing != null)
                Executing(this, args);
        }

        /// <summary>
        /// The interal action
        /// </summary>
        protected Action<TParameter> _InternalAction;

        /// <summary>
        /// Method determining if the command can execute
        /// </summary>
        private readonly Func<bool> _CanExecuteMethod;


        #region ICommand Members

        public bool CanExecute(object parameter) {
            return _CanExecuteMethod();
        }

        public void Execute(object parameter) {
            // Make sure the view bidning was done with a correct parameter type.
            if (parameter != null && parameter is TParameter == false) {
                Type passedType;
                passedType = parameter.GetType();
                throw new InvalidOperationException(string.Format("The command expected a paramter of {0}, but received {1}",
                                                        typeof(TParameter).Name, 
                                                        passedType.Name));
            }
            DoExecute((TParameter)parameter);
        }

        #endregion

        #region events
        /// <summary>
        /// Event fired when CanExecute property is changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() {
            if (CanExecuteChanged != null) {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the command is about to execute.
        /// </summary>
        public event CancelYoctoCommandEventHandler<TParameter> Executing;

        /// <summary>
        /// Occurs when the command executed.
        /// </summary>
        public event CommandEventHandler<TParameter> Executed;
        #endregion events
    }
    #region event handlers
    /// <summary>
    /// The CommandEventHandler delegate.
    /// </summary>
    public delegate void CommandEventHandler(object sender, YoctoCommandEventArgs e);

    /// <summary>
    /// The typed CommandEventHandler delegate.
    /// </summary>
    public delegate void CommandEventHandler<TParameter>(object sender, YoctoCommandEventArgs<TParameter> e);

    /// <summary>
    /// The CancelCommandEvent delegate.
    /// </summary>
    public delegate void CancelCommandEventHandler(object sender, CancelYoctoCommandEventArgs e);

    /// <summary>
    /// The typed CancelCommandEvent delegate.
    /// </summary>
    public delegate void CancelYoctoCommandEventHandler<TParameter>(object sender, CancelYoctoCommandEventArgs<TParameter> e);
    #endregion event handlers
}
