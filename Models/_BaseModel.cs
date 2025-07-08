using System.Text.Json.Serialization;

namespace CinemaManagement.Models
{
    public abstract class BaseModel
    {
        [JsonPropertyOrder(-2)]
        public int Id { get; set; }

        [JsonPropertyOrder(98)]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyOrder(99)]
        public DateTime UpdatedAt { get; set; }
    }
}