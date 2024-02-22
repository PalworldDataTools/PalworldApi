using Microsoft.AspNetCore.Mvc;

namespace PalworldApi.Controllers;

/// <summary>
///     Serve basic pages of the application
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    ///     Get index page
    /// </summary>
    public IActionResult Index() => View();

    /// <summary>
    ///     Get error page
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}
