using System;
using System.Threading.Tasks;

namespace OctoSearch
{
    public interface ICommand
    {
        Task<int> Execute(object command);
        Type CommandType { get; }
    }
}