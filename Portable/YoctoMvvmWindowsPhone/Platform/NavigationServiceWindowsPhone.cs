using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using YoctoMvvm.Platform;
using YoctoMvvm.Common;

namespace YoctoMvvmWindowsPhone.Platform {
    public class NavigationServiceWindowsPhone : INavigationService {
        private const string _QueryString = "qs";
        #region private members
        private Type _SavedState = null;
        /// <summary>
        /// Mapping between view models and views.
        /// </summary>
        private readonly Dictionary<Type, string> _ViewModelsToViewsMapping;
        #endregion private members
        public NavigationServiceWindowsPhone(INavigationMappingProvider navigationMappingProvider) {
            _ViewModelsToViewsMapping = navigationMappingProvider.ProvideViewModelToViewMap();
        }

        #region private helper methods
        private Frame RootFrame {
            get { return Application.Current.RootVisual as Frame; }
        }
        #endregion private helper methods

        #region INavigationService Implementation
        public bool CanGoBack {
            get {
                return RootFrame.CanGoBack;
            }
        }

        public void GoBack() {
            RootFrame.GoBack();
        }

        public void Navigate<TViewModel>(object parameterForView) {
            var navParameter = string.Empty;
            if (parameterForView != null) {
                navParameter = "?" + _QueryString + "=" + parameterForView.SerializeToJson();
            }

            if (_ViewModelsToViewsMapping.ContainsKey(typeof(TViewModel))) {
                var page = _ViewModelsToViewsMapping[typeof(TViewModel)];

                this.RootFrame.Navigate(new Uri("/" + page + navParameter, UriKind.Relative));
            }
        }
        public void NavigateReplaceAndKeepCurrent<TVM, TOLDVM>(object parameterForView = null) {
            var frame = ((PhoneApplicationFrame)RootFrame);
            _SavedState = typeof(TOLDVM);
            Navigate<TVM>(parameterForView);
            frame.RemoveBackEntry();
        }
        public void NavigateToSavedState(object parameterForView = null) {
            if (_SavedState == null) {
                throw new InvalidOperationException("No saved state exists.");
            }
            var frame = ((PhoneApplicationFrame)RootFrame);
            var method = typeof(NavigationServiceWindowsPhone).GetMethod("Navigate").MakeGenericMethod(_SavedState);
            method.Invoke(this, new object[] {parameterForView});
            _SavedState = null;
            frame.RemoveBackEntry();
        }

        #endregion INavigationService Implementation

        /// <summary>
        /// Decodes the navigation parameterForView.
        /// </summary>
        /// <typeparam name="TParam">The type of the json.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>The json result.</returns>
        public static TParam DecodeNavigationParameter<TParam>(NavigationContext context) {
            if (context.QueryString.ContainsKey(_QueryString)) {
                var param = context.QueryString[_QueryString];
                if (string.IsNullOrWhiteSpace(param)) {
                    return default(TParam);
                } else {
                    return param.ParseJson<TParam>();
                }
            }

            throw new KeyNotFoundException();
        }
    }
}
