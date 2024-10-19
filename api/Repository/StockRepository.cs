using System;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Stock>> GetAllAsync() {
            return await _applicationDBContext.Stocks.Include(B => B.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id) {
            return await _applicationDBContext.Stocks
                .Include(B => B.Comments)
                .FirstOrDefaultAsync(B=> B.Id == id);
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
