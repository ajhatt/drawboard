using Drawboard.Model;
using System;
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

        /// <summary>
        /// Get a projects logo as a encoded URI.
        /// </summary>
        /// <param name="parentProjectID"></param>
        /// <returns></returns>
        Task<Uri> GetProjectLogoAsync(string parentProjectID);
    }
}