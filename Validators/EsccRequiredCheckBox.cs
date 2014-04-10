using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace eastsussexgovuk.webservices.FormControls.Validators
{
	/// <summary>
	/// Summary description for EsccRequiredCheckBox.
	/// </summary>
	public class EsccRequiredCheckBox : System.Web.UI.WebControls.BaseValidator 
	{

		private CheckBox checkbox;

        /// <summary>
        /// Initializes a new instance of the <see cref="EsccRequiredCheckBox"/> class.
        /// </summary>
		public EsccRequiredCheckBox()
		{
			base.EnableClientScript = false;
		}


        /// <summary>
        /// Determines whether the control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"></see> property is a valid control.
        /// </summary>
        /// <returns>
        /// true if the control specified by <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"></see> is a valid control; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.Web.HttpException">No value is specified for the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"></see> property.- or -The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"></see> property is not found on the page.- or -The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"></see> property does not have a <see cref="T:System.Web.UI.ValidationPropertyAttribute"></see> attribute associated with it; therefore, it cannot be validated with a validation control.</exception>
		protected override bool ControlPropertiesValid()
		{

			Control ctrl = FindControl(ControlToValidate);
       
			if (ctrl != null) 
			{
				checkbox = (CheckBox) ctrl;
				return (checkbox != null);    
			}
			else 
				return false;  // raise exception
		}

        /// <summary>
        /// When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
		protected override bool EvaluateIsValid()
		{     
			return checkbox.Checked;
		}
	}
}
