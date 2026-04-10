using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Italbytz.Meal.STWPB.Client
{
    public partial class Meal
    {
        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("name_de")]
        public string? NameDe { get; set; }

        [JsonPropertyName("name_en")]
        public string? NameEn { get; set; }

        [JsonPropertyName("description_de")]
        public string? DescriptionDe { get; set; }

        [JsonPropertyName("description_en")]
        public string? DescriptionEn { get; set; }

        [JsonPropertyName("category")]
        public Category Category { get; set; }

        [JsonPropertyName("category_de")]
        public CategoryDe CategoryDe { get; set; }

        [JsonPropertyName("category_en")]
        public CategoryEn CategoryEn { get; set; }

        [JsonPropertyName("subcategory_de")]
        public string? SubcategoryDe { get; set; }

        [JsonPropertyName("subcategory_en")]
        public string? SubcategoryEn { get; set; }

        [JsonPropertyName("priceStudents")]
        public double PriceStudents { get; set; }

        [JsonPropertyName("priceWorkers")]
        public double PriceWorkers { get; set; }

        [JsonPropertyName("priceGuests")]
        public double PriceGuests { get; set; }

        [JsonPropertyName("allergens")]
        public AllergenEnum[] Allergens { get; set; } = Array.Empty<AllergenEnum>();

        [JsonPropertyName("order_info")]
        public long OrderInfo { get; set; }

        [JsonPropertyName("badges")]
        public Badge[] Badges { get; set; } = Array.Empty<Badge>();

        [JsonPropertyName("restaurant")]
        public Restaurant Restaurant { get; set; }

        [JsonPropertyName("pricetype")]
        public Pricetype Pricetype { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }
    }

    public enum AllergenEnum
    {
        [EnumMember(Value = "1")]
        Z1,
        [EnumMember(Value = "2")]
        Z2,
        [EnumMember(Value = "3")]
        Z3,
        [EnumMember(Value = "4")]
        Z4,
        [EnumMember(Value = "5")]
        Z5,
        [EnumMember(Value = "6")]
        Z6,
        [EnumMember(Value = "7")]
        Z7,
        [EnumMember(Value = "8")]
        Z8,
        [EnumMember(Value = "9")]
        Z9,
        [EnumMember(Value = "10")]
        Z10,
        [EnumMember(Value = "11")]
        Z11,
        [EnumMember(Value = "12")]
        Z12,
        [EnumMember(Value = "13")]
        Z13,
        [EnumMember(Value = "14")]
        Z14,
        [EnumMember(Value = "15")]
        Z15,
        A1,
        A2,
        A3,
        A4,
        A5,
        A6,
        A7,
        A8,
        A9,
        A10,
        A11,
        A12,
        A13,
        A14
    }

    public enum Badge
    {
        Nonfat,
        Vegan,
        Vegetarian,
        [EnumMember(Value = "low-calorie")]
        LowCalorie,
        [EnumMember(Value = "lactose-free")]
        LactoseFree,
        [EnumMember(Value = "gluten-free")]
        GlutenFree
    }

    public enum Category
    {
        [EnumMember(Value = "")]
        None,
        Dessert,
        Dish,
        Empty,
        Sidedish,
        Soups,
        [EnumMember(Value = "dish-default")]
        DishDefault,
        [EnumMember(Value = "dessert-counter")]
        DessertCounter,
        [EnumMember(Value = "dish-grill")]
        DishGrill
    }

    public enum CategoryDe
    {
        [EnumMember(Value = "")]
        Keine,
        Beilagen,
        Dessert,
        Empty,
        Essen,
        Suppen
    }

    public enum CategoryEn
    {
        [EnumMember(Value = "")]
        None,
        Dessert,
        Dish,
        Empty,
        [EnumMember(Value = "Side Dish")]
        SideDish,
        Soups
    }

    public enum Pricetype
    {
        Fixed,
        Weighted
    }

    public enum Restaurant
    {
        Cafete,
        [EnumMember(Value = "mensa-hamm")]
        MensaHamm,
        [EnumMember(Value = "mensa-lippstadt")]
        MensaLippstadt,
        [EnumMember(Value = "mensa-academica-paderborn")]
        MensaAcademicaPaderborn,
        [EnumMember(Value = "mensa-forum-paderborn")]
        MensaForumPaderborn,
        [EnumMember(Value = "one-way-snack")]
        OneWaySnack,
        [EnumMember(Value = "zm2")]
        ZM2,
        [EnumMember(Value = "grill-cafe")]
        GrillCafe
    }

    public static class Serialize
    {
        public static string ToJson(this Meal[] self) => JsonSerializer.Serialize(self, Converter.Options);
    }

    public static class Deserialize
    {
        public static Meal[] ToMeals(this string self) => JsonSerializer.Deserialize<Meal[]>(self, Converter.Options) ?? Array.Empty<Meal>();
    }

    internal static class Converter
    {
        public static readonly JsonSerializerOptions Options = new()
        {
            Converters =
            {
                new JsonStringEnumMemberConverter()
            }
        };
    }

    internal sealed class JsonStringEnumMemberConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            var enumType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
            return enumType.IsEnum;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var enumType = Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert;
            var converterType = typeof(EnumMemberConverter<>).MakeGenericType(enumType);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        }

        private sealed class EnumMemberConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
        {
            private readonly Dictionary<string, TEnum> _fromString = new(StringComparer.OrdinalIgnoreCase);
            private readonly Dictionary<TEnum, string> _toString = new();

            public EnumMemberConverter()
            {
                foreach (var value in Enum.GetValues<TEnum>())
                {
                    var name = value.ToString();
                    var field = typeof(TEnum).GetField(name);
                    var enumMemberValue = field?.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? name;

                    _fromString[name] = value;
                    _fromString[enumMemberValue] = value;
                    _toString[value] = enumMemberValue;
                }
            }

            public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    var text = reader.GetString() ?? string.Empty;
                    if (_fromString.TryGetValue(text, out var value))
                    {
                        return value;
                    }

                    if (Enum.TryParse<TEnum>(text, true, out value))
                    {
                        return value;
                    }
                }

                throw new JsonException($"Unable to convert value to {typeof(TEnum).Name}.");
            }

            public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
            {
                if (_toString.TryGetValue(value, out var text))
                {
                    writer.WriteStringValue(text);
                    return;
                }

                writer.WriteStringValue(value.ToString());
            }
        }
    }
}
