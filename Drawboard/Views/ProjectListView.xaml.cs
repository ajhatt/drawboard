using Drawboard.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Drawboard.Views
{
    public sealed partial class ProjectListView : UserControl
    {

        /// <summary>
        /// View model using the app wide project client instance.
        /// </summary>
        public ProjectListViewModel ViewModel { get; } 
            = new ProjectListViewModel(App.ProjectClient);

        public ProjectListView()
        {
            InitializeComponent();
            ViewModel.LoadProjects();
        }
    }
}
