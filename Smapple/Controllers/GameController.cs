using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smapple.DbContext;
using Smapple.Extensions;
using Smapple.Models;

namespace Smapple.Controllers;

[Route("api/Game")]
public class GameController : Controller
{
    private readonly SmappleDbContext _db;

    public GameController(SmappleDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateGame([FromBody] Game game)
    {
        var userId = User.Id();

        game.HostId = userId;
        
        var saved = await _db.Games.AddAsync(game);
        await _db.SaveChangesAsync();

        return Json(saved.Entity);
    }

    [HttpGet]
    public async Task<ActionResult> ListGames()
    {
        var games = _db.Games
            .Include(x => x.Host)
            .Include(x => x.Users)
            .OrderByDescending(x => x.Id).ToList();

        foreach (var game in games)
        {
            foreach (var user in game.Users)
            {
                user.Password = string.Empty;
            }
            game.Host.Password = string.Empty;
        }

        return Json(games);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize]
    public async Task<ActionResult> UpdateGame([FromBody] Game game, int id)
    {
        if (game.HostId != User.Id())
        {
            return Forbid();
        }

        game.Id = id;
        
        var updated = _db.Update(game);
        await _db.SaveChangesAsync();

        return Json(updated.Entity);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetGame(int id)
    {
        try
        {
            var game = _db.Games
                .Include(x => x.GameUsers)
                .ThenInclude(x => x.User)
                .Include(x => x.Host)
                .First(x => x.Id == id);

            game.Host.Password = string.Empty;
            foreach (var gameUser in game.GameUsers)
            {
                gameUser.User.Password = string.Empty;
            }
            
            return Json(game);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Authorize]
    [Route("{id}/start")]
    public async Task<ActionResult> StartGame(int id)
    {
        try
        {
            var hostId = User.Id();

            await ChangeGameStatus(id, hostId, GameStatusEnum.InProgress);

            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }
    
    [HttpPost]
    [Authorize]
    [Route("{id}/counting")]
    public async Task<ActionResult> CountringGameResults(int id)
    {
        try
        {
            var hostId = User.Id();

            await ChangeGameStatus(id, hostId, GameStatusEnum.CountingResults);

            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }
    
    [HttpPost]
    [Authorize]
    [Route("{id}/close")]
    public async Task<ActionResult> CloseGame(int id)
    {
        try
        {
            var hostId = User.Id();

            await ChangeGameStatus(id, hostId, GameStatusEnum.Closed);

            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Authorize]
    [Route("{id}/addPoints")]
    public async Task<ActionResult> AddPoints([FromBody] int points, int id)
    {
        try
        {
            var userId = User.Id();

            var gameUserData = await _db.GameUsers
                .Where(x => x.GameId == id && x.UserId == userId && x.Status == GameUserStatusEnum.Approved)
                .FirstAsync();

            gameUserData.UserScore = points;

            _db.Update(gameUserData);
            await _db.SaveChangesAsync();

            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Authorize]
    [Route("{gameId}/submitUser/{userId}")]
    public async Task<ActionResult> SubmitUser(int gameId, int userId)
    {
        try
        {
            return await ChangeGameUserStatus(gameId, userId, GameUserStatusEnum.Approved);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Authorize]
    [Route("{gameId}/declineUser/{userId}")]
    public async Task<ActionResult> DeclineUser(int gameId, int userId)
    {
        try
        {
            return await ChangeGameUserStatus(gameId, userId, GameUserStatusEnum.Declined);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Authorize]
    [Route("{gameId}/createApplication")]
    public async Task<ActionResult> CreateApplication(int gameId)
    {
        try
        {
            var userId = User.Id();

            var game = await _db.Games.SingleAsync(x => x.Id == gameId);
            
            _db.GameUsers.Add(new GameUser()
            {
                UserId = userId,
                GameId = gameId,
                Status = game.Type == GameTypeEnum.Private 
                    ? GameUserStatusEnum.Pending
                    : GameUserStatusEnum.Approved,
                CreatedDate = DateTime.Now
            });

            await _db.SaveChangesAsync();

            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [Authorize]
    [Route("{gameId}/removeUser/{userId}")]
    public async Task<ActionResult> RemoveUserFromGame(int gameId, int userId)
    {
        try
        {
            var game = await _db.Games
                .Include(x => x.GameUsers.Where(x => x.UserId == userId && x.Status == GameUserStatusEnum.Pending))
                .Where(x => x.Id == gameId)
                .SingleAsync();

            if (game.HostId != User.Id())
            {
                return Forbid();
            }

            if (!game.GameUsers.Any())
            {
                return BadRequest();
            }

            var gameUser = game.GameUsers.First(x => x.UserId == userId);

            _db.GameUsers.Remove(gameUser);
            await _db.SaveChangesAsync();

            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest();
        }
    }

    private async Task<ActionResult> ChangeGameUserStatus(int gameId, int userId, GameUserStatusEnum status)
    {
        var game = await _db.Games
            .Include(x => x.GameUsers.Where(x => x.UserId == userId && x.Status == GameUserStatusEnum.Pending))
            .Where(x => x.Id == gameId)
            .SingleAsync();

        if (game.HostId != User.Id())
        {
            return Forbid();
        }

        if (!game.GameUsers.Any())
        {
            return BadRequest();
        }

        var gameUser = game.GameUsers.First(x => x.UserId == userId);

        gameUser.Status = status;

        _db.GameUsers.Update(gameUser);
        await _db.SaveChangesAsync();

        return Ok();
    }

    private async Task ChangeGameStatus(int id, int hostId, GameStatusEnum gameStatus)
    {
        var game = await _db.Games.SingleAsync(x => x.Id == id && x.HostId == hostId);

        game.Status = gameStatus;

        _db.Games.Update(game);

        await _db.SaveChangesAsync();
    }
}