using Exiled.Events.EventArgs;
using SubclassMod.API.Classes.Managers;

namespace SubclassMod.Events
{
    public class PlayerHandlers
    {
        public void OnSpawned(SpawnedEventArgs ev) => SubclassesManager.AssignPlayer(ev.Player, ev.Player.Role);

        public void OnChangingRole(ChangingRoleEventArgs ev) => SubclassesManager.ResetPlayer(ev.Player);
    }
}