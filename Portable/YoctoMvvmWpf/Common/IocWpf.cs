using System.ComponentModel;
using System.Windows;
using YoctoMvvm.Common;

namespace YoctoMvvmWpf.Common {
    public abstract class IocWpf : YoctoIoc {
        public override bool IsInDesignMode {
            get {
                return DesignerProperties.GetIsInDesignMode(new DependencyObject());
            }
        }
    }
}
