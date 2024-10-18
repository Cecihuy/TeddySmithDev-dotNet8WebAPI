using System;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase {
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly IStockRepository _stockRepository;
        public StockController(ApplicationDBContext applicationDBContext, IStockRepository stockRepository) {
            _stockRepository = stockRepository;
            _applicationDBContext = applicationDBContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var stocks = await _stockRepository.GetAllAsync();
            var stockDto = stocks.Select(fn => fn.ToStockDto());
            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) {
            var stock = await _applicationDBContext.Stocks.FindAsync(id);
            if(stock == null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto) {
            var stockModel = stockDto.toStockFromCreateDTO();
            await _applicationDBContext.Stocks.AddAsync(stockModel);
            await _applicationDBContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto ) {
            var stockModel = await _applicationDBContext.Stocks.FirstOrDefaultAsync(func => func.Id == id);
            if(stockModel == null) {
                return NotFound();
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purcase = updateDto.Purcase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            await _applicationDBContext.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id) {
            var stockModel = await _applicationDBContext.Stocks.FirstOrDefaultAsync(func => func.Id == id);
            if(stockModel == null) {
                return NotFound();
            }
            _applicationDBContext.Stocks.Remove(stockModel);
            await _applicationDBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}