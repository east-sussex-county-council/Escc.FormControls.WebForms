using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Escc.FormControls.WebForms.Validators
{
    /// <summary>
    /// Checks that the MIME type of an uploaded file is among a whitelist of allowed types
    /// </summary>
    public class UploadFormatValidator : EsccCustomValidator
    {
        #region Private fields
        private StringCollection mimeTypes = new StringCollection();
        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the allowed MIME types.
        /// </summary>
        /// <value>The allowed MIME types.</value>
        public StringCollection AllowedMimeTypes
        {
            get { return mimeTypes; }
        }

        /// <summary>
        /// Gets or sets the allowed MIME types as a semi-colon separated list.
        /// </summary>
        /// <value>The allowed MIME types list.</value>
        public string AllowedMimeTypesList
        {
            get
            {
                string[] types = new string[this.mimeTypes.Count];
                this.mimeTypes.CopyTo(types, 0);
                return String.Join(";", types);
            }
            set
            {
                this.mimeTypes.Clear();
                if (value != null) this.mimeTypes.AddRange(value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadFormatValidator"/> class.
        /// </summary>
        public UploadFormatValidator() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:UploadFormatValidator"/> class.
        /// </summary>
        /// <param name="controlToValidateId">The id of the control to validate.</param>
        /// <param name="errorMessage">The error message.</param>
        public UploadFormatValidator(string controlToValidateId, string errorMessage)
            : base(controlToValidateId, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:UploadFormatValidator"/> class.
        /// </summary>
        /// <param name="controlToValidateId">The id of the control to validate.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="allowedFormats">The allowed formats as a semi-colon-separated list of MIME types. Empty string means any format.</param>
        public UploadFormatValidator(string controlToValidateId, string errorMessage, string allowedFormats)
            : base(controlToValidateId, errorMessage)
        {
            if (allowedFormats != null)
            {
                mimeTypes.AddRange(allowedFormats.Split(';'));
            }
        }
        #endregion Constructors

        #region Validation

        /// <summary>
        /// Overrides the <see cref="M:System.Web.UI.BaseValidator.EvaluateIsValid"></see> method.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            // Get control to validate
            HtmlInputFile inputToValidate = this.Parent.FindControl(this.ControlToValidate) as HtmlInputFile;
            FileUpload uploadToValidate = this.Parent.FindControl(this.ControlToValidate) as FileUpload;

            // Get the uploaded file
            HttpPostedFile postedFile;
            if (inputToValidate != null) postedFile = inputToValidate.PostedFile;
            else if (uploadToValidate != null) postedFile = uploadToValidate.PostedFile;
            // If the control doesn't exist, that's an error
            else return false;

            // If no file was uploaded that's not an error (this isn't a RequiredFieldValidator)
            if (postedFile == null || postedFile.ContentLength == 0) return true;

            // If specific format required, check for match
            if (this.mimeTypes.Count > 0)
            {
                bool formatValid = this.mimeTypes.Contains(postedFile.ContentType);
                if (!formatValid) return false;
            }

            return true;
        }

        #endregion Validation

    }

}
