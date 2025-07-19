using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4RTools.Utils
{
    public class GameProcessInfo
    {
        public string ProcessText { get; set; }
        public string CharacterName { get; set; }
        public string CurrentMap { get; set; }

        public GameProcessInfo(string processText, string characterName, string currentMap)
        {
            ProcessText = processText;
            CharacterName = characterName;
            CurrentMap = currentMap;
        }

        public override string ToString()
        {
            return ProcessText;
        }
    }
}