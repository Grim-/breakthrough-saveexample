using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using OutwardModTemplate;
using SideLoader;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MoreBreakthroughs
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public partial class MoreBreakThroughs : BaseUnityPlugin
    {
        public const string GUID = "pop000100.moarbreakthrough";
        public const string NAME = "MoreBreakthroughs";
        public const string VERSION = "1.0.0";

        internal static ManualLogSource Log;
        public static ConfigEntry<int> StartingPoints;

        public static MoreBreakThroughs Instance;

        public Dictionary<string, CharacterBreakThroughSave> CharacterData = new Dictionary<string, CharacterBreakThroughSave>();

        internal void Awake()
        {
            Instance = this;
            Log = this.Logger;
            StartingPoints = Config.Bind(NAME, "Number of Breakthrough Points", 3, "starting number of Breakthrough Points.");

            SL.BeforePacksLoaded += SL_BeforePacksLoaded;
            new Harmony(GUID).PatchAll();
        }

        private void SL_BeforePacksLoaded()
        {
            SL_Item GrantBreakThroughPotion = new SL_Item()
            {
                Target_ItemID = 4300130,
                New_ItemID = -909090,
                Name = "TEST BREAKTHROUGH POTION",
                Description = "DRINK MEEE",
                EffectTransforms = new SL_EffectTransform[]
                {
                    new SL_EffectTransform()
                    {
                        TransformName = "Normal",
                        Effects = new SL_Effect[]
                        {
                            new SL_GrantAdditionalBreakthrough()
                            {
                                AmountToGrant = 1
                            }
                        }
                    }
                }
            };

            GrantBreakThroughPotion.ApplyTemplate();
        }

        public void IncreaseBreakThroughMaxForCharacter(string CharacterUID, int increaseAmount = 1)
        {
            if (HasCharacterDataForUID(CharacterUID))
            {
                CharacterData[CharacterUID].AdditionalPoints += increaseAmount;

            }
        }

        public bool CharacterHasBreakThroughPoint(Character Character)
        {
            MoreBreakThroughs.Log.LogMessage($"CharacterHasBreakThroughPoint ::Character {Character.Name}");

            var Data = GetData(Character.UID);

            if (Data != null)
            {
                int Used = Character.PlayerStats.m_usedBreakthroughCount;
                int CurrentMax = Data.StartingMax + Data.AdditionalPoints;
                MoreBreakThroughs.Log.LogMessage($"CharacterHasBreakThroughPoint:: Character Data EXISTS {Character.Name} [{Character.PlayerStats.m_usedBreakthroughCount}] / [{CurrentMax}] ");
                return Used < CurrentMax;
            }
            else
            {
                MoreBreakThroughs.Log.LogMessage($"CharacterHasBreakThroughPoint:: Character Data DOES NOT EXIST {Character.Name}" +
                    $" Current Max set to StartingPointsValue (this generally means they havent earned any yet) currently used [{Character.PlayerStats.m_usedBreakthroughCount}]");
                return Character.PlayerStats.m_usedBreakthroughCount < StartingPoints.Value;
            }
        }

        public CharacterBreakThroughSave AddCharacterData(string CharacterUID, int StartingPoints, int AdditionalBreakThroughs)
        {
            if (!HasCharacterDataForUID(CharacterUID))
            {
                MoreBreakThroughs.Log.LogMessage($"Adding Character Instance Data for {CharacterUID}");
                CharacterBreakThroughSave characterSaveData = new CharacterBreakThroughSave();
                characterSaveData.CharacterUID = CharacterUID;
                characterSaveData.AdditionalPoints = AdditionalBreakThroughs;
                characterSaveData.StartingMax = StartingPoints;

                CharacterData.Add(CharacterUID, characterSaveData);
                return characterSaveData;
            }

            return null;
        }

        public bool HasCharacterDataForUID(string CharacterUID)
        {
            return CharacterData.ContainsKey(CharacterUID);
        }

        public CharacterBreakThroughSave GetData(string CharacterUID)
        {
            if (HasCharacterDataForUID(CharacterUID))
            {
                return CharacterData[CharacterUID];
            }

            return null;
        }
    }
}
