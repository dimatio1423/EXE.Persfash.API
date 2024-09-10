using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.FashionItemsModel.Request
{
    public class FashionItemCreateReqModel
    {
        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(100, ErrorMessage = "Item name can't be longer than 100 characters.")]
        public string ItemName { get; set; } = null!;

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50, ErrorMessage = "Category can't be longer than 50 characters.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 10000000.00, ErrorMessage = "Price must be between 0.01 and 10,000,000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Fit Type is required.")]
        public string FitType { get; set; }

        [Required(ErrorMessage = "Gender Target is required.")]
        public string GenderTarget { get; set; }

        [Required(ErrorMessage = "Fashion Trend is required.")]
        [MinLength(1, ErrorMessage = "At least one Fashion Trend is required.")]
        public List<string> FashionTrend { get; set; }

        [Required(ErrorMessage = "Size is required.")]
        [MinLength(1, ErrorMessage = "At least one size is required.")]
        public List<string> Size { get; set; }

        [Required(ErrorMessage = "At least one color is required.")]
        [MinLength(1, ErrorMessage = "At least one color is required.")]
        public List<string> Color { get; set; }

        [Required(ErrorMessage = "At least one material is required.")]
        [MinLength(1, ErrorMessage = "At least one material is required.")]
        public List<string> Material { get; set; }

        [Required(ErrorMessage = "At least one occasion is required.")]
        [MinLength(1, ErrorMessage = "At least one occasion is required.")]
        public List<string> Occasion { get; set; }

        [Required(ErrorMessage = "Thumbnail URL is required.")]
        public string Thumbnail { get; set; }

        [Required(ErrorMessage = "Product URL is required.")]
        public string ProductUrl { get; set; }

        [Required(ErrorMessage = "At least one image is required.")]
        [MinLength(1, ErrorMessage = "At least one image is required.")]
        public List<string> ItemImages { get; set; }
    }
}
