using System.Collections.Generic;
using System.Windows.Forms;

namespace BruteGamingMacros.Core.Model
{
    internal class BuffContainer
    {
        public GroupBox Container { get; set; }
        public List<Buff> Skills { get; set; }

        public BuffContainer(GroupBox p, List<Buff> skills)
        {
            this.Skills = skills;
            this.Container = p;
        }
    }
}
