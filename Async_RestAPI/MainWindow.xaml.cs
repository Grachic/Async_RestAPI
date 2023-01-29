using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Async_RestAPI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<string> PrepareLoadSites()
        {
            List<string> sites = new List<string>()
            {
                "https://google.com",
                "https://my.progtime.net"
            };

            return sites;
        }

        private void PrintInfo(DataModel dataModel)
        {
            textBlockInfo.Text += $"\nUrl: {dataModel.Url}, Length: {dataModel.Data.Length}";
        }

        public DataModel LoadSite(string site)
        {
            DataModel dataModel = new DataModel();

            dataModel.Url = site;

            WebClient webClient = new WebClient();
            dataModel.Data = webClient.DownloadString(site);
            
            // обновление дизайна из другого потока
            Dispatcher.BeginInvoke((Action) (() => {
                textBlockInfo.Text = "Downloaded";
            }));

            return dataModel;
        }
        
        

        private void ButtonSync_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            LoadDataSync();

            var time = stopwatch.ElapsedMilliseconds;

            textBlockInfo.Text += $"\n\nTotal time: {time}";
        }

        public void LoadDataSync()
        {
            List<string> sites = PrepareLoadSites();

            foreach (var site in sites)
            {
                var dataModel = LoadSite(site);
                PrintInfo(dataModel);
            }
        }


        private async void ButtonAsync_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            //await LoadDataAsync();
            await LoadDataAsyncParallel();

            var time = stopwatch.ElapsedMilliseconds;

            textBlockInfo.Text += $"\n\nTotal time: {time}";
        }

        public async Task LoadDataAsync()
        {
            List<string> sites = PrepareLoadSites();

            foreach (var site in sites)
            {
                DataModel dataModel = await Task.Run(() => LoadSite(site));  // lock
                PrintInfo(dataModel);
            }
        }

        public async Task LoadDataAsyncParallel()
        {
            List<string> sites = PrepareLoadSites();

            List<Task<DataModel>> tasks = new List<Task<DataModel>>();

            foreach (var site in sites)
            {
                tasks.Add(Task.Run(() => LoadSite(site)));
            }

            DataModel[] dataModels =  await Task.WhenAll(tasks);

            foreach (var dataModel in dataModels)
            {
                PrintInfo(dataModel);
            }

            textBlockInfo.Text = "";
        }
    }
}