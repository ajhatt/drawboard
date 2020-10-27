using Drawboard.API;
using Drawboard.Model;
using Drawboard.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class ProjectListViewModelTest
    {

        [TestMethod]
        public void TestBasicLoading()
        {
            // arrange
            var loadingEvents = new List<bool>();
            var signal = new AutoResetEvent(false);
            var expectedProjects = new List<Project> { 
                new Project(),
                new Project()
            };
            var mockClient = new Mock<IProjectClient>();
            mockClient.Setup(c => c.GetUserProjectsAsync()).Returns(Task.FromResult(expectedProjects));
            var vm = new ProjectListViewModel(mockClient.Object);
            vm.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "Loading")
                {
                    loadingEvents.Add(vm.Loading);
                    if (loadingEvents.Count > 1)
                        signal.Set();
                }
            };

            // act
            vm.LoadProjects();

            // assert
            if (!signal.WaitOne(TimeSpan.FromMilliseconds(500)))
                Assert.Fail("timed out waiting for projects to load");
            Assert.IsTrue(new bool[] { true, false }.SequenceEqual(loadingEvents));
            Assert.AreEqual(expectedProjects.Count, vm.Projects.Count);
        }

        [TestMethod]
        public void TestingLoadingErrorState()
        {
            // arrange
            var signal = new AutoResetEvent(false);
            var mockClient = new Mock<IProjectClient>();
            mockClient.Setup(c => c.GetUserProjectsAsync()).Throws(new Exception("whoops"));
            var vm = new ProjectListViewModel(mockClient.Object);
            vm.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "Error" && vm.Error)
                    signal.Set();
            };

            // act
            vm.LoadProjects();

            // assert
            if (!signal.WaitOne(TimeSpan.FromMilliseconds(500)))
                Assert.Fail("timed out waiting for error");
        }

    }
}
