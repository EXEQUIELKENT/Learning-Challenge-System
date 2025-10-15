using Design;
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

namespace DCP
{
    public partial class SelectionLearning : Form
    {
        private SoundPlayer player;
        public SelectionLearning()
        {
            InitializeComponent();

            player = new SoundPlayer(DCP.Properties.Resources.Click2);
            player.Load();

            player.Play();
            System.Threading.Thread.Sleep(10);
            player.Stop();

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            player.Play();
            // Code for logout confirmation and fade-out effect
            DialogResult result = MessageBox.Show("Are you sure you want to go logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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

        private void button4_Click(object sender, EventArgs e)
        {
            player.Play();
            // Code for close confirmation and fade-out effect
            DialogResult result = MessageBox.Show("Are you sure you want to go close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        private void SelectionLearning_Load(object sender, EventArgs e)
        {

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.Back)
            {
                player.Play();

                var result = MessageBox.Show("Do you want to go back?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                if (result == DialogResult.Yes)
                {
                    Timer fadeOutTimer = new Timer();
                    fadeOutTimer.Interval = 20; // Interval in milliseconds
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
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void EnglishPictureBox_Click(object sender, EventArgs e)
        {
            OpenHealthForm("English");
        }

        private void FilipinoPictureBox_Click(object sender, EventArgs e)
        {
            OpenHealthForm("Filipino");
        }

        private void MathPictureBox_Click(object sender, EventArgs e)
        {
            OpenHealthForm("Math");
        }

        private void RandomPictureBox_Click(object sender, EventArgs e)
        {
            OpenHealthForm(null);

        }
        private bool isTransitioning = false; // Flag to prevent multiple transitions
        private void OpenHealthForm(string category)
        {
            if (isTransitioning) return; // Prevent clicks during transition
            isTransitioning = true; // Mark transition as active

            player.Play();

            // Disable PictureBoxes during transition
            FilipinoPictureBox.Enabled = false;
            EnglishPictureBox.Enabled = false;
            MathPictureBox.Enabled = false;
            RandomPictureBox.Enabled = false;

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
                    fadeOutTimer.Dispose();
                    this.Hide(); // Hide instead of Close to avoid immediate disposal issues

                    // Open Fitness form and pass category info
                    Learning learning = new Learning(category);
                    learning.StartPosition = FormStartPosition.CenterScreen;
                    learning.Opacity = 0;
                    learning.Show();

                    Timer fadeInTimer = new Timer();
                    fadeInTimer.Interval = 20;
                    fadeInTimer.Tick += (s2, ev2) =>
                    {
                        if (learning.Opacity < 1)
                        {
                            learning.Opacity += 0.05;
                        }
                        else
                        {
                            fadeInTimer.Stop();
                            fadeOutTimer.Dispose();

                            FilipinoPictureBox.Enabled = false;
                            EnglishPictureBox.Enabled = false;
                            MathPictureBox.Enabled = false;
                            RandomPictureBox.Enabled = false;
                            isTransitioning = false; // Allow new transitionss
                        }
                    };
                    fadeInTimer.Start();
                }
            };
            fadeOutTimer.Start();
        }
    }
}
