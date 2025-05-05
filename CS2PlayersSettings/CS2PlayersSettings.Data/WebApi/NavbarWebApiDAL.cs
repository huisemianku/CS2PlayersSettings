using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.Repository.Entities.Navbars;
using Microsoft.EntityFrameworkCore;

namespace CS2PlayersSettings.Data.WebApi
{
    public class NavbarWebApiDAL
    {
        private readonly NavbarDbContext _navbarDbContext;
        public NavbarWebApiDAL(NavbarDbContext navbarDbContext)
        {
            _navbarDbContext = navbarDbContext;
        }

        #region 获取Navbar items
        public async Task<List<NavigationItem>> GetNavigationItemsAsync()
        {
            try
            {
                var NavbarItems = await _navbarDbContext.NavigationItems.ToListAsync();
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
