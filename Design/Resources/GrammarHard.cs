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
    public partial class GrammarHard : Form
    {
        private Timer timer;
        private int progressDuration = 1200; // total time for progress bar in seconds
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
        public class Question
        {
            public string QuestionText { get; set; }
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string CorrectAnswer { get; set; }
        }
        public GrammarHard()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "___ of the following statements is correct?", A = "When", B = "Which", C = "What", D = "Who", CorrectAnswer = "B" },
                new Question { QuestionText = "___ do you intend to explain such a complex topic?", A = "Why", B = "When", C = "How", D = "What", CorrectAnswer = "C" },
                new Question { QuestionText = "___ did you arrive at that conclusion?", A = "How", B = "What", C = "Why", D = "When", CorrectAnswer = "A" },
                new Question { QuestionText = "___ do you think is the most challenging aspect of this task?", A = "What", B = "Which", C = "How", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ among these choices provides the clearest explanation?", A = "Which", B = "What", C = "Who", D = "Why", CorrectAnswer = "A" },
                
                new Question { QuestionText = "___ has this decision affected the team’s morale?", A = "Why", B = "How", C = "When", D = "What", CorrectAnswer = "B" },
                new Question { QuestionText = "___ do you think will be the next to present?", A = "Who", B = "What", C = "Which", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ of these two routes is faster?", A = "What", B = "Where", C = "Which", D = "How", CorrectAnswer = "C" },
                new Question { QuestionText = "___ did they say they would deliver the package?", A = "When", B = "Where", C = "Why", D = "How", CorrectAnswer = "A" },
                new Question { QuestionText = "___ do you believe is the primary cause of this error?", A = "What", B = "Why", C = "How", D = "Which", CorrectAnswer = "A" },
                
                new Question { QuestionText = "___ do you find most impressive about their performance?", A = "Which", B = "What", C = "How", D = "Why", CorrectAnswer = "B" },
                new Question { QuestionText = "___ of them can you trust with this responsibility?", A = "Why", B = "How", C = "Which", D = "Who", CorrectAnswer = "C" },
                new Question { QuestionText = "___ do you justify spending so much on this project?", A = "How", B = "Why", C = "What", D = "When", CorrectAnswer = "B" },
                new Question { QuestionText = "___ was the location of the conference last year?", A = "What", B = "Where", C = "When", D = "Who", CorrectAnswer = "B" },
                new Question { QuestionText = "___ would you respond to such criticism?", A = "Why", B = "What", C = "How", D = "When", CorrectAnswer = "C" },
                
                new Question { QuestionText = "___ of these strategies seems the most practical?", A = "Which", B = "What", C = "Why", D = "When", CorrectAnswer = "A" },
                new Question { QuestionText = "___ is responsible for ensuring the documents are accurate?", A = "Where", B = "Why", C = "Who", D = "How", CorrectAnswer = "C" },
                new Question { QuestionText = "___ criteria do you use to evaluate their performance?", A = "Which", B = "How", C = "What", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ can we make this process more efficient?", A = "Why", B = "How", C = "When", D = "What", CorrectAnswer = "B" },
                new Question { QuestionText = "___ would be the consequences of missing this deadline?", A = "What", B = "Why", C = "How", D = "When", CorrectAnswer = "A" },

                new Question { QuestionText = "___ did the government implement this policy despite public opposition?", A = "Where", B = "Why", C = "What", D = "Who", CorrectAnswer = "B" },
                new Question { QuestionText = "___ of these scientific theories best explains the phenomenon?", A = "Who", B = "What", C = "Which", D = "How", CorrectAnswer = "C" },
                new Question { QuestionText = "___ should be held accountable for the financial mismanagement?", A = "Where", B = "Who", C = "How", D = "Why", CorrectAnswer = "B" },
                new Question { QuestionText = "___ did researchers conclude after conducting multiple experiments?", A = "What", B = "Where", C = "Who", D = "How", CorrectAnswer = "A" },
                new Question { QuestionText = "___ does the Supreme Court have the authority to overrule?", A = "Why", B = "When", C = "What", D = "Who", CorrectAnswer = "C" },

                new Question { QuestionText = "___ led to the economic crisis in the early 2000s?", A = "How", B = "Why", C = "Who", D = "What", CorrectAnswer = "B" },
                new Question { QuestionText = "___ of these literary works is considered the most influential?", A = "Where", B = "Which", C = "What", D = "Who", CorrectAnswer = "B" },
                new Question { QuestionText = "___ factors contributed to the decline of ancient civilizations?", A = "What", B = "Where", C = "How", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ should be prioritized in international trade agreements?", A = "What", B = "Where", C = "Why", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ are the ethical implications of using artificial intelligence?", A = "Who", B = "Where", C = "What", D = "Why", CorrectAnswer = "C" },

                new Question { QuestionText = "___ can climate change be mitigated effectively?", A = "Why", B = "Where", C = "How", D = "Who", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these historical events had the greatest global impact?", A = "Which", B = "What", C = "Who", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ role did technological advancements play in world history?", A = "What", B = "Who", C = "Where", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ was the primary cause of the Industrial Revolution?", A = "Who", B = "Where", C = "What", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these leadership styles is the most effective?", A = "Which", B = "Who", C = "Why", D = "Where", CorrectAnswer = "A" },

                new Question { QuestionText = "___ should be considered when drafting new constitutional laws?", A = "Where", B = "Who", C = "Why", D = "What", CorrectAnswer = "D" },
                new Question { QuestionText = "___ does democracy differ from other forms of governance?", A = "How", B = "What", C = "Why", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ do economic policies impact global markets?", A = "What", B = "Where", C = "Why", D = "How", CorrectAnswer = "D" },
                new Question { QuestionText = "___ of these theories best explains human behavior?", A = "Who", B = "What", C = "Which", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ should governments prioritize during national crises?", A = "What", B = "Who", C = "Where", D = "Why", CorrectAnswer = "A" },

                new Question { QuestionText = "___ are the main differences between capitalism and socialism?", A = "Who", B = "Where", C = "What", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these inventions had the greatest impact on society?", A = "Which", B = "Who", C = "Where", D = "How", CorrectAnswer = "A" },
                new Question { QuestionText = "___ did the Renaissance influence modern art and culture?", A = "Where", B = "What", C = "How", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ is the significance of renewable energy for the future?", A = "Who", B = "Where", C = "What", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ challenges does globalization present to local economies?", A = "What", B = "Why", C = "Who", D = "Where", CorrectAnswer = "A" },

                new Question { QuestionText = "___ led scientists to believe in the Big Bang Theory?", A = "Who", B = "What", C = "Where", D = "Why", CorrectAnswer = "B" },
                new Question { QuestionText = "___ impact does deforestation have on biodiversity?", A = "What", B = "Why", C = "Who", D = "Where", CorrectAnswer = "A" },
                new Question { QuestionText = "___ do people experience cultural adaptation in foreign countries?", A = "Why", B = "Where", C = "How", D = "Who", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these research methods is the most reliable?", A = "What", B = "Which", C = "Who", D = "Why", CorrectAnswer = "B" },
                new Question { QuestionText = "___ strategies can companies use to remain competitive?", A = "What", B = "Where", C = "Why", D = "Who", CorrectAnswer = "A" },

                new Question { QuestionText = "___ policies should be implemented to protect the environment?", A = "What", B = "Where", C = "Who", D = "How", CorrectAnswer = "A" },
                new Question { QuestionText = "___ techniques improve memory retention and recall?", A = "Who", B = "What", C = "Where", D = "How", CorrectAnswer = "D" },
                new Question { QuestionText = "___ is the economic impact of automation in industries?", A = "What", B = "Where", C = "Why", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ role does psychology play in decision-making?", A = "What", B = "Where", C = "Who", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ can international conflicts be resolved peacefully?", A = "Where", B = "Why", C = "What", D = "How", CorrectAnswer = "D" },


                new Question { QuestionText = "___ is the most effective way to combat misinformation online?", A = "Who", B = "Where", C = "How", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these philosophical ideologies promotes individual freedom?", A = "Which", B = "What", C = "Where", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ role does ethics play in artificial intelligence development?", A = "Why", B = "Who", C = "What", D = "Where", CorrectAnswer = "C" },
                new Question { QuestionText = "___ challenges do policymakers face in regulating technology?", A = "What", B = "Where", C = "Who", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ should historians consider when interpreting historical events?", A = "Who", B = "What", C = "Where", D = "Why", CorrectAnswer = "B" },

                new Question { QuestionText = "___ is the significance of genetic modification in medicine?", A = "Why", B = "Where", C = "What", D = "Who", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these environmental policies has shown the best results?", A = "Which", B = "What", C = "Who", D = "Where", CorrectAnswer = "A" },
                new Question { QuestionText = "___ are the main economic implications of climate change?", A = "What", B = "Who", C = "Where", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ factors contribute to the reliability of scientific research?", A = "What", B = "Where", C = "Who", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ should be considered when developing public health policies?", A = "What", B = "Who", C = "Where", D = "Why", CorrectAnswer = "A" },

                new Question { QuestionText = "___ strategies can be used to reduce carbon footprints?", A = "Who", B = "Where", C = "How", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ is the most critical factor in national security?", A = "What", B = "Who", C = "Where", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ of these leadership approaches fosters the most innovation?", A = "Which", B = "What", C = "Where", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ influenced the evolution of human communication?", A = "Who", B = "What", C = "Where", D = "Why", CorrectAnswer = "B" },
                new Question { QuestionText = "___ does behavioral economics challenge traditional financial theories?", A = "Where", B = "How", C = "What", D = "Who", CorrectAnswer = "B" },

                new Question { QuestionText = "___ is the impact of automation on global employment?", A = "What", B = "Why", C = "Who", D = "Where", CorrectAnswer = "A" },
                new Question { QuestionText = "___ of these voting systems ensures the most democratic elections?", A = "Which", B = "What", C = "Where", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ ethical dilemmas arise in artificial intelligence research?", A = "What", B = "Where", C = "Who", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ strategies should businesses adopt in an economic downturn?", A = "What", B = "Where", C = "Who", D = "How", CorrectAnswer = "A" },
                new Question { QuestionText = "___ led to the economic collapse of the Great Depression?", A = "Who", B = "What", C = "Where", D = "Why", CorrectAnswer = "B" },

                new Question { QuestionText = "___ is the role of social movements in shaping policy changes?", A = "What", B = "Who", C = "Where", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ approaches can improve cybersecurity in the digital age?", A = "Who", B = "Where", C = "How", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ does cognitive psychology explain decision-making?", A = "How", B = "What", C = "Where", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ influenced major advancements in modern medicine?", A = "Who", B = "What", C = "Where", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ strategies should nations take to reduce income inequality?", A = "What", B = "Where", C = "Who", D = "How", CorrectAnswer = "A" },

                new Question { QuestionText = "___ impact does cryptocurrency have on global banking?", A = "What", B = "Where", C = "Who", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ of these philosophies aligns best with utilitarian principles?", A = "Which", B = "What", C = "Where", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ are the biggest threats to democracy in the modern world?", A = "Who", B = "What", C = "Where", D = "Why", CorrectAnswer = "B" },
                new Question { QuestionText = "___ mechanisms can be used to detect fake news online?", A = "Who", B = "Where", C = "How", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these forms of government has the most sustainable model?", A = "Which", B = "What", C = "Where", D = "Who", CorrectAnswer = "A" },

                new Question { QuestionText = "___ should artificial intelligence be regulated to prevent misuse?", A = "Where", B = "How", C = "What", D = "Why", CorrectAnswer = "D" },
                new Question { QuestionText = "___ drives the success of global e-commerce giants?", A = "Who", B = "What", C = "Where", D = "Why", CorrectAnswer = "B" },
                new Question { QuestionText = "___ principles govern ethical hacking and cybersecurity?", A = "What", B = "Where", C = "Who", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ advancements in medical science have improved global health?", A = "What", B = "Where", C = "Who", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ are the risks of excessive reliance on artificial intelligence?", A = "Who", B = "What", C = "Where", D = "Why", CorrectAnswer = "B" },

                new Question { QuestionText = "___ does linguistic evolution impact cultural identity?", A = "How", B = "Where", C = "What", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ of these governance models leads to the most stable economy?", A = "Which", B = "What", C = "Where", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ should be prioritized when reforming education systems?", A = "What", B = "Who", C = "Where", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ does global trade influence the stability of national economies?", A = "How", B = "Where", C = "What", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ challenges does artificial intelligence pose to employment?", A = "What", B = "Where", C = "Who", D = "Why", CorrectAnswer = "A" }
                // Add more questions here...
            };

            // Initialize answeredQuestions with false values (indicating that no question has been answered yet)
            Random rand = new Random();
            questions = questions.OrderBy(q => rand.Next()).Take(20).ToList();

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
                FormTitle = "Grammar (Hard)",
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
                FormTitle = "Grammar (Hard)",
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
                        if (score >= 15) // Check if the challenge is passed
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

        private void GrammarEasy_Load(object sender, EventArgs e)
        {

        }
    }
}
