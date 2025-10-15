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
    public partial class BudjetProblemEasy : Form
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
        public BudjetProblemEasy()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "If you have a monthly income of ₱30,000 and spend ₱12,000 on rent, ₱4,000 on groceries, ₱2,000 on transportation, and ₱3,000 on utilities, how much do you have left for savings?", A = "₱9,000", B = "₱7,000", C = "₱10,000", D = "₱8,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A family has a budget of ₱25,000 per month. They spend ₱10,000 on housing, ₱5,000 on food, ₱2,000 on transportation, and ₱3,000 on utilities. What is the remaining amount they can spend?", A = "₱2,000", B = "₱5,000", C = "₱10,000", D = "₱6,000", CorrectAnswer = "B" },
                new Question { QuestionText = "Your monthly income is ₱40,000. You spend ₱15,000 on rent, ₱6,000 on bills, and ₱5,000 on groceries. How much can you save this month?", A = "₱14,000", B = "₱10,000", C = "₱15,000", D = "₱12,000", CorrectAnswer = "A" },
                new Question { QuestionText = "You receive a paycheck of ₱32,000. You spend ₱7,000 on rent, ₱5,000 on groceries, ₱3,000 on insurance, and ₱4,000 on entertainment. What is your remaining budget?", A = "₱13,000", B = "₱11,000", C = "₱10,000", D = "₱9,000", CorrectAnswer = "A" },
                new Question { QuestionText = "If your monthly budget is ₱50,000 and you spend ₱25,000 on rent, ₱8,000 on food, ₱3,000 on transportation, and ₱4,000 on utilities, how much do you have left for savings?", A = "₱10,000", B = "₱15,000", C = "₱20,000", D = "₱12,000", CorrectAnswer = "B" },

                new Question { QuestionText = "You earn ₱20,000 per month. Your rent is ₱8,000, groceries cost ₱3,000, and bills are ₱2,000. How much is left?", A = "₱7,000", B = "₱6,000", C = "₱8,000", D = "₱5,000", CorrectAnswer = "A" },
                new Question { QuestionText = "A person has ₱35,000 salary. They spend ₱10,000 on rent, ₱7,000 on bills, and ₱5,000 on groceries. How much do they save?", A = "₱10,000", B = "₱12,000", C = "₱13,000", D = "₱15,000", CorrectAnswer = "C" },
                new Question { QuestionText = "If you have ₱18,000 and spend ₱5,000 on rent, ₱3,000 on food, and ₱2,000 on transportation, how much is left?", A = "₱8,000", B = "₱9,000", C = "₱7,000", D = "₱6,000", CorrectAnswer = "A" },
                new Question { QuestionText = "A student has a budget of ₱5,000. They spend ₱2,000 on school fees and ₱1,000 on food. What is left?", A = "₱1,500", B = "₱2,000", C = "₱1,000", D = "₱2,500", CorrectAnswer = "C" },
                new Question { QuestionText = "Your daily allowance is ₱500. You spend ₱200 on food and ₱100 on transport. How much is left?", A = "₱100", B = "₱150", C = "₱200", D = "₱250", CorrectAnswer = "C" },

                new Question { QuestionText = "If you earn ₱45,000 per month and save ₱10,000, how much do you have for expenses?", A = "₱30,000", B = "₱35,000", C = "₱40,000", D = "₱25,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A worker earns ₱28,000. Their expenses are ₱12,000 for rent, ₱6,000 for food, and ₱4,000 for transportation. How much is left?", A = "₱4,000", B = "₱5,000", C = "₱6,000", D = "₱3,000", CorrectAnswer = "A" },
                new Question { QuestionText = "If a family spends ₱15,000 out of their ₱25,000 budget, how much do they have left?", A = "₱5,000", B = "₱10,000", C = "₱8,000", D = "₱7,000", CorrectAnswer = "B" },
                new Question { QuestionText = "Your electricity bill is ₱2,500, and your water bill is ₱1,500. If you have ₱10,000, how much remains?", A = "₱6,000", B = "₱5,000", C = "₱4,500", D = "₱3,000", CorrectAnswer = "A" },
                new Question { QuestionText = "If a company has a budget of ₱100,000 and spends ₱60,000, how much is left?", A = "₱30,000", B = "₱40,000", C = "₱50,000", D = "₱20,000", CorrectAnswer = "B" },

                new Question { QuestionText = "If you have a monthly income of ₱30,000 and spend ₱12,000 on rent, ₱4,000 on groceries, ₱2,000 on transportation, and ₱3,000 on utilities, how much do you have left for savings?", A = "₱9,000", B = "₱7,000", C = "₱10,000", D = "₱8,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A family has a budget of ₱25,000 per month. They spend ₱10,000 on housing, ₱5,000 on food, ₱2,000 on transportation, and ₱3,000 on utilities. What is the remaining amount they can spend?", A = "₱2,000", B = "₱5,000", C = "₱10,000", D = "₱6,000", CorrectAnswer = "B" },
                new Question { QuestionText = "Your monthly income is ₱40,000. You spend ₱15,000 on rent, ₱6,000 on bills, and ₱5,000 on groceries. How much can you save this month?", A = "₱14,000", B = "₱10,000", C = "₱15,000", D = "₱12,000", CorrectAnswer = "A" },
                new Question { QuestionText = "You receive a paycheck of ₱32,000. You spend ₱7,000 on rent, ₱5,000 on groceries, ₱3,000 on insurance, and ₱4,000 on entertainment. What is your remaining budget?", A = "₱13,000", B = "₱11,000", C = "₱10,000", D = "₱9,000", CorrectAnswer = "A" },
                new Question { QuestionText = "If your monthly budget is ₱50,000 and you spend ₱25,000 on rent, ₱8,000 on food, ₱3,000 on transportation, and ₱4,000 on utilities, how much do you have left for savings?", A = "₱10,000", B = "₱15,000", C = "₱20,000", D = "₱12,000", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A student has ₱3,000 allowance for the week. They spend ₱1,000 on food and ₱500 on transportation. How much do they have left?", A = "₱1,000", B = "₱1,500", C = "₱1,200", D = "₱1,800", CorrectAnswer = "B" },
                new Question { QuestionText = "You have ₱20,000 and you need to pay ₱8,500 for rent and ₱2,500 for food. How much will be left?", A = "₱8,000", B = "₱9,000", C = "₱10,000", D = "₱11,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A worker earns ₱28,000. They spend ₱10,000 on rent, ₱6,000 on bills, and ₱5,000 on groceries. How much is left?", A = "₱7,000", B = "₱6,000", C = "₱5,000", D = "₱8,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A family spends ₱3,500 on groceries, ₱2,000 on electricity, and ₱1,500 on water. Their total budget is ₱10,000. How much remains?", A = "₱3,000", B = "₱2,500", C = "₱4,000", D = "₱3,500", CorrectAnswer = "A" },
                new Question { QuestionText = "Your salary is ₱35,000, but your expenses are ₱12,000 for rent, ₱6,000 for food, and ₱7,000 for bills. How much is left?", A = "₱10,000", B = "₱12,000", C = "₱15,000", D = "₱9,000", CorrectAnswer = "D" },

                new Question { QuestionText = "You earn ₱22,000 per month. Your rent is ₱10,000, groceries cost ₱4,000, and bills are ₱3,000. How much is left?", A = "₱5,000", B = "₱4,000", C = "₱6,000", D = "₱7,000", CorrectAnswer = "A" },
                new Question { QuestionText = "A person has ₱50,000 salary. They spend ₱18,000 on rent, ₱8,000 on bills, and ₱10,000 on groceries. How much do they save?", A = "₱12,000", B = "₱15,000", C = "₱14,000", D = "₱16,000", CorrectAnswer = "C" },
                new Question { QuestionText = "If you have ₱18,000 and spend ₱5,000 on rent, ₱3,000 on food, and ₱2,000 on transportation, how much is left?", A = "₱8,000", B = "₱9,000", C = "₱7,000", D = "₱6,000", CorrectAnswer = "A" },
                new Question { QuestionText = "A student has a budget of ₱5,000. They spend ₱2,000 on school fees and ₱1,000 on food. What is left?", A = "₱1,500", B = "₱2,000", C = "₱1,000", D = "₱2,500", CorrectAnswer = "C" },
                new Question { QuestionText = "Your daily allowance is ₱500. You spend ₱200 on food and ₱100 on transport. How much is left?", A = "₱100", B = "₱150", C = "₱200", D = "₱250", CorrectAnswer = "C" },

                new Question { QuestionText = "A person earns ₱45,000 a month. They spend ₱20,000 on rent, ₱7,000 on food, and ₱5,000 on utilities. How much is left?", A = "₱10,000", B = "₱12,000", C = "₱13,000", D = "₱15,000", CorrectAnswer = "C" },
                new Question { QuestionText = "Your grocery budget is ₱4,500, but you spent ₱3,200. How much is left?", A = "₱1,200", B = "₱1,300", C = "₱1,500", D = "₱1,800", CorrectAnswer = "B" },
                new Question { QuestionText = "A worker receives a ₱32,000 salary. Their expenses include ₱15,000 for rent, ₱5,000 for groceries, and ₱6,000 for bills. How much remains?", A = "₱5,000", B = "₱6,000", C = "₱7,000", D = "₱8,000", CorrectAnswer = "B" },
                new Question { QuestionText = "You have ₱20,000 and spend ₱8,500 on rent, ₱2,500 on groceries, and ₱3,000 on utilities. How much do you have left?", A = "₱6,000", B = "₱7,000", C = "₱5,000", D = "₱6,500", CorrectAnswer = "A" },
                new Question { QuestionText = "A college student has ₱7,000. They spend ₱2,500 on tuition and ₱1,500 on books. How much is left?", A = "₱2,500", B = "₱3,000", C = "₱3,500", D = "₱4,000", CorrectAnswer = "B" },
                
                new Question { QuestionText = "You save ₱500 every week. How much will you save in 6 months?", A = "₱10,000", B = "₱12,000", C = "₱13,000", D = "₱14,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A person spends ₱9,000 on rent and ₱4,000 on utilities. Their income is ₱18,000. How much is left?", A = "₱5,000", B = "₱6,000", C = "₱4,000", D = "₱3,000", CorrectAnswer = "A" },
                new Question { QuestionText = "You have ₱15,000 for the month. You spend ₱6,500 on rent, ₱3,000 on food, and ₱2,000 on transport. How much remains?", A = "₱2,000", B = "₱3,500", C = "₱4,000", D = "₱3,000", CorrectAnswer = "D" },
                new Question { QuestionText = "A family has a ₱50,000 budget. They spend ₱22,000 on rent, ₱10,000 on food, and ₱5,000 on bills. How much is left?", A = "₱12,000", B = "₱13,000", C = "₱15,000", D = "₱10,000", CorrectAnswer = "C" },
                new Question { QuestionText = "You earn ₱60,000 per month. Your expenses are ₱25,000 for rent, ₱12,000 for food, and ₱8,000 for bills. How much do you save?", A = "₱12,000", B = "₱15,000", C = "₱18,000", D = "₱20,000", CorrectAnswer = "C" },
                
                new Question { QuestionText = "A student gets ₱2,000 per week. They spend ₱700 on transport and ₱500 on food. How much is left?", A = "₱800", B = "₱900", C = "₱1,000", D = "₱700", CorrectAnswer = "B" },
                new Question { QuestionText = "Your electricity bill is ₱3,200, and your water bill is ₱1,500. If you budget ₱6,000 for utilities, how much remains?", A = "₱1,000", B = "₱1,200", C = "₱1,300", D = "₱1,500", CorrectAnswer = "D" },
                new Question { QuestionText = "A business earns ₱150,000 monthly. Their expenses are ₱50,000 for rent, ₱30,000 for salaries, and ₱20,000 for utilities. How much is left?", A = "₱50,000", B = "₱60,000", C = "₱70,000", D = "₱80,000", CorrectAnswer = "B" },
                new Question { QuestionText = "You plan to buy a laptop for ₱40,000. You have saved ₱28,000. How much more do you need?", A = "₱10,000", B = "₱11,000", C = "₱12,000", D = "₱15,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A farmer earns ₱20,000 per harvest. If they harvest three times a year, what is their total income?", A = "₱50,000", B = "₱55,000", C = "₱60,000", D = "₱65,000", CorrectAnswer = "C" },
                
                new Question { QuestionText = "You budget ₱12,000 for a vacation. Your hotel costs ₱5,000, and your food costs ₱3,000. How much is left?", A = "₱3,000", B = "₱4,000", C = "₱5,000", D = "₱6,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A phone costs ₱15,000. You pay ₱5,000 upfront. How much do you still need?", A = "₱8,000", B = "₱9,000", C = "₱10,000", D = "₱12,000", CorrectAnswer = "C" },
                new Question { QuestionText = "You spend ₱4,000 on bills and ₱2,500 on food. Your salary is ₱20,000. How much is left?", A = "₱12,000", B = "₱13,500", C = "₱14,000", D = "₱15,500", CorrectAnswer = "B" },
                new Question { QuestionText = "You have ₱5,000 for a party. You spend ₱3,200 on food. How much remains?", A = "₱1,500", B = "₱1,800", C = "₱1,700", D = "₱1,600", CorrectAnswer = "D" },
                new Question { QuestionText = "Your friend owes you ₱2,500. They paid ₱1,000 already. How much do they still owe?", A = "₱1,200", B = "₱1,300", C = "₱1,500", D = "₱1,600", CorrectAnswer = "C" },
                
                new Question { QuestionText = "A tour package costs ₱35,000. You pay ₱10,000 as a deposit. How much is left to pay?", A = "₱20,000", B = "₱22,000", C = "₱25,000", D = "₱27,000", CorrectAnswer = "C" }
                // Add more questions here...
            };

            // Initialize answeredQuestions with false values (indicating that no question has been answered yet)
            Random rand = new Random();
            questions = questions.OrderBy(q => rand.Next()).Take(5).ToList();

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
                FormTitle = "Budjet Problem (Easy)",
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
                FormTitle = "Budget Problem (Easy)",
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
                        if (score >= 3) // Check if the challenge is passed
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
