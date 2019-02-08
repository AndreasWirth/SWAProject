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
            List<Parameter> testWrite = new List<Parameter>();
            testWrite.Add(new Parameter("1", 30000, "Prüfspannung"));
            testWrite.Add(new Parameter("2", 300, "Prüfzeit"));
            testWrite.Add(new Parameter("3", true, "Abschneidestrecke automatisch einstellen"));
            testWrite.Add(new Parameter("4", false, "Polaritätsvorwahl"));
            testWrite.Add(new Parameter("5", false, "Ladespannung"));

            /*
            var writeablePara = m_remote.GetWriteAbleParameterList();
            var readablePara = m_remote.GetReadableParameterList();
            var actChannelPara = m_remote.GetActChannelPara();
            var commands = m_remote.GetCommands();
            var devices = m_remote.GetDeviceList();
            foreach (var device in devices)
            {
                var available = m_remote.CheckDeviceAvailable(device.ID);
            }
            */
            
            var fireKey = m_remote.GetKey();

            m_remote.RenewFireKey(fireKey);
            m_remote.RenewFireKey(fireKey + 1);

            m_remote.SendCommand("2", fireKey);
            m_remote.SendCommand("2", fireKey + 1);

            m_remote.SetChannelPara(testWrite, fireKey);
            m_remote.SetChannelPara(testWrite, fireKey+1);

            var activ = m_remote.IsFireKeyActiv();


            m_remote.ReleaseKey(fireKey + 1);
            m_remote.ReleaseKey(fireKey);

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
