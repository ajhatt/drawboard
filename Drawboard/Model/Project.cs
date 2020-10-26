using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawboard.Model
{
    /// <summary>
    /// Represents a Drawboad project.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Unique ID of project.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Name of the project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Project description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Project owner ID. Might not be set.
        /// </summary>
        public string OwnerID { get; set; }
    }
}
