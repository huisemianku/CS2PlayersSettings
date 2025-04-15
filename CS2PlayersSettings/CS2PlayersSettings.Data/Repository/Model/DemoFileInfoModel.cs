using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.Repository.Model
{
    public class DemoFileInfoModel
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
    }
}
