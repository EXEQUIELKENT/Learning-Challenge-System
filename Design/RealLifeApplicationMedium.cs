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
    public partial class RealLifeApplicationMedium : Form
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
        public RealLifeApplicationMedium()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "Kung ang isang tindahan ay nagbebenta ng mangga sa halagang ₱25 bawat piraso at saging sa ₱10 bawat isa. Kung bumili ka ng 3 mangga at 4 na saging, magkano ang babayaran?", A = "₱115", B = "₱95", C = "₱105", D = "₱125", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang kotse ay bumiyahe ng 240 kilometro sa loob ng 3 oras. Ano ang karaniwang bilis (km/h)?", A = "70 km/h", B = "80 km/h", C = "60 km/h", D = "90 km/h", CorrectAnswer = "B" },
                new Question { QuestionText = "Ang resipi ay nangangailangan ng 3/4 tasa ng asukal. Kung gagawin mo ang kalahati ng recipe, gaano karaming asukal ang kailangan?", A = "1/4 tasa", B = "1/2 tasa", C = "3/8 tasa", D = "1/3 tasa", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang manggagawa ay kumikita ng ₱150 kada oras. Kung nagtrabaho siya ng 8 oras sa isang araw, magkano ang kanyang kita sa araw na iyon?", A = "₱1,200", B = "₱1,000", C = "₱1,050", D = "₱1,400", CorrectAnswer = "A" },
                new Question { QuestionText = "May 600 mag-aaral sa isang paaralan. Kung 40% ay babae, ilan ang bilang ng mga babae?", A = "240", B = "200", C = "220", D = "250", CorrectAnswer = "A" },

                new Question { QuestionText = "Bumili ng motorsiklo sa ₱125,000 at bumaba ang halaga nito ng 10% pagkatapos ng isang taon. Magkano na ang halaga nito pagkatapos ng isang taon?", A = "₱112,500", B = "₱115,000", C = "₱110,000", D = "₱100,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang recipe ay nangangailangan ng 2 1/2 tasa ng harina. Kung mayroon ka lamang 1/2 tasa, ilang beses mo itong kailangang punuin?", A = "4", B = "5", C = "6", D = "7", CorrectAnswer = "B" },
                new Question { QuestionText = "Gagawa ang karpintero ng pader na 3 metro ang taas at 5 metro ang haba. Ano ang area ng pader (square meters)?", A = "15 m²", B = "8 m²", C = "10 m²", D = "20 m²", CorrectAnswer = "A" },
                new Question { QuestionText = "Kung naka-sale ang dyaket na may 30% diskwento at ang orihinal na presyo ay ₱3,000, magkano ang babayaran?", A = "₱2,100", B = "₱2,300", C = "₱2,000", D = "₱2,500", CorrectAnswer = "A" },
                new Question { QuestionText = "Mayroon kang ₱3,000 at nais bumili ng sapatos na ₱750 bawat isa. Ilan ang iyong mabibili?", A = "3", B = "4", C = "5", D = "2", CorrectAnswer = "A" },

                new Question { QuestionText = "Ang tren ay naglakbay ng 240 km sa loob ng 4 na oras. Ano ang average speed nito?", A = "50 km/h", B = "55 km/h", C = "60 km/h", D = "65 km/h", CorrectAnswer = "C" },
                new Question { QuestionText = "Nagkakahalaga ang laptop ng ₱60,000. Kung may 15% diskwento, magkano ang bagong presyo?", A = "₱51,000", B = "₱52,000", C = "₱50,000", D = "₱48,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang water tank ay may kapasidad na 500 litro at kasalukuyang 40% puno. Ilang litro ang laman?", A = "150 L", B = "200 L", C = "250 L", D = "300 L", CorrectAnswer = "C" },
                new Question { QuestionText = "May 15 baka ang magsasaka at bawat isa ay nagpapalabas ng 8 litro ng gatas bawat araw. Gaano karaming gatas ang makukuha sa isang linggo?", A = "560 L", B = "720 L", C = "840 L", D = "900 L", CorrectAnswer = "A" },
                new Question { QuestionText = "Nagjojogging ang isang tao sa bilis na 6 km/h. Gaano kalayo siya makakalakad sa loob ng 90 minuto?", A = "7 km", B = "8 km", C = "9 km", D = "10 km", CorrectAnswer = "C" },

                new Question { QuestionText = "Bumili ang paaralan ng 12 whiteboard sa ₱2,700 bawat isa. Magkano ang kabuuang gastos?", A = "₱32,400", B = "₱30,000", C = "₱29,000", D = "₱33,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Nagbebenta ng bigas ang tindahan: 5 kg kada sako sa ₱900. Magkano ang babayaran kung bibili ng 3 sako?", A = "₱2,700", B = "₱2,500", C = "₱2,600", D = "₱2,800", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang smartphone ay ₱40,000. Kung may 12% buwis, magkano ang total na babayaran?", A = "₱44,800", B = "₱45,000", C = "₱44,000", D = "₱46,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang airfare ay ₱18,000. Kung may 20% discount ang airline, magkano ang bagong presyo?", A = "₱14,400", B = "₱15,000", C = "₱14,000", D = "₱13,500", CorrectAnswer = "A" },
                new Question { QuestionText = "Nagsimula ang palabas sa TV ng 7:45 PM at tumagal ng 1 oras 35 minuto. Anong oras ito nagtapos?", A = "9:10 PM", B = "9:15 PM", C = "9:20 PM", D = "9:30 PM", CorrectAnswer = "C" },

                new Question { QuestionText = "Kung kumokonsumo ang kotse ng 7 litro kada 100 km, gaano karaming gasolina ang kailangan para sa 350 km na byahe?", A = "24.5 L", B = "21 L", C = "26 L", D = "28 L", CorrectAnswer = "A" },
                new Question { QuestionText = "Ang baterya ng cellphone ay bumababa ng 4% kada oras. Kung nagsimula sa 90%, ilang porsyento ang natira pagkatapos ng 5 oras?", A = "70%", B = "72%", C = "75%", D = "80%", CorrectAnswer = "B" },
                new Question { QuestionText = "May 1,200 empleyado ang kumpanya. Kung 25% ang nagtatrabaho mula bahay, ilan ang work-from-home?", A = "300", B = "275", C = "350", D = "325", CorrectAnswer = "A" },
                new Question { QuestionText = "Nag-jogging ang isang tao ng 2 km araw-araw sa loob ng 2 linggo. Gaano kalayo ang natakbo niya sa kabuuan?", A = "24 km", B = "26 km", C = "28 km", D = "30 km", CorrectAnswer = "C" },
                new Question { QuestionText = "Gumagawa ang panaderya ng 300 cupcakes sa loob ng 5 oras. Ilang cupcakes ang nagagawa kada oras (average)?", A = "50", B = "55", C = "60", D = "65", CorrectAnswer = "A" },

                new Question { QuestionText = "May promo na buy 2 get 1 free. Kung ₱900 ang presyo ng 3 items (₱300 bawat isa), magkano ang babayaran ng customer?", A = "₱600", B = "₱700", C = "₱800", D = "₱900", CorrectAnswer = "A" },
                new Question { QuestionText = "Nagbibigay ang hotel ng ₱4,800 kada gabi. Kung magtatagal ka ng 5 gabi, magkano ang kabuuang bayad?", A = "₱24,000", B = "₱22,500", C = "₱20,000", D = "₱25,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Gastos sa grocery ng pamilya ay ₱12,500 kada linggo. Magkano ang ginagastos nila sa loob ng 4 na linggo?", A = "₱45,000", B = "₱50,000", C = "₱40,000", D = "₱48,000", CorrectAnswer = "D" },
                new Question { QuestionText = "Ang pamasahe ng taxi ay ₱75 kada kilometro. Kung ang byahe ay 12 km, magkano ang pamasahe?", A = "₱900", B = "₱850", C = "₱750", D = "₱800", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang hardin ay may area na 40 m² at parisukat ang hugis. Ano ang perimeter nito?", A = "20 m", B = "25 m", C = "40 m", D = "80 m", CorrectAnswer = "C" },

                new Question { QuestionText = "Ang tiket sa tren ay ₱1,000. Kung may 15% discount ang pasahero, magkano ang bagong presyo?", A = "₱850", B = "₱875", C = "₱900", D = "₱925", CorrectAnswer = "B" },
                new Question { QuestionText = "Isang kahon ng cereal ay may 750 g. Kung kumakain ka ng 50 g bawat araw, ilang araw tatagal ang kahon?", A = "12 days", B = "14 days", C = "15 days", D = "18 days", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang konstruksyon na manggagawa ay kumikita ng ₱2,500 kada araw. Kung nagtrabaho siya ng 5 araw, magkano ang kinita niya?", A = "₱12,000", B = "₱12,500", C = "₱13,000", D = "₱11,500", CorrectAnswer = "B" },
                new Question { QuestionText = "May 35 estudyante sa klase at 60% ay lalaki. Ilan ang mga lalaki?", A = "18", B = "20", C = "21", D = "22", CorrectAnswer = "C" },
                new Question { QuestionText = "Ang juice carton ay 1.5 litro. Ilang milliliters ito?", A = "1,500 mL", B = "1,550 mL", C = "1,600 mL", D = "1,700 mL", CorrectAnswer = "A" },

                new Question { QuestionText = "Ang siklista ay nagbiyahe ng 240 km sa loob ng 6 na oras. Ano ang average speed (km/h)?", A = "35 km/h", B = "40 km/h", C = "45 km/h", D = "50 km/h", CorrectAnswer = "B" },
                new Question { QuestionText = "Nagbebenta ang shop ng T-shirt sa ₱900 bawat isa. Magkano ang 4 T-shirts?", A = "₱3,600", B = "₱3,200", C = "₱3,800", D = "₱4,000", CorrectAnswer = "A" },
                new Question { QuestionText = "Nag-iipon ang isang tao ng ₱5,000 kada buwan. Magkano ang mayroon siya sa loob ng isang taon?", A = "₱50,000", B = "₱55,000", C = "₱60,000", D = "₱65,000", CorrectAnswer = "C" },
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
                FormTitle = "Real Life Application (Medium)",
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
                FormTitle = "Real Life Application (Medium)",
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
