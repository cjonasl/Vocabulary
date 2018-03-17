using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;

namespace RuFramework.RuConfigManager
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        /// <summary>
        /// Contains the name of the resource-string
        /// </summary>
        private string rDescription;

        /// <summary>
        /// Creates a new LocalizedDescription Attribute instance
        /// giving it the name of the resource-string
        /// </summary>
        /// <param name="description"></param>
        public LocalizedDescriptionAttribute(string description)
        {
            this.rDescription = description;
        }

        /// <summary>
        /// (Overridden) Get: fetching the description during runtime
        /// from the Resources (with respect to the current culture)
        /// </summary>
        public override string Description
        {
            get
            {
                return Main.RuConfigManager.Resources.PropertyGrid.ResourceManager.GetString(
                        this.rDescription, Thread.CurrentThread.CurrentCulture);
            }
        }

    }
}
