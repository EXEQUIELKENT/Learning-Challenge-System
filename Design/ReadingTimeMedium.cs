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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using static System.Windows.Forms.LinkLabel;

namespace DCP
{
    public partial class ReadingTimeMedium : Form
    {
        private Timer timer;
        private int progressDuration = 720; // total time for progress bar in seconds
        private int timeRemaining;
        private SoundPlayer soundPlayer;
        private SoundPlayer click;
        private SoundPlayer fail;
        private SoundPlayer success;
        private SoundPlayer count;
        private bool isEnterKeyDisabled = false;
        public ReadingTimeMedium()
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
                // Challenge completed successfully when the time is up
                fail.Play();
                timer.Stop();
                progressBar1.Value = progressBar1.Maximum; // Set progress bar to max on completion
                textBox2.Text = TimeSpan.FromSeconds(progressDuration).ToString("hh\\:mm\\:ss"); // Set the final time

                // Save challenge data to JSON file
                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Medium)");

                MessageBox.Show("Challenge not completed. Returning to the homepage.", "Challenge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TransitionToHomePage();
            }
        }
        public void SaveChallengeDataSuccess(string username, string status, string time, string formTitle)
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
                FormTitle = formTitle,
                Challenge = status, // New property for jogging
                Time = time
            };

            challengeList.Add(newChallengeData);

            // Save back to the JSON file
            var updatedChallengeData = JsonConvert.SerializeObject(challengeList, Formatting.Indented);
            File.WriteAllText(challengeFilePath, updatedChallengeData);
        }
        public void SaveChallengeDataFailed(string username, string time, string formTitle)
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
                FormTitle = formTitle,
                Challenge = "Failed", // New property for jogging
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
            button1.Enabled = true;
            textBox2.Enabled = true;
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
                if (!(ActiveControl is RichTextBox))
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
                                // Save challenge data to JSON file
                                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Medium)");

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
        //Drama Story
        "                            A BROKEN HOME\n\n" +

        "Marianne grew up in a happy home where laughter filled the halls and family dinners were a cherished tradition. " +
        "However, her world crumbled when her father suddenly left without explanation, leaving her mother to raise Marianne " +
        "and her three younger siblings alone. The once lively home grew quiet, replaced by the hum of her mother’s sewing machine " +
        "as she worked tirelessly to provide for the family.\n\n" +

        "Struggling to make ends meet, Marianne stepped up to help. She juggled school and part-time jobs, often putting her own " +
        "dreams aside to ensure her siblings were cared for. Over time, resentment toward her father grew, fueled by unanswered questions " +
        "and the sight of her mother’s weary face. Despite her bitterness, Marianne became the glue holding her family together, " +
        "helping her siblings navigate the pain of their father’s absence.\n\n" +

        "Years passed, and Marianne became a strong yet guarded young woman. One day, while working at a café near her neighborhood, " +
        "she was stunned to see her father walk through the door. His once-confident demeanor had been replaced with a look of remorse. " +
        "He asked for a chance to talk, revealing that his departure had been driven by struggles with mental health and a sense of inadequacy " +
        "he couldn’t face. His vulnerability left Marianne torn between anger and a deep-seated yearning for closure.\n\n" +

        "Conflicted, Marianne confided in her siblings, who encouraged her to consider forgiveness. They shared their own feelings, " +
        "each processing their father’s absence differently. Yet, Marianne felt the heaviest burden, having sacrificed so much for the family. " +
        "A late-night conversation with her mother provided a turning point. Her mother spoke of the pain of abandonment but also of the healing " +
        "power of forgiveness—not for her father’s sake but for their own peace.\n\n" +

        "Determined to confront her emotions, Marianne agreed to meet her father. Their conversation was raw and emotional, with Marianne " +
        "expressing her hurt and demanding accountability. Her father listened, acknowledging his failures and promising to earn her trust. " +
        "That moment marked the start of a tentative reconciliation, one built on honesty and shared effort. " +
        "Though the wounds were deep, Marianne found herself slowly letting go of the anger she had carried for years.\n\n" +

        "Their family began a journey of rebuilding, one small step at a time. Her father worked to be present, showing up for milestones " +
        "he had missed and rebuilding relationships with his children. The scars of the past remained, but the family chose to focus on " +
        "what lay ahead rather than what was lost. For Marianne, the journey wasn’t about forgetting but learning to heal and move forward together.",



        // Manga Story
        "                            BORUTO: THE NEXT GENERATION\n\n" +

        "Boruto Uzumaki, the son of the legendary Hokage Naruto, struggles with living in his father’s shadow. " +
        "Despite his immense talent, Boruto's rebellious streak and desire to prove himself often lead to tension at home. " +
        "He feels the weight of expectations from the village and resentment toward the attention Naruto gives to his role as Hokage " +
        "over their family. This creates a rift, leaving Boruto yearning to be seen for who he truly is.\n\n" +

        "During a lively village festival celebrating the anniversary of peace, chaos erupts as masked intruders launch a surprise attack. " +
        "The attackers seem to possess strange, unfamiliar techniques, catching even the seasoned shinobi off guard. " +
        "Boruto, along with his teammates Sarada Uchiha and Mitsuki, leaps into action, determined to protect the villagers. " +
        "The trio’s quick thinking and synergy save lives, but it becomes clear that the attack is only the beginning of a larger conspiracy.\n\n" +

        "Digging deeper into the incident, Boruto and his friends uncover clues leading to a mysterious rogue clan with ties to the Hidden Leaf's past. " +
        "This clan, harboring a grudge against the village, seeks to dismantle the peace Naruto and his allies worked so hard to establish. " +
        "Their investigation reveals shocking truths about the clan’s motives and a dangerous weapon capable of threatening the entire shinobi world. " +
        "Boruto realizes that this threat isn’t just a challenge for him—it’s a test of his ability to step into a leadership role.\n\n" +

        "As the rogue clan's plans unfold, Boruto faces increasingly formidable adversaries, forcing him to dig deep into his skills and resolve. " +
        "His growth as a shinobi becomes evident as he learns the value of teamwork, not just with Sarada and Mitsuki, but also with his father. " +
        "Naruto and Boruto clash over methods but eventually find common ground, deepening their bond as they fight together for the village.\n\n" +
        "The battles are intense, pushing Boruto to his limits. During one climactic fight, Boruto taps into a hidden power, " +
        "blending his unique talents with the lessons he’s learned from Naruto. This moment of triumph not only turns the tide of the battle " +
        "but also earns him the respect of those who doubted his abilities. Boruto begins to understand the weight of his heritage and the legacy " +
        "he carries, realizing that being Naruto’s son is both a challenge and a privilege.\n\n" +

        "In the aftermath, Boruto is celebrated as a hero, but he remains humble, knowing there’s still much to learn. " +
        "Rather than mimicking his father’s path, he decides to forge his own, focusing on innovation and unity. " +
        "The experience solidifies Boruto’s commitment to protecting the village, not just because of his lineage, but because it’s his own choice. " +
        "The journey transforms Boruto into a true leader and sets the stage for the next chapter of the Hidden Leaf Village’s legacy.",

        // Personal Development
        "                            FITNESS FOR BEGINNER\n\n" +

        "Starting a fitness journey can feel daunting, but taking small, manageable steps is the key to success. " +
        "Begin with warm-up exercises such as light jogging, jumping jacks, or dynamic stretches to prepare your body. " +
        "These activities not only improve flexibility but also reduce the risk of injury. Aim for at least 20-30 minutes " +
        "of movement every day to establish a routine and build endurance.\n\n" +

        "Strength training is essential for building muscle and boosting metabolism. " +
        "Begin with simple bodyweight exercises like push-ups, squats, and planks. Focus on mastering proper form before " +
        "progressing to more challenging variations or adding weights. This ensures safety and maximizes results. Over time, " +

        "increasing the intensity or duration of your workouts helps maintain steady progress.\n\n" +
        "Cardiovascular exercises, such as walking, cycling, or swimming, improve heart health and burn calories. " +
        "Find an activity you enjoy to make it easier to stick with your routine. Start with short sessions, " +
        "then gradually increase the duration as your fitness level improves. Consistency is more important than intensity " +
        "at the beginning of your journey.\n\n" +

        "Nutrition plays a crucial role in achieving your fitness goals. Focus on balanced meals that include lean proteins, " +
        "fresh vegetables, healthy fats, and whole grains. Avoid processed foods and sugary snacks whenever possible. " +
        "Drink plenty of water throughout the day to stay hydrated, especially before, during, and after exercise. " +
        "Proper hydration supports energy levels and aids in recovery.\n\n" +

        "Tracking your progress can provide motivation and accountability. Keep a journal of your workouts, meals, and how you feel each day. " +
        "Noticing improvements, such as increased strength or better stamina, reinforces your commitment to the journey. " +
        "Set realistic, achievable goals to celebrate milestones along the way.\n\n" +

        "Rest and recovery are as important as the workouts themselves. Ensure you get enough sleep to allow your body to heal and grow stronger. " +
        "Incorporate rest days into your routine to prevent burnout and injuries. " +
        "Remember, fitness is not a race but a lifelong commitment to your health and well-being. " +
        "Embrace the process and enjoy the positive changes in your body and mindset.",

        };


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
                        // Save challenge data to JSON file
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Medium)");

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
                pictureBox2.Enabled = false;
                pictureBox3.Enabled = false;
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
                // Challenge completed successfully
                timer.Stop();
                success.Play();
                progressBar1.Value = progressBar1.Maximum; // Set progress bar to max
                textBox2.Text = TimeSpan.FromSeconds(progressDuration - timeRemaining).ToString("hh\\:mm\\:ss"); // Update final time

                SaveChallengeDataSuccess(Login.CurrentUsername, "Completed", textBox2.Text, "Reading Time (Medium)");

                MessageBox.Show("Challenge Complete. Returning to the homepage.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TransitionToHomePage();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
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
                        // Save challenge data to JSON file
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Medium)");

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
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplayContent(currentIndex);
            }
            else
            {
                MessageBox.Show("This is the first story!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (currentIndex < contentList.Count - 1)
            {
                currentIndex++;
                DisplayContent(currentIndex);
            }
            else
            {
                MessageBox.Show("This is the last story!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ReadingTimeMedium_Load(object sender, EventArgs e)
        {
            InitializeContent();
            DisplayContent(0); // Display the first item
        }
        private void EssayrichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
