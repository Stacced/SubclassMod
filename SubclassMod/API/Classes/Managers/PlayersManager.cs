using System.Linq;

using Exiled.API.Features;

namespace SubclassMod.API.Classes.Managers
{
    public static class PlayersManager
    {
        public static bool GetByMention(string mention, out Player[] players)
        {
            if (mention.Equals("*"))
            {
                players = Player.List.ToArray();
                return true; 
            }

            if (int.TryParse(mention, out int playerId))
            {
                players = new [] { Player.Get(playerId) };
                return true;
            }

            players = null;
            return false;
        }
    }
}