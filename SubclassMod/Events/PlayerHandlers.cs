using Exiled.Events.EventArgs;
using SubclassMod.API.Classes.Managers;
using SubclassMod.Components;
using UnityEngine;

namespace SubclassMod.Events
{
    public class PlayerHandlers
    {
        public void OnSpawned(SpawnedEventArgs ev) => SubclassesManager.AssignPlayer(ev.Player, ev.Player.Role);

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            ev.Player.DisplayNickname = null;
            ev.Player.CustomInfo = null;
            
            if (ev.Player.GameObject.TryGetComponent(out SubclassedPlayer subclassedPlayerComponent))
                Object.Destroy(subclassedPlayerComponent);
        }
    }
}