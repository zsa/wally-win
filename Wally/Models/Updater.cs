using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Http;
using System.Net.Http.Headers;
using UtilsLibrary;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Wally.Models
{
    public class Updater
    {
        private const string baseUrl = "https://configure.ergodox-ez.com/";

        private string _currentVersion;

        public UpdateViewModel Update { get; set; }

        public Updater(string currentVersion)
        {
            _currentVersion = currentVersion;
            var settings = Properties.Settings.Default;
            if (settings.UpgradeRequired)
            {
                settings.Upgrade();
                settings.UpgradeRequired = false;
                settings.Save();
            }
            var firstExecution = settings.FirstExecution;
            if (firstExecution == true)
            {
                MessageBoxResult result = MessageBox.Show("Would you like to check for updates on startup?", "Wally updates", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    settings.CheckForUpdates = true;
                }
                else
                {
                    settings.CheckForUpdates = false;
                }
                settings.FirstExecution = false;
                settings.Save();
            }
            var checkForUpdates = settings.CheckForUpdates;
            if (checkForUpdates == true)
            {
                fetchUpdate();
            }
        }
        private async Task fetchUpdate()
        {
            var logger = Logger.Instance();
            logger.Log(LogSeverity.Info, "Checking for updates.");
            HttpClient fetch = new HttpClient();
            fetch.BaseAddress = new Uri(baseUrl);
            fetch.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage res = await fetch.GetAsync("wally-win.json");
                var data = await res.Content.ReadAsStringAsync();
                var update = JsonConvert.DeserializeObject<UpdateViewModel>(data);
                if(_currentVersion != update.Version)
                {
                    logger.Log(LogSeverity.Info, "A new version of Wally is available.");
                    var mainWindow = (MainWindow)App.Current.MainWindow;
                    mainWindow.ShowUpdateDialog(update);
                }
                else
                {
                    logger.Log(LogSeverity.Info, "Wally is up to date.");
                }
            }
            catch (Exception e)
            {
            logger.Log(LogSeverity.Warning, $"Error while checking for updates: {e.Message}");
            }
        }
    }
}
