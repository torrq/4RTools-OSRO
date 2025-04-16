using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace _4RTools.Utils
{
    public static class LanguageManager
    {
        public static readonly Dictionary<string, string> SupportedLanguages = new Dictionary<string, string>
        {
            { "en", "English" },
            { "fil", "Filipino" }
        };

        public static List<KeyValuePair<string, string>> GetSupportedLanguages()
        {
            return new List<KeyValuePair<string, string>>(SupportedLanguages);
        }

        public static void ChangeLanguage(string cultureCode, Form form)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);

            ApplyResourcesToControl(form, new ComponentResourceManager(form.GetType()));
        }

        private static void ApplyResourcesToControl(Control control, ComponentResourceManager resource)
        {
            foreach (Control child in control.Controls)
            {
                ApplyResourcesToControl(child, resource);
            }

            resource.ApplyResources(control, control.Name);
        }
    }
}
