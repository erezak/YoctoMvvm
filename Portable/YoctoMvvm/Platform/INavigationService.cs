namespace YoctoMvvm.Platform {
    /// <summary>
    /// Represents a navigation service, to allow navigation in business logic (View models)
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public interface INavigationService {
        /// <summary>
        /// Property indicating if navigation up the stack is allowed
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Goes back up the navigation stack, removing the current view from the stack
        /// </summary>
        void GoBack();

        /// <summary>
        /// Navigates to a new view
        /// </summary>
        /// <param name="parameterForView">
        /// An optional parameter to be sent to the new view
        /// </param>
        /// <typeparam name="TViewModel">
        /// Thew ViewModel type. The navigation service implementation will hold the mapping between the view model and the view.
        /// </typeparam>
        void Navigate<TViewModel>(object parameterForView = null);
        /// <summary>
        /// Navigates to a new view, replacing the current view in the stack.
        /// The implementing class is expected to save the old viewmodel to be used in the <see cref="NavigateToSavedState"/> method
        /// </summary>
        /// <typeparam name="TNewViewModel">The new view model</typeparam>
        /// <typeparam name="TCurrentViewModel">The current view model</typeparam>
        /// <param name="parameterForView">An optional parameter to be sent to the new view </param>
        void NavigateReplaceAndKeepCurrent<TNewViewModel,TCurrentViewModel>(object parameterForView = null);
        /// <summary>
        /// Navigates to the state that was saved in the <see cref="NavigateReplaceAndKeepCurrent"/> method
        /// </summary>
        /// <param name="parameterForView">An optional parameter to be sent to the new view</param>
        void NavigateToSavedState(object parameterForView = null);
    }
}