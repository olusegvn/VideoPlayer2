using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoPlayer
{
    public partial class baseForm : Form
    {
        public static Form1 form1;
        public static incognitoForm incognitoform;

        public baseForm()
        {
            InitializeComponent();
            form1 = new Form1();
            form1.Show();
        }

        private void baseForm_Load(object sender, EventArgs e)
        {
        }

        public static void switchIncognito()
        {
            form1.Close();
            incognitoform = new incognitoForm();
            incognitoform.Show();
        }
        public static void switchNormal()
        {
            incognitoform.Close();
            form1 = new Form1();
            form1.Show();
        }
    }
}
