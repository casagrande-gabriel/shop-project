using System.Text.Json;
using Domain.Services;

namespace Application.Services;

public class PersistenceService : IPersistenceService
{
    private static string BasePath
    {
        get => Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName, "Data");
    }
    private readonly JsonSerializerOptions options = new() { WriteIndented = true };

    private static T? ReadFromFile<T>(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }

        string content = File.ReadAllText(path);

        if (string.IsNullOrEmpty(content))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(content);
    }

    public T? Load<T>(string key)
    {
        try
        {
            Directory.CreateDirectory(BasePath);
            string filePath = Path.Combine(BasePath, $"{key}.json");

            return ReadFromFile<T>(filePath)
                ?? ReadFromFile<T>(filePath + ".bak");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Não foi encontrado o arquivo de {key}");
            return default;
        }
        catch
        {
            Console.WriteLine($"Ocorreu um erro ao carregar dados de {key}");
            return default;
        }
    }

    public bool Save(string key, object data)
    {
        try
        {
            Directory.CreateDirectory(BasePath);
            string filePath = Path.Combine(BasePath, $"{key}.json");

            if (File.Exists(filePath))
            {
                File.Copy(filePath, filePath + ".bak", true);
            }

            File.WriteAllText(filePath, JsonSerializer.Serialize(data, options));

            return true;
        }
        catch
        {
            Console.WriteLine($"Ocorreu um erro ao salvar dados de {key}");
            return false;
        }
    }
}
