using System.Reflection;
using System.Resources;
using System.Web;

namespace Escc.FormControls.WebForms
{
    /// <summary>
    /// Read localised resources from RESX files
    /// </summary>
    internal static class LocalisedResourceReader
    {
        /// <summary>
        /// Gets a localised string from the calling application's global resource file, or an internal resource file if the application does not have a global resource file. Requires ReflectionPermission.
        /// </summary>
        /// <typeparam name="ResourceFile">The type of the resource file.</typeparam>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        internal static string ResourceString<ResourceFile>(string resourceKey)
        {
            try
            {
                var resourceType = typeof(ResourceFile);

                var localised = HttpContext.GetGlobalResourceObject(resourceType.Name, resourceKey) as string;

                if (localised == null)
                {
                    PropertyInfo resourceManagerProperty = resourceType.GetProperty("ResourceManager", (BindingFlags.Static | BindingFlags.NonPublic));
                    var manager = (ResourceManager)resourceManagerProperty.GetValue(null, null);
                    if (manager != null) localised = manager.GetString(resourceKey);
                }

                return localised;

            }
            catch (MissingManifestResourceException ex)
            {
                ex.Data.Add("Resource file", typeof(ResourceFile).ToString());
                ex.Data.Add("Resource key", resourceKey);
                throw;
            }
        }

        /// <summary>
        /// Gets a localised string from the calling application's global resource file, or an internal resource file if the application does not have a global resource file
        /// </summary>
        /// <param name="resourceFileName">Name of the resource file, without the .resx extension</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        internal static string ResourceString(string resourceFileName, string resourceKey, string defaultValue)
        {
            try
            {
                string localised = null;
                localised = HttpContext.GetGlobalResourceObject(resourceFileName, resourceKey) as string;
                if (localised == null) localised = defaultValue;
                return localised;

            }
            catch (MissingManifestResourceException ex)
            {
                ex.Data.Add("Resource file", resourceFileName);
                ex.Data.Add("Resource key", resourceKey);
                ex.Data.Add("Default value", defaultValue);
                throw;
            }
        }
    }
}
