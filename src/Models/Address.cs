namespace Models;

public record Address(string Street, string Number, string? Complement, string Neighborhood, string ZipCode, string City, string State)
{
    public override string ToString()
    {
        return $"Rua {Street}, nº {Number}, Bairro {Neighborhood}, {ZipCode}, {City} - {State}";
    }
}
