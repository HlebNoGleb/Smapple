using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smapple.DbContext;
using Smapple.Extensions;
using Smapple.Interfaces;
using Smapple.Models;
using Smapple.Services;

namespace Smapple.Controllers;

public class AuthController : Controller
{
    private readonly IJwtGenerator _jwtGenerator;
    private readonly SmappleDbContext _dbContext;
    private readonly VerifyingUserService _verifyingUserService;

    public AuthController(SmappleDbContext dbContext, IJwtGenerator jwtGenerator, VerifyingUserService verifyingUserService)
    {
        this._dbContext = dbContext;
        this._jwtGenerator = jwtGenerator;
        _verifyingUserService = verifyingUserService;
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
                    x.Password == login.Password.GetHash());

            var token = _jwtGenerator.CreateToken(user);

            if (!user.IsVerified)
            {
                return Forbid();
            }
            
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
        user.Password = user.Password.GetHash();
        
        var added = _dbContext.Users.Add(user);
        var savedNum = await _dbContext.SaveChangesAsync();

        if (savedNum == 0)
        {
            return BadRequest();
        }

        await _verifyingUserService.SendVerificationMail(added.Entity);
        
        return Ok();
    }

    [HttpGet]
    [Route("api/verify")]
    public async Task<ActionResult> Verify([FromQuery] string code)
    {
        try
        {
            var userId = _verifyingUserService.GetUserIdByCode(code);

            if (userId == null)
            {
                return BadRequest();
            }

            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId);

            user.IsVerified = true;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}