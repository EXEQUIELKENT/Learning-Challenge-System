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
    public partial class GrammarMedium : Form
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
        public class Question
        {
            public string QuestionText { get; set; }
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string CorrectAnswer { get; set; }
        }
        public GrammarMedium()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "___ is responsible for completing this task?", A = "When", B = "Who", C = "Why", D = "Which", CorrectAnswer = "B" },
                new Question { QuestionText = "___ do you plan to solve this issue?", A = "What", B = "Which", C = "How", D = "Where", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these books would you recommend?", A = "Who", B = "Which", C = "Why", D = "Where", CorrectAnswer = "B" },
                new Question { QuestionText = "___ are we having the meeting?", A = "Why", B = "How", C = "Where", D = "When", CorrectAnswer = "D" },
                
                new Question { QuestionText = "___ did you choose this particular option?", A = "Why", B = "When", C = "Which", D = "How", CorrectAnswer = "A" },
                new Question { QuestionText = "___ do you think will win the competition?", A = "Who", B = "Which", C = "What", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ should we contact for assistance?", A = "Where", B = "Why", C = "Who", D = "What", CorrectAnswer = "C" },
                new Question { QuestionText = "___ have you placed the car keys?", A = "How", B = "Why", C = "Where", D = "Which", CorrectAnswer = "C" },
                new Question { QuestionText = "___ is the main reason for your decision?", A = "What", B = "Why", C = "When", D = "How", CorrectAnswer = "B" },
                new Question { QuestionText = "___ do you need to complete this project?", A = "Who", B = "What", C = "When", D = "Where", CorrectAnswer = "B" },

                new Question { QuestionText = "___ is responsible for this mistake?", A = "Who", B = "What", C = "Where", D = "When", CorrectAnswer = "A" },
                new Question { QuestionText = "___ did you leave your phone?", A = "Why", B = "Where", C = "How", D = "When", CorrectAnswer = "B" },
                new Question { QuestionText = "___ did you complete the assignment?", A = "Where", B = "When", C = "Why", D = "How", CorrectAnswer = "B" },
                new Question { QuestionText = "___ can help us solve this problem?", A = "Who", B = "What", C = "How", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ should I bring to the picnic?", A = "When", B = "What", C = "Who", D = "Where", CorrectAnswer = "B" },

                new Question { QuestionText = "___ are you planning to move abroad?", A = "When", B = "Why", C = "How", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ of these books belongs to you?", A = "What", B = "Which", C = "Whose", D = "Who", CorrectAnswer = "B" },
                new Question { QuestionText = "___ was your reaction when you heard the news?", A = "Who", B = "When", C = "What", D = "Where", CorrectAnswer = "C" },
                new Question { QuestionText = "___ did you find this old letter?", A = "Where", B = "When", C = "How", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ do you prefer, coffee or tea?", A = "Which", B = "Who", C = "When", D = "What", CorrectAnswer = "A" },

                new Question { QuestionText = "___ is knocking at the door?", A = "Who", B = "When", C = "What", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ did she say about the project?", A = "What", B = "Who", C = "How", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ of these dresses looks better on me?", A = "Where", B = "What", C = "Which", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ are you upset about?", A = "Who", B = "What", C = "When", D = "How", CorrectAnswer = "B" },
                new Question { QuestionText = "___ do you think will win the competition?", A = "What", B = "Who", C = "How", D = "When", CorrectAnswer = "B" },

                new Question { QuestionText = "___ did you solve the puzzle so fast?", A = "How", B = "Why", C = "When", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ do you describe yourself in one word?", A = "What", B = "How", C = "Who", D = "Which", CorrectAnswer = "B" },
                new Question { QuestionText = "___ should we contact for more information?", A = "When", B = "How", C = "Who", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ is making that noise outside?", A = "Who", B = "What", C = "Where", D = "When", CorrectAnswer = "A" },
                new Question { QuestionText = "___ of these solutions is the best?", A = "What", B = "Which", C = "Where", D = "Why", CorrectAnswer = "B" },

                new Question { QuestionText = "___ did he react to the surprise?", A = "Who", B = "What", C = "How", D = "When", CorrectAnswer = "C" },
                new Question { QuestionText = "___ is the capital of Canada?", A = "Where", B = "When", C = "What", D = "Which", CorrectAnswer = "C" },
                new Question { QuestionText = "___ do you explain this theory?", A = "Who", B = "How", C = "Where", D = "Why", CorrectAnswer = "B" },
                new Question { QuestionText = "___ of these paintings do you prefer?", A = "Which", B = "What", C = "Who", D = "Where", CorrectAnswer = "A" },
                new Question { QuestionText = "___ did the teacher ask about the homework?", A = "Who", B = "What", C = "Why", D = "How", CorrectAnswer = "A" },

                new Question { QuestionText = "___ do you want to achieve in five years?", A = "What", B = "Who", C = "Where", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ is going to help us with this task?", A = "Who", B = "How", C = "Why", D = "What", CorrectAnswer = "A" },
                new Question { QuestionText = "___ will you explain this to your boss?", A = "Who", B = "When", C = "What", D = "How", CorrectAnswer = "D" },
                new Question { QuestionText = "___ of these strategies is most effective?", A = "Which", B = "What", C = "Where", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ should we take into account before deciding?", A = "Who", B = "What", C = "Why", D = "How", CorrectAnswer = "B" },

                new Question { QuestionText = "___ is responsible for cleaning the classroom?", A = "Who", B = "What", C = "Where", D = "How", CorrectAnswer = "A" },
                new Question { QuestionText = "___ did you decide to take this course?", A = "What", B = "How", C = "When", D = "Why", CorrectAnswer = "D" },
                new Question { QuestionText = "___ of these solutions do you think will work best?", A = "Who", B = "What", C = "Which", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ did you manage to solve the problem?", A = "Who", B = "How", C = "What", D = "Where", CorrectAnswer = "B" },
                new Question { QuestionText = "___ do you plan to finish your project?", A = "Where", B = "When", C = "Who", D = "What", CorrectAnswer = "B" },

                new Question { QuestionText = "___ are you feeling so nervous about?", A = "Where", B = "When", C = "Why", D = "Who", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these two restaurants serves better food?", A = "Who", B = "Where", C = "What", D = "Which", CorrectAnswer = "D" },
                new Question { QuestionText = "___ did you leave your keys last night?", A = "When", B = "Where", C = "Why", D = "Who", CorrectAnswer = "B" },
                new Question { QuestionText = "___ is your reason for missing the meeting?", A = "Who", B = "What", C = "Where", D = "Why", CorrectAnswer = "B" },
                new Question { QuestionText = "___ of these laptops is the most affordable?", A = "Which", B = "Who", C = "How", D = "Why", CorrectAnswer = "A" },

                new Question { QuestionText = "___ should we inform about the changes?", A = "Who", B = "What", C = "How", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ do you think will win the presidential election?", A = "Who", B = "What", C = "Where", D = "When", CorrectAnswer = "A" },
                new Question { QuestionText = "___ can you explain this equation step by step?", A = "Where", B = "How", C = "When", D = "Who", CorrectAnswer = "B" },
                new Question { QuestionText = "___ should we call if we have a problem?", A = "What", B = "When", C = "Who", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these paintings was created in the 18th century?", A = "Who", B = "What", C = "Which", D = "Where", CorrectAnswer = "C" },

                new Question { QuestionText = "___ did you respond when they offered you the job?", A = "How", B = "Who", C = "When", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ of these projects should be prioritized?", A = "What", B = "Which", C = "Where", D = "Who", CorrectAnswer = "B" },
                new Question { QuestionText = "___ are you planning to go after work?", A = "Who", B = "Where", C = "When", D = "What", CorrectAnswer = "B" },
                new Question { QuestionText = "___ did you manage to complete the report on time?", A = "What", B = "Where", C = "How", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these movies is your favorite?", A = "Who", B = "What", C = "Which", D = "Where", CorrectAnswer = "C" },

                new Question { QuestionText = "___ did the concert start?", A = "When", B = "Where", C = "Why", D = "How", CorrectAnswer = "A" },
                new Question { QuestionText = "___ should we prepare for the upcoming event?", A = "Who", B = "Where", C = "How", D = "Why", CorrectAnswer = "C" },
                new Question { QuestionText = "___ of these options seems more practical?", A = "Who", B = "Which", C = "When", D = "What", CorrectAnswer = "B" },
                new Question { QuestionText = "___ did they select as the new team leader?", A = "Who", B = "Which", C = "Where", D = "When", CorrectAnswer = "A" },
                new Question { QuestionText = "___ do you handle stressful situations?", A = "What", B = "When", C = "How", D = "Where", CorrectAnswer = "C" },

                new Question { QuestionText = "___ should we invite to the panel discussion?", A = "Who", B = "Which", C = "What", D = "Where", CorrectAnswer = "A" },
                new Question { QuestionText = "___ is the reason behind this policy change?", A = "Who", B = "Why", C = "Where", D = "How", CorrectAnswer = "B" },
                new Question { QuestionText = "___ is the purpose of this training session?", A = "Why", B = "How", C = "What", D = "Who", CorrectAnswer = "C" },
                new Question { QuestionText = "___ do you need to do before the deadline?", A = "What", B = "When", C = "Where", D = "Who", CorrectAnswer = "A" },
                new Question { QuestionText = "___ of these drinks contains the least sugar?", A = "Where", B = "What", C = "Who", D = "Which", CorrectAnswer = "D" },

                new Question { QuestionText = "___ are the qualifications required for the job?", A = "Who", B = "What", C = "Where", D = "Why", CorrectAnswer = "B" },
                new Question { QuestionText = "___ is your plan for improving customer service?", A = "What", B = "Who", C = "Why", D = "Where", CorrectAnswer = "A" },
                new Question { QuestionText = "___ would you rate your experience with our service?", A = "How", B = "When", C = "What", D = "Why", CorrectAnswer = "A" },
                new Question { QuestionText = "___ should we consult before making a decision?", A = "Where", B = "Who", C = "What", D = "Why", CorrectAnswer = "B" },
                new Question { QuestionText = "___ is the correct procedure to follow?", A = "What", B = "Who", C = "Where", D = "Why", CorrectAnswer = "A" }
                // Add more questions here...
            };

            // Initialize answeredQuestions with false values (indicating that no question has been answered yet)
            Random random = new Random();
            questions = questions.OrderBy(q => random.Next()).Take(10).ToList();

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
                FormTitle = "Grammar (Medium)",
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
                FormTitle = "Grammar (Medium)",
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

        private void GrammarEasy_Load(object sender, EventArgs e)
        {

        }
    }
}
