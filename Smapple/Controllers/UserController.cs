using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smapple.DbContext;

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