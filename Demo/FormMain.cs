namespace Demo;

public partial class FormMain : Form
{
    public FormMain()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        labelInfo.Text = SystemInfoHelper.GetSystemInfo();

        var menuGroup = menuTree.API.AddGroup("Wiki");
        menuGroup.AddItem("Demo", "", this, () => new FormWikiDemo());

        var creationGroup = menuTree.AddGroup("Creation");
        creationGroup.AddItem("Fluent API", "", () => CreationDemos.FormCreationDemoHost.Run(this, CreationDemos.FluentAPI.Run));
        creationGroup.AddItem("Builder API", "", () => CreationDemos.FormCreationDemoHost.Run(this, CreationDemos.BuilderAPI.Run));

        menuTree.API.ExpandAllGroups();
        menuTree.API.MouseActivation = CDS.WinFormsMenus.Basic.MouseActivation.SingleClick;
    }
}
