using System;
using System.Collections.Generic;
using System.Linq;

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;

using SubclassMod.API.Enums;
using SubclassMod.Components;

using UnityEngine;

using MEC;

using PlayerRoles;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace SubclassMod.API.Classes.Managers
{
    public static class SubclassesManager
    {
        private static readonly Dictionary<SubclassInfo, byte> _subclassedCounter = new Dictionary<SubclassInfo, byte>();
        private static readonly List<string> _ignoredPlayers = new List<string>();

        private static readonly List<RoomType> _ignoredRooms = new List<RoomType>
        {
            RoomType.EzCollapsedTunnel,
            RoomType.EzCollapsedTunnel
        };
        
        public static void SpawnCustomCharacter(Player player, CharacterInfo character) =>
            Timing.RunCoroutine(ForceAsCharacter(player, character));
        
        public static void AssignPlayer(Player player, RoleTypeId role)
        {
            if (_ignoredPlayers.Contains(player.UserId))
            {
                _ignoredPlayers.Remove(player.UserId);
                
                return;
            }

            if (role.Equals(RoleTypeId.Tutorial))
                return;
            
            if (player.GameObject.TryGetComponent<SubclassedPlayer>(out _))
                return;

            if (TryGetSubclasses(role, out SubclassInfo[] targetSubclasses))
            {
                if (Random.Range(0, 100) <= SubclassMod.Instance.Config.SubclassChance && SubclassMod.Instance.Config.CustomRolesInfo.ContainsKey(role))
                {
                    Timing.RunCoroutine(ForceAsOverriddenRole(player, SubclassMod.Instance.Config.CustomRolesInfo[role]));
                    return;
                }

                Timing.RunCoroutine(ForceAsSubclass(player, targetSubclasses.RandomItem()));

                return;
            }
            
            if (SubclassMod.Instance.Config.CustomRolesInfo.ContainsKey(role))
            {
                Timing.RunCoroutine(ForceAsOverriddenRole(player, SubclassMod.Instance.Config.CustomRolesInfo[role]));
                
                return;
            }


            player.DisplayNickname = NicknamesManager.GetRoleName(player);
        }

        public static void ResetPlayer(Player player)
        {
            player.CustomInfo = null;
            player.DisplayNickname = null;

            if (player.GameObject.TryGetComponent(out SubclassedPlayer subclassedPlayerComponent))
                Object.Destroy(subclassedPlayerComponent);
        }

        public static bool TryGetSubclasses(int id, out SubclassInfo[] subclasses)
        {
            SubclassInfo[] targetSubclasses =
                SubclassMod.Instance.Config.CustomSubclasses.Where(x => x.Id == id).ToArray();

            if (targetSubclasses.Length == 0)
            {
                subclasses = null;
                return false;
            }

            subclasses = targetSubclasses;
            return true;
        }
        
        private static bool TryGetSubclasses(RoleTypeId role, out SubclassInfo[] subclasses)
        {
            SubclassInfo[] roleSubclasses = SubclassMod.Instance.Config.CustomSubclasses.Where(x => x.BaseRole == role && !x.ForceclassOnly && IsSubclassFree(x)).ToArray();

            if (roleSubclasses.Length == 0)
            {
                subclasses = null;
                return false;
            }

            subclasses = roleSubclasses;
            return true;
        }
        
        public static IEnumerator<float> ForceAsSubclass(Player player, SubclassInfo subclassInfo)
        {
            RoleTypeId initialRole = player.Role.Type;
            
            yield return Timing.WaitForSeconds(0.35f);

            if (!initialRole.Equals(player.Role.Type))
                yield break;
            
            try
            {
                if (player.Role.Type != subclassInfo.BaseRole)
                    player.RoleManager.ServerSetRole(subclassInfo.BaseRole, RoleChangeReason.Respawn);

                SubclassedPlayer subclassPlayer = player.GameObject.AddComponent<SubclassedPlayer>();
                
                subclassPlayer.ActiveSubclass = subclassInfo;

                switch (subclassInfo.SpawnMethod)
                {
                    case SpawnMethod.SpawnPositions:
                        player.Position = subclassInfo.SpawnPositions.RandomItem();
                        break;
                    case SpawnMethod.SpawnRooms:
                        Room targetRoom = Room.Get(subclassInfo.SpawnRooms.RandomItem()) ??
                                          Room.Get(RoomType.EzIntercom);

                        player.Position = targetRoom.Position + Vector3.up;
                        break;
                    case SpawnMethod.SpawnZone:
                        ZoneType targetZone = subclassInfo.SpawnZones.RandomItem();
                        player.Position = Room.List.Where(x => x.Zone == targetZone && !_ignoredRooms.Contains(x.Type)).ToList().RandomItem().Position + Vector3.up;
                        break;
                }
                
                player.Health = subclassInfo.Health;
                
                player.ClearInventory();
                
                foreach (ItemType item in subclassInfo.Items)
                    player.AddItem(item);

                foreach (int itemId in subclassInfo.CustomItems)
                    CustomItem.TryGive(player, itemId, false);

                foreach (AmmoType type in subclassInfo.Ammo.Keys)
                    player.AddAmmo(type, subclassInfo.Ammo[type]);

                player.DisplayNickname = NicknamesManager.GetRoleName(player, subclassInfo);

                player.CustomInfo = subclassInfo.CustomInfo;

                BroadcastRole(player, player.DisplayNickname, subclassInfo.Name, subclassInfo.Description);

                if (_subclassedCounter.ContainsKey(subclassInfo))
                    _subclassedCounter[subclassInfo] += 1;
                else 
                    _subclassedCounter.Add(subclassInfo, 1);
            }
            catch (Exception e)
            {
                Log.Debug($"{e.Message} | {e.Source} | {e.StackTrace}");
            }
        }

        private static IEnumerator<float> ForceAsOverriddenRole(Player player, RoleInfo roleInfo)
        {
            RoleTypeId initialRole = player.Role;
            
            yield return Timing.WaitForSeconds(0.35f);

            if (!initialRole.Equals(player.Role.Type))
                yield break;

            try
            {
                SubclassedPlayer subclassPlayer = player.GameObject.AddComponent<SubclassedPlayer>();
                subclassPlayer.ActiveRole = roleInfo;

                player.DisplayNickname = NicknamesManager.GetRoleName(player, roleInfo);
                player.CustomInfo = roleInfo.CustomInfo;

                if (roleInfo.InventoryOverridden)
                {
                    player.ClearInventory();
                
                    foreach (ItemType item in roleInfo.InventoryOverwrite)
                        player.AddItem(item);
                    
                    foreach (int itemId in roleInfo.InventoryCustomItems)
                        CustomItem.TryGive(player, itemId, false);
                }
            }
            catch (Exception e)
            {
                Log.Debug($"{e.Message} | {e.Source} | {e.StackTrace}");
            }
        }

        private static IEnumerator<float> ForceAsCharacter(Player player, CharacterInfo characterInfo)
        {
            _ignoredPlayers.Add(player.UserId);

            yield return Timing.WaitForSeconds(0.15f);

            player.RoleManager.ServerSetRole(characterInfo.BaseRole, RoleChangeReason.Respawn);

            yield return Timing.WaitForSeconds(0.45f);

            Room spawnRoom = Room.Random(characterInfo.SpawnZone);

            player.Position = spawnRoom.Position + Vector3.up;

            player.ClearInventory();

            foreach (ItemType item in characterInfo.InventoryOverride)
                player.AddItem(item);
            
            foreach (int itemId in characterInfo.InventoryCustomItems)
                CustomItem.TryGive(player, itemId, false);

            player.Scale = Vector3.one * characterInfo.Scale;

            player.CustomInfo = characterInfo.Info;
            player.DisplayNickname = characterInfo.Name;
        }

        private static bool IsSubclassFree(SubclassInfo subclassInfo)
        {
            if (subclassInfo.MaxPlayers.Equals(0))
                return true;
            
            if (Random.Range(0, 100) >= subclassInfo.SpawnPercent)
                return false;

            if (!_subclassedCounter.ContainsKey(subclassInfo))
                return true;

            return _subclassedCounter[subclassInfo] < subclassInfo.MaxPlayers;
        }

        private static void BroadcastRole(Player player, string roleName, string className, string classDescription) =>
            player.Broadcast(15, string.Format(SubclassMod.Instance.Translation.SpawnDescriptionInfo, roleName, className, classDescription));
    }
}