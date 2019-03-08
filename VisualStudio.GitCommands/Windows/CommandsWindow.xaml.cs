using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
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

            SelectedGitCommand = null;
        }

        private void OnLoadedEvent(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PopulateViewListItemFromSettings()
        {
            //// Populate list
            //string currentSettings = Settings.Default.UserSettings;

            //if (!String.IsNullOrEmpty(currentSettings))
            //{
            //    List<ViewListItem> currentCommandsList = JsonConvert.DeserializeObject<List<ViewListItem>>(currentSettings);

            //    CommandListView.Items.Clear();

            //    foreach (ViewListItem item in currentCommandsList)
            //    {
            //        CommandListView.Items.Add(item);
            //    }
            //}
        }

        private void SaveUserCommands()
        {
            //// Saves settings based on the gridview

            //List<ViewListItem> currentCommandsView = new List<ViewListItem>();

            //var currentItems = CommandListView.Items;
            //foreach (ViewListItem item in currentItems) currentCommandsView.Add(item);

            //string currentSettings = JsonConvert.SerializeObject(currentCommandsView);

            //Settings.Default.UserSettings = currentSettings;
            //Settings.Default.Save();
        }
    }
}
