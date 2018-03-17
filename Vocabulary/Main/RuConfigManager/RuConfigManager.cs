using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace RuFramework.RuConfigManager
{
    // AppDataPath for select the save path of config file
    public enum AppDataPath { Local, Roaming, Common, ExePath };
    public static class RuConfigManager
    {
        #region AppDataPath
        /// <summary>
        /// Get Path Local, Roaming, Common or ExePath
        /// </summary>
        /// <param name="appDataPointer"></param>
        /// <returns>pstrFilename</returns>
        public static string GetAppDataPath(AppDataPath appDataPointer)
        {
            string fileName = null;
            try
            {
                switch (appDataPointer)
                {
                    case AppDataPath.Local:
                        // C:\users\UserName\AppData\Local\ProductName\ProductName\ProductVersion\ProductName.Config
                        fileName = Application.LocalUserAppDataPath + "\\" + Application.ProductName + ".config";
                        break;
                    case AppDataPath.Roaming:
                        // C:\users\UserName\AppData\Roaming\ProductName\ProductName\ProductVersion\ProductName.Config
                        fileName = Application.UserAppDataPath + "\\" + Application.ProductName + ".config";
                        break;
                    case AppDataPath.Common:
                        // C:\ProgramData\ProductName\ProductName\ProductVersion\ProductName.Config
                        fileName = Application.CommonAppDataPath + "\\" + Application.ProductName + ".config";
                        break;
                    case AppDataPath.ExePath:
                        // ProductSaveDirectory\ProductName.Config, only PortableApps
                        fileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + Application.ProductName + ".config";
                        break;
                    default: // Roaming
                        fileName = Application.UserAppDataPath + "\\" + Application.ProductName + ".config";
                        break;
                }
                return fileName;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region Open
        /// <summary>
        /// Open AppSettings, default path = AppDataPath.Roaming
        /// </summary>
        /// <returns>AppSettings</returns>
        public static AppSettings Open()
        {
            AppSettings appSettings = new AppSettings();
            // C:\users\UserName\AppData\Roaming\ProductName\ProductName\ProductVersion\ProductName.Config
            string UserPath = GetAppDataPath(AppDataPath.Roaming);
            return ReadConfig(UserPath);
        }
        /// <summary>
        /// Open AppSettings, selected path AppDataPath.Roaming, .Local, .Common, ExePath
        /// </summary>
        /// <param name="appDataPath"></param>
        /// <returns>AppSettings</returns>
        public static AppSettings Open(AppDataPath appDataPath = AppDataPath.Roaming)
        {

            AppSettings appSettings = new AppSettings();

            // C:\users\UserName\AppData\Roaming\ProductName\ProductName\ProductVersion\ProductName.Config
            string UserPath = GetAppDataPath(appDataPath);
            return ReadConfig(UserPath);
        }
        /// <summary>
        /// Open AppSettings with UserPath
        /// </summary>
        /// <param name="UserPath"></param>
        /// <returns>AppSettings</returns>
        public static AppSettings Open(string UserPath = null)
        {
            AppSettings appSettings = new AppSettings();
            if (UserPath == null)
            {
                // C:\users\UserName\AppData\Roaming\ProductName\ProductName\ProductVersion\ProductName.Config
                UserPath = GetAppDataPath(AppDataPath.Roaming);
            }
            return ReadConfig(UserPath);
        }
        #endregion
        #region Save
        /// <summary>
        /// Save with path set in appSettings.ConfigPath
        /// </summary>
        /// <param name="appSettings"></param>
        public static void Save(AppSettings appSettings)
        {
            try
            {
                string fileName = appSettings.ConfigPath;
                Regex r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
                if (r.IsMatch(fileName))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileName))
                    {
                        using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                            serializer.Serialize(xmlWriter, appSettings);
                        }
                        streamWriter.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not write file to disk. Original error: " + ex.Message);
            }
        }
        #endregion
        #region private
        private static AppSettings ReadConfig(string UserPath)
        {
            AppSettings appSettings = new AppSettings();
            try
            {
                Regex r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
                if (r.IsMatch(UserPath))
                {
                    using (FileStream fileStream = new FileStream(UserPath, FileMode.Open))
                    {
                        var serializer = new XmlSerializer(typeof(AppSettings));
                        appSettings = (AppSettings)serializer.Deserialize(fileStream);
                    }
                }
                else
                {
                    MessageBox.Show("Error: Bad finename");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " but is new created!");

            }
            appSettings.ConfigPath = UserPath;
            return appSettings;
        }
        #endregion
    }
}
