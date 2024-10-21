using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoomGame.Game
{
    public class Variables
    {
        public int Moolah { get; set; } = 50;
        public int Stress { get; set; } = 0;
        public int Status { get; set; }
        public int DaysWorked { get; set; }
        public List<string> Inventory = new List<string>();
        public Dictionary<string, Action> ItemBuffs = new Dictionary<string, Action>();
    }
}
