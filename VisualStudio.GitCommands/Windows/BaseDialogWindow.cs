using Microsoft.VisualStudio.PlatformUI;

namespace VisualStudio.GitCommands.Windows
{
    public class BaseDialogWindow : DialogWindow
    {
        public BaseDialogWindow()
        {
            this.HasMaximizeButton = false;
            this.HasMinimizeButton = false;
        }
    }
}
