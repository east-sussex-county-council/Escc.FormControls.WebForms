using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Globalization;

namespace EsccWebTeam.FormControls.Validators
{
    /// <summary>
    /// Corrects and validates a URL typed into a TextBox
    /// </summary>
    public class UrlValidator : BaseValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlValidator"/> class.
        /// </summary>
        public UrlValidator()
        {
            this.Display = ValidatorDisplay.None;
            this.EnableClientScript = false;
            this.ErrorMessage = "Please enter a valid web address";
            this.UriKind = UriKind.RelativeOrAbsolute;
        }

        /// <summary>
        /// Gets or sets whether the URL must be relative, absolute, or either
        /// </summary>
        /// <value>The kind of the URI.</value>
        public UriKind UriKind { get; set; }

        /// <summary>
        /// When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            // Get control to validate
            TextBox toValidate = this.Parent.FindControl(this.ControlToValidate) as TextBox;
            if (toValidate == null) return true;

            // Anticipate extra spaces
            toValidate.Text = toValidate.Text.Trim();

            // This isn't a required field validator
            if (toValidate.Text.Length == 0) return true;

            // Anticipate that people might enter www.domain.com rather than http://www.domain.com
            // Anticipate that they might enter a non-www domain without http://
            if (
                (!toValidate.Text.StartsWith("http") && Regex.IsMatch(toValidate.Text, "^[a-z0-9]{2,}[.][a-z0-9]{2,}[a-z0-9.]*$")) ||
                toValidate.Text.StartsWith("www.")
                )
            {
                toValidate.Text = Uri.UriSchemeHttp + "://" + toValidate.Text.Trim().ToLower(CultureInfo.CurrentCulture);
            }

            // Now use the .NET Uri class to do the checking
            try
            {
                new Uri(toValidate.Text, this.UriKind);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
