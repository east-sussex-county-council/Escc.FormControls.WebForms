using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Escc.FormControls.WebForms.Properties;

namespace Escc.FormControls.WebForms
{
    /// <summary>
    /// Specify a date and time using dropdown lists
    /// </summary>
    [DefaultProperty(""),
    ToolboxData("<{0}:DateTimeFormPart runat=server></{0}:DateTimeFormPart>")]
    public class DateTimeFormPart : System.Web.UI.WebControls.WebControl
    {
        #region Fields

        /// <summary>
        /// Store the CSS class for the whole container
        /// </summary>
        private string partCss = "formPart";

        /// <summary>
        /// Store the CSS class for the label
        /// </summary>
        private string labelCss = "formLabel";

        /// <summary>
        /// Store the CSS class for the control
        /// </summary>
        private string controlCss = "formControl";

        /// <summary>
        /// Store the first year to show in the year DropDownList
        /// </summary>
        private int firstYear = DateTime.Now.AddYears(-10).Year;

        /// <summary>
        /// Store the last year to show in the year DropDownList
        /// </summary>
        private int lastYear = DateTime.Now.Year;

        /// <summary>
        /// Store text legend for date fields
        /// </summary>
        private string label;

        /// <summary>
        /// Control to choose the days of the month
        /// </summary>
        DropDownList dayList = new DropDownList();

        /// <summary>
        /// Control to choose the month of the year
        /// </summary>
        DropDownList monthList = new DropDownList();

        /// <summary>
        /// Control to choose the year
        /// </summary>
        DropDownList yearList = new DropDownList();

        /// <summary>
        /// Store date to select in case controls are repopulated
        /// </summary>
        /// <remarks>DateTime.MinValue used as stand-in for null, until we can use DateTime?</remarks>
        private DateTime dateToSelect = DateTime.MinValue;
        private string resourceFileName = typeof(EsccWebTeam_FormControls).Name;

        #endregion

        #region Constructors

        /// <summary>
        /// Specify a date and time using dropdown lists
        /// </summary>
        /// <param name="label">Text to be used for form control label</param>
        /// <remarks>Time has not yet been implemented - needs to remain optional</remarks>
        public DateTimeFormPart(string label)
            : base(HtmlTextWriterTag.Fieldset)
        {
            // set style
            this.CssClass = this.partCss;

            // set up dropdown lists
            HtmlGenericControl dateBox = new HtmlGenericControl("div");
            dateBox.Attributes["class"] = this.controlCss + " dateControl";

            // set up days
            dayList.Attributes["title"] = LocalisedResourceReader.ResourceString(resourceFileName, "DaySelectPopupText", EsccWebTeam_FormControls.DaySelectPopupText);
            dayList.ID = "day";
            dayList.Items.Add(new ListItem());

            for (short i = 1; i <= 31; i++)
            {
                ListItem item = new ListItem(i.ToString(), i.ToString());
                dayList.Items.Add(item);
            }

            // set up months
            monthList.Attributes["title"] = LocalisedResourceReader.ResourceString(resourceFileName, "MonthSelectPopupText", EsccWebTeam_FormControls.MonthSelectPopupText);
            monthList.ID = "month";
            monthList.Items.Add(new ListItem());

            string[] monthNames = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames;
            for (int i = 0; i < monthNames.Length; i++)
            {
                if (monthNames[i].Length > 0)
                {
                    int monthVal = i + 1;
                    ListItem item = new ListItem(monthNames[i], monthVal.ToString());
                    monthList.Items.Add(item);
                }
            }

            // set up year
            yearList.Attributes["title"] = LocalisedResourceReader.ResourceString(resourceFileName, "YearSelectPopupText", EsccWebTeam_FormControls.YearSelectPopupText);
            yearList.ID = "year";
            this.GenerateYearOptions();


            // set up legend 
            this.label = label;
            HtmlGenericControl legend = new HtmlGenericControl("legend");
            legend.Attributes["class"] = this.labelCss;

            // TODO: should be innertext, but labels have been set using HTML for <span class="requiredField">
            legend.InnerHtml = this.label;

            // set up field labels
            Label dayLabel = new Label();
            dayLabel.Text = LocalisedResourceReader.ResourceString(resourceFileName, "DatePartDayEntryPrompt", EsccWebTeam_FormControls.DatePartDayEntryPrompt);
            dayLabel.AssociatedControlID = dayList.ID;

            Label monthLabel = new Label();
            monthLabel.Text = LocalisedResourceReader.ResourceString(resourceFileName, "DatePartMonthEntryPrompt", EsccWebTeam_FormControls.DatePartMonthEntryPrompt);
            monthLabel.AssociatedControlID = monthList.ID;

            Label yearLabel = new Label();
            yearLabel.Text = LocalisedResourceReader.ResourceString(resourceFileName, "DatePartYearEntryPrompt", EsccWebTeam_FormControls.DatePartYearEntryPrompt);
            yearLabel.AssociatedControlID = yearList.ID;

            // add controls
            this.Controls.Add(legend);
            dateBox.Controls.Add(dayLabel);
            dateBox.Controls.Add(dayList);
            dateBox.Controls.Add(monthLabel);
            dateBox.Controls.Add(monthList);
            dateBox.Controls.Add(yearLabel);
            dateBox.Controls.Add(yearList);
            this.Controls.Add(dateBox);

            // add validators
            CustomValidator vcDayReqMonthYear = new CustomValidator();
            vcDayReqMonthYear.ControlToValidate = dayList.ID;
            vcDayReqMonthYear.ServerValidate += new ServerValidateEventHandler(this.DayRequiresMonthAndYearValidator);
            vcDayReqMonthYear.ErrorMessage = LocalisedResourceReader.ResourceString(resourceFileName, "DateRequiresMoreThanDayError", EsccWebTeam_FormControls.DateRequiresMoreThanDayError);
            vcDayReqMonthYear.Display = ValidatorDisplay.None;
            this.Controls.Add(vcDayReqMonthYear);

            CustomValidator vcMonthReqDayYear = new CustomValidator();
            vcMonthReqDayYear.ControlToValidate = monthList.ID;
            vcMonthReqDayYear.ServerValidate += new ServerValidateEventHandler(this.MonthRequiresDayAndYearValidator);
            vcMonthReqDayYear.ErrorMessage = LocalisedResourceReader.ResourceString(resourceFileName, "DateRequiresMoreThanMonthError", EsccWebTeam_FormControls.DateRequiresMoreThanMonthError);
            vcMonthReqDayYear.Display = ValidatorDisplay.None;
            this.Controls.Add(vcMonthReqDayYear);

            CustomValidator vcYearReqDayMonth = new CustomValidator();
            vcYearReqDayMonth.ControlToValidate = yearList.ID;
            vcYearReqDayMonth.ServerValidate += new ServerValidateEventHandler(this.YearRequiresMonthAndDayValidator);
            vcYearReqDayMonth.ErrorMessage = LocalisedResourceReader.ResourceString(resourceFileName, "DateRequiresMoreThanMonthError", EsccWebTeam_FormControls.DateRequiresMoreThanMonthError);
            vcYearReqDayMonth.Display = ValidatorDisplay.None;
            this.Controls.Add(vcYearReqDayMonth);

            CustomValidator vcIsDateValid = new CustomValidator();
            vcIsDateValid.ControlToValidate = dayList.ID;
            vcIsDateValid.ServerValidate += new ServerValidateEventHandler(this.IsDateValidValidator);
            vcIsDateValid.ErrorMessage = "The date you selected is not a valid date.";
            vcIsDateValid.Display = ValidatorDisplay.None;
            this.Controls.Add(vcIsDateValid);

        }

        /// <summary>
        /// OVERRIDDEN FOR EFORMS
        /// Specify a date and time using dropdown lists
        /// </summary>
        /// <param name="label">Text to be used for form control label</param>
        /// <param name="questionId"></param>
        /// <param name="questionRef"></param>
        /// <param name="required"></param>
        /// <param name="lblcss">CSS class to be applied to the &lt;legend&gt; element</param>
        /// <param name="ctrlcss">CSS class to be applied to the whole control</param>
        /// <remarks>Time has not yet been implemented - needs to remain optional</remarks>

        public DateTimeFormPart(string label, string questionId, string questionRef, bool required, string lblcss, string ctrlcss)
            : base(HtmlTextWriterTag.Fieldset)
        {
            // set style
            this.CssClass = this.partCss;

            // set up dropdown lists
            HtmlGenericControl dateBox = new HtmlGenericControl("div");

            dateBox.Attributes["class"] = ctrlcss;

            // set up days
            dayList.Attributes["title"] = LocalisedResourceReader.ResourceString(resourceFileName, "DaySelectPopupText", EsccWebTeam_FormControls.DaySelectPopupText);
            dayList.ID = "day" + questionId;
            dayList.Items.Add(new ListItem());

            for (short i = 1; i <= 31; i++)
            {
                ListItem item = new ListItem(i.ToString(), i.ToString());
                dayList.Items.Add(item);
            }

            // set up months
            monthList.Attributes["title"] = LocalisedResourceReader.ResourceString(resourceFileName, "MonthSelectPopupText", EsccWebTeam_FormControls.MonthSelectPopupText);
            monthList.ID = "month" + questionId;
            monthList.Items.Add(new ListItem());

            string[] monthNames = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames;
            for (int i = 0; i < monthNames.Length; i++)
            {
                if (monthNames[i].Length > 0)
                {
                    int monthVal = i + 1;
                    ListItem item = new ListItem(monthNames[i], monthVal.ToString());
                    monthList.Items.Add(item);
                }
            }

            // set up year
            yearList.Attributes["title"] = LocalisedResourceReader.ResourceString(resourceFileName, "YearSelectPopupText", EsccWebTeam_FormControls.YearSelectPopupText);
            yearList.ID = "year" + questionId;
            this.GenerateYearOptions();


            // set up legend 
            this.label = label;//TextXhtml.TextManipulation.TextManipulationUtilities.LegendWrapLines(label,25);
            //this.label = label;
            HtmlGenericControl legend = new HtmlGenericControl("legend");
            legend.Attributes["class"] = lblcss;

            if (required)
            {
                legend.InnerHtml = questionRef + " " + this.label + "<span class=\"requiredField\">*</span>";
            }
            else
            {
                legend.InnerHtml = questionRef + " " + this.label;
            }
            // set up field labels
            Label dayLabel = new Label();
            dayLabel.Text = LocalisedResourceReader.ResourceString(resourceFileName, "DatePartDayEntryPrompt", EsccWebTeam_FormControls.DatePartDayEntryPrompt);
            dayLabel.AssociatedControlID = dayList.ID;

            Label monthLabel = new Label();
            monthLabel.Text = LocalisedResourceReader.ResourceString(resourceFileName, "DatePartMonthEntryPrompt", EsccWebTeam_FormControls.DatePartMonthEntryPrompt);
            monthLabel.AssociatedControlID = monthList.ID;

            Label yearLabel = new Label();
            yearLabel.Text = LocalisedResourceReader.ResourceString(resourceFileName, "DatePartYearEntryPrompt", EsccWebTeam_FormControls.DatePartYearEntryPrompt);
            yearLabel.AssociatedControlID = yearList.ID;

            // add controls
            this.Controls.Add(legend);
            dateBox.Controls.Add(dayLabel);
            dateBox.Controls.Add(dayList);
            dateBox.Controls.Add(monthLabel);
            dateBox.Controls.Add(monthList);
            dateBox.Controls.Add(yearLabel);
            dateBox.Controls.Add(yearList);
            this.Controls.Add(dateBox);

            if (required)
            {

                questionRef = questionRef.Replace(".", " ");

                RequiredFieldValidator rfvDay = new RequiredFieldValidator();
                rfvDay.ControlToValidate = dayList.ID;
                rfvDay.ErrorMessage = "question " + questionRef + "requires the day to be completed ";
                rfvDay.Display = ValidatorDisplay.None;
                rfvDay.EnableClientScript = false;
                this.Controls.Add(rfvDay);

                RequiredFieldValidator rfvMonth = new RequiredFieldValidator();
                rfvMonth.ControlToValidate = monthList.ID;
                rfvMonth.ErrorMessage = "question " + questionRef + "requires the month to be completed ";
                rfvMonth.Display = ValidatorDisplay.None;
                rfvMonth.EnableClientScript = false;
                this.Controls.Add(rfvMonth);

                RequiredFieldValidator rfvYear = new RequiredFieldValidator();
                rfvYear.ControlToValidate = yearList.ID;
                rfvYear.ErrorMessage = "question " + questionRef + "requires the year to be completed ";
                rfvYear.Display = ValidatorDisplay.None;
                rfvYear.EnableClientScript = false;
                this.Controls.Add(rfvYear);
            }

            CustomValidator vcIsDateValid = new CustomValidator();
            vcIsDateValid.ControlToValidate = dayList.ID;
            vcIsDateValid.ServerValidate += new ServerValidateEventHandler(this.IsDateValidValidator);
            vcIsDateValid.ErrorMessage = "The date you selected for question " + questionRef + " is not a valid date.";
            vcIsDateValid.Display = ValidatorDisplay.None;
            vcIsDateValid.EnableClientScript = false;
            this.Controls.Add(vcIsDateValid);


        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets whether to show controls for setting the time (not implemented)
        /// </summary>
        public bool ShowTime
        {
            get { throw new NotImplementedException("The DateTimeFormPart doesn't support setting times yet"); }
        }

        /// <summary>
        /// Gets or sets the first year to show in the year DropDownList
        /// </summary>
        public int FirstYear
        {
            get { return this.firstYear; }
            set
            {
                this.firstYear = value;
                this.GenerateYearOptions();
            }
        }

        /// <summary>
        /// Gets or sets the last year to show in the year DropDownList
        /// </summary>
        public int LastYear
        {
            get { return this.lastYear; }
            set
            {
                this.lastYear = value;
                this.GenerateYearOptions();
            }
        }

        /// <summary>
        /// Gets or sets the date selected in the dropdown lists
        /// </summary>
        public DateTime SelectedDate
        {
            get
            {
                // get date from the lists
                if (this.dayList.SelectedIndex > 0 && this.monthList.SelectedIndex > 0 && this.yearList.SelectedIndex > 0)
                {
                    return new DateTime(Convert.ToInt32(this.yearList.Items[this.yearList.SelectedIndex].Value), Convert.ToInt32(this.monthList.Items[this.monthList.SelectedIndex].Value), Convert.ToInt32(this.dayList.Items[this.dayList.SelectedIndex].Value));
                }
                else return DateTime.MinValue;
            }

            set
            {
                this.dateToSelect = value;
                this.SelectDate();
            }
        }




        #endregion

        #region Methods




        /// <summary>
        /// Whenever range of years changes, we need to regenerate the options in the dropdown list
        /// </summary>
        private void GenerateYearOptions()
        {
            this.yearList.Items.Clear();
            this.yearList.Items.Add(new ListItem());

            if (this.firstYear <= this.lastYear)
            {
                for (int i = firstYear; i <= this.lastYear; i++)
                {
                    ListItem item = new ListItem(i.ToString(), i.ToString());
                    this.yearList.Items.Add(item);
                }
            }
            else
            {
                for (int i = firstYear; i >= this.lastYear; i--)
                {
                    ListItem item = new ListItem(i.ToString(), i.ToString());
                    this.yearList.Items.Add(item);
                }
            }

            // Reselect any date that was previously selected
            this.SelectDate();
        }

        /// <summary>
        /// Select a date in the dropdown lists
        /// </summary>
        private void SelectDate()
        {
            if (this.dateToSelect != DateTime.MinValue)
            {
                this.dayList.SelectedItem.Selected = false;
                ListItem dayToSelect = this.dayList.Items.FindByValue(this.dateToSelect.Day.ToString());
                if (dayToSelect != null) dayToSelect.Selected = true;

                this.monthList.SelectedItem.Selected = false;
                ListItem monthToSelect = this.monthList.Items.FindByValue(this.dateToSelect.Month.ToString());
                if (monthToSelect != null) monthToSelect.Selected = true;

                this.yearList.SelectedItem.Selected = false;
                ListItem yearToSelect = this.yearList.Items.FindByValue(this.dateToSelect.Year.ToString());
                if (yearToSelect != null) yearToSelect.Selected = true;
            }
        }

        /// <summary>
        /// Validator ensuring a complete date must be selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DayRequiresMonthAndYearValidator(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = (this.monthList.SelectedIndex > 0 && this.yearList.SelectedIndex > 0);
        }

        /// <summary>
        /// Validator ensuring a complete date must be selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthRequiresDayAndYearValidator(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = (this.dayList.SelectedIndex > 0 && this.yearList.SelectedIndex > 0);
        }

        /// <summary>
        /// Validator ensuring a complete date must be selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YearRequiresMonthAndDayValidator(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = (this.dayList.SelectedIndex > 0 && this.monthList.SelectedIndex > 0);
        }

        private void IsDateValidValidator(object sender, ServerValidateEventArgs e)
        {
            try
            {

                string year = this.yearList.SelectedItem.Text;
                string month = this.monthList.SelectedItem.Value;
                string day = this.dayList.SelectedItem.Text;
                string separator = @"/";



                DateTime validDate = DateTime.Parse(year + separator + month + separator + day);
                e.IsValid = true;
            }
            catch (FormatException)
            {
                e.IsValid = false;
            }
            catch (ArgumentOutOfRangeException)
            {
                e.IsValid = false;

            }


        }

        #endregion

    }
}
