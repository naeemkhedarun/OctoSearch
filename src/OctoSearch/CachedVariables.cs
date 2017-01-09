using System.Collections.Generic;
using Octopus.Client.Model;

namespace OctoSearch
{
    class CachedVariables
    {
        public LibraryVariableSetResource Resource { get; set; }
        public IList<VariableResource> Variables { get; set; }
    }
}