using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Linq;
using System.Windows.Forms;

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

            DebugLogger.Info($"Profile list refreshed: {profiles.Count} profiles loaded");
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
                DebugLogger.Warning("Profile name validation failed: Empty or whitespace-only name");
                return null;
            }

            // Check maximum length (Windows 7 path limit: 260 characters, accounting for directory and .json extension)
            const int maxFileNameLength = 160 - 5; // 160 minus ".json" (5 chars) = 155
            if (profileName.Length > maxFileNameLength)
            {
                MessageBox.Show($"Profile name exceeds the maximum length of {maxFileNameLength} characters (accounting for the .json extension and directory path).", "Invalid Profile Name");
                DebugLogger.Warning($"Profile name validation failed: Name exceeds max length ({profileName.Length} > {maxFileNameLength})");
                return null;
            }

            // Check for invalid file name characters
            char[] invalidChars = new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
            if (profileName.Any(c => invalidChars.Contains(c)))
            {
                MessageBox.Show($"Profile name cannot contain any of the following characters: {string.Join(" ", invalidChars)}", "Invalid Profile Name");
                DebugLogger.Warning($"Profile name validation failed: Contains invalid characters: {profileName}");
                return null;
            }

            // Check for control characters (ASCII 0-31)
            if (profileName.Any(c => c < 32))
            {
                MessageBox.Show("Profile name cannot contain control characters (e.g., newlines, tabs).", "Invalid Profile Name");
                DebugLogger.Warning("Profile name validation failed: Contains control characters");
                return null;
            }

            // Additional checks for rename: ensure the new name isn't the same as the old one
            if (isRename && profileName == this.lbProfilesList.SelectedItem?.ToString())
            {
                DebugLogger.Debug($"Profile rename skipped: New name is the same as old name ({profileName})");
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

            DebugLogger.Debug($"Generated unique profile name: {newName} (from base: {baseName})");
            return newName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newProfileName = DialogInput.ShowDialog("Enter new profile name:", "Create Profile", "");
            if (newProfileName == null)
            {
                DebugLogger.Debug("Create profile operation canceled by user");
                return;
            }

            newProfileName = ValidateProfileName(newProfileName);
            if (newProfileName == null) { return; }

            try
            {
                ProfileSingleton.Create(newProfileName);
                RefreshProfileList(); // Refresh and sort the list
                this.container.RefreshProfileList();
                UpdateStatus($"Profile '{newProfileName}' created successfully");
                DebugLogger.Info($"New profile created: '{newProfileName}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating profile: {ex.Message}");
                UpdateStatus($"Error creating profile: {ex.Message}");
                DebugLogger.Error(ex, $"Failed to create profile '{newProfileName}'");
            }
        }

        private void btnCopyProfile_Click(object sender, EventArgs e)
        {
            if (this.lbProfilesList.SelectedItem == null)
            {
                MessageBox.Show("No profile found! To copy a profile, first select an option from the Profile list.");
                DebugLogger.Warning("Copy profile operation failed: No profile selected");
                return;
            }

            string selectedProfile = this.lbProfilesList.SelectedItem.ToString();
            if (selectedProfile == "Default")
            {
                MessageBox.Show("Cannot copy the Default profile!");
                DebugLogger.Warning("Copy profile operation failed: Cannot copy Default profile");
                return;
            }

            // Generate a unique name for the copied profile
            string suggestedName = GenerateUniqueProfileName(selectedProfile);
            string newProfileName = DialogInput.ShowDialog("Enter name for the copied profile:", "Copy Profile", suggestedName);
            if (newProfileName == null)
            {
                DebugLogger.Debug("Copy profile operation canceled by user");
                return;
            }

            newProfileName = ValidateProfileName(newProfileName);
            if (newProfileName == null) { return; }

            try
            {
                ProfileSingleton.Copy(selectedProfile, newProfileName);
                RefreshProfileList(); // Refresh and sort the list
                this.container.RefreshProfileList();
                UpdateStatus($"Profile '{selectedProfile}' copied to '{newProfileName}' successfully");
                DebugLogger.Info($"Profile copied: '{selectedProfile}' to '{newProfileName}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying profile: {ex.Message}");
                UpdateStatus($"Error copying profile: {ex.Message}");
                DebugLogger.Error(ex, $"Failed to copy profile '{selectedProfile}' to '{newProfileName}'");
            }
        }

        private void btnRemoveProfile_Click(object sender, EventArgs e)
        {
            if (this.lbProfilesList.SelectedItem == null)
            {
                MessageBox.Show("No profile found! To delete a profile, first select an option from the Profile list.");
                DebugLogger.Warning("Delete profile operation failed: No profile selected");
                return;
            }

            string selectedProfile = this.lbProfilesList.SelectedItem.ToString();
            if (selectedProfile == "Default")
            {
                MessageBox.Show("Cannot delete a Default profile!");
                DebugLogger.Warning("Delete profile operation failed: Cannot delete Default profile");
                return;
            }

            bool confirmDelete = DialogConfirmDelete.ShowDialog($"Are you sure you want to delete the profile '{selectedProfile}'?");
            if (!confirmDelete)
            {
                DebugLogger.Debug($"Delete profile operation canceled by user for '{selectedProfile}'");
                return;
            }

            try
            {
                ProfileSingleton.Delete(selectedProfile);
                RefreshProfileList(); // Refresh and sort the list
                this.container.RefreshProfileList();
                UpdateStatus($"Profile '{selectedProfile}' deleted successfully");
                DebugLogger.Info($"Profile deleted: '{selectedProfile}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting profile: {ex.Message}");
                UpdateStatus($"Error deleting profile: {ex.Message}");
                DebugLogger.Error(ex, $"Failed to delete profile '{selectedProfile}'");
            }
        }

        private void btnRenameProfile_Click(object sender, EventArgs e)
        {
            if (this.lbProfilesList.SelectedItem == null)
            {
                MessageBox.Show("No profile found! To rename a profile, first select an option from the Profile list.");
                DebugLogger.Warning("Rename profile operation failed: No profile selected");
                return;
            }

            string selectedProfile = this.lbProfilesList.SelectedItem.ToString();
            if (selectedProfile == "Default")
            {
                MessageBox.Show("Cannot rename the Default profile!");
                DebugLogger.Warning("Rename profile operation failed: Cannot rename Default profile");
                return;
            }

            string newProfileName = DialogInput.ShowDialog("Enter new profile name:", "Rename Profile", selectedProfile);
            if (newProfileName == null)
            {
                DebugLogger.Debug($"Rename profile operation canceled by user for '{selectedProfile}'");
                return;
            }

            newProfileName = ValidateProfileName(newProfileName, isRename: true);
            if (newProfileName == null) { return; }

            try
            {
                ProfileSingleton.Rename(selectedProfile, newProfileName);
                RefreshProfileList(); // Refresh and sort the list
                this.container.RefreshProfileList();
                UpdateStatus($"Profile '{selectedProfile}' renamed to '{newProfileName}' successfully");
                DebugLogger.Info($"Profile renamed: '{selectedProfile}' to '{newProfileName}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error renaming profile: {ex.Message}");
                UpdateStatus($"Error renaming profile: {ex.Message}");
                DebugLogger.Error(ex, $"Failed to rename profile '{selectedProfile}' to '{newProfileName}'");
            }
        }
    }
}