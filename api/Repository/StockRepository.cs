using System;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Repository {
    public class StockRepository : IStockRepository {
        private readonly ApplicationDBContext _applicationDBContext;
        public StockRepository(ApplicationDBContext applicationDBContext) {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<Stock> CreateAsync(Stock stockModel) {
            await _applicationDBContext.Stocks.AddAsync(stockModel);
            await _applicationDBContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id) {
            var stockModel = await _applicationDBContext.Stocks.FirstOrDefaultAsync(func => func.Id == id);
            if(stockModel == null) {
                return null;               
            }
            _applicationDBContext.Stocks.Remove(stockModel);
            await _applicationDBContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject queryObject) {
            var stock = _applicationDBContext.Stocks.Include(B => B.Comments).AsQueryable();
            if(!string.IsNullOrWhiteSpace(queryObject.CompanyName)) {
                stock = stock.Where(B=> B.CompanyName.Contains(queryObject.CompanyName));
            }
            if(!string.IsNullOrWhiteSpace(queryObject.Symbol)) {
                stock = stock.Where(B=> B.Symbol.Contains(queryObject.Symbol));
            }
            return await stock.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id) {
            return await _applicationDBContext.Stocks
                .Include(B => B.Comments)
                .FirstOrDefaultAsync(B=> B.Id == id);
        }

        public Task<bool> StockExists(int id)
        {
            return _applicationDBContext.Stocks.AnyAsync(B=> B.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto) {
            var stockModel = await _applicationDBContext.Stocks.FirstOrDefaultAsync(func => func.Id == id);
            if(stockModel == null){
                return null;
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purcase = updateDto.Purcase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            await _applicationDBContext.SaveChangesAsync();
            return stockModel;
        }
    }
}
