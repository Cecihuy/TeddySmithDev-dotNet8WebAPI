using System;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository {
    public class CommentRepository : ICommentRepository {
        private readonly ApplicationDBContext _applicationDBContext;
        public CommentRepository (ApplicationDBContext applicationDBContext) {
            _applicationDBContext = applicationDBContext;
        }
        public async Task<List<Comment>> GetAllAsync() {
            return await _applicationDBContext.Comments.ToListAsync();
        }
        public Task<Comment?> GetByIdAsync(int id)
        {
            return null;
        }
    }
}