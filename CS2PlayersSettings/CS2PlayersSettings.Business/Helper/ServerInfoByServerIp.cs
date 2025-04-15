using MaxMind.GeoIP2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Business.Helper
{
    public class ServerInfoByServerIp
    {
        void GetServerInfo(string serverIp)
        {
            try
            {
                // 使用 IPNetwork2 验证 IP
                var ipnetwork = IPNetwork.Parse(serverIp);

                // 使用 GeoLite2 获取地理信息
                using (var reader = new DatabaseReader("GeoLite2-City.mmdb"))
                {
                    var response = reader.City(serverIp);
                    Console.WriteLine($"IP: {serverIp}");
                    Console.WriteLine($"国家: {response.Country.Name}");
                    var i = response.Subdivisions;
                    foreach (var item in i)
                    {
                       var s =  item.Name;
                    }
                    Console.WriteLine($"州/地区: {response.Subdivisions}");
                    Console.WriteLine($"城市: {response.City.Name}");
                    Console.WriteLine($"经度: {response.Location.Longitude}");
                    Console.WriteLine($"纬度: {response.Location.Latitude}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
            }

        }
    }
}
