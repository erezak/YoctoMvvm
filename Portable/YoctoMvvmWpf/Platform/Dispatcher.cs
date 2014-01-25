using System;
using System.Windows;
using YoctoMvvm.Platform;

namespace YoctoMvvmWpf.Platform {
    public class Dispatcher : IDispatcher {
        public void RunOnUiThread(Action action) {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
