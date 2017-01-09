using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octopus.Client;

namespace OctoSearch.Login
{
    class LoginCommandHandler : ICommand
    {
        public async Task<int> Execute(object command)
        {
            var loginCommand = (LoginCommand) command;
            try
            {
                using (var client = await OctopusAsyncClient.Create(new OctopusServerEndpoint(loginCommand.OctopusUri)))
                {
                    Console.WriteLine("Please enter your pasword...");
                    var password = ReadPassword();

                    await client.Repository.Users.SignIn(loginCommand.Username, password);
                    var key = await client.Repository.Users.CreateApiKey(await client.Repository.Users.GetCurrent(), "OctoSearch");

                    if (!string.IsNullOrEmpty(key.ApiKey))
                    {
                        var directoryInfo = new DirectoryInfo(".cache");
                        if (!directoryInfo.Exists)
                        {
                            directoryInfo.Create();
                        }
                        File.WriteAllText(".cache/octopus.settingsfile", JsonConvert.SerializeObject(new LoginSettings(loginCommand.OctopusUri, key.ApiKey)));
                        Console.WriteLine("Successfully logged in.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to login.");
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

        public Type CommandType => typeof(LoginCommand);

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <param name="mask">a <c>char</c> representing your choice of console mask</param>
        /// <returns>the string the user typed in </returns>
        public static string ReadPassword(char mask)
        {
            const int ENTER = 13, BACKSP = 8, CTRLBACKSP = 127;
            int[] FILTERED = { 0, 27, 9, 10 /*, 32 space, if you care */ }; // const

            var pass = new Stack<char>();
            char chr = (char)0;

            while ((chr = System.Console.ReadKey(true).KeyChar) != ENTER)
            {
                if (chr == BACKSP)
                {
                    if (pass.Count > 0)
                    {
                        System.Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (chr == CTRLBACKSP)
                {
                    while (pass.Count > 0)
                    {
                        System.Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (FILTERED.Count(x => chr == x) > 0) { }
                else
                {
                    pass.Push((char)chr);
                    System.Console.Write(mask);
                }
            }

            System.Console.WriteLine();

            return new string(pass.Reverse().ToArray());
        }

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <returns>the string the user typed in </returns>
        public static string ReadPassword()
        {
            return ReadPassword('*');
        }
    }
}