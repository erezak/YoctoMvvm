using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace YoctoMvvm.Common {
    /// <summary>
    /// Standard implementation of INotifyPropertyChanged
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public class Bindable : INotifyPropertyChanged {
        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the property and notifies - only if value was changed
        /// </summary>
        /// <typeparam name="TProperty">The property data type</typeparam>
        /// <param name="storage">The backing field - passed by reference</param>
        /// <param name="value">The new value to set</param>
        /// <param name="propertyName">The property name</param>
        /// <returns>True if value was changed</returns>
        protected bool SetProperty<TProperty>(ref TProperty storage, TProperty value, [CallerMemberName] String propertyName = null) {
            if (object.Equals(storage, value)) {
                return false;
            }

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notify listener that property was changed
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null) {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged members
    }
}
