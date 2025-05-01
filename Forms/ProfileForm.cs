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

            RefreshProfileList(); // Initial load with sorting

            FormUtils.ApplyColorToButtons(this, new[] { "btnSave" }, AppConfig.CreateButtonBackColor);
            FormUtils.ApplyColorToButtons(this, new[] { "btnCopyProfile" }, AppConfig.CopyButtonBackColor);
            FormUtils.ApplyColorToButtons(this, new[] { "btnRemoveProfile" }, AppConfig.RemoveButtonBackColor);
            FormUtils.ApplyColorToButtons(this, new[] { "btnRenameProfile" }, AppConfig.RenameButtonBackColor);

            // Set initial status message
            UpdateStatus("Ready");
        }

        // Helper method to refresh and sort the profile list
        private void RefreshProfileList()
        {
            // Store the currently selected item (if any)
            string selectedProfile = this.lbProfilesList.SelectedItem?.ToString();

            // Clear the current list
            this.lbProfilesList.Items.Clear();

            // Reload all profiles and sort them
            var profiles = Profile.ListAll()
                .Where(profile => profile != "Default")
                .OrderBy(profile => profile, StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (string profile in profiles)
            {
                this.lbProfilesList.Items.Add(profile);
            }

            // Restore the selection if the profile still exists
            if (selectedProfile != null && profiles.Contains(selectedProfile))
            {
                this.lbProfilesList.SelectedItem = selectedProfile;
            }
        }

        // Helper method to update the status bar message
        private void UpdateStatus(string message)
        {
            statusLabel.Text = message;
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

        private string GenerateUniqueProfileName(string baseName)
        {
            // If the base name doesn't exist, use it directly
            if (!Profile.ListAll().Contains(baseName))
            {
                return baseName;
            }

            // Try appending " (number)" until a unique name is found
            int counter = 1;
            string newName;
            do
            {
                newName = $"{baseName} ({counter})";
                counter++;
            } while (Profile.ListAll().Contains(newName));

            return newName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newProfileName = DialogInput.ShowDialog("Enter new profile name:", "Create Profile", "");
            if (newProfileName == null) { return; }

            newProfileName = ValidateProfileName(newProfileName);
            if (newProfileName == null) { return; }

            ProfileSingleton.Create(newProfileName);
            RefreshProfileList(); // Refresh and sort the list
            this.container.RefreshProfileList();
            UpdateStatus($"Profile '{newProfileName}' created successfully");
        }

        private void btnCopyProfile_Click(object sender, EventArgs e)
        {
            if (this.lbProfilesList.SelectedItem == null)
            {
                MessageBox.Show("No profile found! To copy a profile, first select an option from the Profile list.");
                return;
            }

            string selectedProfile = this.lbProfilesList.SelectedItem.ToString();
            if (selectedProfile == "Default")
            {
                MessageBox.Show("Cannot copy the Default profile!");
                return;
            }

            // Generate a unique name for the copied profile
            string suggestedName = GenerateUniqueProfileName(selectedProfile);
            string newProfileName = DialogInput.ShowDialog("Enter name for the copied profile:", "Copy Profile", suggestedName);
            if (newProfileName == null) { return; }

            newProfileName = ValidateProfileName(newProfileName);
            if (newProfileName == null) { return; }

            try
            {
                ProfileSingleton.Copy(selectedProfile, newProfileName);
                RefreshProfileList(); // Refresh and sort the list
                this.container.RefreshProfileList();
                UpdateStatus($"Profile '{selectedProfile}' copied to '{newProfileName}' successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying profile: {ex.Message}");
                UpdateStatus($"Error copying profile: {ex.Message}");
            }
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
            RefreshProfileList(); // Refresh and sort the list
            this.container.RefreshProfileList();
            UpdateStatus($"Profile '{selectedProfile}' deleted successfully");
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
                RefreshProfileList(); // Refresh and sort the list
                this.container.RefreshProfileList();
                UpdateStatus($"Profile '{selectedProfile}' renamed to '{newProfileName}' successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error renaming profile: {ex.Message}");
                UpdateStatus($"Error renaming profile: {ex.Message}");
            }
        }
    }
}