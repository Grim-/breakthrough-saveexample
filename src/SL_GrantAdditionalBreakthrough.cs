using MoreBreakthroughs;
using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardModTemplate
{
    //Example of a Custom SL_Effect

    //This class is your SL Definition of the effect (For the XML)
    public class SL_GrantAdditionalBreakthrough : SL_Effect, ICustomModel
    {
        //implementing ICustomModel, means you have to define these two variables below, SLTemplateModel is this file, GameModel is going to be where your code is
        public Type SLTemplateModel => typeof(SL_GrantAdditionalBreakthrough);
        public Type GameModel => typeof(GrantAdditionalBreakthrough);


        public int AmountToGrant;

        //here we take the values from XML and apply them to our component below
        public override void ApplyToComponent<T>(T component)
        {
            //cast the component to our GameModel type
            GrantAdditionalBreakthrough comp = component as GrantAdditionalBreakthrough;
            //apply the values from XML to it
            comp.AmountToGrant = AmountToGrant;
        }

        //this takes values from an already defined in-game GameObject and sets this classes member variables to them
        public override void SerializeEffect<T>(T effect)
        {
            GrantAdditionalBreakthrough comp = effect as GrantAdditionalBreakthrough;
            this.AmountToGrant = comp.AmountToGrant;
        }
    }

    public class GrantAdditionalBreakthrough : Effect
    {
        public int AmountToGrant;

        //here is when the Effect is actually called, here goes your logic
        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (MoreBreakThroughs.Instance != null)
            {
                MoreBreakThroughs.Log.LogMessage($"Granting {_affectedCharacter.Name} {AmountToGrant} BreakThrough Points");

                if (MoreBreakThroughs.Instance.HasCharacterDataForUID(_affectedCharacter.UID))
                {
                    MoreBreakThroughs.Log.LogMessage($"Character has existing data");
                    MoreBreakThroughs.Instance.IncreaseBreakThroughMaxForCharacter(_affectedCharacter.UID, AmountToGrant);
                }
                else
                {
                    MoreBreakThroughs.Log.LogMessage($"Character has no existing data");
                    MoreBreakThroughs.Instance.AddCharacterData(_affectedCharacter.UID, MoreBreakThroughs.StartingPoints.Value, AmountToGrant);
                }  
            }
        }
    }
}
