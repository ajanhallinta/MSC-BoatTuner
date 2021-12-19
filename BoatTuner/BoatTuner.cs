using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker.Actions;
using System.Linq;

namespace BoatTuner
{
    public class BoatTuner : Mod
    {
        public override string ID => "BoatTuner"; //Your mod ID (unique)
        public override string Name => "BoatTuner"; //You mod name
        public override string Author => "ajanhallinta"; //Your Username
        public override string Version => "1.0"; //Version
        public override string Description => "A simple mod for tweaking RPMmax value of Boat"; //Short description of your mod

        private SettingsTextBox TextBoxMaxRPM;
        private string maxRpmString = "4000";

        public override void ModSettings()
        {
            // All settings should be created here. 
            // DO NOT put anything else here that settings or keybinds
            Settings.AddHeader(this, "Boat Power Settings");
            Settings.AddText(this, "Boat engine needs to be running before applying new values.");
            Settings.AddText(this, "Default RPM value is 4000.");
            TextBoxMaxRPM = Settings.AddTextBox(this, "maxrpm", "MaxRPM", maxRpmString, maxRpmString);
            Settings.AddButton(this, "apply", "Apply Max RPM", () =>
            {
                ApplyNewMaxRpm();
            });

            Settings.AddHeader(this, "Support");
            Settings.AddText(this, "I think there is some space left for money in my PayPal.");
            Settings.AddButton(this, "support", "PayPal", () =>
            {
                try
                {
                    Application.OpenURL("https://paypal.me/ajanhallinta");
                }
                catch
                {
                }
            });
        }

        private void ApplyNewMaxRpm()
        {
            try
            {
                float newRpm;
                if(float.TryParse(TextBoxMaxRPM.Instance.Value.ToString(), out newRpm))
                {
                    PlayMakerFSM fsm = GameObject.Find("BOAT/Controls").GetComponent<PlayMakerFSM>();
                    FloatOperator RPMmax = GetFloatOperatorFromFSM(fsm, "State 1", 3);
                    if (RPMmax != null)
                    {
                        RPMmax.float2 = newRpm;
                        ModConsole.Print("Applied new RPMmax: " + newRpm.ToString());
                    }
                }
            }
            catch
            {
                ModConsole.Print("BoatTuner: Something went wrong when applying new RPMmax...");
            }
        }

        private FloatOperator GetFloatOperatorFromFSM(PlayMakerFSM fsm, string stateName, int actionIndex)
        {
            try
            {
                return (FloatOperator)fsm.FsmStates.Where(x => x.Name == stateName).FirstOrDefault().Actions[actionIndex];
            }
            catch
            {
                return null;
            }
        }
    }
}
