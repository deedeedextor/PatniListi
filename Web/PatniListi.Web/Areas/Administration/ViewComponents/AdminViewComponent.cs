﻿namespace PatniListi.Web.Areas.Administration.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class AdminViewComponent : ViewComponent
    {
        public AdminViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            return this.View();
        }
    }
}
