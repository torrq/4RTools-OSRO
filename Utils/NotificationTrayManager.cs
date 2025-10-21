using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Resources.BruteGaming;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BruteGamingMacros.Core.Utils
{
    public class NotificationTrayManager : IObserver, IDisposable
    {
        private readonly ContextMenuStrip contextMenu;
        private readonly ToolStripMenuItem menuItemToggle;
        private readonly ToolStripMenuItem menuItemProfiles;
        private readonly ToolStripMenuItem menuItemClose;
        private readonly NotifyIcon notifyIconTray;
        private readonly Subject subject;
        private bool isApplicationOn;

        public NotificationTrayManager(Subject subject, bool initialState = false)
        {
            this.subject = subject;
            this.isApplicationOn = initialState;

            // Initialize NotifyIcon
            this.notifyIconTray = new NotifyIcon
            {
                Text = AppConfig.SystemTrayText,
                Icon = initialState ? Icons.systray_on : Icons.systray_off,
                Visible = true
            };

            contextMenu = new ContextMenuStrip
            {
                ImageScalingSize = new Size(16, 16)
            };
            menuItemToggle = new ToolStripMenuItem { ImageScaling = ToolStripItemImageScaling.None };
            menuItemProfiles = new ToolStripMenuItem { ImageScaling = ToolStripItemImageScaling.None };
            menuItemClose = new ToolStripMenuItem { ImageScaling = ToolStripItemImageScaling.None };

            UpdateToggleMenuItem();
            menuItemToggle.Click += Toggle;

            menuItemProfiles.Text = "Profiles";
            menuItemProfiles.Image = Icons.menu_profiles;
            RefreshProfileMenu();

            menuItemClose.Text = "Exit " + AppConfig.Name;
            menuItemClose.Image = Icons.menu_exit;
            menuItemClose.Click += (s, e) => subject.Notify(new Utils.Message(MessageCode.SHUTDOWN_APPLICATION, null));

            contextMenu.Items.AddRange(new ToolStripItem[] { menuItemToggle, menuItemProfiles, menuItemClose });
            this.notifyIconTray.ContextMenuStrip = contextMenu;

            this.notifyIconTray.DoubleClick += (s, e) => subject.Notify(new Utils.Message(MessageCode.CLICK_ICON_TRAY, null));

            this.subject.Attach(this);
        }

        private void UpdateToggleMenuItem()
        {
            if (isApplicationOn)
            {
                menuItemToggle.Text = "Turn Off";
                menuItemToggle.Image = Icons.menu_toggle_off;
            }
            else
            {
                menuItemToggle.Text = "Turn On";
                menuItemToggle.Image = Icons.menu_toggle_on;
            }
        }

        public void UpdateIcon(bool isOn)
        {
            isApplicationOn = isOn;
            notifyIconTray.Icon = isOn ? Icons.systray_on : Icons.systray_off;
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

        public void RefreshProfileMenu()
        {
            menuItemProfiles.DropDownItems.Clear();

            string currentProfile = ProfileSingleton.GetCurrent()?.Name ?? "Default";

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

        private void AddProfileMenuItem(string profileName, string currentProfile)
        {
            var profileItem = new ToolStripMenuItem
            {
                Text = profileName,
                Image = profileName == currentProfile ? Icons.profile_active : Icons.menu_profile,
                ImageScaling = ToolStripItemImageScaling.None
            };
            profileItem.Click += (s, e) =>
            {
                subject.Notify(new Utils.Message(MessageCode.PROFILE_CHANGED, profileName));
            };
            menuItemProfiles.DropDownItems.Add(profileItem);
        }

        public void Update(ISubject subject)
        {
            if (subject is Subject s && s.Message.Code == MessageCode.PROFILE_CHANGED)
            {
                RefreshProfileMenu();
            }
        }

        public void Dispose()
        {
            notifyIconTray?.Dispose();
            contextMenu?.Dispose();
        }
    }
}