using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smapple.DbContext;
using Smapple.Extensions;

namespace Smapple.Controllers;

[Route("api/user")]
public class UserController : Controller
{
    private readonly SmappleDbContext _db;

    public UserController(SmappleDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [Route("me")]
    [Authorize]
    public async Task<ActionResult> Me()
    {
        try
        {
            var userId = User.Id();

            var user = await _db.Users
                .Include(x => x.HostedGames)
                .Include(x => x.Games)
                .Include(x => x.GameUsers)
                .SingleAsync(x => x.Id == userId);

            return Json(user);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetUser(int id)
    {
        try
        {
            var user = await _db.Users.SingleAsync(x => x.Id == id);

            user.Password = string.Empty;

            return Json(user);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }
}