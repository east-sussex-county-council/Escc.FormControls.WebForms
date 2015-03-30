using System.Web.UI;
using Escc.FormControls.WebForms.Properties;

namespace Escc.FormControls.WebForms
{
    /// <summary>
    /// Standard notice for forms about the Data Protection Act
    /// </summary>
    public class DataProtectionNotice : System.Web.UI.WebControls.WebControl
    {
        /// <summary>
        /// Creates a standard notice for forms about the Data Protection Act
        /// </summary>
        public DataProtectionNotice()
            : base("p")
        {
        }

        /// <summary>
        /// Build the paragraph and assing it a CSS class
        /// </summary>
        protected override void CreateChildControls()
        {
            this.CssClass = "dataProtection";
            this.Controls.Add(new LiteralControl(LocalisedResourceReader.ResourceString(typeof(EsccWebTeam_FormControls).Name, "DataProtectionText", EsccWebTeam_FormControls.DataProtectionText)));
            base.CreateChildControls();
        }

    }
}
