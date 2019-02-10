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
        private FireKey myKey = null;
        private int renewIntervall = 3000;
        private int queuePosition;
        public Main()
        {
            InitializeComponent();
        }

        private bool toggle = true;
        private void button1_Click(object sender, EventArgs e)
        {
            List<Parameter> testWrite = new List<Parameter>();
            testWrite.Add(new Parameter("1", 30000, "Prüfspannung"));
            testWrite.Add(new Parameter("2", 300, "Prüfzeit"));
            testWrite.Add(new Parameter("3", true, "Abschneidestrecke automatisch einstellen"));
            testWrite.Add(new Parameter("4", false, "Polaritätsvorwahl"));
            testWrite.Add(new Parameter("5", false, "Ladespannung"));

            if (toggle)
            {
                RequestFireKey();
                toggle = !toggle;
            }
            else
            {
                ReleaseKey();
                toggle = !toggle;
            }

           
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
            */
        }

        private void Main_Load(object sender, EventArgs e)
        {
            StartupServer();
             
            //Get Writable Data and write it to DataGrid
            var writeablePara = m_remote.GetWriteAbleParameterList();
            foreach (var parameter in writeablePara)
            {
                dataGridView1.Rows.Add(parameter.ID, parameter.Value, parameter.Beschreibung);
            }

            //Get Readable data and write it to DataGrid
            var readablePara = m_remote.GetReadableParameterList();
            foreach (var parameter in readablePara)
            {
                dgvReadableParameters.Rows.Add(parameter.ID, parameter.Value, parameter.Beschreibung);
            }

            // TODO: Use Actual channel Data
            var actChannelPara = m_remote.GetActChannelPara();


            //Get Commands and Write them to ComboBox
            var commands = m_remote.GetCommands();
            string[] listCommands = new string[commands.Count + 1];
            listCommands[0] = "None";
            listCommands = GetCommandStrings(listCommands, commands);
            cbCommands.Items.AddRange(listCommands);
            cbCommands.SelectedIndex = 0;

            //var devices = m_remote.GetDeviceList();
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvReadableParameters.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private string[] GetCommandStrings(string[] commands, List<Parameter> list)
        {
            int i = 1;
            foreach (var command in list)
            {
                commands[i] = command.Beschreibung;
                i++;
            }
            return commands;
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

        private void RequestFireKey()
        {
            var fireKey = m_remote.GetKey();
            // keynumber has to be positiv
            if (fireKey > 0) 
            {
                // generate Key with 
                this.myKey = new FireKey(fireKey, renewIntervall,true);
                // listening to the event
                this.myKey.KeyExpired += FireKey_Expired;
            }
            
        }
        private void ReleaseKey()
        {
            if (myKey != null)
            {
                this.myKey.KeyExpired -= FireKey_Expired;
                m_remote.ReleaseKey(this.myKey.KeyNumber);
            }
        }
        /// <summary>
        /// MEthod for automaticaly renew FireKey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FireKey_Expired(object sender, EventArgs e)
        {
            // TODO: check queue Position somewhere/ or here and give allert if it is -1 (not in queue)
            var key = (FireKey)sender;
            queuePosition = m_remote.RenewFireKey(key.KeyNumber);
        }

        private void btnRequestFireKey_Click(object sender, EventArgs e)
        {
            //TODO: 
            RequestFireKey();
            bwWaitForConnection.RunWorkerAsync();
            // Start waiting-visualization
            waitingTimer.Start();

        }

        private void waitingTimer_Tick(object sender, EventArgs e)
        {
            if (pbConnection.Value == pbConnection.Maximum)
            {
                pbConnection.Value = 0;
            }
            pbConnection.Increment(1);  
        }

        private void bwWaitForConnection_DoWork(object sender, DoWorkEventArgs e)
        {
            //TODO: zyklische Überprüfung(30s) ob geschrieben werden darf (Mein FireKey ist an der Reihe)
            //      Parameter immer mitschicken!
        }

        private void btnSendSelCommand_Click(object sender, EventArgs e)
        {
            //TODO: Button nur aktiv wenn geschrieben werden darf
            //      Senden des aktuell ausgewählten Commands auf combobox
        }
    }
}
