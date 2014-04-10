using System;
using System.Web.UI.WebControls;
using System.Collections;
using eastsussexgovuk.webservices.FormControls.genericforms;
using System.Text.RegularExpressions;

namespace eastsussexgovuk.webservices.FormControls.Validators
{
	/// <summary>
	/// Custom validation summary control only for use with eforms. It's purpose 
	/// is to create a re-sorted list of error messages that matches the sequential
	/// order of the controls as they are rendered on to the page not the order of the control tree.
	/// This control is necessary to avoid the problem on deep nested child controls appearing
	/// out of order when rendered.
	/// </summary>
	public class EformsValidationSummary : ValidationSummary
	{
		
		SortedList sortedControls = new SortedList();


		

		/// <summary>
		/// Display server-side-only validation error messages in a bulleted list
		/// </summary>
		public EformsValidationSummary()
		{
			this.DisplayMode = ValidationSummaryDisplayMode.BulletList;
			this.EnableClientScript = false;
			this.CssClass = "validationSummary";

		}
		/// <summary>
		/// Override of the Render method, for eforms the validation summary control
		/// resorts the error messages into the correct sequential order. The control
		/// is neccessary because the default order is rendered based on the control tree
		/// and because the custom control used on eforms often have deep nested child controls 
		/// the default order is not sequential.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
            //Check that we have validation controls on the page and that the user has
            //click a button to fire off validation
            if (this.Page.Validators.Count > 0 & this.Page.IsPostBack)
            {
                try
                {

                    //Only render the validation summary if there are actually validation errors that need resolving
                    if (!Page.IsValid)
                    {
                        //Build up the html to produce a bullet list of error messages		
                        writer.Write("<div style=\"color: Red;\">");
                        writer.Write("Please answer the following questions:");
                        writer.Write("<ul>");

                        //Use regualar expression to retrieve the key (sequence number) for
                        //the new error message sort order.
                        Regex regex = new Regex(
                            @"(\d+)",
                            RegexOptions.IgnoreCase
                            | RegexOptions.Multiline
                            | RegexOptions.IgnorePatternWhitespace
                            | RegexOptions.Compiled
                            );
                        //Loop through each validation control on the page
                        foreach (BaseValidator validator in this.Page.Validators)
                        {
                            //Only interested in adding validator to the control if it is invalid
                            if (!validator.IsValid)
                            {

                                //Find the key
                                Match m = regex.Match(validator.ErrorMessage);
                                if (m.Success)
                                {
                                    //Cast to integer to avoid problem of 11 being placed above 2 because it is treated as a string
                                    int newKey = Convert.ToInt16(m.Groups[1].Value);

                                    //If key is not already in list, add to collection
                                    if (!sortedControls.ContainsKey(newKey))
                                    {
                                        sortedControls.Add(newKey, validator.ErrorMessage);
                                    }

                                }
                            }
                        }


                        // Loop through each item and add new item to the bullet list
                        foreach (DictionaryEntry listItem in sortedControls)
                        {


                            writer.Write("<li>" + listItem.Value + "</li>");


                        }
                        //Close html for bullet list
                        writer.Write("</ul>");
                        writer.Write("</div>");
                    }
                }
                catch(System.Web.HttpException)
                {
                    //Catch this exception when the address buttons are clicked
                    //Because causes validation is turned off and we need to call page isvalid
                    //It's too early and fails.
                   
                }
                    
            }
      

		 }
	
	}
}
