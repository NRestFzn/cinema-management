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
    public class TransactionDetailDto
    {
        public int TransactionId { get; set; }
        public int ChairId { get; set; }
        public decimal Price { get; set; }
    }

    // public class CreateTransactionDetailDto
    // {
    //     [Required(ErrorMessage = "Schedule is required.")]
    //     public int ScheduleId { get; set; }

    //     [Required(ErrorMessage = "Payment Method is required.")]
    //     public int PaymentMethodId { get; set; }

    //     [Required(ErrorMessage = "You must select at least one chair.")]
    //     [MinLength(1, ErrorMessage = "You must select at least one chair.")]
    //     public List<int> ChairIds { get; set; } = [];
    //     public int? VoucherId { get; set; }
    // }

    // public class UpdateTransactionDetailDto : CreateTransactionDetailDto { }
}