using System;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase {
        private readonly ApplicationDBContext _applicationDBContext;
        public StockController(ApplicationDBContext applicationDBContext) {
            _applicationDBContext = applicationDBContext;
        }
        [HttpGet]
        public IActionResult GetAll() {
            var stocks = _applicationDBContext.Stocks.ToList()
                .Select(fn => fn.ToStockDto());
            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id) {
            var stock = _applicationDBContext.Stocks.Find(id);
            if(stock == null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto) {
            var stockModel = stockDto.toStockFromCreateDTO();
            _applicationDBContext.Stocks.Add(stockModel);
            _applicationDBContext.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto ) {
            var stockModel = _applicationDBContext.Stocks.FirstOrDefault(func => func.Id == id);
            if(stockModel == null) {
                return NotFound();
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purcase = updateDto.Purcase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            _applicationDBContext.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id) {
            var stockModel = _applicationDBContext.Stocks.FirstOrDefault(func => func.Id == id);
            if(stockModel == null) {
                return NotFound();
            }
            _applicationDBContext.Stocks.Remove(stockModel);
            _applicationDBContext.SaveChanges();
            return NoContent();
        }
    }
}