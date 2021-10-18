using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStoreAPI.Models
{
    public class Product
    {
        [Key]
        public string SKU { get; set; }

        public string Name { get; set; }

        public decimal? Price { get; set; }

        public string Description { get; set; }

        public int? NumberInStock { get; set; }

        public bool IsInStock
        {
            get
            {
                return NumberInStock > 0;
            }
        }
    }
}