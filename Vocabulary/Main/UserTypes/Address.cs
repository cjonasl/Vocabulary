using System;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;

namespace RuFramework.RuConfigManager
{
    // Class Address
    #region UserType
    public class Address
    {
        [Category("Behavior")]
        [LocalizedDescription("AddressFirstName")]                          // Localized in PropertyGrid.de-DE.resx
        [NotifyParentProperty(true)]
        public virtual String Firstname { set; get; } = "MyFirstName";

        [Category("Behavior")]
        [LocalizedDescription("AddressLastName")]                           // Localized in PropertyGrid.de-DE.resx
        [NotifyParentProperty(true)]
        public virtual String Lastname { set; get; } = "MyLastName";

        [Category("Behavior")]                                              // Localized in PropertyGrid.de-DE.resx
        [LocalizedDescription("AddressZipCode")]
        [NotifyParentProperty(true)]
        public virtual String Zipcode { set; get; } = "MyZipCode";

        [Category("Behavior")]
        [LocalizedDescription("AddressCity")]                               // Localized in PropertyGrid.de-DE.resx
        [NotifyParentProperty(true)]
        public virtual String City { set; get; } = "MyCity";

        [Category("Behavior")]
        [LocalizedDescription("AddressStreet")]                             // Localized in PropertyGrid.de-DE.resx
        [NotifyParentProperty(true)]
        public virtual String Street { set; get; } = "MyStreet";
    }
    #endregion
}
