using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using DCP;

namespace Design
{
    public partial class Register : Form
    {
        private SoundPlayer player;
        private const string DatabaseFile = "userDatabase.json";
        public static Dictionary<string, string> userDatabase = new Dictionary<string, string>();
        //public static class UserCredentials
        //{
        //public static string RegisteredUsername { get; set; }
        //public static string RegisteredPassword { get; set; }
        //}
        public Register()
        {
            InitializeComponent();
            LoaderUserData();

            player = new SoundPlayer(DCP.Properties.Resources.Click2);
            player.Load();

            player.Play();
            System.Threading.Thread.Sleep(10);
            player.Stop();

            textBox1.Text = "Register Username";

            // Bind events for focus and leave
            textBox1.Enter += usernameTextBox_Enter;
            textBox1.Leave += usernameTextBox_Leave;

            textBox2.Text = "Register Password";
            textBox2.PasswordChar = '\0'; // Initially disable password char to show "Password"

            textBox2.Enter += passwordTextBox_Enter;
            textBox2.Leave += passwordTextBox_Leave;

            this.StartPosition = FormStartPosition.CenterScreen;
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
        private void LoaderUserData()
        {
            if (File.Exists(DatabaseFile))
            {
                string json = File.ReadAllText(DatabaseFile);
                userDatabase = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }

        }
        private bool IsUserNameTaken(string username)
        {
            return
            userDatabase.ContainsKey(username);
        }
        private void Registercs_Load(object sender, EventArgs e)
        {
           
        }
        private void passwordTextBox_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Register Password")
            {
                textBox2.Text = "";              // Clear the placeholder
                textBox2.PasswordChar = '*';     // Set the password character
                //textBox2.ForeColor = Color.Black; // Optional: set font color for real input
            }
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.PasswordChar = '\0';    // Remove password character to show placeholder
                textBox2.Text = "Register Password";      // Reset placeholder text
                //qtextBox2.ForeColor = Color.Black; // Optional: set font color for placeholder
            }
        }
        private void usernameTextBox_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Register Username")
            {
                textBox1.Text = "";              // Clear the placeholder text
            }
        }

        private void usernameTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Register Username";     // Restore placeholder text if empty
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
                                Introduction introduction = new Introduction();
                                introduction.StartPosition = FormStartPosition.CenterScreen;
                                introduction.Opacity = 0;
                                introduction.Show();

                                // Fade-in effect for Introduction form
                                Timer fadeInTimer = new Timer();
                                fadeInTimer.Interval = 20;
                                fadeInTimer.Tick += (s2, ev2) =>
                                {
                                    if (introduction.Opacity < 1)
                                    {
                                        introduction.Opacity += 0.05;
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
        private void button5_Click(object sender, EventArgs e)
        {
            player.Play();

            DialogResult result = MessageBox.Show("Are you sure you want to close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                        Introduction introduction = new Introduction();
                        introduction.StartPosition = FormStartPosition.CenterScreen;
                        introduction.Opacity = 0;
                        introduction.Show();

                        // Fade-in effect for Introduction form
                        Timer fadeInTimer = new Timer();
                        fadeInTimer.Interval = 20;
                        fadeInTimer.Tick += (s2, ev2) =>
                        {
                            if (introduction.Opacity < 1)
                            {
                                introduction.Opacity += 0.05;
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

        private void button1_Click(object sender, EventArgs e)
        {
            player.Play();

            DialogResult result = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                        Introduction introduction = new Introduction();
                        introduction.StartPosition = FormStartPosition.CenterScreen;
                        introduction.Opacity = 0;
                        introduction.Show();

                        // Fade-in effect for Introduction form
                        Timer fadeInTimer = new Timer();
                        fadeInTimer.Interval = 20;
                        fadeInTimer.Tick += (s2, ev2) =>
                        {
                            if (introduction.Opacity < 1)
                            {
                                introduction.Opacity += 0.05;
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            player.Play();

            string newUsername = textBox1.Text;
            string newPassword = textBox2.Text;

            if (IsUserNameTaken(newUsername))
            {
                MessageBox.Show("Username is already taken. Please type a different one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int minLength = 5;

                if (newUsername.Length < minLength || newPassword.Length < minLength)
                {
                    MessageBox.Show("Username and password must be at least 5 characters long.", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!StrongPassword(newPassword))
                {
                    MessageBox.Show("Password must contain letters, numbers, and punctuation marks.", "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Fade out current form before opening User Agreement
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
                            this.Hide(); // Hide instead of close, in case user cancels

                            // Open User Agreement form
                            UserAgreement userAgreement = new UserAgreement(newUsername, newPassword);
                            userAgreement.StartPosition = FormStartPosition.CenterScreen;
                            userAgreement.Opacity = 0;
                            userAgreement.Show();

                            // Fade in the new form
                            Timer fadeInTimer = new Timer();
                            fadeInTimer.Interval = 20;
                            fadeInTimer.Tick += (s2, ev2) =>
                            {
                                if (userAgreement.Opacity < 1)
                                {
                                    userAgreement.Opacity += 0.05;
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            player.Play();

            DialogResult result = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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

                        // Open the Introduction form with a fade-in effect
                        Introduction introduction = new Introduction();
                        introduction.StartPosition = FormStartPosition.CenterScreen;
                        introduction.Opacity = 0;
                        introduction.Show();

                        // Fade-in effect for Introduction form
                        System.Windows.Forms.Timer fadeInTimer = new System.Windows.Forms.Timer();
                        fadeInTimer.Interval = 20;
                        fadeInTimer.Tick += (s2, ev2) =>
                        {
                            if (introduction.Opacity < 1)
                            {
                                introduction.Opacity += 0.05;
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
