using HarmonyLib;
using MoreBreakthroughs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardModTemplate
{
    //All skills unlocked, but all requiere breakthrough points
    [HarmonyPatch(typeof(BaseSkillSlot), "CheckCharacterRequirements")]
    class BaseSkillSlots_CheckCharacterRequirements
    {
        public static bool Prefix(ref bool __result, BaseSkillSlot __instance, Character _character, bool _notify = false)
        {
            if (_character.IsLocalPlayer)
            {
                var Data = MoreBreakThroughs.Instance.GetData(_character.UID);

                if (Data != null)
                {
                    __result = MoreBreakThroughs.Instance.CharacterHasBreakThroughPoint(_character);

                    if (__result == false)
                    {
                        _character.CharacterUI.ShowInfoNotification($"You don't have enough Breakthrough Points. [{_character.PlayerStats.m_usedBreakthroughCount}] / [{Data.StartingMax + Data.AdditionalPoints}] Used.");
                    }

                    //skip original check
                    return false;
                }
                else MoreBreakThroughs.Log.LogMessage($"Character Data null for {_character.UID}");
            }
            //run original check
            return true;
        }
    }
}
