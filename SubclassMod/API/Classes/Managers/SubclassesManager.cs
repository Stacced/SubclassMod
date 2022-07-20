using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using SubclassMod.API.Enums;
using SubclassMod.Components;
using UnityEngine;
using MEC;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace SubclassMod.API.Classes.Managers
{
    public static class SubclassesManager
    {
        // TODO: May be here i'll use queues for spawn players.

        private static readonly Dictionary<SubclassInfo, byte> SubclassedCounter = new Dictionary<SubclassInfo, byte>();
        
        public static void AssignPlayer(Player player, RoleType role)
        {
            if (role.Equals(RoleType.Tutorial))
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

        public static bool TryGetSubclasses(RoleType role, out SubclassInfo[] subclasses)
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
        
        public static IEnumerator<float> ForceAsSubclass(Player player, SubclassInfo subclassInfo)
        {
            RoleType initialRole = player.Role;
            
            yield return Timing.WaitForSeconds(0.35f);

            if (!initialRole.Equals(player.Role.Type))
                yield break;
            
            try
            {
                if (player.Role != subclassInfo.BaseRole)
                    player.SetRole(subclassInfo.BaseRole);

                SubclassedPlayer subclassPlayer = player.GameObject.AddComponent<SubclassedPlayer>();
                subclassPlayer.ActiveSubclass = subclassInfo;

                switch (subclassInfo.SpawnMethod)
                {
                    case SpawnMethod.SpawnPositions:
                        player.Position = subclassInfo.SpawnPositions.RandomItem();
                        break;
                    case SpawnMethod.SpawnRooms:
                        player.Position = Room.Get(subclassInfo.SpawnRooms.RandomItem()).Position + new Vector3(0, 1f, 0);
                        break;
                    case SpawnMethod.SpawnZone:
                        player.Position = Room.Get(subclassInfo.SpawnZones.RandomItem()).ToArray().RandomItem().Position + new Vector3(0, 1f, 0);;
                        break;
                }

                player.Health = subclassInfo.Health;

                player.ClearInventory();

                foreach (ItemType item in subclassInfo.Items)
                    player.AddItem(item);

                foreach (AmmoType type in subclassInfo.Ammo.Keys)
                    player.AddAmmo(type, subclassInfo.Ammo[type]);

                player.DisplayNickname = NicknamesManager.GetRoleName(player, subclassInfo);
                player.CustomInfo = subclassInfo.CustomInfo;

                BroadcastRole(player, player.DisplayNickname, subclassInfo.Name, subclassInfo.Description);

                if (SubclassedCounter.ContainsKey(subclassInfo))
                    SubclassedCounter[subclassInfo] += 1;
                else 
                    SubclassedCounter.Add(subclassInfo, 1);
            }
            catch (Exception e)
            {
                Log.Debug($"{e.Message} | {e.Source} | {e.StackTrace}");
            }
        }

        private static IEnumerator<float> ForceAsOverriddenRole(Player player, RoleInfo roleInfo)
        {
            RoleType initialRole = player.Role;
            
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
                }
            }
            catch (Exception e)
            {
                Log.Debug($"{e.Message} | {e.Source} | {e.StackTrace}");
            }
        }

        private static bool IsSubclassFree(SubclassInfo subclassInfo)
        {
            if (subclassInfo.MaxPlayers.Equals(0))
                return true;
            
            if (Random.Range(0, 100) <= subclassInfo.SpawnPercent)
                return true;

            if (!SubclassedCounter.ContainsKey(subclassInfo))
                return true;

            return SubclassedCounter[subclassInfo] < subclassInfo.MaxPlayers;
        }

        private static void BroadcastRole(Player player, string roleName, string className, string classDescription) =>
            player.Broadcast(15, string.Format(SubclassMod.Instance.Translation.SpawnDescriptionInfo, roleName, className, classDescription));
    }
}