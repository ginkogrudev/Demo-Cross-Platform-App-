using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.LocalStorage;
using AppStudio.Uwp;
using Windows.ApplicationModel.Appointments;
using System.Linq;

using DemoApp.Navigation;
using DemoApp.ViewModels;

namespace DemoApp.Sections
{
    public class PlacesToLearnProgrammingSection : Section<PlacesToLearnProgramming1Schema>
    {
		private LocalStorageDataProvider<PlacesToLearnProgramming1Schema> _dataProvider;

		public PlacesToLearnProgrammingSection()
		{
			_dataProvider = new LocalStorageDataProvider<PlacesToLearnProgramming1Schema>();
		}

		public override async Task<IEnumerable<PlacesToLearnProgramming1Schema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/Data/PlacesToLearnProgramming.json",
				OrderBy = "Learn",
				OrderDirection = SortDirection.Descending
            };
            return await _dataProvider.LoadDataAsync(config, MaxRecords);
        }

        public override async Task<IEnumerable<PlacesToLearnProgramming1Schema>> GetNextPageAsync()
        {
            return await _dataProvider.LoadMoreDataAsync();
        }

        public override bool HasMorePages
        {
            get
            {
                return _dataProvider.HasMoreItems;
            }
        }

        public override bool NeedsNetwork
        {
            get
            {
                return false;
            }
        }

        public override ListPageConfig<PlacesToLearnProgramming1Schema> ListPage
        {
            get 
            {
                return new ListPageConfig<PlacesToLearnProgramming1Schema>
                {
                    Title = "Places to learn programming ",

                    Page = typeof(Pages.PlacesToLearnProgrammingListPage),

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Learn.ToSafeString();
                        viewModel.SubTitle = item.Discription.ToSafeString();
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.Logo.ToSafeString());
                    },
                    DetailNavigation = (item) =>
                    {
						return NavInfo.FromPage<Pages.PlacesToLearnProgrammingDetailPage>(true);
                    }
                };
            }
        }

        public override DetailPageConfig<PlacesToLearnProgramming1Schema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, PlacesToLearnProgramming1Schema>>();
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.Discription.ToSafeString();
                    viewModel.Title = item.Learn.ToSafeString();
                    viewModel.Description = item.Discription.ToSafeString();
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.Logo.ToSafeString());
                    viewModel.Content = null;
                });

                var actions = new List<ActionConfig<PlacesToLearnProgramming1Schema>>
                {
                };

                return new DetailPageConfig<PlacesToLearnProgramming1Schema>
                {
                    Title = "Places to learn programming ",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
    }
}
