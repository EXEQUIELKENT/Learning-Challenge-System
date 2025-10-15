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

namespace DCP.Resources
{
    public partial class CharacterAnalysisMedium : Form
    {
        private Timer timer;
        private int progressDuration = 600; // total time for progress bar in seconds
        private int timeRemaining;
        private SoundPlayer soundPlayer;
        private SoundPlayer click;
        private SoundPlayer fail;
        private SoundPlayer success;
        private SoundPlayer count;
        private bool isEnterKeyDisabled = false;
        private List<Question> questions;
        private int currentIndex = 0;
        private int score = 0;
        private List<bool> answeredQuestions;
        private List<string> answeredAnswers;
        private string selectedStory;
        public class Question
        {
            public string QuestionText { get; set; }
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string CorrectAnswer { get; set; }
        }
        public CharacterAnalysisMedium()
        {
            InitializeComponent();

            Random rand = new Random();
            int selectedStoryIndex = rand.Next(0, stories.Length);
            EssayrichTextBox1.Text = stories[selectedStoryIndex];

            // Load the corresponding quiz based on the selected story
            if (selectedStoryIndex == 0) // Jujutsu Kaisen
            {
                questions = new List<Question>(questions1);
            }
            else if (selectedStoryIndex == 1) // Superhero
            {
                questions = new List<Question>(questions2);
            }
            else // Animal
            {
                questions = new List<Question>(questions3);
            }

            // Shuffle and pick 5 random questions
            questions = questions.OrderBy(q => rand.Next()).Take(10).ToList();

            // Initialize answered questions
            answeredQuestions = new List<bool>(new bool[questions.Count]);
            answeredAnswers = new List<string>();
            for (int i = 0; i < questions.Count; i++)
            {
                answeredAnswers.Add(""); // Default empty answers
            }

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
            pictureBoxA.Enabled = false;
            pictureBoxB.Enabled = false;
            pictureBoxC.Enabled = false;
            pictureBoxD.Enabled = false;
            pictureBox2.Enabled = false;
            pictureBox3.Enabled = false;
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
                success.Play();
                timer.Stop();
                progressBar1.Value = progressBar1.Maximum; // Set progress bar to max on completion
                textBox2.Text = TimeSpan.FromSeconds(progressDuration).ToString("hh\\:mm\\:ss"); // Set the final time

                // Save challenge data to JSON file
                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, score);

                MessageBox.Show("Challenge not completed. Returning to the homepage.", "Challenge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TransitionToHomePage();
            }
        }
        public void SaveChallengeDataSuccess(string username, string status, string time, int score)
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
                FormTitle = "Character Anlysis (Medium)",
                Score = $"Completed {score}", // New property for jogging
                Time = time
            };

            challengeList.Add(newChallengeData);

            // Save back to the JSON file
            var updatedChallengeData = JsonConvert.SerializeObject(challengeList, Formatting.Indented);
            File.WriteAllText(challengeFilePath, updatedChallengeData);
        }
        public void SaveChallengeDataFailed(string username, string time, int score)
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
                FormTitle = "Character Analysis (Medium)",
                Score = $"Failed {score}", // New property for jogging
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
            DisplayQuestion(currentIndex);
            pictureBox2.Enabled = true;
            pictureBox3.Enabled = true;
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
                                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, score);

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
        private void DisplayQuestion(int index)
        {
            if (index >= 0 && index < questions.Count)
            {
                Question question = questions[index];
                EssayrichTextBox1.Text = $"{index + 1}. {question.QuestionText}\n\n" +
                                    $"A. {question.A}\n\n" +
                                    $"B. {question.B}\n\n" +
                                    $"C. {question.C}\n\n" +
                                    $"D. {question.D}";

                // Disable buttons if the question has already been answered
                pictureBoxA.Enabled = !answeredQuestions[index];
                pictureBoxB.Enabled = !answeredQuestions[index];
                pictureBoxC.Enabled = !answeredQuestions[index];
                pictureBoxD.Enabled = !answeredQuestions[index];
            }
        }
        private void AnswerButton_Click(object sender, EventArgs e)
        {
            PictureBox clickedButton = sender as PictureBox;

            if (clickedButton != null && currentIndex < questions.Count)
            {
                Question question = questions[currentIndex];
                string selectedAnswer = "";

                // Map the clicked PictureBox to the appropriate answer
                if (clickedButton == pictureBoxA) selectedAnswer = "A";
                if (clickedButton == pictureBoxB) selectedAnswer = "B";
                if (clickedButton == pictureBoxC) selectedAnswer = "C";
                if (clickedButton == pictureBoxD) selectedAnswer = "D";

                // Update score if the answer changes
                if (answeredQuestions[currentIndex])
                {
                    // Subtract previous score if the prior answer was correct
                    if (answeredAnswers[currentIndex] == question.CorrectAnswer)
                    {
                        score--;
                    }
                }

                // Update the user's answer
                answeredAnswers[currentIndex] = selectedAnswer;

                // Add to score if the new answer is correct
                if (selectedAnswer == question.CorrectAnswer)
                {
                    score++;
                }

                // Mark the current question as answered
                answeredQuestions[currentIndex] = true;

                do
                {
                    currentIndex++;
                }
                while (currentIndex < questions.Count && answeredQuestions[currentIndex]);

                if (currentIndex < questions.Count)
                {
                    DisplayQuestion(currentIndex);
                }


                // Check if all questions are answered
                if (AllQuestionsAnswered())
                {
                    MessageBox.Show("All questions have been answered.", "Answers Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private bool AllQuestionsAnswered()
        {
            foreach (bool answered in answeredQuestions)
            {
                if (!answered)
                {
                    return false;
                }
            }
            return true;
        }
        private string[] stories = {
        // Anime Story
        "                       THE DISTANT VOICE\n\n" +
        "Amelia and Theo had always been close, even though they lived on opposite sides of the world. Amelia " +
        "was a brilliant architect in New York, while Theo was a passionate marine biologist in Australia. They " +
        "maintained their bond through late-night phone calls and messages, supporting each other through the highs " +
        "and lows of their respective careers. Despite the distance, their friendship never wavered.\n\n" +

        "One day, Theo discovered a mysterious sea creature during his research—a creature that could communicate " +
        "through sound waves. Fascinated, he shared his findings with Amelia, who became equally intrigued. They began " +
        "working together, despite the time zone differences, combining her architectural knowledge with his marine " +
        "biology expertise to build an underwater research station.\n\n" +

        "However, their collaboration wasn't without its challenges. The time zone difference made their work hours " +
        "misaligned, and their communication, though constant, started to strain as they faced technical problems. " +
        "Amelia often felt like she was losing touch with the project and Theo. Their long-distance bond, which had " +
        "once seemed unbreakable, was now being tested in ways they hadn't anticipated.\n\n" +

        "As they struggled to maintain the project, Theo started experiencing mysterious phenomena in the ocean, " +
        "which seemed to be linked to the sea creature. He began questioning his sanity, and Amelia noticed the " +
        "worry in his voice. Determined to help, she decided to fly to Australia and join him in person, even if it " +
        "meant leaving everything behind.\n\n" +

        "Reunited, they overcame the creature's mystery together and successfully completed the underwater station. " +
        "Through their trials, they learned that even though distance may separate them physically, their bond would " +
        "always keep them connected. In the end, their love for each other and the shared project created a lasting " +
        "memory of their journey across the world.",
        // Superhero Story
        "                       THE CRYSTAL KINGDOM\n\n" +
        "In the kingdom of Eldoria, magic flowed freely through the land, nourishing its people. The kingdom was " +
        "ruled by a wise and just queen named Elara, who wielded the power of the Crystal Heart, a mystical gem " +
        "that granted her incredible abilities. Her rule brought prosperity, but dark forces from the north coveted " +
        "the Heart's power and sought to destroy Eldoria.\n\n" +

        "One day, a young orphan named Kael discovered a hidden relic that revealed the secret to unlocking an " +
        "ancient prophecy. The prophecy spoke of a hero born under the rare blood moon, who would either save or " +
        "destroy the Crystal Kingdom. Kael, unaware of his destiny, was drawn to the kingdom’s capital where " +
        "he unknowingly attracted the attention of both the kingdom’s allies and enemies.\n\n" +

        "Elara, sensing the growing threat, summoned Kael to the palace and revealed his true heritage: he was " +
        "the descendant of an ancient bloodline that had once served as guardians of the Crystal Heart. Kael was " +
        "reluctant to accept his role in the prophecy, but Elara explained that only he could stop the dark forces " +
        "from corrupting the Heart and plunging Eldoria into darkness.\n\n" +

        "Kael embarked on a perilous journey with his trusted companions, a mage named Lyra and a warrior named " +
        "Thorne, to find the lost Shards of the Crystal Heart before the enemy could. Along the way, they encountered " +
        "dangerous beasts, enchanted forests, and treacherous enemies who sought to prevent Kael from fulfilling " +
        "his destiny. Throughout it all, Kael grew stronger and more confident in his abilities.\n\n" +

        "In the final confrontation, Kael faced the dark sorcerer who sought to control the Crystal Heart. With " +
        "the power of the Shards and the guidance of his friends, Kael managed to defeat the sorcerer and restore " +
        "balance to the kingdom. As a true heir of Eldoria, Kael became the new protector of the Crystal Heart, " +
        "securing the future of the kingdom and ensuring that its magic would be used for the good of all.",
        // Animal Story
        "                       THE WALKING DEAD\n\n" +
        "In the post-apocalyptic world, survival was a daily battle. The outbreak of the undead had turned the " +
        "world upside down, leaving humanity on the brink of extinction. A small group of survivors, led by Lee " +
        "Everett, struggled to find safety in a world overrun by the undead. The group’s dynamic was fragile, as " +
        "every decision made in this world came with dire consequences.\n\n" +

        "Lee, a former history professor, took it upon himself to protect a young girl named Clementine, who had " +
        "been orphaned in the chaos. Their bond grew strong as they traveled together, seeking shelter and food while " +
        "trying to make sense of the world around them. Along the way, they met other survivors, some friendly, " +
        "others not so much.\n\n" +

        "One fateful night, Lee and Clementine encountered a group of hostile survivors who had turned to violence " +
        "to survive. Tensions rose, and Lee was forced to make a difficult choice: defend his group or risk the " +
        "lives of those he cared about. The consequences of his decisions weighed heavily on him, and the group was " +
        "forever changed.\n\n" +

        "As they journeyed through the wreckage of civilization, Lee’s leadership was put to the test as the group " +
        "faced a new threat—a ruthless band of raiders who would stop at nothing to get what they wanted. Lee had " +
        "to make difficult decisions once again, deciding whether to protect his group or seek a new path to safety.\n\n" +

        "In the end, Lee’s choices led to a tragic loss, but Clementine learned valuable lessons about survival, " +
        "trust, and the consequences of the choices we make. The group carried on, but Lee’s legacy lived on through " +
        "the people he helped, showing that even in a world filled with darkness, humanity’s spark could still shine."
        };

        private List<Question> questions1 = new List<Question>
        {
            new Question { QuestionText = "What were Amelia and Theo’s professions?", A = "Amelia was a tech expert, Theo was a reporter", B = "Amelia was a marine biologist, Theo was an architect", C = "Amelia was an architect, Theo was a marine biologist", D = "Amelia was a reporter, Theo was a tech expert", CorrectAnswer = "C" },
            new Question { QuestionText = "What did Theo discover during his research in the ocean?", A = "A new species of fish", B = "A sea creature that communicates through sound waves", C = "A rare coral reef", D = "A dangerous sea storm", CorrectAnswer = "B" },
            new Question { QuestionText = "What caused the strain in Amelia and Theo's collaboration?", A = "They didn’t have enough resources", B = "The time zone differences", C = "Their personalities clashed", D = "They were working on different projects", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Amelia decide to do to help Theo?", A = "Send him more research materials", B = "Visit him in Australia to join him in person", C = "Hire more researchers", D = "Give him space to figure it out on his own", CorrectAnswer = "B" },
            new Question { QuestionText = "Who helped Amelia and Theo complete the underwater research station?", A = "A group of engineers", B = "Timon and Pumbaa", C = "Theo’s new colleagues", D = "Their friendship and collaboration", CorrectAnswer = "D" },
            new Question { QuestionText = "Where did Theo and Amelia’s story primarily take place?", A = "In a city in America", B = "In the wilderness of Africa", C = "In the heart of the jungle", D = "In the ocean and Australia", CorrectAnswer = "D" },
            new Question { QuestionText = "What is the main lesson of The Distant Voice?", A = "Distance can break any bond", B = "The strongest relationships require no effort", C = "True bonds are formed through challenges and support", D = "Friendships fade over time", CorrectAnswer = "C" },
            new Question { QuestionText = "What was Amelia's initial reaction when Theo faced the mysterious sea creature?", A = "She was scared", B = "She was curious and intrigued", C = "She was skeptical", D = "She wanted to abandon the project", CorrectAnswer = "B" },
            new Question { QuestionText = "What were Amelia and Theo working together on?", A = "Building a lighthouse", B = "Designing a building", C = "Creating an underwater research station", D = "Exploring a new country", CorrectAnswer = "C" },
            new Question { QuestionText = "What did the long-distance nature of Amelia and Theo’s work teach them?", A = "The importance of patience and communication", B = "How to ignore time differences", C = "That distance always ruins relationships", D = "How to work independently", CorrectAnswer = "A" },
            new Question { QuestionText = "What was Theo’s main area of study?", A = "Space exploration", B = "Marine biology", C = "Physics", D = "Architecture", CorrectAnswer = "B" },
            new Question { QuestionText = "How did Amelia contribute to Theo’s research?", A = "She provided financial support", B = "She helped design an underwater research station", C = "She found a new species", D = "She trained divers for Theo’s team", CorrectAnswer = "B" },
            new Question { QuestionText = "What was unique about the sea creature Theo discovered?", A = "It glowed in the dark", B = "It could communicate through sound waves", C = "It was invisible", D = "It had human-like intelligence", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Amelia risk by traveling to Australia?", A = "Losing her job", B = "Getting lost in the ocean", C = "Forgetting her past", D = "Being attacked by the sea creature", CorrectAnswer = "A" },
            new Question { QuestionText = "How did Theo react when Amelia arrived in Australia?", A = "He was surprised and grateful", B = "He was angry", C = "He ignored her", D = "He told her to leave", CorrectAnswer = "A" },
            new Question { QuestionText = "What inspired Amelia and Theo to keep going despite the challenges?", A = "Their strong bond and shared curiosity", B = "A government grant", C = "A rival scientist’s challenge", D = "A message from Amelia’s boss", CorrectAnswer = "A" },
            new Question { QuestionText = "How did Theo communicate with the sea creature?", A = "Using a special device", B = "Through body movements", C = "By speaking in different tones", D = "Through written messages", CorrectAnswer = "A" },
            new Question { QuestionText = "What challenge did Amelia face in Australia?", A = "She had trouble adjusting to the time difference", B = "She struggled with underwater exploration", C = "She couldn’t get along with Theo’s team", D = "She lost her voice", CorrectAnswer = "B" },
            new Question { QuestionText = "What was the biggest test of Amelia and Theo’s friendship?", A = "Their struggle to complete the project", B = "Their arguments over funding", C = "A rival scientist’s interference", D = "Amelia’s reluctance to continue", CorrectAnswer = "A" },
            new Question { QuestionText = "What was the final outcome of Amelia and Theo’s project?", A = "They built the research station successfully", B = "They abandoned the project", C = "They were forced to shut it down", D = "They moved to a different country", CorrectAnswer = "A" },
            new Question { QuestionText = "What did Theo learn about the mysterious sea creature?", A = "It was harmless and intelligent", B = "It was dangerous", C = "It was part of an ancient legend", D = "It was created by humans", CorrectAnswer = "A" },
            new Question { QuestionText = "Why did Amelia’s decision to visit Theo matter?", A = "It showed her commitment to their work", B = "It made Theo angry", C = "It proved she was wrong", D = "It ended their friendship", CorrectAnswer = "A" },
            new Question { QuestionText = "What emotion did Theo feel when experiencing the mysterious phenomena?", A = "Fear and confusion", B = "Excitement", C = "Happiness", D = "Boredom", CorrectAnswer = "A" },
            new Question { QuestionText = "What did the project ultimately prove?", A = "That distance cannot break true friendship", B = "That underwater research is too risky", C = "That Theo was wrong", D = "That Amelia should have stayed in New York", CorrectAnswer = "A" },
            new Question { QuestionText = "How did Amelia feel after the project was completed?", A = "Fulfilled and connected to Theo", B = "Regretful", C = "Bored", D = "Uninterested", CorrectAnswer = "A" },
            new Question { QuestionText = "What made the sea creature unique?", A = "Its ability to communicate", B = "Its bright color", C = "Its small size", D = "Its aggressive nature", CorrectAnswer = "A" },
            new Question { QuestionText = "How did Theo and Amelia's story end?", A = "They completed the project and stayed connected", B = "They went their separate ways", C = "Theo moved to New York", D = "They abandoned their research", CorrectAnswer = "A" },
            new Question { QuestionText = "What message does this story convey?", A = "Friendship and passion overcome distance", B = "Science is always full of mysteries", C = "Underwater research is dangerous", D = "Time zones are impossible to manage", CorrectAnswer = "A" }

        };

        private List<Question> questions2 = new List<Question>
        {
            new Question { QuestionText = "What was the name of the protagonist in The Crystal Kingdom?", A = "Eldrin", B = "Arianna", C = "Kael", D = "Lorien", CorrectAnswer = "C" },
            new Question { QuestionText = "What was Kael's main goal?", A = "To become king", B = "To find the legendary Crystal", C = "To rescue his family", D = "To defeat the evil sorcerer", CorrectAnswer = "B" },
            new Question { QuestionText = "What power did the Crystal hold?", A = "It could control time", B = "It granted immortality", C = "It purified the kingdom’s land", D = "It allowed its wielder to fly", CorrectAnswer = "C" },
            new Question { QuestionText = "Who was Kael’s most trusted companion?", A = "A talking owl", B = "A mysterious warrior", C = "A rogue thief", D = "A royal knight", CorrectAnswer = "B" },
            new Question { QuestionText = "What was the name of the villain seeking the Crystal?", A = "Lord Draegon", B = "Malakar", C = "Sorin", D = "Velkar", CorrectAnswer = "A" },
            new Question { QuestionText = "What kingdom did Kael belong to?", A = "Zendor", B = "Velthar", C = "Eldoria", D = "Aetheria", CorrectAnswer = "C" },
            new Question { QuestionText = "What was Kael’s greatest strength?", A = "His swordsmanship", B = "His knowledge of ancient texts", C = "His magical abilities", D = "His determination", CorrectAnswer = "D" },
            new Question { QuestionText = "How did Kael learn about the Crystal's location?", A = "A vision from the gods", B = "An old prophecy", C = "A map left by his ancestors", D = "A riddle in an ancient temple", CorrectAnswer = "B" },
            new Question { QuestionText = "What creature guarded the Crystal?", A = "A fire-breathing dragon", B = "A giant serpent", C = "A golem made of stone", D = "A ghostly wraith", CorrectAnswer = "A" },
            new Question { QuestionText = "What was the final trial Kael had to overcome?", A = "A duel with Lord Draegon", B = "Solving an ancient puzzle", C = "Crossing a collapsing bridge", D = "Escaping a labyrinth", CorrectAnswer = "A" },
            new Question { QuestionText = "What did the Crystal do once Kael touched it?", A = "It shattered into pieces", B = "It merged with his body", C = "It purified the kingdom", D = "It disappeared", CorrectAnswer = "C" },
            new Question { QuestionText = "What was the name of Kael’s mysterious warrior companion?", A = "Lyria", B = "Ronan", C = "Darius", D = "Selene", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Lord Draegon want to do with the Crystal?", A = "Use its power to create an army", B = "Destroy it so no one else could use it", C = "Claim its power for immortality", D = "Sell it for wealth", CorrectAnswer = "C" },
            new Question { QuestionText = "Where was the Crystal hidden?", A = "Inside a volcano", B = "Deep within a frozen cavern", C = "At the heart of an enchanted forest", D = "Inside an ancient castle", CorrectAnswer = "B" },
            new Question { QuestionText = "How did Kael first discover he was meant to find the Crystal?", A = "A secret message in an ancient book", B = "A dream from the gods", C = "A hidden mark on his hand", D = "A prophecy told by an elder", CorrectAnswer = "D" },
            new Question { QuestionText = "What weapon did Kael wield?", A = "A flaming sword", B = "A bow with magical arrows", C = "A spear forged from starlight", D = "A shield that could deflect magic", CorrectAnswer = "A" },
            new Question { QuestionText = "What challenge did Kael face before reaching the Crystal?", A = "A blizzard that trapped him", B = "A giant blocking his path", C = "A puzzle door that required ancient knowledge", D = "A raging river with no bridge", CorrectAnswer = "C" },
            new Question { QuestionText = "What was Kael’s final act after obtaining the Crystal?", A = "Destroying Lord Draegon’s fortress", B = "Restoring the land’s balance", C = "Becoming the ruler of Eldoria", D = "Hiding the Crystal away forever", CorrectAnswer = "B" },
            new Question { QuestionText = "Who betrayed Kael during his journey?", A = "A rogue thief", B = "A trusted knight", C = "A scholar who wanted the Crystal’s power", D = "His own brother", CorrectAnswer = "C" },
            new Question { QuestionText = "What magical force protected the Crystal?", A = "A field of lightning", B = "A protective barrier of ice", C = "A spell that tested the purity of the heart", D = "A circle of enchanted flames", CorrectAnswer = "C" },
            new Question { QuestionText = "How did Kael defeat Lord Draegon?", A = "With his sword", B = "By using the Crystal’s power", C = "By tricking him into a trap", D = "By rallying an army", CorrectAnswer = "B" },
            new Question { QuestionText = "What did the people of Eldoria do after the Crystal was restored?", A = "Crowned Kael as their king", B = "Celebrated for seven days", C = "Rebuilt their kingdom", D = "Sealed away all magic forever", CorrectAnswer = "C" },
            new Question { QuestionText = "What lesson did Kael learn?", A = "Power corrupts absolutely", B = "True strength comes from within", C = "Magic is unpredictable", D = "Trusting others is dangerous", CorrectAnswer = "B" },
            new Question { QuestionText = "What was the ultimate fate of Lord Draegon?", A = "He was destroyed by the Crystal’s power", B = "He vanished into the shadows", C = "He was imprisoned", D = "He escaped to another kingdom", CorrectAnswer = "A" },
            new Question { QuestionText = "Who guided Kael through the final steps of his journey?", A = "A wise elder", B = "The spirit of a past king", C = "His mother", D = "A celestial being", CorrectAnswer = "D" },
            new Question { QuestionText = "What was unique about the Crystal?", A = "It could only be used once", B = "It chose its wielder", C = "It had a hidden counterpart", D = "It granted visions of the future", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Kael do with the Crystal after restoring the kingdom?", A = "Returned it to its resting place", B = "Kept it as a weapon", C = "Gave it to the people", D = "Destroyed it", CorrectAnswer = "A" },
            new Question { QuestionText = "What was the moral of the story?", A = "Greed leads to downfall", B = "True power comes from the heart", C = "Heroes are made, not born", D = "Magic should never be used", CorrectAnswer = "B" }

        };

        private List<Question> questions3 = new List<Question>
        {
            new Question { QuestionText = "Who was the leader of the survivor group?", A = "Rick Grimes", B = "Lee Everett", C = "Daryl Dixon", D = "Shane Walsh", CorrectAnswer = "B" },
            new Question { QuestionText = "What caused the world to fall into chaos?", A = "A nuclear war", B = "A deadly virus", C = "The outbreak of the undead", D = "An alien invasion", CorrectAnswer = "C" },
            new Question { QuestionText = "Who did Lee Everett take responsibility for?", A = "A boy named AJ", B = "His best friend", C = "A young girl named Clementine", D = "His wife", CorrectAnswer = "C" },
            new Question { QuestionText = "What was Lee Everett’s profession before the outbreak?", A = "A soldier", B = "A doctor", C = "A history professor", D = "A journalist", CorrectAnswer = "C" },
            new Question { QuestionText = "What was the biggest challenge the group faced?", A = "Finding weapons", B = "Escaping the military", C = "Surviving both the undead and hostile survivors", D = "Building a new city", CorrectAnswer = "C" },
            new Question { QuestionText = "What made the group’s dynamic fragile?", A = "Lack of food", B = "Internal conflicts and difficult choices", C = "Too many people", D = "They didn't trust Lee", CorrectAnswer = "B" },
            new Question { QuestionText = "What happened to Clementine’s family?", A = "They were trapped in another city", B = "They were turned into zombies", C = "They abandoned her", D = "They were looking for her", CorrectAnswer = "B" },
            new Question { QuestionText = "What was the main reason Lee protected Clementine?", A = "She reminded him of his daughter", B = "He was forced to", C = "He saw her as hope for the future", D = "She had useful survival skills", CorrectAnswer = "C" },
            new Question { QuestionText = "What type of survivors did Lee and Clementine encounter?", A = "Only friendly people", B = "Only hostile people", C = "Both friendly and hostile survivors", D = "Only zombies", CorrectAnswer = "C" },
            new Question { QuestionText = "Why did Lee have to make difficult decisions?", A = "Because resources were limited", B = "Because the military gave orders", C = "Because he was trying to become a leader", D = "Because he wanted to take over a city", CorrectAnswer = "A" },
            new Question { QuestionText = "What happened during the night encounter with hostile survivors?", A = "Lee had to decide whether to fight or negotiate", B = "They made a peace treaty", C = "Lee and Clementine ran away", D = "The group was completely wiped out", CorrectAnswer = "A" },
            new Question { QuestionText = "How did Lee’s decisions impact the group?", A = "They made the group stronger", B = "They weakened the group", C = "They permanently changed the group’s fate", D = "They had no real impact", CorrectAnswer = "C" },
            new Question { QuestionText = "What new threat did the group face after the hostile survivors?", A = "The government", B = "A ruthless band of raiders", C = "A powerful zombie king", D = "A deadly disease", CorrectAnswer = "B" },
            new Question { QuestionText = "What did the raiders want?", A = "To help the survivors", B = "To capture Clementine", C = "To steal resources and take control", D = "To destroy all zombies", CorrectAnswer = "C" },
            new Question { QuestionText = "What made the raiders especially dangerous?", A = "They had powerful weapons", B = "They were highly organized and ruthless", C = "They had control over zombies", D = "They used magic", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Lee have to decide when facing the raiders?", A = "To join them or fight them", B = "To surrender the group or defend them", C = "To run away alone or stay with the group", D = "To negotiate with them or kill them", CorrectAnswer = "B" },
            new Question { QuestionText = "What was the outcome of Lee’s choices?", A = "A tragic loss", B = "The raiders left them alone", C = "The group found a new home", D = "They became allies with the raiders", CorrectAnswer = "A" },
            new Question { QuestionText = "What important lessons did Clementine learn?", A = "How to use weapons", B = "The importance of survival, trust, and choices", C = "To never talk to strangers", D = "That the undead are not real enemies", CorrectAnswer = "B" },
            new Question { QuestionText = "What happened to the group after Lee’s decisions?", A = "They disbanded", B = "They were completely wiped out", C = "They carried on with new leadership", D = "They returned to normal life", CorrectAnswer = "C" },
            new Question { QuestionText = "How was Lee’s legacy remembered?", A = "Through a monument", B = "Through the survivors he helped", C = "Through a book about his journey", D = "Through a radio broadcast", CorrectAnswer = "B" },
            new Question { QuestionText = "What was the main theme of the story?", A = "Finding treasure", B = "The importance of hope and humanity in dark times", C = "The rise of technology", D = "Escaping from prison", CorrectAnswer = "B" },
            new Question { QuestionText = "What was Lee’s main strength as a leader?", A = "His ability to fight", B = "His intelligence and decision-making", C = "His wealth", D = "His ability to build things", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Clementine gain from her time with Lee?", A = "A map to safety", B = "Leadership skills and wisdom", C = "A powerful weapon", D = "A new home", CorrectAnswer = "B" },
            new Question { QuestionText = "What was the hardest thing Lee had to do?", A = "Kill a friend", B = "Make a decision that led to someone’s death", C = "Abandon Clementine", D = "Destroy a whole city", CorrectAnswer = "B" },
            new Question { QuestionText = "What was one of the biggest dangers besides the undead?", A = "Lack of oxygen", B = "Other survivors who turned violent", C = "Deadly animals", D = "Alien invaders", CorrectAnswer = "B" },
            new Question { QuestionText = "What type of leadership did Lee show?", A = "Tyrannical leadership", B = "Compassionate and protective leadership", C = "Careless leadership", D = "Strict dictatorship", CorrectAnswer = "B" },
            new Question { QuestionText = "What did the group learn through their journey?", A = "That zombies could be controlled", B = "That sticking together was key to survival", C = "That weapons were all they needed", D = "That it was better to be alone", CorrectAnswer = "B" },
            new Question { QuestionText = "What message did the story leave for the readers?", A = "Zombies will take over the world", B = "Hope and humanity can survive even in dark times", C = "Survival is all about weapons", D = "Never trust anyone", CorrectAnswer = "B" }
        };
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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, score);

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
                // Start the challenge'
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
                DialogResult result = MessageBox.Show("Are you sure with your answers?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (AllQuestionsAnswered())
                    {
                        // Challenge completed successfully
                        timer.Stop();
                        progressBar1.Value = progressBar1.Maximum; // Set progress bar to max
                        textBox2.Text = TimeSpan.FromSeconds(progressDuration - timeRemaining).ToString("hh\\:mm\\:ss"); // Update final time

                        MessageBox.Show($"Quiz completed! Your score is: {score}/{questions.Count}", "Quiz Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Save challenge data (including score)
                        if (score >= 8) // Check if the challenge is passed
                        {
                            success.Play();
                            SaveChallengeDataSuccess(Login.CurrentUsername, "Completed", textBox2.Text, score);
                            MessageBox.Show("Returning to the homepage.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            TransitionToHomePage();
                        }
                        else
                        {
                            fail.Play();
                            SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, score);
                            MessageBox.Show("Challenge failed. Returning to the homepage.", "Challenge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            TransitionToHomePage();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please answer all questions before submitting.", "Incomplete Quiz", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // Reset answers and mark all questions as unanswered
                    for (int i = 0; i < answeredQuestions.Count; i++)
                    {
                        answeredQuestions[i] = false;  // Mark as unanswered
                        answeredAnswers[i] = ""; // Clear the selected answers
                    }

                    // Optionally, reset score to 0 and re-enable picture box buttons if necessary
                    score = 0;
                    pictureBoxA.Enabled = true;
                    pictureBoxB.Enabled = true;
                    pictureBoxC.Enabled = true;
                    pictureBoxD.Enabled = true;

                    // Reset the display to the first question (or another appropriate starting point)
                    currentIndex = 0;
                    DisplayQuestion(currentIndex);

                    MessageBox.Show("Your answers have been reset. Please answer the questions again.", "Answers Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, score);

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

        private void EssayrichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            do
            {
                currentIndex--;
            }
            while (currentIndex >= 0 && answeredQuestions[currentIndex]); // Skip answered questions going backward

            if (currentIndex >= 0) // Ensure we're still within bounds
            {
                DisplayQuestion(currentIndex);
            }
            else
            {
                MessageBox.Show("You are already at the first unanswered question.", "First Unanswered Question", MessageBoxButtons.OK, MessageBoxIcon.Information);
                currentIndex = 0; // Reset to the first question for safety
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            do
            {
                currentIndex++;
            }
            while (currentIndex < questions.Count && answeredQuestions[currentIndex]); // Skip answered questions going forward

            if (currentIndex < questions.Count) // Ensure we're still within bounds
            {
                DisplayQuestion(currentIndex);
            }
            else
            {
                MessageBox.Show("You have reached the last unanswered question.", "Last Unanswered Question", MessageBoxButtons.OK, MessageBoxIcon.Information);
                currentIndex = questions.Count - 1; // Reset to the last question for safety
            }
        }

        private void pictureBoxA_Click(object sender, EventArgs e)
        {
            click.Play();
            AnswerButton_Click(sender, e);
        }

        private void pictureBoxB_Click(object sender, EventArgs e)
        {
            click.Play();
            AnswerButton_Click(sender, e);
        }

        private void pictureBoxC_Click(object sender, EventArgs e)
        {
            click.Play();
            AnswerButton_Click(sender, e);
        }

        private void pictureBoxD_Click(object sender, EventArgs e)
        {
            click.Play();
            AnswerButton_Click(sender, e);
        }
    }
}
