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
            
            TriggerManager.OnChartLoad += path =>
            {
                // Do sum loading shenanigans
                var triggers = new ITrigger[]
                {
                    new TestTrigger
                    {
                        Message = "This should fire at 2 seconds",
                        Time = 2f,
                        AlreadyTriggered = false,
                    },
                    new TestTrigger
                    {
                        Message = "This should fire at 5 seconds",
                        Time = 5f,
                        AlreadyTriggered = false,
                    },
                    new TestTrigger
                    {
                        Message = "This should fire at 3.5 seconds",
                        Time = 3.5f,
                        AlreadyTriggered = false,
                    }
                };
                TriggerManager.LoadTriggers(triggers);
            };
            
            TriggerManager.RegisterTriggerEvent<TestTrigger>((trigger, trackTime) =>
            {
                var testTrigger = (TestTrigger)trigger;
                if (testTrigger.AlreadyTriggered) return;
                Log(testTrigger.Message);
                testTrigger.AlreadyTriggered = true;
            });
        }

        internal static void Log(object msg) => _logger.LogMessage(msg);
    }
}
