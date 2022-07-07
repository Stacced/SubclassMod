﻿using System;
using Exiled.API.Features;

namespace SubclassMod.API.Classes.Managers
{
    public class NicknamesManager
    {
        public static string GetRoleName(Player player, SubclassInfo subclassInfo)
        {
            if (!player.IsHuman)
                return $"{subclassInfo.NamePrefix}{player.Nickname}{subclassInfo.NamePostfix}";
            
            if (subclassInfo.RoleplaySecondNameEnabled && subclassInfo.RoleplayNameEnabled)
                return $"{subclassInfo.NamePrefix}{SubclassMod.Instance.Config.HumanFirstNames.RandomItem()} {SubclassMod.Instance.Config.HumanSecondNames.RandomItem()}{subclassInfo.NamePostfix}";
            
            if (subclassInfo.RoleplayNameEnabled)
                return $"{subclassInfo.NamePrefix}{SubclassMod.Instance.Config.HumanFirstNames.RandomItem()}{subclassInfo.NamePostfix}";
            
            return $"{subclassInfo.NamePrefix}{player.Nickname}{subclassInfo.NamePostfix}";
        }
        
        public static string GetRoleName(Player player, RoleInfo roleInfo)
        {
            if (!player.IsHuman)
                return $"{roleInfo.Prefix}{player.DisplayNickname}{roleInfo.Postfix}";
            
            if (player.Role == RoleType.ClassD && SubclassMod.Instance.Config.ClassDNumbers)
                return $"{roleInfo.Prefix}{String.Format(SubclassMod.Instance.Translation.ClassDBadge, CalcNumericIdentify())}{roleInfo.Postfix}";
            
            return $"{roleInfo.Prefix}{SubclassMod.Instance.Config.HumanFirstNames.RandomItem()} {SubclassMod.Instance.Config.HumanSecondNames.RandomItem()}{roleInfo.Postfix}";
        }

        public static string GetRoleName(Player player)
        {
            if (!player.IsHuman)
                return player.Nickname;
            
            if (player.Role == RoleType.ClassD && SubclassMod.Instance.Config.ClassDNumbers)
                return $"{String.Format(SubclassMod.Instance.Translation.ClassDBadge, CalcNumericIdentify())}";
            
            return $"{SubclassMod.Instance.Config.HumanFirstNames.RandomItem()} {SubclassMod.Instance.Config.HumanSecondNames.RandomItem()}";
        }
        
        private static int CalcNumericIdentify()
        {
            // Removed due returns NRE
            /*int numIdentify;

            while (true)
            {
                numIdentify = UnityEngine.Random.Range(1000, 9999);

                if (Player.List.Count(x => x.CustomInfo.Contains(numIdentify.ToString())) == 0)
                    break;
            }*/

            return UnityEngine.Random.Range(1000, 9999);
        }
    }
}