using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.Repository.DemoCrosshairCode.Constants
{
    public static class Constants
    {
        public const string DICTIONARY = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefhijkmnopqrstuvwxyz23456789";
        public static readonly BigInteger DICTIONARY_LENGTH = DICTIONARY.Length;
        public static readonly string SHARECODE_PATTERN = @"^CSGO(-?[\w]{5}){5}$";
    }
}
