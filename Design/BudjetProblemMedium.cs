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
    public partial class BudjetProblemMedium : Form
    {
        private Timer timer;
        private int progressDuration = 1500; // total time for progress bar in seconds
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
        public BudjetProblemMedium()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "A person has a monthly income of ₱125,000. They spend ₱60,000 on rent, ₱20,000 on groceries, and ₱7,500 on utilities. How much money is left for other expenses?", A = "₱35,000", B = "₱37,500", C = "₱37,500", D = "₱40,000", CorrectAnswer = "C" },
                new Question { QuestionText = "If you have ₱25,000 in savings and you spend ₱17,500 on groceries, how much money will remain in your savings?", A = "₱5,000", B = "₱7,500", C = "₱10,000", D = "₱2,500", CorrectAnswer = "A" },
                new Question { QuestionText = "A family has a monthly budget of ₱200,000. They spend ₱60,000 on housing, ₱40,000 on food, ₱20,000 on transportation, and ₱10,000 on entertainment. How much is left for savings?", A = "₱50,000", B = "₱75,000", C = "₱60,000", D = "₱70,000", CorrectAnswer = "A" },
                new Question { QuestionText = "You earn ₱160,000 per month and spend ₱50,000 on rent, ₱25,000 on transportation, and ₱15,000 on utilities. What is your remaining balance?", A = "₱70,000", B = "₱75,000", C = "₱80,000", D = "₱85,000", CorrectAnswer = "A" },
                new Question { QuestionText = "If you have a monthly income of ₱150,000 and you spend ₱60,000 on rent, ₱20,000 on groceries, and ₱15,000 on transportation, what percentage of your income is left?", A = "30%", B = "40%", C = "50%", D = "60%", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A person is planning to save 20% of their income each month. If their income is ₱125,000, how much will they save in a month?", A = "₱25,000", B = "₱30,000", C = "₱35,000", D = "₱40,000", CorrectAnswer = "A" },
                new Question { QuestionText = "You have a budget of ₱75,000 for the month. If you spend ₱25,000 on rent, ₱15,000 on food, and ₱10,000 on utilities, how much money is left for entertainment?", A = "₱15,000", B = "₱20,000", C = "₱25,000", D = "₱30,000", CorrectAnswer = "B" },
                new Question { QuestionText = "If your monthly expenses total ₱100,000 and your income is ₱125,000, how much is left for savings?", A = "₱15,000", B = "₱20,000", C = "₱25,000", D = "₱30,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A household budget is set at ₱250,000 per month. If they spend ₱75,000 on mortgage, ₱25,000 on food, and ₱10,000 on insurance, how much is left for savings?", A = "₱140,000", B = "₱130,000", C = "₱120,000", D = "₱110,000", CorrectAnswer = "A" },
                new Question { QuestionText = "A person has a total monthly income of ₱175,000. They spend ₱50,000 on housing, ₱30,000 on food, and ₱20,000 on utilities. How much money is left for savings?", A = "₱50,000", B = "₱60,000", C = "₱75,000", D = "₱85,000", CorrectAnswer = "B" },
                
                new Question { QuestionText = "You earn ₱80,000 a month. If 25% of your salary is deducted for taxes and 10% for insurance, how much is left?", A = "₱50,000", B = "₱55,000", C = "₱52,000", D = "₱54,000", CorrectAnswer = "C" },
                new Question { QuestionText = "If a person saves 30% of their ₱90,000 income, how much do they save monthly?", A = "₱25,000", B = "₱27,000", C = "₱30,000", D = "₱35,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A family spends 40% of their ₱200,000 monthly income on needs and 20% on wants. How much is left?", A = "₱80,000", B = "₱100,000", C = "₱120,000", D = "₱140,000", CorrectAnswer = "A" },
                new Question { QuestionText = "You want to save ₱1,000,000 in 5 years. If you save an equal amount each month, how much should you save?", A = "₱15,000", B = "₱16,500", C = "₱17,000", D = "₱18,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A business earns ₱500,000 per quarter. If 40% goes to expenses, how much is left after a year?", A = "₱800,000", B = "₱900,000", C = "₱1,000,000", D = "₱1,200,000", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Your household expenses total ₱45,000 a month. If this is 60% of your salary, what is your monthly income?", A = "₱70,000", B = "₱75,000", C = "₱80,000", D = "₱85,000", CorrectAnswer = "B" },
                new Question { QuestionText = "If you invest ₱500,000 in a business and earn 10% profit monthly, how much is your profit after one year?", A = "₱600,000", B = "₱650,000", C = "₱700,000", D = "₱750,000", CorrectAnswer = "D" },
                new Question { QuestionText = "A person budgets 35% of their ₱120,000 salary for rent. How much is that?", A = "₱35,000", B = "₱38,000", C = "₱40,000", D = "₱42,000", CorrectAnswer = "C" },
                new Question { QuestionText = "You buy a car for ₱1,200,000 with a 10% down payment. How much do you need to loan?", A = "₱1,000,000", B = "₱1,050,000", C = "₱1,080,000", D = "₱1,100,000", CorrectAnswer = "A" },
                new Question { QuestionText = "If your investments increase by 5% monthly, how much will ₱500,000 be worth in 6 months?", A = "₱600,000", B = "₱625,000", C = "₱650,000", D = "₱675,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A person has a salary of ₱85,000. They save 15%, spend 50% on expenses, and invest 20%. How much is left for miscellaneous expenses?", A = "₱8,500", B = "₱10,000", C = "₱12,750", D = "₱14,500", CorrectAnswer = "C" },
                
                new Question { QuestionText = "You take a loan of ₱500,000 with a 12% annual interest. How much interest do you pay after one year?", A = "₱50,000", B = "₱55,000", C = "₱60,000", D = "₱65,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A family earns ₱150,000 per month and allots 30% for food, 20% for rent, and 10% for transportation. How much remains?", A = "₱60,000", B = "₱70,000", C = "₱75,000", D = "₱80,000", CorrectAnswer = "B" },
                new Question { QuestionText = "You invest ₱250,000 in a business and gain a 15% return. How much profit do you earn?", A = "₱35,000", B = "₱37,500", C = "₱40,000", D = "₱42,500", CorrectAnswer = "B" },
                new Question { QuestionText = "Your monthly expenses are ₱60,000, which is 75% of your income. What is your salary?", A = "₱75,000", B = "₱80,000", C = "₱85,000", D = "₱90,000", CorrectAnswer = "A" },
                new Question { QuestionText = "A student has a budget of ₱12,000 per month. They spend ₱3,000 on rent, ₱4,000 on food, and ₱2,500 on transportation. How much is left?", A = "₱2,000", B = "₱2,500", C = "₱3,000", D = "₱3,500", CorrectAnswer = "B" },
                
                new Question { QuestionText = "You buy a house for ₱4,000,000 with a 20% down payment. How much is your loan amount?", A = "₱2,800,000", B = "₱3,000,000", C = "₱3,200,000", D = "₱3,400,000", CorrectAnswer = "A" },
                new Question { QuestionText = "A person saves ₱5,000 every month. How much will they have saved after 3 years?", A = "₱160,000", B = "₱170,000", C = "₱180,000", D = "₱190,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A company earns ₱500,000 per quarter. If expenses are ₱120,000, what is the net income for the year?", A = "₱1,480,000", B = "₱1,500,000", C = "₱1,520,000", D = "₱1,550,000", CorrectAnswer = "B" },
                new Question { QuestionText = "You spend ₱3,500 per month on groceries. If food prices increase by 5%, how much will you spend next month?", A = "₱3,675", B = "₱3,700", C = "₱3,725", D = "₱3,750", CorrectAnswer = "A" },
                new Question { QuestionText = "A person pays ₱15,000 in monthly rent, which is 25% of their income. What is their monthly salary?", A = "₱55,000", B = "₱58,000", C = "₱60,000", D = "₱62,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A couple earns ₱180,000 monthly and saves 20% of it. How much will they save in a year?", A = "₱400,000", B = "₱420,000", C = "₱432,000", D = "₱450,000", CorrectAnswer = "C" },

                new Question { QuestionText = "Your savings account earns 4% interest annually. If you deposit ₱100,000, how much interest will you earn after one year?", A = "₱4,000", B = "₱4,500", C = "₱5,000", D = "₱5,500", CorrectAnswer = "A" },
                new Question { QuestionText = "If a company's annual revenue is ₱5,000,000 and expenses are 60% of revenue, how much is the profit?", A = "₱1,800,000", B = "₱1,900,000", C = "₱2,000,000", D = "₱2,100,000", CorrectAnswer = "C" },
                new Question { QuestionText = "You spend ₱1,200 daily. If you reduce spending by 10%, how much will you save in a month?", A = "₱3,200", B = "₱3,600", C = "₱3,800", D = "₱4,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A store sells an item for ₱500 and makes a 20% profit. What is the cost price?", A = "₱400", B = "₱420", C = "₱440", D = "₱460", CorrectAnswer = "A" },
                new Question { QuestionText = "A business spends ₱2,000,000 annually, which is 70% of its total income. What is the total income?", A = "₱2,500,000", B = "₱2,750,000", C = "₱2,850,000", D = "₱3,000,000", CorrectAnswer = "D" },
                
                new Question { QuestionText = "A person saves ₱10,000 each month. If they withdraw ₱5,000 per month, how much will they have after 1 year?", A = "₱50,000", B = "₱55,000", C = "₱60,000", D = "₱65,000", CorrectAnswer = "C" },
                new Question { QuestionText = "If a company offers a 12% salary increase on a ₱50,000 salary, what is the new salary?", A = "₱54,000", B = "₱55,000", C = "₱56,000", D = "₱56,500", CorrectAnswer = "C" },
                new Question { QuestionText = "A company reduces its budget by 15%. If the previous budget was ₱3,000,000, what is the new budget?", A = "₱2,500,000", B = "₱2,550,000", C = "₱2,600,000", D = "₱2,650,000", CorrectAnswer = "B" },
            };

            // Initialize answeredQuestions with false values (indicating that no question has been answered yet)
            Random rand = new Random();
            questions = questions.OrderBy(q => rand.Next()).Take(10).ToList();

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
                FormTitle = "Budjet Problem (Medium)",
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
                FormTitle = "Budget Problem (Medium)",
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
