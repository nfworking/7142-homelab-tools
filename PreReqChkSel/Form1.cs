using System.Diagnostics;

namespace PreReqChkSel
{
    public partial class MainApp : Form
    {
        public MainApp()
        {
            InitializeComponent();
        }

        private void moeBut_Click(object sender, EventArgs e)
        {
            {
                string exePath = "moeDEP.exe"; // Adjust the path if needed

                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = exePath,
                        UseShellExecute = true
                    });

                    MessageBox.Show("Joiner executed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to execute Joiner:\n" + ex.Message, "Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MobBut_Click(object sender, EventArgs e)
        {
            {
                string exePath = "mobDEP.exe"; // Adjust the path if needed

                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = exePath,
                        UseShellExecute = true
                    });

                    MessageBox.Show("Joiner executed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to execute Joiner:\n" + ex.Message, "Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}


        