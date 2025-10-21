using BruteGamingMacros.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.Core.Model
{
    public class TransferHelper : IAction
    {
        public static string ACTION_NAME_TRANSFER = "TransferHelper";

        public string ActionName { get; set; } = ACTION_NAME_TRANSFER;

        private ThreadRunner thread;

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public Key TransferKey { get; set; } = Key.None;

        public string GetActionName()
        {
            return ACTION_NAME_TRANSFER;
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        private int TransferExecutionThread(Client roClient)
        {
            var transferKey = ProfileSingleton.GetCurrent().TransferHelper.TransferKey;
            if (transferKey != Key.None && Keyboard.IsKeyDown(transferKey))
            {
                AHKTransferBoost(roClient, new KeyConfig(transferKey, true), (Keys)Enum.Parse(typeof(Keys), transferKey.ToString()));
                return 0;
            }
            Thread.Sleep(100);
            return 0;
        }

        private void AHKTransferBoost(Client roClient, KeyConfig config, Keys thisk)
        {
            Func<int, int> send_click = (evt) =>
            {
                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_RBUTTONDOWN, 0, 0);
                Thread.Sleep(1);
                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_RBUTTONUP, 0, 0);
                return 0;
            };

            keybd_event(Constants.VK_LMENU, 0xA4, Constants.KEYEVENTF_EXTENDEDKEY, 0);

            while (Keyboard.IsKeyDown(config.Key))
            {
                send_click(0);
                Thread.Sleep(10);
            }
            keybd_event(Constants.VK_LMENU, 0xA4, Constants.KEYEVENTF_EXTENDEDKEY | Constants.KEYEVENTF_KEYUP, 0);
        }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (this.thread != null)
                {
                    ThreadRunner.Stop(this.thread);
                }
                this.thread = new ThreadRunner((_) => TransferExecutionThread(roClient));
                ThreadRunner.Start(this.thread);
            }
        }

        public void Stop()
        {
            if (this.thread != null)
            {
                ThreadRunner.Stop(this.thread);
                this.thread.Terminate();
                this.thread = null;
            }
        }
    }
}