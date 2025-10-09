using System.Text.Json.Serialization;

namespace GameCommerce.Aplicacao.Dtos
{
    public class NewAgeRequestDto
    {
        public int Amount { get; set; }
        public Customer Customer { get; set; }
        public Address Address { get; set; }

        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; } = "pix";
        public string Installments { get; set; } = "1";
        public Card Card { get; set; }

        [JsonPropertyName("postback_url")]
        public string PostbackUrl { get; set; }
        public List<Item> Items { get; set; }
    }

    public class NewAgeResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public GatewayResponseData Data { get; set; }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [JsonPropertyName("document_number")]
        public string Document_Number { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [JsonPropertyName("zip_code")]
        public string ZipCode { get; set; }
        public string Complement { get; set; }
        public string Country { get; set; } = "BR";
    }

    public class Card
    {
        public string Number { get; set; }


        [JsonPropertyName("holder_name")]
        public string HolderName { get; set; }


        [JsonPropertyName("expiration_date")]
        public string ExpirationDate { get; set; }
        public string Cvv { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }

    public class GatewayResponseData
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }

        [JsonPropertyName("transaction_id")]
        public string Transaction_Id { get; set; }

        [JsonPropertyName("pix_code")]
        public string Pix_Code { get; set; }
        public Customer? Customer { get; set; }
        [JsonPropertyName("postback_url")]
        public string Postback_Url { get; set; }
    }
}
