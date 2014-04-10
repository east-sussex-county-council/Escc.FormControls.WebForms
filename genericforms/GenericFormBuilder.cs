using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using eastsussexgovuk.webservices.TextXhtml.HouseStyle;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Reflection;
using eastsussexgovuk.webservices.FormControls.Validators;
using EsccWebTeam.FormControls;
using eastsussexgovuk.webservices.TextXhtml.TextManipulation;

namespace eastsussexgovuk.webservices.FormControls.genericforms
{
    /// <summary>
    /// The GenericFormBuilder class accesses the eforms database
    /// and creates an instance of a Generic Form with all its
    /// associated child objects (quesiton and dependencies objects)
    /// </summary>
    public class GenericFormBuilder
    {
        #region member fields
        ///TODO: complete documentation
        /// <summary>
        /// 
        /// </summary>
        private ArrayList savedPages;
        /// <summary>
        /// A datatable collection for storing form questions as individual datatables
        /// </summary>
        private EformsDataTableCollection dtcQuestionCollection;
        /// <summary>
        /// The eforms database primary key corresponding to the form we wish to build.
        /// </summary>
        private int formID;
        /// <summary>
        /// Integer indicating the page of the form we want to build.
        /// </summary>
        private int requiredPage;
        /// <summary>
        /// The total number of pages for a given form.
        /// </summary>
        private int pageCount;
        /// <summary>
        /// A collection of EformsDataTableCollections representing form pages.
        /// </summary>
        private EformsPageCollection epc;
        /// <summary>
        /// A generic form object for encapsulating all information about an indivudual form instance.
        /// </summary>
        private GenericForm form;
        /// <summary>
        /// Http context of the web page hosting the form.
        /// </summary>
        private HttpContext context;
        /// <summary>
        /// The Eforms database citizen id of the user completing the form.
        /// </summary>
        private int citizenId;
        /// <summary>
        /// Used to keep track of the number of address fields that have been processed during form submission.
        /// </summary>
        private int processedFieldCount;
        #endregion
        #region properties

        /// <summary>
        /// Property PageCount (int)
        /// </summary>
        public int PageCount
        {
            get
            {
                return this.pageCount;
            }
            set
            {
                this.pageCount = value;
            }
        }

        #endregion
        #region constructor
        /// <summary>
        /// The class constructor.
        /// </summary>
        public GenericFormBuilder()
        {
            form = new GenericForm();
            processedFieldCount = 0;
        }
        #endregion
        #region public methods
        /// <summary>
        /// Method loops through the submitted form answers. If the question is not a simple date we
        ///can use the questionID to grab the value from the context.Request object.
        ///If the question is a simple date we need to use 'description of field' + questionID
        /// </summary>
        public void GetUserAnswers()
        {
            try
            {

                foreach (FormQuestion question in form.FormQuestionCollection)
                {
                    if ((question.AnswerType.Length > 0) & (question.AnswerType != "EsccEthnicityControl") & (question.AnswerType != "SimpleDate") & (question.AnswerType != "DeclarationOfInterest") & (question.AnswerType != "DatePicker") & (question.AnswerType != "Address")
                        & (question.AnswerType != "AddressNonCitizen") & (question.BaseControl.GetType().ToString() != "System.Web.UI.WebControls.CheckBox") & (question.AnswerType != "CheckBoxListVarchar") & (question.AnswerType != "DropDownListVarchar"))
                    {
                        question.Answer = context.Request.Form[question.BaseControl.UniqueID];//question.QuestionId.ToString()];						
                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "CheckBoxListVarchar"))
                    {
                        string[] keys = HttpContext.Current.Request.Form.AllKeys;
                        foreach (string key in keys)
                        {
                            Regex regex = new Regex(@"[A-Za-z0-9_]+(:|\$)" + question.HtmlQuestionId);
                            if (regex.IsMatch(key))
                            {


                                if (context.Request.Form[key] == "on")
                                {
                                    int indexhash = key.LastIndexOf("$") + 1;
                                    int index = int.Parse(key.Substring(indexhash));
                                    question.Answer += question.PosAnswerValues[index].ToString();
                                    question.Answer += ", ";
                                }
                            }



                        }
                        //This removes the trailing comma separator

                        if (question.Answer != null)
                        {

                            if (question.Answer.ToString().EndsWith(", "))
                            {
                                question.Answer = question.Answer.ToString().Remove(question.Answer.ToString().LastIndexOf(","));
                            }
                        }

                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "DropDownListVarchar"))
                    {
                        string[] keys = HttpContext.Current.Request.Form.AllKeys;
                        foreach (string key in keys)
                        {
                            Regex regex = new Regex(@"[A-Za-z0-9_]+(:|\$)" + question.HtmlQuestionId);
                            if (regex.IsMatch(key))
                            {

                                if (context.Request.Form[key].Length > 0)
                                {

                                    question.Answer = context.Request.Form[key];

                                }
                            }


                        }

                    }
                    else if ((question.AnswerType.Length > 0) & (question.BaseControl.GetType().ToString() == "System.Web.UI.WebControls.CheckBox"))
                    {
                        if (question.AnswerType == "CheckBoxInteger")
                        {

                            if (context.Request.Form[question.BaseControl.UniqueID] == "on")//question.QuestionId.ToString()] == "on")
                            {
                                question.Answer = 1;

                            }
                            else
                            {
                                question.Answer = 0;
                            }
                        }
                        else //CheckBoxVarchar
                        {
                            if (context.Request.Form[question.BaseControl.UniqueID] == "on")//question.QuestionId.ToString()] == "on")
                            {
                                question.Answer = "Yes";

                            }
                            else
                            {
                                question.Answer = "No";
                            }
                        }

                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "SimpleDate"))
                    {

                        string year = context.Request.Form[GetCustomControlRequestKey(question, "year")];
                        string month = context.Request.Form[GetCustomControlRequestKey(question, "month")];
                        string day = context.Request.Form[GetCustomControlRequestKey(question, "day")];

                        if (year == "" || month == "" || day == "")
                        {

                            question.Answer = "";

                        }
                        else
                        {
                            DateTime date = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
                            question.Answer = date;


                        }
                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "DeclarationOfInterest"))
                    {
                        string[] keys = HttpContext.Current.Request.Form.AllKeys;
                        foreach (string key in keys)
                        {
                            Regex regex = new Regex(@"[A-Za-z0-9_]+(:|\$)txbDeclaration");
                            if (regex.IsMatch(key))
                            {
                                question.Answer = context.Request.Form[key];
                            }
                        }

                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "EsccEthnicityControl"))
                    {
                        string other = "";
                        string ethnicity = "";
                        string[] keys = HttpContext.Current.Request.Form.AllKeys;
                        foreach (string key in keys)
                        {


                            if (key == "ethnicityChoice")
                            {
                                ethnicity = context.Request.Form[key];
                            }

                            Regex regex = new Regex(@"[A-Za-z0-9_]+(:|\$)txbOtherEthnicity");
                            if (regex.IsMatch(key))
                            {
                                other = context.Request.Form[key];

                                if (other.Length > 0 & ethnicity.Length > 0)
                                {
                                    question.Answer = ethnicity + " - " + other;
                                }
                                else if (ethnicity.Length > 0)
                                {
                                    question.Answer = ethnicity;
                                }


                            }

                        }

                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "DatePicker"))//Code is redundant
                    {
                        string[] keys = HttpContext.Current.Request.Form.AllKeys;
                        DateTime datepick = new DateTime();
                        foreach (string key in keys)
                        {
                            Regex regex = new Regex(@"[A-Za-z0-9_]+(:|\$)foo");
                            if (regex.IsMatch(key))
                            {
                                datepick = DateTime.Parse(context.Request.Form[key]);
                            }
                        }
                        question.Answer = datepick;
                    }
                    //TODO: get datecontrol to work
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "Datecontrol"))
                    {
                        question.Answer = context.Request.Form[question.BaseControl.UniqueID];//question.QuestionId.ToString()];	
                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "GivenName"))
                    {
                        question.Answer = context.Request.Form[question.BaseControl.UniqueID];//question.QuestionId.ToString()];	
                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "FamilyName"))
                    {
                        question.Answer = context.Request.Form[question.BaseControl.UniqueID];//question.QuestionId.ToString()];	
                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "PhoneNonCitizen"))
                    {
                        question.Answer = context.Request.Form[question.BaseControl.UniqueID];//question.QuestionId.ToString()];	
                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "EmailNonCitizen"))
                    {
                        question.Answer = context.Request.Form[question.BaseControl.UniqueID];//[question.QuestionId.ToString()];	
                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "FaxNonCitizen"))
                    {
                        question.Answer = context.Request.Form[question.BaseControl.UniqueID];//[question.QuestionId.ToString()];	
                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "MobileNonCitizen"))
                    {
                        question.Answer = context.Request.Form[question.BaseControl.UniqueID];//[question.QuestionId.ToString()];	
                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "Address"))
                    {
                        question.Answer = AddressAsAnswer();
                    }
                    else if ((question.AnswerType.Length > 0) & (question.AnswerType == "AddressNonCitizen"))
                    {
                        question.Answer = AddressAsAnswer();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
        }






        private string GetCustomControlRequestKey(FormQuestion question, string controlName)
        {
            string key = question.BaseControl.UniqueID;
            key = key.Remove(key.LastIndexOf("$") + 1);
            key += controlName + question.QuestionId;

            return key;
        }




        private PlaceHolder GenerateCheckBoxHtml(int start, int end)
        {
            PlaceHolder ph = new PlaceHolder();

            FormQuestion question = form.FormQuestionCollection[start];
            //Got Legend text
            string masterQuestionText = question.QuestionText;

            Label lblMasterQuestionText = new Label();
            lblMasterQuestionText.Text = masterQuestionText;

            LiteralControl formPartOpen = new LiteralControl("<div class=\"formPart\">");

            ph.Controls.Add(formPartOpen);
            ph.Controls.Add(lblMasterQuestionText);

            for (int i = start + 1; i < end; i++)
            {


                question = form.FormQuestionCollection[i];



                Label subQuestionLabel = new Label();
                subQuestionLabel.Text = question.QuestionText;

                ph.Controls.Add(subQuestionLabel);

                Label subQuestionAnswer = new Label();
                subQuestionAnswer.Text = question.Answer.ToString();

                ph.Controls.Add(subQuestionAnswer);




            }

            LiteralControl formPartClose = new LiteralControl("</div>");
            ph.Controls.Add(formPartClose);


            return ph;


        }











        /// <summary>
        /// This overload returns a light weight generic form object based on a form instance ID
        /// for admin purposes.
        /// </summary>
        /// <param name="placeHolder"></param>
        /// <param name="FormInstanceID"></param>
        /// <returns>Generic Form</returns>
        public GenericForm SetUpForm(PlaceHolder placeHolder, int FormInstanceID)
        {

            //Call CreateForm to get a generic form filled with all the formInstances questions and answers
            form = CreateForm(FormInstanceID);

            //Create the page heading using the form title (H1) tag
            HtmlGenericControl pageHeading = new HtmlGenericControl("h1");


            /***********************New code 05/01/2006*******************************************/
            pageHeading.InnerText = form.Title + " - reference " + FormInstanceID;

            placeHolder.Controls.Add(pageHeading);

            //Create the yellow form box opening tag
            LiteralControl formBoxOpen = new LiteralControl("<div class=\"formBox\"><div id=\"topedge\" class=\"cornerTR\"><div class = \"cornerTL\"></div></div> ");
            placeHolder.Controls.Add(formBoxOpen);


            //Boolean to test whether there is any question text to render
            bool hasLabel = false;


            //Loop through the form question collection. If its a fieldsetOpen with a following
            //Checkbox as the preceding control we have to call a seperate method 'GenerateCheckBoxHtml'
            //to do so bespoke rendering. This is because of the problems of multiple answers to a question
            for (int i = 0; i < form.FormQuestionCollection.Count; i++)
            {
                FormQuestion question = form.FormQuestionCollection[i];

                if (question.QuestionType == "fieldsetOpen")
                {
                    if (form.FormQuestionCollection[i + 1].BaseControl.GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                    {

                        for (int k = i; k < form.FormQuestionCollection.Count; k++)
                        {
                            if (question.QuestionType == "fieldsetClose")
                            {
                                PlaceHolder ph = GenerateCheckBoxHtml(i, k);

                                placeHolder.Controls.Add(ph);
                                i = k;
                                break;
                            }
                        }
                    }

                }


                //Generate the HTML to do with a question label
                if (question.QuestionText.Length > 0)
                {
                    hasLabel = true;

                    //If Question label then open formpart tag
                    LiteralControl formPartOpen = new LiteralControl("<div class=\"formPart\">");
                    placeHolder.Controls.Add(formPartOpen);


                    //Label lbl = question.QuestionLabel as Label;
                    Label lbl = new Label();

                    //With dates we have to render the question ref text slightly differently
                    if (question.AnswerDataType == "Date")
                    {
                        lbl.Text = question.QuestionRef + question.QuestionText.Replace(question.QuestionRef, "");
                        lbl.CssClass = "formLabel";
                    }
                    else
                    {
                        lbl.Text = question.QuestionRef + question.QuestionText;
                        lbl.CssClass = "formLabel";
                    }

                    //The DatePartFormControl need to have its label visibility property overridden
                    //Otherwise the admin eform detail page did not display correctly
                    if (question.AnswerType == "SimpleDate")
                    {
                        lbl.Visible = true;

                    }

                    //Add question label to placeholder
                    placeHolder.Controls.Add(lbl);

                }


                if (question.Answer != null)
                {
                    Label AnswerLabel = new Label();

                    if (question.AnswerDataType.ToString() == "Date")
                    {

                        if (question.Answer.ToString().Length == 0)
                        {
                            AnswerLabel.Text = "";
                        }
                        else
                        {

                            AnswerLabel.Text = DateTimeFormatter.FullBritishDateWithDay(DateTime.Parse(question.Answer.ToString()));
                        }
                    }
                    else
                    {
                        AnswerLabel.Text = question.Answer.ToString();
                    }

                    AnswerLabel.CssClass = "formControl formAnswer";
                    placeHolder.Controls.Add(AnswerLabel);



                }

                if (hasLabel)
                {
                    LiteralControl formPartClose = new LiteralControl("</div>");
                    placeHolder.Controls.Add(formPartClose);
                    hasLabel = false;
                }







            }

            LiteralControl formBoxClose = new LiteralControl("<div id=\"bottomedge\" + class=\"cornerBR\"><div class = \"cornerBL\"></div></div></div>");
            placeHolder.Controls.Add(formBoxClose);
            return form;
        }

        private GenericForm FailureToLoadForm(PlaceHolder placeHolder)
        {
            Label lbl = new Label();
            lbl.Text = "The form you requested is not available please try again later.";
            placeHolder.Controls.Add(lbl);
            form.Title = "Form unavailable";
            return form;
        }

        public GenericForm BadFormRequest(PlaceHolder placeHolder, string errorMessage)
        {
            Label lbl = new Label();
            lbl.Text = errorMessage;
            placeHolder.Controls.Add(lbl);
            form.Title = "Form unavailable";
            return form;
        }


        /* New code 09/01/2005 

        private void LoadCustomValidators()
        {
            Assembly formControls = Assembly.Load(dllName);
            CustomValidator	 customVal = formControls.CreateInstance(nameSpaceClassName) as CustomValidator;
            customVal.ControlToValidate = qID;
        }
        **************************/

        /// <summary>
        /// Method populates generic form object with form questions based on
        /// form ID and required page ID.
        /// </summary>
        /// <param name="placeHolder"></param>
        /// <param name="Context"></param>
        /// <returns>Generic Form</returns>
        public GenericForm SetUpForm(PlaceHolder placeHolder, HttpContext Context)
        {

            #region Checking the form exists
            context = Context;

            //Defaults to page 1 of any form in absence of a query string parameter for [p]
            requiredPage = 1;


            //Currently we are defaulting to the first form in the development database.
            formID = 0;


            Regex regexQueryString = new Regex(@"^\d{1,10}$", RegexOptions.IgnoreCase);

            bool isMatchForForm;
            bool isMatchForPageCount;
            int ispublished = 99;

            try
            {
                if (!String.IsNullOrEmpty(context.Request.QueryString["f"]) &&
                    !String.IsNullOrEmpty(context.Request.QueryString["p"]))
                {
                    isMatchForForm = regexQueryString.IsMatch(context.Request.QueryString["f"]);
                    isMatchForPageCount = regexQueryString.IsMatch(context.Request.QueryString["p"]);
                    var publishedParameterIsValid = (context.Request.QueryString["pub"] == null || context.Request.QueryString["pub"] == "0");

                    if (isMatchForForm && isMatchForPageCount && publishedParameterIsValid)
                    {
                        formID = Convert.ToInt32(context.Request.QueryString["f"]);
                        requiredPage = Convert.ToInt32(context.Request.QueryString["p"]);
                        if (context.Request.QueryString["pub"] != null)
                        {
                            ispublished = 0;
                        }

                    }
                    else
                    {
                        return FailureToLoadForm(placeHolder);
                    }


                }
                else if (!String.IsNullOrEmpty(context.Request.QueryString["f"]))
                {
                    int formValue = 0;
                    formID = 0;
                    bool success = false;
                    success = Int32.TryParse(context.Request.QueryString["f"], out formValue);
                    if (success) { formID = formValue; }
                    requiredPage = 1;
                }
                else
                {
                    return FailureToLoadForm(placeHolder);
                }
            }
            catch (NullReferenceException nullEx)
            {
                ExceptionManager.Publish(nullEx);
                return BadFormRequest(placeHolder, "The form you requested could not be loaded.");
            }
            catch (FormatException formEx)
            {
                ExceptionManager.Publish(formEx);
                return BadFormRequest(placeHolder, "The form you requested could not be loaded.");
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                return BadFormRequest(placeHolder, "The form you requested could not be loaded.");
            }


            //The presence of the pub parmeter means it is a test form unpublished


            //TODO: Is to check if the form even exists in the database




            string conn = ConfigurationManager.AppSettings["DbConnectionStringEformsReader"];

            SqlConnection cn = new SqlConnection(conn);

            SqlParameter[] parameters = new SqlParameter[4];//Is this right should it be 3 not 4?

            parameters[0] = new SqlParameter("@RequestedFormID", SqlDbType.Int);
            parameters[0].Value = formID;

            parameters[1] = new SqlParameter("@RedirectFormID", SqlDbType.Int);
            parameters[1].Direction = ParameterDirection.Output;

            parameters[2] = new SqlParameter("@isTestForm", SqlDbType.Int);

            if (ispublished == 0)
            {
                parameters[2].Value = 1;
            }
            else
            {
                parameters[2].Value = 2;
            }


            try
            {

                SqlHelper.ExecuteNonQuery(cn, CommandType.StoredProcedure, ConfigurationManager.AppSettings["eFormsCheckFormIsNotExpired"], parameters);
                bool success = false;
                int formValue = 0;
                int QueryStringFormID = 0;
                success = Int32.TryParse(context.Request.QueryString["f"], out formValue);
                if (success) { QueryStringFormID = formValue; }


                try
                {
                    int paramValue = 0;
                    formID = 0;
                    bool successparm = false;
                    successparm = Int32.TryParse(parameters[1].Value.ToString(), out paramValue);
                    if (successparm) { formID = paramValue; }

                }
                catch (InvalidCastException)
                {
                    //Form requested is unpublished this request in error
                    formID = 0;
                }


                //Form has been expired
                if (formID == 0)
                {
                    Label lbl = new Label();
                    lbl.Text = "The form you requested does not exist.";

                    placeHolder.Controls.Add(lbl);

                    return form;
                }
                //else if (formID == -1)
                //{
                //    ////New code 29/11/2005
                //    ////The form is unpublished
                //    //Label lbl = new Label();
                //    //lbl.Text = "The form you requested is not available.";
                //    //form.Title = "Form unavailable";

                //    //placeHolder.Controls.Add(lbl);
                //    //return form;

                //    context.Response.Redirect(context.Request.Url.AbsolutePath + "?f=" + formID + "&p=1" + "&pub=0");


                //}
                else if (formID != QueryStringFormID)
                {
                    context.Response.Redirect(context.Request.Url.AbsolutePath + "?f=" + formID + "&p=1");
                }

            }
            catch (SqlException ex)
            {
                ExceptionManager.Publish(ex);
            }
            #endregion


            //Create the form for the desired page.
            form = this.CreateForm();



            if (form.Title == "Form unavailable")
            {
                return form;
            }


            #region old code not implemented yet
            //	DataSet dsSavedAnswers = null;
            // After we get the form back with question collection, we loop through
            //and add question controls to the placeholder controls collection. We also handle
            //special cases for labels and validation controls and finally add the validation summary and
            //required * message to the top of the form
            // Get previously saved answers for this page if they exist
            //TODO: is this necessary? Don't we have answers as question props if they exist?
            //			if(HttpContext.Current.Session["formInstanceID"] != null)
            //			{
            //				int id = Convert.ToInt32(HttpContext.Current.Session["formInstanceID"]);
            //				// we only need be concerned with this operation for pages other than the first page
            //				if(requiredPage > 1)
            //				{
            //					SqlParameter[] prms = new SqlParameter[2];
            //					SqlParameter prm0 = new SqlParameter("@FormInstanceID", SqlDbType.Int, 4);
            //					prm0.Value = id;
            //					prms[0] = prm0;
            //					SqlParameter prm1 = new SqlParameter("@PageNumber", SqlDbType.Int, 4);
            //					prm1.Value = requiredPage;
            //					prms[1] = prm1;
            //					dsSavedAnswers = SqlHelper.ExecuteDataset(cn, CommandType.StoredProcedure,ConfigurationManager.AppSettings["eFormsGetSavedAnswersForPageSP"],prms);
            //				}
            //			}
            //			int dsIdx = 0;
            #endregion




            //Call Set Labels()
            SetLabels();

            placeHolder = RenderControls(placeHolder);

            #region	old render code
            /*foreach (FormQuestion question in form.FormQuestionCollection)
			{

         

                

                if (question.QuestionType.ToString() != "Confirmation")
				{
					Control c = question.BaseControl;

					
					
			
					//TODO: again this needs checking and testing
					//					if((requiredPage > 1) & (dsSavedAnswers != null))
					//					{
					//						switch (question.AnswerDataType)
					//						{
					//							case "Varchar":
					//						
					//								question.Answer = dsSavedAnswers.Tables[0].Rows[dsIdx]["AnswerVarchar"].ToString();					
					//								dsIdx++;
					//								break;
					//
					//							case "Text":
					//								question.Answer = dsSavedAnswers.Tables[0].Rows[dsIdx]["AnswerText"].ToString();	
					//							dsIdx++;
					//								break;
					//
					//							case "Integer":
					//								question.Answer = dsSavedAnswers.Tables[0].Rows[dsIdx]["AnswerInteger"].ToString();	
					//						dsIdx++;
					//								break; 
					//
					//							case "Date":
					//								question.Answer = dsSavedAnswers.Tables[0].Rows[dsIdx]["AnswerDate"].ToString();				
					//							dsIdx++;
					//								break;
					//
					//							case "Double":
					//								question.Answer = dsSavedAnswers.Tables[0].Rows[dsIdx]["AnswerDecimal"].ToString();	
					//							dsIdx++;
					//								break;
					//
					//							case "Bit":
					//								question.Answer = dsSavedAnswers.Tables[0].Rows[dsIdx]["AnswerBit"].ToString();	
					//							dsIdx++;
					//								break;
					//
					//							default:
					//								break;
					//
					//						}

					//					}
			

					if (question.AnswerType == "DeclarationOfInterest")
					{
						placeHolder.Controls.Add(question.BaseControl);
					}
					else
					{
						
				
						if ((question.QuestionLabel.Text.Length > 1 ) & (question.QuestionType != "fieldsetOpen")& (question.QuestionType != "fieldsetClose"))
						{


                            //
                            if ((question.AnswerType != "EsccEthnicityControl") & (question.AnswerType != "DropDownListVarchar") & (question.AnswerType != "RadioButtonListVarchar") & (question.AnswerType != "DatePicker") & (question.AnswerType != "CheckBoxInteger") & (question.AnswerType != "MaritalStatus") & (question.AnswerType != "DeclarationOfInterest"))
							{
								placeHolder.Controls.Add(question.QuestionLabel);
							}
						}
						if (question.ValidationControl != null)
						{
							placeHolder.Controls.Add(question.ValidationControl);
						}
					
						//TODO: added datecontrol?

						if ((question.IsRequired)& (question.AnswerType !="EsccEthnicityControl")& (question.AnswerType != "SimpleDate")& (question.AnswerType != "DateControl") & (question.AnswerType != "DatePicker")& (question.AnswerType !="Address") & (question.AnswerType !="AddressNonCitizen")) 
						{
							if ((question.AnswerType == "CheckBoxListVarchar") |(question.AnswerType == "CheckBoxListInteger"))

							{
								placeHolder.Controls.Add(question.RequiredCheckBoxListValidator);
							}
							else if (question.AnswerType == "CheckBoxVarchar")
							{
								placeHolder.Controls.Add(question.RequiredCheckBoxValidator);
							}
							else
							{
								placeHolder.Controls.Add(question.RequiredValidator);
							}
						}
						
							
					}


					//Something to do with placing the headings in the right place need to review this section 16/07/2007 DH
					placeHolder.Controls.Add(c);

					if(question.QuestionType == "Section Heading" & firstSectionHeading == 0)
					{
						firstSectionHeading = placeHolder.Controls.IndexOf(question.BaseControl);
					}	
					
					if(question.QuestionType == "formBoxOpen" & firstTopYellowBox == 0)
					{
						firstTopYellowBox = placeHolder.Controls.IndexOf(question.BaseControl);
					}	
				}
			  }*/
            #endregion

            //Something to do with placing the headings in the right place need to review this section 16/07/2007 DH



            return form;
        }
        #endregion


        private PlaceHolder RenderControls(PlaceHolder placeHolder)
        {

            int firstSectionHeading = 0;
            int firstTopYellowBox = 0;

            for (int i = 1; i < form.FormQuestionCollection.Count; i++)
            {
                FormQuestion question = new FormQuestion();
                question = form.FormQuestionCollection[i];


                if (question.QuestionType.ToString() != "Confirmation")
                {
                    Control c = question.BaseControl;
                    if (question.AnswerType == "DeclarationOfInterest")
                    {
                        placeHolder.Controls.Add(question.BaseControl);
                    }
                    else
                    {
                        if ((question.RenderQuestionLabel) & (question.QuestionLabel.Text.Length > 1))
                        {
                            placeHolder.Controls.Add(question.QuestionLabel);
                        }

                        if (question.ValidationControl != null)
                        {
                            placeHolder.Controls.Add(question.ValidationControl);
                        }

                        if ((question.IsRequired) & (question.AnswerType != "EsccEthnicityControl") & (question.AnswerType != "SimpleDate") & (question.AnswerType != "DateControl") & (question.AnswerType != "DatePicker") & (question.AnswerType != "Address") & (question.AnswerType != "AddressNonCitizen"))
                        {
                            if ((question.AnswerType == "CheckBoxListVarchar") | (question.AnswerType == "CheckBoxListInteger"))
                            {
                                placeHolder.Controls.Add(question.RequiredCheckBoxListValidator);
                            }
                            else if (question.AnswerType == "CheckBoxVarchar")
                            {
                                placeHolder.Controls.Add(question.RequiredCheckBoxValidator);
                            }
                            else
                            {
                                placeHolder.Controls.Add(question.RequiredValidator);
                            }
                        }

                        placeHolder.Controls.Add(c);



                    }


                }



            }

            #region validation header
            ////This adds the header controls like validation summary, asterik message prompt
            //EformsValidationSummary valSummary = new EformsValidationSummary();
            //valSummary.EnableClientScript = false;
            //valSummary.HeaderText = "Please answer the following questions:";
            //valSummary.ShowSummary = true;

            //KeyControl requiredNote = new KeyControl();
            //if (firstSectionHeading > 0)
            //{
            //    placeHolder.Controls.AddAt(firstSectionHeading - 1, valSummary);

            //    placeHolder.Controls.AddAt(firstSectionHeading, requiredNote);
            //}
            //else
            //{
            //    placeHolder.Controls.AddAt(firstTopYellowBox, valSummary);

            //    placeHolder.Controls.AddAt(firstTopYellowBox + 1, requiredNote);
            //}
            #endregion

            return placeHolder;
        }

        private void SetLabels()
        {
            ArrayList listControls = new ArrayList();
            listControls.Add("System.Web.UI.WebControls.RadioButtonList");
            listControls.Add("System.Web.UI.WebControls.CheckBoxList");
            listControls.Add("System.Web.UI.WebControls.DropDownList");//Comment this out
            listControls.Add("eastsussexgovuk.webservices.FormControls.CustomControls.EsccCheckBoxList");
            // listControls.Add("eastsussexgovuk.webservices.FormControls.DateTimeFormPart");





            foreach (FormQuestion question in form.FormQuestionCollection)
            {



                if ((question.IsRequired) & (listControls.Contains(question.BaseControl.GetType().ToString())) & ((question.AnswerType != "Title")))
                {
                    //Get the previous question for fieldsets to update the legend tag with the question label text
                    int index = form.FormQuestionCollection.IndexOf(question);
                    FormQuestion previousQuestion = (FormQuestion)form.FormQuestionCollection[index - 1];


                    string legend = previousQuestion.QuestionLabel.Text;
                    previousQuestion.QuestionLabel.Text += "<span class=\"requiredField\">*</span>";
                    string reqAsterik = "<span class=\"requiredField\">*</span>";
                    legend = TextManipulationUtilities.LegendWrapLines(legend, 75);
                    previousQuestion.BaseControl = new LiteralControl("<fieldset class = \"" + previousQuestion.CssClassName + "\"><legend>" + legend + reqAsterik + " </legend> ");


                    //Create and add the validator control
                    question.AddRequiredValidator();

                    // we now update the form's question collection with the new changes that have been applied to the  (previous) question object
                    form.FormQuestionCollection[index - 1].QuestionLabel = previousQuestion.QuestionLabel;
                    form.FormQuestionCollection[index - 1].BaseControl = previousQuestion.BaseControl;

                }
                else if (question.IsRequired)
                {
                    question.QuestionLabel.Text = question.QuestionRef + " " + question.QuestionText + "<span class=\"requiredField\">*</span>";
                    question.AddRequiredValidator();
                }
                else
                {
                    question.QuestionLabel.Text = question.QuestionRef + " " + question.QuestionText;

                }
            }
        }


        /// <summary>
        /// Save submitted form data
        /// </summary>
        public int FormSave()
        {
            // this is the return value and is used to generate a thank you message following a successful form submission.
            int formInstanceID = 0;
            /************************* SQL PARAMETERS FOR GENERATING THE FORMINSTANCE ID *************************************************************/
            SqlParameter[] formParameters = new SqlParameter[2];
            formParameters[0] = new SqlParameter("@formInstanceId", SqlDbType.Int);
            formParameters[0].Direction = ParameterDirection.Output;
            formParameters[1] = new SqlParameter("@formID", SqlDbType.Int);
            formParameters[1].Value = formID;
            /********************* SQL PARAMETERS FOR FORM ANSWERS***************************************************/
            SqlParameter[] parameters = new SqlParameter[9];
            parameters[0] = new SqlParameter("@QuestionID", SqlDbType.Int);
            parameters[1] = new SqlParameter("@FormInstanceId", SqlDbType.Int);
            parameters[2] = new SqlParameter("@AnswerVarchar", SqlDbType.VarChar);
            parameters[3] = new SqlParameter("@AnswerText", SqlDbType.Text);
            parameters[4] = new SqlParameter("@AnswerInteger", SqlDbType.Int);
            parameters[5] = new SqlParameter("@AnswerDate", SqlDbType.DateTime);
            parameters[6] = new SqlParameter("@AnswerBit", SqlDbType.Bit);
            parameters[7] = new SqlParameter("@AnswerDecimal", SqlDbType.Decimal);
            parameters[8] = new SqlParameter("@PageNumber", SqlDbType.Int);
            //parameters for citizen & registration
            SqlParameter[] citizenParams = new SqlParameter[8];
            citizenParams[0] = new SqlParameter("@FamilyName", SqlDbType.VarChar, 35);
            citizenParams[1] = new SqlParameter("@GivenName", SqlDbType.VarChar, 35);
            citizenParams[2] = new SqlParameter("@BirthDate", SqlDbType.DateTime, 8);
            citizenParams[3] = new SqlParameter("@MaritalStatusTypeID", SqlDbType.Int, 4);
            citizenParams[4] = new SqlParameter("@Gender", SqlDbType.Char, 1);
            citizenParams[5] = new SqlParameter("@Title", SqlDbType.NVarChar, 35);
            citizenParams[6] = new SqlParameter("@NameInitials", SqlDbType.NVarChar, 35);
            citizenParams[7] = new SqlParameter("@CitizenID", SqlDbType.Int, 4);
            citizenParams[7].Direction = ParameterDirection.Output;

            SortedList slPhoneNumbers = new SortedList();
            SortedList slEmailAddresses = new SortedList();

            string Conn = ConfigurationManager.AppSettings["DbConnectionStringEformsWriter"];

            SqlConnection cn = new SqlConnection(Conn);

            SqlTransaction transaction = null;

            cn.Open();
            transaction = cn.BeginTransaction();

            try
            {
                try
                {
                    //TODO: deal with users moving backwards and forwards between pages
                    // we only need to call this sp the first time the first page of a form is saved
                    if (HttpContext.Current.Session["forminstanceId"] == null)
                    {
                        // only need to instantiate the savedPages arraylist the first time a form page is saved
                        savedPages = new ArrayList();
                        // put the arraylist in session
                        HttpContext.Current.Session["savedPages"] = savedPages;
                        SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure,
                            ConfigurationManager.AppSettings["eFormsGetNextFormInstanceIdSP"], formParameters);
                        HttpContext.Current.Session["forminstanceId"] = parameters[1].Value
                            = formInstanceID = Convert.ToInt32(formParameters[0].Value);
                    }
                    else
                    {
                        parameters[1].Value = formInstanceID = Convert.ToInt32(HttpContext.Current.Session["forminstanceId"]);
                        // if this is not the first time in we change the lastPage session variable to manage db inserts/updates if users go back 
                        // to previously completed pages and resubmit
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Publish(ex);
                    transaction.Rollback();
                    throw;
                }
                foreach (FormQuestion question in form.FormQuestionCollection)
                {
                    parameters[0].Value = question.QuestionId;

                    if (question.AnswerType.Length > 0)
                    {
                        //Explicit casting of answers to underlying db types
                        switch (question.AnswerDataType)
                        {
                            case "Date":
                                if ((question.Answer == null) || (question.Answer.ToString() == ""))
                                {
                                    parameters[5].Value = DBNull.Value;
                                }
                                else
                                {
                                    parameters[5].Value = (DateTime)question.Answer;
                                }
                                // get dob for citizen & regitration
                                if (question.AnswerType == "BirthDate")
                                {
                                    citizenParams[2].Value = (DateTime)question.Answer;
                                }
                                break;

                            case "Varchar":
                                if ((question.Answer == null) || (question.Answer.ToString() == ""))
                                {
                                    parameters[2].Value = DBNull.Value;
                                }
                                else
                                {
                                    parameters[2].Value = (string)question.Answer;
                                }
                                /***************************** begin citizen & registration field checks ************************/
                                if (question.AnswerType == "GivenName")
                                {
                                    citizenParams[1].Value = (string)question.Answer;
                                }
                                if (question.AnswerType == "FamilyName")
                                {
                                    citizenParams[0].Value = (string)question.Answer;
                                }
                                if (question.AnswerType == "MaritalStatus")
                                {
                                    if (question.Answer != null)
                                    {
                                        citizenParams[3].Value = (int)question.Answer;
                                    }
                                    else
                                    {
                                        citizenParams[3].Value = DBNull.Value;
                                    }
                                }
                                if (question.AnswerType == "Gender")
                                {
                                    citizenParams[4].Value = (string)question.Answer;
                                }
                                if (question.AnswerType == "Title")
                                {
                                    citizenParams[5].Value = (string)question.Answer;
                                }
                                if (question.AnswerType == "NameInitials")
                                {
                                    citizenParams[6].Value = (string)question.Answer;
                                }
                                // NB these are 'citizen' email addresses and phone numbers; any other email addresses or tel nos should be dealt with using
                                // strightforward text fields and be treated as a normal question/answer.
                                if ((question.AnswerType == "HomeEmail") | (question.AnswerType == "WorkEmail"))
                                {
                                    switch (question.AnswerType)
                                    {
                                        case "HomeEmail":
                                            slEmailAddresses.Add("HomeEmail" + question.QuestionId.ToString(), question.Answer.ToString());
                                            break;
                                        case "WorkEmail":
                                            slEmailAddresses.Add("WorkEmail" + question.QuestionId.ToString(), question.Answer.ToString());
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                if ((question.AnswerType == "HomePhone") | (question.AnswerType == "WorkPhone") | (question.AnswerType == "WorkMobile")
                                    | (question.AnswerType == "PersonalMobile") | (question.AnswerType == "HomeFax") | (question.AnswerType == "WorkFax"))
                                {
                                    switch (question.AnswerType)
                                    {
                                        case "HomePhone":
                                            slPhoneNumbers.Add("HomePhone" + question.QuestionId.ToString(), question.Answer.ToString());
                                            break;
                                        case "WorkPhone":
                                            slPhoneNumbers.Add("WorkPhone" + question.QuestionId.ToString(), question.Answer.ToString());
                                            break;
                                        case "WorkMobile":
                                            slPhoneNumbers.Add("WorkMobile" + question.QuestionId.ToString(), question.Answer.ToString());
                                            break;

                                        case "PersonalMobile":
                                            slPhoneNumbers.Add("PersonalMobile" + question.QuestionId.ToString(), question.Answer.ToString());
                                            break;
                                        case "WorkFax":
                                            slPhoneNumbers.Add("WorkFax" + question.QuestionId.ToString(), question.Answer.ToString());
                                            break;

                                        case "HomeFax":
                                            slPhoneNumbers.Add("HomeFax" + question.QuestionId.ToString(), question.Answer.ToString());
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                /***************************** end citizen, email, phone number & registration field checks *****/
                                /***************************** begin address checks *********************************************/
                                if (question.AnswerType == "Address")
                                {
                                    CustomControls.FormAddress frmAddress = question.BaseControl as CustomControls.FormAddress;
                                    // compare submitted address with the confirmed address (paf valid) stored in session from the confirm address event.
                                    // if these addresses are different then the user has manually altered a confirmed address and therefore
                                    // the address is no longer PAF valid.
                                    string[] keys = HttpContext.Current.Request.Form.AllKeys;
                                    // gets a string from web config of the form "Saon, paon, streetDescriptor, locality, town, administrativeArea, postcode";
                                    string addressFields = ConfigurationManager.AppSettings["eFormsAddressFields"];
                                    // get an array of the field names
                                    addressFields = addressFields.Replace(" ", "");
                                    string[] names = addressFields.Split(Convert.ToChar(","));
                                    DataSet ds = null;
                                    DataRow dr = null;
                                    // check if the user 'confirmed' an address by postcode search
                                    if (HttpContext.Current.Session["pafaddress"] != null)
                                    {
                                        ds = HttpContext.Current.Session["pafaddress"] as DataSet;
                                        dr = ds.Tables[0].Rows[0];
                                        // since the session dataset is created when the user confirms an address found by postcode we
                                        //can assume the address is paf valid at this stage
                                        frmAddress.PafCheckValid = true;
                                    }
                                    // we only need to check address form fields if the datatset exists in session
                                    // otherwise we just use any form field address values and set pafCheckValid to false.
                                    if (dr != null)
                                    {
                                        foreach (string key in keys)
                                        {
                                            if (dr != null)
                                            {
                                                foreach (string name in names)
                                                {
                                                    //string temp = name.Trim();
                                                    Regex regex = new Regex(@"[A-Za-z0-9_]+(:|\$)" + name);
                                                    if (regex.IsMatch(key))
                                                    {
                                                        switch (name)
                                                        {
                                                            case "saon":
                                                                if (dr["saon"].ToString() != context.Request.Form[key])
                                                                {
                                                                    frmAddress.PafCheckValid = false;
                                                                }
                                                                break;

                                                            case "paon":

                                                                if (dr["paon"].ToString() != context.Request.Form[key])
                                                                {
                                                                    frmAddress.PafCheckValid = false;
                                                                }
                                                                break;

                                                            case "streetDescriptor":
                                                                if (dr["streetDescriptor"].ToString() != context.Request.Form[key])
                                                                {
                                                                    frmAddress.PafCheckValid = false;
                                                                }
                                                                break;

                                                            case "locality":
                                                                if (context.Request.Form[key] != null & context.Request.Form[key] != "")
                                                                {
                                                                    frmAddress.PafCheckValid = false;
                                                                }
                                                                break;

                                                            case "town":
                                                                if (dr["town"].ToString() != context.Request.Form[key])
                                                                {
                                                                    frmAddress.PafCheckValid = false;
                                                                }
                                                                break;

                                                            case "administrativeArea":
                                                                if (dr["administrativeArea"].ToString() != context.Request.Form[key])
                                                                {
                                                                    frmAddress.PafCheckValid = false;
                                                                }
                                                                break;

                                                            case "postcode":
                                                                if (dr["postcode"].ToString() != context.Request.Form[key])
                                                                {
                                                                    frmAddress.PafCheckValid = false;
                                                                }
                                                                dr = null;
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //write out citizen details and create anonymous registration
                                    //TODO: deal with users moving backwards and forwards between pages
                                    //TODO: This should insert citizen details and flag them as current (the latest addition) do we need to do anything else?
                                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure,
                                        ConfigurationManager.AppSettings["eFormsInsertCitizenAnonymousSP"], citizenParams);
                                    //David changed 22/07/05  citizenid from string to int
                                    citizenId = Convert.ToInt32(citizenParams[7].Value);
                                    //TODO: deal with users moving backwards and forwards between pages
                                    //call saveAddress method in FormAddress	
                                    //TODO: The address will be saved again if a user has used the back button to return to a page with an address control
                                    // but it is probably more effort than it is worth to prevent this being rewritten and in any event they might change address details?
                                    int addressId = frmAddress.SaveAddress(citizenId, (int)parameters[1].Value, transaction, ConfigurationManager.AppSettings["eFormsAddressInsertSP"], false);
                                    //TODO: deal with users moving backwards and forwards between pages
                                    //TODO: This should work as is and simply update the form instance table with the last saved address id?
                                    frmAddress.UpdateFormInstanceWithAddressID((int)parameters[1].Value, citizenId, addressId, transaction, ConfigurationManager.AppSettings["eFormsUpdateFormInstanceSP"]);
                                }
                                /*********************************** end address checks ***********************************/
                                break;

                            case "Text":
                                if ((question.Answer == null) || (question.Answer.ToString() == ""))
                                {
                                    parameters[3].Value = DBNull.Value;
                                }
                                else
                                {
                                    parameters[3].Value = (string)question.Answer;
                                }
                                break;

                            case "Integer":

                                if ((question.Answer == null) || (question.Answer.ToString() == ""))
                                {
                                    parameters[4].Value = DBNull.Value;
                                }
                                else
                                {
                                    if (question.Answer.ToString() == "on")
                                    {
                                        parameters[4].Value = 1;
                                    }
                                    else if (question.Answer.ToString() == "off")
                                    {
                                        parameters[4].Value = 2;
                                    }
                                    else
                                    {
                                        parameters[4].Value = (int)question.Answer;
                                    }

                                }
                                break;

                            case "Double":
                                if ((question.Answer == null) || (question.Answer.ToString() == ""))
                                {
                                    parameters[6].Value = DBNull.Value;
                                }
                                else
                                {
                                    parameters[6].Value = (double)question.Answer;
                                }
                                break;

                            case "Bit":
                                if ((question.Answer == null) || (question.Answer.ToString() == ""))
                                {
                                    parameters[7].Value = DBNull.Value;
                                }
                                else
                                {
                                    parameters[7].Value = (bool)question.Answer;
                                }
                                break;

                            default:
                                break;
                        }
                        //add the page number as a parameter to allow retrieval and repopulation of pages if the user has returned to a previous page 
                        //using the browser back button
                        parameters[8].Value = requiredPage;

                        try
                        {
                            //TODO: deal with users moving backwards and forwards between pages

                            if (CheckIfSaved(requiredPage.ToString()))
                            {
                                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure,
                                    ConfigurationManager.AppSettings["eFormsUpdateFormAnswersSP"], parameters);
                            }
                            else
                            {
                                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure,
                                    ConfigurationManager.AppSettings["eFormsInsertFormAnswersSP"], parameters);
                            }

                            if (requiredPage == pageCount)
                            {
                                SqlParameter prm = new SqlParameter();
                                prm.ParameterName = "@formInstanceID";
                                prm.Value = (int)HttpContext.Current.Session["formInstanceID"];
                                prm.SqlDbType = SqlDbType.Int;
                                //TODO: deal with users moving backwards and forwards between pages
                                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure,
                                    ConfigurationManager.AppSettings["eFormsFlagFormAsCompleteSP"], prm);

                            }
                            // we are not interested in the first 2 params so start at 2
                            int i = 0;
                            foreach (object o in parameters)
                            {
                                if (i >= 2)
                                {
                                    SqlParameter p = o as SqlParameter;
                                    p.Value = DBNull.Value;
                                }
                                i++;
                            }
                        }
                        catch (SqlException ex)
                        {
                            ExceptionManager.Publish(ex);
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                try
                {
                    if (citizenId == 0)
                    {
                        //TODO: This should insert citizen details and flag them as current (the latest addition) do we need to do anything else?
                        SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure,
                            ConfigurationManager.AppSettings["eFormsInsertCitizenAnonymousSP"], citizenParams);
                        //David changed 22/07/05  citizenid from string to int
                        citizenId = Convert.ToInt32(citizenParams[7].Value);
                    }
                    foreach (string key in slEmailAddresses.Keys)
                    {
                        SqlParameter[] prms = new SqlParameter[2];
                        SqlParameter prm = new SqlParameter();
                        prm.SqlDbType = SqlDbType.VarChar;
                        prm.Size = 255;
                        prm.ParameterName = "@EmailAddress";
                        prm.Value = slEmailAddresses[key];
                        prms[0] = prm;
                        SqlParameter prm2 = new SqlParameter();
                        prm2.SqlDbType = SqlDbType.Int;
                        prm2.Size = 4;
                        prm2.ParameterName = "@CitizenID";
                        prm2.Value = citizenId;
                        prms[1] = prm2;

                        //TODO: deal with users moving backwards and forwards between pages
                        SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure,
                            ConfigurationManager.AppSettings["eFormsCitizenEmailAddressInsertSP"], prms);
                    }

                    foreach (string key in slPhoneNumbers.Keys)
                    {
                        SqlParameter[] prms = new SqlParameter[3];
                        SqlParameter prm = new SqlParameter();
                        prm.SqlDbType = SqlDbType.VarChar;
                        prm.Size = 30;
                        prm.ParameterName = "@telNo";
                        prm.Value = slPhoneNumbers[key];
                        prms[0] = prm;

                        SqlParameter prm2 = new SqlParameter();
                        prm2.SqlDbType = SqlDbType.Int;
                        prm2.Size = 4;
                        prm2.ParameterName = "@CitizenID";
                        prm2.Value = citizenId;
                        prms[1] = prm2;

                        SqlParameter prm3 = new SqlParameter();
                        prm3.ParameterName = "@PhoneType";
                        prm3.SqlDbType = SqlDbType.VarChar;
                        prm3.Size = 8;

                        string newKey = Regex.Replace(key, "\\d", "").ToString();

                        switch (newKey)
                        {
                            case "HomePhone":
                                prm3.Value = "IsPhone";
                                break;
                            case "WorkPhone":
                                prm3.Value = "IsPhone";
                                break;
                            case "WorkMobile":
                                prm3.Value = "IsMobile";
                                break;
                            case "PersonalMobile":
                                prm3.Value = "IsMobile";
                                break;
                            case "WorkFax":
                                prm3.Value = "IsFax";
                                break;
                            case "HomeFax":
                                prm3.Value = "IsFax";
                                break;
                            default:
                                break;
                        }

                        prms[2] = prm3;
                        //TODO: deal with users moving backwards and forwards between pages 
                        // now call stored proc
                        SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure,
                            ConfigurationManager.AppSettings["eformsCitizenTelephoneNoInsertAnonymousSP"], prms);
                    }
                    //TODO: check this is working
                    savedPages.Add(requiredPage.ToString());
                    HttpContext.Current.Session["savedPages"] = savedPages;
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Publish(ex);
                    transaction.Rollback();
                    throw;
                }

                transaction.Commit();

                /*	///CODE_CHANGE 22/02/2006
                    ///Email notification - call webservice and pass Eform AD Group name
                    ///

                    string conn = ConfigurationManager.AppSettings["DbConnectionStringEformsReader"];
                    SqlConnection con = new SqlConnection(conn);
				
                    SqlParameter[] sqlparams = new SqlParameter[2];
                    sqlparams[0] =  new SqlParameter("@FormInstanceID", SqlDbType.Int);
                    sqlparams[0].Value = formInstanceID;

                    sqlparams[1] = new SqlParameter("@AdminGroupName", SqlDbType.VarChar,200);
                    sqlparams[1].Direction = ParameterDirection.Output;

                    SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, ConfigurationManager.AppSettings["eFormsGetAdminGroupName"],sqlparams);
				
                    EformsAdminEmailNotification emailAdminUser = new EformsAdminEmailNotification();
                    string AdminGroupName = "";
						
                        try
                        {
                            AdminGroupName =  sqlparams[1].Value.ToString();
                            emailAdminUser.SendEmailNotification("IG_Eform_Test");//AdminGroupName);
                        }
                        catch (Exception emailEx)
                        {
                            ExceptionManager.Publish(emailEx);
                        }
                        */

            }
            catch (SqlException ex)
            {
                ExceptionManager.Publish(ex);
                transaction.Rollback();
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                transaction.Rollback();
                throw;
            }
            finally
            {

                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                    transaction = null;
                }

                HttpContext.Current.Session["pafaddress"] = null;

                if (pageCount == requiredPage)
                {
                    HttpContext.Current.Session.Clear();
                }
            }
            return formInstanceID;
        }
        /// <summary>
        /// method adds a generic error meessage to a label which is added to the control collection of a placeholder on the form page
        /// </summary>
        public void PresentErrorMessage(PlaceHolder placeHolder)
        {
            string err = "An error occured while processing the form. Please try resubmitting the form. If the error persists please contact East Sussex County Council Service Desk on 01273 481234 or email corporate service desk at corporateservdesk@eastsussex.gov.uk.";
            Label lbl = new Label();
            lbl.CssClass = "warning";
            lbl.Text = err;
            placeHolder.Controls.AddAt(4, lbl);
        }
        /// <summary>
        /// method for generating a thank you message following successful form submission.
        /// the actual message is stored as a question in the database of type 'Confirmation'
        /// </summary>
        /// <param name="placeHolder">the placeholder on the page which is used to house the rendered form prior to submission.</param>
        /// <param name="FormInstanceID">the formInstanceID of the submitted form</param>
        public void ThankUser(PlaceHolder placeHolder, int FormInstanceID)
        {



            //the only question we need to retrieve is the thank you message
            //there is no need to create an instance of a generic form 
            //we can simply create a small set of form controls here and populate the placeholder for rendering on the page.
            string conn = ConfigurationManager.AppSettings["DbConnectionStringEformsReader"];
            SqlConnection cn = new SqlConnection(conn);
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@formInstanceID", SqlDbType.Int, 4);
            prms[0].Value = FormInstanceID;
            object retVal = SqlHelper.ExecuteScalar(cn, ConfigurationManager.AppSettings["eformsConfirmationSP"], prms);
            string message = retVal as string;


            if (!form.ShowSummary)
            {
                LiteralControl formBoxOpen = new LiteralControl("<div class=\"formBox\"><div id=\"topedge\" class=\"cornerTR\"><div class = \"cornerTL\"></div></div> ");
                placeHolder.Controls.Add(formBoxOpen);
            }


            //LiteralControl formPartOpen = new LiteralControl("<div class=\"formPart\">");
            //placeHolder.Controls.Add(formPartOpen);
            HtmlGenericControl confirmationMessage = new HtmlGenericControl("p");

            if (form.IsSurvey)
            {
                confirmationMessage.InnerHtml = message;
            }
            else
            {
                confirmationMessage.InnerHtml = message + " Your transaction number is " + "esccform" + FormInstanceID;

            }
            placeHolder.Controls.Add(confirmationMessage);
            //LiteralControl formPartClose = new LiteralControl("</div>");
            //placeHolder.Controls.Add(formPartClose);

            if (!form.ShowSummary)
            {
                LiteralControl formBoxClose = new LiteralControl("<div id=\"bottomedge\" + class=\"cornerBR\"><div class = \"cornerBL\"></div></div></div>");
                placeHolder.Controls.Add(formBoxClose);
            }
            else
            {
                CreateSummary(placeHolder, FormInstanceID);
            }


        }

        /// <summary>
        /// 23/01/2007 - originally for blue badge project
        /// Create a summary page for users to print off details after they have submitted the form to ESCC
        /// </summary>
        /// <param name="placeHolder"></param>
        /// <param name="FormInstanceID"></param>
        public PlaceHolder CreateSummary(PlaceHolder placeHolder, int FormInstanceID)
        {
            GenericForm qaForm = CreateForm(FormInstanceID);


            /******
             * 
             * TODO - need to refactor the code build a method to strip q&A render code into new method and then call it from setupform or summary saves two lots of code.
             * 
             * *****/




            HtmlGenericControl summaryMessage = new HtmlGenericControl("p");
            summaryMessage.InnerHtml = "Please print this page if you wish to keep a copy of your answers.";
            placeHolder.Controls.AddAt(1, summaryMessage);

            //Create the yellow form box opening tag
            LiteralControl formBoxOpen = new LiteralControl("<div class=\"formBox\"><div id=\"topedge\" class=\"cornerTR\"><div class = \"cornerTL\"></div></div> ");
            placeHolder.Controls.Add(formBoxOpen);


            //Boolean to test whether there is any question text to render
            bool hasLabel = false;


            //Loop through the form question collection. If its a fieldsetOpen with a following
            //Checkbox as the preceding control we have to call a seperate method 'GenerateCheckBoxHtml'
            //to do so bespoke rendering. This is because of the problems of multiple answers to a question
            for (int i = 0; i < qaForm.FormQuestionCollection.Count; i++)
            {
                FormQuestion question = qaForm.FormQuestionCollection[i];



                if (question.QuestionType == "fieldsetOpen")
                {
                    if (qaForm.FormQuestionCollection[i + 1].BaseControl.GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                    {

                        for (int k = i; k < qaForm.FormQuestionCollection.Count; k++)
                        {
                            if (question.QuestionType == "fieldsetClose")
                            {
                                PlaceHolder ph = GenerateCheckBoxHtml(i, k);

                                placeHolder.Controls.Add(ph);
                                i = k;
                                break;
                            }
                        }
                    }

                }
                if (question.QuestionType != "Text" & question.QuestionType != "Heading" & question.QuestionType != "Confirmation")
                {



                    //Generate the HTML to do with a question label
                    if (question.QuestionText.Length > 0)
                    {
                        hasLabel = true;

                        //If Question label then open formpart tag
                        LiteralControl formPartOpen = new LiteralControl("<div class=\"formPart\">");
                        placeHolder.Controls.Add(formPartOpen);


                        //Label lbl = question.QuestionLabel as Label;
                        Label lbl = new Label();

                        //With dates we have to render the question ref text slightly differently
                        if (question.AnswerDataType == "Date")
                        {
                            lbl.Text = question.QuestionRef + question.QuestionText.Replace(question.QuestionRef, "");
                            lbl.CssClass = "formLabel";
                        }
                        else
                        {
                            lbl.Text = question.QuestionRef + question.QuestionText;
                            lbl.CssClass = "formLabel";
                        }

                        //The DatePartFormControl need to have its label visibility property overridden
                        //Otherwise the admin eform detail page did not display correctly
                        if (question.AnswerType == "SimpleDate")
                        {
                            lbl.Visible = true;

                        }

                        //Add question label to placeholder
                        placeHolder.Controls.Add(lbl);

                    }
                }

                if (question.Answer != null)
                {
                    Label AnswerLabel = new Label();

                    if (question.AnswerDataType.ToString() == "Date")
                    {

                        if (question.Answer.ToString().Length == 0)
                        {
                            AnswerLabel.Text = "";
                        }
                        else
                        {

                            AnswerLabel.Text = DateTimeFormatter.FullBritishDateWithDay(DateTime.Parse(question.Answer.ToString()));
                        }
                    }
                    else
                    {
                        AnswerLabel.Text = question.Answer.ToString();
                    }

                    AnswerLabel.CssClass = "";// "formControl formAnswer";
                    placeHolder.Controls.Add(AnswerLabel);



                }

                if (hasLabel)
                {
                    LiteralControl formPartClose = new LiteralControl("</div>");
                    placeHolder.Controls.Add(formPartClose);
                    hasLabel = false;
                }








            }

            LiteralControl formBoxClose = new LiteralControl("<div id=\"bottomedge\" + class=\"cornerBR\"><div class = \"cornerBL\"></div></div></div>");
            placeHolder.Controls.Add(formBoxClose);

            return placeHolder;
        }

        //		public void PopulateSavedPageAnswers(int formInstanceID, int pageNumber)
        //		{
        //
        //		}

        #region private methods
        ///TODO: complete documentation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private bool CheckIfSaved(string page)
        {
            bool result = false;
            savedPages = HttpContext.Current.Session["savedPages"] as ArrayList;
            foreach (string item in savedPages)
            {
                if (item == page)
                {
                    result = true;
                }
            }
            return result;
        }
        /*  /// <summary>
          /// Inserts a span tag containing an asterisk inside a fieldset legend tag for required questions
          /// </summary>
          /// <seealso cref="AsteriskHelper">AsteriskHelper() helper method</seealso>
              private void AddAsterisk()
              {	
                  for (int i = 0; i < this.form.FormQuestionCollection.Count; i++)
                  {
                      FormQuestion question = this.form.FormQuestionCollection[i];

                      switch (question.AnswerType)
                      {
                          case "RadioButtonListVarchar":
                              if (question.IsRequired)
                              {
                                  int index = i -1;
                                  question = this.form.FormQuestionCollection[index];
                                  AsteriskHelper(question);
                              }
                              break;		
		
                          case "CheckBoxListVarchar":
                              if (question.IsRequired)
                              {
                                  int index = i -1;
                                  question = this.form.FormQuestionCollection[index];
                                  AsteriskHelper(question);
                              }
                              break;
		
                          case "RadioButtonListInteger":
                              if (question.IsRequired)
                              {
                                  int index = i -1;
                                  question = this.form.FormQuestionCollection[index];
                                  AsteriskHelper(question);
                              }
                              break;		

                          case "CheckBoxListInteger":
                              if (question.IsRequired)
                              {
                                  int index = i -1;
                                  question = this.form.FormQuestionCollection[index];
                                  AsteriskHelper(question);
                              }
                              break;
                          case "DropDownListVarchar":
                              if (question.IsRequired)
                              {
                                  int index = i - 1;
                                  question = this.form.FormQuestionCollection[index];
                                  AsteriskHelper(question);
                              }
                              break;
                          case "Title":
                              if (question.IsRequired)
                              {
                                  int index = i - 1;
                                  question = this.form.FormQuestionCollection[index];
                                  AsteriskHelper(question);
                              }
                              break;
                  	
                      }		
                  }		
              }
           * */
        /*   /// <summary>
           /// Inserts a span tag containing an asterisk inside a fieldset legend tag where any of the control types defined 
           /// in the AddAsterisk() method is a required field.
           /// </summary>
           /// <param name="question">The form question; which has a question type of fieldsetOpen and immediately precedes
           /// one of the control types defined in the AddAsterisk() method.
           ///	</param>
           ///	<seealso cref="AddAsterisk()">The calling AddAsterisk() method</seealso>
           private void AsteriskHelper(FormQuestion question)
           {
               LiteralControl litCtrl = question.BaseControl as LiteralControl;
               string textAppend = "<span class=\"requiredField\">*</span>";
               int idx = litCtrl.Text.LastIndexOf("<");
               litCtrl.Text = litCtrl.Text.Insert(litCtrl.Text.LastIndexOf("<"), textAppend);
           }
           */
        /// <summary>
        /// This helper method loops through form fields to extract address information. Field
        /// values are concatenated for saving as a question answer.
        /// </summary>
        /// <returns>A concatenated address string</returns>
        private string AddressAsAnswer()
        {
            StringBuilder sb = new StringBuilder();
            string[] keys = HttpContext.Current.Request.Form.AllKeys;
            // gets a string from web config of the form "Saon, paon, streetDescriptor, locality, town, administrativeArea, postcode";
            string addressFields = ConfigurationManager.AppSettings["eFormsAddressFields"];
            string[] names = addressFields.Split(Convert.ToChar(","));
            int fieldCount = 0;
            foreach (string key in keys)
            {
                foreach (string name in names)
                {
                    string temp = name.Trim();
                    Regex regex = new Regex(@"[A-Za-z0-9_]+(:|\$)" + temp);
                    if (regex.IsMatch(key))
                    {
                        // This method is called each time we need to process a custom address.
                        // Since we loop through all controls on the page each time the method is called we need to 
                        // count the number of fields processed and store at class level. This is then compared against the
                        // fieldCount variable; if the field count is less than the processedFieldCount we know the field has 
                        // already been dealt with and ignore it. If the fieldCount is greater than the processedFieldCount 
                        // we append the field value to the srtingbuilder. We then check the modulus of the fieldCount 
                        // using the number of fields in the control (this is always equal to the names string array length value.
                        // e.g. if fieldCount Mod names.Count == 0 then we have dealt with each field in the control and can exit 
                        // the method; this ensures that we get a concatenated address string for each address control on a form.

                        // increment this value to count the number of fields dealt with in a single method call.
                        fieldCount++;
                        if (context.Request.Form[key] != "" & context.Request.Form[key] != null)
                        {
                            if (fieldCount != 0 & fieldCount > processedFieldCount)
                            {
                                sb.Append(context.Request.Form[key] + ", ");
                                if (fieldCount % names.Length == 0)
                                {
                                    // increment this value by the fieldCount every time we finish processing an address control (across multiple method calls).
                                    processedFieldCount += fieldCount;
                                    sb = sb.Remove(sb.Length - 1, 1);
                                    sb = StripTrailingCommas(sb);
                                    return sb.ToString();
                                }
                            }
                        }
                    }
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Helper method removes trailing commas from a concatenated address.
        /// </summary>
        /// <param name="sb">A stringbuilder</param>
        /// <returns>A string builder</returns>
        private StringBuilder StripTrailingCommas(StringBuilder sb)
        {
            if (sb[sb.Length - 1] == Convert.ToChar(","))
            {
                sb = sb.Remove(sb.Length - 1, 1);
                StripTrailingCommas(sb);
            }
            return sb;
        }


        private void CreatePossibleAnswers(DataTable dt, FormQuestion formQuestion)
        {
            foreach (DataRow dr in dt.Rows)
            {
                //poss answer display name

                if (dr["posAnswerDisplayName"].ToString() != null & dr["posAnswerDisplayName"].ToString() != "")
                {
                    formQuestion.PosAnswerDisplayName = dr["posAnswerDisplayName"].ToString();
                }


                //TODO: Potential issue if more than one possible value type has been entered in the DB

                if (dr["posAnswerVarchar"].ToString() != "")
                {
                    //Varchar
                    formQuestion.PosAnswerValue = dr["posAnswerVarchar"].ToString();
                }
                else if (dr["posAnswerInteger"].ToString() != "")
                {
                    //Integer
                    formQuestion.PosAnswerValue = dr["posAnswerInteger"].ToString();
                }
                else if (dr["posAnswerDate"].ToString() != "")
                {
                    //Date
                    formQuestion.PosAnswerValue = dr["posAnswerDate"].ToString();
                }
                else
                {
                    //Decimal
                    formQuestion.PosAnswerValue = dr["posAnswerDecimal"].ToString();

                }



            }

        }


        /// <summary>
        /// The method creates a dataset with all the questions, possible answers and dependencies
        /// for a given form. The dataset is then broken down in the BuildQuestionDataTables method
        /// into seperate tables for each questionID and added into a EformsDataTableCollection.
        /// 
        /// Next we create a Generic Form object to represent our html form. This form will hold all our
        /// questions, dependencies etc and is returned by the method.
        /// 
        /// Each table in the EformsDataTableCollection is then in turn looped through and question objects
        /// are instantated and assigned their properities (including possible answers). After each question
        /// object is created its dependencies are created and assigned to a dependency object which in turn
        /// is added to a QuestionDependencyCollection. 
        /// 
        /// Finally the question object is added to a FormQuestionCollection and returned.
        /// 
        /// FormID is passed in from the query string.
        ///   </summary> 
        /// <returns>GenericForm</returns>
        private GenericForm CreateForm()
        {

            /********************************** CREATE QUESTION TABLES *********************************************************/
            DataSet ds = GetDataSet();
            if (requiredPage > pageCount)
            {
                return FailureToLoadForm(new PlaceHolder());
            }
            dtcQuestionCollection = new EformsDataTableCollection();

            //Loop through dataset seperating each question and it associates into distinct tables
            BuildQuestionDataTables(ds, "QuestionID", dtcQuestionCollection);

            // create pages if required
            if (pageCount > 1)
            {
                // creates the eformspagecollection epc
                CreatePageCollection(dtcQuestionCollection);
            }



            GenericForm form = new GenericForm(formID);

            EformsDataTableCollection edtc;
            // epc = eformspagecollection
            if (epc != null)
            {
                edtc = epc[requiredPage - 1];
            }
            else
            {
                edtc = dtcQuestionCollection;
            }
            /***************************************** BIND QUESTION VALUES **************************************************************/

            //Generate all the questions on a per table basis and add them into a questions collection
            foreach (DataTable dt in edtc)
            {
                FormQuestion formQuestion = new FormQuestion();



                //Set some of the question properties to the records returned form the eforms database

                formQuestion.FormId = formID;
                formQuestion.QuestionId = Convert.ToInt32(dt.Rows[0]["QuestionId"]);
                formQuestion.QuestionRef = dt.Rows[0]["QuestionRef"].ToString();
                formQuestion.QuestionType = dt.Rows[0]["QuestionType"].ToString();
                formQuestion.AnswerType = dt.Rows[0]["AnswerType"].ToString();
                formQuestion.QuestionText = dt.Rows[0]["QuestionText"].ToString();
                formQuestion.QuestionHelp = dt.Rows[0]["QuestionHelp"].ToString();
                formQuestion.SecNo = Convert.ToInt32(dt.Rows[0]["SectionNo"]);
                formQuestion.SubSecNo = Convert.ToInt32(dt.Rows[0]["SubSectionNo"]);
                formQuestion.PageNo = Convert.ToInt32(dt.Rows[0]["PageNo"]);
                formQuestion.SeqNo = Convert.ToInt32(dt.Rows[0]["SeqNo"]);
                formQuestion.ErrorMessage = dt.Rows[0]["ErrorMessage"].ToString();
                formQuestion.ValidationTemplate = dt.Rows[0]["ValidationTemplate"].ToString();
                formQuestion.CssClassName = dt.Rows[0]["questionCss"].ToString();
                formQuestion.CssLabelClassName = dt.Rows[0]["LabelCSS"].ToString();
                formQuestion.DefaultAnswerText = dt.Rows[0]["DefaultAnswerText"].ToString();
                formQuestion.DefaultAnswerVarchar = dt.Rows[0]["DefaultAnswerVarchar"].ToString();
                formQuestion.AnswerDataType = dt.Rows[0]["DataType"].ToString();
                formQuestion.IsRequired = Convert.ToBoolean(dt.Rows[0]["IsRequired"]);





                if (dt.Rows[0]["StaffOnly"] != DBNull.Value)
                {
                    formQuestion.IsStaffOnly = Convert.ToBoolean(dt.Rows[0]["StaffOnly"]);

                }
                else
                {
                    formQuestion.IsStaffOnly = false;
                }
                string answerType = "";


                if ((formQuestion.AnswerType == null) || (formQuestion.AnswerType.Length == 0))
                {
                    answerType = formQuestion.QuestionType;
                }
                else
                {
                    answerType = formQuestion.AnswerType;
                }

                if (formQuestion.QuestionType == "Heading")
                {
                    form.Title = formQuestion.QuestionText;
                }

                formQuestion.SetControlType(answerType);
                CreatePossibleAnswers(dt, formQuestion);




                /***************************************** CREATE DEPENDENCIES *************************************************************/
                DataSet dataset = GetDependencies(formQuestion.QuestionId);

                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    QuestionDependency dependency = new QuestionDependency();

                    dependency.DependencyId = Convert.ToInt32(dr["DependencyId"]);
                    dependency.MainQuestionId = Convert.ToInt32(dr["MainQuestionId"]);
                    dependency.DependentQuestionId = Convert.ToInt32(dr["DependentQuestionId"]);
                    dependency.ComparisonOperator = dr["ComparisonOperator"].ToString();
                    dependency.DependencyRef = dr["QuestionRef"].ToString();
                    dependency.AnswerType = dr["AnswerType"].ToString();

                    if (dr["ComparisonVarchar"].ToString().Length > 0)
                    {

                        string temp = dr["ComparisonVarchar"].ToString();
                        if (temp == "\"\"")
                        {
                            temp = "";
                        }
                        dependency.ComparisonVarchar = temp;
                    }


                    if (dr["ComparisonInteger"] != DBNull.Value)
                    {
                        dependency.ComparisonInteger = Convert.ToInt32(dr["ComparisonInteger"]);
                    }


                    if (dr["ComparisonDecimal"] != DBNull.Value)
                    {
                        dependency.ComparisonDecimal = Convert.ToDouble(dr["ComparisonDecimal"]);

                    }

                    if (dr["ComparisonDate"] != DBNull.Value)
                    {
                        dependency.ComparisonDate = Convert.ToDateTime(dr["ComparisonDate"]);
                    }


                    if (dr["ComparisonBit"] != DBNull.Value)
                    {
                        dependency.ComparisonBool = Convert.ToBoolean(dr["ComparisonBit"]);
                    }


                    formQuestion.QuestionDependencyCollection.Add(dependency);

                }


                form.FormQuestionCollection.Add(formQuestion);





            }

            return form;
        }
        /// <summary>
        /// Overload of CreateForm used by admin system
        /// </summary>
        /// <param name="FormInstanceID"></param>
        /// <returns></returns>
        private GenericForm CreateForm(int FormInstanceID)
        {

            /********************************** CREATE QUESTION TABLES *********************************************************/

            DataSet ds = GetQuestionAnswerDataSet(FormInstanceID);


            GenericForm form = new GenericForm();

            /***************************************** BIND QUESTION VALUES **************************************************************/
            int count = -1;
            //Generate all the questions on a per table basis and add them into a questions collection
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                count++;
                FormQuestion formQuestion = new FormQuestion();


                formQuestion.FormId = Convert.ToInt32(dr["FormID"]); //formID;
                formQuestion.QuestionId = Convert.ToInt32(dr["QuestionId"]);
                formQuestion.QuestionText = dr["QuestionText"].ToString();
                formQuestion.QuestionRef = dr["QuestionRef"].ToString();
                formQuestion.AnswerType = dr["AnswerType"].ToString();
                formQuestion.SeqNo = Convert.ToInt32(dr["SeqNo"]);
                formQuestion.QuestionType = dr["QuestionType"].ToString();

                if ((formQuestion.AnswerType == "RadioButtonListVarchar") || (formQuestion.AnswerType == "RadioButtonListInteger") ||
                    (formQuestion.AnswerType == "CheckBoxVarchar") || (formQuestion.AnswerType == "CheckBoxInteger") || (formQuestion.AnswerType == "Title") || (formQuestion.AnswerType == "CheckBoxListVarchar") || (formQuestion.AnswerType == "DropDownListVarchar"))
                {
                    count++;
                    foreach (DataRow dr2 in ds.Tables[2].Rows)
                    {
                        if (Convert.ToInt32(dr2["SeqNo"]) == formQuestion.SeqNo - 1)
                        {
                            if ((formQuestion.AnswerType == "CheckBoxVarchar") || (formQuestion.AnswerType == "CheckBoxInteger") || (formQuestion.AnswerType == "CheckBoxListVarchar") || (formQuestion.AnswerType == "DropDownListVarchar"))
                            {
                                string qref = dr2["QuestionRef"].ToString();
                                foreach (DataRow drx in ds.Tables[2].Rows)
                                {
                                    if (drx["QuestionRef"].ToString() == qref)
                                    {
                                        if (drx["QuestionType"].ToString() == "fieldsetOpen")
                                        {
                                            // add a new question now for the heading
                                            FormQuestion newFormQuestion = new FormQuestion();
                                            newFormQuestion.FormId = formQuestion.FormId;
                                            //needs to be the previous datarow								
                                            newFormQuestion.QuestionId = Convert.ToInt32(drx["QuestionId"]);
                                            newFormQuestion.QuestionText = drx["QuestionText"].ToString();
                                            newFormQuestion.QuestionRef = drx["QuestionRef"].ToString();

                                            newFormQuestion.SeqNo = Convert.ToInt32(drx["SeqNo"]);
                                            newFormQuestion.QuestionType = drx["QuestionType"].ToString();
                                            form.FormQuestionCollection.Add(newFormQuestion);
                                        }

                                    }
                                }


                            }
                            else
                            {

                                formQuestion.QuestionText = dr2["QuestionText"].ToString();
                                formQuestion.QuestionRef = dr2["QuestionRef"].ToString();
                            }
                        }
                    }
                }



                formQuestion.CssClassName = dr["questionCss"].ToString();
                formQuestion.CssLabelClassName = dr["LabelCSS"].ToString();


                formQuestion.AnswerDataType = dr["DataType"].ToString();

                switch (formQuestion.AnswerDataType)
                {
                    case "Varchar":

                        formQuestion.Answer = dr["AnswerVarchar"].ToString();

                        break;

                    case "Text":
                        formQuestion.Answer = dr["AnswerText"].ToString();

                        break;

                    case "Integer":
                        formQuestion.Answer = dr["AnswerInteger"].ToString();

                        break;

                    case "Date":
                        formQuestion.Answer = dr["AnswerDate"].ToString();

                        break;

                    case "Double":
                        formQuestion.Answer = dr["AnswerDecimal"].ToString();

                        break;

                    case "Bit":
                        formQuestion.Answer = dr["AnswerBit"].ToString();

                        break;

                    default:
                        break;

                }

                formQuestion.QuestionType = dr["QuestionType"].ToString();

                //TODO: look at this later will need to be changed
                if (dr["StaffOnly"] != DBNull.Value)
                {
                    formQuestion.IsStaffOnly = Convert.ToBoolean(dr["StaffOnly"]);
                }
                else
                {
                    formQuestion.IsStaffOnly = false;
                }

                string answerType = "";

                if ((formQuestion.AnswerType == null) || (formQuestion.AnswerType.Length == 0))
                {
                    answerType = formQuestion.QuestionType;
                }
                else
                {
                    answerType = formQuestion.AnswerType;
                }

                // Set the control type last to make sure the values we assign already exist
                formQuestion.SetControlType(answerType);

                form.FormQuestionCollection.Add(formQuestion);
            }
            //Assign the page heading
            if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0].ToString()))
            {
                form.Title = ds.Tables[1].Rows[0].ItemArray[0].ToString();
            }


            return form;
        }
        /// <summary>
        /// Return a dataset with all the questions and users submitted answers for the admin system
        /// </summary>
        /// <param name="FormInstanceID"></param>
        /// <returns></returns>
        private DataSet GetQuestionAnswerDataSet(int FormInstanceID)
        {
            DataSet ds = new DataSet();
            string Conn = ConfigurationManager.AppSettings["DbConnectionAdmin"];
            SqlConnection cn = new SqlConnection(Conn);
            SqlParameter parameter = new SqlParameter("@formInstanceID", SqlDbType.BigInt);

            /*
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "usp_GetEformQuestionsAnswers";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            parameter.Value = FormInstanceID;
            cmd.Parameters.Add(parameter);
			
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            */
            parameter.Value = FormInstanceID;

            ds = SqlHelper.ExecuteDataset(cn, CommandType.StoredProcedure, ConfigurationManager.AppSettings["usp_GetEformQuestionsAnswers"], parameter);




            return ds;
        }
        /// <summary>
        /// This method loops through the dataset(DataTable) and compares each row with the next. 
        /// When a difference between the comparison field values is found
        /// a helper method is called to grab the prior sequence of rows. 
        /// The rows are then removed from the original datatable and added to a new dataTable.
        /// The dataTable is then added into the EformsDataTableCollection. 
        /// This method is called recursively. The value of 'dt.Rows.Count' in the 
        /// for condition is reduced by the number of rows removed.
        /// </summary>
        /// <param name="dt"> Is a single table returned from GetDataSet()</param>
        /// <param name="comparisonField">QuestionId</param>
        /// <param name="dtc">EformsDataTableCollection</param>
        private void BuildQuestionDataTables(DataTable dt, string comparisonField, EformsDataTableCollection dtc)
        {
            // ****need to check for sequential order now
            for (int n = 0; n < dt.Rows.Count; n++)
            {

            }
            // Loop through the datatable and compare each row with the next. 
            // When a difference between the comparison field values is found
            // a helper method is called to grab the prior sequence of rows. 
            // The rows are then removed from the original datatable and 
            // this method is called recursively. The value of 'dt.Rows.Count' in the for 
            // condition has been reduced by the number of rows removed.
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // grab the first row
                    DataRow dr = dt.Rows[i];
                    // get a null datarow to hold the next row in the table
                    DataRow dr2 = null;
                    // before assigning dr2 a value make sure we do not try 
                    // to grab a row using an indexer greater than the number of rows
                    if (i < dt.Rows.Count - 1)
                    {
                        dr2 = dt.Rows[i + 1];
                    }
                    // check for when the comparison field values are different
                    // this occurs when a sequence of similar values changes
                    if (dr2 != null && dr[comparisonField].ToString() != dr2[comparisonField].ToString())
                    {
                        // call a helper method which creates a datatable containing 
                        // only those rows in the previous sequence and adds it to a collection
                        // we pass in the datatable along with the index position of the 
                        // last row in the sequence 
                        // do something with the results
                        dtc.Add(CreateQuestionFromResults(dt, i + 1));
                        // remove the previous sequence of rows from the datatable
                        for (int j = 0; j < i + 1; j++)
                        {
                            dt.Rows.RemoveAt(0);
                            // must call this method to ensure the row is completely removed
                            dt.AcceptChanges();
                        }
                        // make a recursive method call passing in the modified dataset
                        // along with the comparison field string value
                        BuildQuestionDataTables(dt, comparisonField, dtc);
                    }
                    // when there are no more rows available to remove
                    // call our helper method one final time
                    // no further recursive calls to this method are now neccesary
                    if (dt.Rows.Count == i + 1)
                    {
                        dtc.Add(CreateQuestionFromResults(dt, i + 1));
                        // must remove the final rows from the datatable to ensure 
                        // dt.Rows.Count = 0 and the for loop is exited
                        for (int j = 0; j < i + 1; j++)
                        {
                            dt.Rows.RemoveAt(0);
                            dt.AcceptChanges();
                        }
                        dt = null;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Overloaded method 
        /// Recursive method for extracting sequential groups of rows from 
        /// a single table dataset, based on like field values. 
        /// The field name must be supplied along with the single table dataset
        /// which must be ordered sequentially by the comparison field values. 
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="comparisonField">String</param>
        /// <param name="dtc">EformsDataTableCollection</param>
        private void BuildQuestionDataTables(DataSet ds, string comparisonField, EformsDataTableCollection dtc)
        {
            // check we only have one datatable
            if (ds.Tables.Count > 1)
            {
                // throw an exception if multiple datatables exist
                throw new Exception("DataSet can only contain one table! DataSet contains " + ds.Tables.Count.ToString() + " tables.");
            }

            BuildQuestionDataTables(ds.Tables[0], comparisonField, dtc);
        }
        /// <summary>
        /// Clones a datatable removes a subset of rows from index 0 to a supplied end index value
        /// from the original and populates the clone with these rows
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="idxEnd"></param>
        /// <returns>A DataTable</returns>
        private DataTable CreateQuestionFromResults(DataTable dt, int idxEnd)
        {
            // clone an empty dataset with the same schema as the original
            DataTable dtClone = dt.Clone();
            // copy the required rows to the dataset clone
            for (int i = 0; i < idxEnd; i++)
            {
                dtClone.ImportRow(dt.Rows[i]);
            }
            return dtClone;
        }
        /// <summary>
        /// Method spilts the form into pages
        /// </summary>
        /// <param name="dtc"></param>
        private void CreatePageCollection(EformsDataTableCollection dtc)
        {
            // instantiate our 'page container' which is a collection of datatable collection objects
            epc = new EformsPageCollection();
            // use the pageCount value and loop to add empty datatable collection objects 
            //corresponding to the number of pages
            for (int i = 0; i < pageCount; i++)
            {
                EformsDataTableCollection Page = new EformsDataTableCollection();
                epc.Add(Page);
            }
            // now loop through each datatable (each table corresponds to a single question) 
            //pull out the question's page number then
            // use this value to work out which datatable collection (i.e. page) to add the question to.
            foreach (DataTable dt in dtc)
            {
                int page = (int)dt.Rows[0]["PageNo"];
                epc[page - 1].Add(dt);
            }
        }
        /// <summary>
        /// This method returns all questions, possible answers and dependencies for a given form.
        /// </summary>
        /// <returns>DataSet</returns>
        private DataSet GetDataSet()
        {
            DataSet ds = new DataSet();
            //TODO: Remember to change the server name when we move to staging
            string Conn = ConfigurationManager.AppSettings["DbConnectionStringEformsReader"];

            SqlConnection cn = new SqlConnection(Conn);

            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = new SqlParameter("@formId", SqlDbType.Int);
            parameters[0].Value = formID;

            parameters[1] = new SqlParameter("@PageCount", SqlDbType.Int);
            parameters[1].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(cn, CommandType.StoredProcedure, ConfigurationManager.AppSettings["eFormsGetFormQuestions"], parameters);


            //CODEFIX 15/02/2006
            //Catches Invalid Cast Exception if user enters a form ID that doesn't return a page count
            try
            {
                pageCount = (int)parameters[1].Value;
            }
            catch (InvalidCastException ex)
            {
                pageCount = 0;
                ExceptionManager.Publish(ex);
            }




            return ds;

        }
        /// <summary>
        /// This method takes the quesitonID and returns a dataset
        /// of all of the question dependencies for the questionID.
        /// </summary>
        /// <param name="QuestionId"></param>
        /// <returns>DataSet</returns>
        private DataSet GetDependencies(int QuestionId)
        {

            DataSet ds = new DataSet();
            string Conn = ConfigurationManager.AppSettings["DbConnectionStringEformsReader"];
            SqlConnection cn = new SqlConnection(Conn);
            SqlParameter parameter = new SqlParameter("@questionId", SqlDbType.Int);
            parameter.Value = QuestionId;
            ds = SqlHelper.ExecuteDataset(cn, CommandType.StoredProcedure, ConfigurationManager.AppSettings["eFormsGetDependencies"], parameter);


            return ds;
        }
        /// <summary>
        /// Helper method which gets a dataset based on form ID
        /// </summary>
        /// <returns>DataSet</returns>
        private DataSet CreateDataSet()
        {
            DataSet ds = new DataSet();
            string Conn = ConfigurationManager.AppSettings["DbConnectionStringEformsReader"];

            SqlConnection cn = new SqlConnection(Conn);

            SqlParameter param = new SqlParameter("@formId", SqlDbType.Int);
            param.Value = formID;

            ds = SqlHelper.ExecuteDataset(cn, CommandType.StoredProcedure, ConfigurationManager.AppSettings["eFormsGetFormQuestions"], param);

            return ds;
        }
        #endregion
    }
}
