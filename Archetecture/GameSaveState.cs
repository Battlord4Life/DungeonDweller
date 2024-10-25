using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace DungeonDweller.Archetecture
{
    public class GameSaveState
    { 

        public int Level { get; set; } = 0;
        public int Checkpoint { get; set; } = 0;

        public bool HasFist { get; set; } = true;
        public bool HasShout { get; set; } = true;
        public bool HasFlash { get; set; } = true;
        public bool HasLantern { get; set; } = false;
        public bool HasCamera { get; set; } = false;
        public bool HasNightVision { get; set; } = false;

        public int Kills { get; set; } = 0;
        public int MaxHealth { get; set; } = 5;
        public int CurrHealth { get; set; } = 5;
    }
}
