using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.WebApi
{
    public class NavbarWebApiDAL
    {
        private readonly PlayerDbContext _playerDbContext;
        public NavbarWebApiDAL(PlayerDbContext playerDbContext)
        {
            _playerDbContext = playerDbContext;
        }

        #region 获取Navbar items
        public async Task<List<NavigationItem>> GetNavigationItemsAsync()
        {
            try
            {
                var NavbarItems = await _playerDbContext.NavigationItems.ToListAsync();
                return NavbarItems;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
