using Drawboard.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drawboard.API
{
    /// <summary>
    /// Drawboard project API client interface.
    /// </summary>
    public interface IProjectClient
    {
        Task<List<Project>> GetUserProjectsAsync();
    }
}