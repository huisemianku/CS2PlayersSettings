
using CS2PlayersSettings.Data.Repository.DemoCrosshairCode.Constants;
using CS2PlayersSettings.Data.Repository.DemoCrosshairCode.Exceptions;
using CS2PlayersSettings.Data.Repository.DemoCrosshairCode.Structs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.Repository.DemoCrosshairCode
{
    public static class CS2ShareCodeConverter
    {
        private static string BytesToHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        private static BigInteger BytesToBigInteger(byte[] bytes)
        {
            return BigInteger.Parse("0" + BytesToHex(bytes), NumberStyles.HexNumber);
        }

        private static byte[] StringToBytes(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("Input string cannot be null or empty.", nameof(str));
            }

            // 格式化长度为 6 位
            if (str.Length < 36)
            {
                str = str.PadLeft(36, '0'); // 少于 36 位，左侧填充 0
            }
            else if (str.Length > 36)
            {
                str = str.Substring(str.Length - 36, 36); // 多于 36 位，取最后 36 位
            }

            // 确保长度是偶数36位
            if (str.Length % 2 != 0)
            {
                throw new ArgumentException("Input string must have an even length for byte conversion.", nameof(str));
            }

            var bytes = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
            {
                bytes.Add(Convert.ToByte(str.Substring(i, 2), 16));
            }
            return bytes.ToArray();
        }

        private static byte[] Int16ToBytes(int number)
        {
            return BitConverter.GetBytes((short)number).Reverse().ToArray();
        }

        private static sbyte Uint8ToInt8(byte number)
        {
            return (sbyte)number;
        }

        private static int SumArray(byte[] array)
        {
            return array.Sum(b => b);
        }

        private static byte[] ShareCodeToBytes(string shareCode)
        {
            if (!Regex.IsMatch(shareCode, Constants.Constants.SHARECODE_PATTERN))
            {
                throw new InvalidShareCodeException();
            }

            shareCode = shareCode.Replace("CSGO", "").Replace("-", "");
            var chars = shareCode.Reverse().ToArray();
            BigInteger big = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                big = big * Constants.Constants.DICTIONARY_LENGTH + Constants.Constants.DICTIONARY.IndexOf(chars[i]);
            }

            var str = big.ToString("x").PadLeft(36, '0');
            return StringToBytes(str);
        }

        private static string BytesToShareCode(byte[] bytes)
        {
            var hex = BytesToHex(bytes);
            BigInteger total = BigInteger.Parse("0" + hex, NumberStyles.HexNumber);
            var chars = new StringBuilder();
            BigInteger rem;
            for (int i = 0; i < 25; i++)
            {
                rem = total % Constants.Constants.DICTIONARY_LENGTH;
                chars.Append(Constants.Constants.DICTIONARY[(int)rem]);
                total /= Constants.Constants.DICTIONARY_LENGTH;
            }

            var code = chars.ToString();
            return $"CSGO-{code.Substring(0, 5)}-{code.Substring(5, 5)}-{code.Substring(10, 5)}-{code.Substring(15, 5)}-{code.Substring(20, 5)}";
        }

        //public static string EncodeMatch(MatchInformation match)
        //{
        //    var matchBytes = StringToBytes(match.MatchId.ToString("x")).Reverse().ToArray();
        //    var reservationBytes = StringToBytes(match.ReservationId.ToString("x")).Reverse().ToArray();
        //    var tvBytes = Int16ToBytes(match.TvPort).Reverse().ToArray();
        //    var bytes = matchBytes.Concat(reservationBytes).Concat(tvBytes).ToArray();
        //    return BytesToShareCode(bytes);
        //}
        public static string EncodeMatch(MatchInformation match)
        {
            var matchHex = match.MatchId.ToString("x").PadLeft(16, '0'); // 确保长度为偶数
            var reservationHex = match.ReservationId.ToString("x").PadLeft(16, '0'); // 确保长度为偶数
            var matchBytes = StringToBytes(matchHex).Reverse().ToArray();
            var reservationBytes = StringToBytes(reservationHex).Reverse().ToArray();
            var tvBytes = Int16ToBytes(match.TvPort).Reverse().ToArray();
            var bytes = matchBytes.Concat(reservationBytes).Concat(tvBytes).ToArray();
            return BytesToShareCode(bytes);
        }

        public static MatchInformation DecodeMatchShareCode(string shareCode)
        {
            var bytes = ShareCodeToBytes(shareCode);
            return new MatchInformation
            {
                MatchId = BytesToBigInteger(bytes.Take(8).Reverse().ToArray()),
                ReservationId = BytesToBigInteger(bytes.Skip(8).Take(8).Reverse().ToArray()),
                TvPort = (int)BytesToBigInteger(bytes.Skip(16).Take(2).Reverse().ToArray())
            };
        }

        public static Crosshair DecodeCrosshairShareCode(string shareCode)
        {
            var bytes = ShareCodeToBytes(shareCode);
            var size = SumArray(bytes.Skip(1).ToArray()) % 256;

            if (bytes[0] != size)
            {
                throw new InvalidCrosshairShareCodeException();
            }

            return new Crosshair
            {
                cl_crosshairgap = Uint8ToInt8(bytes[2]) / 10.0,
                cl_crosshair_outlinethickness = bytes[3] / 2.0,
                cl_crosshaircolor_r = bytes[4],
                cl_crosshaircolor_g = bytes[5],
                cl_crosshaircolor_b = bytes[6],
                cl_crosshairalpha = bytes[7],
                cl_crosshair_dynamic_splitdis = bytes[8] & 7,
                cl_crosshair_recoil = ((bytes[8] >> 4) & 8) == 8,
                cl_fixedcrosshairgap = Uint8ToInt8(bytes[9]) / 10.0,
                cl_crosshaircolor = bytes[10] & 7,
                cl_crosshair_drawoutline = (bytes[10] & 8) == 8,
                cl_crosshair_dynamic_splitalpha_innermod = (bytes[10] >> 4) / 10.0,
                cl_crosshair_dynamic_splitalpha_outermod = (bytes[11] & 0xf) / 10.0,
                cl_crosshair_dynamic_maxdist_splitratio = (bytes[11] >> 4) / 10.0,
                cl_crosshairthickness = bytes[12] / 10.0,
                cl_crosshairdot = ((bytes[13] >> 4) & 1) == 1,
                cl_crosshairgap_useweaponvalue = ((bytes[13] >> 4) & 2) == 2,
                cl_crosshairusealpha = ((bytes[13] >> 4) & 4) == 4,
                cl_crosshair_t = ((bytes[13] >> 4) & 8) == 8,
                cl_crosshairstyle = (bytes[13] & 0xf) >> 1,
                cl_crosshairsize = bytes[14] / 10.0
            };
        }

        public static string EncodeCrosshair(Crosshair crosshair)
        {
            var bytes = new byte[]
            {
                0,
                1,
                (byte)(crosshair.cl_crosshairgap * 10),
                (byte)(crosshair.cl_crosshair_outlinethickness * 2),
                (byte)crosshair.cl_crosshaircolor_r,
                (byte)crosshair.cl_crosshaircolor_g,
                (byte)crosshair.cl_crosshaircolor_b,
                (byte)crosshair.cl_crosshairalpha,
                (byte)((crosshair.cl_crosshair_dynamic_splitdis & 7) | (Convert.ToInt32(crosshair.cl_crosshair_recoil) << 7)),
                (byte)(crosshair.cl_fixedcrosshairgap * 10),
                (byte)((crosshair.cl_crosshaircolor & 7) | (Convert.ToInt32(crosshair.cl_crosshair_drawoutline) << 3) | ((int)(crosshair.cl_crosshair_dynamic_splitalpha_innermod * 10) << 4)),
                (byte)((int)(crosshair.cl_crosshair_dynamic_splitalpha_outermod * 10) | ((int)(crosshair.cl_crosshair_dynamic_maxdist_splitratio * 10) << 4)),
                (byte)(crosshair.cl_crosshairthickness * 10),
                (byte)((crosshair.cl_crosshairstyle << 1) |
                       (Convert.ToInt32(crosshair.cl_crosshairdot) << 4) |
                       (Convert.ToInt32(crosshair.cl_crosshairgap_useweaponvalue) << 5) |
                       (Convert.ToInt32(crosshair.cl_crosshairusealpha) << 6) |
                       (Convert.ToInt32(crosshair.cl_crosshair_t) << 7)),
                (byte)(crosshair.cl_crosshairsize * 10),
                0,
                0,
                0
            };

            bytes[0] = (byte)(SumArray(bytes) & 0xff);
            return BytesToShareCode(bytes);
        }

        public static string CrosshairToConVars(Crosshair crosshair)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"cl_crosshair_drawoutline \"{Convert.ToInt32(crosshair.cl_crosshair_drawoutline)}\"");
            sb.AppendLine($"cl_crosshair_dynamic_maxdist_splitratio \"{crosshair.cl_crosshair_dynamic_maxdist_splitratio}\"");
            sb.AppendLine($"cl_crosshair_dynamic_splitalpha_innermod \"{crosshair.cl_crosshair_dynamic_splitalpha_innermod}\"");
            sb.AppendLine($"cl_crosshair_dynamic_splitalpha_outermod \"{crosshair.cl_crosshair_dynamic_splitalpha_outermod}\"");
            sb.AppendLine($"cl_crosshair_dynamic_splitdist \"{crosshair.cl_crosshair_dynamic_splitdis}\"");
            sb.AppendLine($"cl_crosshair_outlinethickness \"{crosshair.cl_crosshair_outlinethickness}\"");
            sb.AppendLine($"cl_crosshair_t \"{Convert.ToInt32(crosshair.cl_crosshair_t)}\"");
            sb.AppendLine($"cl_crosshairalpha \"{crosshair.cl_crosshairalpha}\"");
            sb.AppendLine($"cl_crosshaircolor \"{crosshair.cl_crosshaircolor}\"");
            sb.AppendLine($"cl_crosshaircolor_b \"{crosshair.cl_crosshaircolor_b}\"");
            sb.AppendLine($"cl_crosshaircolor_g \"{crosshair.cl_crosshaircolor_g}\"");
            sb.AppendLine($"cl_crosshaircolor_r \"{crosshair.cl_crosshaircolor_r}\"");
            sb.AppendLine($"cl_crosshairdot \"{Convert.ToInt32(crosshair.cl_crosshairdot)}\"");
            sb.AppendLine($"cl_crosshairgap \"{crosshair.cl_crosshairgap}\"");
            sb.AppendLine($"cl_crosshairgap_useweaponvalue \"{Convert.ToInt32(crosshair.cl_crosshairgap_useweaponvalue)}\"");
            sb.AppendLine($"cl_crosshairsize \"{crosshair.cl_crosshairsize}\"");
            sb.AppendLine($"cl_crosshairstyle \"{crosshair.cl_crosshairstyle}\"");
            sb.AppendLine($"cl_crosshairthickness \"{crosshair.cl_crosshairthickness}\"");
            sb.AppendLine($"cl_crosshairusealpha \"{Convert.ToInt32(crosshair.cl_crosshairusealpha)}\"");
            sb.AppendLine($"cl_fixedcrosshairgap \"{crosshair.cl_fixedcrosshairgap}\"");
            sb.AppendLine($"cl_crosshair_recoil \"{Convert.ToInt32(crosshair.cl_crosshair_recoil)}\"");
            return sb.ToString().TrimEnd();
        }
    }
}
