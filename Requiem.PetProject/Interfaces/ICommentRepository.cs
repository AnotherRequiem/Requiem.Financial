﻿using Requiem.PetProject.Helpers;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync(CommentQueryObject commentQuery);

    Task<Comment?> GetByIdAsync(Guid id);

    Task<Comment> CreateAsync(Comment commentModel);
    
    Task<Comment?> UpdateAsync(Guid id, Comment commentModel);

    Task<Comment?> DeleteAsync(Guid id);
}