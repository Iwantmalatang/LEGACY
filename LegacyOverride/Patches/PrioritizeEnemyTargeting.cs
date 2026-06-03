using System;
using System.Collections.Generic;
using Enemies;
using GameData;
using HarmonyLib;
using LEGACY.Utils;
using Player;
using SNetwork;

namespace LEGACY.LegacyOverride.Patches
{
    [HarmonyPatch]
    internal class PrioritizeEnemyTargeting
    {
        private static bool s_patch = true;
		private static HashSet<uint> ForceOverrideLevels = new();

		[HarmonyPostfix]
        [HarmonyPatch(typeof(EnemyCourseNavigation), nameof(EnemyCourseNavigation.UpdateTracking))]
        private static void UpdateTracking(EnemyCourseNavigation __instance)
        {
			if (ForceOverride())
            {
				if (!s_patch) return;

				int playerCnt = PlayerManager.PlayerAgentsInLevel.Count;
				if (!SNet.IsMaster || playerCnt <= 1) return;

				var enemy = __instance.m_owner;
				if (enemy.Locomotion.m_currentState.m_stateEnum == ES_StateEnum.Hibernate) return;

				var originalTarget = __instance.m_targetRef.m_agent;

				if (originalTarget.CourseNode.Pointer == enemy.CourseNode.Pointer) return;

				PlayerAgent newTarget = null;

				for (int i = UnityEngine.Random.RandomRangeInt(0, playerCnt),
					cnt = 0;
					cnt < playerCnt;
					i = (i + 1) % playerCnt, cnt++)
				{
					var player = PlayerManager.PlayerAgentsInLevel[i];
					if (player.Alive && player.CourseNode.Pointer == enemy.CourseNode.Pointer)
					{
						newTarget = player;
						break;
					}
				}

				if (newTarget != null)
				{
					s_patch = false;
					enemy.AI.SetTarget(newTarget); // SetTarget calls EnemyCourseNavigation.UpdateTracking,
					s_patch = true;
				}
			}
        }

		private static bool ForceOverride() => ForceOverrideLevels.Contains(RundownManager.ActiveExpedition.LevelLayoutData);

		static PrioritizeEnemyTargeting()
		{
			var block = LevelLayoutDataBlock.GetBlock("2026_F.04_Layout");
			if (block != null)
			{
				ForceOverrideLevels.Add(block.persistentID);
			}
		}
	}
}
