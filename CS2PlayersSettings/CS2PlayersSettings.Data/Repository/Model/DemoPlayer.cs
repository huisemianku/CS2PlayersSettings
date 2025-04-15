using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayerSettings.Data.Repository.Models
{
    public class DemoPlayer
    {
        public string DemoPlayerName { get; set; } = "";
        public string DemoPlayerCrosshairCode { get; set; } = ""; 
        public string DemoPlayerSteamId { get; set; }
        public int DemoPlayerCrosshairFOV { get; set; } 
    }
}
