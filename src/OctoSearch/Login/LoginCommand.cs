using System;
using System.Collections.Generic;
using CommandLine;

namespace OctoSearch.Login
{
    [Verb("login", HelpText = "")]
    class LoginCommand : Command
    {

        [Option('l', "octopus-uri", Required = true, HelpText = "The Octopus server URI.")]
        public string OctopusUri { get; set; }

        [Option('u', "username", Required = true, HelpText = "The octopus username to authenticate with.")]
        public string Username { get; set; }
            
        public override IEnumerable<string> GetErrors()
        {
            foreach (var error in base.GetErrors())
            {
                yield return error;
            }

            if(!Uri.IsWellFormedUriString(OctopusUri, UriKind.RelativeOrAbsolute))
                yield return $"Uri {OctopusUri} is not a valid location.";
        }
    }
}