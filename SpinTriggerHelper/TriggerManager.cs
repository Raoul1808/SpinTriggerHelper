using System;
using System.Collections.Generic;
using System.Reflection;

namespace SpinTriggerHelper
{
    public static class TriggerManager
    {
        private static readonly Dictionary<string, ModTriggerStore> TriggerStores = new Dictionary<string, ModTriggerStore>();

        public delegate void ChartLoad(string path);
        public static event ChartLoad OnChartLoad;

        private static string GetKeyForTrigger(Assembly assembly, ITrigger trigger)
        {
            return $"{assembly.GetName().Name}-{trigger.GetType().Name}";
        }

        private static string GetKeyForTrigger<T>(Assembly assembly)
        {
            return $"{assembly.GetName().Name}-{typeof(T).Name}";
        }

        public static void LoadTriggers(ITrigger[] triggers)
        {
            if (triggers.Length == 0)
                return;
            var trigger = triggers[0];
            string key = GetKeyForTrigger(Assembly.GetCallingAssembly(), trigger);
            if (!TriggerStores.TryGetValue(key, out var store))
            {
                store = new ModTriggerStore();
                TriggerStores.Add(key, store);
            }

            store.Clear();
            store.AddTriggers(triggers);
        }
        
        public delegate void TriggerUpdate(ITrigger trigger, float trackTime);
        
        public static void RegisterTriggerEvent<T>(TriggerUpdate action) where T : ITrigger
        {
            string key = GetKeyForTrigger<T>(Assembly.GetCallingAssembly());
            if (!TriggerStores.TryGetValue(key, out var store))
            {
                store = new ModTriggerStore();
                TriggerStores.Add(key, store);
            }

            store.OnTriggerUpdate += action;
        }

        internal static void Update(float trackTime)
        {
            foreach (var store in TriggerStores.Values)
            {
                store.Update(trackTime);
            }
        }

        internal static void InvokeChartLoadEvent(string path)
        {
            OnChartLoad?.Invoke(path);
        }

        internal static void ResetTriggerStores()
        {
            foreach (var store in TriggerStores.Values)
            {
                store.Reset();
            }
        }
    }
}
