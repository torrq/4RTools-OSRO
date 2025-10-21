using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Core.Utils;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace BruteGamingMacros.UI.Forms
{
    public partial class ProfileForm : Form
    {
        private Container container;
        private Font italicFont; // Font for italic "Default" entry
        private Font regularFont; // Font for other entries
        private float iconY = -1; // Y-coordinate of the icon; -1 means don't draw
        private readonly int iconX = 105; // X-coordinate to the left of the ListBox (125 - 20)
        private readonly int iconSize = 16; // Icon size (16x16 pixels)

        public ProfileForm(Container container)
        {
            InitializeComponent();
            this.container = container;

            // Initialize fonts for custom drawing
            this.regularFont = this.lbProfilesList.Font;
            this.italicFont = new Font(this.regularFont, FontStyle.Italic);

            // Configure ListBox for custom drawing
            this.lbProfilesList.DrawMode = DrawMode.OwnerDrawFixed;
            this.lbProfilesList.DrawItem += new DrawItemEventHandler(this.lbProfilesList_DrawItem);
            this.lbProfilesList.MouseDoubleClick += new MouseEventHandler(this.lbProfilesList_MouseDoubleClick);
            this.lbProfilesList.SelectedIndexChanged += new EventHandler(this.lbProfilesList_SelectedIndexChanged);

            RefreshProfileList(); // Initial load with sorting
            UpdateIconPosition(); // Ensure icon is drawn on launch

            FormUtils.ApplyColorToButtons(this, new[] { "btnSave" }, AppConfig.CreateButtonBackColor);
            FormUtils.ApplyColorToButtons(this, new[] { "btnCopyProfile" }, AppConfig.CopyButtonBackColor);
            FormUtils.ApplyColorToButtons(this, new[] { "btnRemoveProfile" }, AppConfig.RemoveButtonBackColor);
            FormUtils.ApplyColorToButtons(this, new[] { "btnRenameProfile" }, AppConfig.RenameButtonBackColor);

            // Set initial status message
            UpdateStatus("Ready");
        }

        // Custom drawing for ListBox items (no icon drawing here anymore)
        private void lbProfilesList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return; // No items to draw

            // Get the item text
            string itemText = this.lbProfilesList.Items[e.Index].ToString();

            // Choose font based on whether the item is "Default"
            Font font = itemText == "Default" ? this.italicFont : this.regularFont;

            // Determine the background and foreground colors
            Brush backgroundBrush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                ? SystemBrushes.Highlight
                : SystemBrushes.Window;
            Brush foregroundBrush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                ? SystemBrushes.HighlightText
                : SystemBrushes.WindowText;

            // Draw the background
            e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

            // Draw the text
            e.Graphics.DrawString(itemText, font, foregroundBrush, e.Bounds.Left, e.Bounds.Top);

            // Draw the focus rectangle if the item has focus
            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
            {
                e.DrawFocusRectangle();
            }
        }

        // Update icon position when selection changes (for visual feedback, if needed)
        private void lbProfilesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateIconPosition();
        }

        // Override OnPaint to draw the icon on the form
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (iconY >= 0) // Only draw if iconY is set (i.e., there’s an active profile)
            {
                Image icon = global::BruteGamingMacros.Resources.BruteGaming.Icons.profile_active;
                e.Graphics.DrawImage(icon, iconX, iconY, iconSize, iconSize);
            }
        }

        // Helper method to update the icon’s position based on the active profile
        private void UpdateIconPosition()
        {
            string currentProfile = ProfileSingleton.GetCurrent().Name;
            int activeIndex = this.lbProfilesList.Items.IndexOf(currentProfile);

            if (activeIndex >= 0)
            {
                // Calculate the y-coordinate of the active profile’s row
                int itemHeight = this.lbProfilesList.ItemHeight;
                int listBoxTop = this.lbProfilesList.Location.Y; // 33, from Designer
                iconY = listBoxTop + (activeIndex * itemHeight) + (itemHeight - iconSize) / 2; // Center vertically in the row
            }
            else
            {
                iconY = -1; // Don’t draw the icon if the active profile isn’t in the list
            }

            this.Invalidate(); // Force a repaint to update the icon position
        }

        // Helper method to refresh and sort the profile list
        public void RefreshProfileList()
        {
            // Clear the current list
            this.lbProfilesList.Items.Clear();

            // Reload all profiles and sort them, with "Default" at the top
            var profiles = Profile.ListAll().ToList();
            if (profiles.Contains("Default"))
            {
                this.lbProfilesList.Items.Add("Default");
                profiles.Remove("Default");
            }
            foreach (string profile in profiles.OrderBy(profile => profile, StringComparer.OrdinalIgnoreCase))
            {
                this.lbProfilesList.Items.Add(profile);
            }

            // Set the selected item to the current profile
            string currentProfile = ProfileSingleton.GetCurrent().Name;
            if (profiles.Contains(currentProfile))
            {
                // Temporarily clear the selection to force SelectedIndexChanged to fire
                this.lbProfilesList.SelectedItem = null;
                this.lbProfilesList.SelectedItem = currentProfile;
            }
            else if (this.lbProfilesList.Items.Count > 0)
            {
                this.lbProfilesList.SelectedIndex = 0;
            }

            //DebugLogger.Info($"Profile list refreshed: {this.lbProfilesList.Items.Count} profiles loaded");

            UpdateIconPosition();
        }

        // New method to update the profile icon for a given profile name
        public void UpdateProfileIcon(string profileName)
        {
            // Ensure the profile list is up-to-date
            RefreshProfileList();

            // Force the form to repaint immediately
            this.Refresh();
        }

        // Load profile on double click
        private void lbProfilesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.lbProfilesList.SelectedItem != null)
            {
                string selectedProfile = this.lbProfilesList.SelectedItem.ToString();
                try
                {
                    this.container.LoadProfile(selectedProfile);
                    UpdateStatus($"Profile '{selectedProfile}' loaded successfully");
                    RefreshProfileList(); // Refresh to ensure the active profile icon updates
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading profile: {ex.Message}");
                    UpdateStatus($"Error loading profile: {ex.Message}");
                    DebugLogger.Error(ex, $"Failed to load profile '{selectedProfile}'");
                }
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
                DebugLogger.Warning("Profile name validation failed: Empty or whitespace-only name");
                return null;
            }

            // Replace invalid characters with numeric HTML entities
            string originalName = profileName;

            // Check maximum length (Windows 7 path limit: 260 characters, accounting for directory and .json extension)
            const int maxFileNameLength = 160 - 5; // 160 minus ".json" (5 chars) = 155
            if (profileName.Length > maxFileNameLength)
            {
                MessageBox.Show($"Profile name exceeds the maximum length of {maxFileNameLength} characters after substitution (accounting for the .json extension and directory path). Original: '{originalName}', Substituted: '{profileName}'", "Invalid Profile Name");
                DebugLogger.Warning($"Profile name validation failed: Name exceeds max length after substitution ({profileName.Length} > {maxFileNameLength})");
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

            string newProfileNameLog = newProfileName;
            newProfileName = ValidateProfileName(newProfileName);
            if (newProfileName == null) { return; }

            try
            {
                ProfileSingleton.Create(newProfileName);
                RefreshProfileList(); // Refresh and sort the list
                this.container.RefreshProfileList();
                this.container.LoadProfile(newProfileName); // Load the new profile immediately
                UpdateStatus($"Profile '{newProfileNameLog}' created and loaded successfully");
                DebugLogger.Info($"New profile created and loaded: '{newProfileNameLog}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating profile: {ex.Message}");
                UpdateStatus($"Error creating profile: {ex.Message}");
                DebugLogger.Error(ex, $"Failed to create profile '{newProfileNameLog}'");
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
                MessageBox.Show("Cannot delete the Default profile!");
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
                this.container.LoadProfile("Default"); // Load Default profile after deletion
                UpdateStatus($"Profile '{selectedProfile}' deleted, switched to 'Default' successfully");
                DebugLogger.Info($"Profile deleted and switched to Default: '{selectedProfile}'");
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

            string newProfileNameLog = newProfileName;
            newProfileName = ValidateProfileName(newProfileName, isRename: true);
            if (newProfileName == null) { return; }

            try
            {
                ProfileSingleton.Rename(selectedProfile, newProfileName);
                RefreshProfileList(); // Refresh and sort the list
                this.container.RefreshProfileList();
                UpdateStatus($"Profile '{selectedProfile}' renamed to '{newProfileNameLog}' successfully");
                DebugLogger.Info($"Profile renamed: '{selectedProfile}' to '{newProfileNameLog}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error renaming profile: {ex.Message}");
                UpdateStatus($"Error renaming profile: {ex.Message}");
                DebugLogger.Error(ex, $"Failed to rename profile '{selectedProfile}' to '{newProfileNameLog}'");
            }
        }
    }
}