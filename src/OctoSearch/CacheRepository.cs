using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OctoSearch
{
    class CacheRepository
    {

        public Task Save(CachedVariables variables)
        {
            File.WriteAllText($".cache/{variables.Resource.Name}.json", JsonConvert.SerializeObject(variables, Formatting.Indented));

            return Task.CompletedTask;
        }

        public Task<IEnumerable<CachedVariables>>  LoadAll()
        {
            var cacheDirectory = new DirectoryInfo(".cache");
            if (!cacheDirectory.Exists)
            {
                return Task.FromResult(Enumerable.Empty<CachedVariables>());
            }

            var cachedVariables = cacheDirectory.GetFiles("*.json")
                .Select(info => JsonConvert.DeserializeObject<CachedVariables>(File.ReadAllText(info.FullName)));

            return Task.FromResult(cachedVariables);
        }
    }
}