using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octopus.Client;
using OctoSearch.Login;

namespace OctoSearch.Cache
{
    class CacheCommandHandler : ICommand
    {
        private readonly CacheRepository _repository;

        public CacheCommandHandler(CacheRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Execute(object command)
        {
            try
            {
                var settings = new FileInfo(".cache/octopus.settingsfile");
                if (!settings.Exists)
                {
                    Console.WriteLine("Please use the login command first to authenticate.");
                    return -1;
                }

                var login = JsonConvert.DeserializeObject<LoginSettings>(File.ReadAllText(settings.FullName));

                using (var client = await OctopusAsyncClient.Create(new OctopusServerEndpoint(login.OctopusUri, login.ApiKey)))
                {
                    var all = await client.Repository.LibraryVariableSets.FindAll();

                    foreach (var resource in all)
                    {
                        var variables = (await client.Repository.VariableSets.Get(resource.VariableSetId)).Variables;
                        await _repository.Save(new CachedVariables { Variables = variables, Resource = resource });

                        Console.WriteLine($"Saved {resource.Name}.");
                    }
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        public Type CommandType => typeof(CacheCommand);
    }
}