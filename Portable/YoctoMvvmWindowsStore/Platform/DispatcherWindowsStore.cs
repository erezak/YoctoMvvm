using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using YoctoMvvm.Platform;

namespace YoctoMvvmWindowsStore.Platform {
    public class DispatcherWindowsStore : IDispatcher {
        public async void RunOnUiThread(Action action) {
            await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                action();
            });
        }
    }
}
