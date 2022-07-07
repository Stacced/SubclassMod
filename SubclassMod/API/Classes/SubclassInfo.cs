using System.Collections.Generic;
using Exiled.API.Enums;
using SubclassMod.API.Enums;
using UnityEngine;

namespace SubclassMod.API.Classes
{
    public class SubclassInfo
    {
        public int Id { get; set; } = 0;
        public int MaxPlayers { get; set; } = 0;
        public float Health { get; set; } = 100f;
        public float SpawnPercent { get; set; } = 50f;

        public string Name { get; set; } = "Unknown subclass";
        public string Description { get; set; } = "Description of unknown subclass";
        public string NamePrefix { get; set; } = "Unk.";
        public string NamePostfix { get; set; } = "!";
        public string CustomInfo { get; set; } = "Unknown Person";
        
        public bool ForceclassOnly { get; set; } = false;
        public bool RoleplayNameEnabled { get; set; } = true;
        public bool RoleplaySecondNameEnabled { get; set; } = true;

        public RoleType BaseRole { get; set; } = RoleType.ClassD;
        public SpawnMethod SpawnMethod { get; set; } = SpawnMethod.SpawnZone;

        public Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>
        {
            [AmmoType.Nato9] = 10,
            [AmmoType.Nato556] = 10,
            [AmmoType.Nato762] = 10
        };

        public ItemType[] Items { get; set; } = { ItemType.Coin };
        public ZoneType[] SpawnZones { get; set; } = { ZoneType.Entrance };
        public RoomType[] SpawnRooms { get; set; } = { RoomType.LczChkpA };
        public Vector3[] SpawnPositions { get; set; } = { Vector3.zero };
    }
}