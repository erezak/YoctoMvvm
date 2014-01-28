using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoctoMvvmWindowsStore.Platform {
    public interface INavigationMappingProvider {
        Dictionary<Type, Type> ProvideViewModelToViewMap();
    }
}
