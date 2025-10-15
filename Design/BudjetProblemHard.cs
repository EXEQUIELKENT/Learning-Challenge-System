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
    public partial class BudjetProblemHard : Form
    {
        private Timer timer;
        private int progressDuration = 3600; // total time for progress bar in seconds
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
        public BudjetProblemHard()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "A company has a budget of ₱5,600,000 for marketing. It spends 40% of the budget on digital marketing and 30% on traditional media. How much is left for other expenses?", A = "₱1,680,000", B = "₱2,240,000", C = "₱2,800,000", D = "₱3,360,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A family plans to spend ₱168,000 a month. They spend ₱67,200 on rent, ₱33,600 on groceries, and ₱22,400 on utilities. How much money is left for savings?", A = "₱44,800", B = "₱56,000", C = "₱67,200", D = "₱84,000", CorrectAnswer = "A" },
                new Question { QuestionText = "A business allocated 50% of its revenue for employee salaries, 20% for marketing, and 15% for rent. If the total revenue is ₱11,200,000, how much is allocated for marketing?", A = "₱1,680,000", B = "₱1,960,000", C = "₱2,240,000", D = "₱2,520,000", CorrectAnswer = "A" },
                new Question { QuestionText = "If a person spends 30% of their monthly income on housing, 20% on groceries, and 15% on transportation, how much of their income is left for savings?", A = "35%", B = "25%", C = "40%", D = "50%", CorrectAnswer = "B" },
                new Question { QuestionText = "A company has a budget of ₱28,000,000 for the year. It plans to spend 10% on research and development, 30% on marketing, and 20% on operations. How much will be spent on operations?", A = "₱5,600,000", B = "₱8,400,000", C = "₱11,200,000", D = "₱14,000,000", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A manufacturing company has a budget of ₱50,000,000. It allocates 35% to raw materials, 25% to labor costs, and 20% to equipment maintenance. How much is left for other expenses?", A = "₱10,000,000", B = "₱12,500,000", C = "₱15,000,000", D = "₱17,500,000", CorrectAnswer = "A" },
                new Question { QuestionText = "A person takes out a loan of ₱1,500,000 with an annual interest rate of 10%. If they plan to repay in 5 years with equal yearly payments, how much is their total repayment amount?", A = "₱1,750,000", B = "₱1,800,000", C = "₱1,875,000", D = "₱2,000,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A software company generates ₱12,000,000 in revenue. If operating costs are 60% of revenue and taxes take up another 15%, what is the net profit?", A = "₱1,800,000", B = "₱2,400,000", C = "₱3,000,000", D = "₱3,200,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A business earns ₱3,500,000 per quarter. If 45% is allocated to operational expenses and 30% to expansion, how much is left after expenses and expansion?", A = "₱875,000", B = "₱950,000", C = "₱1,000,000", D = "₱1,050,000", CorrectAnswer = "A" },
                new Question { QuestionText = "A company's advertising budget was cut by 12%. If the previous budget was ₱18,000,000, what is the new budget?", A = "₱15,480,000", B = "₱15,800,000", C = "₱16,200,000", D = "₱16,500,000", CorrectAnswer = "A" },
                
                new Question { QuestionText = "A retail store buys products at ₱1,200 per unit and sells them at a 30% markup. If it sells 5,000 units, what is the total revenue?", A = "₱7,200,000", B = "₱7,500,000", C = "₱7,800,000", D = "₱8,000,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A family's monthly expenses include ₱25,000 for rent, ₱15,000 for food, and ₱5,000 for transportation. If their total income is ₱80,000, what percentage of their income is spent?", A = "40%", B = "45%", C = "50%", D = "55%", CorrectAnswer = "C" },
                new Question { QuestionText = "A business received a loan of ₱2,000,000 at 8% annual interest. If they plan to pay it off in 4 years, how much total interest will they pay?", A = "₱560,000", B = "₱600,000", C = "₱640,000", D = "₱680,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A hotel earns ₱500,000 per month. If they allocate 50% for salaries, 20% for utilities, and 10% for maintenance, how much remains for profit?", A = "₱75,000", B = "₱100,000", C = "₱125,000", D = "₱150,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A city government has a ₱100,000,000 annual budget. 35% goes to public services, 30% to infrastructure, and 20% to education. How much is left for emergency funds?", A = "₱10,000,000", B = "₱12,000,000", C = "₱15,000,000", D = "₱18,000,000", CorrectAnswer = "C" },
                
                new Question { QuestionText = "A company has an annual revenue of ₱150,000,000. If 40% is allocated to operations, 25% to salaries, and 15% to research and development, how much is left for profit?", A = "₱15,000,000", B = "₱20,000,000", C = "₱30,000,000", D = "₱35,000,000", CorrectAnswer = "C" },
                new Question { QuestionText = "An entrepreneur invests ₱5,000,000 in a business. If the business generates a 12% return per year, how much profit will they earn after 3 years?", A = "₱1,500,000", B = "₱1,800,000", C = "₱2,000,000", D = "₱2,500,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A business purchases raw materials worth ₱2,400,000 per month. If material costs increase by 8% next year, what will be the new monthly expense?", A = "₱2,500,000", B = "₱2,592,000", C = "₱2,620,000", D = "₱2,700,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A family's annual income is ₱960,000. They spend 40% on necessities, 30% on lifestyle expenses, and 20% on savings. How much is left unallocated?", A = "₱48,000", B = "₱72,000", C = "₱96,000", D = "₱120,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A retail store sells a product at ₱3,500 per unit with a 25% profit margin. If they sell 2,000 units, what is their total profit?", A = "₱1,500,000", B = "₱1,700,000", C = "₱1,750,000", D = "₱1,800,000", CorrectAnswer = "C" },
                
                new Question { QuestionText = "A hospital allocates ₱75,000,000 annually. 50% goes to salaries, 20% to medical supplies, and 15% to maintenance. How much is left for research and development?", A = "₱7,500,000", B = "₱8,500,000", C = "₱10,000,000", D = "₱12,500,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A company invests ₱30,000,000 in new technology, expecting a return of 18% in 2 years. How much total profit will they make from the investment?", A = "₱9,600,000", B = "₱10,500,000", C = "₱11,000,000", D = "₱11,500,000", CorrectAnswer = "A" },
                new Question { QuestionText = "A government agency has a budget of ₱200,000,000. If 45% is allocated to social welfare, 30% to infrastructure, and 15% to salaries, how much remains for emergency funds?", A = "₱10,000,000", B = "₱20,000,000", C = "₱25,000,000", D = "₱30,000,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A factory spends ₱6,500,000 per month on production. If operating costs increase by 5% next year, what will be the new monthly expense?", A = "₱6,800,000", B = "₱6,950,000", C = "₱7,000,000", D = "₱7,200,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A business earns ₱14,000,000 per quarter. If 50% is allocated to expenses, 20% to expansion, and 15% to taxes, how much remains as net profit?", A = "₱1,500,000", B = "₱2,100,000", C = "₱2,800,000", D = "₱3,000,000", CorrectAnswer = "C" },
                
                new Question { QuestionText = "A sports facility earns ₱800,000 per month. If they allocate 40% to maintenance, 30% to salaries, and 15% to marketing, how much remains for profit?", A = "₱80,000", B = "₱100,000", C = "₱120,000", D = "₱140,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A fast-food chain operates 200 stores nationwide, with each store generating ₱1,200,000 per month. If operational costs per store are 65% of revenue, what is the company's total profit per month?", A = "₱76,000,000", B = "₱78,000,000", C = "₱80,000,000", D = "₱82,000,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A transport company has a budget of ₱350,000,000. It allocates 55% to vehicle maintenance, 25% to employee salaries, and 10% to fuel. How much is left for future investments?", A = "₱10,000,000", B = "₱25,000,000", C = "₱30,000,000", D = "₱35,000,000", CorrectAnswer = "D" },
                new Question { QuestionText = "A college receives ₱500,000,000 in funding. It allocates 60% to faculty salaries, 20% to facilities, and 10% to research. How much remains for scholarships?", A = "₱40,000,000", B = "₱50,000,000", C = "₱60,000,000", D = "₱70,000,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A fashion company spends ₱18,000,000 annually on production. If raw material costs increase by 7% next year, what will be the new annual production cost?", A = "₱18,900,000", B = "₱19,200,000", C = "₱19,260,000", D = "₱19,500,000", CorrectAnswer = "C" },
                
                new Question { QuestionText = "A software company earns ₱24,000,000 per year. If 45% is spent on development, 30% on marketing, and 10% on taxes, how much remains as net income?", A = "₱3,600,000", B = "₱4,000,000", C = "₱4,200,000", D = "₱4,800,000", CorrectAnswer = "D" },
                new Question { QuestionText = "A public park project requires ₱75,000,000 in funding. If the government covers 60%, a private sponsor provides 25%, and the rest is from donations, how much must be raised from donations?", A = "₱8,000,000", B = "₱9,000,000", C = "₱10,000,000", D = "₱11,250,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A construction company has a budget of ₱1,200,000,000. If 35% is allocated for materials, 25% for labor, and 20% for equipment, how much remains for miscellaneous expenses?", A = "₱180,000,000", B = "₱200,000,000", C = "₱220,000,000", D = "₱240,000,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A high-end restaurant earns ₱5,000,000 monthly. If 50% is spent on ingredients, 30% on staff salaries, and 10% on utilities, how much is left for profit?", A = "₱250,000", B = "₱500,000", C = "₱1,000,000", D = "₱1,500,000", CorrectAnswer = "C" },
                new Question { QuestionText = "An IT company generates ₱80,000,000 annually. If 40% is used for salaries, 25% for software development, and 20% for operations, how much is allocated for expansion?", A = "₱8,000,000", B = "₱10,000,000", C = "₱12,000,000", D = "₱16,000,000", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A university has an annual budget of ₱750,000,000. It spends 50% on faculty salaries, 20% on research, and 15% on facility maintenance. How much is left for student programs?", A = "₱50,000,000", B = "₱75,000,000", C = "₱85,000,000", D = "₱90,000,000", CorrectAnswer = "D" },
                new Question { QuestionText = "A hotel earns ₱3,000,000 per month. If 35% is allocated to maintenance, 25% to staff salaries, and 15% to marketing, how much remains as net profit?", A = "₱500,000", B = "₱650,000", C = "₱750,000", D = "₱850,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A private hospital earns ₱20,000,000 per quarter. If 40% is spent on medical equipment, 30% on staff salaries, and 20% on utilities, how much is left as net income?", A = "₱1,000,000", B = "₱2,000,000", C = "₱3,000,000", D = "₱4,000,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A logistics company has an annual budget of ₱600,000,000. It spends 45% on fleet maintenance, 25% on salaries, and 15% on warehouse operations. How much remains for expansion?", A = "₱60,000,000", B = "₱80,000,000", C = "₱90,000,000", D = "₱100,000,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A supermarket chain earns ₱500,000,000 annually. It allocates 55% to purchasing goods, 20% to employee salaries, and 10% to marketing. How much is left as net income?", A = "₱50,000,000", B = "₱75,000,000", C = "₱100,000,000", D = "₱125,000,000", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A pharmaceutical company has a yearly revenue of ₱1,500,000,000. If 50% is allocated for research and development, 30% for production, and 10% for marketing, how much is left for dividends?", A = "₱100,000,000", B = "₱150,000,000", C = "₱200,000,000", D = "₱250,000,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A publishing company makes ₱30,000,000 per year. It spends 40% on printing, 25% on distribution, and 20% on marketing. How much is left as net earnings?", A = "₱3,000,000", B = "₱4,500,000", C = "₱5,000,000", D = "₱6,000,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A theme park earns ₱50,000,000 per month. It spends 50% on maintenance, 30% on employee salaries, and 10% on advertising. How much is left as profit?", A = "₱3,000,000", B = "₱4,000,000", C = "₱5,000,000", D = "₱6,000,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A fitness gym makes ₱2,500,000 per month. If 40% is spent on equipment, 30% on rent, and 20% on salaries, how much remains for expansion?", A = "₱100,000", B = "₱200,000", C = "₱250,000", D = "₱300,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A luxury car dealership earns ₱100,000,000 annually. It spends 60% on acquiring vehicles, 20% on staff salaries, and 10% on marketing. How much is left as net profit?", A = "₱5,000,000", B = "₱10,000,000", C = "₱15,000,000", D = "₱20,000,000", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A national bank earns ₱3,000,000,000 per year. It allocates 50% to loans, 30% to salaries, and 10% to infrastructure. How much is left for future investments?", A = "₱200,000,000", B = "₱250,000,000", C = "₱300,000,000", D = "₱350,000,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A water utility company has a budget of ₱700,000,000. If it spends 40% on operations, 30% on expansion, and 20% on salaries, how much is left for emergency funds?", A = "₱50,000,000", B = "₱60,000,000", C = "₱70,000,000", D = "₱80,000,000", CorrectAnswer = "C" },
                new Question { QuestionText = "A mobile network provider earns ₱4,000,000,000 per year. It allocates 55% to infrastructure, 25% to customer service, and 15% to marketing. How much remains for research?", A = "₱100,000,000", B = "₱200,000,000", C = "₱300,000,000", D = "₱400,000,000", CorrectAnswer = "B" },
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
                FormTitle = "Budjet Problem (Hard)",
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
                FormTitle = "Budget Problem (Hard)",
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

