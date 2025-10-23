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
    public partial class TimeChallengeMedium : Form
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
        public TimeChallengeMedium()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "Ilang minuto mayroon sa 2 oras?", A = "60 minuto", B = "90 minuto", C = "120 minuto", D = "180 minuto", CorrectAnswer = "C" },
                new Question { QuestionText = "Kailangan mong 30 minuto para matapos ang isang gawain at nagsimula ka ng 2:15 PM. Anong oras ka matatapos?", A = "2:30 PM", B = "2:45 PM", C = "2:50 PM", D = "2:55 PM", CorrectAnswer = "B" },
                new Question { QuestionText = "Gaano katagal ang isang-kapat (quarter) ng isang oras?", A = "5 minuto", B = "10 minuto", C = "15 minuto", D = "30 minuto", CorrectAnswer = "C" },
                new Question { QuestionText = "May pulong mula 3:00 PM na tumagal ng 45 minuto. Anong oras ito natapos?", A = "3:45 PM", B = "4:00 PM", C = "4:15 PM", D = "4:30 PM", CorrectAnswer = "A" },
                new Question { QuestionText = "Nagtrabaho ka ng 45 minuto sa isang gawain at 30 minuto sa isa pa. Gaano katagal ang kabuuang ginugol?", A = "75 minuto", B = "70 minuto", C = "80 minuto", D = "90 minuto", CorrectAnswer = "A" },

                new Question { QuestionText = "May 2-oras na pahinga. Gumugol ka ng 45 minuto sa pagkain at 30 minuto sa pag‑browse. Ilang minuto ang natitira?", A = "60 minuto", B = "45 minuto", C = "50 minuto", D = "55 minuto", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung ang bawat gawain ay 20 minuto at gagawin mo ang 3 gawain, gaano katagal ka magtatrabaho?", A = "50 minuto", B = "60 minuto", C = "70 minuto", D = "80 minuto", CorrectAnswer = "B" },
                new Question { QuestionText = "Isang gawain ay tumatagal ng 25 minuto. Ilang gawain ang matatapos mo sa loob ng 2 oras?", A = "3 gawain", B = "4 gawain", C = "5 gawain", D = "6 gawain", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pinakamahusay na paraan ng pamamahala ng oras kung maraming gawain na magkakalapit ang deadline?", A = "Mag‑focus sa isang gawain nang paisa‑isa", B = "Gawin lahat sabay‑sabay", C = "Tapusin muna ang pinakamadali", D = "Ibigay sa iba lahat", CorrectAnswer = "A" },
                new Question { QuestionText = "May 90 minuto bago ang deadline at may dalawang gawain: A = 40 minuto, B = 50 minuto. Ano ang unang dapat tapusin para mag‑manage ng oras nang maayos?", A = "Tapusin muna ang Gawain A", B = "Tapusin muna ang Gawain B", C = "Gawin sabay ang dalawang gawain", D = "Tapusin ayon sa pakiramdam", CorrectAnswer = "A" },

                new Question { QuestionText = "Umalis ang tren ng 9:40 AM at dumating 3 oras at 25 minuto pagkatapos. Anong oras ito dumating?", A = "12:00 PM", B = "12:05 PM", C = "12:15 PM", D = "12:30 PM", CorrectAnswer = "C" },
                new Question { QuestionText = "Nagsimula kang mag‑aral ng 2:15 PM at natapos ng 5:50 PM. Gaano katagal kang nag‑aral?", A = "2 oras 35 minuto", B = "3 oras 25 minuto", C = "3 oras 35 minuto", D = "4 oras", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang panaderya ay bukas mula 6:45 AM hanggang 7:30 PM. Ilang oras at minuto ito bukas sa isang araw?", A = "12 oras 45 minuto", B = "13 oras 45 minuto", C = "14 oras 15 minuto", D = "15 oras", CorrectAnswer = "B" },
                new Question { QuestionText = "I‑convert ang 3:45 PM sa 24‑hour format.", A = "15:45", B = "16:45", C = "17:45", D = "18:45", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung ang oras ay 10:55 AM, ilang minuto pa hanggang mag‑12:00 PM?", A = "55 minuto", B = "60 minuto", C = "65 minuto", D = "70 minuto", CorrectAnswer = "C" },

                new Question { QuestionText = "Nag‑break ka mula 11:20 AM nang 40 minuto. Anong oras ka bumalik sa trabaho?", A = "11:50 AM", B = "12:00 PM", C = "12:10 PM", D = "12:20 PM", CorrectAnswer = "C" },
                new Question { QuestionText = "Kung 1 araw = 24 oras, ilan ang oras sa loob ng 5 araw?", A = "100", B = "110", C = "120", D = "125", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang palaro sa barangay ay tumagal ng 1 oras 45 minuto at nagsimula ng 3:30 PM. Anong oras ito natapos?", A = "4:45 PM", B = "5:00 PM", C = "5:15 PM", D = "5:30 PM", CorrectAnswer = "C" },
                new Question { QuestionText = "May conference na nagsimula 9:00 AM na may 4 session, bawat session 1 oras 20 minuto. Anong oras ito matatapos?", A = "1:00 PM", B = "1:20 PM", C = "1:40 PM", D = "2:00 PM", CorrectAnswer = "C" },
                new Question { QuestionText = "I‑convert ang 135 minuto sa oras at minuto.", A = "2 oras 10 minuto", B = "2 oras 15 minuto", C = "2 oras 20 minuto", D = "2 oras 30 minuto", CorrectAnswer = "B" },

                new Question { QuestionText = "May pulong mula 2:10 PM hanggang 4:05 PM. Gaano ito katagal?", A = "1 oras 45 minuto", B = "1 oras 55 minuto", C = "2 oras", D = "2 oras 5 minuto", CorrectAnswer = "B" },
                new Question { QuestionText = "Lumipad ang eroplano ng 11:30 PM at tumagal ng 6 oras 40 minuto. Anong oras ang paglapag (24‑hour format)?", A = "05:40", B = "06:10", C = "06:30", D = "06:40", CorrectAnswer = "D" },
                new Question { QuestionText = "I‑convert ang 8:20 AM sa 24‑hour format.", A = "08:20", B = "09:20", C = "10:20", D = "11:20", CorrectAnswer = "A" },
                new Question { QuestionText = "Ilang linggo mayroon sa 100 araw (approx)?", A = "12 linggo", B = "14 linggo", C = "15 linggo", D = "16 linggo", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang shift ng manggagawa ay nagsimula 7:45 AM at nagtapos 3:15 PM. Gaano kahaba ang shift?", A = "7 oras", B = "7 oras 30 minuto", C = "8 oras", D = "8 oras 30 minuto", CorrectAnswer = "D" },

                new Question { QuestionText = "Ilang oras at minuto ang 90 minuto?", A = "1 oras 20 minuto", B = "1 oras 30 minuto", C = "1 oras 40 minuto", D = "2 oras", CorrectAnswer = "B" },
                new Question { QuestionText = "Kung bukas ang tindahan ng 11 oras sa isang araw, ilang oras ito bukas sa isang linggo?", A = "60", B = "70", C = "77", D = "80", CorrectAnswer = "C" },
                new Question { QuestionText = "Sumakay ka ng bus 4:05 PM at 1 oras 25 minuto ang biyahe. Anong oras ka darating?", A = "5:10 PM", B = "5:20 PM", C = "5:30 PM", D = "5:45 PM", CorrectAnswer = "C" },
                new Question { QuestionText = "I‑convert ang 7200 segundo sa oras.", A = "1 oras", B = "2 oras", C = "2 oras 30 minuto", D = "3 oras", CorrectAnswer = "B" },
                new Question { QuestionText = "Kung ang byahe ay nagsimula 7:25 AM at tumagal ng 3 oras 40 minuto, anong oras ito natapos?", A = "10:50 AM", B = "11:00 AM", C = "11:05 AM", D = "11:10 AM", CorrectAnswer = "C" },

                new Question { QuestionText = "Ang biyahe ng tren ay 5 oras 45 minuto. Umalis ito 9:15 AM. Anong oras dumating?", A = "2:45 PM", B = "3:00 PM", C = "3:15 PM", D = "3:30 PM", CorrectAnswer = "C" },
                new Question { QuestionText = "I‑convert ang 5 oras 20 minuto sa minuto.", A = "300 minuto", B = "320 minuto", C = "340 minuto", D = "360 minuto", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang klase ay nagsimula 10:50 AM at tumagal ng 75 minuto. Anong oras ito natapos?", A = "11:50 AM", B = "12:00 PM", C = "12:05 PM", D = "12:15 PM", CorrectAnswer = "C" },
                new Question { QuestionText = "Matulog ka ng 10:20 PM at nagising pagkatapos ng 7 oras. Anong oras ka nagising?", A = "5:00 AM", B = "5:10 AM", C = "5:20 AM", D = "5:30 AM", CorrectAnswer = "C" },
                new Question { QuestionText = "Nagsimula ang karera ng 8:15 AM at tumagal ng 3 oras 50 minuto. Anong oras ito natapos?", A = "11:50 AM", B = "12:00 PM", C = "12:05 PM", D = "12:15 PM", CorrectAnswer = "C" },

                new Question { QuestionText = "Nagsimula ang pelikula 6:40 PM at tumagal ng 2 oras 15 minuto. Anong oras ito natapos?", A = "8:45 PM", B = "8:50 PM", C = "9:00 PM", D = "9:05 PM", CorrectAnswer = "D" },
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
                FormTitle = "Time Challenge (Medium)",
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
                FormTitle = "Time Challenge (Medium)",
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