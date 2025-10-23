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

    public partial class RealLifeApplicationEasy : Form
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
        public RealLifeApplicationEasy()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "Si Ana ay may baong ₱200. Gumastos siya ng ₱45 para sa pamasahe at ₱55 para sa meryenda. Magkano ang natira?", A = "₱90", B = "₱100", C = "₱85", D = "₱95", CorrectAnswer = "A" },
                new Question { QuestionText = "Nagbenta si Lola ng 3 tray ng itlog sa palengke. Bawat tray ay ₱180. Kung kumita siya ng ₱50 tubo, magkano ang kabuuang kita (kita = presyo ng benta)?", A = "₱540", B = "₱590", C = "₱600", D = "₱620", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang jeepney pamasahe ay ₱9. Kung gumastos si Marco ng ₱36 araw-araw sa pag-commute, ilang biyahe ang nagawa niya?", A = "4 biyahe", B = "3 biyahe", C = "5 biyahe", D = "6 biyahe", CorrectAnswer = "C" },
                new Question { QuestionText = "Bumili ang pamilya ng 5 kilo ng bigas ₱48/kg at gulay na ₱120. Kung may ₱500 sila, magkano ang natira?", A = "₱20", B = "₱40", C = "₱60", D = "₱80", CorrectAnswer = "C" },
                new Question { QuestionText = "Sa fiesta ng barangay, may ₱2,000 na pondo. Gumastos ₱650 para sa pagkain at ₱450 para sa dekorasyon. Magkano ang natira?", A = "₱850", B = "₱900", C = "₱1,000", D = "₱1,200", CorrectAnswer = "B" },

                new Question { QuestionText = "Si Liza nagtitinda ng pandesal at kumita ng ₱1,200 sa isang linggo. Kung 25% ang inilaan niya sa ipon, magkano ang inilaan sa ipon?", A = "₱250", B = "₱300", C = "₱350", D = "₱400", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang sari-sari store ay nag-order ng softdrinks na ₱1,200 kabuuan at ititinda na may 15% tubo. Magkano ang kabuuang bentahan?", A = "₱1,380", B = "₱1,300", C = "₱1,440", D = "₱1,500", CorrectAnswer = "A" },
                new Question { QuestionText = "Nagbayad si Juan ng tricycle na ₱60 para sa 3 biyaheng pareho ang presyo. Magkano ang presyo ng isang biyahe?", A = "₱18", B = "₱20", C = "₱25", D = "₱30", CorrectAnswer = "D" },
                new Question { QuestionText = "Si Nora bumili ng 2 kg mangga ₱120/kg at 1 tray itlog ₱180. Kung nagbayad siya ng ₱500, magkano ang sukli?", A = "₱80", B = "₱60", C = "₱40", D = "₱100", CorrectAnswer = "A" },
                new Question { QuestionText = "May ₱5,000 ang PTA. Nagbayad sila ng ₱1,250 para sa cleaning supplies at ₱2,000 para sa prizes. Magkano ang natira?", A = "₱1,750", B = "₱1,850", C = "₱1,500", D = "₱1,250", CorrectAnswer = "C" },

                new Question { QuestionText = "Si Carlo nag-ipon ng ₱3,600 sa 6 buwan. Kung pantay-pantay ang naiipon niya kada buwan, magkano kada buwan?", A = "₱600", B = "₱700", C = "₱500", D = "₱800", CorrectAnswer = "A" },
                new Question { QuestionText = "Sa palengke, ang isang prutas ay ₱80 pero may 20% diskwento. Magkano ang presyo matapos ang diskwento?", A = "₱64", B = "₱60", C = "₱68", D = "₱70", CorrectAnswer = "A" },
                new Question { QuestionText = "Isang pamilya kumain sa carinderia ng ₱420. Hatiin sa 7 miyembro, magkano ang babayaran bawat isa (pantay)?", A = "₱60", B = "₱70", C = "₱65", D = "₱75", CorrectAnswer = "B" },
                new Question { QuestionText = "Bumili si Aling Nena ng 10 kg bigas ₱48/kg at ulam ₱1,250. Kung naglaan siya ng ₱2,000, kulang o sobra at magkano?", A = "₱570 kulang", B = "₱570 sobra", C = "₱150 kulang", D = "₱150 sobra", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang prutas sa palengke ay 3 para sa ₱100. Kung bibili ka ng 9 prutas, magkano babayaran?", A = "₱300", B = "₱275", C = "₱320", D = "₱250", CorrectAnswer = "A" },

                new Question { QuestionText = "Si Maria bumili ng school uniform ₱450 at sapatos ₱1,200. Kung may ₱2,000 na inilaan, magkano ang natira o kulang?", A = "₱350 natira", B = "₱250 kulang", C = "₱350 kulang", D = "₱250 natira", CorrectAnswer = "A" },
                new Question { QuestionText = "Nag-ambag ang 15 mag-aaral ng pantay upang makalikom ng ₱3,900. Magkano ang ambag kada isa?", A = "₱260", B = "₱240", C = "₱280", D = "₱300", CorrectAnswer = "A" },
                new Question { QuestionText = "Si Jun bumili ng 5 lata ng bunga ng niyog ₱85 bawat isa. Kung nagbayad siya ng ₱500, magkano ang sukli?", A = "₱75", B = "₱125", C = "₱100", D = "₱150", CorrectAnswer = "A" },
                new Question { QuestionText = "Nagbayad ang pamilya ng entrance fee sa Intramuros ₱250 bawat isa para sa 4 katao. Magkano lahat?", A = "₱1,000", B = "₱900", C = "₱1,050", D = "₱1,200", CorrectAnswer = "A" },
                new Question { QuestionText = "May ₱8,000 para sa environmental cleanup. Ginamit ang 1/4 sa pagkain at 3/8 sa kagamitan. Magkano ang natitira?", A = "₱3,000", B = "₱3,250", C = "₱3,500", D = "₱4,000", CorrectAnswer = "B" },

                new Question { QuestionText = "Si Maya nag-ipon ng ₱2,400 sa loob ng 6 buwan. Kung ginamit niya ang 1/3 sa school project, magkano ang natira?", A = "₱1,600", B = "₱1,800", C = "₱1,200", D = "₱1,400", CorrectAnswer = "A" },
                new Question { QuestionText = "Sa tindahan, 1 pen ay ₱5. Kung bibili ka ng 12 pens, magkano ang babayaran?", A = "₱60", B = "₱55", C = "₱50", D = "₱65", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang pamilya ay may ₱25,000 na buwanang kita. Gumastos sila ₱8,000 renta, ₱6,000 pagkain at ₱2,000 kuryente. Magkano ang natira?", A = "₱9,000", B = "₱10,000", C = "₱8,500", D = "₱7,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Si Aling Rosa ay may paninda na ₱4,500. Kung kumita siya ng 10% tubo, magkano ang tubo?", A = "₱450", B = "₱400", C = "₱500", D = "₱350", CorrectAnswer = "A" },
                new Question { QuestionText = "Bumili si Pedro ng 3 notebooks ₱45 bawat isa at lapis set ₱60. Kung nagbayad siya ng ₱200, magkano ang sukli?", A = "₱5", B = "₱10", C = "₱15", D = "₱20", CorrectAnswer = "B" },

                new Question { QuestionText = "May ₱3,600 ang klase para sa stickers. Kailangan ng 12 packs ₱275 bawat pack. Kulang o sobra at magkano?", A = "₱700 kulang", B = "₱700 sobra", C = "₱300 kulang", D = "₱300 sobra", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang pamasahe sa jeepney ay ₱9. Kung gumastos ang estudyante ng ₱81 sa isang linggo, ilang biyahe ito?", A = "9 biyahe", B = "8 biyahe", C = "10 biyahe", D = "7 biyahe", CorrectAnswer = "A" },
                new Question { QuestionText = "Isang baker ang nagbenta ng 20 pandesal sa ₱7 bawat isa. Magkano ang kinita niya sa araw na iyon (bago gastos)?", A = "₱120", B = "₱140", C = "₱160", D = "₱100", CorrectAnswer = "B" },
                new Question { QuestionText = "Kung ang isang tindahan ay nagbigay ng 10% diskwento sa damit na ₱500, magkano ang babayaran?", A = "₱450", B = "₱400", C = "₱475", D = "₱480", CorrectAnswer = "A" },
                new Question { QuestionText = "Si Kuya nag-donate ng ₱2,500 sa relief at hinati sa 25 pamilya. Magkano ang natanggap ng bawat pamilya?", A = "₱75", B = "₱100", C = "₱125", D = "₱150", CorrectAnswer = "C" },

                new Question { QuestionText = "May pondo ₱10,000 para sa project. Gumastos ng ₱2,500 materyales, ₱3,000 pag-print at ₱1,200 dekorasyon. Magkano ang natira?", A = "₱3,300", B = "₱3,200", C = "₱2,300", D = "₱2,800", CorrectAnswer = "A" },
                new Question { QuestionText = "Bumili ng halo-halo sa tindahan ₱60 at tubig sa malamig na araw ₱30. Kung nagbayad ka ng ₱200, magkano ang sukli?", A = "₱110", B = "₱120", C = "₱130", D = "₱140", CorrectAnswer = "C" },
                new Question { QuestionText = "Si Ana nag-ipon ng ₱8,400 sa loob ng 6 buwan. Kung pantay-pantay ang naiipon, magkano kada buwan?", A = "₱1,200", B = "₱1,400", C = "₱1,600", D = "₱1,800", CorrectAnswer = "B" },
                new Question { QuestionText = "Kung ang isang karton ng pandesal ay 6 piraso at nagkakahalaga ng ₱30, magkano ang 2 karton?", A = "₱60", B = "₱90", C = "₱45", D = "₱75", CorrectAnswer = "A" },
                new Question { QuestionText = "Sa outreach, bumili sila ng 12 hygiene kits ₱350 bawat isa. Magkano ang ginastos lahat?", A = "₱4,200", B = "₱3,500", C = "₱4,700", D = "₱5,000", CorrectAnswer = "A" },
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
                FormTitle = "Real Life Application (Easy)",
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
                FormTitle = "Real Life Application (Easy)",
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
