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
                new Question { QuestionText = "Magkano ang perimeter ng parihabang bakuran na may haba na 20 m at lapad na 12 m?", A = "64 m", B = "44 m", C = "40 m", D = "32 m", CorrectAnswer = "A" },
                new Question { QuestionText = "Mayroong 24 litro ng tubig. Hatiin ito sa 6 bote nang pantay‑pantay. Ilang litro ang laman ng bawat bote?", A = "3 L", B = "4 L", C = "6 L", D = "2 L", CorrectAnswer = "B" },
                new Question { QuestionText = "Kung ang isang jeepney fare ay ₱12 at sumakay ka nang 7 beses, magkano ang kabuuan?", A = "₱84", B = "₱72", C = "₱90", D = "₱96", CorrectAnswer = "A" },
                new Question { QuestionText = "Bumili ka ng 3 kg ng bigas ₱55/kg at 2 kg gulay ₱40/kg. Magkano lahat?", A = "₱265", B = "₱255", C = "₱275", D = "₱245", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang libro ay ₱420. Kung may 15% diskwento, magkano ang iyong babayaran?", A = "₱357", B = "₱360", C = "₱370", D = "₱350", CorrectAnswer = "A" },

                new Question { QuestionText = "Si Ella ay nag‑ipon ng ₱9,000 at nais makapagbayad ng ₱2,500 kada buwan. Ilang buwan bago maubos?", A = "3 buwan", B = "4 buwan", C = "2 buwan", D = "5 buwan", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang isang tricycle ay naglalakbay ng 18 km sa 30 minuto. Ano ang average speed sa km/h?", A = "36 km/h", B = "32 km/h", C = "24 km/h", D = "30 km/h", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung ang isang cake recipe ay para sa 12 tao at nais mo para sa 20 tao, ano ang factor ng pagtaas ng sangkap?", A = "1.5", B = "1.66", C = "1.25", D = "2.0", CorrectAnswer = "B" },
                new Question { QuestionText = "May ₱15,000 pondo. Gumasta ng ₱4,200 sa kagamitan at ₱3,350 sa pagkain. Magkano natira?", A = "₱7,450", B = "₱7,600", C = "₱7,150", D = "₱8,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Isang school bus may capacity na 50 estudyante. Kung 38 ang sumakay, ilang upuan ang bakante?", A = "12", B = "10", C = "8", D = "14", CorrectAnswer = "A" },

                new Question { QuestionText = "Kung ₱1,200 ang kabuuang halaga at hatiin sa 15 mag‑aambag, magkano bawat isa (rounded to nearest peso)?", A = "₱80", B = "₱79", C = "₱81", D = "₱75", CorrectAnswer = "A" },
                new Question { QuestionText = "Si Marco bumili ng school supplies ₱345 at nagbigay ng ₱500. Magkano ang sukli?", A = "₱155", B = "₱145", C = "₱135", D = "₱160", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang presyo ng itlog ay ₱6 bawat piraso. Magkano ang 1 dosenang itlog?", A = "₱60", B = "₱72", C = "₱50", D = "₱66", CorrectAnswer = "A" },
                new Question { QuestionText = "May 120 pirasong stickers. Hatiin sa 8 grupo nang pantay‑pantay. Ilan bawat grupo?", A = "15", B = "20", C = "10", D = "12", CorrectAnswer = "B" },
                new Question { QuestionText = "Kung 25% ng kita ay inilaan sa ipon at kumita ka ng ₱8,000, magkano ang ipon?", A = "₱2,000", B = "₱1,800", C = "₱2,200", D = "₱2,500", CorrectAnswer = "A" },

                new Question { QuestionText = "Isang van nagbiyahe ng 360 km gamit ang 30 L ng gasolina. Ano ang fuel efficiency (km/L)?", A = "12 km/L", B = "10 km/L", C = "9 km/L", D = "15 km/L", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung ang presyo ng manok ay ₱150/kg at bumili ka ng 2.5 kg, magkano babayaran?", A = "₱375", B = "₱350", C = "₱325", D = "₱380", CorrectAnswer = "A" },
                new Question { QuestionText = "Bumili ng 3 shirts ₱420 bawat isa, may buy 2 get 1 half price. Magkano kabuuan?", A = "₱1,050", B = "₱1,260", C = "₱1,190", D = "₱1,240", CorrectAnswer = "C" },
                new Question { QuestionText = "Isang bahay may monthly electric bill ₱3,200. Kung tataas ng 8%, magkano ang bagong bill?", A = "₱3,456", B = "₱3,500", C = "₱3,420", D = "₱3,600", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung ang isang klase ay may 28 estudyante at 3/7 ay babae, ilan ang babae?", A = "12", B = "10", C = "14", D = "16", CorrectAnswer = "A" },

                new Question { QuestionText = "May 500 mL juice. Ibuhos sa 200 mL na baso. Ilang baso ang mapupuno?", A = "2 baso", B = "2.5 baso", C = "3 baso", D = "2 baso (may leftovers)", CorrectAnswer = "D" },
                new Question { QuestionText = "Isang manggagawa kumikita ng ₱420 kada araw. Kung nagtrabaho siya ng 22 araw, magkano kinita niya?", A = "₱9,240", B = "₱8,800", C = "₱9,000", D = "₱10,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung ang bus trip ay 180 km at bumaba ang average speed mula 60 km/h sa 45 km/h dahil sa traffic, gaano karaming oras ang nadagdag?", A = "1 hour", B = "0.5 hour", C = "2 hours", D = "1.5 hours", CorrectAnswer = "A" },
                new Question { QuestionText = "May pondo ₱25,000. Ilalaan 40% sa proyekto A at natitira ilalaan sa proyekto B. Magkano ang napunta sa proyekto A?", A = "₱10,000", B = "₱9,000", C = "₱12,000", D = "₱11,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Isang tindahan nagbebenta ₱3,600 ng items at may 12% profit margin. Magkano ang tubo?", A = "₱432", B = "₱360", C = "₱420", D = "₱480", CorrectAnswer = "A" },

                new Question { QuestionText = "Kung ang average ng 5 numero ay 72, ano ang kabuuan ng mga numero?", A = "₂₈₀", B = "₃₆₀", C = "₃₆₀", D = "₃₆₀", CorrectAnswer = "A" },
                new Question { QuestionText = "Si Ana nagbayad ₱2,750 para sa 5 buwan ng tutorial. Magkano kada buwan?", A = "₱550", B = "₱500", C = "₱600", D = "₱450", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung ang one‑way fare ng ferry ay ₱125 at gusto mong pumunta at bumalik ng 4 na beses (round trips), magkano lahat?", A = "₱1,000", B = "₱1,000", C = "₱2,000", D = "₱1,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Isang tindahan nagbigay ng 20% diskwento sa ₱2,500 na item at may 12% VAT pagkatapos. Magkano ang final price? (compute diskwento, saka VAT)", A = "₱2,240", B = "₱2,200", C = "₱2,240", D = "₱2,250", CorrectAnswer = "A" },
                new Question { QuestionText = "May 3 itlog na pino‑package per tray; bumili ka ng 14 trays. Ilang itlog mayroon ka?", A = "42", B = "40", C = "36", D = "45", CorrectAnswer = "A" },

                new Question { QuestionText = "Kung isang barangay naglaan ₱18,000 at ginamit 2/9 para sa kainan, magkano nagastos para kainan?", A = "₱4,000", B = "₱4,500", C = "₱4,200", D = "₱5,000", CorrectAnswer = "C" },
                new Question { QuestionText = "Isang motor na may speed 72 km/h, ilang minuto para makarating sa 36 km?", A = "30 minutes", B = "20 minutes", C = "40 minutes", D = "50 minutes", CorrectAnswer = "A" },
                new Question { QuestionText = "Bumili ng 8 liters ng gatas sa ₱75 bawat litro. Kung nag‑promo na buy 2 get 10% off sa bawat dalawang litro, magkano matatapos? (approx)", A = "₱540", B = "₱550", C = "₱560", D = "₱600", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung isang klase may average height na 140 cm at may 30 estudyante, ano ang kabuuang height (sum)?", A = "₄,２００ cm", B = "₄,０００ cm", C = "₄,３００ cm", D = "₄,１００ cm", CorrectAnswer = "A" },
                new Question { QuestionText = "Isang speedboat naglalayag 48 km sa 1.5 oras. Ano ang speed sa km/h?", A = "32 km/h", B = "30 km/h", C = "28 km/h", D = "36 km/h", CorrectAnswer = "A" },

                new Question { QuestionText = "May 1,500 students; 60% ang pumasa sa exam. Ilan ang pumasa?", A = "₉００", B = "₈５０", C = "₉５０", D = "₁,０００", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung magrenta ka ng function hall ₱6,000 at mag‑cater ₱220 bawat tao para sa 50 tao, magkano lahat?", A = "₱17,000", B = "₱16,000", C = "₱17,500", D = "₱15,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Isang mangunguma may 12 sako ng palay; bawat sako 50 kg. Kung bawat kilo ₱18, magkano ang kabuuang halaga?", A = "₱10,800", B = "₱11,000", C = "₱12,000", D = "₱9,600", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung buwanang gastusin ₱18,500 at 30% nito ay para sa renta, magkano ang renta?", A = "₱5,550", B = "₱6,000", C = "₱5,000", D = "₱5,650", CorrectAnswer = "A" },
                new Question { QuestionText = "May project budget ₱20,000. Gumastos ₱7,250. Ano ang porsyento ng nagamit (rounded to 1 decimal)?", A = "36.3%", B = "35.0%", C = "36.2%", D = "37.5%", CorrectAnswer = "A" },

                new Question { QuestionText = "Kung isang motorcycle nagkakahalaga ng ₱85,000 at may 5% downpayment, magkano ang downpayment?", A = "₱4,250", B = "₱4,500", C = "₱5,000", D = "₱4,750", CorrectAnswer = "A" },
                new Question { QuestionText = "Isang jeep nagdala ng 120 pasahero sa 3 trips. Kung pantay ang pasahero kada trip, ilan kada trip?", A = "40", B = "30", C = "45", D = "50", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung ang isang empleyado kumikita ng ₱12,000 at nakatanggap ng 8% bonus, magkano kabuuan kasama bonus?", A = "₱12,960", B = "₱13,000", C = "₱12,800", D = "₱13,200", CorrectAnswer = "A" },
                new Question { QuestionText = "May 2.5 kg ng bigas. Ilang gramo ito?", A = "2,500 g", B = "2,050 g", C = "250 g", D = "2,750 g", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang tasa ng pagbabago ng presyo mula ₱450 naging ₱495, ano ang porsyento ng pagtaas?", A = "10%", B = "9%", C = "11%", D = "12%", CorrectAnswer = "A" },
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
