using Exiled.Events.EventArgs;
using SubclassMod.API.Classes.Managers;

namespace SubclassMod.Events
{
    public class PlayerHandlers
    {
        public void OnSpawned(SpawnedEventArgs ev) => SubclassesManager.AssignPlayer(ev.Player, ev.Player.Role);

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            ev.Player.DisplayNickname = null;
            ev.Player.CustomInfo = null;
        }
    }
}