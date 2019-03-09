using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisualStudio.GitCommands.Models;
using VisualStudio.GitCommands.Properties;

namespace VisualStudio.GitCommands.Windows
{
    /// <summary>
    /// Interaction logic for CommandsWindow.xaml
    /// </summary>
    public partial class CommandsWindow : BaseDialogWindow
    {
        public string SelectedGitCommand { get; set; }

        public CommandsWindow()
        {
            InitializeComponent();

            PopulateViewListItemFromSettings();

            SelectedGitCommand = null;
        }

        private void OnLoadedEvent(object sender, RoutedEventArgs e)
        {
            ListView listview = (ListView)sender;

            if (listview.ActualWidth > 0)
            {
                var workingWidth = listview.ActualWidth - SystemParameters.VerticalScrollBarWidth; // take into account vertical scrollbar

                ((GridView)listview.View).Columns[0].Width = workingWidth;
                ((GridView)listview.View).Columns[0].Header = "Stored Git Commands";
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string gitCommand = TxtCommand.Text;

            if (String.IsNullOrEmpty(gitCommand)) return;

            CommandListView.Items.Add(gitCommand);
            TxtCommand.Text = String.Empty;

            SaveUserCommands();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = CommandListView.SelectedItem;
            CommandListView.Items.Remove(selectedItem);

            SaveUserCommands();
        }

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            var item = (string)CommandListView.SelectedItem;
            SelectedGitCommand = item;

            this.Close();
        }

        private void PopulateViewListItemFromSettings()
        {
            // Populate list based on user settings
            string currentSettings = Settings.Default.UserCommands;

            if (!String.IsNullOrEmpty(currentSettings))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<string> currentCommandsList = serializer.Deserialize<List<string>>(currentSettings);

                CommandListView.Items.Clear();

                foreach (string item in currentCommandsList)
                {
                    CommandListView.Items.Add(item);
                }
            }
        }

        private void SaveUserCommands()
        {
            // Saves settings based on the gridview
            List<string> currentCommandsView = new List<string>();

            var currentItems = CommandListView.Items;
            foreach (string item in currentItems) currentCommandsView.Add(item);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string currentSettings = serializer.Serialize(currentCommandsView);

            Settings.Default.UserCommands = currentSettings;
            Settings.Default.Save();
        }
    }
}
