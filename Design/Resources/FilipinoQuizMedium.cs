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
    public partial class FilipinoQuizMedium : Form
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
        public FilipinoQuizMedium()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "Ano ang pambansang hayop ng Pilipinas?", A = "Tarsier", B = "Carabao", C = "Agila", D = "Pagong", CorrectAnswer = "C" },
                new Question { QuestionText = "Sa anong lungsod matatagpuan ang Banaue Rice Terraces?", A = "Baguio", B = "Cebu", C = "Davao", D = "Ifugao", CorrectAnswer = "D" },
                new Question { QuestionText = "Anong sikat na festival ang ipinagdiriwang sa Cebu tuwing Enero?", A = "Panagbenga", B = "Sinulog", C = "Ati-Atihan", D = "Kadayawan", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang tawag sa tradisyunal na Filipino na kasuotan ng mga lalaki?", A = "Terno", B = "Barong Tagalog", C = "Baro’t Saya", D = "Habi", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pambansang pagkain ng Pilipinas?", A = "Adobo", B = "Sinigang", C = "Lechon", D = "Pancit", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Ano ang pangalan ng pambansang bayani ng Pilipinas?", A = "Andres Bonifacio", B = "Emilio Aguinaldo", C = "José Rizal", D = "Antonio Luna", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang ibig sabihin ng 'Bayanihan' sa kultura ng Pilipino?", A = "Pagkakaisa at pagtutulungan", B = "Pagtanggap sa kultura", C = "Pagbibigay ng ayuda", D = "Pagdiriwang ng kasaysayan", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang tawag sa tradisyunal na sayaw na ginagamitan ng mga kawayan?", A = "Tinikling", B = "Singkil", C = "Cariñosa", D = "Binasuan", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang tawag sa sikat na karne na niluluto gamit ang suka at toyo?", A = "Adobo", B = "Sinigang", C = "Kare-Kare", D = "Bicol Express", CorrectAnswer = "A" },
                new Question { QuestionText = "Anong pangkat ng mga tao ang kilala sa kanilang matibay na tradisyon ng panggugubat at pagtatanggol?", A = "Tagalog", B = "Igorot", C = "Visayan", D = "Moro", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Ano ang tinaguriang 'Perlas ng Silanganan'?", A = "Thailand", B = "Pilipinas", C = "Indonesia", D = "Malaysia", CorrectAnswer = "B" },
                new Question { QuestionText = "Sino ang nagdisenyo ng watawat ng Pilipinas?", A = "Jose Rizal", B = "Emilio Aguinaldo", C = "Andres Bonifacio", D = "Marcelo H. del Pilar", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pangunahing kayamanan ng Pilipinas noong panahon ng mga Espanyol?", A = "Ginto", B = "Perlas", C = "Spices", D = "Kahoy", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang ibig sabihin ng kulay asul sa watawat ng Pilipinas?", A = "Katapangan", B = "Kapayapaan", C = "Dugo ng mga bayani", D = "Kalayaan", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pangalan ng barko ni Ferdinand Magellan?", A = "Santa Maria", B = "Victoria", C = "Trinidad", D = "Santiago", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Ano ang pangalan ng pamahalaan na itinatag ni Emilio Aguinaldo?", A = "Katipunan", B = "Republika ng Malolos", C = "Haring Bayan", D = "Hukbalahap", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang tawag sa sinaunang sistema ng pagsulat ng mga Pilipino?", A = "Sanskrit", B = "Baybayin", C = "Latin", D = "Alibata", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang tawag sa sinaunang batas ng mga Pilipino na nakasulat sa mga balumbon ng dahon?", A = "Code of Hammurabi", B = "Kodigo ni Kalantiaw", C = "Kartilya ng Katipunan", D = "Saligang Batas ng Biak-na-Bato", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pinakamahabang ilog sa Pilipinas?", A = "Cagayan River", B = "Pasig River", C = "Agusan River", D = "Pampanga River", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang opisyal na wikang ginamit sa Saligang Batas ng 1899?", A = "Tagalog", B = "Ingles", C = "Espanyol", D = "Hiligaynon", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Ano ang sikat na epikong Tagalog na may pangunahing tauhang si Lam-ang?", A = "Hinilawod", B = "Biag ni Lam-ang", C = "Ibalon", D = "Hudhud", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang tawag sa kasunduang nagwakas sa Digmaang Espanyol-Amerikano na nagbigay ng Pilipinas sa Estados Unidos?", A = "Kasunduan sa Paris", B = "Kasunduan sa Biak-na-Bato", C = "Kasunduan sa Tordesillas", D = "Kasunduan sa Washington", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang pangunahing relihiyon ng mga sinaunang Pilipino bago dumating ang mga Espanyol?", A = "Kristiyanismo", B = "Islam", C = "Animismo", D = "Buddhism", CorrectAnswer = "C" },
                new Question { QuestionText = "Saan ipinadala si Jose Rizal matapos ipatapon ng mga Espanyol?", A = "Dapitan", B = "Cebu", C = "Palawan", D = "Bohol", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang pangalan ng unang pamahalaang itinatag ng mga Amerikano sa Pilipinas?", A = "Komisyong Taft", B = "Republika ng Pilipinas", C = "Hukbalahap", D = "Commonwealth Government", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Sino ang huling gobernador-heneral ng Espanya sa Pilipinas?", A = "Diego Silang", B = "Basilio Agustin", C = "Miguel López de Legazpi", D = "Jaime de Veyra", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang kahulugan ng 'Maharlika' sa sinaunang lipunang Pilipino?", A = "Alipin", B = "Karaniwang tao", C = "Manggagawa", D = "Noble o mataas na antas", CorrectAnswer = "D" },
                new Question { QuestionText = "Ano ang tawag sa pagsakop ng Espanya sa Pilipinas sa loob ng mahigit 300 taon?", A = "Kolonyalismo", B = "Imperyalismo", C = "Feudalismo", D = "Sosyalismo", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang sagisag ng araw na may walong sinag sa watawat ng Pilipinas?", A = "Walong unang lalawigang naghimagsik", B = "Walong bayaning Pilipino", C = "Walong relihiyon sa bansa", D = "Walong malalaking isla", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang pangalan ng grupo ng kababaihang naghimagsik laban sa Espanya?", A = "Gabriela Silang", B = "Katipuneras", C = "Red Cross Women", D = "Pambansang Hukbong Bayan", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Ano ang pangunahing layunin ng Kilusang Propaganda?", A = "Ganap na kalayaan", B = "Rebolusyon", C = "Pagbabago sa pamamalakad ng Espanya sa Pilipinas", D = "Pag-aalis ng relihiyon sa bansa", CorrectAnswer = "C" },
                new Question { QuestionText = "Sino ang kauna-unahang presidente ng Pilipinas?", A = "Jose Rizal", B = "Andres Bonifacio", C = "Emilio Aguinaldo", D = "Manuel Quezon", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang pangalan ng unang babaeng presidente ng Pilipinas?", A = "Cory Aquino", B = "Gloria Macapagal Arroyo", C = "Imelda Marcos", D = "Leni Robredo", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang batas na nagtatag ng Komisyong Pangwikang Filipino?", A = "Batas Komonwelt Blg. 184", B = "Batas Republika Blg. 7104", C = "Saligang Batas ng 1987", D = "Batas Blg. 1425", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang tawag sa pinakamalawak na kapatagan sa Pilipinas?", A = "Cagayan Valley", B = "Central Luzon Plain", C = "Cotabato Basin", D = "Agusan Marsh", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Ano ang pangalan ng pinakahuling lalawigan na itinayo sa Pilipinas?", A = "Davao Occidental", B = "Dinagat Islands", C = "Zamboanga Sibugay", D = "Sarangani", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang batas na nagdedeklara ng Wikang Pambansa?", A = "Batas Komonwelt Blg. 184", B = "Batas Republika Blg. 7104", C = "Batas Blg. 1425", D = "Batas Komonwelt Blg. 570", CorrectAnswer = "D" }


                                // Add more questions here...
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
                FormTitle = "Filipino Quiz (Medium)",
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
                FormTitle = "Filipino Quiz (Medium)",
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
