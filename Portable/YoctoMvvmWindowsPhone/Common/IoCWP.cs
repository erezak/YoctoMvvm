using System;
using System.ComponentModel;
using System.Reflection;
using YoctoMvvm.Common;

namespace ReaderDoseLibWP.Common {
    public abstract class IoCWP : YoctoIoc {
        public override bool IsInDesignMode {
            get {
                return DesignerProperties.IsInDesignTool;
            }
        }
    }
}
