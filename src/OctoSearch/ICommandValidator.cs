using System.Collections.Generic;

namespace OctoSearch
{

    public interface ICommandValidator
    {
        IEnumerable<string> GetErrors();
    }
}