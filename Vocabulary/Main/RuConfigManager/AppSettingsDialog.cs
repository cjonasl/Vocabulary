using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RuFramework.RuConfigManager
{
    public partial class AppSettingsDialog : Form
    {
        /// <summary>
        /// Calling PropertyGrid with the appsettings object
        /// </summary>
        /// <param name="appSettings"></param>
        public AppSettingsDialog(AppSettings appSettings)
        {
            InitializeComponent();
            this.propertyGrid1.SelectedObject = appSettings;
        }
    }

}
