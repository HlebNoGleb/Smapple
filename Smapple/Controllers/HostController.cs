using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smapple.DbContext;
using Smapple.Models;

namespace Smapple.Controllers;

[Route("api/host")]
public class HostController : Controller
{
    private readonly SmappleDbContext _db;

    public HostController(SmappleDbContext db)
    {
        _db = db;
    }

    // [HttpGet]
    // [Authorize]
    // public async Task<ActionResult> GetHosts()
    // {
    // }
}