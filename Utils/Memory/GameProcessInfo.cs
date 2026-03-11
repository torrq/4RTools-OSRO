using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using _ORTools.Model;

namespace _ORTools.Utils
{
    public class GameProcessInfo
    {
        public string ProcessText { get; set; }
        public string CharacterName { get; set; }
        public string CurrentMap { get; set; }

        /// <summary>Already-opened Client built during list refresh — reused on selection to avoid a second OpenProcess.</summary>
        public Client CachedClient { get; set; }

        public GameProcessInfo(string processText, string characterName, string currentMap, Client cachedClient = null)
        {
            ProcessText = processText;
            CharacterName = characterName;
            CurrentMap = currentMap;
            CachedClient = cachedClient;
        }

        public override string ToString()
        {
            return ProcessText;
        }
    }
}