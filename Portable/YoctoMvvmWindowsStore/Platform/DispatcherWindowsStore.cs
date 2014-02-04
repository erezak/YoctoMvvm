using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using YoctoMvvm.Platform;

namespace YoctoMvvmWindowsStore.Platform {
    public class DispatcherWindowsStore : IDispatcher {
        public async void RunOnUiThread(Action action) {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                action();
            });
        }
    }
}
