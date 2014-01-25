using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using YoctoMvvm.Common;

namespace YoctoMvvmWindowsStore.Common {
    public abstract class IocWindowsStore : YoctoIoc {
        public override bool IsInDesignMode {
            get {
                return DesignMode.DesignModeEnabled;
            }
        }
    }
}
