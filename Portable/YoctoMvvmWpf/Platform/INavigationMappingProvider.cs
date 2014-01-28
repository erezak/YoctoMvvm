using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoctoMvvmWpf.Platform {
    public interface INavigationMappingProvider {
        Dictionary<Type, string> ProvideViewModelToViewMap();
    }
}
