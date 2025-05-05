using CS2PlayersSettings.Business.WebApi;
using CS2PlayersSettings.Data.Repository.Entities.Navbars;
using Microsoft.AspNetCore.Mvc;

namespace CS2PlayersSettings.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NavbarController : Controller
    {
        private readonly NavbarWebApiBLL _navbarWebApiBLL;
        public NavbarController(NavbarWebApiBLL navbarWebApiBLL)
        {
            _navbarWebApiBLL = navbarWebApiBLL;
        }

        [HttpGet]
        [Route("GetNavBarItems")]
        public async Task<List<NavigationItem>> GetNavBarItems()
        {
            return await _navbarWebApiBLL.GetNavigationItemsAsync();
        }
    }
}
