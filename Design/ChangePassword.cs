using Design;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Media;

namespace DCP
{
    public partial class ChangePassword : Form
    {
        private SoundPlayer player;
        private const string DatabaseFile = "userDatabase.json";
        private Dictionary<string, string> userDatabase;
        public ChangePassword()
        {
            InitializeComponent();

            player = new SoundPlayer(DCP.Properties.Resources.Click2);
            player.Load();

            player.Play();
            System.Threading.Thread.Sleep(10);
            player.Stop();

            textBox3.Text = "Existing Username";

            // Bind events for focus and leave
            textBox3.Enter += existingusernameTextBox_Enter;
            textBox3.Leave += existingusernameTextBox_Leave;
            
            textBox1.Text = "New Password";

            textBox1.Enter += newpasswordTextBox_Enter;
            textBox1.Leave += newpasswordTextBox_Leave;

            textBox2.Text = "Confirm Password";
            textBox2.PasswordChar = '\0'; // Initially disable password char to show "Password"

            textBox2.Enter += confirmpasswordTextBox_Enter;
            textBox2.Leave += confirmpasswordTextBox_Leave;

            LoadUserData();
            textBox3.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    textBox1.Focus();
                }
            };
            textBox1.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    textBox2.Focus();
                }
            };
            textBox2.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    e.Handled = true;
                    button3.PerformClick();
                }
            };
        }
        private void LoadUserData()
        {
            if (File.Exists(DatabaseFile))
            {
                string json = File.ReadAllText(DatabaseFile);
                userDatabase = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            else
            {
                userDatabase = new Dictionary<string, string>();
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            player.Play();

            if (textBox2.PasswordChar == '*')
            {
                textBox2.PasswordChar = '\0';
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            player.Play();

            string username = textBox3.Text.Trim();
            string newPassword = textBox1.Text.Trim();
            string confirmPassword = textBox2.Text.Trim();
            
            if (string.IsNullOrEmpty(username) || !userDatabase.ContainsKey(username))
            {
                MessageBox.Show("Invalid username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Type your new password.", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Type your confirm password.", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("Username and password must be at least 5 characters long.", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("Username and password must be at least 5 characters long.", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (!StrongPassword(newPassword))
            {
                MessageBox.Show("Password must contain letters, numbers, and punctuation marks.", "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords don't match.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            userDatabase[username] = newPassword;
            SaveUserData();

            MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Timer fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
            fadeOutTimer.Tick += (s, ev) =>
            {
                if (this.Opacity > 0)
                {
                    this.Opacity -= 0.05; // Decrease opacity for fade-out
                }
                else
                {
                    fadeOutTimer.Stop();
                    this.Close();

                    // Start the new form with fade-in
                    Login login = new Login();
                    login.StartPosition = FormStartPosition.CenterScreen;
                    login.Opacity = 0; // Start at 0 for fade-in effect
                    login.Show();

                    // Fade in the new form
                    Timer fadeInTimer = new Timer();
                    fadeInTimer.Interval = 20;
                    fadeInTimer.Tick += (s2, ev2) =>
                    {
                        if (login.Opacity < 1)
                        {
                            login.Opacity += 0.05; // Increase opacity for fade-in
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            player.Play();

            DialogResult result = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
                fadeOutTimer.Tick += (s, ev) =>
                {
                    if (this.Opacity > 0)
                    {
                        this.Opacity -= 0.05; // Decrease opacity for fade-out
                    }
                    else
                    {
                        fadeOutTimer.Stop();
                        this.Close();

                        // Start the new form with fade-in
                        Login login = new Login();
                        login.StartPosition = FormStartPosition.CenterScreen;
                        login.Opacity = 0; // Start at 0 for fade-in effect
                        login.Show();

                        // Fade in the new form
                        Timer fadeInTimer = new Timer();
                        fadeInTimer.Interval = 20;
                        fadeInTimer.Tick += (s2, ev2) =>
                        {
                            if (login.Opacity < 1)
                            {
                                login.Opacity += 0.05; // Increase opacity for fade-in
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

        private void button5_Click(object sender, EventArgs e)
        {
            player.Play();

            DialogResult result = MessageBox.Show("Are you sure you want to close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
                fadeOutTimer.Tick += (s, ev) =>
                {
                    if (this.Opacity > 0)
                    {
                        this.Opacity -= 0.05; // Decrease opacity for fade-out
                    }
                    else
                    {
                        fadeOutTimer.Stop();
                        this.Close();

                        // Start the new form with fade-in
                        Login login = new Login();
                        login.StartPosition = FormStartPosition.CenterScreen;
                        login.Opacity = 0; // Start at 0 for fade-in effect
                        login.Show();

                        // Fade in the new form
                        Timer fadeInTimer = new Timer();
                        fadeInTimer.Interval = 20;
                        fadeInTimer.Tick += (s2, ev2) =>
                        {
                            if (login.Opacity < 1)
                            {
                                login.Opacity += 0.05; // Increase opacity for fade-in
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

        private void ChangePassword_Load(object sender, EventArgs e)
        {

        }
        private void confirmpasswordTextBox_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Confirm Password")
            {
                textBox2.Text = "";              // Clear the placeholder
                textBox2.PasswordChar = '*';     // Set the password character
                //textBox2.ForeColor = Color.Black; // Optional: set font color for real input
            }
        }

        private void confirmpasswordTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.PasswordChar = '\0';    // Remove password character to show placeholder
                textBox2.Text = "Confirm Password";      // Reset placeholder text
                //qtextBox2.ForeColor = Color.Black; // Optional: set font color for placeholder
            }
        }
        private void newpasswordTextBox_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "New Password")
            {
                textBox1.Text = "";              // Clear the placeholder text
            }
        }

        private void newpasswordTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "New Password";     // Restore placeholder text if empty
            }
        }
        private void existingusernameTextBox_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "Existing Username")
            {
                textBox3.Text = "";              // Clear the placeholder text
            }
        }

        private void existingusernameTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox3.Text = "Existing Username";     // Restore placeholder text if empty
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                player.Play();
            }


            if (keyData == Keys.Back)
            {
                if (!(ActiveControl is TextBox))
                {
                    var result = MessageBox.Show("Do you want to go back?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        Timer fadeOutTimer = new Timer();
                        fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
                        fadeOutTimer.Tick += (s, ev) =>
                        {
                            if (this.Opacity > 0)
                            {
                                this.Opacity -= 0.05; // Decrease opacity for fade-out
                            }
                            else
                            {
                                fadeOutTimer.Stop();
                                this.Close();

                                // Start the new form with fade-in
                                Login login = new Login();
                                login.StartPosition = FormStartPosition.CenterScreen;
                                login.Opacity = 0; // Start at 0 for fade-in effect
                                login.Show();

                                // Fade in the new form
                                Timer fadeInTimer = new Timer();
                                fadeInTimer.Interval = 20;
                                fadeInTimer.Tick += (s2, ev2) =>
                                {
                                    if (login.Opacity < 1)
                                    {
                                        login.Opacity += 0.05; // Increase opacity for fade-in
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
        private void SaveUserData()
        {
            var json = JsonConvert.SerializeObject(userDatabase, Formatting.Indented);
            File.WriteAllText(DatabaseFile, json);
        }
        private bool StrongPassword(string password)
        {

            bool hasLetter = false;
            bool hasDigit = false;
            bool hasPunctuation = false;

            foreach (char c in password)
            {
                if (char.IsLetter(c)) hasLetter = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (char.IsPunctuation(c) || char.IsSymbol(c)) hasPunctuation = true;
            }


            return hasLetter && hasDigit && hasPunctuation;
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            player.Play();

            DialogResult result = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
                fadeOutTimer.Tick += (s, ev) =>
                {
                    if (this.Opacity > 0)
                    {
                        this.Opacity -= 0.05; // Decrease opacity for fade-out
                    }
                    else
                    {
                        fadeOutTimer.Stop();
                        this.Close();

                        // Start the new form with fade-in
                        Login login = new Login();
                        login.StartPosition = FormStartPosition.CenterScreen;
                        login.Opacity = 0; // Start at 0 for fade-in effect
                        login.Show();

                        // Fade in the new form
                        Timer fadeInTimer = new Timer();
                        fadeInTimer.Interval = 20;
                        fadeInTimer.Tick += (s2, ev2) =>
                        {
                            if (login.Opacity < 1)
                            {
                                login.Opacity += 0.05; // Increase opacity for fade-in
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
