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
    public class ProjectListViewModel : ViewModelBase
    {
        private readonly IProjectClient _projectClient;

        public ProjectListViewModel(IProjectClient projectClient)
        {
            if (null == projectClient)
                throw new ArgumentNullException("projectClient");
            _projectClient = projectClient;
        }

        public ObservableCollection<Project> Projects { get; private set; } = 
            new ObservableCollection<Project>();

        private bool _loading = false;

        public bool Loading {
            get => _loading;
            set => Set(ref _loading, value);
        }

        private bool _error = false;

        public bool Error {
            get => _error;
            set => Set(ref _error, value);
        }

        public async void LoadProjects()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => {
                Loading = true;
                Projects.Clear();
            });

            try
            {
                var projects = await Task.Run(_projectClient.GetUserProjectsAsync);
                await DispatcherHelper.ExecuteOnUIThreadAsync(() => {
                    foreach (var p in projects)
                        Projects.Add(p);
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
