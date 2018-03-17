/*
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;


using RuFramework.RuConfigManager;

namespace Main
{
    public partial class Form1 : Form
    {
        public AppSettings appSettings = null;
  
        public Form1()
        {
            InitializeComponent();

            // Default
            appSettings = RuConfigManager.Open();                       // Default AppDataPath.Roaming

            // Set Location with AppDataPath
            // ******************************************************************************************************
            // appSettings = RuConfigManager.Open(AppDataPath.Common);  // Config path
            //
            //                                   AppDataPath.           // Defined in RuConfigManager
            //                                              .Common
            //                                              .ExePath    // Only PortableApps
            //                                              .Local
            //                                              .Roaming
            // Requires entries in AssemblyInfo.cs
            // [assembly: AssemblyCompany("My Company")]
            // [assembly: AssemblyProduct("My Product")]
            // [assembly: AssemblyFileVersion("1.0.0.0")]
            // ******************************************************************************************************
            
            // User config path
            // appSettings = RuConfigManager.Open(@"D:\My.config");     // User config path

            // Set property
            appSettings.AppNr = 10;         
           
            // Save properties with open selected Path
            RuConfigManager.Save(appSettings);                         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Open property grid
            AppSettingsDialog appSettingsDialog = new AppSettingsDialog(appSettings);
            appSettingsDialog.ShowDialog();

            // Save properties with path used in open
            RuConfigManager.Save(appSettings);
        }
    }
 }
*/

using System;
using System.ComponentModel;
using System.Globalization;
using System.Configuration;
using System.Design;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace RuFramework.RuConfigManager
{
    #region sample Appsettings
    [XmlType(TypeName = "appSettings")]
    public class AppSettings : EventArgs
    {
        /*
        Standard category translated into the national language

        Action      Properties related to available actions.
        Appearance  Properties related to how an entity appears.
        Behavior    Properties related to how an entity acts. 
        Data        Properties related to data and data source management.
        Default     Properties that are grouped in a default category.
        Design      Properties that are available only at design time.
        DragDrop    Properties related to drag-and-drop operations. 
        Focus       Properties related to focus.
        Format      Properties related to formatting.
        Key         Properties related to the keyboard. 
        Layout      Properties related to layout.
        Mouse       Properties related to the mouse. 
        WindowStyle Properties related to the window style of top-level forms.
        */

        [CategoryAttribute("UserTypes")]
        [LocalizedDescription("MyAddress")]                                 // Localized in PropertyGrid.de-DE.resx
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DisplayNameAttribute("MyAddress")]
        [BrowsableAttribute(true)]
        public Address MyAddress { set; get; } = new Address();

        [Category("Focus")]
        [LocalizedDescription("MyAppPath")]                                 // Localized in PropertyGrid.de-DE.resx
        [NotifyParentProperty(true)]
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string AppPath { get; set; } = @"C:\tmp";

        [Category("Data")]
        [LocalizedDescription("MyAppNumber")]                               // Localized in PropertyGrid.de-DE.resx
        [NotifyParentProperty(false)]
        public int AppNr { get; set; } = 5;

        // ********************************************************************************** //
        //  DDDD                                        TT           DD              LL                           
        //  DD  DD                                     TTTT          DD              LL                             
        //  DD   DD     OOOO       NNN    NN    OOOO    TT        DD DD     EEEE     LL                                     
        //  DD    DD   OO  OO      NNUN   NN   OO  OO   TT      DD   DD   EE    EE   LL                                     
        //  DD    DD  OO    OO     NN NN  NN  OO    OO  TT     DD    DD  EE      EE  LL                                  
        //  DD   DD   OO    OO     NN  NN NN  OO    OO  TT     DD    DD  EEEEEEEEE   LL                         
        //  DD  DD     OO  OO      NN   NNNN   OO  OO   TT      DD   DD   EE         LL                              
        //  DDDD        OOOO       NN    NNN    OOOO    TT        DD DD     EEEE     LLLL                            
        // ********************************************************************************** //
        #region Please do not delete the system variable ConfigPath, do not change the value yourself
        [Category("System")]
        [Description("Standard path of the config file")]
        [NotifyParentProperty(true)]
        [Browsable(false)]
        public string ConfigPath { get; set; } = RuConfigManager.GetAppDataPath(AppDataPath.Roaming);

        #endregion
        // ********************************************************************************** //
    }
    #endregion
}
