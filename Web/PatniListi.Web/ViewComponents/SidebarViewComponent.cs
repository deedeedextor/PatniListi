namespace PatniListi.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return this.View();
        }
    }
}
