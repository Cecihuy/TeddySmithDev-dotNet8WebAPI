using System;
using System.Linq.Expressions;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers {
    public static class StockMappers {
        public static StockDto ToStockDto(this Stock stockModel) {
            return new StockDto {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purcase = stockModel.Purcase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(func => func.ToCommentDto()).ToList()
            };
        }
        public static Stock toStockFromCreateDTO(this CreateStockRequestDto createStockRequestDto) {
            return new Stock {
                Symbol = createStockRequestDto.Symbol,
                CompanyName = createStockRequestDto.CompanyName,
                Purcase = createStockRequestDto.Purcase,
                LastDiv = createStockRequestDto.LastDiv,
                Industry = createStockRequestDto.Industry,
                MarketCap = createStockRequestDto.MarketCap
            };
        }
    }
}