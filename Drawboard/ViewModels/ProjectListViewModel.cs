using Drawboard.API;
using Drawboard.Model;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Drawboard.ViewModels
{
    /// <summary>
    /// Project list view model.
    /// </summary>
    public class ProjectListViewModel : ViewModelBase
    {
        private readonly IProjectClient _projectClient;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="projectClient">Drawboard project API client.</param>
        public ProjectListViewModel(IProjectClient projectClient)
        {
            if (null == projectClient)
                throw new ArgumentNullException("projectClient");
            _projectClient = projectClient;
        }

        /// <summary>
        /// Observable collection of authenticated users projects.
        /// </summary>
        public ObservableCollection<ProjectViewModel> Projects { get; private set; } =
            new ObservableCollection<ProjectViewModel>();

        private bool _loading = false;

        /// <summary>
        /// Indicates of the project list is currently loading.
        /// </summary>
        public bool Loading
        {
            get => _loading;
            set => Set(ref _loading, value);
        }

        private bool _error = false;

        /// <summary>
        /// Indicates if there was a problem loading the users data.
        /// </summary>
        public bool Error
        {
            get => _error;
            set => Set(ref _error, value);
        }

        /// <summary>
        /// Load/refresh the authenticated users project list.
        /// </summary>
        public async void LoadProjects()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Loading = true;
                Projects.Clear();
            });

            try
            {
                var projects = await _projectClient.GetUserProjectsAsync();
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    foreach (var p in projects.Select(x => new ProjectViewModel(x, _projectClient)))
                    {
                        Projects.Add(p);
                        p.LoadImageAsync();
                    }
                    Loading = false;
                });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Error = true;
                    Loading = false;
                });
            }
        }
    }
}
