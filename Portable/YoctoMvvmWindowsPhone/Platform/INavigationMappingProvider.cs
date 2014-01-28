using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoctoMvvmWindowsPhone.Platform {
    public interface INavigationMappingProvider {
        Dictionary<Type, string> ProvideViewModelToViewMap();
    }
}
