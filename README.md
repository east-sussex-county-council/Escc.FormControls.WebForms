# Escc.FormControls.WebForms

Generic form controls and validators for use on WebForms pages. In some cases this library adds custom functionality. In other cases it simply applies our default settings to otherwise standard ASP.NET controls.

One recommended way to apply these controls to you pages is to use the `tagMapping` section in `web.config`, which lets you state that, for example, all `asp:Button` controls should be replaced by a `FormControls:EsccButton`, ensuring none are missed.

	  <system.web>
	    <pages>
	      <tagMapping>
	        <add tagType="System.Web.UI.WebControls.Button" mappedTagType="Escc.FormControls.WebForms.EsccButton, Escc.FormControls.WebForms" />
	      </tagMapping>
	    </pages>
	  </system.web>
