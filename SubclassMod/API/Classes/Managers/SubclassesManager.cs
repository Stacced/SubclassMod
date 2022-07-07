using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MEC;
using SubclassMod.API.Enums;
using SubclassMod.Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SubclassMod.API.Classes.Managers
{
    public static class SubclassesManager   
    {
        public static void AssignPlayer(Player player, RoleType role)
        {
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

            player.DisplayNickname = $"{NicknamesManager.GetRoleName(player)} [{player.Nickname}]";
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
            yield return Timing.WaitForSeconds(1.5f);
            
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

            player.DisplayNickname = $"{NicknamesManager.GetRoleName(player, subclassInfo)} [{player.Nickname}]";
            player.CustomInfo = subclassInfo.CustomInfo;

            BroadcastRole(player, player.DisplayNickname, subclassInfo.Name, subclassInfo.Description);
        }

        private static IEnumerator<float> ForceAsOverriddenRole(Player player, RoleInfo roleInfo)
        {
            yield return Timing.WaitForSeconds(1f);
            
            SubclassedPlayer subclassPlayer = player.GameObject.AddComponent<SubclassedPlayer>();
            subclassPlayer.ActiveRole = roleInfo;

            player.DisplayNickname = $"{NicknamesManager.GetRoleName(player, roleInfo)} [{player.Nickname}]";
            player.CustomInfo = roleInfo.CustomInfo;

            if (roleInfo.InventoryOverridden)
            {
                player.ClearInventory();
                
                foreach (ItemType item in roleInfo.InventoryOverwrite)
                    player.AddItem(item);
            }
        }

        private static bool IsSubclassFree(SubclassInfo subclassInfo)
        {
            if (subclassInfo.MaxPlayers.Equals(0))
                return true;

            if (Random.Range(0, 100) <= subclassInfo.SpawnPercent)
                return true;
            
            List<SubclassedPlayer> subclassedPlayers = new List<SubclassedPlayer>();
            
            foreach (Player target in Player.List)
                if (target.GameObject.TryGetComponent(out SubclassedPlayer subclassedPlayerComponent))
                    subclassedPlayers.Add(subclassedPlayerComponent);

            return subclassedPlayers.Count(x => x.ActiveSubclass.Equals(subclassInfo)) >= subclassInfo.MaxPlayers;
        }

        private static void BroadcastRole(Player player, string roleName, string className, string classDescription) =>
            player.Broadcast(15, string.Format(SubclassMod.Instance.Translation.SpawnDescriptionInfo, roleName, className, classDescription));
    }
}