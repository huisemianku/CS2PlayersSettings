using CS2PlayersSettings.Data.Repository.Entities;
using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Business.WebApi
{
    public class NavbarWebApiBLL
    {
        private readonly NavbarWebApiDAL _navbarWebApiDAL;

        public NavbarWebApiBLL(NavbarWebApiDAL navbarWebApiDAL)
        {
            _navbarWebApiDAL = navbarWebApiDAL;
        }

        #region 获取Navbar items
        public async Task<List<NavigationItem>> GetNavigationItemsAsync()
        {
            return await _navbarWebApiDAL.GetNavigationItemsAsync();
        }
        #endregion

    }
}
