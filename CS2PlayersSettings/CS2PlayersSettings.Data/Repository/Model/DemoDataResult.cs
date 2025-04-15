using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayerSettings.Data.Repository.Models
{
    public class DemoDataResult
    {
        public string PlayerPing { get; set; } = "";
        public string PlayerName { get; set; } = "";
        public int PlayerKills { get; set; } 
        public int PlayerDeaths { get; set; }
        public int PlayerAssists { get; set; }
        public string PlayerHs { get; set; } = "";
        public int PlayerDamage { get; set; } 

        public int PlayerTripleKills { get; set; } 
        public int PlayerQuadroKills { get; set; } 
        public int PlayerPentaKills { get; set; } 
        public int Score { get; set; } 
    }
}
