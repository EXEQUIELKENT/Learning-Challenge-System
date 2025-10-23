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
                new Question { QuestionText = "Umalis ang tren ng 9:20 AM at dumating matapos ang 8 oras 15 minuto. Anong oras ito dumating?", A = "5:25 PM", B = "5:35 PM", C = "5:40 PM", D = "5:50 PM", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang isang spacecraft ay tumagal ng 4 araw, 7 oras, at 25 minuto para makarating sa Mars. I-convert ito sa kabuuang oras.", A = "103 hours", B = "104 hours", C = "105 hours", D = "106 hours", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang isang TV episode ay tumatagal ng 42 minuto. Kung manonood ka ng 15 episodes ng sunod‑sunod, gaano katagal ang iyong panonood?", A = "10 hours 10 minutes", B = "10 hours 20 minutes", C = "10 hours 30 minutes", D = "10 hours 40 minutes", CorrectAnswer = "B" },
                new Question { QuestionText = "Umalis ang ferry ng 11:55 PM at tumagal ng 6 oras 50 minuto bago dumating. Anong oras ang pagdating (24‑hour format)?", A = "06:35", B = "06:45", C = "07:05", D = "07:15", CorrectAnswer = "C" },
                new Question { QuestionText = "Isang kompanya ang gumagana ng 16 oras kada araw, 6 na araw kada linggo. Ilang oras ito gumagana sa isang hindi‑leap na taon?", A = "4,992 hours", B = "5,012 hours", C = "5,032 hours", D = "5,052 hours", CorrectAnswer = "A" },

                new Question { QuestionText = "Umalis ang eroplano ng 3:25 AM at tumagal ng 13 oras 45 minuto. Anong oras ang paglapag (24‑hour format)?", A = "17:00", B = "17:05", C = "17:10", D = "17:15", CorrectAnswer = "A" },
                new Question { QuestionText = "May deadline ka sa loob ng 11 linggo. Ilang araw ang mayroon ka kabuuan?", A = "76 days", B = "77 days", C = "78 days", D = "79 days", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang siklista ay tumatakbo ng 18 miles kada oras. Gaano katagal bago matapos ang 216‑mile na ruta?", A = "11 hours", B = "12 hours", C = "13 hours", D = "14 hours", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang planta ng kuryente ay nagpapatakbo ng 23 oras 15 minuto bawat araw. Ilang minuto ito tumatakbo sa loob ng 30 araw?", A = "41,750 minutes", B = "41,800 minutes", C = "41,850 minutes", D = "41,900 minutes", CorrectAnswer = "C" },
                new Question { QuestionText = "Nag‑aaral ang isang estudyante ng 3 oras 40 minuto ng umaga at 4 oras 15 minuto ng gabi. Gaano ang kabuuang oras ng pag‑aaral bawat araw?", A = "7 hours 45 minutes", B = "7 hours 50 minutes", C = "7 hours 55 minutes", D = "8 hours 0 minutes", CorrectAnswer = "C" },

                new Question { QuestionText = "Mag‑uupdate ng software na tatagal ng 3 oras 55 minuto. Kung magsisimula ito ng 8:35 PM, anong oras ito matatapos?", A = "12:25 AM", B = "12:30 AM", C = "12:35 AM", D = "12:40 AM", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang orasan ay nadadagdagan ng 2 minuto kada oras. Ilang dagdag na minuto ang makikita nito pagkatapos ng 36 oras?", A = "68 minutes", B = "70 minutes", C = "72 minutes", D = "74 minutes", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang spaceship ay naglalakbay ng 25,000 km/h. Gaano katagal bago tumawid ng 600,000 km?", A = "22 hours", B = "23 hours", C = "24 hours", D = "25 hours", CorrectAnswer = "C" },
                new Question { QuestionText = "Nag‑eehersisyo ang isang tao ng 35 minuto araw‑araw. Ilang oras ng ehersisyo ang matatapos niya sa loob ng 8 linggo?", A = "31 hours 10 minutes", B = "31 hours 20 minutes", C = "31 hours 30 minutes", D = "31 hours 40 minutes", CorrectAnswer = "B" },
                new Question { QuestionText = "May event na tatagal ng 17 oras 25 minuto at magsisimula ng 5:10 AM. Anong oras ito matatapos?", A = "10:30 PM", B = "10:35 PM", C = "10:40 PM", D = "10:45 PM", CorrectAnswer = "B" },

                new Question { QuestionText = "Ang bullet train ay tumakbo ng 560 km sa loob ng 4 oras. Ano ang average speed nito?", A = "135 km/h", B = "140 km/h", C = "145 km/h", D = "150 km/h", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang isang Martian day ay 24 oras 39 minuto. Ilang Earth minutes mayroon sa 10 Martian days?", A = "14,200 minutes", B = "14,350 minutes", C = "14,390 minutes", D = "14,440 minutes", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang high‑speed train ay tumatagal ng 9 oras 30 minuto. Kung umalis ito ng 10:20 PM, anong oras ito darating?", A = "7:30 AM", B = "7:40 AM", C = "7:50 AM", D = "8:00 AM", CorrectAnswer = "B" },
                new Question { QuestionText = "Movie marathon: 5 pelikula, bawat isa 2 oras 15 minuto. Gaano katagal ang kabuuan?", A = "10 hours 45 minutes", B = "11 hours 00 minutes", C = "11 hours 15 minutes", D = "11 hours 30 minutes", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang pabrika ay tumatakbo 7 araw bawat linggo at 22 oras kada araw. Ilang oras ito tumatakbo sa loob ng 2 linggo?", A = "308 hours", B = "309 hours", C = "310 hours", D = "311 hours", CorrectAnswer = "A" },

                new Question { QuestionText = "Umalis ang bus ng 11:50 PM at tumagal ng 7 oras 25 minuto. Anong oras ang pagdating (24‑hour format)?", A = "06:05", B = "06:15", C = "07:05", D = "07:15", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang flight ay umalis ng 9:45 PM at tumagal ng 10 oras 35 minuto. Anong oras ang paglapag?", A = "8:10 AM", B = "8:20 AM", C = "8:30 AM", D = "8:40 AM", CorrectAnswer = "B" },
                new Question { QuestionText = "May deadline na 5 araw, 12 oras, at 45 minuto. Ilang kabuuang minuto meron ka?", A = "8,345", B = "8,365", C = "8,385", D = "8,405", CorrectAnswer = "C" },
                new Question { QuestionText = "Umalis ang tren mula City A 8:35 AM, tumigil sa City B ng 25 minuto, at nagpatuloy pa ng 6 oras 50 minuto patungong City C. Anong oras dumating sa City C?", A = "3:30 PM", B = "3:40 PM", C = "4:00 PM", D = "4:10 PM", CorrectAnswer = "B" },
                new Question { QuestionText = "Nagsimula ang road trip ng 11:25 AM. Nagmaneho ka ng 4 oras 40 minuto, nag‑lunch 1 oras 15 minuto, at nagmaneho uli ng 3 oras 20 minuto. Anong oras ka dumating?", A = "8:20 PM", B = "8:30 PM", C = "8:40 PM", D = "8:50 PM", CorrectAnswer = "C" },

                new Question { QuestionText = "I‑convert ang 567,000 segundo sa araw, oras, minuto, at segundo.", A = "6 days 13 hours 50 minutes 0 seconds", B = "6 days 13 hours 55 minutes 0 seconds", C = "6 days 14 hours 50 minutes 0 seconds", D = "6 days 14 hours 55 minutes 0 seconds", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang estudyante ay kumuha ng exam 9:15 AM hanggang 12:05 PM, nag‑break ng 35 minuto, at nag‑study uli ng 2 oras 50 minuto. Anong oras natapos ang study session?", A = "3:20 PM", B = "3:30 PM", C = "3:40 PM", D = "3:50 PM", CorrectAnswer = "B" },
                new Question { QuestionText = "May pulong na tatagal ng 2 oras 40 minuto na nagsimula ng 2:55 PM. Anong oras ito matatapos?", A = "5:15 PM", B = "5:25 PM", C = "5:35 PM", D = "5:45 PM", CorrectAnswer = "B" },
                new Question { QuestionText = "Umalis ang bus 10:25 PM at dumating 4:50 AM kinabukasan. Gaano katagal ang biyahe?", A = "6 hours 15 minutes", B = "6 hours 25 minutes", C = "6 hours 35 minutes", D = "6 hours 45 minutes", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang proyekto ay tumagal ng 3 weeks, 2 days, 14 hours, at 30 minuto. I‑convert ang kabuuang tagal sa oras.", A = "602 hours", B = "614 hours", C = "626 hours", D = "638 hours", CorrectAnswer = "C" },

                new Question { QuestionText = "Ang pabrika ay tumatakbo 16 oras kada araw. Ilang oras ito tumatakbo sa isang hindi‑leap na taon?", A = "5,760 hours", B = "5,840 hours", C = "5,920 hours", D = "6,000 hours", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang spaceship ay naglakbay ng 7 araw, 18 oras, at 25 minuto. I‑convert ito sa kabuuang minuto.", A = "11,065", B = "11,085", C = "11,105", D = "11,125", CorrectAnswer = "D" },
                new Question { QuestionText = "Nagsimula ang marathon 6:40 AM at ang huling runner ay natapos matapos ang 4 oras 55 minuto. Anong oras ito opisyal na nagtapos?", A = "11:30 AM", B = "11:35 AM", C = "11:40 AM", D = "11:45 AM", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang cargo ship ay naglayag ng 10 knots kada oras. Kung naglayag ito ng 2 araw, 14 oras, at 30 minuto, gaano kalayo ang nalayag?", A = "640 knots", B = "650 knots", C = "660 knots", D = "670 knots", CorrectAnswer = "C" },
                new Question { QuestionText = "Kung 1 buwan = 4.33 linggo, ilang linggo mayroon sa 9 buwan?", A = "38.97 weeks", B = "39.87 weeks", C = "40.97 weeks", D = "41.87 weeks", CorrectAnswer = "A" },

                new Question { QuestionText = "I‑convert ang 15,300 segundo sa oras, minuto, at segundo.", A = "4 hours 15 minutes 0 seconds", B = "4 hours 15 minutes 20 seconds", C = "4 hours 15 minutes 30 seconds", D = "4 hours 15 minutes 40 seconds", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang shift ay nagsimula ng 11:15 PM at tumagal ng 9 oras 20 minuto. Anong oras ito nagtapos?", A = "8:30 AM", B = "8:35 AM", C = "8:40 AM", D = "8:45 AM", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang delivery truck ay umalis 4:45 AM, nagmaneho ng 6 oras 10 minuto, tumigil ng 50 minuto, at nagpatuloy ng 5 oras 30 minuto. Anong oras dumating sa huling destinasyon?", A = "5:00 PM", B = "5:10 PM", C = "5:15 PM", D = "5:20 PM", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang karera ay 3,600 segundo ang haba. Ilang minuto at segundo ito?", A = "58 minutes 40 seconds", B = "59 minutes 30 seconds", C = "60 minutes 0 seconds", D = "61 minutes 10 seconds", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang warehouse ay bukas 24 oras araw‑araw ngunit tuwing Linggo isinasara ito ng 5 oras. Ilang oras ito bukas sa loob ng 31-araw na buwan?", A = "695 hours", B = "705 hours", C = "715 hours", D = "725 hours", CorrectAnswer = "B" },
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
