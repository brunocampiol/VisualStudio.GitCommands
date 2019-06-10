using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using System;
using System.ComponentModel.Design;
using VisualStudio.GitCommands.GitHelpers;
using VisualStudio.GitCommands.Models;
using VisualStudio.GitCommands.Static;
using Task = System.Threading.Tasks.Task;

namespace VisualStudio.GitCommands
{
    internal sealed class GitOriginMaster
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0103;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("67d49f0f-8005-4955-930c-a84f3760061d");
        //public static readonly Guid CommandSet = new Guid("b7ae99f4-b2b2-4ebd-a482-e5fb6c4cdb27");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        private readonly IGitExt _gitService;

        private readonly IVsOutputWindowPane _vsOutputWindowPane;

        private readonly DTE _dteVsCoreAutomation;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitCommands"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private GitOriginMaster(AsyncPackage package,
                                        OleMenuCommandService commandService,
                                        IGitExt gitService,
                                        IVsOutputWindowPane vsOutputWindow,
                                        DTE dteVsCoreAutomation)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            _gitService = gitService ?? throw new ArgumentNullException(nameof(gitService));
            _vsOutputWindowPane = vsOutputWindow ?? throw new ArgumentNullException(nameof(vsOutputWindow));
            _dteVsCoreAutomation = dteVsCoreAutomation ?? throw new ArgumentNullException(nameof(dteVsCoreAutomation));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static GitOriginMaster Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in GitCommands's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            IGitExt gitService = await package.GetServiceAsync((typeof(IGitExt))) as IGitExt;
            DTE vsCoreAutomation = await package.GetServiceAsync((typeof(DTE))) as DTE;
            IVsOutputWindow outputWindow = await package.GetServiceAsync((typeof(IVsOutputWindow))) as IVsOutputWindow;

            // Instantiates VS output window
            var paneGuid = Microsoft.VisualStudio.VSConstants.OutputWindowPaneGuid.GeneralPane_guid;
            var paneName = ExtensionConstants.PanelName;
            IVsOutputWindowPane vsoutputwindow;
            outputWindow.CreatePane(paneGuid, paneName, 1, 0);
            outputWindow.GetPane(paneGuid, out vsoutputwindow);

            Instance = new GitOriginMaster(package, commandService, gitService, vsoutputwindow, vsCoreAutomation);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            GitCommandExecuter gitExecuter = new GitCommandExecuter(_gitService);
            GitCommandResult result = gitExecuter.Execute(ExtensionConstants.PullOriginMaster);

            BringPanelToFront();

            WriteLineToOutputWindow("########################################");
            if (result.IsError)
            {
                WriteLineToOutputWindow($"Git Error - at {DateTime.Now}");
                WriteLineToOutputWindow(result.ErrorMessage);
            }
            else
            {
                WriteLineToOutputWindow($"Git Command OK - at {DateTime.Now}");
            }
            WriteLineToOutputWindow("########################################");
            WriteLineToOutputWindow(String.Empty);

            WriteLineToOutputWindow(result.OutputMessage);
            WriteLineToOutputWindow(String.Empty);
            WriteLineToOutputWindow(String.Empty);
        }

        private void WriteLineToOutputWindow(string text)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _vsOutputWindowPane.OutputString(text + Environment.NewLine);
        }

        private void BringPanelToFront()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _dteVsCoreAutomation.ExecuteCommand("View.Output");
        }
    }
}
