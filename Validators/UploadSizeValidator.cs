using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using eastsussexgovuk.webservices.FormControls.Validators;

namespace EsccWebTeam.FormControls.Validators
{
    /// <summary>
    /// Validates an uploaded file to ensure it's not bigger than a given maximum size
    /// </summary>
    public class UploadSizeValidator : EsccCustomValidator
    {
        #region Private fields
        private int maxBytes;
        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the maximum file size in bytes. Use 0 for unlimited.
        /// </summary>
        /// <value>The maximum file size in bytes.</value>
        public int MaximumBytes
        {
            get { return maxBytes; }
            set { maxBytes = value; }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadSizeValidator"/> class.
        /// </summary>
        public UploadSizeValidator() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:UploadSizeValidator"/> class.
        /// </summary>
        /// <param name="controlToValidateId">The id of the control to validate.</param>
        /// <param name="errorMessage">The error message.</param>
        public UploadSizeValidator(string controlToValidateId, string errorMessage)
            : base(controlToValidateId, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:UploadSizeValidator"/> class.
        /// </summary>
        /// <param name="controlToValidateId">The id of the control to validate.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="maximumBytes">The maximum file size in bytes. Use 0 for unlimited.</param>
        public UploadSizeValidator(string controlToValidateId, string errorMessage, int maximumBytes)
            : base(controlToValidateId, errorMessage)
        {
            this.maxBytes = maximumBytes;
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

            // If max size, check it
            if (this.maxBytes > 0)
            {
                bool bytesValid = (this.maxBytes >= postedFile.ContentLength);
                if (!bytesValid) return false;
            }

            return true;
        }

        #endregion Validation

    }

}
