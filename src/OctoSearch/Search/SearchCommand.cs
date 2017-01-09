using CommandLine;

namespace OctoSearch.Search
{
    [Verb("search", HelpText = "")]
    class SearchCommand : Command
    {
        [Option('r', "regex", Required = false, HelpText = "The regex to search variable names with.")]
        public string SearchRegex { get; set; }
    }
}