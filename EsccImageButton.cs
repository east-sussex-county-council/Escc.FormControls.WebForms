using System;

namespace eastsussexgovuk.webservices.FormControls
{
    /// <summary>
    /// ImageButton which uses valid XHTML
    /// </summary>
    /// <remarks>Using the .ImageUrl property in RenderBeginTag makes the page load twice. No idea why.</remarks>
    public class EsccImageButton : System.Web.UI.WebControls.ImageButton
    {
        /// <summary>
        /// Creates an ImageButton which uses valid XHTML
        /// </summary>
        public EsccImageButton()
            : base()
        {
        }

        /// <summary>
        /// Write out an &lt;input&gt; element which uses only valid attributes
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<input type=\"image\" src=\"{0}\" name=\"{1}\" id=\"{2}\" alt=\"{3}\"", this.ImageUrl, this.UniqueID, this.ClientID, this.AlternateText);
            if (this.CssClass != null && this.CssClass.Length > 0) writer.Write(" class=\"{0}\"", this.CssClass);
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
