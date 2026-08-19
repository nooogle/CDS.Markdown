namespace Demo
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// <param name="args">
        /// Command-line arguments. <c>--wiki</c> and <c>--wiki-no-toolbar</c> jump straight
        /// to <see cref="FormWikiDemo"/> (with the navigation toolbar shown or hidden
        /// respectively); <c>--textbox</c> jumps straight to
        /// <see cref="FormMarkdownTextBoxDemo"/>. All bypass <see cref="FormMain"/>'s menu tree.
        /// This gives UI-automation tests (see UiTests) a reliable entry point that doesn't
        /// depend on automating the third-party CDS.WinFormsMenus MenuTree control.
        /// </param>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            if (args.Contains("--wiki-no-toolbar", StringComparer.OrdinalIgnoreCase))
            {
                Application.Run(new FormWikiDemo(showNavigationToolbar: false));
            }
            else if (args.Contains("--wiki", StringComparer.OrdinalIgnoreCase))
            {
                Application.Run(new FormWikiDemo(showNavigationToolbar: true));
            }
            else if (args.Contains("--textbox", StringComparer.OrdinalIgnoreCase))
            {
                Application.Run(new FormMarkdownTextBoxDemo());
            }
            else
            {
                Application.Run(new FormMain());
            }
        }
    }
}
