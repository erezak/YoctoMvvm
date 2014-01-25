using System;
using System.Windows;
using YoctoMvvm.Platform;

namespace ReaderDoseLibWP.Common {
    public class Dispatcher : IDispatcher {
        public void RunOnUiThread(Action action) {
            Deployment.Current.Dispatcher.BeginInvoke(action);
        }
    }
}
