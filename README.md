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

Some controls are translated into multiple languages, including Kurdish Sorani. The Kurdish culture is not installed on Windows by default. The code to create and install it can be found in the `Escc.ContactUs.Website` project, with the form for which these controls were translated.

## Address lookup

The `FormAddressNonCitizen` control allows users to enter a postcode and have their address populated automatically, or enter their address manually if they prefer.

This can be placed on the page like any other control:

    <%@ Register TagPrefix="FormControls" Namespace="Escc.FormControls.WebForms" Assembly="Escc.FormControls.WebForms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=06fad7304560ae6f" %>
	
	<FormControls:FormAddressNonCitizen ID="address" runat="server"/> 

However, it also needs the source of the address data to be configured in `web.config`:

	<appSettings>
	  <add key="LocateApiAddressesUrl" value="https://hostname/addresses?query=residentialAndCommercial&amp;format=all&amp;postcode={0}"/>
	  <add key="LocateApiToken" value="YOUR_TOKEN" />
	</appSettings>

This should be a `locate-api` service based on [https://github.com/alphagov/locate-api](https://github.com/alphagov/locate-api). If you need to configure a proxy server to access the service, use the format documented in the [Escc.Net](https://github.com/east-sussex-county-council/Escc.Net) project.

The selected address is accessible via the control's `AddressInfo` property, or its `BS7666Address`, `Uprn`, `Latitude` and `Longitude` properties.