using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using AppStudio.Uwp;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Navigation;
using AppStudio.Uwp.Commands;
using AppStudio.DataProviders;

using AppStudio.DataProviders.Html;
using AppStudio.DataProviders.Rss;
using AppStudio.DataProviders.Menu;
using AppStudio.DataProviders.LocalStorage;
using AppStudio.DataProviders.DynamicStorage;
using DemoApp.Sections;


namespace DemoApp.ViewModels
{
    public class MainViewModel : PageViewModelBase
    {
        public ListViewModel GreetingsTraveler { get; private set; }
        public ListViewModel CardGameProject { get; private set; }
        public ListViewModel SocialMedia { get; private set; }
        public ListViewModel Contact { get; private set; }
        public ListViewModel PlacesToLearnProgramming { get; private set; }
        public ListViewModel Learning { get; private set; }

        public MainViewModel(int visibleItems) : base()
        {
            Title = "Demo App";
            GreetingsTraveler = ViewModelFactory.NewList(new GreetingsTravelerSection(), visibleItems);
            CardGameProject = ViewModelFactory.NewList(new CardGameProjectSection(), visibleItems);
            SocialMedia = ViewModelFactory.NewList(new SocialMediaSection());
            Contact = ViewModelFactory.NewList(new ContactSection());
            PlacesToLearnProgramming = ViewModelFactory.NewList(new PlacesToLearnProgrammingSection(), visibleItems);
            Learning = ViewModelFactory.NewList(new LearningSection(), visibleItems);

            if (GetViewModels().Any(vm => !vm.HasLocalData))
            {
                Actions.Add(new ActionInfo
                {
                    Command = RefreshCommand,
                    Style = ActionKnownStyles.Refresh,
                    Name = "RefreshButton",
                    ActionType = ActionType.Primary
                });
            }
        }

		#region Commands
		public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var refreshDataTasks = GetViewModels()
                        .Where(vm => !vm.HasLocalData).Select(vm => vm.LoadDataAsync(true));

                    await Task.WhenAll(refreshDataTasks);
					LastUpdated = GetViewModels().OrderBy(vm => vm.LastUpdated, OrderType.Descending).FirstOrDefault()?.LastUpdated;
                    OnPropertyChanged("LastUpdated");
                });
            }
        }
		#endregion

        public async Task LoadDataAsync()
        {
            var loadDataTasks = GetViewModels().Select(vm => vm.LoadDataAsync());

            await Task.WhenAll(loadDataTasks);
			LastUpdated = GetViewModels().OrderBy(vm => vm.LastUpdated, OrderType.Descending).FirstOrDefault()?.LastUpdated;
            OnPropertyChanged("LastUpdated");
        }

        private IEnumerable<ListViewModel> GetViewModels()
        {
            yield return GreetingsTraveler;
            yield return CardGameProject;
            yield return SocialMedia;
            yield return Contact;
            yield return PlacesToLearnProgramming;
            yield return Learning;
        }
    }
}
