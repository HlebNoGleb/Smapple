using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smapple.DbContext;
using Smapple.Extensions;
using Smapple.Models;

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
                .ThenInclude(x => x.GameUsers)
                .ThenInclude(x => x.User)
                .Include(x => x.Games)
                .SingleAsync(x => x.Id == userId);

            foreach (var hostedGame in user.HostedGames)
            {
                foreach (var hostedGameUser in hostedGame.GameUsers)
                {
                    hostedGameUser.User.Password = string.Empty;
                }
            }
            
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
            var user = await _db.Users.Include(x => x.Games).SingleAsync(x => x.Id == id);

            user.Password = string.Empty;

            return Json(user);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<ActionResult> UsersList()
    {
        var  users = _db.Users.Where(x => x.Role != RoleEnum.Admin).ToList();

        foreach (var user in users)
        {
            user.Password = string.Empty;
        }

        return Json(users);
    }
}