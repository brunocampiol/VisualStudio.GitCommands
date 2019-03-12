using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualStudio.GitCommands.Models;
using VisualStudio.GitCommands.Static;

namespace VisualStudio.GitCommands.GitHelpers
{
    public class GitCommandExecuter
    {
        private IGitExt _gitService;

        public GitCommandExecuter(IGitExt gitService)
        {
            _gitService = gitService;
        }

        public GitCommandResult Execute(string gitCommand)
        {
            try
            {
                var activeRepository = _gitService.ActiveRepositories.FirstOrDefault();
                if (activeRepository == null)
                    return new GitCommandResult { ErrorMessage = ExtensionConstants.UnknownRepositoryErrorMessage };

                var gitExePath = GitPathHelper.GetGitPath();

                var gitStartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    FileName = (gitExePath ?? "git.exe"),
                    Arguments = gitCommand,
                    WorkingDirectory = activeRepository.RepositoryPath
                };

                using (var gitProcess = Process.Start(gitStartInfo))
                {
                    var errorMessage = Task.Run(() => gitProcess.StandardError.ReadToEndAsync());
                    var outputMessage = Task.Run(() => gitProcess.StandardOutput.ReadToEndAsync());

                    gitProcess.WaitForExit();

                    return new GitCommandResult
                    {
                        OutputMessage = outputMessage.Result,
                        ErrorMessage = errorMessage.Result
                    };
                }
            }
            catch (Win32Exception e)
            {
                if (e.TargetSite.Name != "StartWithCreateProcess") throw;

                return new GitCommandResult { ErrorMessage = ExtensionConstants.UnableFindGitMessage };
            }
            catch (Exception e)
            {
                return new GitCommandResult { ErrorMessage = ExtensionConstants.UnexpectedErrorMessage + Environment.NewLine + e.AllExceptionMessages() };
            }
        }
    }
}
