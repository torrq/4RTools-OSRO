using _4RTools.Utils;
using _4RTools.Model;
using System.Windows.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace _4RTools.Forms
{
    public partial class ClientUpdaterForm : Form
    {
        private System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

        public ClientUpdaterForm()
        {
            InitializeComponent();
            StartUpdate();
        }

        private async void StartUpdate()
        {
            List<ClientDTO> clients = new List<ClientDTO>();


            /**
             * Try to load remote supported_server.json file and append all data in clients list.
             */
            try
            {
                clients.AddRange(LocalServerManager.GetLocalClients()); //Load Local Servers First
                LoadServers(clients);                                              //If fetch successfully update and load local file.
                Thread.Sleep(100);
            }
            finally
            {
                new Container().Show();
                Hide();
            }
        }

        private string LoadResourceServerFile()
        {
            return Resources._4RTools.ETCResource.supported_servers;
        }

        private void LoadServers(List<ClientDTO> clients)
        {
            foreach (ClientDTO clientDTO in clients)
            {
                try
                {
                    ClientListSingleton.AddClient(new Client(clientDTO));
                    pbSupportedServer.Increment(1);
                }
                catch { }
                
            }
        }
    }
}
