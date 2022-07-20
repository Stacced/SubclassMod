using System;
using System.Linq;
using Exiled.API.Features;
using SubclassMod.API.Enums;
using SubclassMod.API.Interfaces;

namespace SubclassMod.API.Classes.Managers
{
    public static class NicknamesManager
    {
        public static string GetRoleName(Player player, INamingData namingData = null)
        {
            if (player.Role == RoleType.ClassD && SubclassMod.Instance.Config.ClassDNumbers)
            {
                if (namingData == null)
                    return String.Format(SubclassMod.Instance.Translation.ClassDBadge, CalcNumericIdentify());

                return $"{namingData.NamePrefix}{String.Format(SubclassMod.Instance.Translation.ClassDBadge, CalcNumericIdentify())}{namingData.NamePostfix}";
            }

            if (namingData == null)
                return player.Nickname;
            
            switch (namingData.NamingMethod)
            {
                case NamingMethod.Firstname:
                    return $"{namingData.NamePrefix}{SubclassMod.Instance.Config.HumanFirstNames.RandomItem()}{namingData.NamePostfix} [{player.Nickname}]";
                case NamingMethod.Signs:
                    return $"{namingData.NamePrefix}{SubclassMod.Instance.Config.HumanSpecialSigns.RandomItem()}{namingData.NamePostfix} [{player.Nickname}]";
                default:
                    return player.Nickname;
            }
        }
        
        private static int CalcNumericIdentify()
        {
            int numIdentify;

            Player[] displayNamedPlayers = Player.List.Where(x => x.DisplayNickname != null).ToArray();

            do
            {
                numIdentify = UnityEngine.Random.Range(1000, 9999);
            } while (displayNamedPlayers.Count(x => x.DisplayNickname.Contains(numIdentify.ToString())) != 0);

            return numIdentify;
        }
    }
}