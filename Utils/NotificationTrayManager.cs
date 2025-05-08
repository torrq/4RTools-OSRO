using _4RTools.Resources._4RTools;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace _4RTools.Utils
{
    public class NotificationTrayManager
    {
        private readonly ContextMenuStrip contextMenu;
        private readonly ToolStripMenuItem menuItemToggle;
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
            menuItemClose = new ToolStripMenuItem();

            // Configure Toggle menu item (dynamic Start/Stop)
            UpdateToggleMenuItem();
            menuItemToggle.Click += Toggle;

            // Configure Close menu item
            menuItemClose.Text = "Exit " + AppConfig.Name;
            menuItemClose.Image = SystemIcons.Application.ToBitmap();
            menuItemClose.Click += (s, e) => subject.Notify(new Utils.Message(MessageCode.SHUTDOWN_APPLICATION, null));

            contextMenu.Items.AddRange(new ToolStripItem[] { menuItemToggle, menuItemClose });
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
    }
}