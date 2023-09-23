namespace Carq.Ops.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    //[ClaimRequired(ClaimTypes.Role, Roles.CarqUser)]
    public class SpaController : Controller
    {
        [HttpGet("/")]
        [ResponseCache(Duration = 360000)]
        public IActionResult Board() => View();

    }
}