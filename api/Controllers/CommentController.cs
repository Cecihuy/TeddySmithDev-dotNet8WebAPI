using System;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers {
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository) {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var comments = await _commentRepository.GetAllAsync();
            var commentDto = comments.Select(func => func.ToCommentDto());
            return Ok(commentDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) {
            var comment = await _commentRepository.GetByIdAsync(id);
            if(comment == null) {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());

        }
        [HttpPost]
        [Route("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto createCommentDto) {
            if(!await _stockRepository.StockExists(stockId)) {
                return BadRequest("Stock does not exist");
            }
            var commentModel = createCommentDto.ToCommentFromCreate(stockId);
            await _commentRepository.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id}, commentModel.ToCommentDto() );
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateCommentRequestDto) {
            var comment = await _commentRepository.UpdateAsync(id, updateCommentRequestDto.ToCommentFromUpdate());
            if(comment == null) {
                return NotFound("Comment not found");
            }
            return Ok(comment.ToCommentDto());
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id) {
            var commentModel = await _commentRepository.DeleteAsync(id);
            if(commentModel == null) {
                return NotFound("comment does not exist");
            }
            return Ok(commentModel);
        }
    }
}