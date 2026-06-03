using HarmonyLib;
using LevelGeneration;
using UnityEngine;

namespace LEGACY.LegacyOverride.Patches
{
    [HarmonyPatch]
    internal static class PouncerArenaPosition_Fix
    {
        [HarmonyPatch(typeof(LG_Floor), nameof(LG_Floor.CreateDimension))]
        [HarmonyPrefix]
        static void CreateDimension_Prefix(eDimensionIndex dimensionIndex, bool arenaDimension, ref Vector3 position)
        {
            switch (dimensionIndex)
            {
                case eDimensionIndex.Dimension_17:
                case eDimensionIndex.Dimension_18:
                case eDimensionIndex.Dimension_19:
                case eDimensionIndex.Dimension_20:
                    position += new Vector3 { y = -500f };
                    break;
            }
        }
    }
}
