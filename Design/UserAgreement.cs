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
using Design;

namespace DCP
{
    public partial class UserAgreement : Form
    {
        private SoundPlayer player;
        private const string DatabaseFile = "userDatabase.json";
        public static Dictionary<string, string> userDatabase = new Dictionary<string, string>();
        private string username;
        private string password;
        private Register registerForm; // Reference to Register Form

        public UserAgreement(string user, string pass)
        {
            InitializeComponent();
            LoaderUserData();
            this.username = user;
            this.password = pass;
            this.Opacity = 0;
            FadeIn();
        }

        private void LoaderUserData()
        {
            if (File.Exists(DatabaseFile))
            {
                string json = File.ReadAllText(DatabaseFile);
                userDatabase = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }

        }
        private void SaveUserData()
        {
            string json = JsonConvert.SerializeObject(userDatabase, Formatting.Indented);
            File.WriteAllText(DatabaseFile, json);
        }
        private void buttonBack_Click(object sender, EventArgs e)
        {
            FadeOutAndShowRegister();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (radioButtonAccept.Checked)
            {
                // Register user
                userDatabase[username] = password;
                SaveUserData();

                MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Fade out and open Introduction form
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

                        // Open Introduction form
                        Introduction introduction = new Introduction();
                        introduction.StartPosition = FormStartPosition.CenterScreen;
                        introduction.Opacity = 0;
                        introduction.Show();

                        // Fade in new form
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
            else if (radioButtonDontAccept.Checked) // If "I don't accept" is selected
            {
                MessageBox.Show("You must accept the agreement to continue.", "Agreement Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FadeOutAndShowRegister(); // Go back to Register Form
            }
            else
            {
                MessageBox.Show("Please select an option before proceeding.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FadeIn()
        {
            Timer fadeInTimer = new Timer();
            fadeInTimer.Interval = 20;
            fadeInTimer.Tick += (s, e) =>
            {
                if (this.Opacity < 1)
                {
                    this.Opacity += 0.05;
                }
                else
                {
                    fadeInTimer.Stop();
                }
            };
            fadeInTimer.Start();
        }

        private void FadeOutAndShowRegister()
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
                    this.Hide(); // Hide first to prevent errors

                    // Open Register form
                    Register registerForm = new Register();
                    registerForm.StartPosition = FormStartPosition.CenterScreen;
                    registerForm.Opacity = 0;
                    registerForm.Show();

                    // Fade in effect
                    Timer fadeInTimer = new Timer();
                    fadeInTimer.Interval = 20;
                    fadeInTimer.Tick += (s2, ev2) =>
                    {
                        if (registerForm.Opacity < 1)
                        {
                            registerForm.Opacity += 0.05;
                        }
                        else
                        {
                            fadeInTimer.Stop();
                            this.Close(); // Close only after fade-in completes
                        }
                    };
                    fadeInTimer.Start();
                }
            };
            fadeOutTimer.Start();
        }
    }
}