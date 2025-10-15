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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using Microsoft.VisualBasic.Devices;

namespace DCP.Resources
{

    public partial class WordCountMedium : Form
    {
        private Timer timer;
        private int progressDuration = 300; // total time for progress bar in seconds
        private int timeRemaining;
        private SoundPlayer soundPlayer;
        private SoundPlayer click;
        private SoundPlayer fail;
        private SoundPlayer success;
        private SoundPlayer count;
        private bool isEnterKeyDisabled = false;
        private int wordCount; // Private variable to store word count
        private int userGuess; // Private variable for the user's guess
        public WordCountMedium()
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

            timer = new Timer();
            timer.Interval = 1000; // 1 second intervals
            timer.Tick += Timer_Tick;

            // Initialize TextBox for time to "00:00:00"
            textBox2.Text = "00:00:00";

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
                userGuess = 0;
                // Challenge completed successfully when the time is up
                fail.Play();
                timer.Stop();
                progressBar1.Value = progressBar1.Maximum; // Set progress bar to max on completion
                textBox2.Text = TimeSpan.FromSeconds(progressDuration).ToString("hh\\:mm\\:ss"); // Set the final time

                // Save challenge data to JSON file
                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, userGuess);

                MessageBox.Show("Challenge not completed. Returning to the homepage.", "Challenge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TransitionToHomePage();
            }
        }
        public void SaveChallengeDataSuccess(string username, string time, int userGuess)
        {
            // File path for the challenge data
            string date = DateTime.Now.ToString("MM-dd-yy");
            string challengeFilePath = $"{username}_challenge.json";
            var challengeList = new List<dynamic>(); // Use dynamic for flexibility with old/new records

            // Load existing challenge data
            if (File.Exists(challengeFilePath))
            {
                var existingData = File.ReadAllText(challengeFilePath);
                challengeList = JsonConvert.DeserializeObject<List<dynamic>>(existingData);
            }

            // Add the new challenge data
            var newChallengeData = new
            {
                Date = date,
                FormTitle = "Word Count (Medium)",
                Words = $"Completed {userGuess}", // New property for jogging
                Time = time
            };

            challengeList.Add(newChallengeData);

            // Save back to the JSON file
            var updatedChallengeData = JsonConvert.SerializeObject(challengeList, Formatting.Indented);
            File.WriteAllText(challengeFilePath, updatedChallengeData);
        }
        public void SaveChallengeDataFailed(string username, string time, int userGuess)
        {
            // File path for the challenge data
            string date = DateTime.Now.ToString("MM-dd-yy");
            string challengeFilePath = $"{username}_challenge.json";
            var challengeList = new List<dynamic>(); // Use dynamic for flexibility with old/new records

            // Load existing challenge data
            if (File.Exists(challengeFilePath))
            {
                var existingData = File.ReadAllText(challengeFilePath);
                challengeList = JsonConvert.DeserializeObject<List<dynamic>>(existingData);
            }

            // Add the new challenge data
            var newChallengeData = new
            {
                Date = date,
                FormTitle = "Word Count (Medium)",
                Words = $"Failed {userGuess}", // New property for jogging
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
            textBox1.Enabled = true;
            button1.Enabled = true;
            textBox2.Enabled = true;
            pictureBox11.Enabled = true;
            button4.Enabled = true;
            isEnterKeyDisabled = false;

            InitializeContent(); // Ensure the content is initialized
            DisplayRandomStory();

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
                                userGuess = 0;
                                // Save challenge data to JSON file
                                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, userGuess);

                                timer.Stop();
                                fail.Play();
                                progressBar1.Value = progressBar1.Maximum; // Set progress bar to max

                                MessageBox.Show("Challenge failed as you exited before completing it.", "Challenge Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void DisplayContent(int index)
        {
            if (index >= 0 && index < contentList.Count)
            {
                EssayrichTextBox1.Text = contentList[index];
            }
        }
        private List<string> contentList;
        private int currentIndex = 0;

        private void InitializeContent()
        {
            contentList = new List<string>
        {
        //Story 1: Magic Story

        "                       THE MAGIC OF THE ANCIENT FOREST\n\n" +
        "Long ago, in a mystical land far beyond the mountains, there was a forest known for its magical powers. " +
        "Whispers told of a hidden treasure within its depths, a treasure that could grant unimaginable power to those who " +
        "could unlock its secrets. But the forest was not kind to trespassers. Strange creatures roamed its shadowed paths, " +
        "and the trees themselves seemed to move, guiding the lost deeper into the unknown.\n\n" +

        "A young mage named Elara, driven by curiosity and ambition, ventured into the forest. She was determined to uncover " +
        "the secrets hidden within, believing that the treasure could bring prosperity to her struggling village. As she traveled " +
        "deeper, she encountered many challenges. The forest tested her wit, her bravery, and even her heart, but Elara’s spirit " +
        "remained strong. Along her journey, she met a wise old owl who revealed that the true magic of the forest lay not in its " +
        "treasure, but in the lessons it taught those who sought it.\n\n" +

        "Elara continued her journey, and over time, she learned to see the world in a new light. She realized that the forest’s " +
        "power was not just in the physical realm, but in the strength of the mind and soul. The true magic was in overcoming " +
        "fear, in finding peace within, and in understanding that the greatest treasures were often found within one’s heart. " +
        "Elara returned to her village, not with riches, but with wisdom and knowledge, becoming a beacon of hope for all who " +
        "needed guidance.\n\n" +

        "Years passed, and Elara became a revered mage, known far and wide for her deep understanding of magic. She passed " +
        "on the teachings of the forest to many, reminding them that magic was not something to be possessed, but something to " +
        "be understood. And so, the legend of the ancient forest lived on, not as a place of danger, but as a reminder of the " +
        "power that lies in seeking knowledge and understanding the magic that exists within us all.",
        //Story 2: The Bible Story

        "                       THE GOOD SAMARITAN\n\n" +
        "One day, a lawyer asked Jesus, 'Teacher, what must I do to inherit eternal life?' Jesus replied, 'What is written " +
        "in the law? How do you read it?' The lawyer answered, 'Love the Lord your God with all your heart, and love your " +
        "neighbor as yourself.' Jesus said, 'You have answered correctly. Do this and you will live.' But the lawyer, wanting " +
        "to justify himself, asked, 'Who is my neighbor?'\n\n" +

        "In reply, Jesus told a parable: 'A man was going down from Jerusalem to Jericho when he was attacked by robbers. " +
        "They stripped him of his clothes, beat him and went away, leaving him half dead. A priest happened to be going down the " +
        "same road, and when he saw the man, he passed by on the other side. So too, a Levite, when he came to the place and saw " +
        "him, passed by on the other side.'\n\n" +

        "But a Samaritan, as he traveled, came where the man was; and when he saw him, he took pity on him. He went to him and " +
        "bandaged his wounds, pouring on oil and wine. Then he put the man on his own donkey, brought him to an inn and took care " +
        "of him. The next day, he took out two denarii and gave them to the innkeeper. 'Look after him,' he said, 'and when I " +
        "return, I will reimburse you for any extra expense you may have.'\n\n" +

        "Jesus then asked, 'Which of these three do you think was a neighbor to the man who fell into the hands of robbers?' " +
        "The expert in the law replied, 'The one who had mercy on him.' Jesus told him, 'Go and do likewise.' This parable teaches " +
        "us the importance of showing kindness and compassion to everyone, regardless of their background or circumstances. " +
        "True neighborliness is about loving others as we love ourselves.",

        //Story 3: Christmas Story

        "                       THE FIRST CHRISTMAS NIGHT\n\n" +
        "On a cold winter night, in a humble town called Bethlehem, a miracle was about to happen. A young woman named Mary " +
        "and her husband Joseph had traveled from their home in Nazareth to Bethlehem to register for a census. Mary, who was " +
        "pregnant with a child, was about to give birth, but there was no room in the inn. The couple found shelter in a stable, " +
        "and there, in the quiet of the night, Mary gave birth to a baby boy. She wrapped him in swaddling clothes and laid him " +
        "in a manger, for there was no crib for his bed.\n\n" +

        "That night, in the fields nearby, shepherds were watching their flocks when an angel appeared to them, shining with the " +
        "glory of God. The angel said to them, 'Do not be afraid. I bring you good news that will cause great joy for all the " +
        "people. Today in the town of David, a Savior has been born to you; he is the Messiah, the Lord.' The angel told the " +
        "shepherds how they would find the baby wrapped in cloths and lying in a manger. Suddenly, a great company of heavenly " +
        "hosts appeared, praising God and saying, 'Glory to God in the highest heaven, and on earth peace to those on whom his " +
        "favor rests.'\n\n" +

        "The shepherds hurried to Bethlehem and found Mary and Joseph, and the baby, who was lying in the manger. When they " +
        "saw him, they spread the word concerning what had been told them about this child, and all who heard it were amazed " +
        "at what the shepherds said to them. But Mary treasured up all these things and pondered them in her heart. The shepherds " +
        "returned, glorifying and praising God for all the things they had heard and seen, which were just as they had been told.\n\n" +

        "The birth of Jesus was the fulfillment of a prophecy, the arrival of the Savior who would bring peace and hope to the " +
        "world. On that first Christmas night, the world was forever changed, and the message of love, joy, and peace spread to " +
        "all nations. Christmas became a time to celebrate the birth of the Savior, the ultimate gift of love given to humanity."

        };


        }
        private void DisplayRandomStory()
        {
            if (contentList == null || contentList.Count == 0)
            {
                MessageBox.Show("No content available to display.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Assign fixed word counts for each story
            Dictionary<int, int> storyWordCounts = new Dictionary<int, int>
            {
                { 0, 333 }, // Story 1 word count
                { 1, 310  }, // Story 2 word count
                { 2, 372  }  // Story 3 word count
            };

            // Generate a random index
            Random random = new Random();
            currentIndex = random.Next(contentList.Count);

            // Retrieve the selected story and its word count
            string selectedStory = contentList[currentIndex];
            wordCount = storyWordCounts.ContainsKey(currentIndex) ? storyWordCounts[currentIndex] : 0;

            // Display the story in the EssayrichTextBox1
            EssayrichTextBox1.Text = selectedStory;

            // Log or validate the fixed word count (optional)
            Console.WriteLine($"Displayed story index: {currentIndex}, Word count: {wordCount}");
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
                        userGuess = 0;
                        // Save challenge data to JSON file
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, userGuess);

                        timer.Stop();
                        fail.Play();
                        progressBar1.Value = progressBar1.Maximum; // Set progress bar to max

                        MessageBox.Show("Challenge failed as you exited before completing it.", "Challenge Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button1_Click(object sender, EventArgs e)
        {
            click.Play();

            if (button1.Text == "START")
            {
                // Start the challenge
                button1.Text = "DONE";
                button1.Enabled = false;
                textBox2.Enabled = false;
                button4.Enabled = false;
                pictureBox11.Enabled = false;
                isEnterKeyDisabled = true;

                soundPlayer.Play();

                // Delay the start of the timer and progress bar
                Timer audioTimer = new Timer();
                audioTimer.Interval = 4000; // Adjusted delay for audio
                audioTimer.Tick += (s, args) =>
                {
                    audioTimer.Stop();
                    audioTimer.Dispose();
                    StartChallenge();
                };
                audioTimer.Start();
            }
            else if (button1.Text == "DONE")
            {
                DialogResult result = MessageBox.Show("Are you sure with your guess?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Challenge completed, check the word count guess
                    int userGuess = 0;

                    if (int.TryParse(textBox1.Text, out userGuess))
                    {
                        if (userGuess == wordCount)
                        {
                            timer.Stop();
                            progressBar1.Value = progressBar1.Maximum; // Set progress bar to max
                            textBox2.Text = TimeSpan.FromSeconds(progressDuration - timeRemaining).ToString("hh\\:mm\\:ss"); // Update final time
                            SaveChallengeDataSuccess(Login.CurrentUsername, textBox2.Text, userGuess);

                            MessageBox.Show($"Word count guess: {userGuess}/{wordCount} words", "Word Count Score", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            success.Play();

                            MessageBox.Show("Challenge Complete. Returning to the homepage.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            TransitionToHomePage();
                        }
                        else
                        {
                            timer.Stop();
                            progressBar1.Value = progressBar1.Maximum; // Set progress bar to max
                            textBox2.Text = TimeSpan.FromSeconds(progressDuration - timeRemaining).ToString("hh\\:mm\\:ss"); // Update final time
                            SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, userGuess);

                            MessageBox.Show($"Word count guess: {userGuess}/{wordCount} words", "Word Count Score", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            fail.Play();

                            MessageBox.Show("Challenge not completed. Returning to the homepage.", "Challenge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            TransitionToHomePage();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid number for the word count.",
                               "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void EssayrichTextBox1_TextChanged(object sender, EventArgs e)
        {

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
                        userGuess = 0;
                        // Save challenge data to JSON file
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, userGuess);

                        timer.Stop();
                        fail.Play();
                        progressBar1.Value = progressBar1.Maximum; // Set progress bar to max

                        MessageBox.Show("Challenge failed as you closed the challenge before completing it.", "Challenge Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void WordCountMedium_Load(object sender, EventArgs e)
        {
            textBox1.KeyPress += textBox1_KeyPress;
        }
    }
}
