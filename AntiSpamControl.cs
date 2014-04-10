using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EsccWebTeam.FormControls
{
    /// <summary>
    /// Control to prevent bots sucessfully submitting a form. Relies on them filling in every text field.
    /// </summary>
    public class AntiSpamControl : PlaceHolder, INamingContainer
    {
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            // Create field and label
            var field = new TextBox();
            field.ID = "leaveBlank";
            field.TabIndex = -1;

            var label = new Label();
            label.ID = field.ID + "Label";
            label.CssClass = "aural";
            label.Controls.Add(new LiteralControl(Properties.EsccWebTeam_FormControls.AntiSpamLabel));

            label.Controls.Add(field);
            this.Controls.Add(label);

            label.AssociatedControlID = field.ID;

            // Associate validator
            var valid = new MustBeEmptyValidator();
            valid.ControlToValidate = field.ID;
            this.Controls.Add(valid);
        }

        /// <summary>
        /// Ensures the anti-spam field is left empty. Spaces are allowed because keyboard users use space to select so may accidentally fill in a space.
        /// </summary>
        private class MustBeEmptyValidator : BaseValidator
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MustBeEmptyValidator"/> class.
            /// </summary>
            public MustBeEmptyValidator()
            {
                this.EnableClientScript = false;
                this.Display = ValidatorDisplay.None;
            }

            /// <summary>
            /// When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
            /// </summary>
            /// <returns>
            /// true if the value in the input control is valid; otherwise, false.
            /// </returns>
            protected override bool EvaluateIsValid()
            {
                // Get the control
                var control = this.NamingContainer.FindControl(this.ControlToValidate) as IEditableTextControl;
                if (control == null) return true;

                this.ErrorMessage = String.Format(CultureInfo.InvariantCulture, Properties.EsccWebTeam_FormControls.ErrorMustBeEmpty, this.ControlToValidate);

                // Check it's empty
                return String.IsNullOrEmpty(control.Text.Trim());
            }
        }
    }
}
