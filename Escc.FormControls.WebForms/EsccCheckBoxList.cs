using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Escc.FormControls.WebForms
{
	/// <summary>
	/// Summary description for EsccCheckBoxList.
	/// </summary>
	public class EsccCheckBoxList : CheckBoxList, IRepeatInfoUser
	{

			private CheckBox controlToRepeat;




            /// <summary>
            /// Initializes a new instance of the <see cref="EsccCheckBoxList"/> class.
            /// </summary>
		public EsccCheckBoxList()
		{
			this.controlToRepeat = new CheckBox();
			this.controlToRepeat.ID = "0";
			this.controlToRepeat.EnableViewState = false;
			this.Controls.Add(this.controlToRepeat);

		}
        /// <summary>
        /// Configures the <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see> control prior to rendering on the client.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			this.controlToRepeat.AutoPostBack = this.AutoPostBack;
			if (this.Page != null)
			{
				for (int num1 = 0; num1 < this.Items.Count; num1++)
				{
					this.controlToRepeat.ID = num1.ToString(NumberFormatInfo.InvariantInfo);
					this.Page.RegisterRequiresPostBack(this.controlToRepeat);
				}
			}
		}

        /// <summary>
        /// Do not renders the default HTML opening tag of the control.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"></see> that represents the output stream to render HTML content on the client.</param>
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			
		}

        /// <summary>
        /// Do not render the default HTML closing tag of the control.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"></see> that represents the output stream to render HTML content on the client.</param>
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			
		}



        /// <summary>
        /// Saves the current view state of the <see cref="T:System.Web.UI.WebControls.ListControl"></see> -derived control and the items it contains.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that contains the saved state of the <see cref="T:System.Web.UI.WebControls.ListControl"></see> control.
        /// </returns>
		protected override object SaveViewState()
		{
			// Create an object array with one element for the CheckBoxList's
			// ViewState contents, and one element for each ListItem in skmCheckBoxList
			object [] state = new object[this.Items.Count + 1];

			object baseState = base.SaveViewState();
			state[0] = baseState;

			// Now, see if we even need to save the view state
			bool itemHasAttributes = false;
			for (int i = 0; i < this.Items.Count; i++)
			{
				if (this.Items[i].Attributes.Count > 0)
				{
					itemHasAttributes = true;
					
					// Create an array of the item's Attribute's keys and values
					object [] attribKV = new object[this.Items[i].Attributes.Count * 2];
					int k = 0;
					foreach(string key in this.Items[i].Attributes.Keys)
					{
						attribKV[k++] = key;
						attribKV[k++] = this.Items[i].Attributes[key];
					}

					state[i+1] = attribKV;
				}
			}

			// return either baseState or state, depending on whether or not
			// any ListItems had attributes
			if (itemHasAttributes)
				return state;
			else
				return baseState;
		}

        /// <summary>
        /// Loads the previously saved view state of the <see cref="T:System.Web.UI.WebControls.DetailsView"></see> control.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object"></see> that represents the state of the <see cref="T:System.Web.UI.WebControls.ListControl"></see> -derived control.</param>
		protected override void LoadViewState(object savedState)
		{
			if (savedState == null) return;

			// see if savedState is an object or object array
			if (savedState is object[])
			{
				// we have an array of items with attributes
				object [] state = (object[]) savedState;
				base.LoadViewState(state[0]);	// load the base state

				for (int i = 1; i < state.Length; i++)
				{
					if (state[i] != null)
					{
						// Load back in the attributes
						object [] attribKV = (object[]) state[i];
						for (int k = 0; k < attribKV.Length; k += 2)
							this.Items[i-1].Attributes.Add(attribKV[k].ToString(), attribKV[k+1].ToString());
					}
				}
			}
			else
				// we have just the base state
				base.LoadViewState(savedState);
		}



        /// <summary>
        /// Displays the <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see> on the client.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"></see> that contains the output stream for rendering on the client.</param>
		protected override void Render(HtmlTextWriter writer)
		{
			RepeatInfo info1 = new RepeatInfo();
			Style style1 = base.ControlStyleCreated ? base.ControlStyle : null;
			short num1 = this.TabIndex;
			bool flag1 = false;
			this.controlToRepeat.TabIndex = num1;
			if (num1 != 0)
			{
				if (!this.ViewState.IsItemDirty("TabIndex"))
				{
					flag1 = true;
				}
				this.TabIndex = 0;
			}
			info1.RepeatColumns = this.RepeatColumns;
			info1.RepeatDirection = this.RepeatDirection;
			info1.RepeatLayout = this.RepeatLayout;
			info1.RenderRepeater(writer, this, style1, this);
			if (num1 != 0)
			{
				this.TabIndex = num1;
			}
			if (flag1)
			{
				this.ViewState.SetItemDirty("TabIndex", false);
			}
		}
		#region IRepeatInfoUser Members

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see> control contains a heading section.
        /// </summary>
        /// <value></value>
        /// <returns>false, indicating that a <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see> does not contain a heading section.</returns>
        protected override bool HasHeader
		{
			get
			{
				// TODO:  Add skmCheckBoxList.HasHeader getter implementation
				return false;
			}
		}

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see> control contains a separator between items in the list.
        /// </summary>
        /// <value></value>
        /// <returns>false, indicating that a <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see> does not contain separators.</returns>
        protected override bool HasSeparators
		{
			get
			{
				// TODO:  Add skmCheckBoxList.HasSeparators getter implementation
				return false;
			}
		}

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see> control contains a footer section.
        /// </summary>
        /// <value></value>
        /// <returns>false, indicating that a <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see> does not contain a footer section.</returns>
        public bool HasFooter
		{
			get
			{
				// TODO:  Add skmCheckBoxList.HasFooter getter implementation
				return false;
			}
		}

        /// <summary>
        /// Renders a list item in the <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see> control.
        /// </summary>
        /// <param name="itemType">One of the <see cref="T:System.Web.UI.WebControls.ListItemType"></see> enumeration values.</param>
        /// <param name="repeatIndex">An ordinal index that specifies the location of the item in the list control.</param>
        /// <param name="repeatInfo">A <see cref="T:System.Web.UI.WebControls.RepeatInfo"></see> that represents the information used to render the item in the list.</param>
        /// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter"></see> that represents the output stream to render HTML content on the client.</param>
		public void RenderItem(System.Web.UI.WebControls.ListItemType itemType, int repeatIndex, RepeatInfo repeatInfo, HtmlTextWriter writer)
		{
			this.controlToRepeat.ID = repeatIndex.ToString(NumberFormatInfo.InvariantInfo);
			this.controlToRepeat.Text = this.Items[repeatIndex].Text;
			this.controlToRepeat.TextAlign = this.TextAlign;
			this.controlToRepeat.Checked = this.Items[repeatIndex].Selected;
			this.controlToRepeat.Enabled = this.Enabled;
			this.controlToRepeat.Attributes.Clear();
			foreach (string key in this.Items[repeatIndex].Attributes.Keys)
				this.controlToRepeat.Attributes.Add(key, this.Items[repeatIndex].Attributes[key]);
			
			//this.controlToRepeat.RenderBeginTag(writer);
			
			//HtmlGenericControl span = new HtmlGenericControl("span");
			//span.Attributes.Add("class", "formControl");
			
			
			//this.controlToRepeat.RenderControl(writer);/**/
			this.controlToRepeat.RenderControl(writer);
			//span.Controls.Add(controlToRepeat);
			//span.RenderControl(writer);
			//this.Controls.Add(span);

		}

        /// <summary>
        /// Retrieves the style of the specified item type at the specified index in the list control.
        /// </summary>
        /// <param name="itemType">One of the <see cref="T:System.Web.UI.WebControls.ListItemType"></see> enumeration values.</param>
        /// <param name="repeatIndex">An ordinal index that specifies the location of the item in the list control.</param>
        /// <returns>
        /// null, indicating that style attributes are not set on individual list items in a <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see> control.
        /// </returns>
		public Style GetItemStyle(System.Web.UI.WebControls.ListItemType itemType, int repeatIndex)
		{
			// TODO:  Add skmCheckBoxList.GetItemStyle implementation
			return null;
		}

        /// <summary>
        /// Gets the number of list items in the <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see> control.
        /// </summary>
        /// <value></value>
        /// <returns>The number of items in the <see cref="T:System.Web.UI.WebControls.CheckBoxList"></see>.</returns>
		public int RepeatedItemCount
		{
			get
			{
				// TODO:  Add skmCheckBoxList.RepeatedItemCount getter implementation
				return this.Items.Count;
			}
		}

		#endregion
	}
}
