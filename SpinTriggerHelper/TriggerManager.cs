using System.Reflection;

namespace SpinTriggerHelper
{
    public static class TriggerManager
    {
        public static void TestCallingAssembly()
        {
            var assembly = Assembly.GetCallingAssembly();
            Plugin.Log($"{assembly.GetName().Name} says hi!");
        }
    }
}
