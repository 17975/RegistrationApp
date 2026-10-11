using System;
using System.Windows.Forms;

namespace RegistrationApp
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Close registration and return to login
            this.Close();
        }
    }
}
