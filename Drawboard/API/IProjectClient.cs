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
        /// <summary>
        /// Query the authenticated users project list.
        /// </summary>
        /// <returns></returns>
        Task<List<Project>> GetUserProjectsAsync();
    }
}