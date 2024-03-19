using System.Text.Json;
using KeyValueStore.Models;
using InvalidOperationException = System.InvalidOperationException;

namespace KeyValueStore.Services;

public class JsonFileStoreService
{
    private readonly string _filePath = "store.json";

    public JsonFileStoreService()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(new List<KeyValueItem>()));
        }
    }

    public IEnumerable<KeyValueItem?> GetItems()
    {
        using var reader = new StreamReader(_filePath);
        var json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<List<KeyValueItem>>(json) ?? throw new InvalidOperationException();
    }

    public void AddItem(KeyValueItem? item)
    {
        var items = GetItems().ToList();
        var existingItem = items.FirstOrDefault(i => i?.Key == item?.Key);
        if (existingItem != null)
        {
            items.Remove(existingItem);
        }
        items.Add(item);
        File.WriteAllText(_filePath, JsonSerializer.Serialize(items));
    }

    public KeyValueItem? GetItem(string key)
    {
        return GetItems().FirstOrDefault(i => i.Key == key);
    }
}