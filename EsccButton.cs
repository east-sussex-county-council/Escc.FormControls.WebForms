using System;

namespace eastsussexgovuk.webservices.FormControls
{
    /// <summary>
    /// Button which uses valid XHTML
    /// </summary>
    public class EsccButton : System.Web.UI.WebControls.Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EsccButton"/> class.
        /// </summary>
        public EsccButton()
            : base()
        {
        }

        /// <summary>
        /// Write out an &lt;input&gt; element which uses only valid attributes
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<input type=\"submit\" value=\"{0}\" name=\"{1}\" id=\"{2}\"", this.Text, this.UniqueID, this.ClientID);
            if (this.CssClass != null && this.CssClass.Length > 0) writer.Write(" class=\"{0}\"", this.CssClass);
            if (this.Attributes["disabled"] != null) writer.Write(" disabled=\"{0}\"", this.Attributes["disabled"]);
            writer.Write(" />");
        }


        /// <summary>
        /// Override to prevent an error due to override of <c>RenderBeginTag</c>
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(System.Web.UI.HtmlTextWriter writer)
        {
        }


    }
}
