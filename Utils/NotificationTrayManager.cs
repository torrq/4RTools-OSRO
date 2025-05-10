using _4RTools.Model;
using _4RTools.Resources._4RTools;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _4RTools.Utils
{
    public class NotificationTrayManager : IObserver
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

            contextMenu = new ContextMenuStrip
            {
                ImageScalingSize = new Size(16, 16) // Ensure 16x16 icons are not scaled
            };
            menuItemToggle = new ToolStripMenuItem
            {
                ImageScaling = ToolStripItemImageScaling.None // Prevent individual scaling
            };
            menuItemProfiles = new ToolStripMenuItem
            {
                ImageScaling = ToolStripItemImageScaling.None // Prevent individual scaling
            };
            menuItemClose = new ToolStripMenuItem
            {
                ImageScaling = ToolStripItemImageScaling.None // Prevent individual scaling
            };

            // Configure Toggle menu item (dynamic Start/Stop)
            UpdateToggleMenuItem();
            menuItemToggle.Click += Toggle;

            // Configure Profiles menu item
            menuItemProfiles.Text = "Profiles";
            menuItemProfiles.Image = global::_4RTools.Resources._4RTools.Icons.menu_profiles; // Use new menu_profiles icon
            RefreshProfileMenu(); // Populate the profile list initially

            // Configure Close menu item
            menuItemClose.Text = "Exit " + AppConfig.Name;
            menuItemClose.Image = global::_4RTools.Resources._4RTools.Icons.menu_exit; // Use new menu_exit icon
            menuItemClose.Click += (s, e) => subject.Notify(new Utils.Message(MessageCode.SHUTDOWN_APPLICATION, null));

            contextMenu.Items.AddRange(new ToolStripItem[] { menuItemToggle, menuItemProfiles, menuItemClose });
            this.notifyIconTray.ContextMenuStrip = contextMenu;

            this.notifyIconTray.DoubleClick += (s, e) => subject.Notify(new Utils.Message(MessageCode.CLICK_ICON_TRAY, null));

            UpdateIcon(initialState);

            // Attach this class as an observer to the subject
            this.subject.Attach(this);
        }

        private void UpdateToggleMenuItem()
        {
            if (isApplicationOn)
            {
                menuItemToggle.Text = "Turn Off";
                menuItemToggle.Image = global::_4RTools.Resources._4RTools.Icons.menu_toggle_off; // Use new menu_toggle_off icon
            }
            else
            {
                menuItemToggle.Text = "Turn On";
                menuItemToggle.Image = global::_4RTools.Resources._4RTools.Icons.menu_toggle_on; // Use new menu_toggle_on icon
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

            // Get the current profile name
            string currentProfile = ProfileSingleton.GetCurrent()?.Name ?? "Default";

            // Load all profiles and sort them, with "Default" at the top
            var profiles = Profile.ListAll().ToList();
            if (profiles.Contains("Default"))
            {
                AddProfileMenuItem("Default", currentProfile);
                profiles.Remove("Default");
            }
            foreach (string profile in profiles.OrderBy(profile => profile, StringComparer.OrdinalIgnoreCase))
            {
                AddProfileMenuItem(profile, currentProfile);
            }
        }

        // Helper method to add a profile to the submenu
        private void AddProfileMenuItem(string profileName, string currentProfile)
        {
            var profileItem = new ToolStripMenuItem
            {
                Text = profileName,
                Image = profileName == currentProfile
                    ? global::_4RTools.Resources._4RTools.Icons.profile_active // Use profile_active for current profile
                    : global::_4RTools.Resources._4RTools.Icons.menu_profile, // Use menu_profile for others
                ImageScaling = ToolStripItemImageScaling.None // Prevent individual scaling
            };
            profileItem.Click += (s, e) =>
            {
                // Notify the subject to change the profile
                subject.Notify(new Utils.Message(MessageCode.PROFILE_CHANGED, profileName));
            };
            menuItemProfiles.DropDownItems.Add(profileItem);
        }

        // IObserver implementation to handle profile change notifications
        public void Update(ISubject subject)
        {
            if (subject is Subject s && s.Message.Code == MessageCode.PROFILE_CHANGED)
            {
                RefreshProfileMenu(); // Update menu to reflect new active profile
            }
        }
    }
}