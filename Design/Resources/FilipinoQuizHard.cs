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
    public partial class FilipinoQuizHard : Form
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
        public FilipinoQuizHard()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "Ano ang pinakamahabang tulay sa Pilipinas?", A = "San Juanico Bridge", B = "Mactan-Mandaue Bridge", C = "Taal Volcano Bridge", D = "Cagayan Valley Bridge", CorrectAnswer = "A" },
                new Question { QuestionText = "Saan matatagpuan ang Banaue Rice Terraces?", A = "Kalinga", B = "Ifugao", C = "Benguet", D = "Ilocos", CorrectAnswer = "B" },
                new Question { QuestionText = "Anong pangalan ng pambansang ibon ng Pilipinas?", A = "Eagle ng Mindanao", B = "Philippine Eagle", C = "Tarsier", D = "Balubad", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang tawag sa tradisyonal na kasuotan ng mga Pilipino?", A = "Barong Tagalog", B = "Kimono", C = "Sari", D = "Toga", CorrectAnswer = "A" },
                new Question { QuestionText = "Anong sikat na festival sa Cebu ang ginaganap tuwing Enero?", A = "Sinulog", B = "Ati-Atihan", C = "Kadayawan", D = "Panagbenga", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Saan matatagpuan ang Chocolate Hills?", A = "Bohol", B = "Batangas", C = "Cebu", D = "Palawan", CorrectAnswer = "A" },
                new Question { QuestionText = "Anong pagkain ang itinuturing na pambansang pagkain ng Pilipinas?", A = "Pancit", B = "Adobo", C = "Sinigang", D = "Lumpia", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pinagmulan ng kasaysayan ng Mayon Volcano?", A = "Dahil sa kasaysayan ng erupsyon", B = "Kwento ng magkasintahang darna", C = "Alamat ng magkasintahang sina Magayon at Pangaronan", D = "Sakit sa lupa", CorrectAnswer = "C" },
                new Question { QuestionText = "Anong uri ng sining ang 'Banga'?", A = "Pagtitinda", B = "Pagpinta", C = "Pag-ukit", D = "Pag-uukit ng banga", CorrectAnswer = "D" },
                new Question { QuestionText = "Ano ang tawag sa pamana ng mga katutubo sa Mindanao?", A = "Vinta", B = "Bangka", C = "Sampaguita", D = "Pangolin", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Sino ang pangunahing bayani na nagsimula ng rebolusyon laban sa mga Kastila?", A = "Andres Bonifacio", B = "Emilio Aguinaldo", C = "Jose Rizal", D = "Antonio Luna", CorrectAnswer = "A" },
                new Question { QuestionText = "Anong kilalang kaganapan ang ginanap sa Luneta?", A = "Pagpapakamatay ni Rizal", B = "Pagpapahayag ng Kalayaan", C = "Laban sa Kastila", D = "Pagka-pangulo ni Aguinaldo", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pangunahing wika sa Visayas?", A = "Ilokano", B = "Cebuano", C = "Bikol", D = "Tagalog", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang tawag sa paggamit ng mga tatak na Pilipino?", A = "Bayani", B = "Bilog", C = "Pilipinismo", D = "Tatak Pinoy", CorrectAnswer = "C" },
                new Question { QuestionText = "Saan matatagpuan ang Pagsanjan Falls?", A = "Pangasinan", B = "Laguna", C = "Zambales", D = "Quezon", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Ano ang tawag sa kasaysayan ng mga katutubong Pilipino bago dumating ang mga Kastila?", A = "Pananakop", B = "Sambayanan", C = "Pre-kolonyal na panahon", D = "Panahon ng Bago", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang pangalan ng pambansang puno ng Pilipinas?", A = "Mango", B = "Narra", C = "Acacia", D = "Mahogany", CorrectAnswer = "B" },
                new Question { QuestionText = "Anong pangalan ng pinakaunang unibersidad sa Pilipinas?", A = "University of the Philippines", B = "Ateneo de Manila", C = "San Beda University", D = "University of Santo Tomas", CorrectAnswer = "D" },
                new Question { QuestionText = "Sino ang unang Pilipinong nagwagi ng Nobel Prize?", A = "Jose Rizal", B = "Carlos P. Romulo", C = "Manuel L. Quezon", D = "Eugenio Lopez, Jr.", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang ibig sabihin ng 'Bayanihan'?", A = "Kultura ng pagtutulungan", B = "Pagtanggap ng pondo", C = "Pagkakaroon ng sariling negosyo", D = "Pag-aalaga ng kalikasan", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Anong festival ang ipinagdiriwang tuwing Pebrero sa Baguio?", A = "Pista ng Pagtangkilik", B = "Panagbenga", C = "Ati-Atihan", D = "Sinulog", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pangalan ng pinakaunang libro na nalimbag sa Pilipinas?", A = "Noli Me Tangere", B = "Doctrina Christiana", C = "El Filibusterismo", D = "Ang Barlaan at Josephat", CorrectAnswer = "B" },
                new Question { QuestionText = "Sino ang unang gobernador-heneral ng Pilipinas?", A = "Miguel López de Legazpi", B = "Ferdinand Magellan", C = "Emilio Aguinaldo", D = "Diego Silang", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang pinakaunang pangalan ng Pilipinas na ibinigay ni Magellan?", A = "Las Islas del Poniente", B = "Las Islas Filipinas", C = "Islas de San Lazaro", D = "Perlas ng Silangan", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang tawag sa lumang sistema ng pagsulat ng mga sinaunang Pilipino?", A = "Baybayin", B = "Balangay", C = "Alibata", D = "Kawi", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Ano ang pinakaunang pahayagan sa Pilipinas?", A = "La Solidaridad", B = "La Independencia", C = "Del Superior Gobierno", D = "Ang Kalayaan", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang dahilan ng pagkamatay ni Andres Bonifacio?", A = "Pinatay ng mga Kastila", B = "Namatay sa labanan", C = "Pinapatay ng kapwa rebolusyonaryo", D = "Namatay sa sakit", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang pangunahing layunin ng Pact of Biak-na-Bato?", A = "Ganap na kalayaan", B = "Pansamantalang tigil-putukan", C = "Pagtatapos ng digmaan", D = "Pagtanggal ng mga Espanyol", CorrectAnswer = "B" },
                new Question { QuestionText = "Sino ang unang Pilipino na naging santo?", A = "Lorenzo Ruiz", B = "San Pedro Calungsod", C = "Jose Rizal", D = "Jaime Cardinal Sin", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang kilalang tawag sa hukbong sandatahan ng rebolusyonaryong Pilipino noong panahon ng mga Amerikano?", A = "Hukbalahap", B = "Katipunan", C = "Sandatahan ng Bayan", D = "Gerilya", CorrectAnswer = "A" },
               
                new Question { QuestionText = "Saan naganap ang unang labanan ng Himagsikang Pilipino noong 1896?", A = "Cavite", B = "Balintawak", C = "Pugad Lawin", D = "Intramuros", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang pangalan ng pinakamalaking kampo ng militar sa Pilipinas?", A = "Camp Crame", B = "Fort Bonifacio", C = "Camp Aguinaldo", D = "Camp Abad", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang pangalan ng unang barkong dumaan sa Pilipinas sa ilalim ni Magellan?", A = "Santa Maria", B = "Victoria", C = "Santiago", D = "Trinidad", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang tawag sa kasunduan kung saan ibinenta ng Espanya ang Pilipinas sa Estados Unidos?", A = "Treaty of Versailles", B = "Treaty of Paris", C = "Pact of Biak-na-Bato", D = "Treaty of Washington", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pangunahing dahilan ng Labanan sa Tirad Pass?", A = "Pag-aagaw ng teritoryo", B = "Pagtakas ni Aguinaldo", C = "Rebolusyon laban sa Amerikano", D = "Pagtatanggol ng Katipunan", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Ano ang naging papel ni Apolinario Mabini sa pamahalaan ni Aguinaldo?", A = "Pangulo", B = "Kalihim ng Digmaan", C = "Punong Ministro", D = "Kalihim ng Edukasyon", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang orihinal na pangalan ng Rizal Park?", A = "Bagumbayan", B = "Luneta", C = "Plaza Mayor", D = "Fort Santiago", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang unang pangalan ng Makati noong panahon ng mga Espanyol?", A = "San Pedro de Makati", B = "Santa Ana", C = "San Felipe Neri", D = "Paco", CorrectAnswer = "A" },
                new Question { QuestionText = "Anong lungsod ang unang naging kabisera ng Pilipinas sa ilalim ng mga Amerikano?", A = "Manila", B = "Cebu", C = "Baguio", D = "Davao", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang pangalan ng katutubong hari na lumaban sa mga Espanyol sa Visayas?", A = "Lapu-Lapu", B = "Rajah Sulayman", C = "Datu Puti", D = "Tamblot", CorrectAnswer = "D" },
                
                new Question { QuestionText = "Ano ang dahilan ng Battle of Mactan noong 1521?", A = "Pagtanggol ni Lapu-Lapu sa kanyang teritoryo", B = "Paggalugad ni Magellan", C = "Pakikipagkasundo sa Espanya", D = "Pagkalat ng Kristiyanismo", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang tawag sa opisyal na pahayagan ng Katipunan?", A = "La Solidaridad", B = "Kalayaan", C = "La Independencia", D = "El Renacimiento", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pangalan ng sikat na Datu na nagmula sa Panay at lumipat sa Mindoro?", A = "Datu Kalantiaw", B = "Datu Puti", C = "Datu Makabulos", D = "Datu Lapu-Lapu", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang tawag sa mga sundalong Pilipino na sumuporta sa mga Amerikano noong Ikalawang Digmaang Pandaigdig?", A = "Hukbalahap", B = "Filipino Scouts", C = "Gerilya", D = "Makapili", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang tunay na pangalan ni Gregorio del Pilar?", A = "Gregorio Hizon del Pilar", B = "Gregorio Hilario del Pilar", C = "Gregorio Santos del Pilar", D = "Gregorio Antonio del Pilar", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Sino ang sumulat ng 'Kartilya ng Katipunan'?", A = "Emilio Jacinto", B = "Andres Bonifacio", C = "Jose Rizal", D = "Apolinario Mabini", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang kahulugan ng 'Maka-Diyos, Maka-Tao, Makakalikasan, at Makabansa'?", A = "Pambansang Panata", B = "Pambansang Panunumpa", C = "Pambansang Sumpa", D = "Pambansang Hangarin", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang batas na nagtakda sa pagiging pambansang wika ng Filipino?", A = "Batas Republika Blg. 1425", B = "Batas Komonwelt Blg. 184", C = "Saligang Batas ng 1987", D = "Batas Republika Blg. 7104", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang pangalan ng pinakamalaking pulo sa Pilipinas?", A = "Mindanao", B = "Luzon", C = "Visayas", D = "Palawan", CorrectAnswer = "B" }

                                // Add more questions here...
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
                FormTitle = "Filipino Quiz (Hard)",
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
                FormTitle = "Filipino Quiz (Hard)",
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
