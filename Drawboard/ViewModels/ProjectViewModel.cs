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
    public class ProjectViewModel : ViewModelBase
    {

        private readonly IProjectClient _client;
        private Uri _imageSource;

        public Uri ImageSource
        {
            get => _imageSource;
            set => Set(ref _imageSource, value);
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ID { get; set; }

        public ProjectViewModel(IProjectClient client)
        {
            if (null == client)
                throw new ArgumentNullException("client");
            _client = client;
        }

        public async void LoadImage()
        {
            try
            {
                 ImageSource = await _client.GetProjectLogo(ID);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                ImageSource = null;
            }
        }
    }
}
