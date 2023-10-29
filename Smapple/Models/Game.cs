using System.ComponentModel.DataAnnotations.Schema;

namespace Smapple.Models;

public class Game
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int HostId { get; set; }
    
    public GameStatusEnum Status { get; set; }

    public int SlotsCount { get; set; }

    public string Name { get; set; }
    
    public DateTime GameDateTime { get; set; }
    
    public GameTypeEnum Type { get; set; }

    public string Image { get; set; }
    
    public string Address { get; set; }
    
    public User Host { get; set; }
    
    public List<GameUser> GameUsers { get; set; }
    public List<User> Users { get; set; }
}