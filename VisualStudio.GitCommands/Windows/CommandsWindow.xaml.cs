using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using VisualStudio.GitCommands.Properties;
using VisualStudio.GitCommands.Static;

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

            CommandListView.MouseDoubleClick += CommandListView_MouseDoubleClick;

            SelectedGitCommand = null;
        }

        private void CommandListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (((System.Windows.Controls.Primitives.Selector)sender).SelectedValue != null)
            {
                string selectedCommand = ((System.Windows.Controls.Primitives.Selector)sender).SelectedValue.ToString();
                SelectedGitCommand = selectedCommand;

                this.Close();
            }
        }

        private void OnLoadedEvent(object sender, RoutedEventArgs e)
        {
            ListView listview = (ListView)sender;

            if (listview.ActualWidth > 0)
            {
                var workingWidth = listview.ActualWidth - SystemParameters.VerticalScrollBarWidth; // take into account vertical scrollbar

                ((GridView)listview.View).Columns[0].Width = workingWidth;
                ((GridView)listview.View).Columns[0].Header = ExtensionConstants.StoredGitCommands;
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string gitCommand = TxtCommand.Text;

            if (String.IsNullOrEmpty(gitCommand)) return;
            if (gitCommand.ContainsIgnoreCase("git")) gitCommand = Regex.Replace(gitCommand, "git", "", RegexOptions.IgnoreCase).Trim();
            if (CommandListView.Items.Contains(gitCommand))
            {
                TxtCommand.Text = String.Empty;
                return;
            }

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
