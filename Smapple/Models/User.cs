using System.ComponentModel.DataAnnotations.Schema;

namespace Smapple.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
    
    public string NickName { get; set; }
    
    public string? Description { get; set; }

    public bool IsVerified { get; set; }

    public int Points { get; set; }
    
    public RoleEnum Role { get; set; }
    
    public List<GameUser> GameUsers { get; set; }
    public List<Game> Games { get; set; }
    
    public List<Game> HostedGames { get; set; }
}