using System;
using System.Threading.Tasks;

namespace Core.Lib.Core
{
    public interface IWorker : IDisposable
    {
        Task<bool> Complete();
    }
}