using _4RTools.Model;
using _4RTools.Resources._4RTools;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _4RTools.Utils
{
    public class NotificationTrayManager
    {
        private readonly ContextMenuStrip contextMenu;
        private readonly ToolStripMenuItem menuItemToggle;
        private readonly ToolStripMenuItem menuItemProfiles; // New "Profiles" menu item
        private readonly ToolStripMenuItem menuItemClose;
        private readonly NotifyIcon notifyIconTray;
        private readonly Subject subject;
        private bool isApplicationOn;

        public NotificationTrayManager(NotifyIcon notifyIconTray, Subject subject, bool initialState = false)
        {
            this.notifyIconTray = notifyIconTray;
            this.subject = subject;
            this.isApplicationOn = initialState;

            contextMenu = new ContextMenuStrip();
            menuItemToggle = new ToolStripMenuItem();
            menuItemProfiles = new ToolStripMenuItem(); // Initialize "Profiles" menu item
            menuItemClose = new ToolStripMenuItem();

            // Configure Toggle menu item (dynamic Start/Stop)
            UpdateToggleMenuItem();
            menuItemToggle.Click += Toggle;

            // Configure Profiles menu item
            menuItemProfiles.Text = "Profiles";
            menuItemProfiles.Image = SystemIcons.WinLogo.ToBitmap(); // Use a generic icon for the Profiles menu
            RefreshProfileMenu(); // Populate the profile list initially

            // Configure Close menu item
            menuItemClose.Text = "Exit " + AppConfig.Name;
            menuItemClose.Image = SystemIcons.Application.ToBitmap();
            menuItemClose.Click += (s, e) => subject.Notify(new Utils.Message(MessageCode.SHUTDOWN_APPLICATION, null));

            contextMenu.Items.AddRange(new ToolStripItem[] { menuItemToggle, menuItemProfiles, menuItemClose });
            this.notifyIconTray.ContextMenuStrip = contextMenu;

            this.notifyIconTray.DoubleClick += (s, e) => subject.Notify(new Utils.Message(MessageCode.CLICK_ICON_TRAY, null));

            UpdateIcon(initialState);
        }

        private void UpdateToggleMenuItem()
        {
            if (isApplicationOn)
            {
                menuItemToggle.Text = "Turn Off";
                menuItemToggle.Image = SystemIcons.Error.ToBitmap();
            }
            else
            {
                menuItemToggle.Text = "Turn On";
                menuItemToggle.Image = SystemIcons.Information.ToBitmap();
            }
        }

        public void UpdateIcon(bool isOn)
        {
            isApplicationOn = isOn;
            notifyIconTray.Icon = isOn ? ETCResource.icon4rtools_on : ETCResource.icon4rtools_off;
            UpdateToggleMenuItem();
        }

        private void Toggle(object sender, EventArgs e)
        {
            if (isApplicationOn)
            {
                subject.Notify(new Utils.Message(MessageCode.TURN_OFF, null));
            }
            else
            {
                subject.Notify(new Utils.Message(MessageCode.TURN_ON, null));
            }
        }

        // Method to refresh the profile list in the tray
        public void RefreshProfileMenu()
        {
            menuItemProfiles.DropDownItems.Clear();

            // Load all profiles and sort them, with "Default" at the top
            var profiles = Profile.ListAll().Select(FormUtils.RestoreInvalidCharacters).ToList();
            if (profiles.Contains("Default"))
            {
                AddProfileMenuItem("Default");
                profiles.Remove("Default");
            }
            foreach (string profile in profiles.OrderBy(profile => profile, StringComparer.OrdinalIgnoreCase))
            {
                AddProfileMenuItem(profile);
            }
        }

        // Helper method to add a profile to the submenu
        private void AddProfileMenuItem(string profileName)
        {
            var profileItem = new ToolStripMenuItem
            {
                Text = profileName,
                Image = SystemIcons.WinLogo.ToBitmap() // Optional: Use a small icon for each profile
            };
            profileItem.Click += (s, e) =>
            {
                // Notify the subject to change the profile
                subject.Notify(new Utils.Message(MessageCode.PROFILE_CHANGED, profileName));
            };
            menuItemProfiles.DropDownItems.Add(profileItem);
        }
    }
}