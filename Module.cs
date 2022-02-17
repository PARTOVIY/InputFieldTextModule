using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uScript.API.Attributes;
using uScript.Core;
using uScript.Module.Main.Classes;
using uScript.Unturned;

namespace InputFieldTextModule
{
    public partial class Module : ScriptModuleBase
    {
        public static string ModuleName = "InputFieldTextModule";
        public static Dictionary<Player, List<InputFieldModel>> InputFields = new Dictionary<Player, List<InputFieldModel>>();
        protected override void OnModuleLoaded()
        {
            CommandWindow.Log($"[{ModuleName}] | INFO >> Loaded");
            CommandWindow.Log($"[{ModuleName}] | INFO >> Author PARTOVIY#9987");
            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
            EffectManager.onEffectTextCommitted += EffectTextCommitted;
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            InputFields.Remove(player.Player);
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            InputFields.Add(Player.player, null);
        }

        private void EffectTextCommitted(Player player, string buttonName, string text)
        {
            if (InputFields[player] == null)
            {
                InputFields[player] = new List<InputFieldModel>()
                {
                    new InputFieldModel() { InputName = buttonName, InputValue = text }
                };
            }
            else
            {
                if(InputFields[player].Where(x => x.InputName == buttonName).First() == null)
                {
                    InputFields[player].Add(new InputFieldModel() { InputName = buttonName, InputValue= text });
                }
                else
                {
                    int index = InputFields[player].IndexOf(InputFields[player].Where(x => x.InputName == buttonName).First());
                    InputFields[player][index] = new InputFieldModel() { InputName = buttonName, InputValue = text };
                }
            }
        }
    }
    [ScriptTypeExtension(typeof(PlayerClass))]
    public class PlayerExtension
    {
        [ScriptFunction("get_getText")]
        public static string getText([ScriptInstance] ExpressionValue instance, string inputName)
        {
            if (instance.Data is PlayerClass playerClass)
            {
                if (Module.InputFields[playerClass.Player] != null)
                {
                    if (Module.InputFields[playerClass.Player].Where(x => x.InputName == inputName).First() != null)
                    {
                        return Module.InputFields[playerClass.Player].Where(x => x.InputName == inputName).First().InputValue;
                    }
                    else
                    {
                        return "null";
                    }
                }
                else
                {
                    return "null";
                }
            }
            return "null";
        }
    }
}
