using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using eastsussexgovuk.webservices.FormControls.CustomControls;
using eastsussexgovuk.webservices.FormControls.Validators;
using eastsussexgovuk.webservices.TextXhtml.TextManipulation;
namespace eastsussexgovuk.webservices.FormControls.genericforms
{
    /// <summary>
    /// Summary description for FormQuestion.
    /// </summary>
    public class FormQuestion
    {
        #region private fields

        private string htmlQuestionId;

        /// <summary>
        /// Property HtmlQuestionId (string)
        /// </summary>
        public string HtmlQuestionId
        {
            get
            {
                return this.htmlQuestionId;
            }
            set
            {
                this.htmlQuestionId = value;
            }
        }

        private bool renderQuestionLabel;

        private RegularExpressionValidator validationControl;

        private bool isRequired;

        private RequiredFieldValidator requiredValidator;

        private RequiredFieldValidatorForCheckBoxLists requiredCheckBoxListValidator;

        private EsccRequiredCheckBox requiredCheckBoxValidator;

        private QuestionDependencyCollection questionDependencyCollection = new QuestionDependencyCollection();

        private string questionType;

        private int formId;

        private int formInstance;

        private int seqNo;

        private int pageNo;

        private int secNo;

        private int subSecNo;

        private string answerType;

        private string answerDataType;

        private bool isStaffOnly;

        private int questionId;


        private string questionRef;

        private string questionText;

        private string questionHelp;

        private string validationTemplate;

        private string errorMessage;

        private string cssClassName;

        private string cssLabelClassName;

        private string defaultAnswerText;

        private string defaultAnswerVarchar;

        private int defaultAnswerInteger;

        private DateTime defaultAnswerDate;

        private double defaultAnswerDecimal;

        private bool defaultAnswerBit;

        private string posAnswerDisplayName;

        private ArrayList posAnswerDisplayNames;

        private Label questionLabel = new Label();

        private object answer;

        private Control baseControl;

        #endregion



        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public FormQuestion()
        {
            posAnswerDisplayNames = new ArrayList();


        }


        #endregion

        #region Public properties


        public bool RenderQuestionLabel
        {
            get
            { return this.renderQuestionLabel; }
            set
            { this.renderQuestionLabel = value; }
        }

        public string CssLabelClassName
        {
            get
            {
                return this.cssLabelClassName;
            }
            set
            {

                this.cssLabelClassName = value;
            }
        }

        /// <summary>
        /// Property Answer (object)
        /// </summary>
        public object Answer
        {
            get
            {
                return this.answer;
            }
            set
            {
                this.answer = value;
            }
        }

        /// <summary>
        /// Property FormInstance (int)
        /// </summary>
        public int FormInstance
        {
            get
            {
                return this.formInstance;
            }
            set
            {
                this.formInstance = value;
            }
        }

        /// <summary>
        /// Property IsRequired (bool)
        /// </summary>
        public bool IsRequired
        {
            get
            {
                return this.isRequired;
            }
            set
            {
                this.isRequired = value;


            }
        }


        /// <summary>
        /// Property ValidationControl (RegularExpressionValidator)
        /// </summary>
        public RegularExpressionValidator ValidationControl
        {
            get
            {
                return this.validationControl;
            }
            set
            {
                this.validationControl = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public RequiredFieldValidator RequiredValidator
        {
            get
            {
                return this.requiredValidator;
            }
            set
            {
                this.requiredValidator = value;
            }
        }

        public RequiredFieldValidatorForCheckBoxLists RequiredCheckBoxListValidator
        {
            get
            {
                return this.requiredCheckBoxListValidator;
            }
            set
            {
                this.requiredCheckBoxListValidator = value;
            }
        }

        public EsccRequiredCheckBox RequiredCheckBoxValidator
        {
            get
            {
                return this.requiredCheckBoxValidator;
            }
            set
            {
                this.requiredCheckBoxValidator = value;
            }
        }

        /// <summary>
        /// Property ErrorMessage (string)
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
            set
            {
                this.errorMessage = value;
            }
        }

        /// <summary>
        /// Property BaseControl (string)
        /// Is the  actual control that will be rendered to the page
        /// </summary>
        public Control BaseControl
        {
            get
            {

                return this.baseControl;
            }
            set
            {
                this.baseControl = value;
            }
        }

        /// <summary>
        /// Property QuestionType (string)
        /// Can either be a Question or a section heading
        /// </summary>
        public string QuestionType
        {
            get
            {
                return this.questionType;
            }
            set
            {
                this.questionType = value;
            }
        }
        /// <summary>
        /// Property PosAnswerDisplayName (ArrayList)
        /// </summary>
        public string PosAnswerDisplayName
        {
            get
            {
                return this.posAnswerDisplayName;
            }
            set
            {

                this.posAnswerDisplayName = value;
                this.posAnswerDisplayNames.Add(value);

            }

        }
        /// <summary>
        /// Property PosAnswerDisplayName (ArrayList)
        /// </summary>
        public ArrayList PosAnswerDisplayNames
        {
            get
            {
                return this.posAnswerDisplayNames;
            }

        }

        private string posAnswerValue;
        public string PosAnswerValue
        {
            get
            {
                return this.posAnswerValue;
            }
            set
            {
                this.posAnswerValue = value;
                this.posAnswerValues.Add(value);
                //call populate method
                PopulatePosAnswers();

            }


        }



        private ArrayList posAnswerValues = new ArrayList();


        public ArrayList PosAnswerValues
        {
            get
            {
                return this.posAnswerValues;
            }
        }


        /// <summary>
        /// Property FormId (int)
        /// </summary>
        public int FormId
        {
            get
            {
                return this.formId;
            }
            set
            {
                this.formId = value;
            }
        }

        /// <summary>
        /// Property SeqNo (int)
        /// </summary>
        public int SeqNo
        {
            get
            {
                return this.seqNo;
            }
            set
            {
                this.seqNo = value;
            }
        }


        /// <summary>
        /// Property PageNo (int)
        /// </summary>
        public int PageNo
        {
            get
            {
                return this.pageNo;
            }
            set
            {
                this.pageNo = value;
            }
        }

        /// <summary>
        /// Property SecNo (int)
        /// </summary>
        public int SecNo
        {
            get
            {
                return this.secNo;
            }
            set
            {
                this.secNo = value;
            }
        }

        /// <summary>
        /// Property SubSecNo (int)
        /// </summary>
        public int SubSecNo
        {
            get
            {
                return this.subSecNo;
            }
            set
            {
                this.subSecNo = value;
            }
        }






        /// <summary>
        /// Property AnswerType (string)
        /// This is used to decide what input type the 
        /// form will genereate e.g. simpletext = textbox
        /// </summary>
        public string AnswerType
        {
            get
            {
                return this.answerType;
            }
            set
            {
                this.answerType = value;


            }
        }

        /// <summary>
        /// Property AnswerDataType (string)
        /// </summary>
        public string AnswerDataType
        {
            get
            {
                return this.answerDataType;
            }
            set
            {
                this.answerDataType = value;
            }
        }

        /// <summary>
        /// Property IsStaffOnly (bool)
        /// </summary>
        public bool IsStaffOnly
        {
            get
            {
                return this.isStaffOnly;
            }
            set
            {
                this.isStaffOnly = value;
            }
        }


        /// <summary>
        /// Property QuestionId (int)
        /// </summary>
        public int QuestionId
        {
            get
            {
                return this.questionId;
            }
            set
            {
                this.questionId = value;

                //Set new property to fix HTML attribute problemm
                this.htmlQuestionId = "q" + value.ToString();

            }
        }

        /// <summary>
        /// Property QuestionText (string)
        /// This is used as the text for a label control
        /// unless it is a heading.
        /// </summary>
        public string QuestionText
        {
            get
            {

                return this.questionText;
            }
            set
            {
                this.questionText = value;

            }
        }

        /// <summary>
        /// Property QuestionHelp (string)
        /// </summary>
        public string QuestionHelp
        {
            get
            {
                return this.questionHelp;
            }
            set
            {
                this.questionHelp = value;
            }
        }

        /// <summary>
        /// Property QuestionRef (string)
        /// </summary>
        public string QuestionRef
        {
            get
            {
                return this.questionRef;
            }
            set
            {
                this.questionRef = value;
            }
        }

        /// <summary>
        /// Property ValidationTemplate (string)
        /// </summary>
        public string ValidationTemplate
        {
            get
            {
                return this.validationTemplate;
            }
            set
            {
                this.validationTemplate = value;

                if (this.ValidationTemplate.Length > 0)
                {
                    this.validationControl = new RegularExpressionValidator();
                    SetValidationControl();
                }
            }
        }


        /// <summary>
        /// Property CssClassName (string)
        /// </summary>
        public string CssClassName
        {
            get
            {
                return this.cssClassName;
            }
            set
            {
                this.cssClassName = value;
            }
        }


        /// <summary>
        /// Property DefaultAnswer (string)
        /// </summary>
        public string DefaultAnswerText
        {
            get
            {
                return this.defaultAnswerText;
            }
            set
            {
                this.defaultAnswerText = value;
            }
        }



        /// <summary>
        /// Property DefaultAnswerVarchar (string)
        /// </summary>
        public string DefaultAnswerVarchar
        {
            get
            {
                return this.defaultAnswerVarchar;
            }
            set
            {
                this.defaultAnswerVarchar = value;
            }
        }

        /// <summary>
        /// Property DefaultAnswerInteger (int)
        /// </summary>
        public int DefaultAnswerInteger
        {
            get
            {
                return this.defaultAnswerInteger;
            }
            set
            {
                this.defaultAnswerInteger = value;
            }
        }

        /// <summary>
        /// Property DefaultAnswerDate (DateTime)
        /// </summary>
        public DateTime DefaultAnswerDate
        {
            get
            {
                return this.defaultAnswerDate;
            }
            set
            {
                this.defaultAnswerDate = value;
            }
        }


        /// <summary>
        /// Property DefaultAnswerDecimal (double)
        /// </summary>
        public double DefaultAnswerDecimal
        {
            get
            {
                return this.defaultAnswerDecimal;
            }
            set
            {
                this.defaultAnswerDecimal = value;
            }
        }

        /// <summary>
        /// Property DefaultAnswerBit (bool)
        /// </summary>
        public bool DefaultAnswerBit
        {
            get
            {
                return this.defaultAnswerBit;
            }
            set
            {
                this.defaultAnswerBit = value;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public Label QuestionLabel
        {


            get
            {
                this.questionLabel.CssClass = this.cssLabelClassName;
                if ((questionText.Length > 0) & (this.answerType != "Address") & (this.answerType != "AddressNonCitizen") & (this.questionType != "fieldsetOpen") & (this.questionType != "fieldsetClose"))
                {
                    this.questionLabel.AssociatedControlID = this.htmlQuestionId;
                }
                return this.questionLabel;
            }
            set
            {
                this.questionLabel = value;
            }


        }

        /// <summary>
        /// Property QuestionDependencyCollection (QuestionDependencyCollection)
        /// </summary>
        public QuestionDependencyCollection QuestionDependencyCollection
        {
            get
            {
                return this.questionDependencyCollection;
            }
            set
            {

                this.questionDependencyCollection = value;
            }
        }

        #endregion

        #region Private methods
        /// <summary>
        /// This method looks at the basecontrol's type and casts it
        /// into one of three list controls (RadioButtonList, CheckBoxList or DropDownList)
        /// We then add each of the questions possible answers to the list controls item
        /// collection.
        /// </summary>
        private void PopulatePosAnswers()
        {
            ListItem item = new ListItem(this.posAnswerDisplayName, this.posAnswerValue);


            switch (this.baseControl.GetType().ToString())
            {
                case "System.Web.UI.WebControls.DropDownList":

                    DropDownList d = this.baseControl as DropDownList;

                    d.Items.Add(item);
                    break;

                case "System.Web.UI.WebControls.RadioButtonList":
                    RadioButtonList r = this.baseControl as RadioButtonList;
                    r.RepeatLayout = RepeatLayout.Flow;

                    r.RepeatDirection = RepeatDirection.Horizontal;


                    r.Items.Add(item);
                    break;

                case "eastsussexgovuk.webservices.FormControls.CustomControls.EsccCheckBoxList":


                    EsccCheckBoxList chbl = this.baseControl as EsccCheckBoxList;

                    chbl.RepeatLayout = RepeatLayout.Flow;

                    chbl.RepeatDirection = RepeatDirection.Horizontal;




                    chbl.Items.Add(item);
                    break;
            }

        }


        /// <summary>
        /// This method creates a regular expression validator and assigns it properties
        /// it is only called if the question has a validation template specifed in the database.
        /// </summary>
        private void SetValidationControl()
        {
            this.validationControl.ValidationExpression = this.validationTemplate;
            this.validationControl.ControlToValidate = this.htmlQuestionId;//this.questionId.ToString();
            this.validationControl.Display = ValidatorDisplay.None;
            this.validationControl.EnableClientScript = false;
            this.validationControl.ErrorMessage = "question " + this.questionRef.Replace(".", " ") + this.errorMessage;
        }


        public FormQuestion AddRequiredValidator(FormQuestion question)
        {

            question.requiredValidator = new RequiredFieldValidator();


            question.requiredValidator.ControlToValidate = question.BaseControl.Controls[4].ClientID.Replace("_", "$");
            question.requiredValidator.Display = ValidatorDisplay.None;
            question.requiredValidator.EnableClientScript = false;

            //TODO fieldsets setting ref in error message


            question.requiredValidator.ErrorMessage = "question  " + question.questionRef.Replace(".", "") + " must be completed";

            question.requiredValidator.IsValid = false;

            return question;

        }


        /// <summary>
        /// This method creates a required validator for questions that have isRequired set
        /// to true. There are exceptions with custom controls, where the control itself handles
        /// its own validation and is bubbled up to its parents validation summary control.
        /// </summary>
        public void AddRequiredValidator()
        {

            if ((AnswerType != "EsccEthnicityControl") & (AnswerType != "SimpleDate") & (AnswerType != "DatePicker") & (AnswerType != "Address") & (AnswerType != "AddressNonCitizen"))
            {
                if ((AnswerType == "CheckBoxListVarchar") | (AnswerType == "CheckBoxListInteger"))
                {
                    this.requiredCheckBoxListValidator = new RequiredFieldValidatorForCheckBoxLists();

                    this.requiredCheckBoxListValidator.ControlToValidate = this.htmlQuestionId;//this.questionId.ToString();

                    this.requiredCheckBoxListValidator.Display = ValidatorDisplay.None;
                    this.requiredCheckBoxListValidator.EnableClientScript = false;
                    this.requiredCheckBoxListValidator.ErrorMessage = "question " + this.questionRef.Replace(".", "") + " must be completed";

                }
                else if (AnswerType == "CheckBoxVarchar")
                {
                    this.requiredCheckBoxValidator = new EsccRequiredCheckBox();
                    this.requiredCheckBoxValidator.ControlToValidate = this.htmlQuestionId;
                    this.requiredCheckBoxValidator.Display = ValidatorDisplay.None;
                    this.requiredCheckBoxValidator.EnableClientScript = false;
                    this.requiredCheckBoxValidator.ErrorMessage = "question " + this.questionRef.Replace(".", "") + " must be completed";



                }
                else if (this.baseControl.GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                {

                    this.requiredValidator = new RequiredFieldValidator();
                    this.requiredValidator.ErrorMessage = "question  " + this.questionRef.Replace(".", "") + " must be completed";
                    this.requiredValidator.ControlToValidate = this.htmlQuestionId;
                    this.requiredValidator.InitialValue = this.PosAnswerDisplayNames[0].ToString();
                    this.requiredValidator.Display = ValidatorDisplay.None;
                    this.requiredValidator.EnableClientScript = false;


                     
                }
                else //All other cases 
                {

                    this.requiredValidator = new RequiredFieldValidator();


                    this.requiredValidator.ControlToValidate = this.htmlQuestionId;//this.questionId.ToString();

                    this.requiredValidator.Display = ValidatorDisplay.None;
                    this.requiredValidator.EnableClientScript = false;

                    //TODO fieldsets setting ref in error message


                    this.requiredValidator.ErrorMessage = "question  " + this.questionRef.Replace(".", "") + " must be completed";



                    
                }
            }
        }

        /// <summary>
        /// This method looks at the answerType (input type) and uses a
        /// switch statement to create a basecontrol of answerType
        /// </summary>
        /// <example> controlType = new TextBox</example>
        /// <param name="answerType"></param>
        public void SetControlType(string answerType)
        {
            Control controlType = null;
            switch (answerType)
            {
                case "DeclarationOfInterest":
                    EsccDeclarationOfInterest declaration = new EsccDeclarationOfInterest();

                    if (this.questionText.Length > 0)
                    {
                        declaration.DeclarationStatement = this.questionText;
                    }
                    else
                    {
                        //default declaration statement
                        declaration.DeclarationStatement = "I declare that all the information I have given on this form is correct." +
                            "(If you knowingly give false information, your access to this information may be withdrawn)";

                    }


                    declaration.Required = this.isRequired;
                    declaration.ToolTip = this.questionHelp;
                    declaration.RefText = this.questionRef;
                    declaration.FullnameCssStyle = this.cssClassName;
                    declaration.FullnameLabelCssStyle = this.cssLabelClassName;
                    renderQuestionLabel = false;
                    controlType = declaration;

                    break;
                case "Address":

                 FormAddress address = new FormAddress(this.questionId);

                 address.IsRequired = this.isRequired;
                 

                 if (address.IsRequired)
                 {
                     address.MessageAddressRequired = "question " + this.questionRef.Replace(".", " ") + "please enter an address";
                     address.MessageFullAddressRequired = "question " + this.questionRef.Replace(".", " ") + "please click 'Find address' to complete your address";
                 }
                 else
                 {
                     address.MessageAddressRequired = "";
                     address.MessageFullAddressRequired = "question " + this.questionRef.Replace(".", " ") + "please click 'Find address' to complete your address";
                 }
                    address.EnableValidation = true;
                    renderQuestionLabel = true;
                    controlType = address;
                    break;

                case "AddressNonCitizen":
                    FormAddressNonCitizen addressNC = new FormAddressNonCitizen(this.questionId);
                    addressNC.IsRequired = this.isRequired;
                    if (addressNC.IsRequired)
                    {
                        addressNC.MessageAddressRequired = "question " + this.questionRef.Replace(".", " ") + "please enter an address";
                        addressNC.MessageFullAddressRequired = "question " + this.questionRef.Replace(".", " ") + "please click 'Find address' to complete your address";
                    }
                    
                    else
                    {
                        addressNC.MessageAddressRequired = "";
                        addressNC.MessageFullAddressRequired = "question " + this.questionRef.Replace(".", " ") + "please click 'Find address' to complete your address";
                    }
                    
                    addressNC.EnableValidation = true;
                    renderQuestionLabel = true;
                    controlType = addressNC;
                    break;

                case "Heading": //1
                    HtmlGenericControl head = new HtmlGenericControl("h1");
                    head.InnerText = this.QuestionText;
                    this.questionLabel.Visible = false;
                    renderQuestionLabel = true;
                    controlType = head;
                    break;

                case "Section Heading"://2
                    HtmlGenericControl heading = new HtmlGenericControl("h2");
                    heading.InnerText = this.QuestionText;
                    this.questionLabel.Visible = false;
                    renderQuestionLabel = true;
                    controlType = heading;
                    break;

                case "Question"://3	
                    HtmlGenericControl heading2 = new HtmlGenericControl("h3");
                    heading2.InnerText = this.QuestionText;
                    renderQuestionLabel = true;
                    controlType = heading2;
                    break;

                case "fieldsetOpen"://9
                    string legend = this.questionRef + " " + this.questionText;
                    legend = TextManipulationUtilities.LegendWrapLines(legend, 75);
                    LiteralControl fieldSetOpen = new LiteralControl("<fieldset class = \"" + this.cssClassName + "\"><legend>" + legend + " </legend> ");


                    renderQuestionLabel = false;
                    controlType = fieldSetOpen;
                    break;

                case "fieldsetClose"://10
                    LiteralControl fieldsetClose = new LiteralControl("</fieldset>");
                    renderQuestionLabel = false;
                    controlType = fieldsetClose;
                    break;

                case "formBoxOpen"://5
                    //LiteralControl formBoxOpen = new LiteralControl("<div class=\"formBox\"><div id=\"topedge\" class=\"cornerTR\"><div class = \"cornerTL\"></div></div> ");
                    LiteralControl formBoxOpen = new LiteralControl("<div class=\"formBox\"><div></div><div></div><div></div>");
                    renderQuestionLabel = false;
                    controlType = formBoxOpen;
                    break;

                case "formBoxClose"://6
                    //LiteralControl formBoxClose = new LiteralControl("<div id=\"bottomedge\"  class=\"cornerBR\"><div class = \"cornerBL\"></div></div></div>");
                    LiteralControl formBoxClose = new LiteralControl("</div>");
                    renderQuestionLabel = false;
                    controlType = formBoxClose;
                    break;

                case "formPartOpen"://7
                    LiteralControl formPartOpen = new LiteralControl("<div class=\"formPart\">");
                    renderQuestionLabel = false;
                    controlType = formPartOpen;
                    break;


                case "formPartClose"://8

                    LiteralControl formPartClose = new LiteralControl("</div>");
                    renderQuestionLabel = false;
                    controlType = formPartClose;
                    break;



                case "Text":
                    HtmlGenericControl text = new HtmlGenericControl("p");
                    text.InnerHtml = this.questionText;
                    if (this.cssClassName.Length > 0)
                    {
                        text.Attributes.Add("class", this.cssClassName);
                    }
                    this.questionLabel.Visible = false;
                    renderQuestionLabel = true;
                    controlType = text;
                    break;

                case "SimpleText"://3

                    TextBox textBox = new TextBox();
                    textBox.CssClass = this.cssClassName; // "formControl";
                    textBox.ToolTip = this.questionHelp;
                    textBox.ID = this.htmlQuestionId;//this.questionId.ToString();
                    renderQuestionLabel = true;
                    controlType = textBox;
                    break;

                case "SimpleInteger"://4
                    //TODO:We need to remember to create all the regular expressions
                    TextBox textInteger = new TextBox();
                    textInteger.CssClass = this.cssClassName;
                    textInteger.ID = this.htmlQuestionId;//this.questionId.ToString();
                    textInteger.ToolTip = this.questionHelp;
                    renderQuestionLabel = true;
                    controlType = new TextBox();
                    break;

                case "SimpleDouble"://5
                    TextBox textDouble = new TextBox();
                    textDouble.ID = this.htmlQuestionId;//this.questionId.ToString();
                    textDouble.CssClass = this.cssClassName;
                    textDouble.ToolTip = this.questionHelp;
                    renderQuestionLabel = true;
                    controlType = new TextBox();
                    break;

                case "SimpleDate"://6
                    DateTimeFormPart date = new DateTimeFormPart(this.questionText, this.questionId.ToString(), this.questionRef, this.isRequired, this.cssLabelClassName, this.cssClassName);
                    DateTime dt = DateTime.Now.AddYears(10);
                    date.FirstYear = dt.Year;
                    date.LastYear = 1900;
                    this.questionLabel.Visible = false;
                    date.ToolTip = this.questionHelp;
                    renderQuestionLabel = false;
                    controlType = date;
                    break;


                case "RadioButtonListVarchar"://9
                    RadioButtonList radioButtonlist = new RadioButtonList();

                    radioButtonlist.ID = this.htmlQuestionId;//= questionId.ToString();
                    //radioButtonlist.Attributes.Add("name", radioButtonlist.ID);
                    radioButtonlist.CssClass = this.cssClassName;
                    radioButtonlist.RepeatLayout = RepeatLayout.Flow;
                    radioButtonlist.RepeatDirection = RepeatDirection.Horizontal;
                    radioButtonlist.ToolTip = this.questionHelp;
                    radioButtonlist.Attributes.Remove("name");
                    renderQuestionLabel = true;
                    controlType = radioButtonlist;
                    break;

                case "DropDownListVarchar"://1
                    DropDownList dropdownlist = new DropDownList();
                    dropdownlist.CssClass = this.cssClassName;
                    dropdownlist.ID = this.htmlQuestionId;//questionId.ToString();
                    //dropdownlist.Attributes.Add("name", dropdownlist.ID);	
                    renderQuestionLabel = true;
                    controlType = dropdownlist;
                    break;

                case "DropDownListDouble"://10
                    DropDownList dropDownListDouble = new DropDownList();
                    dropDownListDouble.CssClass = this.cssClassName;
                    dropDownListDouble.ID = this.htmlQuestionId;//this.questionId.ToString();
                    dropDownListDouble.Attributes.Add("name", dropDownListDouble.ID);
                    renderQuestionLabel = false;
                    controlType = dropDownListDouble;
                    break;

                case "DropDownListInteger"://11
                    DropDownList dropDownListInteger = new DropDownList();
                    dropDownListInteger.CssClass = this.cssClassName;
                    dropDownListInteger.ID = this.htmlQuestionId;//this.questionId.ToString();
                    dropDownListInteger.Attributes.Add("name", dropDownListInteger.ID);
                    renderQuestionLabel = true;

                    controlType = dropDownListInteger;
                    break;

                case "RadioButtonVarchar"://12
                    RadioButton radioButtonVarchar = new RadioButton();
                    radioButtonVarchar.ID = this.htmlQuestionId;//this.questionId.ToString();
                    radioButtonVarchar.Text = this.questionText;
                    radioButtonVarchar.CssClass = this.cssClassName;
                    radioButtonVarchar.Attributes.Add("name", radioButtonVarchar.ID);
                    radioButtonVarchar.ToolTip = this.questionHelp;
                    renderQuestionLabel = true;

                    controlType = radioButtonVarchar;
                    break;

                case "CheckBoxVarchar"://13	
                    CheckBox checkbox = new CheckBox();
                    checkbox.CssClass = this.cssClassName;
                    checkbox.Text = this.questionRef + " " + this.questionText;
                    checkbox.ID = this.htmlQuestionId;//questionId.ToString();
                    checkbox.Attributes.Add("name", checkbox.ID);
                    renderQuestionLabel = true;

                    checkbox.ToolTip = this.questionHelp;

                    checkbox.Attributes.Remove("name");
                    renderQuestionLabel = false;


                    controlType = checkbox;

                    break;

                case "CheckBoxListVarchar"://14
                    EsccCheckBoxList checkboxList = new EsccCheckBoxList();
                    checkboxList.CssClass = this.cssClassName;
                    checkboxList.RepeatDirection = RepeatDirection.Horizontal;
                    checkboxList.RepeatLayout = RepeatLayout.Flow;
                    checkboxList.ID = this.htmlQuestionId;//questionId.ToString();
                    //checkboxList.Attributes.Add("name", checkboxList.ID);
                    checkboxList.ToolTip = this.questionHelp;
                    renderQuestionLabel = true;

                    controlType = checkboxList;
                    break;

                case "TextArea":
                    TextBox textArea = new TextBox();
                    textArea.CssClass = this.cssClassName;
                    textArea.TextMode = TextBoxMode.MultiLine;
                    textArea.ID = this.htmlQuestionId;//this.questionId.ToString();
                    textArea.ToolTip = this.questionHelp;
                    textArea.Rows = 60;
                    textArea.Columns = 10;
                    // Fix issue of duplicate row and column attributes highlighted by XML parsing by bing for translations service
                    // textArea.Attributes.Add("rows", "60");
                    // textArea.Attributes.Add("cols", "10");
                    renderQuestionLabel = true;

                    controlType = textArea;
                    break;

                case "FileUpload":
                    HtmlInputFile FileUpload = new HtmlInputFile();
                    renderQuestionLabel = false;

                    FileUpload.ID = this.htmlQuestionId;//this.questionId.ToString();
                    controlType = new HtmlInputFile();

                    break;

                case "RadioButtonInteger":
                    RadioButton radioButton = new RadioButton();
                    radioButton.ID = this.htmlQuestionId;//this.questionId.ToString();
                    radioButton.Text = this.questionText;
                    radioButton.ToolTip = this.questionHelp;
                    renderQuestionLabel = true;

                    controlType = new RadioButton();
                    break;

                case "RadioButtonListInteger":
                    RadioButtonList radioButtonlistInteger = new RadioButtonList();
                    radioButtonlistInteger.CssClass = "radioButtonList";
                    radioButtonlistInteger.ID = this.htmlQuestionId;//questionId.ToString();
                    radioButtonlistInteger.Attributes.Add("name", radioButtonlistInteger.ID);
                    radioButtonlistInteger.CssClass = this.cssClassName;
                    radioButtonlistInteger.RepeatLayout = RepeatLayout.Flow;
                    radioButtonlistInteger.RepeatDirection = RepeatDirection.Horizontal;
                    radioButtonlistInteger.ToolTip = this.questionHelp;
                    renderQuestionLabel = true;

                    controlType = radioButtonlistInteger;
                    break;

                case "CheckBoxInteger":
                    CheckBox checkboxInteger = new CheckBox();
                    checkboxInteger.ID = this.htmlQuestionId;//this.questionId.ToString();
                    checkboxInteger.Text = this.questionRef + " " + this.questionText;
                    checkboxInteger.ToolTip = this.questionHelp;
                    renderQuestionLabel = true;

                    controlType = checkboxInteger;
                    break;

                case "CheckBoxListInteger":
                    EsccCheckBoxList checkboxListInteger = new EsccCheckBoxList();
                    checkboxListInteger.CssClass = "formControl";
                    checkboxListInteger.RepeatDirection = RepeatDirection.Horizontal;
                    checkboxListInteger.RepeatLayout = RepeatLayout.Flow;
                    checkboxListInteger.ID = this.htmlQuestionId;//questionId.ToString();
                    checkboxListInteger.Attributes.Add("name", checkboxListInteger.ID);
                    checkboxListInteger.ToolTip = this.questionHelp;
                    renderQuestionLabel = true;

                    controlType = checkboxListInteger;
                    break;

                case "DatePicker":
                    DatePicker datepicker = new DatePicker();
                    datepicker.imgDirectory = "/EastSussexCC/EformsAdmin/images/";
                    datepicker.DateType = "dd mmm yyyy";
                    datepicker.CssClass = this.cssClassName;
                    datepicker.QuestionRef = questionRef.Replace(".", "");
                    datepicker.IsRequired = this.isRequired;
                    datepicker.Label.CssClass = this.cssLabelClassName;
                    datepicker.Label.Text = questionText;
                    datepicker.Label.AssociatedControlID = datepicker.ClientID;
                    renderQuestionLabel = true;

                    controlType = datepicker;
                    break;

                case "DateControl":
                    //TODO: need to modify the dtabase to allow date control ranges to be set at design time.
                    DateControl dateControl = new DateControl(this.questionText, this.htmlQuestionId, this.questionRef, "1900", "2010", this.isRequired);
                    dateControl.CssClass = this.cssClassName;
                    renderQuestionLabel = false;

                    controlType = dateControl;

                    break;

                case "GivenName":
                    TextBox givenName = new TextBox();
                    givenName.ID = this.htmlQuestionId;//questionId.ToString();
                    givenName.CssClass = this.cssClassName;
                    givenName.ToolTip = questionHelp;
                    renderQuestionLabel = true;

                    controlType = givenName;

                    break;

                case "FamilyName":
                    TextBox familyName = new TextBox();
                    familyName.ID = this.htmlQuestionId;//uestionId.ToString();
                    familyName.CssClass = this.cssClassName;
                    familyName.ToolTip = questionHelp;
                    renderQuestionLabel = true;

                    controlType = familyName;
                    break;


                case "HomeEmail":
                    TextBox homeEmail = new TextBox();
                    homeEmail.ID = this.htmlQuestionId;//questionId.ToString();
                    homeEmail.CssClass = this.cssClassName;
                    homeEmail.ToolTip = questionHelp;
                    renderQuestionLabel = true;

                    controlType = homeEmail;
                    break;

                case "WorkPhone":
                    TextBox workPhone = new TextBox();
                    workPhone.ID = this.htmlQuestionId;//questionId.ToString();
                    workPhone.CssClass = this.cssClassName;
                    workPhone.ToolTip = questionHelp;
                    renderQuestionLabel = true;
                    controlType = workPhone;
                    break;

                case "BirthDate":
                    DatePicker birthDate = new DatePicker();
                    birthDate.imgDirectory = "/EastSussexCC/EformsAdmin/images/";
                    birthDate.DateType = "dd mmm yyyy";
                    birthDate.IsRequired = this.isRequired;
                    birthDate.Label.Text = questionText;
                    birthDate.Label.CssClass = this.cssLabelClassName;
                    birthDate.Label.AssociatedControlID = birthDate.ClientID;
                    birthDate.CssClass = this.cssClassName;
                    birthDate.QuestionRef = questionRef.Replace(".", "");
                    // TODO: FEW more properties to add later	
                    renderQuestionLabel = true;
                    controlType = birthDate;
                    break;

                case "MaritalStatus":
                    RadioButtonList maritalStatus = new RadioButtonList();
                    maritalStatus.ID = this.htmlQuestionId;//questionId.ToString();
                    maritalStatus.CssClass = this.cssClassName;
                    maritalStatus.ToolTip = questionHelp;
                    renderQuestionLabel = true;

                    maritalStatus.RepeatDirection = RepeatDirection.Horizontal;
                    maritalStatus.RepeatLayout = RepeatLayout.Flow;
                    controlType = maritalStatus;
                    break;

                case "Gender":
                    RadioButtonList gender = new RadioButtonList();
                    gender.ID = this.htmlQuestionId;//questionId.ToString();
                    gender.CssClass = this.cssClassName;
                    gender.ToolTip = questionHelp;
                    renderQuestionLabel = true;
                    controlType = gender;
                    break;

                case "Title":
                    DropDownList title = new DropDownList();
                    title.ID = this.htmlQuestionId;//questionId.ToString();
                    title.CssClass = this.cssClassName;
                    
                    renderQuestionLabel = true;
                    controlType = title;
                    break;

                case "NameInitials":
                    TextBox nameInitials = new TextBox();
                    nameInitials.ID = this.htmlQuestionId;//questionId.ToString();
                    nameInitials.CssClass = this.cssClassName;
                    nameInitials.ToolTip = questionHelp;
                    renderQuestionLabel = true;
                    controlType = nameInitials;
                    break;

                case "HomeFax":
                    TextBox homeFax = new TextBox();
                    homeFax.ID = this.htmlQuestionId;//questionId.ToString();
                    homeFax.CssClass = this.cssClassName;
                    homeFax.ToolTip = questionHelp;
                    renderQuestionLabel = true;
                    controlType = homeFax;
                    break;


                case "PersonalMobile":
                    TextBox personalMobile = new TextBox();
                    personalMobile.ID = this.htmlQuestionId;//questionId.ToString();
                    personalMobile.CssClass = this.cssClassName;
                    personalMobile.ToolTip = questionHelp;
                    renderQuestionLabel = true;
                    controlType = personalMobile;
                    break;

                case "HomePhone":
                    TextBox homePhone = new TextBox();
                    homePhone.ID = this.htmlQuestionId;//questionId.ToString();
                    homePhone.CssClass = this.cssClassName;
                    homePhone.ToolTip = questionHelp;
                    renderQuestionLabel = true;
                    controlType = homePhone;
                    break;

                case "WorkEmail":
                    TextBox workEmail = new TextBox();
                    workEmail.ID = this.htmlQuestionId;//questionId.ToString();
                    workEmail.CssClass = this.cssClassName;
                    workEmail.ToolTip = questionHelp;
                    renderQuestionLabel = true;
                    controlType = workEmail;
                    break;

                case "WorkMobile":
                    TextBox workMobile = new TextBox();
                    workMobile.ID = this.htmlQuestionId;//questionId.ToString();
                    workMobile.CssClass = this.cssClassName;
                    workMobile.ToolTip = questionHelp;
                    renderQuestionLabel = true;
                    controlType = workMobile;
                    break;

                case "WorkFax":
                    TextBox workFax = new TextBox();
                    workFax.ID = this.htmlQuestionId;//questionId.ToString();
                    workFax.CssClass = this.cssClassName;
                    workFax.ToolTip = questionHelp;
                    controlType = workFax;
                    break;

                case "Bulleted List"://Use to be List, changed to distiguish between ol / ul
                    HtmlGenericControl bulletedlist = new HtmlGenericControl("ul");
                    bulletedlist.InnerHtml = this.questionText;
                    this.questionLabel.Visible = false;
                    bulletedlist.Attributes.Add("class", this.cssClassName);
                    controlType = bulletedlist;
                    renderQuestionLabel = true;
                    break;

                case "Numbered List":
                    HtmlGenericControl numberedlist = new HtmlGenericControl("ol");
                    numberedlist.InnerHtml = this.questionText;
                    this.questionLabel.Visible = false;
                    numberedlist.Attributes.Add("class", this.cssClassName);
                    renderQuestionLabel = true;
                    controlType = numberedlist;
                    break;

                case "Confirmation":
                    HtmlGenericControl confirmation = new HtmlGenericControl("p");
                    confirmation.InnerHtml = questionText;
                    renderQuestionLabel = false;
                    controlType = confirmation;
                    break;

                case "PhoneNonCitizen":
                    TextBox phoneNonCitizen = new TextBox();
                    phoneNonCitizen.ID = this.htmlQuestionId;//questionId.ToString();
                    renderQuestionLabel = true;
                    controlType = phoneNonCitizen;
                    phoneNonCitizen.ToolTip = questionHelp;
                    phoneNonCitizen.CssClass = this.cssClassName;
                    break;

                case "FaxNonCitizen":
                    TextBox faxNonCitizen = new TextBox();
                    faxNonCitizen.ID = this.htmlQuestionId;//questionId.ToString();
                    renderQuestionLabel = true;
                    controlType = faxNonCitizen;
                    faxNonCitizen.ToolTip = questionHelp;
                    faxNonCitizen.CssClass = this.cssClassName;
                    break;

                case "MobileNonCitizen":
                    TextBox mobileNonCitizen = new TextBox();
                    mobileNonCitizen.ID = this.htmlQuestionId;//questionId.ToString();
                    renderQuestionLabel = true;
                    controlType = mobileNonCitizen;
                    mobileNonCitizen.ToolTip = questionHelp;
                    mobileNonCitizen.CssClass = this.cssClassName;
                    break;

                case "EmailNonCitizen":
                    TextBox emailNonCitizen = new TextBox();
                    emailNonCitizen.ID = this.htmlQuestionId;//questionId.ToString();
                    renderQuestionLabel = true;
                    emailNonCitizen.CssClass = this.cssClassName;
                    emailNonCitizen.ToolTip = questionHelp;
                    controlType = emailNonCitizen;
                    break;

                case "EsccEthnicityControl":
                    EsccEthnicityControl esccEthnicity = new EsccEthnicityControl();
                    esccEthnicity.Reference = this.questionRef;
                    esccEthnicity.QuestionText = this.questionText;
                    esccEthnicity.Required = this.isRequired;
                    esccEthnicity.ID = this.htmlQuestionId;
                    renderQuestionLabel = false;
                    controlType = esccEthnicity;
                    break;
                default:
                    break;
            }
            this.baseControl = controlType;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// If appropriate, adds an indicator to the question label to show that this field is required
        /// </summary>
        public void SetRequiredValidator()
        {

            if (
                (this.questionType != "fieldsetOpen") &
                (this.questionType != "fieldsetClose") &
                (this.answerType != "RadioButtonListInteger") &
                (this.answerType != "RadioButtonListVarchar") &
                (this.answerType != "CheckBoxListInteger") &
                (this.answerType != "CheckBoxListVarchar") &
                (this.answerType != "DeclarationOfInterest") &
                (this.answerType != "EsccEthnicityControl") &
                (this.answerType != "RadioButtonListVarchar") &
                (this.answerType != "CheckBoxListInteger") &
                (this.answerType != "CheckBoxListVarchar") &
                (this.answerType != "DeclarationOfInterest") 
              &(this.answerType != "DropDownListVarchar")//Needs commenting out
              & (this.answerType != "Title")//Needs commenting out
                )
            {
                this.questionLabel.Text = this.questionRef + " " + this.questionText + "<span class=\"requiredField\">*</span>";

            }

        }
        #endregion

    }
}
