using System.ComponentModel.DataAnnotations.Schema;

namespace Smapple.Models;

public class GameUser
{
    public int Id { get; set; }
    
    public int GameId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedDate { get; set; }
    
    public GameUserStatusEnum Status { get; set; }

    public Game Game { get; set; }
    
    public User User { get; set; }
}