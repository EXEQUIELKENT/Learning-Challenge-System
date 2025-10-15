using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using System.Media;
using Design;

namespace DCP
{
    public partial class FeedBack : Form
    {
        private SoundPlayer player;
        public FeedBack()
        {
            InitializeComponent();


            player = new SoundPlayer(DCP.Properties.Resources.Click2);
            player.Load();

            player.Play();
            System.Threading.Thread.Sleep(10);
            player.Stop();

            textBox1.Text = "Name";

            // Bind events for focus and leave
            textBox1.Enter += usernameTextBox_Enter;
            textBox1.Leave += usernameTextBox_Leave;

            textBox1.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {

                    e.Handled = true;
                    EssayrichTextBox1.Focus();
                }
            };
            EssayrichTextBox1.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {

                    e.Handled = true;
                    button2.PerformClick();
                }
            };
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.Back)
            {
                if (!(ActiveControl is TextBox || ActiveControl is RichTextBox))
                {
                    var result = MessageBox.Show("Do you want to go back?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                    if (result == DialogResult.Yes)
                    {
                        System.Windows.Forms.Timer fadeOutTimer = new System.Windows.Forms.Timer();
                        fadeOutTimer.Interval = 10;
                        fadeOutTimer.Tick += (s, ev) =>
                        {
                            if (this.Opacity > 0)
                            {
                                this.Opacity -= 0.05;
                            }
                            else
                            {
                                fadeOutTimer.Stop();
                                this.Close();

                                // Open the Introduction form with a fade-in effect
                                HOMEPAGE hOMEPAGE = new HOMEPAGE();
                                hOMEPAGE.StartPosition = FormStartPosition.CenterScreen;
                                hOMEPAGE.Opacity = 0;
                                hOMEPAGE.Show();

                                // Fade-in effect for Introduction form
                                System.Windows.Forms.Timer fadeInTimer = new System.Windows.Forms.Timer();
                                fadeInTimer.Interval = 20;
                                fadeInTimer.Tick += (s2, ev2) =>
                                {
                                    if (hOMEPAGE.Opacity < 1)
                                    {
                                        hOMEPAGE.Opacity += 0.05;
                                    }
                                    else
                                    {
                                        fadeInTimer.Stop();
                                    }
                                };
                                fadeInTimer.Start();
                            }
                        };
                        fadeOutTimer.Start();
                    }
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void usernameTextBox_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Name")
            {
                textBox1.Text = "";              // Clear the placeholder text
            }
        }

        private void usernameTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Name";     // Restore placeholder text if empty
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            player.Play();
            // Get user input from TextBox1 (Name) and RichTextBox1 (Feedback)
            string userName = textBox1.Text;
            string userFeedback = EssayrichTextBox1.Text;

            // Validate that both fields are filled out
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userFeedback))
            {
                MessageBox.Show("Please fill in both your name and feedback before submitting.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Define the recipient email address
                string recipientEmail = "bartolomeexequielkent@gmail.com"; // Change this to the feedback email address

                // URL encode the subject and body text to handle special characters correctly
                string subject = Uri.EscapeDataString(userName); // Use the user's name as the subject
                string body = Uri.EscapeDataString(userFeedback); // Use the user's feedback as the body

                // Build the mailto URL with the subject and body
                string mailtoLink = $"mailto:{recipientEmail}?subject={subject}&body={body}";

                // Open the default email client with the pre-filled information
                System.Diagnostics.Process.Start(mailtoLink);

                // Provide feedback to the user
                MessageBox.Show("Your feedback has been opened in your email client. Please send it to submit.", "Feedback Opened", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optionally clear the input fields after submission
                textBox1.Clear();
                EssayrichTextBox1.Clear();

            }
            catch (Exception ex)
            {
                // Handle errors (e.g., unable to open email client)
                MessageBox.Show($"Error: {ex.Message}", "Submission Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            player.Play();
            // Code for logout confirmation and fade-out effect
            DialogResult result = MessageBox.Show("Are you sure you want to go back?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 10;
                fadeOutTimer.Tick += (s, ev) =>
                {
                    if (this.Opacity > 0)
                    {
                        this.Opacity -= 0.05;
                    }
                    else
                    {
                        fadeOutTimer.Stop();
                        this.Close();

                        // Open Introduction form with fade-in effect
                        HOMEPAGE hOMEPAGE = new HOMEPAGE();
                        hOMEPAGE.StartPosition = FormStartPosition.CenterScreen;
                        hOMEPAGE.Opacity = 0;
                        hOMEPAGE.Show();

                        Timer fadeInTimer = new Timer();
                        fadeInTimer.Interval = 20;
                        fadeInTimer.Tick += (s2, ev2) =>
                        {
                            if (hOMEPAGE.Opacity < 1)
                            {
                                hOMEPAGE.Opacity += 0.05;
                            }
                            else
                            {
                                fadeInTimer.Stop();
                            }
                        };
                        fadeInTimer.Start();
                    }
                };
                fadeOutTimer.Start();
            }
        }

        private void EssayrichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            player.Play();
            // Code for logout confirmation and fade-out effect
            DialogResult result = MessageBox.Show("Are you sure you want to close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 10;
                fadeOutTimer.Tick += (s, ev) =>
                {
                    if (this.Opacity > 0)
                    {
                        this.Opacity -= 0.05;
                    }
                    else
                    {
                        fadeOutTimer.Stop();
                        this.Close();

                        // Open Introduction form with fade-in effect
                        HOMEPAGE hOMEPAGE = new HOMEPAGE();
                        hOMEPAGE.StartPosition = FormStartPosition.CenterScreen;
                        hOMEPAGE.Opacity = 0;
                        hOMEPAGE.Show();

                        Timer fadeInTimer = new Timer();
                        fadeInTimer.Interval = 20;
                        fadeInTimer.Tick += (s2, ev2) =>
                        {
                            if (hOMEPAGE.Opacity < 1)
                            {
                                hOMEPAGE.Opacity += 0.05;
                            }
                            else
                            {
                                fadeInTimer.Stop();
                            }
                        };
                        fadeInTimer.Start();
                    }
                };
                fadeOutTimer.Start();
            }
        }
    }
}
