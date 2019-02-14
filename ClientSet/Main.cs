using IRemoteHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Main : Form
    {
        const string PathSavedData = @"..\..\SavedData\";
        private IRemoteGetSet m_remote;
        private FireKey myKey = null;
        private int renewIntervall = 3000;
        private int queuePosition;
        private List<Parameter> commands;
        private List<Parameter> devices;
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
            if (StartupServer())
            {
                tbOutputWindow.Text = "Welcome. You are sucessfully connected to the server!";
                btnReleaseFireKey.Enabled = false;
                btnSendSelCommand.Enabled = false;
                btnCheckAvailability.Enabled = false;

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
                // var actChannelPara = m_remote.GetActChannelPara();


                //Get Commands and Write them to ComboBox
                commands = m_remote.GetCommands();
                string[] listCommands = new string[commands.Count + 1];
                listCommands[0] = "None";
                listCommands = GetStrings(listCommands, commands);
                cbCommands.Items.AddRange(listCommands);
                cbCommands.SelectedIndex = 0;

                // Get Devices and write them to ComboBox
                devices = m_remote.GetDeviceList();
                string[] listDevices = new string[devices.Count + 1];
                listDevices[0] = "None";
                listDevices = GetStrings(listDevices, devices);
                cbCheckDeviceAvailable.Items.AddRange(listDevices);
                cbCheckDeviceAvailable.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Server not found");
                btnReleaseFireKey.Enabled = false;
                btnSendSelCommand.Enabled = false;
                btnCheckAvailability.Enabled = false;
                btnSaveToCsv.Enabled = false;
                btnRequestFireKey.Enabled = false;
                tbOutputWindow.Text = "connection problem";
            }
        }

        private string[] GetStrings(string[] commands, List<Parameter> list)
        {
            int i = 1;
            foreach (var command in list)
            {
                commands[i] = command.Beschreibung;
                i++;
            }
            return commands;
        }

        private bool StartupServer()
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
                return TestConnection(m_remote); ;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem with startup \n" + ex.Message);
                return false;
            }
        }

        private bool TestConnection(IRemoteGetSet conection)
        {
            try
            {
                //test connection
                conection.IsFireKeyActiv();
                return true;
            }
            catch (Exception)
            {
                return false;
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

        private void DeleateFireKey()
        {
            this.myKey.KeyExpired -= FireKey_Expired;
            this.myKey = null;
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
            var key = (FireKey)sender;
            queuePosition = m_remote.RenewFireKey(key.KeyNumber);
            // Delete Fire Key if it is not in queue
            if (queuePosition == -1)
            {
                // Use invoke for multithreading
                DeleateFireKey();
                Invoke(new MethodInvoker(() =>
                {
                    btnSendSelCommand.Enabled = false;
                    btnCheckAvailability.Enabled = false;
                    tbOutputWindow.Text = "Fire Key was deleted on the server and must therefore be requested again";
                }));
            }
            // Commands können geschrieben werden
            else if(queuePosition == 0)
            {
                // send actual set channel Data
                if(m_remote.SetChannelPara(GetWriteableList(), myKey.KeyNumber))
                {
                    // Use invoke for multithreading
                    Invoke(new MethodInvoker(() =>
                    {
                        tbOutputWindow.Text = "Actual Channel Parameters have been transferred to the server. You are connected to the test bench!";
                    }));
                }
                else
                {
                    // TODO: Hier muss man möglicherweise erneut senden und abprüfen!
                    // dürfte nicht eintretten da dieser fehler nur bei einem falschen key auftritt
                    Invoke(new MethodInvoker(() =>
                    {
                        tbOutputWindow.Text = "Actual Channel Parameters have not been transferred to the server.";
                    }));
                    
                }
                // Use invoke for multithreading
                Invoke(new MethodInvoker(() =>
                {
                    btnSendSelCommand.Enabled = true;
                    btnCheckAvailability.Enabled = true;
                }));
            }
            else
            {
                // Use invoke for multithreading
                Invoke(new MethodInvoker(() =>
                {
                    btnSendSelCommand.Enabled = false;
                    btnCheckAvailability.Enabled = false;
                    tbOutputWindow.Text = "You are currently on place " + queuePosition.ToString() + " in the waiting list";
                }));
            }
        }
        private void btnRequestFireKey_Click(object sender, EventArgs e)
        { 
            RequestFireKey();
            if(myKey != null)
            {
                tbOutputWindow.Text = "Fire Key was sucessfully requested";
                btnReleaseFireKey.Enabled = true;
                btnRequestFireKey.Enabled = false;
            }
            else
            {
                tbOutputWindow.Text = "Fire Key was not assigned by the server";
                MessageBox.Show("Fire Key was not assigned by the server");
            }
        }

        private void btnSendSelCommand_Click(object sender, EventArgs e)
        {
            if (cbCommands.SelectedItem.ToString().CompareTo("None") == 0)
            {
                tbOutputWindow.Text = "Please select a Command from the list";
            }
            else
            {
                var command = GetCommandObject(cbCommands.SelectedItem.ToString());
                var resp = m_remote.SendCommand(command.ID, myKey.KeyNumber);
                tbOutputWindow.Text = resp;
            }   
        }

        private Parameter GetCommandObject(string value)
        {
            foreach (var command in commands)
            {
                if(command.Beschreibung.Equals(value))
                {
                    return command;
                }
            }
            return null;
        }

        private Parameter GetDeviceObject(string value)
        {
            foreach (var device in devices)
            {
                if (device.Beschreibung.Equals(value))
                {
                    return device;
                }
            }
            return null;
        }

        private void btnReleaseFireKey_Click(object sender, EventArgs e)
        {
            // Release Fire Key if Button pressed
            if (m_remote.ReleaseKey(myKey.KeyNumber))
            {
                DeleateFireKey();
                tbOutputWindow.Text = "Fire Key has been released";
                if (myKey == null) {
                    btnReleaseFireKey.Enabled = false;
                    btnRequestFireKey.Enabled = true;
                    btnSendSelCommand.Enabled = false;
                    btnCheckAvailability.Enabled = false;
                }
            }
            else
            {
                tbOutputWindow.Text = "Fire Key could not be released";
                MessageBox.Show("Fire Key could not be released");
            }
        }

        private void btnUpdateAcPara_Click(object sender, EventArgs e)
        {
            //Update Readable Parameter
            var actChannelPara = m_remote.GetActChannelPara();
            var rows = dgvReadableParameters.RowCount;
            var i = 0;
            foreach (var parameter in actChannelPara)
            {
                if (i < rows) { dgvReadableParameters.Rows[i].SetValues(parameter.ID, parameter.Value, parameter.Beschreibung); }
                else { dgvReadableParameters.Rows.Add(parameter.ID, parameter.Value, parameter.Beschreibung);}
                i++;
            }
            tbOutputWindow.Text = "Readable Parameters have been successfully updated!";
        }

        private List<Parameter> GetWriteableList()
        {
            string id; int value; string description;
            List<Parameter> list = new List<Parameter>();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                value = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
                description = dataGridView1.Rows[i].Cells[2].Value.ToString();
                list.Add(new Parameter(id, value, description));  
            }
            return list;
        }

        private void btnCheckAvailability_Click(object sender, EventArgs e)
        {
            if (cbCheckDeviceAvailable.SelectedItem.ToString().CompareTo("None") == 0)
            {
                tbOutputWindow.Text = "Please select a Device from the list";
            }
            else
            {
                var device = GetDeviceObject(cbCheckDeviceAvailable.SelectedItem.ToString());
                if (m_remote.CheckDeviceAvailable(device.ID))
                {
                    tbOutputWindow.Text = "Selected device is available!";
                }
                else { tbOutputWindow.Text = "Selected device is not available!"; }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //TODO: Check Changed Parameter and rechange it if necessary
        }

        private void btnSaveToCsv_Click(object sender, EventArgs e)
        {
            SaveToCSV();
        }

        private void SaveToCSV()
        {
            string filename = "";
            SaveFileDialog sfd = new SaveFileDialog();
            string combinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), PathSavedData);
            sfd.InitialDirectory = System.IO.Path.GetFullPath(combinedPath);
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = "Output.csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                tbOutputWindow.Text = "Data will be exported and you will be notified when it is ready.";
                if (File.Exists(filename))
                {
                    try
                    {
                        File.Delete(filename);
                    }
                    catch (IOException ex)
                    {
                        tbOutputWindow.Text = "It wasn't possible to write the data to the disk." + ex.Message;
                    }
                }
                int columnCount = dgvReadableParameters.ColumnCount;
                string columnNames = "";
                string[] output = new string[dgvReadableParameters.RowCount + dataGridView1.RowCount + 2];
                output[0] = "Readable Parameters";
                for (int i = 0; i < columnCount; i++)
                {
                    columnNames += dgvReadableParameters.Columns[i].Name.ToString() + ";";
                }
                output[1] += columnNames;
                for (int i = 2; i  < output.Length; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        if(i< dgvReadableParameters.RowCount)
                        {
                            output[i] += dgvReadableParameters.Rows[i - 1].Cells[j].Value.ToString() + ";";
                        }
                        else if(i == dgvReadableParameters.RowCount) { /*Space*/}
                        else if(i == dgvReadableParameters.RowCount+1) { output[i] = "Writeable Parameters"; }
                        else
                        {
                            output[i] += dataGridView1.Rows[i - dgvReadableParameters.RowCount - 2].Cells[j].Value.ToString() + ";";
                        }
                        
                    }
                }

                System.IO.File.WriteAllLines(sfd.FileName, output, System.Text.Encoding.UTF8);
                tbOutputWindow.Text = "Your file was generated and its ready for use.";
            }
        }
    }
}
