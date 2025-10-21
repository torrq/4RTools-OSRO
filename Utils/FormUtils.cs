using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.Core.Utils
{
    public class FormUtils
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

        public static void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                Key thisk;

                if (e.KeyCode == Keys.Oemplus)
                {
                    thisk = Key.OemPlus;
                }
                else if (e.KeyCode == Keys.Oemtilde)
                {
                    thisk = Key.OemTilde;
                }
                else if (e.KeyCode == Keys.Oemcomma)
                {
                    thisk = Key.OemComma;
                }
                else if (e.KeyCode == Keys.D0)
                {
                    thisk = Key.D0;
                }
                else if (e.KeyCode == Keys.D1)
                {
                    thisk = Key.D1;
                }
                else if (e.KeyCode == Keys.D2)
                {
                    thisk = Key.D2;
                }
                else if (e.KeyCode == Keys.D3)
                {
                    thisk = Key.D3;
                }
                else if (e.KeyCode == Keys.D4)
                {
                    thisk = Key.D4;
                }
                else if (e.KeyCode == Keys.D5)
                {
                    thisk = Key.D5;
                }
                else if (e.KeyCode == Keys.D6)
                {
                    thisk = Key.D6;
                }
                else if (e.KeyCode == Keys.D7)
                {
                    thisk = Key.D7;
                }
                else if (e.KeyCode == Keys.D8)
                {
                    thisk = Key.D8;
                }
                else if (e.KeyCode == Keys.D9)
                {
                    thisk = Key.D9;
                }
                else
                {
                    thisk = (Key)Enum.Parse(typeof(Key), e.KeyCode.ToString());
                }

                switch (thisk)
                {
                    case Key.Escape:
                    case Key.Back:
                        textBox.Text = Key.None.ToString();
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
                DebugLogger.Error("Error in OnKeyDown: " + ex.Message);
            }
        }

        public static bool IsValidKey(Key key)
        {
            return (key != Key.Back && key != Key.Escape && key != Key.None);
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

        private static void resetForm(Control control)
        {
            IEnumerable<Control> texts = GetAll(control, typeof(TextBox));
            IEnumerable<Control> checks = GetAll(control, typeof(CheckBox));
            IEnumerable<Control> combos = GetAll(control, typeof(ComboBox));
            IEnumerable<Control> numericUpDown = GetAll(control, typeof(NumericUpDown));

            foreach (Control c in texts)
            {
                TextBox textBox = (TextBox)c;
                textBox.Text = Key.None.ToString();
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

        private static void resetCheckboxForm(Control control)
        {
            IEnumerable<Control> checks = GetAll(control, typeof(CheckBox));
            IEnumerable<Control> combos = GetAll(control, typeof(ComboBox));

            foreach (Control c in checks)
            {
                CheckBox checkBox = (CheckBox)c;
                checkBox.Checked = false;
            }

            foreach (Control c in combos)
            {
                ComboBox comboBox = (ComboBox)c;
                if (comboBox.Items.Count > 0)
                    comboBox.SelectedIndex = 0;
            }
        }

        public static void ResetCheckboxForm(Form form)
        {
            resetCheckboxForm(form);
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