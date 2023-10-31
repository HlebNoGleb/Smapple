using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smapple.DbContext;
using Smapple.Interfaces;
using Smapple.Models;

namespace Smapple.Controllers;

public class AuthController : Controller
{
    private readonly IJwtGenerator _jwtGenerator;
    private readonly SmappleDbContext _dbContext;

    public AuthController(SmappleDbContext dbContext, IJwtGenerator jwtGenerator)
    {
        this._dbContext = dbContext;
        this._jwtGenerator = jwtGenerator;
    }
    
    [HttpPost]
    [Route("api/login")]
    public async Task<ActionResult> Login([FromBody]LoginQuery login)
    {
        try
        {
            var user = await _dbContext.Users.SingleAsync(
                x =>
                    x.Email == login.Email &&
                    x.Password == login.Password);

            var token = _jwtGenerator.CreateToken(user);
            
            return Json(new
            {
                user = user, token = token
            });
        }
        catch (Exception e)
        {
            return Unauthorized();
        }
    }

    [HttpPost]
    [Route("api/register")]
    public async Task<ActionResult> Register([FromBody] User user)
    {
        var existingUser = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == user.Email);

        if (existingUser != null)
        {
            return BadRequest();
        }
        
        user.Role = RoleEnum.Simple;
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}