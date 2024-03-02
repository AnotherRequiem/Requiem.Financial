using Requiem.PetProject.Models;

namespace Requiem.PetProject.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser appUser);
}