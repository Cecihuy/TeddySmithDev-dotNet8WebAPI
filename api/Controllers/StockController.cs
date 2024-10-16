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
    }
}