using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.Repository.Model
{
    public class ExcelUpsertResult
    {
        public int InsertedCount { get; set; }
        public int UpdatedCount { get; set; }
        public string UpdateLog { get; set; } = ""!;
    }
}
