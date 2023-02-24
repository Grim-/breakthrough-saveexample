using System.Collections.Generic;
using System.Linq;
using SideLoader.SaveData;

namespace MoreBreakthroughs
{
    //dont need it
    public class MoreBreakThroughsSaveData : PlayerSaveExtension
    {
        //think of this list as purely a way to get your data in and out of the save system,
        //whatever you fill it up with will be saved
        public List<CharacterBreakThroughSave> CharacterSaveData;

        public override void ApplyLoadedSave(Character character, bool isWorldHost)
        {
            //read from here and load into the main plug
            foreach (var _character in CharacterSaveData)
            {
                MoreBreakThroughs.Log.LogMessage($"Loaded Save  Data for {_character.CharacterUID}  Start Max : {_character.StartingMax} Current Additional : {_character.AdditionalPoints}");
                MoreBreakThroughs.Instance.AddCharacterData(character.UID, _character.StartingMax, _character.AdditionalPoints);
            }
        }

        public override void Save(Character character, bool isWorldHost)
        {
            //clear the list, we dont care what was in it previously
            CharacterSaveData.Clear();

            //read from main plugin into this list list
            foreach (var characterData in MoreBreakThroughs.Instance.CharacterData)
            {
                //add the data from plugin to list so its saved
                MoreBreakThroughs.Log.LogMessage($"Saving Data for {characterData.Key}  Start Max : {characterData.Value.StartingMax} Current Additional : {characterData.Value.AdditionalPoints}");
                AddCharacterSaveData(characterData.Key, characterData.Value.StartingMax, characterData.Value.AdditionalPoints);
            }
        }

        public void AddCharacterSaveData(string CharacterUID, int StartingMaximum, int CurrentMaximum)
        {
            MoreBreakThroughs.Log.LogMessage($"Saving Data for {CharacterUID}");
            CharacterBreakThroughSave data = new CharacterBreakThroughSave();
            data.CharacterUID = CharacterUID;
            data.StartingMax = StartingMaximum;
            data.AdditionalPoints = CurrentMaximum;
            CharacterSaveData.Add(data);          
        }
    }

}
