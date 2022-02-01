using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPhoneBookManager.Repository.RepositoriesWithJson
{
    // Thread-safety json serializer methods
    public static class JsonHelper
    {
        public static IEnumerable<T> ReadAndDeserializeFromFile<T>(string path)
        {
            try
            {
                string jsonString = File.ReadAllText(path);
                var records = JsonSerializer.Deserialize<IEnumerable<T>>(jsonString);
                return records;
            }
            catch (Exception e)
            {
                return null;
            }
            
        }
        public static void WriteJsonToText<T>(string path, List<T> objectToSerialize)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonStringUpdate = JsonSerializer.Serialize(objectToSerialize, options);
            File.WriteAllText(path, jsonStringUpdate);
        }
    }
}
