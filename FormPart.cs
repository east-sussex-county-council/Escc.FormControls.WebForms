#region Using Directives
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;
using EsccWebTeam.FormControls;
#endregion

namespace eastsussexgovuk.webservices.FormControls
{
	/// <summary>
	/// Presentation container for a form label and its associated control
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:FormPart runat=server></{0}:FormPart>")]
	public class FormPart : System.Web.UI.WebControls.WebControl
    {
        #region Declarations
        private string partCss = "formPart";
        private string labelCss = "formLabel";
        private string controlCss = "formControl";
		private Label labelControl = null;
		private HtmlGenericControl legendControl = null;
		private bool required;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the default CSS class for the FormPart
        /// </summary>
        /// <value>The default CSS class.</value>
        public string DefaultCssClass
        {
            get { return partCss; }
            set { partCss = value; }
        }

        /// <summary>
        /// Gets or sets the CSS class for the label.
        /// </summary>
        /// <value>The label CSS class.</value>
        public string LabelCssClass
        {
            get { return labelCss; }
            set { labelCss = value; }
        }

        /// <summary>
        /// Gets or sets the CSS class for the form control
        /// </summary>
        /// <value>The control CSS class.</value>
        public string ControlCssClass
        {
            get { return controlCss; }
            set { controlCss = value; }
        }
        
        /// <summary>
		/// Gets or sets a value indicating whether this <see cref="FormPart"/> is required.
		/// </summary>
		/// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
		public bool Required
		{
			get { return this.required; }
			set { this.required = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
		/// Presentation container for a form label and its associated control
		/// </summary>
		public FormPart() : base(HtmlTextWriterTag.Div) 
		{
			// set style
			this.CssClass = this.partCss;
		}

        /// <summary>
        /// Presentation container for a form label and its associated control
        /// </summary>
        /// <param name="label">Text to be used for form control label</param>
        /// <param name="webControl">Form control</param>
        public FormPart(string label, WebControl webControl)
            : base(HtmlTextWriterTag.Div)
        {
            // Call the generic initialise method
            this.initialiseGenericFormPart(label, webControl);
        }

        /// <summary>
        /// Presentation container for a form label and its associated control
        /// </summary>
        /// <param name="label">Text to be used for form control label</param>
        /// <param name="webControl">Form control</param>
        public FormPart(Label label, WebControl webControl)
            : base(HtmlTextWriterTag.Div)
        {
            // Call the generic initialise method
            this.initialiseGenericFormPart(label, webControl);
        }

		/// <summary>
		/// Presentation container for a form label and its associated control
		/// </summary>
		/// <param name="label">Label for the form control</param>
		/// <param name="textbox">Form control</param>
		public FormPart(Label label, TextBox textbox) : base(HtmlTextWriterTag.Div) 
		{
            // Call the generic initialise method
            this.initialiseGenericFormPart(label, textbox);
        }

		/// <summary>
		/// Presentation container for a form label and its associated control
		/// </summary>
		/// <param name="label">Text to be used for form control label</param>
		/// <param name="textbox">Form control</param>
		public FormPart(string label, TextBox textbox) : base(HtmlTextWriterTag.Div) 
		{
            // Call the generic initialise method
            this.initialiseGenericFormPart(label, textbox);
        }

		/// <summary>
		/// Presentation container for a form label and its associated control
		/// </summary>
		/// <param name="label">Text to be used for form control label</param>
		/// <param name="textbox">Form control</param>
		/// <param name="useContainer">Surround TextBox with a div, allowing more flexible styling</param>
		public FormPart(string label, TextBox textbox, bool useContainer) : base(HtmlTextWriterTag.Div) 
		{
			// configure controls
			this.labelControl = new Label();
			labelControl.Text = label;
			labelControl.CssClass = this.labelCss;
			labelControl.AssociatedControlID = textbox.ID;
			this.Controls.Add(labelControl);
			
			if (useContainer)
			{
				HtmlGenericControl div = new HtmlGenericControl("div");
				div.Attributes["class"] = this.controlCss;
				div.Controls.Add(textbox);
				this.Controls.Add(div);
			}
			else
			{
				textbox.CssClass = (textbox.CssClass.Length > 0) ? textbox.CssClass += " " + this.controlCss : this.controlCss;
				this.Controls.Add(textbox);
			}

			// set style
			this.CssClass = this.partCss;
		}

		/// <summary>
		/// Presentation container for a form label and its associated control
		/// </summary>
		/// <param name="label">Label for the form control</param>
		/// <param name="dropDownList">Form control</param>
		public FormPart(Label label, DropDownList dropDownList) : base(HtmlTextWriterTag.Div) 
		{
            // Call the generic initialise method
            this.initialiseGenericFormPart(label, dropDownList);
        }

		/// <summary>
		/// Presentation container for a form label and its associated control
		/// </summary>
		/// <param name="label">Text to be used for form control label</param>
		/// <param name="dropDownList">Form control</param>
		public FormPart(string label, DropDownList dropDownList) : base(HtmlTextWriterTag.Div) 
		{
            // Call the generic initialise method
            this.initialiseGenericFormPart(label, dropDownList);
        }

        /// <summary>
        /// Presentation container for a form label and its associated control
        /// </summary>
        /// <param name="label">Text to be used for form control label</param>
        /// <param name="listBox">Form control</param>
        public FormPart(string label, ListBox listBox)
            : base(HtmlTextWriterTag.Div)
        {
            // Call the generic initialise method
            this.initialiseGenericFormPart(label, listBox);
        }

		/// <summary>
		/// Presentation container for a form label and its associated control
		/// </summary>
		/// <param name="label">Label for the form control</param>
		/// <param name="controlContainer">XHTML container (usually a <div></div> element) with an attached form control</param>
		public FormPart(Label label, HtmlGenericControl controlContainer) : base(HtmlTextWriterTag.Div) 
		{
			// configure controls
			label.CssClass = (label.CssClass.Length > 0) ? label.CssClass += " " + this.labelCss : this.labelCss;
			controlContainer.Attributes.Add("class", (controlContainer.Attributes["class"] != null) ? controlContainer.Attributes["class"] += " " + this.controlCss : this.controlCss);
			if (controlContainer.Controls.Count > 0) label.AssociatedControlID = controlContainer.Controls[0].ID;

			// add controls
			this.labelControl = label;
			this.Controls.Add(label);
			this.Controls.Add(controlContainer);

			// set style
			this.CssClass = this.partCss;
		}

		/// <summary>
		/// Presentation container for a form label and its associated control
		/// </summary>
		/// <param name="label">Text to be used for form control label</param>
		/// <param name="controlContainer">XHTML container (usually a <div></div> element) with an attached form control</param>
		public FormPart(string label, HtmlGenericControl controlContainer) : base(HtmlTextWriterTag.Div) 
		{
			// configure controls
			this.labelControl = new Label();
			labelControl.Text = label;

			labelControl.CssClass = this.labelCss;
			controlContainer.Attributes.Add("class", (controlContainer.Attributes["class"] != null) ? controlContainer.Attributes["class"] += " " + this.controlCss : this.controlCss);
			if (controlContainer.Controls.Count > 0) labelControl.AssociatedControlID = controlContainer.Controls[0].ID;

			// add controls
			this.Controls.Add(labelControl);
			this.Controls.Add(controlContainer);

			// set style
			this.CssClass = this.partCss;
		}

		
		/// <summary>
		/// Presentation container for a set of radio buttons
		/// </summary>
		/// <param name="label">Text to label the group of buttons</param>
		/// <param name="groupName">GroupName for the radio buttons</param>
		/// <param name="radioButtons">The RadioButtons in the group</param>
		/// <remarks>Supplied radio buttons should have their text and value already set</remarks>
		public FormPart(string label, string groupName, params RadioButton[] radioButtons) : base(HtmlTextWriterTag.Fieldset)
		{
			// this overload is a little different - it takes a set of radio buttons which need to be grouped in a fieldset

			// the fieldset is the FormPart
			this.CssClass = this.partCss;
			
			// the label on the left is a legend - it relates to the set of fields, not to an individual field
			this.legendControl = new HtmlGenericControl("legend");
			legendControl.InnerHtml = label;
			legendControl.Attributes.Add("class", this.labelCss);
			this.Controls.Add(legendControl);

			// use a div to contain the radio buttons
			HtmlGenericControl radioButtonList = new HtmlGenericControl("div");
			radioButtonList.Attributes.Add("class", "formControl radioButtonList");
			this.Controls.Add(radioButtonList);

			// add each radio button to the list
			if (radioButtons != null)
			{
				foreach (RadioButton radio in radioButtons)
				{
					radio.GroupName = groupName;
					radio.ID = groupName + radio.Text.Replace(" ", "");
					radioButtonList.Controls.Add(radio);
				}
			}
		}

		/// <summary>
		/// Presentation container for a set of radio buttons
		/// </summary>
		/// <param name="label">Text to label the group of buttons</param>
		/// <param name="radioButtons">The RadioButtons in the group</param>
		/// <remarks>Supplied radio buttons should have their text and value already set</remarks>
		public FormPart(string label, RadioButtonList radioButtons) : base(HtmlTextWriterTag.Fieldset)
		{
			// this overload is a little different - it takes a set of radio buttons which need to be grouped in a fieldset

			// the fieldset is the FormPart
			this.CssClass = this.partCss;
			
			// the label on the left is a legend - it relates to the set of fields, not to an individual field
			this.legendControl = new HtmlGenericControl("legend");
			legendControl.InnerHtml = label;
			legendControl.Attributes.Add("class", this.labelCss);
			this.Controls.Add(legendControl);

			// a radio button list contains the radio buttons
			radioButtons.RepeatLayout = RepeatLayout.Flow;
			radioButtons.RepeatDirection = RepeatDirection.Horizontal;
			radioButtons.Attributes.Add("class", "formControl radioButtonList");
			this.Controls.Add(radioButtons);
		}

        /// <summary>
        /// Presentation container for a checkbox control
        /// </summary>
        /// <param name="label">Text to label the group of boxes</param>
        /// <param name="checkBox">The checkbox in the group</param>
        /// <remarks>Supplied checkbox should have its text and value already set</remarks>
        public FormPart(string label, CheckBox checkBox)
            : base(HtmlTextWriterTag.Fieldset)
        {
            // the fieldset is the FormPart
            this.CssClass = this.partCss;

            // the label on the left is a legend - it relates to the set of fields, not to an individual field
            this.legendControl = new HtmlGenericControl("legend");
            legendControl.InnerHtml = label;
            legendControl.Attributes.Add("class", this.labelCss);
            this.Controls.Add(legendControl);

            checkBox.Attributes.Add("class", "formControl radioButtonList");
            this.Controls.Add(checkBox);
        }
		
		/// <summary>
		/// Presentation container for a set of checkboxes
		/// </summary>
		/// <param name="label">Text to label the group of boxes</param>
		/// <param name="checkBoxes">The checkboxes in the group</param>
		/// <remarks>Supplied checkboxes should have their text and value already set</remarks>
		public FormPart(string label, CheckBoxList checkBoxes) : base(HtmlTextWriterTag.Fieldset)
		{
			// the fieldset is the FormPart
			this.CssClass = this.partCss;
			
			// the label on the left is a legend - it relates to the set of fields, not to an individual field
			this.legendControl = new HtmlGenericControl("legend");
			legendControl.InnerHtml = label;
			legendControl.Attributes.Add("class", this.labelCss);
			this.Controls.Add(legendControl);

			// a checkbox list contains the checkboxes
			checkBoxes.RepeatLayout = RepeatLayout.Flow;
			checkBoxes.RepeatDirection = RepeatDirection.Horizontal;
			checkBoxes.Attributes.Add("class", "formControl radioButtonList");
			this.Controls.Add(checkBoxes);
        }
        #endregion

        #region Initialisation
        /// <summary>
        /// Presentation container for a form label and its associated control
        /// </summary>
        /// <param name="label">Label for the form control</param>
        /// <param name="webControl">Form control</param>
        private void initialiseGenericFormPart(string label, WebControl webControl)
        {
            // Configure controls
            this.labelControl = new Label();
            labelControl.Text = label;
            labelControl.CssClass = this.labelCss;
            webControl.CssClass = (webControl.CssClass.Length > 0) ? webControl.CssClass += " " + this.controlCss : this.controlCss;
            labelControl.AssociatedControlID = webControl.ID;

            // Add controls
            this.Controls.Add(labelControl);
            this.Controls.Add(webControl);

            // Set style
            this.CssClass = this.partCss;
        }

        /// <summary>
        /// Presentation container for a form label and its associated control
        /// </summary>
        /// <param name="label">Label for the form control</param>
        /// <param name="webControl">Form control</param>
        private void initialiseGenericFormPart(Label label, WebControl webControl)
        {
            // configure controls
            label.CssClass = (label.CssClass.Length > 0) ? label.CssClass += " " + this.labelCss : this.labelCss;
            webControl.CssClass = (webControl.CssClass.Length > 0) ? webControl.CssClass += " " + this.controlCss : this.controlCss;
            label.AssociatedControlID = webControl.ID;

            // add controls
            this.labelControl = label;
            this.Controls.Add(label);
            this.Controls.Add(webControl);

            // set style
            this.CssClass = this.partCss;
        }
        #endregion

        /// <summary>
		/// Notifies server controls that use composition-based implementation to create any child
		/// controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			// If the Required property is set to true, add an asterisk
			if (this.required)
			{
				if (this.labelControl != null)
				{
					this.labelControl.Controls.Add(new LiteralControl(this.labelControl.Text));
					this.labelControl.Controls.Add(new RequiredFieldIndicator());
				}
				else if (this.legendControl != null)
				{
					this.legendControl.Controls.Add(new RequiredFieldIndicator());
				}
			}

			base.CreateChildControls ();
		}

	}
}
