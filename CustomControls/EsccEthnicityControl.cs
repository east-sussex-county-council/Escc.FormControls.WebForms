using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using eastsussexgovuk.webservices.TextXhtml.HouseStyle;
using eastsussexgovuk.webservices.FormControls.Validators;
using System.Collections;
using System.Collections.Specialized;

namespace eastsussexgovuk.webservices.FormControls.CustomControls
{
	/// <summary>
	/// Summary description for EsccEthnicityControl.
	/// </summary>
	public class EsccEthnicityControl : WebControl, INamingContainer
	{

	
		#region fields

		bool required = true;
		string fullnameCssStyle = "";
		string fullnameLabelCssStyle = "";
		string reference = "";
		string questionText = "To which of these groups do you consider you belong?";
		string ethnicity = "";


		//GENERIC CONTROLS

		HtmlGenericControl fieldset = new HtmlGenericControl("fieldset");
		HtmlGenericControl legend = new	 HtmlGenericControl("legend");
		HtmlGenericControl intro = new HtmlGenericControl("p");
		HtmlGenericControl select = new HtmlGenericControl("select");
		HtmlGenericControl optgroupPleaseSelect = new HtmlGenericControl("optgroup");
		HtmlGenericControl optionPleaseSelect = new	HtmlGenericControl("option");
		HtmlGenericControl optgroupWhite = new HtmlGenericControl("optgroup");
		HtmlGenericControl optionWhiteBritish = new HtmlGenericControl("option");
		HtmlGenericControl optionWhiteIrish = new HtmlGenericControl("option");
		HtmlGenericControl optionWhiteGypsyRoma = new HtmlGenericControl("option");
		HtmlGenericControl optionWhiteIrishTraveller = new HtmlGenericControl("option");
		HtmlGenericControl optionWhiteOther = new HtmlGenericControl("option");
		HtmlGenericControl optgroupMixed = new HtmlGenericControl("optgroup");
		HtmlGenericControl optionMixedWhiteBlackCaribbean = new HtmlGenericControl("option");
		HtmlGenericControl optionMixedWhiteBlackAfrican = new HtmlGenericControl("option");
		HtmlGenericControl optionMixedWhiteAsian = new HtmlGenericControl("option");
		HtmlGenericControl optionMixedOther = new HtmlGenericControl("option");
		HtmlGenericControl optgroupBlackOrBlackBritish = new HtmlGenericControl("optgroup");
		HtmlGenericControl optionBlackOrBlackBritishCaribbean = new HtmlGenericControl("option");
		HtmlGenericControl optionBlackOrBlackBritishAfrican = new HtmlGenericControl("option");
		HtmlGenericControl optionBlackOrBlackBritishOther = new HtmlGenericControl("option");
		HtmlGenericControl optgroupAsianOrAsianBritish = new HtmlGenericControl("optgroup");
		HtmlGenericControl optionAsianOrAsianBritishIndian = new HtmlGenericControl("option");
		HtmlGenericControl optionAsianOrAsianBritishPakistani = new HtmlGenericControl("option");
		HtmlGenericControl optionAsianOrAsianBritishBangladeshi = new HtmlGenericControl("option");
		HtmlGenericControl optionAsianOrAsianBritishOther = new HtmlGenericControl("option");
		HtmlGenericControl optgroupChineseAndOther = new HtmlGenericControl("optgroup");
		HtmlGenericControl optionChineseAndOtherChinese  = new HtmlGenericControl("option");
		HtmlGenericControl optionChineseAndOtherChineseOther  = new HtmlGenericControl("option");


		TextBox txbOtherEthnicity = new TextBox();
		Label lblOtherEthnicity = new Label();
	
		#endregion

		#region properties

		

		
		public string Ethnicity
		{
			get
			{
				return this.ethnicity + " " + txbOtherEthnicity.Text;
			}
			
		}

		
		//CSS is present but not implement, at the moment I don't think we will want to change the design
		/// <summary>
		/// Property FullnameCssStyle (string)
		/// </summary>
		public string FullnameCssStyle
		{
			get
			{
				return this.fullnameCssStyle;
			}
			set
			{
				this.fullnameCssStyle = value;
			}
		}

		/// <summary>
		/// Property FullnameLabelCssStyle (string)
		/// </summary>
		public string FullnameLabelCssStyle
		{
			get
			{
				return this.fullnameLabelCssStyle;
			}
			set
			{
				this.fullnameLabelCssStyle = value;
			}
		}


	
		
		/// <summary>
		/// Property Required (bool)
		/// </summary>
		public bool Required
		{
			get
			{
				return this.required;
			}
			set
			{
				this.required = value;
			}
		}
		
		public string Reference
		{
			get
			{
				return this.reference;
			}
			set
			{
				this.reference = value;
			}
			
		}

		public string QuestionText
		{
			get
			{
				return this.questionText;
			}
			set
			{
				if (value.Length > 0)
				{
					this.questionText = value;
				}
			}
		}

		

		#endregion

		#region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EsccEthnicityControl"/> class.
        /// </summary>
		public EsccEthnicityControl():base(HtmlTextWriterTag.Div)
		{
	
		}
		#endregion

		#region methods

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
		protected override void CreateChildControls()
		{



			this.EnsureChildControls();
			
			if (required)
			{
				
				legend.InnerHtml = this.reference + " " + this.questionText + "<span class=\"requiredField\">*</span>";
				
			}
			else
			{
				legend.InnerText = this.reference + " " + this.questionText;
			}


            intro.InnerText = "If your ethnic group was not specified in the list and you selected one of the ‘Other’ options, please describe your ethnic group below:";

            legend.Attributes.Add("class", fullnameLabelCssStyle);

			optgroupPleaseSelect.Attributes.Add("label", "");
			optionPleaseSelect.InnerText = "Please select";
			optgroupPleaseSelect.Controls.Add(optionPleaseSelect);
			optgroupWhite.Attributes.Add("label", "White");
			optionWhiteBritish.InnerText = "British";
			optionWhiteIrish.InnerText = "Irish";
			optionWhiteGypsyRoma.InnerText = "Gypsy/Roma";
			optionWhiteIrishTraveller.InnerText = "Traveller of Irish Heritage";
			optionWhiteOther.InnerText = "Any other white background";

			optgroupMixed.Attributes.Add("label", "Mixed");
			optionMixedWhiteBlackCaribbean.InnerText = "White and Black Caribbean";
			optionMixedWhiteBlackAfrican.InnerText  = "White and Black African";
			optionMixedWhiteAsian.InnerText = "White and Asian";
			optionMixedOther.InnerText = "Any other Mixed Background";

			optgroupBlackOrBlackBritish.Attributes.Add("label","Black or Black British");
			optionBlackOrBlackBritishCaribbean.InnerText = "Caribbean";
			optionBlackOrBlackBritishAfrican.InnerText = "African";
			optionBlackOrBlackBritishOther.InnerText = "Any other Black Background";

			optgroupAsianOrAsianBritish.Attributes.Add("label", "Asian or Asian British");
			optionAsianOrAsianBritishIndian.InnerText = "Indian";
			optionAsianOrAsianBritishPakistani.InnerText = "Pakistani";
			optionAsianOrAsianBritishBangladeshi.InnerText = "Bangladeshi";
			optionAsianOrAsianBritishOther.InnerText = "Any other Asian Background";

			optgroupChineseAndOther.Attributes.Add("label", "Chinese and other");
			optionChineseAndOtherChinese.InnerText = "Chinese";
			optionChineseAndOtherChineseOther.InnerText = "Other ethnic group";
			
			select.ID = "ethnicityCtrl";
			select.Attributes.Add("name", "ethnicityChoice");
			
			
			fieldset.Controls.Add(legend);
			
			fieldset.Controls.Add(select);


			select.Controls.Add(optgroupPleaseSelect);
			select.Controls.Add(optgroupWhite);


			optgroupWhite.Controls.Add(optionWhiteBritish);
			optgroupWhite.Controls.Add(optionWhiteIrish);
			optgroupWhite.Controls.Add(optionWhiteGypsyRoma);
			optgroupWhite.Controls.Add(optionWhiteIrishTraveller);
			optgroupWhite.Controls.Add(optionWhiteOther);

			select.Controls.Add(optgroupMixed);
			
			optgroupMixed.Controls.Add(optionMixedWhiteBlackCaribbean);
			optgroupMixed.Controls.Add(optionMixedWhiteBlackAfrican);
			optgroupMixed.Controls.Add(optionMixedWhiteAsian);
			optgroupMixed.Controls.Add(optionMixedOther);


			select.Controls.Add(optgroupBlackOrBlackBritish);

			optgroupBlackOrBlackBritish.Controls.Add(optionBlackOrBlackBritishCaribbean);
			optgroupBlackOrBlackBritish.Controls.Add(optionBlackOrBlackBritishAfrican);
			optgroupBlackOrBlackBritish.Controls.Add(optionBlackOrBlackBritishOther);

			select.Controls.Add(optgroupAsianOrAsianBritish);

			optgroupAsianOrAsianBritish.Controls.Add(optionAsianOrAsianBritishIndian);
			optgroupAsianOrAsianBritish.Controls.Add(optionAsianOrAsianBritishPakistani);
			optgroupAsianOrAsianBritish.Controls.Add(optionAsianOrAsianBritishBangladeshi);
			optgroupAsianOrAsianBritish.Controls.Add(optionAsianOrAsianBritishOther);

			select.Controls.Add(optgroupChineseAndOther);

			optgroupChineseAndOther.Controls.Add(optionChineseAndOtherChinese);
			optgroupChineseAndOther.Controls.Add(optionChineseAndOtherChineseOther);

            fieldset.Controls.Add(intro);

			lblOtherEthnicity.Text = "If you chose other, please specify:";
            lblOtherEthnicity.CssClass = "formLabel";
			txbOtherEthnicity.Columns = 10;
			txbOtherEthnicity.Rows = 60;
			txbOtherEthnicity.ID = "txbOtherEthnicity";
			txbOtherEthnicity.CssClass = "WideText";
			txbOtherEthnicity.TextMode = TextBoxMode.MultiLine;
			HtmlGenericControl formPart = new HtmlGenericControl("div");
			formPart.Attributes.Add("class", "formPart");
			formPart.Controls.Add(lblOtherEthnicity);
			formPart.Controls.Add(txbOtherEthnicity);
			fieldset.Controls.Add(formPart);
			this.Controls.Add(fieldset);



			Hashtable options = new Hashtable();
			//White catergories
			options.Add("Please select", optionPleaseSelect);
			options.Add("British" , optionWhiteBritish);
			options.Add("Irish", optionWhiteIrish);
			options.Add("Gypsy/Roma", optionWhiteGypsyRoma);
			options.Add("Irish Traveller", optionWhiteIrishTraveller);
			options.Add("Any other white background", optionWhiteOther);
			//Mixed
			options.Add("White and Black Caribbean", optionMixedWhiteBlackCaribbean);
			options.Add("White and Black African", optionMixedWhiteBlackAfrican);
			options.Add("White and Asian", optionMixedWhiteAsian);
			options.Add("Any other Mixed Background", optionMixedOther);
			//Black
			options.Add("Caribbean", optionBlackOrBlackBritishCaribbean);
			options.Add("African", optionBlackOrBlackBritishAfrican);
			options.Add("Any other Black Background", optionBlackOrBlackBritishOther);
			//Asian
			options.Add("Indian",optionAsianOrAsianBritishIndian);
			options.Add("Pakistani",optionAsianOrAsianBritishPakistani);
			options.Add("Bangladeshi",optionAsianOrAsianBritishBangladeshi);
			options.Add("Any other Asian Background",  optionAsianOrAsianBritishOther);
			//Chinese
			options.Add("Chinese",optionChineseAndOtherChinese);
			options.Add("Other ethnic group", optionChineseAndOtherChineseOther);		




			if (Page.IsPostBack)
			{
				string text = Context.Request.Form["ethnicityChoice"];
				if ((text != "Please select") & (text.ToLower().Contains("other") == false))
				{
					if (options.ContainsKey(text))
					{
						HtmlGenericControl ctrl = options[text] as HtmlGenericControl;
						ctrl.InnerText = text;
						ctrl.Attributes.Add("selected", "true");
						ethnicity = text;
						
					}
				}
				else if (text == "Please select" & required == true)
				{
					RequiredFieldValidator	requiredFieldValidator =  new RequiredFieldValidator();
					requiredFieldValidator.Display = ValidatorDisplay.None;
					requiredFieldValidator.ControlToValidate = "txbOtherEthnicity";
					requiredFieldValidator.EnableClientScript= false;
				
					if (this.reference == "")
					{
						requiredFieldValidator.ErrorMessage = "Please select your ethinicity for the list";
					}
					else
					{

						requiredFieldValidator.ErrorMessage = "question " + reference.Replace(".", "") + " must be completed";
					}
					this.txbOtherEthnicity.Controls.Add(requiredFieldValidator);


				}
				else if (text.ToLower().Contains("other") && txbOtherEthnicity.Text.Length == 0)
				{

					HtmlGenericControl ctrl = options[text] as HtmlGenericControl;
					ctrl.InnerText = text;
					ctrl.Attributes.Add("selected", "true");
					ethnicity = text;

					RequiredFieldValidator	requiredFieldValidator =  new RequiredFieldValidator();
					requiredFieldValidator.Display = ValidatorDisplay.None;
					requiredFieldValidator.ControlToValidate = "txbOtherEthnicity";
					requiredFieldValidator.EnableClientScript= false;
					if (this.reference == "")
					{
						requiredFieldValidator.ErrorMessage = "Please tell us about your ethinicity";
					}
					else
					{

						requiredFieldValidator.ErrorMessage = "question " + reference.Replace(".", "") + " must be completed";
					}
					this.txbOtherEthnicity.Controls.Add(requiredFieldValidator);

				}
			}
			

						



		}
		#endregion
	}
}
