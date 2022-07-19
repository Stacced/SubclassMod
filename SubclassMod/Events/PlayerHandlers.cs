using Exiled.API.Features;
using Exiled.Events.EventArgs;
using SubclassMod.API.Classes.Managers;
using SubclassMod.Components;
using UnityEngine;

namespace SubclassMod.Events
{
    public class PlayerHandlers
    {
        public void OnSpawned(SpawnedEventArgs ev)
        {
            Log.Debug("OnSpawned() called");
            
            SubclassesManager.AssignPlayer(ev.Player, ev.Player.Role);
        }

        public void OnChangingRole(ChangingRoleEventArgs ev) => SubclassesManager.ResetPlayer(ev.Player);
    }
}