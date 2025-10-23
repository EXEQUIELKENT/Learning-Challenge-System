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
                new Question { QuestionText = "May ₱1,200 ang pamilya para sa isang barangay fiesta. Gumastos sila ₱450 para sa pagkain, ₱320 para sa dekorasyon, at ₱150 para sa upa ng sound system. Magkano ang natira?", A = "₱280", B = "₱300", C = "₱250", D = "₱260", CorrectAnswer = "A" },
                new Question { QuestionText = "Bumili si Liza ng 3 kilong bigas sa halagang ₱48/kilo at 2 tray ng itlog sa ₱180/tray. Kung nagbayad siya ng ₱400, magkano ang sukli?", A = "₱16", B = "₱4", C = "₱24", D = "₱10", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang pamasahe ng jeepney ay ₱9.00. Kung gumastos ang estudyante ng ₱90 sa isang buwan, ilang biyahe ang nagawa niya?", A = "9", B = "10", C = "8", D = "11", CorrectAnswer = "B" },
                new Question { QuestionText = "May ₱5,000 ang PTA. Nagbayad sila ng ₱1,250 para sa cleaning supplies at ₱1,450 para sa prizes. Magkano ang natira?", A = "₱2,300", B = "₱2,000", C = "₱2,300", D = "₱2,500", CorrectAnswer = "B" },
                new Question { QuestionText = "Si Marco ay nag-ipon ng ₱6,000 sa loob ng 12 buwan. Kung pare‑pareho ang naiipon niya bawat buwan, magkano ang naiipon niya kada buwan?", A = "₱600", B = "₱500", C = "₱550", D = "₱650", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Sa palabas ng barangay, bumili sila ng 15 tickets sa halagang ₱45 bawat isa. Kung may diskwentong 10% sa kabuuan, magkano ang kabuuang babayaran?", A = "₱607.5", B = "₱607", C = "₱608", D = "₱675", CorrectAnswer = "A" },
                new Question { QuestionText = "May pondo na ₱8,000 para sa outreach. Ginamit ang 1/4 para sa pagkain at 3/8 para sa hygiene kits. Magkano ang natira?", A = "₱2,500", B = "₱3,000", C = "₱2,000", D = "₱2,250", CorrectAnswer = "C" },
                new Question { QuestionText = "Si Ana bumili ng 4 pandesal sa ₱6 bawat isa at isang juice ₱18. Kung nagbayad siya ng ₱50, magkano ang sukli?", A = "₱2", B = "₱8", C = "₱10", D = "₱6", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang tindahan ay nag‑offer ng buy 2 get 1 free para sa canned goods na ₱45 bawat piraso. Kung bumili ka ng 3 piraso, magkano ang babayaran?", A = "₱90", B = "₱135", C = "₱45", D = "₱120", CorrectAnswer = "B" },
                new Question { QuestionText = "May perang ₱2,400 ang klase para sa field trip. Kung 20 estudyante at pantay ang ambag, magkano dapat ang kontribusyon kada isa?", A = "₱120", B = "₱140", C = "₱100", D = "₱150", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Nagbenta si Aling Nena ng 30 puto sa ₱15 bawat isa. Kung 20% ang gastos sa materyales, magkano ang netong kita niya?", A = "₱360", B = "₱360", C = "₱360", D = "₱360", CorrectAnswer = "A" },
                new Question { QuestionText = "May ₱12,000 ang munisipyo para sa sports equipment. Napagdesisyunan na ilaan ang 1/3 para sa basketball at 1/4 ng natira para sa volleyball. Magkano ang inilaan para sa volleyball?", A = "₱2,000", B = "₱3,000", C = "₱2,750", D = "₱2,500", CorrectAnswer = "D" },
                new Question { QuestionText = "Si Ben ay nagbayad ng ₱1,250 para sa 5 buwan na tuition. Magkano ang bayad kada buwan?", A = "₱250", B = "₱200", C = "₱300", D = "₱275", CorrectAnswer = "A" },
                new Question { QuestionText = "May ₱3,600 para sa stickers. Kailangan nila ng 12 packs ₱275 kada pack. Kulang o sobra at magkano?", A = "₱700 kulang", B = "₱700 sobra", C = "₱300 kulang", D = "₱300 sobra", CorrectAnswer = "A" },
                new Question { QuestionText = "Nagbenta ang palengke ng 20 sako ng palay sa ₱1,850 bawat sako. Kung nagastos ₱6,000 sa pagdala, magkano ang kabuuang kinita bago gastos?", A = "₱37,000", B = "₱36,000", C = "₱38,000", D = "₱35,000", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Si Maya ay nag-ipon ng ₱2,400 sa 6 buwan. Kung ginamit niya ang 1/3 para sa school project, magkano ang natira?", A = "₱1,600", B = "₱1,800", C = "₱1,400", D = "₱1,200", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang isang sari‑sari store ay may paninda ₱4,500. Nagkaroon ng 10% tubo. Ilan ang tubo?", A = "₱450", B = "₱400", C = "₱500", D = "₱350", CorrectAnswer = "A" },
                new Question { QuestionText = "Si Jun bumili ng 8 mangga sa halagang 3 mangga = ₱100. Kung bibili siya ng 8, magkano ang babayaran?", A = "₱266.67", B = "₱267", C = "₱300", D = "₱250", CorrectAnswer = "A" },
                new Question { QuestionText = "May ₱7,200 pondo ang club. Naglaan sila ng 25% para sa venue at ₱1,200 para sa pagkain. Magkano ang natira?", A = "₱3,600", B = "₱3,000", C = "₱3,600", D = "₱4,200", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang presyo ng isda ay ₱180/kg. Bumili si Aling Rosa ng 3 kg at gulay ₱95. Kung may ₱700, magkano ang natira?", A = "₱145", B = "₱225", C = "₱170", D = "₱95", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Si Carlo ay nagbayad ng ₱500 para sa 5 lata ng niyog ₱85 bawat isa. Kung may discount na ₱25 sa kabuuan, magkano ang karaniwang kabayaran pagkatapos ng diskwento?", A = "₱400", B = "₱400", C = "₱400", D = "₱400", CorrectAnswer = "A" },
                new Question { QuestionText = "May ₱10,000 pondo. Gumastos ng ₱2,500 sa materyales, ₱3,000 sa printing at ₱1,200 sa dekorasyon. Magkano ang natira?", A = "₱3,300", B = "₱3,200", C = "₱2,300", D = "₱3,800", CorrectAnswer = "A" },
                new Question { QuestionText = "Nag‑organisa ang klase ng fundraiser at kumita ng ₱12,000. Kung ibabahagi nila ang 20% para sa charity at hahatiin ang natira sa 10 volunteers, magkano ang matatanggap ng bawat volunteer?", A = "₱960", B = "₱1,000", C = "₱920", D = "₱880", CorrectAnswer = "A" },
                new Question { QuestionText = "Si Liza kumita ng ₱1,500 sa isang linggo. Kung ilalaan niya ang 30% para sa ipon at 20% para sa gastos, magkano ang naiipon at magkano para sa gastos?", A = "₱450 at ₱300", B = "₱500 at ₱250", C = "₱450 at ₱350", D = "₱400 at ₱300", CorrectAnswer = "A" },
                new Question { QuestionText = "May ₱2,500 donasyon at hinati sa 25 pamilya. Magkano ang natanggap ng bawat pamilya?", A = "₱100", B = "₱125", C = "₱150", D = "₱75", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Kailangan ng klase ng ₱2,700 para sa van at ₱1,200 para sa pagkain. Kung 15 estudyante ang magbabayad pantay, magkano bawat isa?", A = "₱260", B = "₱300", C = "₱280", D = "₱240", CorrectAnswer = "A" },
                new Question { QuestionText = "Si Pedro bumili ng 3 notebooks ₱45 bawat isa at lapis set ₱60. Nagbayad siya ng ₱200. Magkano ang sukli?", A = "₱5", B = "₱10", C = "₱15", D = "₱20", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang presyo ng mangga ay bumaba mula ₱80 sa ₱64. Ilang porsyento ang pagbaba?", A = "20%", B = "15%", C = "25%", D = "10%", CorrectAnswer = "A" },
                new Question { QuestionText = "May pondong ₱8,000 para sa environmental cleanup. Ginamit ang 1/4 sa pagkain at 3/8 sa kagamitan. Magkano ang natitira?", A = "₱3,000", B = "₱3,500", C = "₱3,250", D = "₱4,000", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Si Tita Mayang bumili ng 10 kg bigas ₱48/kg at mga ulam ₱1,250. Kung naglaan siya ng ₱2,000, magkano ang kakulangan o sobra?", A = "₱570 sobra", B = "₱570 kulang", C = "₱150 sobra", D = "₱150 kulang", CorrectAnswer = "B" },
                new Question { QuestionText = "May ₱3,600 pondo. Kailangan ng 12 packs ₱275 bawat pack. Kulang o sobra at magkano?", A = "₱700 kulang", B = "₱700 sobra", C = "₱300 kulang", D = "₱300 sobra", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang palengke ay nagbenta ng 20 sako ng palay ₱1,850 bawat sako. Kita bago gastos kung ibenta lahat?", A = "₱37,000", B = "₱36,000", C = "₱38,000", D = "₱35,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Si Maya nag-ipon ₱2,400 sa 6 buwan. Kung ginamit ang 1/3 sa proyekto, magkano ang natira?", A = "₱1,600", B = "₱1,800", C = "₱1,400", D = "₱1,200", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang hostel fee ay ₱2,400 para sa 3 buwan. Kung babayaran ito sa 3 installments, magkano kada installment?", A = "₱800", B = "₱700", C = "₱900", D = "₱750", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang isang tindahan ay nagtaas ng 5% sa presyo ng items. Kung ang dating presyo ay ₱200, magkano na ngayon?", A = "₱210", B = "₱200", C = "₱205", D = "₱215", CorrectAnswer = "A" }
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

