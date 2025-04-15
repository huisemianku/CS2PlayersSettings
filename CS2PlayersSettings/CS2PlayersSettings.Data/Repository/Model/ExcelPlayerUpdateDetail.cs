using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.Repository.Model
{
    public class ExcelPlayerUpdateDetail
    {
        public string PlayerNickName { get; set; } = ""!;
        public List<string> UpdatedFields { get; set; } = new List<string>();
        public DateTime UpdateTime { get; set; }
    }
}
