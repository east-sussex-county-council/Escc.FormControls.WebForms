using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace eastsussexgovuk.webservices.FormControls
{
	/// <summary>
	/// Presentation container for a form label and its associated CheckBox control
	/// </summary>
	[ToolboxData("<{0}:FormPart runat=server></{0}:FormPart>")]
	public class CheckBoxFormPart : FormPart
	{
		#region Fields

		private CheckBoxPartStyle partStyle = CheckBoxPartStyle.AlignedWithControls;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Presentation container for a form label and its associated control
		/// </summary>
		public CheckBoxFormPart() : base() 
		{
		}


		/// <summary>
		/// Presentation container for a checkbox
		/// </summary>
		/// <param name="checkbox">Form control</param>
		public CheckBoxFormPart(CheckBox checkbox) : base() 
		{
			checkbox.CssClass = (checkbox.CssClass.Length > 0) ? checkbox.CssClass += " " + this.ControlCssClass : this.ControlCssClass;

			// add controls
			this.Controls.Add(checkbox);
		}

		/// <summary>
		/// Presentation container for a checkbox
		/// </summary>
		/// <param name="checkbox">Form control</param>
		/// <param name="style">Display mode</param>
		public CheckBoxFormPart(CheckBox checkbox, CheckBoxPartStyle style) : base() 
		{
			checkbox.CssClass = (checkbox.CssClass.Length > 0) ? checkbox.CssClass += " " + this.ControlCssClass : this.ControlCssClass;
			this.partStyle = style;

			// add controls
			this.Controls.Add(checkbox);

		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the display mode
		/// </summary>
		public CheckBoxPartStyle PartStyle
		{
			get
			{
				return this.partStyle;
			}
			set
			{
				this.partStyle = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Build the controls
		/// </summary>
		protected override void CreateChildControls()
		{
			// set style
			switch (this.partStyle)
			{
				case CheckBoxPartStyle.Wide:
					this.CssClass = "checkboxFormPart";
					break;
				default:
					this.CssClass = this.DefaultCssClass;
					break;
			}

			base.CreateChildControls ();
		}

		#endregion

	}

	#region Dependent enum types

	/// <summary>
	/// Display modes for a CheckBoxFormPart
	/// </summary>
	public enum CheckBoxPartStyle
	{
		/// <summary>
		/// Checkbox is aligned with other form controls
		/// </summary>
		AlignedWithControls,
		
		/// <summary>
		/// Checkbox takes up the whole width of the form
		/// </summary>
		Wide
	}

	#endregion
}
