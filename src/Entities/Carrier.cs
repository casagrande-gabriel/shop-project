namespace Entities;

public class Carrier
{
    private static int _lastId = 1;
    public int Id { get; init; }
    public string Name { get; set; }
    public double PricePerKm { get; set; }

    public Carrier(string name, double priceKm)
    {
        Id = _lastId++;
        Name = name;
        PricePerKm = priceKm;
    }

    public override string ToString()
    {
        return $"ID: {Id} | Nome: {Name} | Preço por Km: {PricePerKm:C2}";
    }
}
