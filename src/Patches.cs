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
                int CurrentUsed = _character.PlayerStats.m_usedBreakthroughCount;
                

                //have we used more than the normal amount, and 
                if (MoreBreakThroughs.Instance.CharacterHasBreakThroughPoint(_character))
                {

                    var Data = MoreBreakThroughs.Instance.GetData(_character.UID);


                    if (Data == null)
                    {
                        Data = MoreBreakThroughs.Instance.AddCharacterData(_character.UID, MoreBreakThroughs.StartingPoints.Value, 0);
                    }


                    if (Data != null)
                    {
                        int CurrentMaxPoints = Data.AdditionalPoints;

                        int DifferenceBetweenCurrentAndNewMax = CurrentMaxPoints - _character.PlayerStats.m_usedBreakthroughCount;
                        MoreBreakThroughs.Log.LogMessage($" Character : {_character.Name} loaded Current UsedBreakthrough points : [{_character.PlayerStats.m_usedBreakthroughCount}] Maximum is [{CurrentMaxPoints}] Left to spend : {DifferenceBetweenCurrentAndNewMax}");
                        __result = true;
                        //skip original check
                        return false;
                    }
                    else MoreBreakThroughs.Log.LogMessage($"Character Data null for {_character.UID}");
                }
            }
            //run original check
            return true;
        }
    }
}
