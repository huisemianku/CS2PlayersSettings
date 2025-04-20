using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data
{
    public class NavbarDAL
    {
        private readonly PlayerDbContext _playerDbContext;
        public NavbarDAL(PlayerDbContext playerDbContext)
        {
            _playerDbContext = playerDbContext;
        }

        #region 获取所有导航栏
        public async Task<List<NavigationItem>> GetAllNavbarItemsAsync()
        {
            try
            {
                var items = await _playerDbContext.NavigationItems.ToListAsync();
                return items;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
