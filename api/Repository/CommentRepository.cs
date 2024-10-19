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
        public async Task<Comment> CreateAsync(Comment commentModel) {
            await _applicationDBContext.Comments.AddAsync(commentModel);
            await _applicationDBContext.SaveChangesAsync();
            return commentModel;
        }
        public async Task<List<Comment>> GetAllAsync() {
            return await _applicationDBContext.Comments.ToListAsync();
        }
        public Task<Comment?> GetByIdAsync(int id) {
            return null;
        }
        public async Task<Comment?> UpdateAsync(int id, Comment commentModel) {
            var existingComment = await _applicationDBContext.Comments.FindAsync(id);
            if(existingComment == null) {
                return null;
            }
            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;
            await _applicationDBContext.SaveChangesAsync();
            return existingComment;
        }
    }
}