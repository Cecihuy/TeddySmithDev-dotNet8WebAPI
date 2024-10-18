using System;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository {
    public class StockRepository : IStockRepository {
        private readonly ApplicationDBContext _applicationDBContext;
        public StockRepository(ApplicationDBContext applicationDBContext) {
            _applicationDBContext = applicationDBContext;
        }
        public Task<List<Stock>> GetAllAsync() {
            return _applicationDBContext.Stocks.ToListAsync();
        }
    }
}
