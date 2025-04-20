using CS2PlayersSettings.Business;
using CS2PlayersSettings.Data.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CS2PlayersSettings.Pages.Navbar
{
    public class IndexModel : PageModel
    {
        private readonly NavbarBLL _navbarBLL;
        public IndexModel(NavbarBLL navbarBLL)
        {
            _navbarBLL = navbarBLL;
        }
        public List<NavigationItem> NavigationItems { get; set; }

        public async Task OnGet()
        {
            NavigationItems = await _navbarBLL.GetAllNavbarItemsAsync();
        }
    }
}
