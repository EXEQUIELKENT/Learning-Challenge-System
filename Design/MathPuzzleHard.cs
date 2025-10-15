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
    public partial class MathPuzzleHard : Form
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
        public MathPuzzleHard()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "(8 × 3) + ___ = 32", A = "5", B = "6", C = "7", D = "8", CorrectAnswer = "C" },
                new Question { QuestionText = "72 ÷ (9 - 3) = ___", A = "8", B = "9", C = "10", D = "12", CorrectAnswer = "A" },
                new Question { QuestionText = "(25 + 15) ÷ ___ = 10", A = "3", B = "4", C = "5", D = "6", CorrectAnswer = "C" },
                new Question { QuestionText = "84 - (12 × 6) = ___", A = "8", B = "12", C = "14", D = "16", CorrectAnswer = "A" },
                new Question { QuestionText = "___ ÷ (6 + 2) = 10", A = "60", B = "70", C = "80", D = "90", CorrectAnswer = "C" },
                
                new Question { QuestionText = "(144 ÷ 12) × 3 = ___", A = "30", B = "32", C = "36", D = "40", CorrectAnswer = "C" },
                new Question { QuestionText = "(18 + 42) ÷ (5 × 2) = ___", A = "4", B = "5", C = "6", D = "7", CorrectAnswer = "B" },
                new Question { QuestionText = "96 ÷ (12 - 4) + ___ = 16", A = "3", B = "4", C = "5", D = "6", CorrectAnswer = "C" },
                new Question { QuestionText = "(5 × 7) - (6 × 4) = ___", A = "10", B = "11", C = "12", D = "13", CorrectAnswer = "C" },
                new Question { QuestionText = "[(72 ÷ 8) + 9] × 2 = ___", A = "30", B = "32", C = "34", D = "36", CorrectAnswer = "B" },
                
                new Question { QuestionText = "(9 × 9) - (6 × 7) = ___", A = "21", B = "23", C = "25", D = "27", CorrectAnswer = "A" },
                new Question { QuestionText = "(36 ÷ 6) × (8 ÷ 2) = ___", A = "20", B = "24", C = "28", D = "32", CorrectAnswer = "B" },
                new Question { QuestionText = "[(8 + 4) × 5] ÷ 2 = ___", A = "24", B = "26", C = "28", D = "30", CorrectAnswer = "A" },
                new Question { QuestionText = "(54 ÷ 6) + (45 ÷ 9) = ___", A = "11", B = "12", C = "13", D = "14", CorrectAnswer = "B" },
                new Question { QuestionText = "(7 × 6) - (5 × 4) + ___ = 34", A = "2", B = "3", C = "4", D = "5", CorrectAnswer = "D" },
                
                new Question { QuestionText = "[(9 × 5) + (4 × 3)] ÷ 3 = ___", A = "14", B = "15", C = "16", D = "17", CorrectAnswer = "A" },
                new Question { QuestionText = "[(100 ÷ 5) × (4 + 1)] ÷ 10 = ___", A = "8", B = "9", C = "10", D = "11", CorrectAnswer = "C" },
                new Question { QuestionText = "[(7 + 3) × 4] - (6 × 5) = ___", A = "10", B = "12", C = "14", D = "16", CorrectAnswer = "C" },
                new Question { QuestionText = "(81 ÷ 9) + (56 ÷ 7) = ___", A = "15", B = "16", C = "17", D = "18", CorrectAnswer = "A" },
                new Question { QuestionText = "[(120 ÷ 10) × (3 + 2)] ÷ 5 = ___", A = "6", B = "7", C = "8", D = "9", CorrectAnswer = "B" },
                
                new Question { QuestionText = "(12 × 5) ÷ (3 × 2) = ___", A = "8", B = "9", C = "10", D = "11", CorrectAnswer = "A" },
                new Question { QuestionText = "[(9 × 8) + (4 × 6)] ÷ 6 = ___", A = "14", B = "15", C = "16", D = "17", CorrectAnswer = "B" },
                new Question { QuestionText = "(100 ÷ 4) - (36 ÷ 6) = ___", A = "19", B = "20", C = "21", D = "22", CorrectAnswer = "C" },
                new Question { QuestionText = "(18 × 3) - (45 ÷ 5) = ___", A = "42", B = "43", C = "44", D = "45", CorrectAnswer = "A" },
                new Question { QuestionText = "[(16 × 5) + (6 × 8)] ÷ 4 = ___", A = "21", B = "22", C = "23", D = "24", CorrectAnswer = "B" },
                
                new Question { QuestionText = "(60 ÷ 5) × (3 + 2) = ___", A = "55", B = "56", C = "57", D = "58", CorrectAnswer = "C" },
                new Question { QuestionText = "(14 × 7) ÷ (10 - 4) = ___", A = "14", B = "15", C = "16", D = "17", CorrectAnswer = "A" },
                new Question { QuestionText = "(80 ÷ 4) + (9 × 2) = ___", A = "38", B = "39", C = "40", D = "41", CorrectAnswer = "C" },
                new Question { QuestionText = "(48 ÷ 6) × (9 - 6) = ___", A = "22", B = "23", C = "24", D = "25", CorrectAnswer = "C" },
                new Question { QuestionText = "[(132 ÷ 11) × 4] ÷ 2 = ___", A = "22", B = "23", C = "24", D = "25", CorrectAnswer = "C" },
                
                new Question { QuestionText = "(90 ÷ 9) + (64 ÷ 8) = ___", A = "16", B = "17", C = "18", D = "19", CorrectAnswer = "A" },
                new Question { QuestionText = "[(72 ÷ 9) × 5] ÷ 3 = ___", A = "12", B = "13", C = "14", D = "15", CorrectAnswer = "B" },
                new Question { QuestionText = "(84 ÷ 12) + (30 ÷ 5) = ___", A = "12", B = "13", C = "14", D = "15", CorrectAnswer = "C" },
                new Question { QuestionText = "[(15 × 4) - 10] ÷ 5 = ___", A = "9", B = "10", C = "11", D = "12", CorrectAnswer = "A" },
                new Question { QuestionText = "(100 ÷ 5) + (8 × 3) = ___", A = "34", B = "35", C = "36", D = "37", CorrectAnswer = "B" },
                
                new Question { QuestionText = "(144 ÷ 12) × (18 ÷ 3) = ___", A = "68", B = "70", C = "72", D = "74", CorrectAnswer = "C" },
                new Question { QuestionText = "[(18 × 5) + (60 ÷ 4)] ÷ 6 = ___", A = "14", B = "15", C = "16", D = "17", CorrectAnswer = "A" },
                new Question { QuestionText = "(225 ÷ 15) × (36 ÷ 6) = ___", A = "86", B = "88", C = "90", D = "92", CorrectAnswer = "C" },
                new Question { QuestionText = "(14 × 7) - (10 × 5) = ___", A = "48", B = "50", C = "52", D = "54", CorrectAnswer = "A" },
                new Question { QuestionText = "[(96 ÷ 12) × 5] - (9 × 3) = ___", A = "30", B = "32", C = "34", D = "36", CorrectAnswer = "B" },
                
                new Question { QuestionText = "(144 ÷ 8) + (54 ÷ 9) = ___", A = "24", B = "25", C = "26", D = "27", CorrectAnswer = "A" },
                new Question { QuestionText = "(120 ÷ 10) × (6 + 3) = ___", A = "102", B = "104", C = "108", D = "110", CorrectAnswer = "C" },
                new Question { QuestionText = "(88 ÷ 8) + (64 ÷ 8) = ___", A = "18", B = "19", C = "20", D = "21", CorrectAnswer = "A" },
                new Question { QuestionText = "[(132 ÷ 11) × (4 + 2)] ÷ 3 = ___", A = "22", B = "23", C = "24", D = "25", CorrectAnswer = "C" },
                new Question { QuestionText = "(225 ÷ 15) - (96 ÷ 12) = ___", A = "11", B = "12", C = "13", D = "14", CorrectAnswer = "A" },
                
                new Question { QuestionText = "[(90 ÷ 9) + (72 ÷ 9)] × 2 = ___", A = "34", B = "35", C = "36", D = "37", CorrectAnswer = "C" },
                new Question { QuestionText = "(84 ÷ 12) × (48 ÷ 8) = ___", A = "28", B = "29", C = "30", D = "31", CorrectAnswer = "A" },
                new Question { QuestionText = "(196 ÷ 14) + (80 ÷ 10) = ___", A = "20", B = "21", C = "22", D = "23", CorrectAnswer = "B" },
                new Question { QuestionText = "[(144 ÷ 12) × (4 + 2)] ÷ 3 = ___", A = "22", B = "23", C = "24", D = "25", CorrectAnswer = "C" },
                new Question { QuestionText = "(60 ÷ 6) + (100 ÷ 10) = ___", A = "15", B = "16", C = "17", D = "18", CorrectAnswer = "A" },
                
                new Question { QuestionText = "[(132 ÷ 11) × (5 + 3)] ÷ 4 = ___", A = "24", B = "25", C = "26", D = "27", CorrectAnswer = "B" },
                new Question { QuestionText = "(180 ÷ 12) × (24 ÷ 6) = ___", A = "28", B = "29", C = "30", D = "31", CorrectAnswer = "C" },
                new Question { QuestionText = "(81 ÷ 9) + (72 ÷ 8) = ___", A = "17", B = "18", C = "19", D = "20", CorrectAnswer = "B" },
                new Question { QuestionText = "[(84 ÷ 12) × (9 + 3)] ÷ 4 = ___", A = "14", B = "15", C = "16", D = "17", CorrectAnswer = "A" },
                new Question { QuestionText = "(72 ÷ 6) × (100 ÷ 10) = ___", A = "120", B = "122", C = "124", D = "126", CorrectAnswer = "A" },
                
                new Question { QuestionText = "(144 ÷ 12) + (96 ÷ 8) = ___", A = "24", B = "25", C = "26", D = "27", CorrectAnswer = "B" },
                new Question { QuestionText = "(225 ÷ 15) × (80 ÷ 10) = ___", A = "120", B = "122", C = "124", D = "126", CorrectAnswer = "A" },
                new Question { QuestionText = "(160 ÷ 10) + (108 ÷ 12) = ___", A = "26", B = "27", C = "28", D = "29", CorrectAnswer = "B" },
                new Question { QuestionText = "[(96 ÷ 12) × (5 + 4)] ÷ 3 = ___", A = "14", B = "15", C = "16", D = "17", CorrectAnswer = "C" },
                new Question { QuestionText = "(132 ÷ 11) × (6 + 4) = ___", A = "108", B = "110", C = "112", D = "114", CorrectAnswer = "B" },
                
                new Question { QuestionText = "(180 ÷ 12) × (48 ÷ 8) = ___", A = "90", B = "92", C = "94", D = "96", CorrectAnswer = "A" },
                new Question { QuestionText = "(72 ÷ 6) + (100 ÷ 10) = ___", A = "22", B = "23", C = "24", D = "25", CorrectAnswer = "C" },
                new Question { QuestionText = "(144 ÷ 12) × (10 + 2) = ___", A = "108", B = "110", C = "112", D = "114", CorrectAnswer = "B" },
                new Question { QuestionText = "(160 ÷ 10) × (36 ÷ 6) = ___", A = "92", B = "94", C = "96", D = "98", CorrectAnswer = "C" },
                new Question { QuestionText = "(225 ÷ 15) × (64 ÷ 8) = ___", A = "120", B = "122", C = "124", D = "126", CorrectAnswer = "A" },
                
                new Question { QuestionText = "(81 ÷ 9) × (72 ÷ 9) = ___", A = "64", B = "66", C = "68", D = "70", CorrectAnswer = "C" },
                new Question { QuestionText = "(72 ÷ 6) × (96 ÷ 8) = ___", A = "144", B = "146", C = "148", D = "150", CorrectAnswer = "A" },
                new Question { QuestionText = "(180 ÷ 12) × (24 ÷ 6) = ___", A = "90", B = "92", C = "94", D = "96", CorrectAnswer = "A" },

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
                FormTitle = "Math Puzzle (Hard)",
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
                FormTitle = "Math Puzzle (Hard)",
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

