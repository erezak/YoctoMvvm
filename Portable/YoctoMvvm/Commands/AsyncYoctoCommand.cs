using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YoctoMvvm.Commands {
    /// <summary>
    /// An ICommand that triggers an awaitable action
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public class AsyncYoctoCommand : AsyncYoctoCommand<object> {
        public AsyncYoctoCommand(Func<Task> asyncExecute,
                       Predicate<object> canExecute)
            : base((a) => {
                return asyncExecute();
            }, canExecute) {
        }
    }
    /// <summary>
    /// An ICommand that triggers an awaitable action
    /// This version of the AsyncYoctoCommand class invokes a function that takes TParameter as its parameter.
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public class AsyncYoctoCommand<TParameter> : ICommand {
        protected Predicate<object> _canExecute;
        protected Func<TParameter, Task> _asyncExecute;

        /// <summary>
        /// Occurs when can execute is changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Event fired when CanExecute property is changed.
        /// </summary>
        public void RaiseCanExecuteChanged() {
            if (CanExecuteChanged != null) {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Creates a new AsyncYoctoCommand
        /// </summary>
        /// <param name="asyncExecute">Awaitable action</param>
        /// <param name="canExecute">A method to determine if the action can be executed</param>
        public AsyncYoctoCommand(Func<TParameter, Task> asyncExecute,
                       Predicate<object> canExecute) {
            _asyncExecute = asyncExecute;
            _canExecute = canExecute;
            RaiseCanExecuteChanged();
        }
        /// <summary>
        /// Manually set CanExecute to false
        /// </summary>
        /// <param name="param">Parameter to the function, if needed (ignored at this time)</param>

        public void Disable(TParameter param) {
            _canExecute = (p) => {
                return false;
            };
            RaiseCanExecuteChanged();
        }
        /// <summary>
        /// Manually set CanExecute to true
        /// </summary>
        /// <param name="param">Parameter to the function, if needed (ignored at this time)</param>
        public void Enable(TParameter param) {
            _canExecute = (p) => {
                return true;
            };
            RaiseCanExecuteChanged();
        }

        #region ICommand Members
        public bool CanExecute(object parameter) {
            return _canExecute(parameter);
        }

        public async void Execute(object parameter) {
            if (CanExecute(parameter)) {
                await ExecuteAsync((TParameter)parameter);
            }
        }
        #endregion ICommand Members

        protected virtual async Task ExecuteAsync(TParameter parameter) {
            await _asyncExecute(parameter);
        }

    }
}
