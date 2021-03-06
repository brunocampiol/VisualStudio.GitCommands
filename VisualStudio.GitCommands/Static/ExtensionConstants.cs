﻿namespace VisualStudio.GitCommands.Static
{
    /// <summary>
    /// Represents container for constants.
    /// </summary>
    public static class ExtensionConstants
    {
        public const string StoredGitCommands = "Stored Git Commands";

        public const string PanelName = "Git Commands Output";

        public const string PullOriginDevelopment = "pull origin development";

        public const string PullOriginMaster = "pull origin master";

        public const string UnknownRepositoryErrorMessage = "Select repository to execute commands.";

        public const string UnexpectedErrorMessage = "Unexpected error.";

        public const string UnableFindGitMessage = "git.exe wasn't found. Please, verify that git was installed on your computer.";

        public const string DiffToolErrorMessage = "Can't run vsDiffMerge.exe to compare files. \n" +
                                                   "Please check tool install path: \n" +
                                                   "%visual_studio_install-dir%Common7\\IDE\\CommonExtensions\\Microsoft\\TeamFoundation\\Team Explorer\\vsDiffMerge.exe";
    }
}
