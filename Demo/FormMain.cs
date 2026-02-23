using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
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

            var menuGroup = menuTree.AddGroup("Wiki");
            menuGroup.AddDemo("Demo", "", this, () => new FormWikiDemo());

            var creationGroup = menuTree.AddGroup("Creation");
            creationGroup.AddDemo("Fluent API", "", this, () => CreationDemos.FormCreationDemoHost.Run(this, CreationDemos.FluentAPI.Run));

            menuTree.ExpandAllGroups();
        }
    }
}
