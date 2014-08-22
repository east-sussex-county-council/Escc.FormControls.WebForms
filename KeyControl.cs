using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EsccWebTeam.FormControls.Properties;

namespace EsccWebTeam.FormControls
{
    /// <summary>
    /// Standard key for markings used on forms
    /// </summary>
    public class KeyControl : PlaceHolder
    {
        /// <summary>
        /// Build controls for key
        /// </summary>
        protected override void CreateChildControls()
        {
            var reqPara = new HtmlGenericControl("p");
            reqPara.Controls.Add(new RequiredFieldIndicator());
            reqPara.Controls.Add(new LiteralControl(EsccWebTeam_FormControls.RequiredFieldIndicatorHelp));
            this.Controls.Add(reqPara);
        }

    }
}
