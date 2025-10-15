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


namespace DCP
{
    public partial class ReadingTimeEasy : Form
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
        public ReadingTimeEasy()
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
                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Easy)");

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
                                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Easy)");

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
        //Love Story
        "                            FORGOTTEN PROMISES\n\n" +

        "Clara and David were childhood friends who vowed to stay together no matter what. Life, however, had different plans. " +
        "After high school, David moved to the city to pursue his dreams, leaving Clara behind in their small town. " +
        "Before he left, they exchanged heartfelt letters, promising to write and visit, but as time passed, the letters stopped coming.\n\n" +

        "Years passed, and Clara found herself running her family's bookstore, trying to forget the boy who once promised forever. " +
        "The store was her sanctuary, a place where she buried herself in work to avoid thinking about the past. " +
        "One quiet afternoon, while organizing shelves, she heard the doorbell chime and turned to see David standing there. " +
        "He had changed, now a successful writer, but his familiar smile made her heart skip a beat.\n\n" +

        "David explained his long absence, apologizing for breaking his promise. He admitted that the city and his career consumed him, " +
        "but he had never stopped thinking about her. The bookstore, filled with memories of their childhood, brought back feelings " +
        "he thought he had lost. Clara, hurt yet curious, agreed to meet him for coffee to talk things through.\n\n" +

        "Over coffee, they reminisced about their youth, laughing at shared memories and discussing their separate journeys. " +
        "Clara realized that her feelings for David had never truly faded, but she struggled with trusting him again. " +
        "David assured her of his intentions, revealing that his return to the town wasn’t just for nostalgia—he wanted to rebuild their bond. " +
        "His vulnerability struck a chord in Clara’s heart, making her question her guarded emotions.\n\n" +

        "Days turned into weeks of heartfelt conversations and small gestures of reconciliation. " +
        "David helped Clara at the bookstore, reigniting the spark of their shared dreams. Clara decided to take a leap of faith, " +
        "believing in second chances. Slowly, their connection grew stronger, built on honesty and mutual respect.\n\n" 
        +
        "Together, they transformed their regrets into hope, creating a new chapter of their lives. " +
        "Their rekindled love story was not about picking up where they left off but about creating something even more beautiful. " +
        "Hand in hand, they faced their future, proving that some promises, though forgotten, could still find their way back home.",

        // Manga Story
        "                            DEMON SLAYER\n\n" +

        "Tanjiro Kamado, a kind-hearted and hardworking boy, lives a simple yet fulfilling life in the mountains with his loving family. " +
        "Every day, he supports them by selling charcoal in the nearby village, dreaming of a peaceful future. " +
        "However, his world is shattered one fateful day when he returns home to find his family brutally attacked by demons. " +
        "Only his sister, Nezuko, survives the massacre, but she has been transformed into a demon herself.\n\n" +

        "Despite her transformation, Nezuko shows signs of retaining her humanity, refusing to harm Tanjiro. " +
        "Determined to save her and avenge his family, Tanjiro seeks out a mysterious mentor, Sakonji Urokodaki, " +
        "who trains him to become a skilled demon slayer. Under Urokodaki’s guidance, Tanjiro learns to wield a sword " +
        "and masters the Water Breathing technique, a powerful combat style that allows him to face deadly demons.\n\n" +

        "Tanjiro embarks on a perilous journey with Nezuko by his side. Together, they encounter both allies and foes, " +
        "including other demon slayers and malevolent demons with terrifying abilities. Each battle tests Tanjiro’s courage, " +
        "resilience, and unwavering determination to protect his sister and innocent lives. Along the way, they uncover " +
        "hints about Muzan Kibutsuji, the sinister leader of the demons and the key to curing Nezuko.\n\n" +

        "As their journey progresses, Tanjiro and Nezuko forge strong bonds with new companions like Zenitsu, " +
        "a timid yet talented fighter, and Inosuke, a fierce warrior with a wild spirit. Their shared struggles and victories " +
        "strengthen their resolve to stand against the growing darkness. Together, they face increasingly powerful demons, " +
        "each with unique abilities and tragic pasts, revealing the complexity of their enemies.\n\n" +

        "Through his kindness and empathy, Tanjiro discovers that even demons were once human, often shaped by despair and suffering. " +
        "This understanding fuels his resolve not only to defeat them but also to honor the humanity they once had. " +
        "Tanjiro's journey is not just about vengeance but about bringing hope and light to a world overshadowed by evil.\n\n" +

        "With every battle, Tanjiro grows stronger and closer to his ultimate goal: finding a cure for Nezuko " +
        "and eradicating the demon scourge. Their path is fraught with danger, but Tanjiro’s unbreakable spirit " +
        "and the bond with his sister drive him forward, proving that even in the darkest times, love and hope can prevail.",


        // Personal Development
        "                            BASKETBALL SKILL\n\n" +

        "Dribbling is one of the most fundamental skills in basketball and serves as the foundation for controlling the game. " +
        "Begin by practicing basic dribbling techniques, such as bouncing the ball alternately with both hands while keeping your head up. " +
        "This helps you maintain awareness of the court and develop confidence. Gradually progress to more advanced drills, " +
        "like crossovers and between-the-legs dribbles, which improve ball handling and agility.\n\n" +

        "Passing is another essential skill that fosters teamwork and ensures fluid gameplay. " +
        "Start with chest passes by holding the ball with both hands and pushing it forward to a teammate's chest level. " +
        "Practice bounce passes as well, where the ball hits the floor before reaching your teammate. " +
        "These techniques enhance accuracy and communication, which are critical during fast-paced games.\n\n" +

        "Shooting is the skill that often defines success in basketball. Begin close to the basket to focus on your form and accuracy. " +
        "Practice proper hand placement, foot positioning, and follow-through with each shot. " +
        "Gradually extend your range, working on layups, mid-range shots, and eventually three-pointers as your confidence grows. " +
        "Consistency is key to becoming a reliable scorer.\n\n" +

        "Defense is just as important as offense. Work on maintaining a low stance with your knees bent and your hands active. " +
        "Practice defensive slides to improve lateral quickness, allowing you to guard opponents effectively and prevent easy baskets.\n\n" +

        "Rebounding is another crucial skill that can turn the tide of a game. Focus on positioning yourself near the basket and timing your jump. " +
        "Learn to box out opponents by using your body to create space, ensuring your team gains possession of missed shots.\n\n" +

        "Finally, develop your overall fitness and coordination through exercises like sprinting, jumping drills, and strength training. " +
        "A strong, well-rounded skill set combined with physical endurance and mental focus lays the foundation for excelling in basketball. " +
        "With regular practice and determination, you can build a solid skill base and advance to higher levels of play.",


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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Easy)");

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

                SaveChallengeDataSuccess(Login.CurrentUsername, "Completed", textBox2.Text, "Reading Time (Easy)");

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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Easy)");

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

        private void ReadingTimeEasy_Load(object sender, EventArgs e)
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
