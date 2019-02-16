﻿using FilterCore;
using FilterCore.FilterComponents.Tier;
using FilterPolishZ.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FilterPolishUtil;
using FilterEconomy.Facades;
using FilterPolishZ.Economy;
using FilterEconomy.Model.ItemInformationData;
using System.Linq;
using FilterEconomy.Model;
using FilterPolishUtil.Collections;
using FilterPolishZ.ModuleWindows.ItemInfo;
using FilterPolishZ.ModuleWindows.Configuration;
using FilterPolishZ.Domain;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using MethodTimer;

namespace FilterPolishZ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public EconomyRequestFacade.RequestType RequestMode { get; set; } = EconomyRequestFacade.RequestType.Dynamic;

        // Components
        public LocalConfiguration Configuration { get; set; } = LocalConfiguration.GetInstance();
        public EconomyRequestFacade EconomyData { get; set; }
        public ItemInformationFacade ItemInfoData { get; set; }
        public List<string> FilterRawString { get; set; }

        public List<KeyValuePair<string, ItemList<NinjaItem>>> UnhandledUniqueItems { get; }

        public MainWindow()
        {
            ConcreteEnrichmentProcedures.Initialize();

            // Initialize Modules
            var filterData = this.PerformFilterWorkAsync();
            this.EconomyData = this.LoadEconomyOverviewData();
            this.ItemInfoData = this.LoadItemInformationOverview();
            var tierListData = this.LoadTierLists(filterData);

            // Initialize 
            var economyTieringSystem = new ConcreteEconomyRules()
            {
                TierInformation = tierListData,
                EconomyInformation = this.EconomyData
            };

            economyTieringSystem.Execute();

            // Initialize Settings
            this.InitializeComponent();
            this.DataContext = new MainWindowViewModel();

            Task.Run(() => WriteFilter(filterData));

        }

        [Time]
        private Filter PerformFilterWorkAsync()
        {
           this.FilterRawString = FileWork.ReadLinesFromFile(Configuration.AppSettings["SeedFile Folder"] + "/" + "NeverSink's filter - SEED (SeedFilter) .filter");
           return new Filter(this.FilterRawString);
        }

        [Time]
        private async Task WriteFilter(Filter filter)
        {
            var result = filter.Serialize();
            var seedFolder = Configuration.AppSettings["SeedFile Folder"];
            await FileWork.WriteTextAsync(seedFolder + "/" + "test" + ".filter", result);
        }

        [Time]
        private Dictionary<string,TierGroup> LoadTierLists(Filter filter)
        {
            var workTiers = new HashSet<string> { "uniques", "divination", "maps->uniques", "currency->fossil", "currency->resonator", "fragments", "currency->prophecy", "rares->shaperbases", "rares->elderbases", "crafting", "currency" };
            var tiers = filter.ExtractTiers(workTiers);
            return tiers;
        }

        [Time]
        private EconomyRequestFacade LoadEconomyOverviewData()
        {
            var result = EconomyRequestFacade.GetInstance();

            var seedfolder = Configuration.AppSettings["SeedFile Folder"];
            var ninjaurl = Configuration.AppSettings["Ninja Request URL"];
            var variation = Configuration.AppSettings["Ninja League"];
            var league = Configuration.AppSettings["betrayal"];

            PerformEcoRequest("divination", "divination", "?");
            PerformEcoRequest("maps->uniques", "uniqueMaps", "?");
            PerformEcoRequest("uniques", "uniqueWeapons", "?");
            PerformEcoRequest("uniques", "uniqueFlasks", "?");
            PerformEcoRequest("uniques", "uniqueArmours", "?");
            PerformEcoRequest("uniques", "uniqueAccessory", "?");
            PerformEcoRequest("basetypes", "basetypes", "&");

            void PerformEcoRequest(string dictionaryKey, string requestKey, string prefix) =>
                result.AddToDictionary(dictionaryKey,
                result.PerformRequest(league, variation, requestKey, prefix, this.RequestMode, seedfolder, ninjaurl));

            return result;
        }

        [Time]
        private ItemInformationFacade LoadItemInformationOverview()
        {
            ItemInformationFacade result = ItemInformationFacade.GetInstance();

            var baseStoragePath = Configuration.AppSettings["SeedFile Folder"];

            var variation = "defaultSorting";

            PerformItemInfoRequest(variation, "divination");
            PerformItemInfoRequest(variation, "uniques");
            PerformItemInfoRequest(variation, "uniqueMaps");
            PerformItemInfoRequest(variation, "basetypes");

            void PerformItemInfoRequest(string loadPath, string requestKey) =>
                result.AddToDictionary(requestKey,
                result.LoadItemInformation(loadPath, requestKey, baseStoragePath));

            return result;
        }

        private void OnScreenTabSelect(object sender, RoutedEventArgs e)
        {
//            GenerationOptions.Vo;
//            this.DataContext = ConfigurationView;
//            GenerationOptions.OpenSc
//            MainBorder.Child = new GenerationOptions();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }
    }
}
