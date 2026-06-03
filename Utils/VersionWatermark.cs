using HarmonyLib;
using LEGACY;
using MTFO;

[HarmonyPatch(typeof(PUI_Watermark), nameof(PUI_Watermark.UpdateWatermark))]
internal static class Patch_WatermarkUpdateWatermark
{
    private static void Postfix(PUI_Watermark __instance)
    {
        string MTFOVersion = MTFO.MTFO.VERSION.Remove("x.x.x".Length);
        __instance.m_watermarkText.SetText(
            $"<color=red>MODDED</color> <color=orange>{MTFOVersion}</color>" +
			$"<color=#FF3111>\nPE 2.6.6</color>");
		//$"<color=#FF3111>\nPE <color=#00ae9d>TEST</color></color>");
		// PE: #FF3111
		// TRIALS: #00ae9d
		// SURVIVAL: #33FF99
		// <color=#00ae9d>PROJECT</color><color=orange>:</color> <color=#00ae9d>ETERNAL</color> <color=orange>II</color>
	}
}