using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.Repository.DemoCrosshairCode.Exceptions
{
    public class InvalidShareCodeException : Exception
    {
        public InvalidShareCodeException() : base("Invalid share code") { }
    }
}
