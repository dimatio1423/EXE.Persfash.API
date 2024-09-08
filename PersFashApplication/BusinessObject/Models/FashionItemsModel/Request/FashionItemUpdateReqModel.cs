using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.FashionItemsModel.Request
{
    public class FashionItemUpdateReqModel
    {
        [Required(ErrorMessage ="ItemId is required")]
        public int itemId { get; set; }

        [StringLength(100, ErrorMessage = "Item name can't be longer than 100 characters.")]
        public string? ItemName { get; set; }

        [StringLength(50, ErrorMessage = "Category can't be longer than 50 characters.")]
        public string? Category { get; set; }

        [Range(0.01, 10000000.00, ErrorMessage = "Price must be between 0.01 and 10,000,000,00.")]
        public decimal? Price { get; set; }

        public string? FitType { get; set; }

        public string? GenderTarget { get; set; }

        public List<string>? FashionTrend { get; set; }

        public List<string>? Size { get; set; }

        public List<string>? Color { get; set; }

        public List<string>? Material { get; set; }

        public List<string>? Occasion { get; set; }

        public string? ProductUrl { get; set; }

        public IFormFile? Thumbnail { get; set; }

        public List<IFormFile>? ItemImages { get; set; }
    }
}
