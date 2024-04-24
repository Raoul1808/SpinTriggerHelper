using System;
using System.Collections.Generic;
using System.Reflection;

namespace SpinTriggerHelper
{
    public static class TriggerManager
    {
        private static readonly Dictionary<string, ModTriggerStore> TriggerStores = new Dictionary<string, ModTriggerStore>();

        public delegate void ChartLoad(string path);
        
        /// <summary>
        /// This event is fired whenever the game loads a custom chart.
        /// </summary>
        public static event ChartLoad OnChartLoad;

        private static string GetKeyForTrigger(Assembly assembly, ITrigger trigger)
        {
            return $"{assembly.GetName().Name}-{trigger.GetType().Name}";
        }

        private static string GetKeyForTrigger<T>(Assembly assembly)
        {
            return $"{assembly.GetName().Name}-{typeof(T).Name}";
        }

        /// <summary>
        /// Loads the given triggers into the internal trigger manager.
        /// </summary>
        /// <param name="triggers">The list of triggers</param>
        /// <exception cref="ArgumentException">Raised if the given array contains nothing</exception>
        public static void LoadTriggers(ITrigger[] triggers)
        {
            if (triggers.Length == 0)
                throw new ArgumentException("ITrigger array needs to contain triggers");
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
        
        /// <summary>
        /// Fires the given method when a trigger is fired/updates
        /// </summary>
        /// <param name="action">A callback method</param>
        /// <typeparam name="T">The affected trigger (required)</typeparam>
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
