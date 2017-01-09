using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OctoSearch.Search
{
    class SearchCommandHandler : ICommand
    {
        private readonly CacheRepository _repository;

        public SearchCommandHandler(CacheRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Execute(object command)
        {
            var searchCommand = (SearchCommand) command;

            var searchRegex = new Regex(
                string.IsNullOrEmpty(searchCommand.SearchRegex) ? "\\w." : searchCommand.SearchRegex, 
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            try
            {
                var allVariables = await _repository.LoadAll();

                var matches = allVariables
                    .SelectMany(all => all.Variables.Select(variable => new {all.Resource, Variable = variable}))
                    .Select(x => new {Variable = x, Match = searchRegex.Match(x.Variable.Name)})
                    .Where(x => x.Match.Success)
                    .ToList();

                var formatVariableSet = matches.Max(x => x.Variable.Resource.Name.Length);
                var formatVariableName = matches.Max(x => x.Variable.Variable.Name.Length);

                foreach (var match in matches)
                {
                    Console.WriteLine($"{match.Variable.Resource.Name.PadRight(formatVariableSet)} {match.Variable.Variable.Name.PadRight(formatVariableName)}");
                }

                new ReportWriter().WriteReport(searchCommand, () =>
                {
                    return string.Join(Environment.NewLine, matches.Select(x => $"{x.Variable.Resource.Name.PadRight(formatVariableSet)} {x.Variable.Variable.Name.PadRight(formatVariableName)}"));
                }, () =>
                {
                    return matches.GroupBy(x => x.Variable.Resource).Select(x => new { VariableSet = x.Key, Variables = x.Select(y => y.Variable.Variable)});
                }, () =>
                {
                    return matches.GroupBy(x => x.Variable.Resource).Select(x => new { VariableSet = x.Key, Variables = x.Select(y => y.Variable.Variable) });
                });

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        public Type CommandType => typeof(SearchCommand);
    }
}