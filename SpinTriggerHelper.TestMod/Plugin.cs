using BepInEx;
using BepInEx.Logging;

namespace SpinTriggerHelper.TestMod
{
    [BepInPlugin(Guid, Name, Version)]
    [BepInDependency("srxd.raoul1808.triggerhelper")]
    public class Plugin : BaseUnityPlugin
    {
        private const string Guid = "srxd.raoul1808.triggerhelper.testmod";
        private const string Name = "Trigger Test Mod";
        private const string Version = "0.1.0";

        private static ManualLogSource _logger;

        private void Awake()
        {
            _logger = Logger;
            Log("Hello from test mod!");
            
            TriggerManager.TestCallingAssembly();
        }

        internal static void Log(object msg) => _logger.LogMessage(msg);
    }
}
