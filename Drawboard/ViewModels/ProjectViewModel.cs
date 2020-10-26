using Drawboard.API;
using Drawboard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Drawboard.ViewModels
{
    /// <summary>
    /// Project view model.
    /// </summary>
    public class ProjectViewModel : ViewModelBase
    {
        private readonly Project _project;
        private readonly IProjectClient _client;
        private Uri _imageSource;

        /// <summary>
        /// Project image source.
        /// </summary>
        public Uri ImageSource
        {
            get => _imageSource;
            set => Set(ref _imageSource, value);
        }

        /// <summary>
        /// Project name.
        /// </summary>
        public string Name => _project.Name;

        /// <summary>
        /// Project description.
        /// </summary>
        public string Description => _project.Description;

        /// <summary>
        /// Project ID.
        /// </summary>
        public string ID => _project.ID;

        public ProjectViewModel(Project project, IProjectClient client)
        {
            if (null == project)
                throw new ArgumentNullException("project");
            if (null == client)
                throw new ArgumentNullException("client");
            _project = project;
            _client = client;
        }

        /// <summary>
        /// Begin loading the projects image.
        /// </summary>
        public async void LoadImageAsync()
        {
            try
            {
                 ImageSource = await _client.GetProjectLogoAsync(ID);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                ImageSource = null;
            }
        }
    }
}
