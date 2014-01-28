using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YoctoMvvm.Platform;

namespace YoctoMvvmWindowsStore.Platform {
    public class NavigationServiceWindowsStore : INavigationService {
        private Frame _RootFrame;
        private Dictionary<Type, Type> _ViewModelToViewMap;
        private Type _SavedState;
        public NavigationServiceWindowsStore(INavigationMappingProvider navigationMappingProvider) {
            _ViewModelToViewMap = navigationMappingProvider.ProvideViewModelToViewMap();
            _RootFrame = Window.Current.Content as Frame;
        }

        public bool CanGoBack {
            get {
                if (_RootFrame != null) {
                    return _RootFrame.CanGoBack;
                } else {
                    return false;
                }
            }
        }

        public void GoBack() {
            if (_RootFrame != null) {
                _RootFrame.GoBack();
            }
        }

        public void Navigate<TViewModel>(object parameterForView = null) {
            var navParameter = string.Empty;
            if (_ViewModelToViewMap.ContainsKey(typeof(TViewModel))) {
                var page = _ViewModelToViewMap[typeof(TViewModel)];
                _RootFrame.Navigate(page, parameterForView);
            }
        }

        public void NavigateReplaceAndKeepCurrent<TNewViewModel, TCurrentViewModel>(object parameterForView = null) {
            _SavedState = typeof(TCurrentViewModel);
            Navigate<TNewViewModel>(parameterForView);
            if (_RootFrame.BackStack.Count > 0) {
                _RootFrame.BackStack.RemoveAt(_RootFrame.BackStack.Count - 1);
            }
        }

        public void NavigateToSavedState(object parameterForView = null) {
            if (_SavedState == null) {
                throw new InvalidOperationException("No saved state exists.");
            }
            var method = typeof(NavigationServiceWindowsStore).GetTypeInfo().GetDeclaredMethod("Navigate").MakeGenericMethod(_SavedState);
            method.Invoke(this, new object[] { parameterForView });
            _SavedState = null;
            if (_RootFrame.BackStack.Count > 0) {
                _RootFrame.BackStack.RemoveAt(_RootFrame.BackStack.Count - 1);
            }
        }
    }
}
