using CS2PlayersSettings.Data.Repository.Entities.Navbars;
using CS2PlayersSettings.Data;

namespace CS2PlayersSettings.Business
{
    public class NavbarBLL
    {
        private readonly NavbarDAL _navbarDAL;
        public NavbarBLL(NavbarDAL navbarDAL)
        {
            _navbarDAL = navbarDAL;
        }

        #region 获取所有导航栏
        public async Task<List<NavigationItem>> GetAllNavbarItemsAsync()
        {
            return await _navbarDAL.GetAllNavbarItemsAsync();
        }
        #endregion
    }
}
