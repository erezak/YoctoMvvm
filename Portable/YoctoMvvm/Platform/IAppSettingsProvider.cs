
namespace YoctoMvvm.Platform {
    /// <summary>
    /// Represents general settings provider
    /// </summary>
    /// 
    /// <author>
    /// Erez A. Korn
    /// </author>
    public interface IAppSettingsProvider {
        /// <summary>
        /// Saves a single named setting
        /// </summary>
        /// <param name="name">Name of setting (key)</param>
        /// <param name="value">value of setting</param>
        void SaveSingleSetting(string name, object value);
        /// <summary>
        /// Deletes a single setting from store
        /// </summary>
        /// <param name="name">Name of setting (key)</param>
        void DeleteSingleSetting(string name);
        /// <summary>
        /// Loads the value of a single setting from store
        /// </summary>
        /// <param name="name">Name of setting (key)</param>
        object LoadSingleSetting(string name);
        /// <summary>
        /// Returns device ID (IMEI, MAC, etc.)
        /// </summary>
        /// <returns></returns>
        string GetDeviceUniqueId();
    }
}
