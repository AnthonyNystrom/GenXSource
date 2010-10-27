using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;
using System;
using System.IO;
[assembly: AssemblyTitle("Netron.Cobalt.Core")]
[assembly: AssemblyDescription("Coablt IDE v3.2")]
[assembly: AssemblyCompany("The Netron Project [http://www.netronproject.com], The Orbifold [http://www.orbifold.net]")]
[assembly: AssemblyProduct("Cobalt IDE v3.0")]
[assembly: AssemblyCopyright("GPL, http://www.fsf.org/licensing/licenses/gpl.html")]
[assembly: AssemblyVersion("3.0.*")]
[assembly: AssemblyDelaySign(false)]
[assembly: ComVisibleAttribute(false)]
[assembly: NeutralResourcesLanguageAttribute("en-US")]

/// <summary>
/// Static class which collects info about the graph library assembly
/// </summary>
static class AssemblyInfo
{

    static string binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
    const string AssemblyName = "Netron.Diagramming.Core.dll";
    #region Assembly Attribute Accessors



    /// <summary>
    /// Gets the assembly title.
    /// </summary>
    /// <value>The assembly title.</value>
    public static string AssemblyTitle
    {
        get
        {
            Assembly embly = Assembly.LoadFile(binPath + AssemblyName);
            // Get all Title attributes on this assembly
            object[] attributes = embly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            // If there is at least one Title attribute
            if (attributes.Length > 0)
            {
                // Select the first one
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                // If it is not an empty string, return it
                if (titleAttribute.Title != "")
                    return titleAttribute.Title;
            }
            // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
            return System.IO.Path.GetFileNameWithoutExtension(embly.CodeBase);
        }
    }

    /// <summary>
    /// Gets the assembly version.
    /// </summary>
    /// <value>The assembly version.</value>
    public static string AssemblyVersion
    {
        get
        {

            Assembly embly = Assembly.LoadFile(binPath + AssemblyName);
            return embly.GetName().Version.ToString();
        }
    }

    /// <summary>
    /// Gets the assembly description.
    /// </summary>
    /// <value>The assembly description.</value>
    public static string AssemblyDescription
    {
        get
        {

            Assembly embly = Assembly.LoadFile(binPath + AssemblyName);
            // Get all Description attributes on this assembly
            object[] attributes = embly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            // If there aren't any Description attributes, return an empty string
            if (attributes.Length == 0)
                return "";
            // If there is a Description attribute, return its value
            return ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }
    }

    /// <summary>
    /// Gets the assembly product.
    /// </summary>
    /// <value>The assembly product.</value>
    public static string AssemblyProduct
    {
        get
        {
            Assembly embly = Assembly.LoadFile(binPath + AssemblyName);
            // Get all Product attributes on this assembly
            object[] attributes = embly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            // If there aren't any Product attributes, return an empty string
            if (attributes.Length == 0)
                return "";
            // If there is a Product attribute, return its value
            return ((AssemblyProductAttribute)attributes[0]).Product;
        }
    }

    /// <summary>
    /// Gets the assembly copyright.
    /// </summary>
    /// <value>The assembly copyright.</value>
    public static string AssemblyCopyright
    {
        get
        {
            Assembly embly = Assembly.LoadFile(binPath + AssemblyName);
            // Get all Copyright attributes on this assembly
            object[] attributes = embly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            // If there aren't any Copyright attributes, return an empty string
            if (attributes.Length == 0)
                return "";
            // If there is a Copyright attribute, return its value
            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }
    }

    /// <summary>
    /// Gets the assembly company.
    /// </summary>
    /// <value>The assembly company.</value>
    public static string AssemblyCompany
    {
        get
        {
            Assembly embly = Assembly.LoadFile(binPath + AssemblyName);
            // Get all Company attributes on this assembly
            object[] attributes = embly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            // If there aren't any Company attributes, return an empty string
            if (attributes.Length == 0)
                return "";
            // If there is a Company attribute, return its value
            return ((AssemblyCompanyAttribute)attributes[0]).Company;
        }
    }
    #endregion

}
