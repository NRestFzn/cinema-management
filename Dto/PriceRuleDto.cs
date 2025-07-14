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
    public class PriceRuleDto : BaseModel
    {
        public required string Name { get; set; }
        public required int ChairTypeId { get; set; }
        public required int StudioId { get; set; }
        public DayTypeEnum DayType { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
        public DateTime DaysDate { get; set; }
        public decimal Price { get; set; }
    }

    public class PriceRuleDetailDto : PriceRuleDto
    {
        [JsonPropertyOrder(97)]
        public required MasterChairType ChairType { get; set; }

        [JsonPropertyOrder(96)]
        public required Studio Studio { get; set; }
    }

    public class CreatePriceRuleDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Rule name is required.")]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Chair Type ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Chair Type ID must be a valid ID.")]
        public required int ChairTypeId { get; set; }

        [Required(ErrorMessage = "Studio ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Studio ID must be a valid ID.")]
        public required int StudioId { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public required decimal Price { get; set; }

        [EnumDataType(typeof(DayTypeEnum), ErrorMessage = "Invalid day type specified.")]
        public DayTypeEnum? DayType { get; set; }

        [Required(ErrorMessage = "Is holiday is required.")]
        public bool IsHoliday { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime DaysDate { get; set; }
    }

    public class UpdatePriceRuleDto : CreatePriceRuleDto { }
}