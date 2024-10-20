using System;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock {
    public class CreateStockRequestDto {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol can not over 10 characters")]
        public string Symbol {get;set;} = string.Empty;
        [Required]
        [MaxLength(10, ErrorMessage = "Company name can not over 10 characters")]
        public string CompanyName {get;set;} = string.Empty;
        [Required]
        [Range(1,100000)]
        public decimal Purcase {get;set;}
        [Required]
        [Range(1,100000)]
        public decimal LastDiv {get;set;}
        [Required]
        [MaxLength(10, ErrorMessage = "Industry can not over 10 characters")]
        public string Industry {get;set;} = string.Empty;        
        [Range(1,100000)]
        public long MarketCap {get;set;}
    }
}