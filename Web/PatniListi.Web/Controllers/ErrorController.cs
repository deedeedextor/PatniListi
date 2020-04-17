namespace PatniListi.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [Route("Error")]
    public class ErrorController : BaseController
    {
        [Route("404")]
        public ActionResult NotFound()
        {
            return this.View();
        }
    }
}
