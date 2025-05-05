using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.Repository.Entities.Navbars;
using Microsoft.EntityFrameworkCore;

namespace CS2PlayersSettings.Data
{
    public class NavbarDAL
    {
        private readonly NavbarDbContext _navbarDbContext;
        public NavbarDAL(NavbarDbContext navbarDbContext)
        {
            _navbarDbContext = navbarDbContext;
        }

        #region 获取所有导航栏
        public async Task<List<NavigationItem>> GetAllNavbarItemsAsync()
        {
            try
            {
                var items = await _navbarDbContext.NavigationItems.ToListAsync();
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
