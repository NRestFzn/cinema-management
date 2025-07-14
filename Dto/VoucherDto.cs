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
    public class VoucherDto : BaseModel
    {
        public required string Code { get; set; }
        public required int Discount { get; set; }
        public required DateTime ValidDate { get; set; }
        public required DateTime ExpiredDate { get; set; }
        public required int Quota { get; set; }
    }

    public class VoucherDetailDto : VoucherDto
    {
        [JsonPropertyOrder(97)]
        public ICollection<Transaction>? Transactions { get; set; }
    }

    public class CreateVoucherDto : IValidatableObject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Code is required")]
        public required string Code { get; set; }

        [Required(ErrorMessage = "Discount is required")]
        public required int Discount { get; set; }

        [Required(ErrorMessage = "Valid date is required")]
        public required DateTime ValidDate { get; set; }

        [Required(ErrorMessage = "Expired date is required")]
        public required DateTime ExpiredDate { get; set; }

        [Required(ErrorMessage = "Quota is required")]
        public required int Quota { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ValidDate > ExpiredDate)
            {
                yield return new ValidationResult(
                    "The valid date cannot be later than the expired date.",
                    new[] { nameof(ValidDate), nameof(ExpiredDate) }
                );
            }
        }
    }


    public class UpdateVoucherDto : CreateVoucherDto { }
}