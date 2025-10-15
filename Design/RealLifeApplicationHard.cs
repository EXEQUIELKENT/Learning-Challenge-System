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
    public partial class RealLifeApplicationHard : Form
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
        public RealLifeApplicationHard()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "If you want to build a fence around a rectangular garden with a length of 15 meters and a width of 10 meters, how much fencing material will you need?", A = "30 meters", B = "40 meters", C = "50 meters", D = "60 meters", CorrectAnswer = "B" },
                new Question { QuestionText = "You are planning to buy paint for a room with an area of 150 square feet. If one gallon of paint covers 350 square feet, how many gallons do you need?", A = "1 gallon", B = "2 gallons", C = "3 gallons", D = "4 gallons", CorrectAnswer = "B" },
                new Question { QuestionText = "A car travels 180 miles in 3 hours. What is the average speed of the car?", A = "45 miles per hour", B = "50 miles per hour", C = "60 miles per hour", D = "70 miles per hour", CorrectAnswer = "C" },
                new Question { QuestionText = "You are buying a laptop that costs $800. The store offers a 20% discount. How much will you pay for the laptop after the discount?", A = "$600", B = "$640", C = "$700", D = "$720", CorrectAnswer = "B" },
                new Question { QuestionText = "A worker is paid $15 per hour. If they work 40 hours per week, how much will they earn in a month (4 weeks)?", A = "$2,000", B = "$2,400", C = "$2,600", D = "$3,000", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A company increases the price of its product by 15%. If the original price was $50, what is the new price?", A = "$55", B = "$57.50", C = "$60", D = "$62.50", CorrectAnswer = "B" },
                new Question { QuestionText = "You are planning to take a trip that will require 360 miles of travel. If your car uses 25 miles per gallon and the price of gas is $3 per gallon, how much will you spend on gas?", A = "$30", B = "$36", C = "$40", D = "$45", CorrectAnswer = "B" },
                new Question { QuestionText = "A recipe calls for 2 cups of sugar, but you want to make half of the recipe. How much sugar should you use?", A = "1 cup", B = "1.5 cups", C = "2 cups", D = "2.5 cups", CorrectAnswer = "A" },
                new Question { QuestionText = "A store sells a shirt for $25. The shirt is on sale with a 30% discount. How much is the discount?", A = "$5", B = "$7.50", C = "$8", D = "$10", CorrectAnswer = "B" },
                new Question { QuestionText = "You want to buy a new phone that costs $600. The store offers a 12-month installment plan with 0% interest. How much will you pay each month?", A = "$50", B = "$55", C = "$60", D = "$65", CorrectAnswer = "A" },
                
                new Question { QuestionText = "If a product sells for $120 and the sales tax is 8%, what is the total price you will pay for the product?", A = "$128", B = "$130", C = "$132", D = "$135", CorrectAnswer = "A" },
                new Question { QuestionText = "You need to invest $5,000 in an account that offers an annual interest rate of 5%. How much interest will you earn after 3 years?", A = "$250", B = "$500", C = "$750", D = "$1,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A contractor needs to build a driveway with a total area of 400 square feet. If the concrete costs $3 per square foot, how much will the driveway cost?", A = "$1,000", B = "$1,200", C = "$1,500", D = "$2,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A cyclist is training for a race and rides 40 miles in 2 hours. How long will it take the cyclist to ride 100 miles at the same pace?", A = "4 hours", B = "5 hours", C = "6 hours", D = "8 hours", CorrectAnswer = "B" },
                new Question { QuestionText = "You are decorating a wall that is 10 feet long and 8 feet high. If wallpaper costs $15 per square meter, how much will it cost to cover the wall? (1 square meter = 10.764 square feet)", A = "$10", B = "$12", C = "$15", D = "$18", CorrectAnswer = "C" },
                
                new Question { QuestionText = "A store sells shoes for $60 a pair. If you want to buy 5 pairs, how much will you spend before taxes?", A = "$250", B = "$270", C = "$280", D = "$300", CorrectAnswer = "A" },
                new Question { QuestionText = "A group of friends went to a restaurant and the total bill was $200. If they split the bill equally among 4 people, how much does each person pay?", A = "$45", B = "$50", C = "$55", D = "$60", CorrectAnswer = "B" },
                new Question { QuestionText = "A student earns a grade of 85% on a test with 40 questions. How many questions did the student answer correctly?", A = "32", B = "34", C = "36", D = "38", CorrectAnswer = "A" },
                new Question { QuestionText = "You are saving money for a trip. You want to save $1,500 in 6 months. How much money do you need to save each month?", A = "$250", B = "$300", C = "$350", D = "$400", CorrectAnswer = "B" },
                new Question { QuestionText = "A car rental company charges $40 per day for renting a car. If you rent the car for 5 days, how much will you pay in total?", A = "$180", B = "$200", C = "$220", D = "$240", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A rectangular swimming pool is 25 meters long and 10 meters wide. If the depth is 2 meters, how much water (in cubic meters) is needed to fill it?", A = "500", B = "450", C = "600", D = "550", CorrectAnswer = "A" },
                new Question { QuestionText = "A bakery sells cakes at $35 each. If a customer buys 7 cakes and receives a 10% discount, how much will they pay?", A = "$200", B = "$220", C = "$225", D = "$230", CorrectAnswer = "B" },
                new Question { QuestionText = "A car depreciates in value by 12% each year. If the original price was $30,000, what is its value after 2 years?", A = "$23,232", B = "$24,576", C = "$25,000", D = "$26,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A train is traveling at a speed of 90 km/h. How long will it take to travel 360 km?", A = "3 hours", B = "4 hours", C = "4.5 hours", D = "5 hours", CorrectAnswer = "B" },
                new Question { QuestionText = "If a store applies a 25% markup on a product that originally costs $80, what is the final selling price?", A = "$90", B = "$95", C = "$100", D = "$105", CorrectAnswer = "C" },
                
                new Question { QuestionText = "A group of people is sharing a total bill of $360 equally among 8 people. How much does each person pay?", A = "$40", B = "$45", C = "$50", D = "$55", CorrectAnswer = "A" },
                new Question { QuestionText = "A cyclist travels at an average speed of 20 km/h for 5 hours. How far does he travel?", A = "80 km", B = "90 km", C = "100 km", D = "110 km", CorrectAnswer = "C" },
                new Question { QuestionText = "A cell phone plan costs $50 per month plus $0.10 per text message. If a person sends 300 messages, how much is the total bill?", A = "$60", B = "$70", C = "$80", D = "$90", CorrectAnswer = "B" },
                new Question { QuestionText = "A store offers a 15% discount on a $250 jacket. How much will you pay after the discount?", A = "$200", B = "$210", C = "$212.50", D = "$220", CorrectAnswer = "C" },
                new Question { QuestionText = "If a 5 kg bag of rice costs $25, what is the price per kilogram?", A = "$4", B = "$5", C = "$5.50", D = "$6", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A man invests $4,000 in a savings account that earns 4% annual interest. How much will the investment be worth after 3 years (simple interest)?", A = "$4,480", B = "$4,500", C = "$4,600", D = "$4,800", CorrectAnswer = "A" },
                new Question { QuestionText = "A farm has 12 cows, and each cow produces 15 liters of milk per day. How much milk is produced in a week?", A = "1,200 liters", B = "1,250 liters", C = "1,260 liters", D = "1,280 liters", CorrectAnswer = "C" },
                new Question { QuestionText = "A software developer works 40 hours per week at a rate of $30 per hour. How much does the developer earn per month (4 weeks)?", A = "$4,500", B = "$4,800", C = "$5,000", D = "$5,200", CorrectAnswer = "B" },
                new Question { QuestionText = "If a computer processes 500 files per minute, how many files can it process in 2.5 hours?", A = "50,000", B = "60,000", C = "70,000", D = "75,000", CorrectAnswer = "B" },
                new Question { QuestionText = "A school orders 25 tables at $40 each. If they receive a 10% bulk discount, how much is the total cost?", A = "$850", B = "$875", C = "$900", D = "$1,000", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A cyclist covers a distance of 120 km in 3 hours. What is his average speed?", A = "30 km/h", B = "35 km/h", C = "40 km/h", D = "45 km/h", CorrectAnswer = "C" },
                new Question { QuestionText = "A movie streaming service charges $12 per month. How much will you pay in a year?", A = "$120", B = "$132", C = "$144", D = "$156", CorrectAnswer = "C" },
                new Question { QuestionText = "A bottle contains 750 ml of juice. How many bottles are needed to fill 6 liters?", A = "6", B = "7", C = "8", D = "9", CorrectAnswer = "B" },
                new Question { QuestionText = "A train ticket costs $80. If a person gets a 20% discount, what is the new price?", A = "$60", B = "$64", C = "$66", D = "$68", CorrectAnswer = "B" },
                new Question { QuestionText = "A company has 1,500 employees. If 30% of them are working remotely, how many employees are working remotely?", A = "400", B = "425", C = "450", D = "500", CorrectAnswer = "C" },
                new Question { QuestionText = "A plane travels 3,600 km in 4 hours. What is its average speed?", A = "800 km/h", B = "850 km/h", C = "900 km/h", D = "1,000 km/h", CorrectAnswer = "C" },

                new Question { QuestionText = "A tree grows at a rate of 25 cm per year. How tall will it be after 12 years?", A = "280 cm", B = "290 cm", C = "300 cm", D = "310 cm", CorrectAnswer = "C" },
                new Question { QuestionText = "A hotel charges $120 per night. If you stay for 6 nights, how much will you pay?", A = "$600", B = "$650", C = "$700", D = "$720", CorrectAnswer = "D" },
                new Question { QuestionText = "A bakery makes 450 loaves of bread in 5 hours. How many loaves can they make in 8 hours?", A = "700", B = "720", C = "750", D = "800", CorrectAnswer = "B" },
                new Question { QuestionText = "A train departs at 6:15 AM and arrives at its destination at 10:45 AM. How long is the journey?", A = "3.5 hours", B = "4 hours", C = "4.5 hours", D = "5 hours", CorrectAnswer = "C" },
                new Question { QuestionText = "A student scores 85%, 78%, and 92% on three exams. What is the average score?", A = "81%", B = "84%", C = "85%", D = "86%", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A man walks at a speed of 5 km/h for 3 hours. How far does he travel?", A = "12 km", B = "13 km", C = "14 km", D = "15 km", CorrectAnswer = "D" },
                new Question { QuestionText = "A cellphone battery loses 5% charge per hour. If the battery starts at 90%, what is the battery level after 7 hours?", A = "50%", B = "55%", C = "60%", D = "65%", CorrectAnswer = "B" },
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
                FormTitle = "Real Life Application (Hard)",
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
                FormTitle = "Real Life Application (Hard)",
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
