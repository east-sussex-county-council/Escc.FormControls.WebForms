using System;
using eastsussexgovuk.webservices.FormControls.genericforms;
using System.Web;
using System.Web.UI.WebControls;


namespace eastsussexgovuk.webservices.FormControls.genericforms
{
	/// <summary>
	/// DependencyManagement. Utility class for evaluating any question dependencies.
	/// </summary>
	public class DependencyManagement
	{
		private static int count = -1;	
	
		#region Constructor
		/// <summary>
		/// Class constructor.
		/// </summary>
		public DependencyManagement()
		{
		}
		#endregion
		#region public methods
		/// <summary>
		/// This method takes the Generic form and the user values (httpContext object)
		/// It then checks the users responses against the database dependencies and if
		/// they are different it generates a required field validator
		/// </summary>
		/// <param name="placeHolder">A place holder for any error message.</param>
		/// <param name="form">The form object whose dependencies we wish to evaluate.</param>
		/// <param name="context">The http context of the web page containing the form.</param>
		public static void EvaluateDependencies(PlaceHolder placeHolder, GenericForm form, HttpContext context)
		{

            // TODO: The dependency maybe x but the user enters y. The dependency will fail but is generating
            // a required validator really the correct option? I would have thought that a regular expression
            // validator would be more suitable. If so we need to have a validationtemplate in the db for this
            // question.
			
			foreach (FormQuestion question in form.FormQuestionCollection)
			{
			
				if ((question.AnswerType != null)&&(question.AnswerType != "")&&(question.QuestionType == "Question"))
				{

					int Id = question.QuestionId;
	

					if(question.QuestionDependencyCollection.Count > 0)
					{
						string frmQuestionValue = context.Request.Form[question.HtmlQuestionId];//Id.ToString()];

						if (frmQuestionValue == null)
						{
							frmQuestionValue = "";
						}

						string ComparisonOperator;
						string comparisonValue = null;
						bool result;

						//I had to change this because != doesn't work with null for all types
						foreach(QuestionDependency qd in question.QuestionDependencyCollection)
						{
							ComparisonOperator = qd.ComparisonOperator.Trim();
							if(qd.ComparisonVarchar != null)
							{
								comparisonValue = qd.ComparisonVarchar;
							}
							if(qd.ComparisonInteger > -1)
							{
								comparisonValue = qd.ComparisonInteger.ToString();
							}
							if(qd.ComparisonDate != DateTime.MinValue)
							{
								comparisonValue = qd.ComparisonDate.ToString();
							}
							if(qd.ComparisonDecimal > -1)
							{
								comparisonValue = qd.ComparisonDecimal.ToString();
							}
							// this always has a default of false so we cannot set on a dependency on false
							// use something else...need to trap this error at form creation time
							if(qd.ComparisonBool == true)
							{
								comparisonValue = qd.ComparisonBool.ToString();
							}
							result = DependencyManagement.DoComparison(ComparisonOperator, frmQuestionValue,  comparisonValue, question);
								
							// if true a dependency exists and needs to be fulfilled
							if (result)
				
							{
								//Generate error message
								
								RequiredFieldValidator rfvDependency  = new RequiredFieldValidator();

								if (qd.AnswerType == "SimpleDate")
								{
									rfvDependency.ControlToValidate = "day" + qd.DependentQuestionId.ToString();
								}
								else
								{
									//Code fix 12/12/2005 due to  broken dependencies caused by "q" (html ID fix) 
									//to make html XHTML valid 
									string htmlDependentQuesitonID = "q";
									htmlDependentQuesitonID += qd.DependentQuestionId.ToString();

									rfvDependency.ControlToValidate =  htmlDependentQuesitonID;



									//if dependent question is list type get legend
									

									foreach (FormQuestion quest in form.FormQuestionCollection)
									{
											count++;

										if (quest.QuestionId == qd.DependentQuestionId)
										{
											if ((quest.BaseControl.GetType().ToString() == "System.Web.UI.WebControls.RadioButtonList") || (quest.BaseControl.GetType().ToString() == "System.Web.UI.WebControls.CheckBox"))
											{
												FormQuestion previousQuestion = form.FormQuestionCollection[count -1] ;
											
												
												qd.DependencyRef = previousQuestion.QuestionRef;
											}
										}

									}
								}


								rfvDependency.Display = ValidatorDisplay.None;
								rfvDependency.EnableClientScript = false;

								
						//Hopefully the wording of the error message should cover both
						//events eg no answer or wrong answer.
						rfvDependency.ErrorMessage  = "You are required to answer question " + qd.DependencyRef.Replace(".", " ") + " please check your answer is correct"; 
								placeHolder.Controls.Add(rfvDependency);

							}		
						}
					}
					
				}
			}
		}
		
		/// <summary>
		/// This is the main method for comparing entered form values
		/// It takes the dependency comparison value and based on the type of 
		/// comparison operator casts
		/// </summary>
		/// <param name="ComparisonOperator"></param>
		/// <param name="formValue"></param>
		/// <param name="comparisonValue"></param>
		/// <param name="question"></param>
		/// <returns></returns>
		/// TODO: Should this be private?
		public static bool DoComparison(string ComparisonOperator, string formValue, string comparisonValue, FormQuestion question)
		{

			bool result = false;

				//find expected answerdatatype, convert the formvalue and comparison value to
				// the appropriate types and evaluate
							
				if ((question.AnswerDataType == "Varchar")||(question.AnswerDataType == "Text"))
				{
					result = DoStringComparison(ComparisonOperator, formValue, comparisonValue);
				}
				
				if (question.AnswerDataType == "Integer")
				{
					int fValue = Convert.ToInt32(formValue) ;
					int cValue = Convert.ToInt32(comparisonValue) ;

					result = DoInt32Comparison(ComparisonOperator,  fValue,  cValue);
				}	
					
				if (question.AnswerDataType == "Decimal")
				{
					double fValue = Convert.ToDouble(formValue) ;
					double cValue = Convert.ToDouble(comparisonValue) ;

					result = DoDoubleComparison(ComparisonOperator,  fValue,  cValue);
				}		

				if (question.AnswerDataType == "Bit")
				{
					bool fValue = Convert.ToBoolean(formValue) ;
					bool cValue = Convert.ToBoolean(comparisonValue) ;

					result = DoBooleanComparison(ComparisonOperator,  fValue,  cValue);
				}	
					
				if (question.AnswerDataType == "Date")
				{
					DateTime fValue = Convert.ToDateTime(formValue) ;
					DateTime cValue = Convert.ToDateTime(comparisonValue) ;

					result = DoDateTimeComparison(ComparisonOperator,  fValue,  cValue);
				}					

			
			return result;
		}

		#endregion
		#region private methods
		/// <summary>
		/// This method takes a string form vaule like text from a texbox
		/// and compares it against the comparison in the database. If there
		/// is a match the dependency is fulfilled and false is returned. If the
		/// dependency is still outstanding true is returned.
		/// </summary>
		/// <param name="ComparisonOperator"></param>
		/// <param name="formValue"></param>
		/// <param name="comparisonValue"></param>
		/// <returns></returns>
		private static bool DoStringComparison(string ComparisonOperator, string formValue, string comparisonValue)
		{
			bool result = false;

			switch (ComparisonOperator)
			{// here we are asking "is the user entered text data equal to a particular answer?" 
			// if so there is a dependency so we return true
				case "=":
					if (formValue == comparisonValue)
					{
						result = true;
					}
					break;
					// here we are asking "Is the user entered text data different from an expected value?"
					// if it is then we return true to indicate a dependency
				case "<>":
					if (formValue != comparisonValue)
					{
						result = true;
					}
					break;

				default:
					break;
			}

			return result;
		}
		
		/// This method takes an int form vaule like  a radiobutton
		/// and compares it against the comparison in the database. If there
		/// is a match the dependency is fulfilled and false is returned. If the
		/// dependency is still outstanding true is returned.
		private static bool DoInt32Comparison(string ComparisonOperator, int formValue, int comparisonValue)
		{
			bool result = false;
			switch(ComparisonOperator)
			{
				case "<=":
					if (formValue <= comparisonValue)
					{
						result = true;
					}
					break;

				case "<":
					if (formValue < comparisonValue)
					{
						result = true;
					}
					break;

				case ">=":
					if (formValue >= comparisonValue)
					{
						result = true;
					}
					break;

				case ">":
					if (formValue > comparisonValue)
					{
						result = true;
					}
					break;

				case "=":
					if (formValue == comparisonValue)
					{
						result = true;
					}
					break;

				default:
					break;
			}
			
			return result;
		}

		/// <summary>
		///This method takes a string form value like text from a texbox
		///	and compares it against the comparison in the database. If there
		///is a match the dependency is fulfilled and false is returned. If the
		///dependency is still outstanding true is returned.
		/// </summary>

		private static bool DoDoubleComparison(string ComparisonOperator, double formValue, double comparisonValue)
		{
			bool result = false;
			switch(ComparisonOperator)
			{
				case "<=":
					if (formValue <= comparisonValue)
					{
						result = true;
					}
					break;

				case "<":
					if (formValue < comparisonValue)
					{
						result = true;
					}
					break;

				case ">=":
					if (formValue >= comparisonValue)
					{
						result = true;
					}
					break;

				case ">":
					if (formValue > comparisonValue)
					{
						result = true;
					}
					break;

				case "=":
					if (formValue == comparisonValue)
					{
						result = true;
					}
					break;

				default:
					break;
			}
			
			return result;
		}


		/// <summary>
		/// Simple dateTime comparison
		/// </summary>
		/// <param name="ComparisonOperator"></param>
		/// <param name="formValue"></param>
		/// <param name="comparisonValue"></param>
		/// <returns></returns>
		private static bool DoDateTimeComparison(string ComparisonOperator, DateTime formValue, DateTime comparisonValue)
		{
			bool result = false;
			switch(ComparisonOperator)
			{
				case "<=":
					if (formValue <= comparisonValue)
					{
						result = true;
					}
					break;

				case "<":
					if (formValue < comparisonValue)
					{
						result = true;
					}
					break;

				case ">=":
					if (formValue >= comparisonValue)
					{
						result = true;
					}
					break;

				case ">":
					if (formValue > comparisonValue)
					{
						result = true;
					}
					break;

				case "=":
					if (formValue == comparisonValue)
					{
						result = true;
					}
					break;

				default:
					break;
			}
			
			return result;
		}
		
		/// <summary>
		/// Performs a boolean comparison.
		/// </summary>
		/// <param name="ComparisonOperator"></param>
		/// <param name="formValue"></param>
		/// <param name="comparisonValue"></param>
		/// <returns></returns>
		private static bool DoBooleanComparison(string ComparisonOperator, bool formValue, bool comparisonValue)
		{
			bool result = false;
			switch(ComparisonOperator)
			{
				
				case "=":
					if (formValue == comparisonValue)
					{
						result = true;
					}
					break;

				default:
					break;
			}
			
			return result;
		}
		#endregion
	}
}
