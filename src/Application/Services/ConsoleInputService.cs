using Domain.Entities;
using Domain.Services;
using Domain.ValueObjects;

namespace Application.Services;

public sealed class ConsoleInputService : IInputService
{
    public string ReadString(string query = ": ", string? originalValue = null)
    {
        Console.Write(query);

        if (originalValue is null)
        {
            return Console.ReadLine() ?? string.Empty;
        }

        int startingLeft = Console.CursorLeft;
        int startingTop = Console.CursorTop;

        string currentValue = originalValue ?? string.Empty;

        Console.Write(currentValue);

        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (currentValue.Length > 0)
                {
                    currentValue = currentValue[..^1];
                }
            }
            else if (!char.IsControl(key.KeyChar))
            {
                currentValue += key.KeyChar;
            }

            // Apaga completamente o que estava escrito anteriormente
            Console.SetCursorPosition(startingLeft, startingTop);
            Console.Write(new string(' ', Console.WindowWidth - startingLeft));
            Console.SetCursorPosition(startingLeft, startingTop);
            Console.Write(currentValue);

        } while (true);

        Console.WriteLine();
        return currentValue;
    }

    public Address ReadAddress(Address? address = null)
    {
        string street = ReadString("Insira a rua: ", address?.Street);
        string number = ReadString("Insira o número: ", address?.Number);
        string complement = ReadString("Insira o complemento: ", address?.Complement);
        string neighborhood = ReadString("Insira o nome do bairro: ", address?.Neighborhood);
        string zipCode = ReadString("Insira o CEP: ", address?.ZipCode);
        string city = ReadString("Insira a cidade: ", address?.City);
        string state = ReadString("Insira o estado: ", address?.City);

        return new Address(
            street,
            number,
            complement,
            neighborhood,
            zipCode,
            city,
            state);
    }

    public T? Choose<T>(IEnumerable<T> options) where T : BaseEntity
    {
        if (options.Count() == 1) return options.First();

        T? selected = null;

        while (selected is null)
        {
            Console.Clear();

            foreach (var option in options)
            {
                Console.WriteLine(option.ToString());
            }

            Console.Write("\nDigite o ID para selecionar ou 0 para cancelar: ");

            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) continue;

            if (input == "0") break;

            if (int.TryParse(input, out int id))
            {
                selected = options.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("O ID digitádo é inválido");
                Thread.Sleep(1500);
                Console.Clear();
            }

            if (selected is null)
            {
                Console.Clear();
                Console.WriteLine("O ID digitádo é inválido");
                Thread.Sleep(1500);
            }
        }

        return selected;
    }
}