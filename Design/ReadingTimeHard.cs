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
    public partial class ReadingTimeHard : Form
    {
        private Timer timer;
        private int progressDuration = 420; // total time for progress bar in seconds
        private int timeRemaining;
        private SoundPlayer soundPlayer;
        private SoundPlayer click;
        private SoundPlayer fail;
        private SoundPlayer success;
        private SoundPlayer count;
        private bool isEnterKeyDisabled = false;
        public ReadingTimeHard()
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
                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Hard)");

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
                                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Hard)");

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
        //Thriller Story
        "                            THE VANISHING\n\n" +

        "Detective Claire Monroe was assigned to solve a series of mysterious disappearances in a remote, fog-shrouded town. " +
        "Every victim vanished without a trace, leaving behind eerie symbols carved into their homes—symbols that seemed to pulse with a dark energy. " +
        "The locals whispered about ancient curses that plagued the town, but Claire, a woman of reason and logic, refused to believe in superstitions. " +
        "She relied solely on evidence, determined to uncover the truth behind the vanishing victims and the growing sense of dread hanging over the town.\n\n" +

        "Her investigation led her to the outskirts of the town, to an abandoned mine once used for mining silver but long since shut down. " +
        "The mine, once a bustling center of industry, now stood as a silent monument to something far darker. " +
        "Inside the depths of the mine, Claire uncovered strange artifacts—dusty relics that seemed to pulse with an unnatural energy. " +
        "The artifacts bore symbols identical to those found on the victims' homes. It was clear that the disappearances were connected to the mine, and the rituals that had been practiced there long ago.\n\n" +

        "As Claire delved deeper into the mysteries surrounding the mine, she began to feel an unsettling presence, a sense that she was being watched. " +
        "Shadows seemed to move at the edges of her vision, and whispers echoed through the dark tunnels, though there was no one there. " +
        "Someone, or something, was watching her every move, warning her to stop her investigation. " +
        "Undeterred, Claire pushed forward, piecing together fragments of cryptic symbols and ancient texts, all of which pointed to the existence of a secret society operating in the heart of the town.\n\n" +
    
        "With each revelation, the town's true horror unfolded. Claire discovered that the secret society, known only as The Keepers, had been performing occult rituals for centuries. " +
        "Their aim was to summon an ancient and powerful entity from another realm, one they believed would grant them immortality in exchange for a blood sacrifice. " +
        "The missing victims were not random—they were chosen as sacrifices to the entity, their lives offered to ensure the society’s dark power would be sustained. " +
        "Claire’s heart raced as she realized the full scope of the danger she was facing. The society’s influence ran deep, woven into the very fabric of the town.\n\n" +

        "Desperate to stop the ritual, Claire infiltrated the society's inner circle. " +
        "She disguised herself and attended their midnight gathering, where a ritual was being prepared to bring the entity into the physical world. " +
        "With every passing moment, the atmosphere grew thick with a malevolent energy, and Claire felt herself losing control of her thoughts. " +
        "Her mind screamed for her to run, but she knew she had to stop them, or the town would fall into darkness. As the ritual reached its crescendo, Claire acted quickly, " +
        "disrupting the ceremony by destroying the sacred artifacts that powered the summoning. The ground trembled as the entity began to stir in the depths of the town.\n\n" +

        "Chaos erupted. The Keepers, enraged by the disruption, turned on Claire with a fury that was almost inhuman. " +
        "She fought back, using her knowledge of the occult to turn the tables, but the leader of the society, a tall, shadowy figure known as Elder Blackwood, proved to be her greatest adversary. " +
        "The battle between them was fierce, with Claire barely able to hold her ground against his supernatural strength and the sinister power of the entity that lurked in the shadows. " +
        "At the last moment, Claire summoned the courage to confront the elder head-on, using a ritual she had learned to banish the entity back to its realm and seal the society’s powers once and for all.\n\n" +

        "Though Claire succeeded in her mission, the victory came at a heavy cost. " +
        "As the sun rose over the town, Claire stood amidst the ruins of the secret society, their leaders arrested and their dark influence eradicated. " +
        "The town began to heal, but Claire could not shake the deep, lingering sense of unease that now followed her. " +
        "She knew the darkness she had uncovered was not just a relic of the past; it was a force that had been nurtured within the very souls of the people. " +
        "The knowledge that true evil often lurks in the hearts of humans haunted her every step, and no amount of light could fully banish the shadows that had taken root within her mind.\n\n" +

        "Claire left the town, but the experience had changed her forever. " +
        "Her resolve had been tested in ways she never expected, and the faces of the victims would stay with her always. " +
        "As she returned to her work, the knowledge of the darkness within the world was never far from her thoughts. " +
        "The mysteries of the vanished had been solved, but Claire now understood that some questions, once asked, could never be truly answered.",


        // Manga Story
        "                            ONE PIECE: THE GRAND QUEST\n\n" +

        "Luffy and his Straw Hat Pirates continue their journey across the unpredictable Grand Line, searching for the legendary treasure, One Piece. " +
        "With each island they visit, the crew grows closer and stronger, facing enemies that test their limits. " +
        "Their dream of ultimate freedom fuels their unbreakable resolve to overcome any obstacle.\n\n" +

        "One day, their Log Pose guides them to a strange and desolate island shrouded in eternal twilight. " +
        "As they dock, they notice time moves differently here—days seem like hours, and hours stretch into eternity. " +
        "A mysterious figure named Kael, known as 'The Chrono Tyrant,' emerges to confront them. Kael possesses a devil fruit power that allows him to manipulate time, " +
        "freezing and accelerating moments at will. His island is a labyrinth of distorted reality, designed to confuse and disorient intruders.\n\n" +

        "Separated by Kael's powers, the Straw Hats find themselves trapped in different pockets of frozen time. " +
        "Zoro battles against phantom enemies from his past, while Nami struggles with storms that defy logic. " +
        "Sanji faces illusions of those he couldn’t protect, and Usopp must overcome fears amplified by Kael's manipulations. " +
        "Luffy, meanwhile, confronts Kael directly, refusing to let anyone stand in the way of his dream.\n\n" +

        "Kael reveals that he was once a pirate who sought the One Piece but lost everything to its pursuit. " +
        "He claims the treasure isn’t worth the suffering it brings, warning Luffy to abandon his quest. " +
        "Unfazed, Luffy declares that the journey itself—the friends he’s made, the adventures they’ve shared—is worth any risk. " +
        "His unwavering determination ignites hope in his scattered crew, who begin breaking free from Kael’s illusions.\n\n" +

        "Franky discovers the island hides an ancient mechanism tied to Kael’s powers, a relic left by the Void Century. " +
        "With Robin’s knowledge of the Poneglyphs, they realize this relic holds not only a clue to the One Piece but also a deeper secret about the Pirate King’s legacy. " +
        "The crew works together to dismantle Kael’s control over the island, but doing so risks unleashing chaotic temporal energy.\n\n" +

        "As the final battle unfolds, Kael uses his full strength to trap Luffy in a time loop, forcing him to relive the same moments endlessly. " +
        "Drawing on his bond with his crew, Luffy breaks free through sheer willpower and a creative use of his Gear Fifth form. " +
        "In a climactic clash, Luffy defeats Kael, shattering the relic and freeing the island from its temporal curse.\n\n" +

        "Kael, weakened and defeated, shares his last words with Luffy. He reveals that the One Piece is more than just a treasure—it’s a revelation about the world’s history " +
        "that will shake the very foundations of the current order. He warns Luffy of an even greater threat looming beyond the Marines and the Yonko: " +
        "an ancient force that has waited centuries to rise again. With his dying breath, Kael entrusts Luffy with the remaining piece of the map to the One Piece.\n\n" +

        "Though victorious, the Straw Hats are left with heavy hearts, knowing the dangers ahead will only intensify. " +
        "With renewed determination, they set sail once more, ready to face whatever challenges the Grand Line has in store. " +
        "Together, they embrace their unyielding quest, knowing their journey is as important as the treasure that awaits at its end.",

        // Personal Development (Hard)
        "                            THE SCIENCE OF SUCCESS\n\n" +

        "Success is not just a stroke of luck; it’s a deliberate combination of discipline, knowledge, and resilience. " +
        "The road to achieving anything meaningful in life requires intentional effort, and the first step is setting clear goals. " +
        "Start by setting SMART goals—Specific, Measurable, Achievable, Relevant, and Time-bound. " +
        "These goals provide clarity, focus, and motivation, serving as a guidepost to tackle even the most complex challenges. " +
        "When your goals are well-defined, it’s easier to break them down into manageable steps and avoid feeling overwhelmed.\n\n" +

        "To achieve success, you must also understand the science of decision-making. Cognitive psychology plays a significant role in how we approach problems and make choices. " +
        "Learning how the brain processes information helps avoid cognitive biases, such as confirmation bias or overconfidence bias, which can cloud judgment. " +
        "Improving critical thinking skills through education, reading books, or taking courses on neuroscience and human behavior equips you with tools to make better decisions. " +
        "By recognizing the mental shortcuts our brains take, you can train yourself to pause, think critically, and evaluate all available options before acting, ensuring smarter and more informed choices.\n\n" +

        "Effective time management is another critical factor in achieving success. " +
        "Without proper time management, even the most talented individuals can struggle to reach their full potential. " +
        "Use techniques like the Pomodoro Technique, which involves working in focused intervals, or the Eisenhower Matrix, which helps you prioritize tasks based on urgency and importance. " +
        "These tools allow you to allocate your time efficiently, ensuring that you focus on what truly matters. " +
        "However, it's essential to balance productivity with rest—taking breaks to recharge prevents burnout, enhances focus, and increases overall efficiency. " +
        "Remember, success isn’t about working harder but working smarter.\n\n" +

        "Additionally, embrace failure as a crucial part of the learning process. " +
        "Failure is not the end, but a stepping stone on the path to success. Great scientists, entrepreneurs, and innovators have all faced setbacks, but they didn’t let them stop their progress. " +
        "Instead of viewing failure as a negative experience, they saw it as an opportunity for growth and improvement. " +
        "Failure teaches resilience, as it forces us to analyze what went wrong, adjust our strategies, and come back stronger. " +
        "With a growth mindset, where you see challenges as opportunities to learn and develop, any goal becomes achievable, no matter how daunting it may seem at first.\n\n" +

        "Persistence is key when it comes to success. Even when the road gets tough, those who keep going are the ones who eventually succeed. " +
        "Persistence is about staying committed to your goals, even when immediate results aren’t visible. " +
        "It’s about showing up every day, learning from mistakes, and adapting your approach as needed. " +
        "Success doesn’t come overnight, and the journey is often filled with obstacles. " +
        "But those who continue, despite the challenges, will find their perseverance eventually paying off. " +
        "By developing resilience and persistence, you equip yourself with the mental strength to keep moving forward, no matter how difficult the journey may become.\n\n" +

        "Moreover, building a supportive network is essential. " +
        "Surround yourself with individuals who inspire, motivate, and challenge you. " +
        "Having a strong support system, whether it’s mentors, peers, or friends, helps you stay accountable and gain new perspectives. " +
        "Networking with like-minded individuals opens doors to opportunities you may not have otherwise encountered, and it helps you grow personally and professionally. " +
        "Great success rarely happens in isolation—collaboration and support from others can accelerate your progress. " +
        "Take time to nurture relationships that will add value to your life, as the people you surround yourself with can influence your trajectory.\n\n" +

        "Finally, it's important to take care of your well-being. " +
        "Success can be hollow if it comes at the cost of your mental and physical health. " +
        "Take the time to eat well, exercise regularly, and practice mindfulness to keep your body and mind in optimal condition. " +
        "A healthy body fuels a clear mind, and a clear mind is necessary for making sound decisions, staying focused, and overcoming obstacles. " +
        "By maintaining a balance between work and self-care, you ensure that you have the energy and motivation to keep pushing toward your goals.",

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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Hard)");

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

                SaveChallengeDataSuccess(Login.CurrentUsername, "Completed", textBox2.Text, "Reading Time (Hard)");

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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Reading Time (Hard)");

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

        private void ReadingTimeHard_Load(object sender, EventArgs e)
        {
            InitializeContent();
            DisplayContent(0); // Display the first item
        }
        private void EssayrichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
