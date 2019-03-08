using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualStudio.GitCommands.Models
{
    public class GitCommandResult
    {
        /// <summary>
        /// Message after git command execution.
        /// </summary>
        public string OutputMessage { get; set; }

        /// <summary>
        /// Error message after git command execution.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Flag indicates whether or not git command was finished with errors.
        /// </summary>
        public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
    }
}
