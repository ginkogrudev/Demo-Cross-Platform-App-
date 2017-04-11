using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.DynamicStorage;
using AppStudio.Uwp;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using Windows.ApplicationModel.Appointments;
using System.Linq;
using Windows.Storage;

using DemoApp.Navigation;
using DemoApp.ViewModels;

namespace DemoApp.Sections
{
    public class LearningSection : Section<Learning1Schema>
    {
		private DynamicStorageDataProvider<Learning1Schema> _dataProvider;		

		public LearningSection()
		{
			_dataProvider = new DynamicStorageDataProvider<Learning1Schema>();
		}

		public override async Task<IEnumerable<Learning1Schema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var config = new DynamicStorageDataConfig
            {
                Url = new Uri("http://ds.winappstudio.com/api/data/collection?dataRowListId=fa57f726-f6d6-4ff1-a09e-506b090e5a5a&appId=d042324a-6aed-4021-85ce-6bd02a90a155"),
                AppId = "d042324a-6aed-4021-85ce-6bd02a90a155",
                StoreId = ApplicationData.Current.LocalSettings.Values[LocalSettingNames.StoreId] as string,
                DeviceType = ApplicationData.Current.LocalSettings.Values[LocalSettingNames.DeviceType] as string,
            };
            return await _dataProvider.LoadDataAsync(config, MaxRecords);
        }

        public override async Task<IEnumerable<Learning1Schema>> GetNextPageAsync()
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

        public override ListPageConfig<Learning1Schema> ListPage
        {
            get 
            {
                return new ListPageConfig<Learning1Schema>
                {
                    Title = "Learning ",

                    Page = typeof(Pages.LearningListPage),

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.ProgrammingLanguage.ToSafeString();
                        viewModel.SubTitle = item.Technology.ToSafeString();
                        viewModel.Description = item.Technology.ToSafeString();
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.img.ToSafeString());
                    },
                    DetailNavigation = (item) =>
                    {
						return NavInfo.FromPage<Pages.LearningDetailPage>(true);
                    }
                };
            }
        }

        public override DetailPageConfig<Learning1Schema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, Learning1Schema>>();
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.ProgrammingLanguage.ToSafeString();
                    viewModel.Title = item.Technology.ToSafeString();
                    viewModel.Description = "";
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.img.ToSafeString());
                    viewModel.Content = null;
                });
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = "More Information";
                    viewModel.Title = item.Description.ToSafeString();
                    viewModel.Description = item.Resources.ToSafeString();
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl("");
                    viewModel.Content = null;
                });

                var actions = new List<ActionConfig<Learning1Schema>>
                {
                };

                return new DetailPageConfig<Learning1Schema>
                {
                    Title = "Learning ",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
    }
}
