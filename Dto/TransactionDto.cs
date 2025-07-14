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
    public class TransactionDto : BaseModel
    {
        public int UserId { get; set; }
        public int PaymentMethodId { get; set; }
        public int ScheduleId { get; set; }
        public int? VoucherId { get; set; }
        public required string TransactionCode { get; set; }
        public TransactionStatus Status { get; set; }
        public int VoucherDiscount { get; set; }
        public int TotalDiscount { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public User User { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }
        public MovieScheduleDto MovieSchedule { get; set; }
        public VoucherDto? Voucher { get; set; }
        public ICollection<TransactionDetailDto> TransactionDetail { get; set; } = [];
    }

    public class CreateTransactionFormData
    {
        [Required(ErrorMessage = "Schedule is required.")]
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "Payment Method is required.")]
        public int PaymentMethodId { get; set; }

        [Required(ErrorMessage = "You must select at least one chair.")]
        [MinLength(1, ErrorMessage = "You must select at least one chair.")]
        public List<int> ChairIds { get; set; } = [];
        public int? VoucherId { get; set; }
    }

    public class CreateTransactionDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Transaction code is required.")]
        public required string TransactionCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Status is required.")]
        public TransactionStatus Status { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Original amount is required.")]
        public decimal OriginalAmount { get; set; }

        public int? VoucherDiscount { get; set; }
        public int? TotalDiscount { get; set; }
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Schedule is required.")]
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "Payment Method is required.")]
        public int PaymentMethodId { get; set; }

        [Required(ErrorMessage = "You must select at least one chair.")]
        [MinLength(1, ErrorMessage = "You must select at least one chair.")]
        public List<int> ChairIds { get; set; } = [];
        public int? VoucherId { get; set; }
    }

    public class UpdateTransactionDto : CreateTransactionDto { }
}