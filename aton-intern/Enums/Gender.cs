using System.Text.Json.Serialization;

namespace Aton_intern.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        Female,
        Male, 
        Unknown
    }
}
