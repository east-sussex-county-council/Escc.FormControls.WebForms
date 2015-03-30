using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Escc.FormControls.WebForms.Validators
{
    /// <summary>
    /// Validate an email address against the e-GIF email address standard
    /// </summary>
    public class EmailValidator : RegularExpressionValidator
    {
        #region Declarations

        private const string EGifEmailRegex = @"^[0-9A-Za-z'\.\-_]{1,127}@[0-9A-Za-z'\.\-_]{1,127}$";

        /// <summary>
        /// An email field can have multiple email addresses separated by semicolons. By default it is assumed only a single email address exists.
        /// If this property is set to true, then multiple email addresses are catered for and validated.
        /// </summary>
        private bool allowMultipleEmails = false;

        #endregion

        #region Properties

        /// <summary>
        /// An email field can have multiple email addresses separated by semicolons. By default it is assumed only a single email address exists.
        /// If this property is set to true, then multiple email addresses are catered for and validated.
        /// </summary>
        public bool AllowMultipleEmails
        {
            get { return allowMultipleEmails; }
            set { allowMultipleEmails = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailValidator"/> class.
        /// </summary>
        public EmailValidator()
        {
            this.Display = ValidatorDisplay.None;
            this.EnableClientScript = false;
            this.ErrorMessage = "Please enter a valid email address";
            this.ValidationExpression = EGifEmailRegex;
        }

        /// <summary>
        /// Validate an email address against the e-GIF email address standard
        /// </summary>
        /// <param name="controlToValidateId">Id of the TextBox in which the email address is being entered</param>
        /// <param name="errorMessage">The error message to display in the validation summary</param>
        public EmailValidator(string controlToValidateId, string errorMessage)
        {
            this.Display = ValidatorDisplay.None;
            this.EnableClientScript = false;
            this.ControlToValidate = controlToValidateId;
            this.ErrorMessage = errorMessage;
            this.ValidationExpression = EGifEmailRegex;
        }

        #endregion

        /// <summary>
        /// Indicates whether the value in the input control is valid.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            // Get control to validate
            var textboxToValidate = this.Parent.FindControl(this.ControlToValidate) as TextBox;
            var inputToValidate = this.Parent.FindControl(this.ControlToValidate) as HtmlInputText;

            var textToValidate = String.Empty;
            if (textboxToValidate != null)
            {
                textToValidate = textboxToValidate.Text;
            }
            else if (inputToValidate != null)
            {
                textToValidate = inputToValidate.Value;
            }
            else
            {
                return true;
            }

            bool isValid = true;

            if (this.allowMultipleEmails)
            {
                // Split the content into multiple addresses if they exist
                string[] split = textToValidate.Split(';');
                var addresses = new string[split.Length];
                var i = 0;
                foreach (string s in split)
                {
                    // Tidy address to add back to control
                    string emailAddress = TidyEmailAddress(s);
                    addresses[i] = emailAddress;
                    i++;

                    // Validate this email address
                    isValid = this.ValidateSingleEmailAddress(emailAddress);
                    if (!isValid) break; // Do not continue if an invalid email address is already discovered!
                }

                // Update the control with the tidy addresses, because we don't want to tidy it to make it validate, 
                // but leave the invalid version in the control for our code to go on and use.
                textToValidate = String.Join(";", addresses);
            }
            else
            {
                // Validate as a single email address
                // Tidy the address in the control, because we don't want to tidy it to make it validate, 
                // but leave the invalid version in the control for our code to go on and use.
                textToValidate = TidyEmailAddress(textToValidate);
                isValid = this.ValidateSingleEmailAddress(textToValidate);
            }

            if (textboxToValidate != null)
            {
                textboxToValidate.Text = textToValidate;
            }
            else if (inputToValidate != null)
            {
                inputToValidate.Value = textToValidate;
            }
            return isValid;
        }

        private bool ValidateSingleEmailAddress(string emailAddress)
        {
            // this is not a required field validator
            if (String.IsNullOrEmpty(emailAddress)) return true;

            // Do the e-GIF validation
            if (!Regex.IsMatch(emailAddress, EGifEmailRegex))
            {
                return false;
            }

            // But that doesn't catch everything so try the Microsoft one
            try
            {
                new MailAddress(emailAddress);
            }
            catch (FormatException)
            {
                // If it can't create a MailAddress object, it's an invalid address
                return false;
            }

            // Even that doesn't catch everything and some fail on SmtpClient.Send(), 
            // so check for other common patterns
            if (emailAddress.Contains(".@"))
            {
                return false;
            }

            // Some characters and combinations are allowed in the local part of the address but not the domain
            if (!IsValidDomain(emailAddress))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether the domain part of an email address is valid
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        private static bool IsValidDomain(string emailAddress)
        {
            var domain = emailAddress.Substring(emailAddress.IndexOf("@", StringComparison.Ordinal) + 1);
            if (domain.Contains(".."))
            {
                return false;
            }

            if (domain.StartsWith("."))
            {
                return false;
            }

            if (domain.Contains("'"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Deals with common typos that we can anticipate to avoid an error message
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        private static string TidyEmailAddress(string emailAddress)
        {
            // Trim spaces 
            emailAddress = emailAddress.Trim();

            // Trim fullstops from the end, but only if that doesn't leave us with an empty string because that enables the following scenario: 
            //
            // 1. a field has already passed a RequiredFieldValidator using the value "."
            // 2. this method removes the "." leaving an empty required field
            // 3. this validator passes, because its job is to check an entered email address is valid, not to check whether an address was entered at all
            // 4. the invalid value has snuck though validation and an empty string gets used as an email address
            var emailCopy = emailAddress.TrimEnd(new char[1] { '.' });
            if (!String.IsNullOrEmpty(emailCopy)) emailAddress = emailCopy;

            return emailAddress;
        }
    }
}
