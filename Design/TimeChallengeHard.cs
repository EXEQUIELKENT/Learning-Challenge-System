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
    public partial class TimeChallengeHard : Form
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
        public TimeChallengeHard()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "A train departs at 9:20 AM and reaches its destination after 8 hours 15 minutes. What time does it arrive?", A = "5:25 PM", B = "5:35 PM", C = "5:40 PM", D = "5:50 PM", CorrectAnswer = "A" },
                new Question { QuestionText = "A spacecraft takes 4 days, 7 hours, and 25 minutes to reach Mars. Convert this time into total hours.", A = "103 hours", B = "104 hours", C = "105 hours", D = "106 hours", CorrectAnswer = "C" },
                new Question { QuestionText = "A TV show runs for 42 minutes per episode. If you binge-watch 15 episodes, how much total time do you spend watching?", A = "10 hours 10 minutes", B = "10 hours 20 minutes", C = "10 hours 30 minutes", D = "10 hours 40 minutes", CorrectAnswer = "B" },
                new Question { QuestionText = "A ferry leaves at 11:55 PM and reaches its destination after 6 hours and 50 minutes. What time does it arrive (24-hour format)?", A = "06:35", B = "06:45", C = "07:05", D = "07:15", CorrectAnswer = "C" },
                new Question { QuestionText = "A company operates 16 hours per day, 6 days a week. How many hours does it operate in a non-leap year?", A = "4,992 hours", B = "5,012 hours", C = "5,032 hours", D = "5,052 hours", CorrectAnswer = "A" },
                
                new Question { QuestionText = "A plane takes off at 3:25 AM and lands after 13 hours 45 minutes. What time does it land (24-hour format)?", A = "17:00", B = "17:05", C = "17:10", D = "17:15", CorrectAnswer = "A" },
                new Question { QuestionText = "You need to submit a project 11 weeks from today. How many total days do you have?", A = "76 days", B = "77 days", C = "78 days", D = "79 days", CorrectAnswer = "B" },
                new Question { QuestionText = "A cyclist maintains a speed of 18 miles per hour. How long will it take to complete a 216-mile journey?", A = "11 hours", B = "12 hours", C = "13 hours", D = "14 hours", CorrectAnswer = "B" },
                new Question { QuestionText = "A power plant produces electricity for 23 hours 15 minutes each day. How many total minutes does it operate in 30 days?", A = "41,750 minutes", B = "41,800 minutes", C = "41,850 minutes", D = "41,900 minutes", CorrectAnswer = "C" },
                new Question { QuestionText = "A student studies for 3 hours 40 minutes in the morning and 4 hours 15 minutes in the evening. How much total study time per day?", A = "7 hours 45 minutes", B = "7 hours 50 minutes", C = "7 hours 55 minutes", D = "8 hours 0 minutes", CorrectAnswer = "C" },
                
                new Question { QuestionText = "A software update will take 3 hours 55 minutes. If it starts at 8:35 PM, when will it finish?", A = "12:25 AM", B = "12:30 AM", C = "12:35 AM", D = "12:40 AM", CorrectAnswer = "C" },
                new Question { QuestionText = "A clock gains 2 minutes every hour. How many extra minutes will it show after 36 hours?", A = "68 minutes", B = "70 minutes", C = "72 minutes", D = "74 minutes", CorrectAnswer = "C" },
                new Question { QuestionText = "A spaceship travels at 25,000 km/h. How long does it take to travel 600,000 km?", A = "22 hours", B = "23 hours", C = "24 hours", D = "25 hours", CorrectAnswer = "C" },
                new Question { QuestionText = "A person spends 35 minutes exercising every day. How many total hours of exercise do they complete in 8 weeks?", A = "31 hours 10 minutes", B = "31 hours 20 minutes", C = "31 hours 30 minutes", D = "31 hours 40 minutes", CorrectAnswer = "B" },
                new Question { QuestionText = "An event is scheduled to last for 17 hours and 25 minutes. If it starts at 5:10 AM, what time will it end?", A = "10:30 PM", B = "10:35 PM", C = "10:40 PM", D = "10:45 PM", CorrectAnswer = "B" },
                
                new Question { QuestionText = "A bullet train travels 560 km in 4 hours. What is its average speed?", A = "135 km/h", B = "140 km/h", C = "145 km/h", D = "150 km/h", CorrectAnswer = "B" },
                new Question { QuestionText = "A day on Mars lasts 24 hours 39 minutes. How many total Earth minutes are in 10 Martian days?", A = "14,200 minutes", B = "14,350 minutes", C = "14,390 minutes", D = "14,440 minutes", CorrectAnswer = "C" },
                new Question { QuestionText = "A high-speed train takes 9 hours 30 minutes for a trip. If it starts at 10:20 PM, what time does it reach its destination?", A = "7:30 AM", B = "7:40 AM", C = "7:50 AM", D = "8:00 AM", CorrectAnswer = "B" },
                new Question { QuestionText = "A movie marathon consists of 5 movies, each lasting 2 hours 15 minutes. How long is the total viewing time?", A = "10 hours 45 minutes", B = "11 hours 00 minutes", C = "11 hours 15 minutes", D = "11 hours 30 minutes", CorrectAnswer = "C" },
                new Question { QuestionText = "A factory operates 7 days a week and runs for 22 hours daily. How many total hours does it operate in 2 weeks?", A = "308 hours", B = "309 hours", C = "310 hours", D = "311 hours", CorrectAnswer = "A" },
                
                new Question { QuestionText = "A bus leaves the terminal at 11:50 PM and reaches its destination after 7 hours and 25 minutes. What time does it arrive (24-hour format)?", A = "06:05", B = "06:15", C = "07:05", D = "07:15", CorrectAnswer = "C" },
                new Question { QuestionText = "A flight departs at 9:45 PM and takes 10 hours 35 minutes to reach its destination. What time does it land?", A = "8:10 AM", B = "8:20 AM", C = "8:30 AM", D = "8:40 AM", CorrectAnswer = "B" },
                new Question { QuestionText = "You are given a deadline of 5 days, 12 hours, and 45 minutes. How many total minutes do you have?", A = "8,345", B = "8,365", C = "8,385", D = "8,405", CorrectAnswer = "C" },
                new Question { QuestionText = "A train leaves City A at 8:35 AM. It stops at City B for 25 minutes before continuing for another 6 hours and 50 minutes to City C. What time does it arrive in City C?", A = "3:30 PM", B = "3:40 PM", C = "4:00 PM", D = "4:10 PM", CorrectAnswer = "B" },
                new Question { QuestionText = "You start a road trip at 11:25 AM. You drive for 4 hours 40 minutes, stop for lunch for 1 hour 15 minutes, and then continue driving for 3 hours 20 minutes. What time do you reach your destination?", A = "8:20 PM", B = "8:30 PM", C = "8:40 PM", D = "8:50 PM", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Convert 567,000 seconds into days, hours, minutes, and seconds.", A = "6 days 13 hours 50 minutes 0 seconds", B = "6 days 13 hours 55 minutes 0 seconds", C = "6 days 14 hours 50 minutes 0 seconds", D = "6 days 14 hours 55 minutes 0 seconds", CorrectAnswer = "B" },
                new Question { QuestionText = "A student takes a test from 9:15 AM to 12:05 PM, takes a break for 35 minutes, and then studies for another 2 hours 50 minutes. What time does the study session end?", A = "3:20 PM", B = "3:30 PM", C = "3:40 PM", D = "3:50 PM", CorrectAnswer = "B" },
                new Question { QuestionText = "If a meeting is scheduled to last 2 hours and 40 minutes and it starts at 2:55 PM, what time will it end?", A = "5:15 PM", B = "5:25 PM", C = "5:35 PM", D = "5:45 PM", CorrectAnswer = "B" },
                new Question { QuestionText = "A bus departs at 10:25 PM and reaches its destination at 4:50 AM the next day. How long was the journey?", A = "6 hours 15 minutes", B = "6 hours 25 minutes", C = "6 hours 35 minutes", D = "6 hours 45 minutes", CorrectAnswer = "C" },
                new Question { QuestionText = "A project takes 3 weeks, 2 days, 14 hours, and 30 minutes to complete. Convert the total duration into hours.", A = "602 hours", B = "614 hours", C = "626 hours", D = "638 hours", CorrectAnswer = "C" },
                
                new Question { QuestionText = "A factory operates for 16 hours a day. How many hours does it run in a non-leap year?", A = "5,760 hours", B = "5,840 hours", C = "5,920 hours", D = "6,000 hours", CorrectAnswer = "A" },
                new Question { QuestionText = "A spaceship travels for 7 days, 18 hours, and 25 minutes. Convert this to total minutes.", A = "11,065", B = "11,085", C = "11,105", D = "11,125", CorrectAnswer = "D" },
                new Question { QuestionText = "A marathon starts at 6:40 AM and the last runner finishes after 4 hours and 55 minutes. What time does the race officially end?", A = "11:30 AM", B = "11:35 AM", C = "11:40 AM", D = "11:45 AM", CorrectAnswer = "B" },
                new Question { QuestionText = "A cargo ship sails at 10 knots per hour. If it travels for 2 days, 14 hours, and 30 minutes, how far does it travel?", A = "640 knots", B = "650 knots", C = "660 knots", D = "670 knots", CorrectAnswer = "C" },
                new Question { QuestionText = "If 1 month = 4.33 weeks, how many weeks are in 9 months?", A = "38.97 weeks", B = "39.87 weeks", C = "40.97 weeks", D = "41.87 weeks", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Convert 15,300 seconds into hours, minutes, and seconds.", A = "4 hours 15 minutes 0 seconds", B = "4 hours 15 minutes 20 seconds", C = "4 hours 15 minutes 30 seconds", D = "4 hours 15 minutes 40 seconds", CorrectAnswer = "C" },
                new Question { QuestionText = "A shift starts at 11:15 PM and lasts for 9 hours and 20 minutes. What time does it end?", A = "8:30 AM", B = "8:35 AM", C = "8:40 AM", D = "8:45 AM", CorrectAnswer = "B" },
                new Question { QuestionText = "A delivery truck leaves at 4:45 AM and drives for 6 hours and 10 minutes before stopping for 50 minutes. It then resumes for another 5 hours 30 minutes. What time does it reach its final stop?", A = "5:00 PM", B = "5:10 PM", C = "5:15 PM", D = "5:20 PM", CorrectAnswer = "B" },
                new Question { QuestionText = "A race is 3,600 seconds long. How many minutes and seconds is that?", A = "58 minutes 40 seconds", B = "59 minutes 30 seconds", C = "60 minutes 0 seconds", D = "61 minutes 10 seconds", CorrectAnswer = "C" },
                new Question { QuestionText = "A warehouse operates 24 hours a day, but every Sunday it closes for 5 hours. How many hours does it operate in a 31-day month?", A = "695 hours", B = "705 hours", C = "715 hours", D = "725 hours", CorrectAnswer = "B" },
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
                FormTitle = "Time Challenge (Hard)",
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
                FormTitle = "Time Challenge (Hard)",
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
