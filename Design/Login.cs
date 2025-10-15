using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static Design.Register;
using System.Media;
using DCP;


namespace Design
{
    public partial class Login : Form
    {
        private SoundPlayer player;
        private const string DatabaseFile = "userDatabase.json";
        public static Dictionary<string, string> userDatabase = new Dictionary<string, string>();
        public static string CurrentUsername { get; private set; }
        private readonly Dictionary<string, string>
        predefinedUsers = new
        Dictionary<string, string>
        {
            
        };

        public Login()
        {
            InitializeComponent();

            player = new SoundPlayer(DCP.Properties.Resources.Click2);
            player.Load();

            player.Play();
            System.Threading.Thread.Sleep(10);
            player.Stop();

            textBox1.Text = "Username";

            // Bind events for focus and leave
            textBox1.Enter += usernameTextBox_Enter;
            textBox1.Leave += usernameTextBox_Leave;

            textBox2.Text = "Password";
            textBox2.PasswordChar = '\0'; // Initially disable password char to show "Password"

            textBox2.Enter += passwordTextBox_Enter;
            textBox2.Leave += passwordTextBox_Leave;

            LoadUserData();
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
                    button2.PerformClick();
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
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            

        }
        private void passwordTextBox_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Password")
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
                textBox2.Text = "Password";      // Reset placeholder text
                //qtextBox2.ForeColor = Color.Black; // Optional: set font color for placeholder
            }
        }
        private void usernameTextBox_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Username")
            {
                textBox1.Text = "";              // Clear the placeholder text
            }
        }

        private void usernameTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Username";     // Restore placeholder text if empty
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
          
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

        private void textBox2_TextChanged(object sender, EventArgs e)
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

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                player.Play(); // Play sound only if triggered by a click, not by voice command
            }

            string enteredUsername = textBox1.Text;
            string enteredPassword = textBox2.Text;

            if (string.IsNullOrWhiteSpace(enteredUsername) || string.IsNullOrWhiteSpace(enteredPassword))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (predefinedUsers.ContainsKey(enteredUsername) && predefinedUsers[enteredUsername] == enteredPassword)
            {
                CurrentUsername = enteredUsername;  // Store the username for later use
                await SuccessfulLogin();
            }
            else if (userDatabase.ContainsKey(enteredUsername) && userDatabase[enteredUsername] == enteredPassword)
            {
                CurrentUsername = enteredUsername;  // Store the username for later use
                await SuccessfulLogin();
            }
            else
            {
                MessageBox.Show("Invalid username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SuccessfulLogin()
        {
            // Fade-out effect for the current form
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
                    this.Hide();

                    // Show loading screen with fade-in effect
                    LOADINGSCREEN lOADINGSCREEN = new LOADINGSCREEN();
                    lOADINGSCREEN.StartPosition = FormStartPosition.CenterScreen;
                    lOADINGSCREEN.Opacity = 0;
                    lOADINGSCREEN.Show();

                    // Fade-in effect for loading screen
                    System.Windows.Forms.Timer fadeInTimer = new System.Windows.Forms.Timer();
                    fadeInTimer.Interval = 20;
                    fadeInTimer.Tick += (s2, ev2) =>
                    {
                        if (lOADINGSCREEN.Opacity < 1)
                        {
                            lOADINGSCREEN.Opacity += 0.05;
                        }
                        else
                        {
                            fadeInTimer.Stop();
                        }
                    };
                    fadeInTimer.Start();

                    // Wait for loading screen duration
                    Task.Run(async () =>
                    {
                        await Task.Delay(3000);

                        // Fade-out effect for loading screen
                        this.Invoke((MethodInvoker)(() =>
                        {
                            System.Windows.Forms.Timer loadingFadeOutTimer = new System.Windows.Forms.Timer();
                            loadingFadeOutTimer.Interval = 20;
                            loadingFadeOutTimer.Tick += (s3, ev3) =>
                            {
                                if (lOADINGSCREEN.Opacity > 0)
                                {
                                    lOADINGSCREEN.Opacity -= 0.05;
                                }
                                else
                                {
                                    loadingFadeOutTimer.Stop();
                                    lOADINGSCREEN.Close();

                                    // Fade-in the main form (HOMEPAGE)
                                    HOMEPAGE hOMEPAGE = new HOMEPAGE();
                                    hOMEPAGE.StartPosition = FormStartPosition.CenterScreen;
                                    hOMEPAGE.Opacity = 0;
                                    hOMEPAGE.Show();

                                    System.Windows.Forms.Timer mainFormFadeInTimer = new System.Windows.Forms.Timer();
                                    mainFormFadeInTimer.Interval = 20;
                                    mainFormFadeInTimer.Tick += (s4, ev4) =>
                                    {
                                        if (hOMEPAGE.Opacity < 1)
                                        {
                                            hOMEPAGE.Opacity += 0.05;
                                        }
                                        else
                                        {
                                            mainFormFadeInTimer.Stop();
                                        }
                                    };
                                    mainFormFadeInTimer.Start();
                                }
                            };
                            loadingFadeOutTimer.Start();
                        }));
                    });
                }
            };
            fadeOutTimer.Start();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                //player.Play();
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
                    return true;
                }               
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            player.Play();

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
                    ChangePassword changePassword = new ChangePassword();
                    changePassword.StartPosition = FormStartPosition.CenterScreen;
                    changePassword.Opacity = 0;
                    changePassword.Show();

                    // Fade-in effect for Introduction form
                    System.Windows.Forms.Timer fadeInTimer = new System.Windows.Forms.Timer();
                    fadeInTimer.Interval = 20;
                    fadeInTimer.Tick += (s2, ev2) =>
                    {
                        if (changePassword.Opacity < 1)
                        {
                            changePassword.Opacity += 0.05;
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

        private void pictureBox11_Click(object sender, EventArgs e)
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
