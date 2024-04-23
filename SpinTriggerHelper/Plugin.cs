using BepInEx;
using BepInEx.Logging;

namespace SpinTriggerHelper
{
    [BepInPlugin(Guid, Name, Version)]
    internal class Plugin : BaseUnityPlugin
    {
        private const string Guid = "srxd.raoul1808.triggerhelper";
        private const string Name = "Spin Trigger Helper";
        private const string Version = "0.1.0";
        
        private static ManualLogSource _logger;
        
        private void Awake()
        {
            _logger = Logger;
            Log($"Hello from {Name}");
        }

        internal static void Log(object msg) => _logger.LogMessage(msg);
    }
}
