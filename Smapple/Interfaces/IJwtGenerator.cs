using Smapple.Models;

namespace Smapple.Interfaces;

public interface IJwtGenerator
{
    string CreateToken(User user);
}