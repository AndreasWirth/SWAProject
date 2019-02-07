using IRemoteHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Main : Form
    {
        private IRemoteGetSet m_remote;
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var writeablePara = m_remote.GetReadableParameterList();
            dataGridView1.DataSource = writeablePara;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            StartupServer();

            //var writeablePara = m_remote.GetWriteAbleParameterList();
            //var readablePara = m_remote.GetReadableParameterList();
            //var actChannelPara = m_remote.GetActChannelPara();
            //var commands = m_remote.GetCommands();
            //var devices = m_remote.GetDeviceList();

            //var fireKey = m_remote.GetKey();

            //dataGridView1.DataSource = writeablePara;
        }

        private void StartupServer()
        {
            try
            {
                // server muss zuvor gestartet werden
                // im bin/debug ordner manuell starten
                // !!!!! WICHTIG!!!! als Administrator ausführen!!!!!!!!
                // connect to the service with channelFactory
                ChannelFactory<IRemoteGetSet> cFactory;
                cFactory = new ChannelFactory<IRemoteGetSet>("WSHttpBinding_HWManager"); // endpoint
                m_remote = cFactory.CreateChannel();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with startup \n" + ex.Message);
            }
        }
    }
}
