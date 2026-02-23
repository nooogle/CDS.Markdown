using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.CreationDemos;

public partial class FormCreationDemoHost : Form
{
    private string _markdown;

    public static void Run(Form parent, Func<string> creationFunc)
    {
        using var form = new FormCreationDemoHost(creationFunc());
        form.ShowDialog(parent);
    }

    public FormCreationDemoHost(string markdown)
    {
        InitializeComponent();
        _markdown = markdown;
    }

    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);
        await markdownViewer.LoadMarkdownFromStringAsync(_markdown);
    }
}
