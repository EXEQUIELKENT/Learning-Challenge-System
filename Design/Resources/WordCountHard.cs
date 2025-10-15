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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace DCP.Resources
{

    public partial class WordCountHard : Form
    {
        private Timer timer;
        private int progressDuration = 900; // total time for progress bar in seconds
        private int timeRemaining;
        private SoundPlayer soundPlayer;
        private SoundPlayer click;
        private SoundPlayer fail;
        private SoundPlayer success;
        private SoundPlayer count;
        private bool isEnterKeyDisabled = false;
        private int wordCount; // Private variable to store word count
        private int userGuess; // Private variable for the user's guess
        public WordCountHard()
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
                FormTitle = "Word Count (Hard)",
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
                FormTitle = "Word Count (Hard)",
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
        // Story 1: Science Fiction
        "                      THE LAST FRONTIER\n\n" +
        "Captain Aiden Harper stood on the observation deck of the starship Perseus, gazing at the vast expanse of space before him. " +
        "The mission was simple: explore the unknown sector, map the planets, and report any signs of extraterrestrial life. However, " +
        "the deeper they ventured into the void, the more anomalies they encountered. Strange signals echoed through the ship's " +
        "communications systems, and objects in the distance defied all logic. Harper could feel the weight of the unknown pressing in on him.\n\n" +

        "As the crew delved further into the mysterious region, they discovered a hidden planet, one not marked on any star chart. " +
        "From the surface, the planet appeared peaceful, but an ancient structure buried beneath its soil told a different story. The " +
        "team quickly uncovered a long-lost civilization, their technology far surpassing anything humanity had ever achieved. Yet, " +
        "what intrigued Harper the most was the message encoded in the ruins – a warning about an impending cosmic threat.\n\n" +

        "Harper’s team was torn between returning to Earth with their discovery and investigating the strange signal that seemed to " +
        "lead them further into danger. Against the advice of his officers, Harper decided to follow the signal, hoping to uncover " +
        "the truth before it was too late. But as they moved closer to the source, they realized that the warning was not just " +
        "about their galaxy – it was a message meant for the entire universe.\n\n" +

        "In a race against time, Harper and his team discovered that an ancient, destructive force was on its way to annihilate " +
        "everything in its path. With no time to spare, the crew scrambled to devise a plan to stop the threat. As Harper gazed " +
        "at the approaching danger, he realized that the fate of countless worlds rested in his hands. He made the ultimate " +
        "sacrifice, activating the ship's self-destruct sequence to prevent the catastrophic force from reaching the core of the galaxy.\n\n" +

        "Harper’s final moments were spent in quiet contemplation, knowing that although he would not live to see the end, his " +
        "actions would ensure the survival of countless civilizations. The crew of the Perseus had given their lives for the " +
        "greater good, and as the starship exploded in a blaze of light, the message of hope they sent would live on in the " +
        "stars forever.",
        // Story 2: Romance
         "                     THE TIME WE HAD\n\n" +
        "Emma had always believed that love was something out of her reach. She was practical, focused on her career, and rarely " +
        "gave in to the fantasies of romance that her friends often talked about. But then, she met Nathan. He was everything she " +
        "wasn’t—spontaneous, adventurous, and full of life. Their worlds collided one rainy afternoon in a small coffee shop, " +
        "where an accidental spill of coffee sparked a conversation that would change both their lives forever.\n\n" +

        "As the days turned into weeks, Emma found herself drawn to Nathan in a way she had never experienced before. He made " +
        "her laugh, challenged her views, and encouraged her to see the world in a different light. She found herself opening up " +
        "to him in ways she never had with anyone before. But even as their connection grew stronger, Emma struggled with the " +
        "fear that love might complicate the life she had so carefully built.\n\n" +

        "One evening, while walking through a park, Nathan confessed that he had fallen in love with her. Emma froze. The words " +
        "felt like a heavy weight on her chest. She had been afraid of this moment, afraid of what it would mean for her future. " +
        "She told him that she needed time to think, not because she didn’t feel the same, but because she was terrified of " +
        "what their love might cost her.\n\n" +

        "Days passed, and Emma wrestled with her emotions. She knew Nathan was special, but was she ready to give up the control " +
        "she had over her life? Then, on a quiet night under the stars, Nathan appeared at her door, holding a small bouquet of " +
        "wildflowers. With a gentle smile, he said, 'I don’t want to change your life, Emma. I just want to be a part of it.' " +
        "Her heart swelled with emotion, and in that moment, she realized that she had already given him her heart.\n\n" +

        "From that night on, Emma and Nathan built a life together—one filled with love, laughter, and shared dreams. Emma " +
        "finally understood that love didn’t have to be an obstacle; it was the one thing that made life worth living. They " +
        "traveled the world, faced challenges, and grew together, hand in hand. And as they looked toward the future, Emma " +
        "couldn’t imagine a life without him by her side.",

        // Story 3: Detective Fiction
        "                     THE MYSTERY OF THE SILENT STREETS\n\n" +
        "Detective James Carter paced the darkened alley, his eyes scanning the shadows for any signs of movement. The city had " +
        "been quiet for weeks, almost too quiet. There had been a string of disappearances, and the police were stumped. People " +
        "were vanishing without a trace, and Carter knew that it wasn’t just a coincidence. Something darker was at play. " +
        "He had seen enough crime scenes in his life to recognize when something didn’t add up.\n\n" +

        "His investigation led him to an old, abandoned building on the edge of the city. The building had been empty for years, " +
        "but something about it seemed off. As Carter pushed through the broken door, he felt a chill run down his spine. The " +
        "walls were lined with strange symbols, and the air smelled stale, like something had been trapped inside for too long. " +
        "He ventured deeper into the building, his flashlight illuminating the path ahead.\n\n" +

        "In one of the rooms, Carter found something that made his blood run cold: a collection of photographs, all depicting " +
        "people who had disappeared in the last month. They were all arranged on a wall, each one marked with a strange, red " +
        "X. But what caught Carter's attention the most was the final photograph – it was of him. His own face stared back at him, " +
        "frozen in time, with an X drawn across it.\n\n" +

        "Panicked, Carter rushed out of the building, but the streets outside seemed to close in around him. Everywhere he turned, " +
        "he saw the same strange symbols. It was as if the entire city was part of a grand conspiracy, and he was the next target. " +
        "Determined to get to the bottom of it, Carter continued his search, digging through city records and speaking to informants " +
        "until he uncovered a hidden truth: an ancient secret society had been operating in the shadows for centuries, and they " +
        "were using the disappearances to gather power.\n\n" +

        "In a final showdown, Carter confronted the leader of the society, an elusive figure known only as The Architect. In a " +
        "battle of wits, Carter outsmarted The Architect and exposed the society’s plans. The missing people were returned, " +
        "but the mystery of the silent streets lingered. Carter had solved the case, but in the process, he had uncovered a " +
        "dark truth that would haunt him forever. The city had secrets—secrets that no one, not even a seasoned detective, " +
        "could ever fully understand."

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
                { 0, 381 }, // Story 1 word count
                { 1, 385  }, // Story 2 word count
                { 2, 406  }  // Story 3 word count
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

        private void WordCountHard_Load(object sender, EventArgs e)
        {
            textBox1.KeyPress += textBox1_KeyPress;
        }
    }
}
