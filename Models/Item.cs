using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicApi.Models;
using System.ComponentModel.DataAnnotations;

namespace BasicApi.Models
{
    public class Item : BaseModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok tidak boleh bernilai negatif.")]
        public int Stock { get; set; } = 0;
    }
}