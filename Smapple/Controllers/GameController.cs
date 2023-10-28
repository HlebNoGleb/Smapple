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
            var game = _db.Games.First(x => x.Id == id);
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

    private async Task ChangeGameStatus(int id, int hostId, GameStatusEnum gameStatus)
    {
        var game = await _db.Games.SingleAsync(x => x.Id == id && x.HostId == hostId);

        game.Status = gameStatus;

        _db.Games.Update(game);

        await _db.SaveChangesAsync();
    }
}