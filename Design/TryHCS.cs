using DCP;
using DCP.Properties;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.IO;
using DCP.Resources;
using Design;

namespace DCP
{
    public partial class TryHCS : Form
    {
        private SoundPlayer player;
        private SoundPlayer randomizer;
        private TryDCRDescriptions challengeDescriptions; // Instance of ChallengeDescriptions class
        private Dictionary<string, Form> formMap; // Map of string identifiers to forms
        private Random random = new Random(); // Random object for shuffling
        private Timer timer; // Timer for randomization duration
        private int timeElapsed; // Track time for shuffling
        private Image currentImage; // Variable to save the randomized image
        private int currentImageIndex = 0; // Track the current image index
        public TryHCS()
        {
            InitializeComponent();

            randomizer = new SoundPlayer(DCP.Properties.Resources.Randomizer6s);
            randomizer.Load();

            player = new SoundPlayer(DCP.Properties.Resources.Click2);
            player.Load();

            player.Play();
            System.Threading.Thread.Sleep(10);
            player.Stop();

            label8.TextAlign = ContentAlignment.MiddleCenter;
            this.StartPosition = FormStartPosition.CenterScreen;

            challengeDescriptions = new TryDCRDescriptions();
            // Initialize ChallengeDescriptions instance

            formMap = new Dictionary<string, Form> {

              //Fitness
              { "Push_Ups__Easy__F", null },
              
              { "Hold_Your_Breath__Easy_H", null },
              
              { "Grammar__Easy_E", null },
              
        };


            timer = new Timer();
            timer.Interval = 100; // Set the interval for timer tick (100ms)
            timer.Tick += Timer_Tick;

            random = new Random();

            // Attach events
            //pictureBox2.Click += RandomizerPictureBox;      // Randomizer PictureBox
            //pictureBox14.Click += CheckPictureBox;          // Check PictureBox
        }
        private void RandomizerPictureBox()
        {
            // Disable check and x picture boxes
            pictureBox2.Enabled = false;
            pictureBox14.Enabled = false; // check picture box

            pictureBox2.Image = DCP.Properties.Resources.Ex;
            pictureBox14.Image = DCP.Properties.Resources.Ex;

            // Clear the richTextBox when randomizing again
            richTextBox1.Clear();

            timeElapsed = 0; // Reset time elapsed
            timer.Start(); // Start the timer for randomization
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            timeElapsed += timer.Interval;

            if (timeElapsed < 5000) // 5 seconds duration
            {
                // Randomly select an image and update index
                int newIndex = random.Next(challengeDescriptions.Images.Count);
                currentImage = challengeDescriptions.Images[newIndex];
                currentImageIndex = newIndex; // Save the randomized index

                pictureBox6.Image = currentImage;

                if (challengeDescriptions.ImageIdentifiers.TryGetValue(currentImage, out string identifier))
                {
                    label8.Text = FormatIdentifier(identifier);
                    if (challengeDescriptions.ImageDescriptions.TryGetValue(identifier, out string description))
                    {
                        richTextBox1.Text = description;
                    }
                }

                // Play the audio only once every 6 seconds or at the start of the timer
                if (timeElapsed == timer.Interval || timeElapsed % 6000 == 0)
                {
                    try
                    {
                        randomizer.Play(); // Play the randomizer sound
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error playing audio: " + ex.Message);
                    }
                }
            }
            else
            {
                timer.Stop();
                pictureBox2.Enabled = true;
                pictureBox14.Enabled = true;
                ArrowLeft.Enabled = true;
                ArrowRight.Enabled = true;

                pictureBox2.Image = DCP.Properties.Resources.Random;
                pictureBox14.Image = DCP.Properties.Resources.Check;
            }
        }
        private string FormatIdentifier(string identifier)
        {
            switch (identifier)
            {
                //Fitness
                case "Push_Ups__Easy__F":
                    return "               PUSH UPS";
                case "Hold_Your_Breath__Easy_H":
                    return "            HOLD BREATH";
                case "Grammar__Easy_E":
                    return "    GRAMMAR QUESTION ENG";
                default:
                    return identifier; // Default case for unknown identifiers
            }
        }

        private void CheckPictureBox()
        {

            if (currentImage != null && challengeDescriptions.ImageIdentifiers.ContainsKey(currentImage))
            {
                // Retrieve the identifier associated with the current image
                string identifier = challengeDescriptions.ImageIdentifiers[currentImage];


                // Check if the identifier exists in the form map
                if (formMap.ContainsKey(identifier))
                {
                    // Lazy load the form if not already created
                    if (formMap[identifier] == null)
                    {
                        formMap[identifier] = CreateFormInstance(identifier);
                    }

                    // Begin fade-out transition
                    Timer fadeOutTimer = new Timer();
                    fadeOutTimer.Interval = 10; // Adjust interval for smoothness of fade-out
                    fadeOutTimer.Tick += (s, ev) =>
                    {
                        if (this.Opacity > 0)
                        {
                            this.Opacity -= 0.05; // Reduce opacity gradually
                        }
                        else
                        {
                            fadeOutTimer.Stop();
                            this.Hide(); // Hide the current form after fade-out

                            // Open the associated form with fade-in effect
                            Form associatedForm = formMap[identifier];
                            associatedForm.StartPosition = FormStartPosition.CenterScreen;
                            associatedForm.Opacity = 0; // Set initial opacity to 0
                            associatedForm.Show();

                            // Begin fade-in transition for the new form
                            Timer fadeInTimer = new Timer();
                            fadeInTimer.Interval = 20; // Adjust interval for smoothness of fade-in
                            fadeInTimer.Tick += (s2, ev2) =>
                            {
                                if (associatedForm.Opacity < 1)
                                {
                                    associatedForm.Opacity += 0.05; // Increase opacity gradually
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
                else
                {
                    MessageBox.Show("The challenge form could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Randomize a challenge first.", "No Challenge", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private Form CreateFormInstance(string formKey)
        {
            switch (formKey)
            {
                //Challenges
                case "Push_Ups__Easy__F": return new PushUpTry();
                case "Hold_Your_Breath__Easy_H": return new HoldYourBreathTry();
                case "Grammar__Easy_E": return new GrammarTry();

                default:
                    MessageBox.Show("Form not defined for this challenge.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
            }
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            player.Play();
            // Disable check and x picture boxes
            pictureBox2.Enabled = false;
            pictureBox14.Enabled = false; // check picture box

            pictureBox2.Image = DCP.Properties.Resources.Ex;
            pictureBox14.Image = DCP.Properties.Resources.Ex;

            // Clear the richTextBox when randomizing again
            richTextBox1.Clear();

            timeElapsed = 0; // Reset time elapsed
            timer.Start(); // Start the timer for randomization
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            player.Play();

            if (currentImage != null && challengeDescriptions.ImageIdentifiers.ContainsKey(currentImage))
            {
                // Retrieve the identifier associated with the current image
                string identifier = challengeDescriptions.ImageIdentifiers[currentImage];

                DialogResult dialogResult = MessageBox.Show("Do you want to accept the challenge?", "Challenge", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    // Check if the identifier exists in the form map
                    if (formMap.ContainsKey(identifier))
                    {
                        // Lazy load the form if not already created
                        if (formMap[identifier] == null)
                        {
                            formMap[identifier] = CreateFormInstance(identifier);
                        }

                        // Begin fade-out transition
                        Timer fadeOutTimer = new Timer();
                        fadeOutTimer.Interval = 10; // Adjust interval for smoothness of fade-out
                        fadeOutTimer.Tick += (s, ev) =>
                        {
                            if (this.Opacity > 0)
                            {
                                this.Opacity -= 0.05; // Reduce opacity gradually
                            }
                            else
                            {
                                fadeOutTimer.Stop();
                                this.Hide(); // Hide the current form after fade-out

                                // Open the associated form with fade-in effect
                                Form associatedForm = formMap[identifier];
                                associatedForm.StartPosition = FormStartPosition.CenterScreen;
                                associatedForm.Opacity = 0; // Set initial opacity to 0
                                associatedForm.Show();

                                // Begin fade-in transition for the new form
                                Timer fadeInTimer = new Timer();
                                fadeInTimer.Interval = 20; // Adjust interval for smoothness of fade-in
                                fadeInTimer.Tick += (s2, ev2) =>
                                {
                                    if (associatedForm.Opacity < 1)
                                    {
                                        associatedForm.Opacity += 0.05; // Increase opacity gradually
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
                    else
                    {
                        MessageBox.Show("The challenge form could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Randomize a challenge first.", "No Challenge", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        Introduction introduction = new Introduction();
                        introduction.StartPosition = FormStartPosition.CenterScreen;
                        introduction.Opacity = 0;
                        introduction.Show();

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

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void ArrowLeft_Click(object sender, EventArgs e)
        {
            player.Play();

            if (challengeDescriptions.Images.Count == 0) return; // Check if there are images

            currentImageIndex = (currentImageIndex - 1 + challengeDescriptions.Images.Count) % challengeDescriptions.Images.Count; // Move to the previous image

            if (challengeDescriptions.Images.Count == 0) return; // Check if there are images

            // Get the current image
            currentImage = challengeDescriptions.Images[currentImageIndex];
            pictureBox6.Image = currentImage; // Update PictureBox6

            // Update label8 text based on the selected image
            if (challengeDescriptions.ImageIdentifiers.TryGetValue(currentImage, out string identifier))
            {
                label8.Text = FormatIdentifier(identifier);

                // Automatically update the description in richTextBox1
                if (challengeDescriptions.ImageDescriptions.TryGetValue(identifier, out string description))
                {
                    richTextBox1.Text = description; // Set the description
                }
            }
        }

        private void ArrowRight_Click(object sender, EventArgs e)
        {
            player.Play();

            if (challengeDescriptions.Images.Count == 0) return; // Check if there are images

            currentImageIndex = (currentImageIndex + 1) % challengeDescriptions.Images.Count; // Move to the next image

            if (challengeDescriptions.Images.Count == 0) return; // Check if there are images

            // Get the current image
            currentImage = challengeDescriptions.Images[currentImageIndex];
            pictureBox6.Image = currentImage; // Update PictureBox6

            // Update label8 text based on the selected image
            if (challengeDescriptions.ImageIdentifiers.TryGetValue(currentImage, out string identifier))
            {
                label8.Text = FormatIdentifier(identifier);

                // Automatically update the description in richTextBox1
                if (challengeDescriptions.ImageDescriptions.TryGetValue(identifier, out string description))
                {
                    richTextBox1.Text = description; // Set the description
                }
            }
        }
    }
}
