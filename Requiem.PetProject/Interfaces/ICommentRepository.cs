using Requiem.PetProject.Models;

namespace Requiem.PetProject.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
}