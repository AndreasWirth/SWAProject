using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IRemoteHandler;
using System.ServiceModel;

namespace Client
{
    public partial class ClientForm : Form
    {
        private IRemoteGetSet m_remote;
        public ClientForm()
        {
            InitializeComponent();

            try
            {
                // connect to the service with channelFactory
                ChannelFactory<IRemoteGetSet> cFactory;
                cFactory = new ChannelFactory<IRemoteGetSet>("WSHttpBinding_ValueManager"); // endpoint
                m_remote = cFactory.CreateChannel();

                //initialize
                if (!m_remote.isInitialized())
                {
                    MessageBox.Show("Initializing the server");
                    m_remote.Initialize(); // doesn't has to be inizialized but do it for practice
                }
                else
                {
                    //MessageBox.Show("Server already initialized");
                }

            }
            catch (Exception e)
            {

                MessageBox.Show("Problem with initialization \n" +e.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnStore_Click(object sender, EventArgs e)
        {
            m_remote.SetValue(tbValue.Text);
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            tbShowValue.Text = m_remote.GetValue();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
