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
    public partial class DialogAnalysisMedium : Form
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
        public DialogAnalysisMedium()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "Paano karaniwang nagsisimula ang isang pormal na usapan sa Pilipinas?", A = "Magandang araw po", B = "Kamusta!", C = "Anong balita?", D = "Hala, ikaw na naman!", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang pinakamahalagang aspeto ng usapan sa harap ng nakatatanda?", A = "Pagpapakita ng respeto", B = "Pagbibigay ng opinyon", C = "Pagiging masaya", D = "Pagtawa nang malakas", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang karaniwang sinasabi ng isang tao kapag nais niyang magpaalam nang magalang?", A = "Sige, alis na ako", B = "Paalam na po", C = "Bye na lang", D = "Hindi na ako magtatagal", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang tawag sa salitang ginagamit upang magpakita ng pasasalamat?", A = "Pakiusap", B = "Salamat po", C = "Patawad", D = "Walang anuman", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang ibig sabihin ng ‘Po’ at ‘Opo’ sa isang usapan?", A = "Pagsusuri ng impormasyon", B = "Pagpapakita ng galit", C = "Paggalang sa nakatatanda o may awtoridad", D = "Pagtatanong", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Anong salitang ginagamit upang magpakita ng pagkabahala o pag-aalala?", A = "Bakit?", B = "Hala!", C = "Ay, naku!", D = "Oo nga", CorrectAnswer = "C" },
                new Question { QuestionText = "Paano ipinapakita ng mga Pilipino ang pagiging magalang sa usapan?", A = "Pag-iiwas sa mata ng kausap", B = "Pagpapakita ng tuwa sa lahat ng oras", C = "Pag-gamit ng mga 'po' at 'opo'", D = "Pagpapatawa sa harap ng iba", CorrectAnswer = "C" },
                new Question { QuestionText = "Anong uri ng tanong ang ginagamit ng mga Pilipino upang makipag-usap ng maayos?", A = "Direct at blunt", B = "Maingat at magalang", C = "Walang pakialam", D = "Malupit at matalim", CorrectAnswer = "B" },
                new Question { QuestionText = "Paano pinapakita ang respeto sa mga nakatatanda sa usapan?", A = "Sa pagtanggi sa kanilang mga ideya", B = "Sa pagsunod sa kanilang mga utos", C = "Sa paggamit ng pormal na pananalita at 'po' o 'opo'", D = "Sa pag-alis ng usapan", CorrectAnswer = "C" },
                new Question { QuestionText = "Anong reaksyon ang karaniwang ipinapakita ng mga Pilipino kapag ang isang tao ay nagbigay ng isang maliit na pabor?", A = "Walang reaksyon", B = "Pagpapakita ng labis na pasasalamat", C = "Pag-aalinlangan", D = "Pagpapakita ng galit", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Ano ang madalas gamitin ng mga Pilipino upang ipahayag ang pagsang-ayon sa isang pahayag?", A = "Oo", B = "Siguro", C = "Ewan", D = "Bahala na", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang ibig sabihin ng pagsagot ng 'Bahala ka' sa isang usapan?", A = "Pagtanggi sa isang imbitasyon", B = "Pagsuko sa isang usapan", C = "Pagbibigay ng kalayaan sa desisyon ng kausap", D = "Pagpapakita ng galit", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang karaniwang ibig sabihin ng 'Pasensya ka na' sa isang usapan?", A = "Pagtanggi ng paumanhin", B = "Pagpapakita ng pagsisisi o paghingi ng tawad", C = "Pagpapahayag ng pagkainip", D = "Pagpapakita ng galit", CorrectAnswer = "B" },
                new Question { QuestionText = "Paano ipinapakita ng isang Pilipino ang pagiging interesado sa sinasabi ng kausap?", A = "Pagsingit sa usapan", B = "Pagtango at pagsagot ng 'Ah, ganun ba?'", C = "Pagtitig sa ibang direksyon", D = "Pagpapatuloy ng ibang gawain", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang ibig sabihin ng 'Tignan natin' kapag sumagot ang isang Pilipino?", A = "Katiyakan na gagawin ang isang bagay", B = "Pagsuway sa patakaran", C = "Pagkakaroon ng pag-aalinlangan", D = "Pagpapahayag ng pagkainip", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Kapag may nagsabi ng 'Paano kung hindi ako sumang-ayon?', ano ang nais ipahiwatig?", A = "Pagtanggi sa isang ideya", B = "Pagpapakita ng pagsang-ayon", C = "Pagpapahayag ng kasiyahan", D = "Paghingi ng payo", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang ibig sabihin ng pagsagot ng 'Depende' sa isang tanong?", A = "Pagpapakita ng tiwala", B = "Hindi sigurado o may kondisyon", C = "Pagsang-ayon nang buo", D = "Pagpapahayag ng galit", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang maaaring ipakahulugan ng sagot na 'Ewan ko' sa isang usapan?", A = "Pagpapakita ng pagkainip", B = "Pagpapahayag ng hindi pagkakaintindi o kawalan ng interes", C = "Pagpapakita ng tiwala", D = "Pagpapahayag ng kasiyahan", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang ibig sabihin ng sagot na 'Bakit hindi?' kapag may nag-alok ng isang bagay?", A = "Pagtanggi ng alok", B = "Pag-aalinlangan", C = "Pagsang-ayon sa alok", D = "Pagpapahayag ng pagkainip", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang ibig sabihin ng 'Susubukan ko' kapag humingi ng pabor ang isang tao?", A = "Siguradong gagawin", B = "Hindi kayang gawin", C = "May posibilidad pero hindi tiyak", D = "Pagtanggi nang magalang", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Ano ang ipinapahiwatig ng pagsagot ng 'Saka na'?", A = "Pagpapaliban ng isang bagay", B = "Pagtanggi nang direkta", C = "Pagsang-ayon", D = "Pagpapakita ng kasiyahan", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang kahulugan ng pagsabi ng 'Bakit ba?' nang may mataas na tono?", A = "Pagtatanong ng dahilan", B = "Pagpapahayag ng pagkainis", C = "Pagpapakita ng paggalang", D = "Pagpapahayag ng kasiyahan", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang ibig sabihin ng pagsagot ng 'Sino may sabi?' kapag may narinig na balita?", A = "Pagtanggap ng impormasyon", B = "Pagtatanong tungkol sa pinagmulan ng impormasyon", C = "Pagtanggi sa impormasyon", D = "Pagpapakita ng galit", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang ibig sabihin ng pagsagot ng 'Ikaw na bahala'?", A = "Pagbibigay ng desisyon sa iba", B = "Pagtanggi ng responsibilidad", C = "Pagpapakita ng pagkainip", D = "Pagpapahayag ng kasiyahan", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang ipinapahiwatig ng 'Nagbibiro ka lang, di ba?'?", A = "Pagtanggap ng impormasyon", B = "Pagpapakita ng pag-aalinlangan", C = "Pagpapahayag ng kasiyahan", D = "Pagtanggi ng tulong", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Ano ang kahulugan ng 'Naku, wala akong magagawa'?", A = "Pagtanggap ng pagkatalo", B = "Pagtanggi sa isang utos", C = "Pagpapahayag ng pagkainip", D = "Pagpapakita ng tiwala", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang ipinapahiwatig ng 'Hayaan mo na lang' sa isang usapan?", A = "Pagtanggap ng sitwasyon", B = "Pagtanggi ng ideya", C = "Pagpapakita ng galit", D = "Pagpapahayag ng kasiyahan", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang ibig sabihin ng 'Ayos lang' bilang sagot sa 'Pasensya ka na'?", A = "Pagpapakita ng galit", B = "Pagtanggap at pagpapatawad", C = "Pagtanggi ng kasalanan", D = "Pagtatanong ng opinyon", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang ipinapahiwatig ng 'Sabagay, tama ka'?", A = "Pagsang-ayon na may pag-aalinlangan", B = "Pagpapahayag ng pagkainip", C = "Pagtanggi ng tulong", D = "Pagpapakita ng galit", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang ibig sabihin ng 'Kung sabagay' sa isang usapan?", A = "Pag-aalinlangan", B = "Pagpapakita ng tiwala", C = "Pagtanggap ng opinyon ng iba", D = "Pagpapahayag ng kasiyahan", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Ano ang kahulugan ng 'Kahit papaano' sa isang pangungusap?", A = "Pagsang-ayon", B = "Pagbibigay ng limitadong katiyakan", C = "Pagpapakita ng tiwala", D = "Pagpapahayag ng pagkainip", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang ibig sabihin ng 'Sa totoo lang' bago magpahayag ng opinyon?", A = "Pagpapahayag ng kasiyahan", B = "Pagbibigay diin sa katotohanan", C = "Pagpapakita ng tiwala", D = "Pagpapahayag ng pagkainip", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang ipinapahiwatig ng pagsagot ng 'Totoo nga ba?'?", A = "Pagtanggap ng impormasyon", B = "Pagtatanong ng katiyakan", C = "Pagsang-ayon", D = "Pagpapahayag ng kasiyahan", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang ibig sabihin ng pagsagot ng 'Wala na akong masasabi'?", A = "Pagtanggap ng sitwasyon", B = "Pagpapakita ng pagkainip", C = "Pagtanggi ng tulong", D = "Pagpapahayag ng kasiyahan", CorrectAnswer = "A" }


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
                FormTitle = "Dialog Analysis (Medium)",
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
                FormTitle = "Dialog Analysis (Medium)",
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
