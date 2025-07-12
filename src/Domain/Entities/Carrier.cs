using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Carrier : BaseEntity
{
    public string Name { get; set; }
    public double PricePerKm { get; set; }

    public Carrier(string name, double priceKm) : base()
    {
        Name = name;
        PricePerKm = priceKm;
    }

    [JsonConstructor]
    public Carrier(int id, string name, double pricePerKm) : base(id)
    {
        Id = id;
        Name = name;
        PricePerKm = pricePerKm;
    }

    public override string ToString()
    {
        return $"ID: {Id} | Nome: {Name} | Preço por Km: {PricePerKm:C2}";
    }
}
