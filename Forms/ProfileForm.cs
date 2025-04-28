using System;
using System.Linq;
using System.Windows.Forms;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Forms
{
    public partial class ProfileForm : Form
    {
        private Container container;
        public ProfileForm(Container container)
        {
            InitializeComponent();
            this.container = container;

            foreach (string profile in Profile.ListAll())
            {
                if (profile != "Default") { this.lbProfilesList.Items.Add(profile); }
            }

            FormUtils.ApplyColorToButtons(this, new[] { "btnSave" }, AppConfig.CreateButtonBackColor);
            FormUtils.ApplyColorToButtons(this, new[] { "btnRemoveProfile" }, AppConfig.RemoveButtonBackColor);
            FormUtils.ApplyColorToButtons(this, new[] { "btnRenameProfile" }, AppConfig.RenameButtonBackColor);
        }

        private string ValidateProfileName(string profileName, bool isRename = false)
        {
            // Trim leading/trailing spaces
            profileName = profileName?.Trim();

            // Check for empty or whitespace-only names
            if (string.IsNullOrWhiteSpace(profileName))
            {
                MessageBox.Show("Profile name cannot be empty or consist only of whitespace.", "Invalid Profile Name");
                return null;
            }

            // Check maximum length (Windows 7 path limit: 260 characters, accounting for directory and .json extension)
            // Assuming a conservative directory path length (e.g., "C:\Users\<User>\AppData\<App>\Profiles\")
            // Let's assume 100 characters for the directory path, leaving 160 for the file name including ".json"
            const int maxFileNameLength = 160 - 5; // 160 minus ".json" (5 chars) = 155
            if (profileName.Length > maxFileNameLength)
            {
                MessageBox.Show($"Profile name exceeds the maximum length of {maxFileNameLength} characters (accounting for the .json extension and directory path).", "Invalid Profile Name");
                return null;
            }

            // Check for invalid file name characters
            char[] invalidChars = new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
            if (profileName.Any(c => invalidChars.Contains(c)))
            {
                MessageBox.Show($"Profile name cannot contain any of the following characters: {string.Join(" ", invalidChars)}", "Invalid Profile Name");
                return null;
            }

            // Check for control characters (ASCII 0-31)
            if (profileName.Any(c => c < 32))
            {
                MessageBox.Show("Profile name cannot contain control characters (e.g., newlines, tabs).", "Invalid Profile Name");
                return null;
            }

            // Additional checks for rename: ensure the new name isn't the same as the old one
            if (isRename && profileName == this.lbProfilesList.SelectedItem?.ToString())
            {
                return null; // No change, so return null to skip the rename operation
            }

            return profileName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newProfileName = DialogInput.ShowDialog("Enter new profile name:", "Create Profile", "");
            if (newProfileName == null) { return; }

            newProfileName = ValidateProfileName(newProfileName);
            if (newProfileName == null) { return; }

            ProfileSingleton.Create(newProfileName);
            this.lbProfilesList.Items.Add(newProfileName);
            this.container.RefreshProfileList();
        }

        private void btnRemoveProfile_Click(object sender, EventArgs e)
        {
            if (this.lbProfilesList.SelectedItem == null)
            {
                MessageBox.Show("No profile found! To delete a profile, first select an option from the Profile list.");
                return;
            }

            string selectedProfile = this.lbProfilesList.SelectedItem.ToString();
            if (selectedProfile == "Default")
            {
                MessageBox.Show("Cannot delete a Default profile!");
                return;
            }

            bool confirmDelete = DialogConfirmDelete.ShowDialog($"Are you sure you want to delete the profile '{selectedProfile}'?");
            if (!confirmDelete) { return; }

            ProfileSingleton.Delete(selectedProfile);
            this.lbProfilesList.Items.Remove(selectedProfile);
            this.container.RefreshProfileList();
        }

        private void btnRenameProfile_Click(object sender, EventArgs e)
        {
            if (this.lbProfilesList.SelectedItem == null)
            {
                MessageBox.Show("No profile found! To rename a profile, first select an option from the Profile list.");
                return;
            }

            string selectedProfile = this.lbProfilesList.SelectedItem.ToString();
            if (selectedProfile == "Default")
            {
                MessageBox.Show("Cannot rename the Default profile!");
                return;
            }

            string newProfileName = DialogInput.ShowDialog("Enter new profile name:", "Rename Profile", selectedProfile);
            if (newProfileName == null) { return; }

            newProfileName = ValidateProfileName(newProfileName, isRename: true);
            if (newProfileName == null) { return; }

            try
            {
                ProfileSingleton.Rename(selectedProfile, newProfileName);
                int selectedIndex = this.lbProfilesList.SelectedIndex;
                this.lbProfilesList.Items[selectedIndex] = newProfileName;
                this.container.RefreshProfileList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error renaming profile: {ex.Message}");
            }
        }
    }
}