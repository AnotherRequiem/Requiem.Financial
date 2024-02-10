using Microsoft.EntityFrameworkCore;
using Requiem.PetProject.Data;
using Requiem.PetProject.Interfaces;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;
    
    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }
    
    
}