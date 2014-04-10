using System;
using System.Web.UI.WebControls;

namespace eastsussexgovuk.webservices.FormControls.Validators
{
	/// <summary>
	/// Validate a UK postcode against the regular expressions provided in the BS7666 e-GIF standard
	/// </summary>
	public class UKPostcodeValidator : RegularExpressionValidator
	{
		/// <summary>
		/// Postcode validation expression defined by BS7666 schema as compatible with all parsers
		/// </summary>
		private string simpleExpression = "^[A-Z]{1,2}[0-9R][0-9A-Z]? [0-9][A-Z]{2}$";

		/// <summary>
		/// Postcode validation expression defined by BS7666 schema - not compatible with all parsers
		/// </summary>
		private string complexExpression = "(GIR 0AA)|((([A-Z-[QVX]][0-9][0-9]?)|(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|(([A-Z-[QVX]][0-9][A-HJKSTUW])|([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY])))) [0-9][A-Z-[CIKMOV]]{2})";

		/// <summary>
		/// Validate a UK postcode against the BS7666 e-GIF address standard
		/// </summary>
		public UKPostcodeValidator()
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
			this.ValidationExpression = this.simpleExpression;
		}
		



		/// <summary>
		/// Validate a UK postcode against the BS7666 e-GIF address standard
		/// </summary>
		/// <param name="controlToValidateId">Id of the TextBox in which the postcode is being entered</param>
		/// <param name="errorMessage">The error message to display in the validation summary</param>
		public UKPostcodeValidator(string controlToValidateId, string errorMessage)
		{
			this.Display = ValidatorDisplay.None;
			this.EnableClientScript = false;
			this.ControlToValidate = controlToValidateId;
			
			
			this.ErrorMessage = errorMessage;
			this.ValidationExpression = this.simpleExpression;
		}
		
		/// <summary>
		/// Convert the postcode to e-GIF compliant (uppercase and with whitespace) and validates
		/// </summary>
		/// <returns></returns>
		protected override bool EvaluateIsValid()
		{
			TextBox textboxToValidate = this.Parent.FindControl(this.ControlToValidate) as TextBox;
			textboxToValidate.Text = textboxToValidate.Text.ToUpper();

			textboxToValidate.Text  = textboxToValidate.Text.Replace(" ", "");

			if (textboxToValidate.Text.Length == 6)
			{
				textboxToValidate.Text = textboxToValidate.Text.Insert(3, " ");
			}
			else if (textboxToValidate.Text.Length == 7)
			{
				textboxToValidate.Text = textboxToValidate.Text.Insert(4, " ");
			}
			
			return base.EvaluateIsValid();
		}
		

		/// <summary>
		/// Sets which BS7666-compliant regular expression to use for validating postcodes
		/// </summary>
		/// <remarks>The BS7666 schema notes that the complex expression doesn't work with all parsers. As of .NET 1.1 SP1, it seems not to work.</remarks>
		public PostcodeExpression PostcodeExpression
		{
			get 
			{
				if (this.ValidationExpression == this.complexExpression) return PostcodeExpression.BS7666ComplexExpression;
				else if (this.ValidationExpression == this.simpleExpression) return PostcodeExpression.BS7666SimpleExpression;
				else return PostcodeExpression.Unknown;
			}
			set 
			{ 
				if (value == PostcodeExpression.BS7666ComplexExpression) this.ValidationExpression = this.complexExpression;
				else if (value == PostcodeExpression.BS7666SimpleExpression) this.ValidationExpression = this.simpleExpression;
				else this.ValidationExpression = "";
			}
		}
	}



	/// <summary>
	/// BS7666 address standard provides two regular expressions for validating UK postcodes
	/// </summary>
	public enum PostcodeExpression
	{
		/// <summary>
		/// Neither the simple nor the complex expression has been specified
		/// </summary>
		Unknown,
		
		/// <summary>
		/// The simple e-GIF regular expression for validating UK postcodes - works with all parsers
		/// </summary>
		BS7666SimpleExpression,
		
		/// <summary>
		/// The complex e-GIF regular expression for validating UK postcodes - does not work with all parsers
		/// </summary>
		BS7666ComplexExpression
	}

	



}
