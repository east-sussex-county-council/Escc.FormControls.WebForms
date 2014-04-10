using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using eastsussexgovuk.webservices.TextXhtml.HouseStyle;

namespace eastsussexgovuk.webservices.FormControls.CustomControls
{
	/// <summary>
	/// Summary description for EsccDeclarationOfInterest.
	/// </summary>
	public class EsccDeclarationOfInterest  : WebControl, INamingContainer
	{


		
		private TextBox txbFullname;
		private string fullnameText;
		private Label lblFullname;
		private string fullnameLabelCssStyle;
	    private string refText;

//Removed 08/06/2006 because webcontrol allows inheritance of tooltip property
//	private string toolTip;
//
//		/// <summary>
//		/// Property ToolTip (string)
//		/// </summary>
//		public string ToolTip
//		{
//			get
//			{
//				return this.toolTip;
//			}
//			set
//			{
//				this.toolTip = value;
//			}
//		}

		
		
		/// <summary>
		/// Property Ref (string)
		/// </summary>
		public string RefText
		{
			get
			{
				return this.refText;
			}
			set
			{
				this.refText = value;
			}
		}
		
		
		
		private string declarationStatement;
		private HtmlGenericControl DecStatement;

		private Label lblDeclarationDate;
		private string fullnameCssStyle;
		
		/// <summary>
		/// Property FullnameCssStyle (string)
		/// </summary>
		public string FullnameCssStyle
		{
			get
			{
				return this.fullnameCssStyle;
			}
			set
			{
				this.fullnameCssStyle = value;
			}
		}

		/// <summary>
		/// Property FullnameText (string)
		/// </summary>
		public string FullnameText
		{
			get
			{
				return this.fullnameText;
			}
			set
			{
				this.fullnameText = value;
			}
		}
		/// <summary>
		/// Property DeclarationStatement (string)
		/// </summary>
		public string DeclarationStatement
		{
			get
			{
				return this.declarationStatement;
			}
			set
			{
				this.declarationStatement = value;
			}
		}
		/// <summary>
		/// Property FullnameLabelCssStyle (string)
		/// </summary>
		public string FullnameLabelCssStyle
		{
			get
			{
				return this.fullnameLabelCssStyle;
			}
			set
			{
				this.fullnameLabelCssStyle = value;
			}
		}

		private RequiredFieldValidator requiredFieldValidator;
		private bool required;
		
		/// <summary>
		/// Property Required (bool)
		/// </summary>
		public bool Required
		{
			get
			{
				return this.required;
			}
			set
			{
				this.required = value;
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="EsccDeclarationOfInterest"/> class.
        /// </summary>
		public EsccDeclarationOfInterest():base (HtmlTextWriterTag.Div)
		{
			
		}


        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
		protected override void CreateChildControls()
		{
			this.EnsureChildControls();
			
			DecStatement = new HtmlGenericControl("p");
			DecStatement.InnerText = declarationStatement;
			
			DecStatement.Attributes.Add("class","spaceDeclaration");
			
			

			txbFullname = new TextBox();
			txbFullname.CssClass =  fullnameCssStyle;
			txbFullname.ID = "txbDeclaration";
			txbFullname.ToolTip = this.ToolTip;

			
			lblFullname = new Label();
			lblFullname.CssClass = fullnameLabelCssStyle;
			lblFullname.AssociatedControlID = "txbDeclaration";

			if (required)
			{
				lblFullname.Text = refText + " " + "Full name" + "<span class=\"requiredField\">*</span>";
			}
			else
			{
				lblFullname.Text = refText + " " + "Full name";
			}
		

			if (required)
			{
				requiredFieldValidator =  new RequiredFieldValidator();
				requiredFieldValidator.Display = ValidatorDisplay.None;
				requiredFieldValidator.ControlToValidate = "txbDeclaration";
				requiredFieldValidator.EnableClientScript= false;
				requiredFieldValidator.ErrorMessage = "question " + refText.Replace(".", "") + " must be completed";
				this.txbFullname.Controls.Add(requiredFieldValidator);
				
			}

            HtmlGenericControl lblDateofDeclaration = new HtmlGenericControl("p");
			
			lblDateofDeclaration.InnerText = "Date of declaration - " + DateTimeFormatter.FullBritishDate(DateTime.Now);


			this.Controls.Add(DecStatement);
			this.Controls.Add(lblFullname);
			this.Controls.Add(txbFullname);
			this.Controls.Add(lblDateofDeclaration);
			
			
		

			

		}

        /// <summary>
        /// Do not render the default HTML opening tag of the control.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"></see> that represents the output stream to render HTML content on the client.</param>
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			
		}

        /// <summary>
        /// Do not render the default HTML closing tag of the control.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"></see> that represents the output stream to render HTML content on the client.</param>
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			
		}


	}
}
