using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using YoctoMvvm.Platform;

namespace YoctoMvvmWpf.Platform {
    public class NavigationServiceWpf : INavigationService {
        private NavigationWindow _RootFrame;
        private Dictionary<Type, string> _ViewModelToViewMap;
        private Type _SavedState;
        public NavigationServiceWpf(INavigationMappingProvider navigationMappingProvider) {
            _ViewModelToViewMap = navigationMappingProvider.ProvideViewModelToViewMap();
            _RootFrame = Application.Current.MainWindow  as NavigationWindow;
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
            if (_RootFrame != null) {
                var navParameter = string.Empty;
                if (_ViewModelToViewMap.ContainsKey(typeof(TViewModel))) {
                    var page = _ViewModelToViewMap[typeof(TViewModel)];
                    _RootFrame.Navigate(new Uri(page, UriKind.Relative), parameterForView);
                }
            }
        }

        public void NavigateReplaceAndKeepCurrent<TNewViewModel, TCurrentViewModel>(object parameterForView = null) {
            if (_RootFrame != null) {
                _SavedState = typeof(TCurrentViewModel);
                Navigate<TNewViewModel>(parameterForView);
                if (_RootFrame.CanGoBack) {
                    _RootFrame.RemoveBackEntry();
                }
            }
        }

        public void NavigateToSavedState(object parameterForView = null) {
            if (_RootFrame != null) {
                if (_SavedState == null) {
                    throw new InvalidOperationException("No saved state exists.");
                }
                var method = typeof(NavigationServiceWpf).GetTypeInfo().GetDeclaredMethod("Navigate").MakeGenericMethod(_SavedState);
                method.Invoke(this, new object[] { parameterForView });
                _SavedState = null;
                if (_RootFrame.CanGoBack) {
                    _RootFrame.RemoveBackEntry();
                }
            }
        }
    }
}
