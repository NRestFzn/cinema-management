using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CinemaManagement.Dto;
using CinemaManagement.Models;

namespace CinemaManagement.Dto
{
    public class PaymentMethodDto : BaseModel
    {
        public required string Name { get; set; }
    }

    public class PaymentMethodDetailDto : PaymentMethodDto
    {
        [JsonPropertyOrder(97)]
        public List<TransactionDto> Transactions { get; set; } = [];
    }

    public class CreatePaymentMethodDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "PaymentMethod name is required")]
        public required string Name { get; set; }
    }

    public class UpdatePaymentMethodDto : CreatePaymentMethodDto { }
}