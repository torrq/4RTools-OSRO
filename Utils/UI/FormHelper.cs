using _ORTools.Controls;
using _ORTools.Forms;
using _ORTools.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace _ORTools.Utils
{
    public static class FormHelper
    {
        public static void ApplyColorToButtons(Control parentControl, string[] buttonNames, Color color)
        {
            if (buttonNames == null || buttonNames.Length == 0) return;

            int darkenAmount = AppConfig.ProfileButtonBorderDarkenAmount; // Get darkness amount from AppConfig

            foreach (string buttonName in buttonNames)
            {
                Control[] foundControls = parentControl.Controls.Find(buttonName, true);

                if (foundControls.Length > 0 && foundControls[0] is Button button)
                {
                    button.BackColor = color;

                    // Calculate and apply darker border color
                    Color borderColor = DarkenColor(color, darkenAmount);
                    button.FlatAppearance.BorderColor = borderColor;
                    button.FlatAppearance.BorderSize = 1; // Ensure border is visible
                }
                else { DebugLogger.Warning($"Button '{buttonName}' not found in {parentControl.Name}."); }
            }
        }

        public static void ToggleStateOff(string DebugLogType = "FormHelper")
        {
            var frmStateSwitch = (StateSwitchForm)Application.OpenForms["StateSwitchForm"];
            if (frmStateSwitch != null)
            {
                frmStateSwitch.TurnOFF();
            }
            else
            {
                DebugLogger.Error($"{DebugLogType}: Could not find 'StateSwitchForm' to toggle status.");
            }
        }

        public static void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                Keys thisk;

                if (e.KeyCode == Keys.Oemplus)
                {
                    thisk = Keys.Oemplus;
                }
                else if (e.KeyCode == Keys.Oemtilde)
                {
                    thisk = Keys.Oemtilde;
                }
                else if (e.KeyCode == Keys.Oemcomma)
                {
                    thisk = Keys.Oemcomma;
                }
                else if (e.KeyCode == Keys.D0)
                {
                    thisk = Keys.D0;
                }
                else if (e.KeyCode == Keys.D1)
                {
                    thisk = Keys.D1;
                }
                else if (e.KeyCode == Keys.D2)
                {
                    thisk = Keys.D2;
                }
                else if (e.KeyCode == Keys.D3)
                {
                    thisk = Keys.D3;
                }
                else if (e.KeyCode == Keys.D4)
                {
                    thisk = Keys.D4;
                }
                else if (e.KeyCode == Keys.D5)
                {
                    thisk = Keys.D5;
                }
                else if (e.KeyCode == Keys.D6)
                {
                    thisk = Keys.D6;
                }
                else if (e.KeyCode == Keys.D7)
                {
                    thisk = Keys.D7;
                }
                else if (e.KeyCode == Keys.D8)
                {
                    thisk = Keys.D8;
                }
                else if (e.KeyCode == Keys.D9)
                {
                    thisk = Keys.D9;
                }
                else
                {
                    thisk = (Keys)e.KeyCode;
                }

                switch (thisk)
                {
                    case Keys.Escape:
                    case Keys.Back:
                        textBox.Text = Keys.None.ToString();
                        break;

                    default:
                        textBox.Text = thisk.ToString();
                        break;
                }

                textBox.Parent.Focus();
                e.Handled = true;
            }
            catch (Exception ex)
            {
                DebugLogger.Error("Error in OnKeyDown: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static bool IsValidKey(Keys key)
        {
            return (key != Keys.Back && key != Keys.Escape && key != Keys.None);
        }

        public static void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public static IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                 .Concat(controls)
                                 .Where(c => c.GetType() == type);
        }

        private static readonly string KeyNoneString = Keys.None.ToString();

        private static void resetForm(Control control)
        {
            IEnumerable<Control> texts = GetAll(control, typeof(TextBox));
            IEnumerable<Control> checks = GetAll(control, typeof(CheckBox));
            IEnumerable<Control> combos = GetAll(control, typeof(ComboBox));
            IEnumerable<Control> numericUpDown = GetAll(control, typeof(NumericUpDown));

            foreach (Control c in texts)
            {
                TextBox textBox = (TextBox)c;
                textBox.Text = KeyNoneString;
            }

            foreach (Control c in checks)
            {
                CheckBox checkBox = (CheckBox)c;
                checkBox.Checked = false;
                checkBox.CheckState = 0;
            }

            foreach (Control c in combos)
            {
                ComboBox comboBox = (ComboBox)c;
                if (comboBox.Items.Count > 0)
                    comboBox.SelectedIndex = 0;
            }

            foreach (Control n in numericUpDown)
            {
                NumericUpDown numeric = (NumericUpDown)n;

                decimal safeValue = Math.Max(numeric.Minimum, 0);
                numeric.Value = safeValue;
            }
        }

        public static void ResetCheckboxForm(Form form)
        {
            foreach (Control c in form.Controls)
            {
                if (c is BorderedCheckBox check)
                {
                    check.CheckState = CheckState.Unchecked;
                }
            }
        }

        public static void ResetForm(Form form)
        {
            resetForm(form);
        }

        public static void ResetForm(Control form)
        {
            resetForm(form);
        }

        public static void ResetForm(GroupBox group)
        {
            resetForm(group);
        }

        public static void SetNumericUpDownMinimumDelays(Form form, decimal? overrideMinimumDelay = null)
        {
            decimal minimumDelayValue = overrideMinimumDelay ?? AppConfig.DefaultMinimumDelay;

            foreach (Control control in GetAllControls(form))
            {
                if (control is NumericUpDown delayInput && delayInput.Name.IndexOf("delay", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    delayInput.Minimum = minimumDelayValue;

                    if (delayInput.Value < delayInput.Minimum)
                    {
                        delayInput.Value = delayInput.Minimum;
                    }
                }
            }
        }

        public static class TabIconHelper
        {
            public static void SetTabIcons(TabControl tabControl, List<Image> images)
            {
                if (tabControl.TabPages.Count != images.Count)
                    throw new ArgumentException("The number of images must match the number of tab pages.");

                ImageList imageList = new ImageList
                {
                    ImageSize = new Size(10, 12)
                };

                for (int i = 0; i < images.Count; i++)
                {
                    imageList.Images.Add(images[i]);
                    tabControl.TabPages[i].ImageIndex = i;
                }

                tabControl.ImageList = imageList;
            }
        }

        private static IEnumerable<Control> GetAllControls(Control container)
        {
            var controls = container.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAllControls(ctrl))
                                 .Concat(controls);
        }

        // Helper method to darken a color
        private static Color DarkenColor(Color color, int amount)
        {
            int r = Math.Max(0, color.R - amount);
            int g = Math.Max(0, color.G - amount);
            int b = Math.Max(0, color.B - amount);
            return Color.FromArgb(r, g, b);
        }

        public static void AttachBlankFix(NumericUpDown num)
        {
            num.Leave += ForceValidZeroIfBlank;
            num.Validating += ForceValidZeroIfBlank;
        }

        private static void ForceValidZeroIfBlank(object sender, EventArgs e)
        {
            var num = sender as NumericUpDown;
            if (num != null && string.IsNullOrWhiteSpace(num.Text))
            {
                num.Value = 0;
                // Force re-parse by resetting text
                num.Text = "0";
            }
        }

        public class ResourceLoader : IResourceLoader
        {
            public Bitmap LoadIcon(string iconName)
            {
                return Resources.Media.Icons.ResourceManager.GetObject(iconName) as Bitmap;
            }
        }

        public class Logger : ILogger
        {
            public void Error(string message) => DebugLogger.Error(message);
            public void Error(Exception ex, string message) => DebugLogger.Error(ex, message);
            public void Warn(string message) => DebugLogger.Error(message); // Using Error for Warn as a fallback
        }

        // Interface for resource loading
        public interface IResourceLoader
        {
            Bitmap LoadIcon(string iconName);
        }

        // Interface for logging
        public interface ILogger
        {
            void Error(string message);
            void Error(Exception ex, string message);
        }

        public static void ApplyInputKeyStyle(TextBox textBox, bool hasKey = false)
        {
            if (textBox == null)
            {
                DebugLogger.Error("ApplyInputKeyStyle: TextBox is null");
                return;
            }

            try
            {
                Font baseFont;

                // Keep original Tag for business logic, store cached font in AccessibleDescription
                if (textBox.AccessibleDescription != null &&
                    textBox.AccessibleDescription.StartsWith("cachedFont:", StringComparison.OrdinalIgnoreCase))
                {
                    // Try to retrieve cached font
                    baseFont = textBox.Font ?? Control.DefaultFont;
                }
                else
                {
                    baseFont = textBox.Font ?? Control.DefaultFont;
                    textBox.AccessibleDescription = "cachedFont:"; // mark as cached
                }

                FontStyle newStyle = hasKey ? FontStyle.Bold : FontStyle.Regular;

                if (textBox.Font == null || textBox.Font.Style != newStyle)
                {
                    if (textBox.Font != null && textBox.Font != baseFont)
                    {
                        textBox.Font.Dispose();
                    }
                    textBox.Font = new Font(baseFont, newStyle);
                }

                textBox.ForeColor = hasKey ? AppConfig.ActiveKeyColor : AppConfig.InactiveKeyColor;
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"ApplyInputKeyStyle: Failed to apply style to TextBox - {ex.Message}");
            }
        }

        /// <summary>
        /// Copies event handlers from a source control to a target control
        /// </summary>
        /// <param name="sourceControl">The control to copy event handlers from</param>
        /// <param name="targetControl">The control to copy event handlers to</param>
        public static void CopyEventHandlers(Control sourceControl, Control targetControl)
        {
            if (sourceControl == null || targetControl == null)
                return;

            // Only copy if controls are of the same type
            if (sourceControl.GetType() != targetControl.GetType())
                return;

            try
            {
                // Get the Events property using reflection
                PropertyInfo eventsProperty = typeof(Component).GetProperty("Events",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (eventsProperty == null)
                    return;

                EventHandlerList sourceEvents = (EventHandlerList)eventsProperty.GetValue(sourceControl, null);
                EventHandlerList targetEvents = (EventHandlerList)eventsProperty.GetValue(targetControl, null);

                if (sourceEvents == null || targetEvents == null)
                    return;

                // Get common event keys for common control types
                var eventKeys = GetEventKeys(sourceControl);

                foreach (var eventKey in eventKeys)
                {
                    if (eventKey != null)
                    {
                        Delegate sourceHandler = sourceEvents[eventKey];
                        if (sourceHandler != null)
                        {
                            // Copy the event handler to the target control
                            targetEvents.AddHandler(eventKey, sourceHandler);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Event copying is not critical, but we can log for debugging
                DebugLogger.Debug($"Failed to copy event handlers: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets event keys for common control events
        /// </summary>
        /// <param name="control">The control to get event keys for</param>
        /// <returns>List of event keys</returns>
        private static List<object> GetEventKeys(Control control)
        {
            var eventKeys = new List<object>();

            try
            {
                Type controlType = control.GetType();

                // Get common event field keys using reflection
                var eventFields = new[]
                {
            "EventClick",
            "EventDoubleClick",
            "EventMouseClick",
            "EventMouseDoubleClick",
            "EventMouseDown",
            "EventMouseUp",
            "EventMouseMove",
            "EventMouseEnter",
            "EventMouseLeave",
            "EventMouseHover",
            "EventKeyDown",
            "EventKeyUp",
            "EventKeyPress",
            "EventTextChanged",
            "EventEnter",
            "EventLeave",
            "EventGotFocus",
            "EventLostFocus",
            "EventValueChanged" // For NumericUpDown
        };

                // Try to get event keys from Control class and the specific control type
                Type[] typesToCheck = { typeof(Control), controlType };

                foreach (Type typeToCheck in typesToCheck)
                {
                    foreach (string eventField in eventFields)
                    {
                        FieldInfo field = typeToCheck.GetField(eventField,
                            BindingFlags.Static | BindingFlags.NonPublic);

                        if (field != null)
                        {
                            object eventKey = field.GetValue(null);
                            if (eventKey != null && !eventKeys.Contains(eventKey))
                            {
                                eventKeys.Add(eventKey);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log reflection errors for debugging, but don't throw
                DebugLogger.Debug($"Failed to get event keys for {control?.GetType().Name}: {ex.Message}");
            }

            return eventKeys;
        }
    }

    public static class EnumExtensions
    {
        public static string ToDescriptionString(this EffectStatusIDs val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
                .GetType()
                .GetField(val.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static EffectStatusIDs ToEffectStatusId(this string val)
        {
            EffectStatusIDs t = Enum.GetValues(typeof(EffectStatusIDs))
                    .Cast<EffectStatusIDs>()
                    .FirstOrDefault(v => v.GetDescription() == val);
            return t;
        }

    }
}