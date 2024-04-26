using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SpinTriggerHelper
{
    [BepInPlugin(Guid, Name, Version)]
    internal class Plugin : BaseUnityPlugin
    {
        private const string Guid = "srxd.raoul1808.triggerhelper";
        private const string Name = "Spin Trigger Helper";
        private const string Version = "1.0.0";
        
        private static ManualLogSource _logger;
        
        private void Awake()
        {
            _logger = Logger;
            Log($"Hello from {Name}");
            var harmony = new Harmony(Guid);
            harmony.PatchAll(typeof(Patches));
        }

        internal static void Log(object msg) => _logger.LogMessage(msg);

        private class Patches
        {
            [HarmonyPatch(typeof(Track), nameof(Track.Update))]
            [HarmonyPostfix]
            private static void UpdateTriggers()
            {
                if (Track.PlayStates.Length == 0)
                    return;
                var playStateFirst = Track.PlayStates[0];
                TriggerManager.Update(playStateFirst.currentTrackTime);
            }

            [HarmonyPatch(typeof(Track), nameof(Track.PlayTrack))]
            [HarmonyPostfix]
            private static void ChartPlay()
            {
                TriggerManager.ResetTriggerStores();
            }

            [HarmonyPatch(typeof(Track), nameof(Track.ReturnToPickTrack))]
            [HarmonyPostfix]
            private static void ReturnToPickTrack()
            {
                TriggerManager.ClearAllTriggers();
            }

            [HarmonyPatch(typeof(SplineTrackData.DataToGenerate), MethodType.Constructor, typeof(PlayableTrackData))]
            [HarmonyPostfix]
            private static void ChartLoaded(PlayableTrackData trackData)
            {
                // TODO: find a better patch than this
                if (trackData.TrackDataList.Count == 0)
                    return;
                var data = trackData.TrackDataList[0];
                string path = data.CustomFile?.FilePath;
                if (string.IsNullOrEmpty(path))
                    return;
                TriggerManager.InvokeChartLoadEvent(data);
            }
        }
    }
}
