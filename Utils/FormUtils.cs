using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace _4RTools.Utils
{
    public class FormUtils
    {

        public static void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                Key thisk;

                DebugLogger.Info("KEY: " + e.KeyCode.ToString());

                // Handle special cases for keys like OemPlus, OemTilde, and OemComma
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
                // Handle numeric keys (D0-D9) explicitly
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
                    // Default case for other keys
                    thisk = (Key)Enum.Parse(typeof(Key), e.KeyCode.ToString());
                }

                // Handle specific actions based on the key
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
                numeric.Value = 0;
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

        public static void ResetForm(Panel panel)
        {
            resetForm(panel);
        }

        public static void ResetForm(GroupBox group)
        {
            resetForm(group);
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
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static EffectStatusIDs ToEffectStatusId(this String val)
        {

            EffectStatusIDs t = Enum.GetValues(typeof(EffectStatusIDs))
                .Cast<EffectStatusIDs>()
                .FirstOrDefault(v => v.GetDescription() == val);
            return t;
        }

        
    }
}
