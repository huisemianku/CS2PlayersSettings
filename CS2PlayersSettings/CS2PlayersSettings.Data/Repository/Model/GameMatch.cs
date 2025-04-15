using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.Repository.Model
{
    public class GameMatch
    {
        public string? MapName { get; set; }
        public string? HostName { get; set; }
        public string? Test { get; set; }
        public string? TerroristName { get; set; }
        public string? TerroristLogo { get; set; }
        public int TerroristScore { get; set; }
        public string? CounterTerroristName { get; set; }
        public string? CounterTerroristLogo { get; set; }
        public int CounterTerroristScore { get; set; }
    }
}
