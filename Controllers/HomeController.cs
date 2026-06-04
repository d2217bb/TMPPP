using Microsoft.AspNetCore.Mvc;

namespace TMPP.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
