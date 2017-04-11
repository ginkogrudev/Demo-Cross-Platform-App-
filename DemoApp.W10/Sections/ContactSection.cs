using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.LocalStorage;
using AppStudio.DataProviders.Menu;
using AppStudio.Uwp;
using System.Linq;

using DemoApp.Navigation;
using DemoApp.ViewModels;

namespace DemoApp.Sections
{
    public class ContactSection : Section<MenuSchema>
    {
		private LocalStorageDataProvider<MenuSchema> _dataProvider;

		public ContactSection()
		{
			_dataProvider = new LocalStorageDataProvider<MenuSchema>();
		}

		public override async Task<IEnumerable<MenuSchema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var config = new LocalStorageDataConfig
            {
                FilePath = "/Assets/Data/Contact.json"
            };
            return await _dataProvider.LoadDataAsync(config, MaxRecords);
        }

        public override async Task<IEnumerable<MenuSchema>> GetNextPageAsync()
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

        public override ListPageConfig<MenuSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<MenuSchema>
                {
                    Title = "Contact",

                    Page = typeof(Pages.ContactListPage),

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Title;						
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.Icon);
                    },
                    DetailNavigation = (item) =>
                    {
                        return NavInfo.FromMenu(item);
                    }
                };
            }
        }

        public override DetailPageConfig<MenuSchema> DetailPage
        {
            get { return null; }
        }
    }
}
