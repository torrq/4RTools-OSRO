using System;
using System.Collections.Generic;
using System.Windows.Forms;
using _4RTools.Utils;
using System.Drawing;
using System.Windows.Input;

namespace _4RTools.Model
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
