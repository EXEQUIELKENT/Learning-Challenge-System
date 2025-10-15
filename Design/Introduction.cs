using Microsoft.Win32;
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
    public partial class Introduction : Form
    {
        private SoundPlayer player;
        public Introduction()
        {
            InitializeComponent();
            player = new SoundPlayer(DCP.Properties.Resources.Click2);
            player.Load();

            player.Play();
            System.Threading.Thread.Sleep(10);
            player.Stop();


            this.StartPosition = FormStartPosition.CenterScreen;
            
            //pictureBox5.Click += new EventHandler(PictureBox_Click);
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
           
            Login login = new Login();

            login.Show();

        }
    

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            player.Play();

            Timer fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
            fadeOutTimer.Tick += (s, ev) =>
            {
                if (this.Opacity > 0)
                {
                    this.Opacity -= 0.05; // Decrease opacity for fade-outs
                }
                else
                {

                    fadeOutTimer.Stop();
                    this.Hide();

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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            player.Play();

            if (keyData == Keys.Back)
            {

                var result = MessageBox.Show("Do you want to go back?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                if (result == DialogResult.Yes)
                {
                    Timer fadeOutTimer = new Timer();
                    fadeOutTimer.Interval = 20; // Interval in milliseconds
                    fadeOutTimer.Tick += (s, ev) =>
                    {
                        if (this.Opacity > 0) // Decrease opacity until fully transparent
                        {
                            this.Opacity -= 0.05;
                        }
                        else
                        {
                            fadeOutTimer.Stop(); // Stop timer when fade-out is complete
                            Application.Exit(); // Close the application
                        }
                    };
                    fadeOutTimer.Start();
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Login(object sender, EventArgs e)
        {
            

            this.Opacity = 0; // Start with full transparency

            // Create and start a timer for fade-in effect
            Timer fadeInTimer = new Timer();
            fadeInTimer.Interval = 20; // Interval in milliseconds
            fadeInTimer.Tick += (s, ev) =>
            {
                if (this.Opacity < 1) // Increment opacity until fully visible
                {
                    this.Opacity += 0.05;
                }
                else
                {
                    fadeInTimer.Stop(); // Stop the timer when fade-in is complete
                }
            };
            fadeInTimer.Start();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            player.Play();

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
                    this.Hide();

                    // Start the new form with fade-in

                    Register register = new Register();
                    register.StartPosition = FormStartPosition.CenterScreen;                  
                    register.Opacity = 0; // Start at 0 for fade-in effect
                    register.Show();

                    // Fade in the new form
                    Timer fadeInTimer = new Timer();
                    fadeInTimer.Interval = 20;
                    fadeInTimer.Tick += (s2, ev2) =>
                    {
                        if (register.Opacity < 1)
                        {
                            register.Opacity += 0.05; // Increase opacity for fade-in
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

        private void button4_Click(object sender, EventArgs e)
        {
            player.Play();

            DialogResult result = MessageBox.Show("Are you sure you want to close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 20; // Interval in milliseconds
                fadeOutTimer.Tick += (s, ev) =>
                {
                    if (this.Opacity > 0) // Decrease opacity until fully transparent
                    {
                        this.Opacity -= 0.05;
                    }
                    else
                    {
                        fadeOutTimer.Stop(); // Stop timer when fade-out is complete
                        Application.Exit(); // Close the application
                    }
                };
                fadeOutTimer.Start();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            player.Play();

            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                

                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 20; // Interval in milliseconds
                fadeOutTimer.Tick += (s, ev) =>
                {
                    if (this.Opacity > 0) // Decrease opacity until fully transparent
                    {
                        this.Opacity -= 0.05;
                    }
                    else
                    {
                        fadeOutTimer.Stop(); // Stop timer when fade-out is complete
                        Application.Exit(); // Close the application
                    }
                };
                fadeOutTimer.Start();
                

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            player.Play();

            Timer fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
            fadeOutTimer.Tick += (s, ev) =>
            {
                if (this.Opacity > 0)
                {
                    this.Opacity -= 0.05; // Decrease opacity for fade-outs
                }
                else
                {

                    fadeOutTimer.Stop();
                    this.Hide();

                    // Start the new form with fade-in
                    TryHCS tryDCR = new TryHCS();
                    tryDCR.StartPosition = FormStartPosition.CenterScreen;
                    tryDCR.Opacity = 0; // Start at 0 for fade-in effect
                    tryDCR.Show();

                    // Fade in the new form
                    Timer fadeInTimer = new Timer();
                    fadeInTimer.Interval = 20;
                    fadeInTimer.Tick += (s2, ev2) =>
                    {
                        if (tryDCR.Opacity < 1)
                        {
                            tryDCR.Opacity += 0.05; // Increase opacity for fade-in
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
