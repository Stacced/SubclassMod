using System.Collections.Generic;

using Exiled.API.Enums;

namespace SubclassMod.API.Classes
{
    public class CharacterInfo
    {
        public int Id { get; set; } = 0;
        public float Scale { get; set; } = 1f;

        public string Name { get; set; } = "Default name";
        public string Info { get; set; } = "Default Info";

        public RoleType BaseRole { get; set; } = RoleType.Spectator;
        public ZoneType SpawnZone { get; set; } = ZoneType.Entrance;

        public List<string> AllowedUsers { get; set; } = new List<string>();

        public List<ItemType> InventoryOverride { get; set; } = new List<ItemType>();
        
        public List<int> InventoryCustomItems { get; set; } = new List<int>();
    }
}