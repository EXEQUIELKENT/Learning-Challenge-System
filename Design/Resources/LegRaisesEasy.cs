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
using System.IO;
using Newtonsoft.Json;

namespace DCP.Resources
{
    public partial class LegRaisesEasy : Form
    {
        private Timer timer;
        private int progressDuration = 30; // total time for progress bar in seconds
        private int timeRemaining;
        private SoundPlayer soundPlayer;
        private SoundPlayer click;
        private SoundPlayer fail;
        private SoundPlayer success;
        private SoundPlayer count;
        private bool isEnterKeyDisabled = false;
        public LegRaisesEasy()
        {
            InitializeComponent();

            success = new SoundPlayer(DCP.Properties.Resources.Success);
            success.Load();

            count = new SoundPlayer(DCP.Properties.Resources.Countdown);
            count.Load();

            fail = new SoundPlayer(DCP.Properties.Resources.Fail);
            fail.Load();

            click = new SoundPlayer(DCP.Properties.Resources.Click2);
            click.Load();

            soundPlayer = new SoundPlayer(DCP.Properties.Resources.Counting);
            soundPlayer.Load();

            click.Play();
            soundPlayer.Play();
            fail.Play();
            count.Play();
            success.Play();

            System.Threading.Thread.Sleep(10);
            soundPlayer.Stop();
            click.Stop();
            fail.Stop();
            count.Stop();
            success.Stop();

            textBox1.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {

                    e.Handled = true;
                    button2.PerformClick();
                }
            };

            timer = new Timer();
            timer.Interval = 1000; // 1 second intervals
            timer.Tick += Timer_Tick;

            // Initialize TextBox for time to "00:00:00"
            textBox2.Text = "00:00:00";

            // Disable the DONE button initially
            button2.Enabled = false;
            button1.Enabled = true;  // "START" button is enabled initially
            textBox1.Enabled = false;
        }

        //Function Codes

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the progress bar and time display
            if (timeRemaining > 0)
            {
                if (progressBar1.Value < progressBar1.Maximum)
                {
                    progressBar1.Value = progressDuration - timeRemaining;
                }
                textBox2.Text = TimeSpan.FromSeconds(progressDuration - timeRemaining).ToString("hh\\:mm\\:ss");

                count.Play();

                timeRemaining--;
            }
            else
            {
                fail.Play();
                timer.Stop();
                progressBar1.Value = progressBar1.Maximum; // Set progress bar to max on completion
                textBox2.Text = TimeSpan.FromSeconds(progressDuration).ToString("hh\\:mm\\:ss"); // Set the final time

                string timeTaken = textBox2.Text; // Use the time from the TextBox

                if (int.TryParse(textBox1.Text, out int reps))
                {
                    // Save challenge data to JSON file
                    SaveChallengeDataFailed(Login.CurrentUsername, reps, timeTaken);
                }

                // Show Challenge Failed message
                DialogResult result = MessageBox.Show("Challenge not completed. Returning to the homepage.", "Challenge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (result == DialogResult.OK)
                {
                    // Fade-out effect and transition to HOMEPAGE
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

                            // Open HOMEPAGE with fade-in effect
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
        public void SaveChallengeDataFailed(string username, int reps, string time)
        {
            // File path for the challenge data
            string date = DateTime.Now.ToString("MM-dd-yy");
            string challengeFilePath = $"{username}_challenge.json";
            var challengeList = new List<dynamic>(); // Use var to handle both old and new records

            // Load existing challenge data
            if (File.Exists(challengeFilePath))
            {
                var existingData = File.ReadAllText(challengeFilePath);
                challengeList = JsonConvert.DeserializeObject<List<dynamic>>(existingData);
            }

            // Prepare the challenge data for PushUpEasy
            var newChallengeData = new
            {
                Date = date,
                FormTitle = "Leg Raises (Easy)",
                Reps = $"Failed {reps}", // For PushUpEasy
                Time = time
            };

            challengeList.Add(newChallengeData);

            // Save back to the JSON file
            var updatedChallengeData = JsonConvert.SerializeObject(challengeList, Formatting.Indented);
            File.WriteAllText(challengeFilePath, updatedChallengeData);
        }
        public void SaveChallengeDataSuccess(string username, int reps, string time)
        {
            // File path for the challenge data
            string date = DateTime.Now.ToString("MM-dd-yy");
            string challengeFilePath = $"{username}_challenge.json";
            var challengeList = new List<dynamic>(); // Use var to handle both old and new records

            // Load existing challenge data
            if (File.Exists(challengeFilePath))
            {
                var existingData = File.ReadAllText(challengeFilePath);
                challengeList = JsonConvert.DeserializeObject<List<dynamic>>(existingData);
            }

            // Prepare the challenge data for PushUpEasy
            var newChallengeData = new
            {
                Date = date,
                FormTitle = "Leg Raises (Easy)",
                Reps = $"Completed {reps}", // For PushUpEasy
                Time = time
            };

            challengeList.Add(newChallengeData);

            // Save back to the JSON file
            var updatedChallengeData = JsonConvert.SerializeObject(challengeList, Formatting.Indented);
            File.WriteAllText(challengeFilePath, updatedChallengeData);
        }
        private void ResetChallenge()
        {
            timer.Stop();
            progressBar1.Value = 0;
            textBox2.Text = "00:00:00";
            textBox1.Clear();
        }
        private void TransitionToHomePage()
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

                    // Open HOMEPAGE with fade-in effect
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
        private void StartChallenge()
        {
            // Enable DONE button
            textBox2.Enabled = true;
            textBox1.Enabled = true;
            button2.Enabled = true;
            pictureBox11.Enabled = true;
            button4.Enabled = true;
            isEnterKeyDisabled = false;

            // Initialize progress bar and timer
            progressBar1.Value = 0;
            progressBar1.Maximum = progressDuration - 1; // Adjusted to fill fully at end
            timeRemaining = progressDuration;

            // Start timer and update time textbox
            timer.Start();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {


            if (keyData == Keys.Back)
            {
                if (!(ActiveControl is TextBox))
                {
                    click.Play();

                    DialogResult result = MessageBox.Show("Are you sure you want to go back?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // Check if the timer is still running
                        if (timer != null && timer.Enabled) // Assuming 'challengeTimer' is your timer
                        {
                            DialogResult result2 = MessageBox.Show("Are you sure you want to go back? This will fail the challenge.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result2 == DialogResult.Yes)
                            {
                                if (int.TryParse(textBox1.Text, out int reps))
                                {
                                    reps = 0;
                                    string timeTaken = textBox2.Text; // Use the time from the TextBox
                                                                      // Save challenge data to JSON file
                                    SaveChallengeDataFailed(Login.CurrentUsername, reps, timeTaken);

                                    timer.Stop();
                                    fail.Play();
                                    progressBar1.Value = progressBar1.Maximum; // Set progress bar to max

                                    MessageBox.Show("Challenge failed as you exited before completing it.", "Challenge Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    reps = 0;
                                    string timeTaken = textBox2.Text; // Use the time from the TextBox
                                                                      // Save challenge data to JSON file
                                    SaveChallengeDataFailed(Login.CurrentUsername, reps, textBox2.Text);

                                    timer.Stop();
                                    fail.Play();
                                    progressBar1.Value = progressBar1.Maximum; // Set progress bar to max

                                    MessageBox.Show("Challenge failed as you exited before completing it.", "Challenge Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                        }
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
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Stop the timer and sound when the form is closing
            if (timer != null && timer.Enabled)
            {
                timer.Stop();
            }

            if (count != null)
            {
                count.Stop();
            }

            base.OnFormClosing(e);
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the entered key is a digit or a control key (e.g., Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Block the character if it's not a digit or control key
            }
        }

        //Button Codes
        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            click.Play();

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Type how many reps you've done.", "No Reps Entered", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (int.TryParse(textBox1.Text, out int reps))
            {
                string message;
                string timeTaken = textBox2.Text; // Use the time from the TextBox

                // Saving challenge results based on the reps
                if (reps <= 3)
                {
                    timer.Stop();
                    count.Stop();
                    success.Play();
                    SaveChallengeDataFailed(Login.CurrentUsername, reps, textBox2.Text);
                    message = "Nice try! You'll get stronger next time.";
                    MessageBox.Show(message, "Challenge Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult result = MessageBox.Show("Challenge not completed. Returning to the homepage.", "Challenge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ResetChallenge();
                    TransitionToHomePage();
                }
                else if (reps <= 8)
                {
                    timer.Stop();
                    count.Stop();
                    success.Play();
                    SaveChallengeDataFailed(Login.CurrentUsername, reps, textBox2.Text);
                    message = "Good try! Keep practicing.";
                    MessageBox.Show(message, "Challenge Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult result = MessageBox.Show("Challenge not completed. Returning to the homepage.", "Challenge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ResetChallenge();
                    TransitionToHomePage();
                }
                else if (reps >= 10)
                {
                    timer.Stop();
                    count.Stop();
                    success.Play();
                    SaveChallengeDataSuccess(Login.CurrentUsername, reps, timeTaken);
                    message = "Excellent work! Keep it up!";
                    MessageBox.Show(message, "Challenge Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("Challenge Complete. Returning to the homepage.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetChallenge();
                    TransitionToHomePage();
                }


            }
            else
            {
                MessageBox.Show("Please enter a valid number for reps.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            click.Play();
            // Disable the START button once clicked

            textBox2.Enabled = false;
            textBox1.Enabled = false;
            button1.Enabled = false;
            button4.Enabled = false;
            pictureBox11.Enabled = false;
            isEnterKeyDisabled = true;

            // Play the audio
            soundPlayer.Play();

            // Delay the start of the timer and progress bar until audio is finished
            Timer audioTimer = new Timer();
            audioTimer.Interval = 4000; // 14 seconds
            audioTimer.Tick += (s, args) =>
            {
                audioTimer.Stop();
                audioTimer.Dispose();
                StartChallenge();
            };
            audioTimer.Start();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            click.Play();

            DialogResult result = MessageBox.Show("Are you sure you want to go back?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Check if the timer is still running
                if (timer != null && timer.Enabled) // Assuming 'challengeTimer' is your timer
                {
                    DialogResult result2 = MessageBox.Show("Are you sure you want to go back? This will fail the challenge.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result2 == DialogResult.Yes)
                    {
                        if (int.TryParse(textBox1.Text, out int reps))
                        {
                            reps = 0;
                            string timeTaken = textBox2.Text; // Use the time from the TextBox
                                                              // Save challenge data to JSON file
                            SaveChallengeDataFailed(Login.CurrentUsername, reps, textBox2.Text);

                            timer.Stop();
                            fail.Play();
                            progressBar1.Value = progressBar1.Maximum; // Set progress bar to max

                            MessageBox.Show("Challenge failed as you exited before completing it.", "Challenge Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            reps = 0;
                            string timeTaken = textBox2.Text; // Use the time from the TextBox
                                                              // Save challenge data to JSON file
                            SaveChallengeDataFailed(Login.CurrentUsername, reps, textBox2.Text);

                            timer.Stop();
                            fail.Play();
                            progressBar1.Value = progressBar1.Maximum; // Set progress bar to max

                            MessageBox.Show("Challenge failed as you exited before completing it.", "Challenge Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
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
            click.Play();

            DialogResult result = MessageBox.Show("Are you sure you want to close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Check if the timer is still running
                if (timer != null && timer.Enabled) // Assuming 'challengeTimer' is your timer
                {
                    DialogResult result2 = MessageBox.Show("Are you sure you want to close? This will fail the challenge.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result2 == DialogResult.Yes)
                    {
                        if (int.TryParse(textBox1.Text, out int reps))
                        {
                            reps = 0;
                            string timeTaken = textBox2.Text; // Use the time from the TextBox
                                                              // Save challenge data to JSON file
                            SaveChallengeDataFailed(Login.CurrentUsername, reps, timeTaken);

                            timer.Stop();
                            fail.Play();
                            progressBar1.Value = progressBar1.Maximum; // Set progress bar to max

                            MessageBox.Show("Challenge failed as you closed the challenge before completing it.", "Challenge Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            reps = 0;
                            string timeTaken = textBox2.Text; // Use the time from the TextBox
                                                              // Save challenge data to JSON file
                            SaveChallengeDataFailed(Login.CurrentUsername, reps, textBox2.Text);

                            timer.Stop();
                            fail.Play();
                            progressBar1.Value = progressBar1.Maximum; // Set progress bar to max

                            MessageBox.Show("Challenge failed as you closed the challenge before completing it.", "Challenge Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
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

        private void LegRaisesEasy_Load(object sender, EventArgs e)
        {
            textBox1.KeyPress += textBox1_KeyPress;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string url = "https://www.youtube.com/watch?v=l4kQd9eWclE";
            System.Diagnostics.Process.Start("cmd", $"/c start {url}");
        }

        private void label6_Click(object sender, EventArgs e)
        {
            string url = "https://www.youtube.com/@howcast";
            System.Diagnostics.Process.Start("cmd", $"/c start {url}");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string url = "https://www.youtube.com/@howcast";
            System.Diagnostics.Process.Start("cmd", $"/c start {url}");
        }
    }
}
