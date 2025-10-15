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
    public partial class CharacterAnalysisFilEasy : Form
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
        private string selectedStory;
        public class Question
        {
            public string QuestionText { get; set; }
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string CorrectAnswer { get; set; }
        }
        public CharacterAnalysisFilEasy()
        {
            InitializeComponent();

            Random rand = new Random();
            int selectedStoryIndex = rand.Next(0, stories.Length);
            EssayrichTextBox1.Text = stories[selectedStoryIndex];

            // Load the corresponding quiz based on the selected story
            if (selectedStoryIndex == 0) // Jujutsu Kaisen
            {
                questions = new List<Question>(questions1);
            }
            else if (selectedStoryIndex == 1) // Superhero
            {
                questions = new List<Question>(questions2);
            }
            else // Animal
            {
                questions = new List<Question>(questions3);
            }

            // Shuffle and pick 5 random questions
            questions = questions.OrderBy(q => rand.Next()).Take(5).ToList();

            // Initialize answered questions
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
                FormTitle = "Filipino Analysis (Easy)",
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
                FormTitle = "Filipino Analysis (Easy)",
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
        private string[] stories = {
        //Story 1
        "                       THE CAPTAIN'S SACRIFICE\n\n" +
        "Si Kapitan Luis Alonzo ay isang magiting na lider ng sundalo na nakipaglaban sa isang digmaan na nagbukas ng mga " +
        "bagong pagsubok sa kanyang buhay. Sa kabila ng lahat ng kalupitan ng digmaan, si Kapitan Luis ay hindi natatakot. Sa " +
        "bawat laban, lumalakas ang kanyang paninindigan at natutunan niyang maging mas mapanuri sa mga sitwasyon. Kahit na " +
        "nahirapan, hindi siya sumuko sa mga misyon at palaging inuuna ang kapakanan ng kanyang mga tauhan at ang layunin ng digmaan.\n\n" +

        "Ngunit isang malupit na pangyayari ang tumama sa kanya, nang ang kanyang pinakamatalik na kaibigan ay mamatay sa isang engkwentro. " +
        "Ang pagkawala ng kanyang kaibigan ay naging isang malupit na aral sa kanya, at dito nagsimulang magbago ang kanyang pananaw. " +
        "Nagsimula siyang magtatanong tungkol sa mga dahilan ng digmaan at kung anong uri ng kapayapaan ang mangyayari pagkatapos ng " +
        "lahat ng sakripisyo. Sa kabila ng mga tanong na ito, nagpatuloy siya sa pakikibaka at ginawa ang lahat upang matulungan ang kanyang " +
         "mga kasamahan.\n\n" +

        "Sa huli, natutunan ni Kapitan Luis na ang tunay na lakas ay hindi lamang nasusukat sa mga tagumpay sa laban, " +
        "kundi sa pagtanggap sa mga sakripisyo at pagkatalo, at sa patuloy na pagtulong sa mga kapwa. Ang mga aral na natutunan " +
        "niya sa digmaan ay nagbigay sa kanya ng lakas upang magpatuloy sa buhay at magpatibay ng mga ugnayan sa mga tao sa paligid niya.",
        //Story 2
        "                       THE STUDENT'S STRUGGLE\n\n" +
        "Si Sarah ay isang mag-aaral na naghahanda para sa kanyang huling taon sa kolehiyo, ngunit natutunan niya na ang buhay ng isang estudyante " +
        "ay hindi lamang tungkol sa mga grado. Ang kanyang mga araw ay puno ng stress, presyon, at paghihirap, ngunit hindi siya nagpatinag. " +
        "Sa kanyang mga pagsubok, natutunan niyang mas pagtuunan ng pansin ang kanyang kalusugan at emosyonal na estado. Sa tulong ng kanyang " +
        "mga kaibigan at isang tagapayo, nakahanap siya ng mga paraan upang mapanatili ang balanse sa pagitan ng mga akademiko at personal na buhay.\n\n" +

        "Ang pinakamahirap na bahagi ay ang paghahati ng oras para sa lahat ng bagay na kailangang gawin. Madalas ay siya ay nahirapan sa mga " +
        "deadline at pressure mula sa mga proyekto, ngunit nagpatuloy siya sa paghahanap ng mga epektibong estratehiya tulad ng time management " +
        "at self-care techniques. Ang mga simpleng hakbang na ito ay nakatulong sa kanya na mas madali at mas maayos na makayanan ang mga gawain sa araw-araw.\n\n" +

        "Sa pagtatapos ng kanyang kolehiyo, napagtanto ni Sarah na ang pinaka-mahalaga ay hindi lamang ang natutunan niya sa mga libro, " +
        "kundi ang mga buhay na nagbago at mga kasamahan na naging matatag na bahagi ng kanyang paglalakbay. Ang kanyang pagtatapos " +
        "ay hindi lamang isang akademikong tagumpay, kundi isang personal na tagumpay din sa pagtanggap sa sarili at sa mga natutunan sa mga pagsubok.\n",
        //Story 3
        "                       THE GLOW-UP JOURNEY\n\n" +
        "Si Zoe ay isang binibini na nagsimula ng isang bagong kabanata sa kanyang buhay, isang journey na tinatawag na 'glow-up.' Nais niyang " +
        "magbago, hindi lamang ang panlabas na hitsura kundi pati na rin ang kanyang pag-iisip at emosyonal na kalusugan. Simula nang magpasya " +
        "siyang baguhin ang kanyang mga nakagawian at buhay, nagsimula siyang mag-focus sa kanyang sarili, maglaan ng oras para sa self-care, at " +
        "magpatuloy sa mga positibong gawain. Ginamit niya ang kanyang oras upang mag-ehersisyo, magbasa ng mga libro, at gawin ang mga bagay na " +
        "magpapalakas sa kanyang loob.\n\n" +

        "Ang unang hakbang na ginawa ni Zoe ay ang pag-aalaga sa kanyang balat at kalusugan, isang aspeto ng buhay na madalas niyang napapabayaan " +
        "noon. Kasabay nito, nagbago rin ang kanyang social media habits at pinili niyang mag-focus sa mga bagay na tunay na mahalaga. Hindi naging " +
        "madali ang mga unang hakbang, ngunit sa paglipas ng panahon, natutunan niyang tanggapin ang sarili at ang kanyang mga imperfections. Ito " +
        "ay naging isang mahalagang bahagi ng kanyang glow-up journey.\n\n" +

        "Sa huli, natutunan ni Zoe na ang tunay na pagbabago ay hindi nakasalalay sa mga pisikal na aspeto lamang, kundi sa kung paano mo " +
        "pinapahalagahan at minamahal ang iyong sarili. Sa pamamagitan ng mga bagong kaalaman at karanasan, hindi lamang ang kanyang hitsura " +
        "ang nagbago kundi pati na rin ang kanyang mindset at relasyon sa mga tao sa kanyang paligid. Naging mas positibo siya at mas kumpiyansa " +
        "sa sarili, na naging daan upang magkaroon siya ng mas malalim na koneksyon sa kanyang pamilya, kaibigan, at komunidad.\n"
        };

        private List<Question> questions1 = new List<Question>
        {
            new Question { QuestionText = "Ano ang pangunahing papel ni Kapitan Luis Alonzo sa labanan?", A = "Siya ay isang mediko", B = "Siya ay isang sniper", C = "Siya ay pinuno ng kanyang yunit", D = "Siya ay isang intelligence officer", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang nag-udyok kay Kapitan Luis na magpatuloy sa pakikibaka sa kabila ng matinding panganib?", A = "Ang kanyang paghihiganti", B = "Ang kanyang katapatan sa kanyang mga tauhan at bansa", C = "Ang kanyang pag-asa sa isang mapayapang resolusyon", D = "Ang kanyang kagustuhan na mapromote", CorrectAnswer = "B" },
            new Question { QuestionText = "Anong malupit na pangyayari ang nakakaapekto sa pag-iisip ni Kapitan Luis?", A = "Ang pagkawala ng kanyang pinakamatalik na kaibigan", B = "Ang pagtataksil ng kanyang tiyuhin", C = "Ang pagkamatay ng kanyang ama", D = "Ang pagkamatay ng kanyang mga kasama", CorrectAnswer = "D" },
            new Question { QuestionText = "Ano ang naramdaman ni Kapitan Luis pagkatapos ng labanan?", A = "Tinalo at walang pag-asa", B = "Proud ngunit pagod", C = "Magaan at masaya", D = "Walang pakialam", CorrectAnswer = "B" },
            new Question { QuestionText = "Anong aral ang natutunan ni Kapitan Luis mula sa digmaan?", A = "Ang tunay na lakas ay nasa kapangyarihan", B = "Ang tagumpay ay nakakamtan mag-isa", C = "Ang sakripisyo at pagkakaisa ang susi sa tagumpay", D = "Ang tagumpay ay makakamtan nang walang kapalit", CorrectAnswer = "C" },
            new Question { QuestionText = "Anong karakteristika ni Kapitan Luis ang nagpapakita ng kanyang pagiging lider?", A = "Pagiging matapang at mapanuri", B = "Pagiging tahimik at mahiyain", C = "Pag-iwas sa tungkulin", D = "Pagpapabaya sa kanyang tauhan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging epekto ng digmaan kay Kapitan Luis?", A = "Naging mas mahina ang kanyang loob", B = "Nagsimula siyang magduda sa layunin ng digmaan", C = "Naging pabaya siya sa kanyang tungkulin", D = "Nagdesisyon siyang sumuko", CorrectAnswer = "B" },
            new Question { QuestionText = "Bakit hindi sumuko si Kapitan Luis sa kabila ng hirap ng laban?", A = "Dahil gusto niyang ipaghiganti ang kanyang kaibigan", B = "Dahil iniisip niyang mas mabuti siyang pinuno kaysa sa iba", C = "Dahil naniniwala siyang may mas mataas na dahilan ang kanilang laban", D = "Dahil natatakot siyang maparusahan", CorrectAnswer = "C" },
            new Question { QuestionText = "Paano naapektuhan ni Kapitan Luis ang kanyang mga kasamahan?", A = "Pinamunuan niya sila ng may katatagan at inspirasyon", B = "Pinabayaan niya sila sa laban", C = "Pinilit niyang umalis sila sa digmaan", D = "Naging mahina siya sa kanilang harapan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangunahing tema ng kwento ni Kapitan Luis?", A = "Pagpapahalaga sa kapangyarihan", B = "Pagtanggap sa sakripisyo at pagkatuto mula sa pagkatalo", C = "Paghahanap ng kayamanan", D = "Pag-iwas sa tungkulin", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang isang mahalagang aral na natutunan ni Kapitan Luis?", A = "Mas mahalaga ang tagumpay kaysa sa moralidad", B = "Ang sakripisyo ay walang saysay", C = "Ang tunay na lakas ay nasa loob ng puso", D = "Ang digmaan ay isang laro", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang naging epekto ng pagkawala ng kanyang kaibigan kay Kapitan Luis?", A = "Nagbago ang kanyang pananaw sa digmaan", B = "Lalo siyang naging agresibo", C = "Tinalikuran niya ang kanyang yunit", D = "Pinili niyang hindi na lumaban", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pinakamalaking pagsubok na hinarap ni Kapitan Luis?", A = "Ang kanyang personal na pagdududa at lungkot", B = "Ang pag-atake ng isang malaking hukbo", C = "Ang kakulangan ng pagkain", D = "Ang kakulangan sa mga sundalo", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ipinapakita ng relasyon ni Kapitan Luis sa kanyang mga kasamahan?", A = "Walang malasakit sa kanila", B = "Handang magsakripisyo para sa kanila", C = "Ginagamit lang sila para sa kanyang layunin", D = "Pinoprotektahan niya lamang ang malalakas", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang pangunahing dahilan ng digmaan sa kwento?", A = "Pagkamit ng kalayaan at kapayapaan", B = "Pagnanais ng mas malaking teritoryo", C = "Personal na away ng dalawang pinuno", D = "Paghahanap ng kayamanan", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano ipinakita ng kwento ang tunay na kahulugan ng pagiging isang lider?", A = "Sa pamamagitan ng kapangyarihan at pananakot", B = "Sa pamamagitan ng pagmamalasakit at sakripisyo", C = "Sa pamamagitan ng pag-abandona sa kanyang tauhan", D = "Sa pamamagitan ng pag-iwas sa responsibilidad", CorrectAnswer = "B" },
            new Question { QuestionText = "Paano nagbago ang pananaw ni Kapitan Luis sa digmaan?", A = "Nais niyang mas makilala ito nang mas malalim", B = "Nagsimula siyang kwestyunin ang halaga nito", C = "Mas lalo niyang gustong lumaban", D = "Gusto niyang sumuko", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging epekto ng digmaan sa kaisipan ni Kapitan Luis?", A = "Naging mas mapanuri siya sa kanyang mga desisyon", B = "Naging pabaya siya sa kanyang tungkulin", C = "Naging mas makasarili siya", D = "Naging walang pakialam siya sa kanyang mga tauhan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang simbolismo ng sakripisyong ginawa ni Kapitan Luis?", A = "Ang digmaan ay walang saysay", B = "Ang tunay na tagumpay ay nasa puso ng isang pinuno", C = "Ang sakripisyo ay isang tanda ng kahinaan", D = "Ang digmaan ay laging dapat ipagpatuloy", CorrectAnswer = "B" }
        };

        private List<Question> questions2 = new List<Question>
        {
            new Question { QuestionText = "Ano ang pangunahing hamon ni Sarah sa kanyang huling taon sa kolehiyo?", A = "Ang paghahanap ng trabaho", B = "Ang pagpapantay ng akademiko at personal na buhay", C = "Ang paggawa ng mga bagong kaibigan", D = "Ang pagpili ng kanyang major", CorrectAnswer = "B" },
            new Question { QuestionText = "Sino ang tumulong kay Sarah na makayanan ang kanyang stress at presyon sa akademiko?", A = "Ang kanyang mga magulang", B = "Ang kanyang mga kaibigan", C = "Isang tagapayo", D = "Ang kanyang mga guro", CorrectAnswer = "C" },
            new Question { QuestionText = "Anong estratehiya ang ginamit ni Sarah upang mapabuti ang kanyang akademikong pagganap?", A = "Ang pag-iwas sa personal na buhay", B = "Pag-aaral ng mas matagal na oras bawat araw", C = "Paggamit ng time management at self-care techniques", D = "Pagtrabaho mag-isa nang walang tulong", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang natutunan ni Sarah pagkatapos ng kanyang pagtatapos?", A = "Ang kanyang degree ang pinakamahalagang bahagi ng kanyang kolehiyo", B = "Ang tunay na lakas ay mula sa pagpupursige at self-care", C = "Ang kanyang tagumpay ay dahil sa pagtatrabaho mag-isa", D = "Ang pagtatapos ang pinakamahirap na bahagi", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naramdaman ni Sarah sa araw ng kanyang pagtatapos?", A = "Nababalisa at hindi sigurado", B = "Proud at matagumpay", C = "Walang pakialam at pagod", D = "Panghihinayang sa kanyang mga pagpili", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang pangunahing dahilan kung bakit nahihirapan si Sarah sa kanyang huling taon?", A = "Dahil sa dami ng kanyang akademikong gawain", B = "Dahil sa kanyang mga problema sa pamilya", C = "Dahil sa kawalan ng interes sa pag-aaral", D = "Dahil sa kakulangan ng suporta mula sa kanyang guro", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ginawa ni Sarah upang mapanatili ang balanse sa pagitan ng pag-aaral at personal na buhay?", A = "Nag-focus lamang sa pag-aaral", B = "Pinaubaya ang lahat ng gawain sa kanyang kaibigan", C = "Gumamit ng time management at self-care techniques", D = "Sumuko sa kanyang pag-aaral", CorrectAnswer = "C" },
            new Question { QuestionText = "Bakit mahalaga ang self-care techniques para kay Sarah?", A = "Upang mapanatili ang kanyang kalusugang mental at emosyonal", B = "Upang magkaroon ng mas mataas na grado", C = "Upang mapabilib ang kanyang mga guro", D = "Upang maiwasan ang paggawa ng takdang-aralin", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pinakamahirap na aspeto ng pagiging isang estudyante para kay Sarah?", A = "Pag-aaral ng mahihirap na asignatura", B = "Pagharap sa stress at presyon", C = "Paggawa ng maraming kaibigan", D = "Pagdalo sa klase araw-araw", CorrectAnswer = "B" },
            new Question { QuestionText = "Paano nakaapekto ang kanyang mga kaibigan sa kanyang buhay?", A = "Tinulungan siya upang manatiling motivated", B = "Inimpluwensyahan siyang tumigil sa pag-aaral", C = "Binigyan siya ng mas maraming trabaho", D = "Hindi sila naging malaking bahagi ng kanyang buhay", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naramdaman ni Sarah habang hinaharap ang kanyang mga deadline?", A = "Relaxed at kalmado", B = "Balisa at stress", C = "Walang pakialam", D = "Masaya at excited", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging epekto ng time management sa kanyang buhay?", A = "Nakatulong ito upang mapanatili ang balanse sa kanyang schedule", B = "Nagdagdag ito ng mas maraming trabaho", C = "Wala itong naitulong sa kanya", D = "Naging dahilan ito upang maging pabaya siya", CorrectAnswer = "A" },
            new Question { QuestionText = "Bakit mahalaga para kay Sarah ang suporta mula sa isang tagapayo?", A = "Upang magkaroon ng gabay sa pagharap sa kanyang stress", B = "Upang mabawasan ang kanyang mga takdang-aralin", C = "Upang makaiwas sa pag-aaral", D = "Upang maging mas sikat", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangunahing mensahe ng kwento ni Sarah?", A = "Ang tagumpay ay hindi lamang nasusukat sa grado", B = "Ang pag-aaral ay dapat palaging unahin sa lahat ng bagay", C = "Mas madali ang buhay ng isang estudyante kung hindi ka mag-aaral", D = "Mas mahalaga ang pakikisalamuha kaysa sa edukasyon", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang natutunan ni Sarah tungkol sa stress management?", A = "Mahalaga ito upang mapanatili ang focus at kalusugan", B = "Hindi ito mahalaga sa buhay ng isang estudyante", C = "Ito ay isang bagay na hindi niya kailangang pagtuunan ng pansin", D = "Mas mabuti ang hindi pagharap sa stress", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano nakatulong ang kanyang karanasan sa kolehiyo sa kanyang personal na pag-unlad?", A = "Natuto siyang harapin ang mga hamon nang mas matatag", B = "Nawala ang kanyang interes sa pag-aaral", C = "Lalong lumala ang kanyang stress", D = "Naging mas mahina ang kanyang kumpiyansa", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isang aral na natutunan ni Sarah matapos ang kanyang paglalakbay bilang estudyante?", A = "Mahalaga ang pag-aalaga sa sarili sa kabila ng abala sa buhay", B = "Mas mahalaga ang mataas na marka kaysa sa mental health", C = "Dapat iwasan ang pakikisalamuha upang makapagtapos", D = "Ang pagiging masyadong abala ay normal at hindi problema", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naramdaman ni Sarah sa araw ng kanyang pagtatapos?", A = "Isang personal na tagumpay at kasiyahan", B = "Walang saysay ang lahat ng kanyang ginawa", C = "Pagod at walang gana", D = "Hindi sigurado sa kanyang kinabukasan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isang positibong epekto ng kanyang mga pagsubok bilang estudyante?", A = "Mas naging handa siya sa hinaharap", B = "Naging dahilan ito upang sumuko siya sa buhay", C = "Lalo siyang natakot sa bagong hamon", D = "Nawala ang kanyang interes sa personal na paglago", CorrectAnswer = "A" }
        };

        private List<Question> questions3 = new List<Question>
        {
            new Question { QuestionText = "Ano ang nag-udyok kay Zoe na magsimula ng kanyang glow-up journey?", A = "Presyon mula sa mga kaibigan", B = "Ang kanyang pagnanais na maging sikat", C = "Ang kanyang kagustuhan na mag-improve at magkaroon ng kumpiyansa", D = "Isang makeover show", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang unang hakbang na ginawa ni Zoe sa kanyang transformasyon?", A = "Bumili ng bagong damit", B = "Binago ang kanyang social media habits", C = "Pinangalagaan ang kanyang balat at kalusugan", D = "Binago ang kanyang hairstyle", CorrectAnswer = "C" },
            new Question { QuestionText = "Paano naapektuhan ng transformasyon ni Zoe ang kanyang mindset?", A = "Siya ay naging mas self-conscious", B = "Siya ay nagkaroon ng kumpiyansa at positibong pananaw", C = "Siya ay naging mayabang", D = "Siya ay nawalan ng focus sa kanyang mga layunin", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang natutunan ni Zoe tungkol sa kanyang glow-up process?", A = "Ito ay tungkol lamang sa panlabas na anyo", B = "Ang tunay na pagbabago ay tungkol sa panloob na paglago at pagmamahal sa sarili", C = "Madali at mabilis ito", D = "Wala itong epekto sa kanyang mga kaibigan", CorrectAnswer = "B" },
            new Question { QuestionText = "Paano naapektuhan ng journey ni Zoe ang kanyang relasyon sa iba?", A = "Siya ay naging isolated", B = "Siya ay nagkaroon ng mas matibay at malusog na relasyon", C = "Siya ay nawalan ng karamihan sa kanyang mga kaibigan", D = "Ang kanyang mga relasyon ay naging mababaw", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang pangunahing dahilan kung bakit sinimulan ni Zoe ang kanyang glow-up journey?", A = "Upang mapansin ng iba", B = "Dahil gusto niyang baguhin ang kanyang buhay", C = "Dahil sa pressure mula sa social media", D = "Dahil gusto niyang sumali sa isang beauty contest", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang unang aspeto ng kanyang buhay na pinili niyang baguhin?", A = "Kanyang pananamit", B = "Kanyang balat at kalusugan", C = "Kanyang trabaho", D = "Kanyang ugali", CorrectAnswer = "B" },
            new Question { QuestionText = "Paano naapektuhan ng kanyang glow-up journey ang kanyang emosyonal na kalusugan?", A = "Naging mas tiwala siya sa sarili", B = "Naging mas insecure siya", C = "Naging mas dependent siya sa social media", D = "Naging mas mahiyain siya", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isang mahalagang pagbabago sa kanyang pang-araw-araw na gawain?", A = "Naglaan siya ng oras para sa self-care", B = "Tumigil siya sa pagkain ng malusog", C = "Pinutol niya ang komunikasyon sa kanyang pamilya", D = "Nag-focus lang siya sa kanyang hitsura", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isa sa pinakamalaking hamon na hinarap ni Zoe sa kanyang pagbabago?", A = "Pag-iwas sa social media", B = "Pagtanggap sa kanyang imperfections", C = "Pagpili ng bagong pananamit", D = "Pagkuha ng suporta mula sa kanyang pamilya", CorrectAnswer = "B" },
            new Question { QuestionText = "Paano nakatulong ang pagbabasa ng mga libro sa kanyang journey?", A = "Nakatulong ito upang mapalawak ang kanyang isipan at pananaw", B = "Ginamit niya ito upang gayahin ang ibang tao", C = "Naging paraan ito upang makaiwas sa ibang tao", D = "Hindi ito nakaapekto sa kanyang journey", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isang positibong epekto ng kanyang glow-up journey?", A = "Mas naging positibo siya sa buhay", B = "Mas nag-alala siya tungkol sa opinyon ng iba", C = "Naging sobrang perfectionist siya", D = "Naging masyado siyang abala sa kanyang hitsura", CorrectAnswer = "A" },
            new Question { QuestionText = "Bakit mahalaga ang pagtanggap sa sarili sa kanyang journey?", A = "Dahil hindi palaging perpekto ang lahat", B = "Dahil gusto niyang sumunod sa beauty standards", C = "Dahil gusto niyang mapansin ng iba", D = "Dahil gusto niyang maging sikat sa social media", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging epekto ng kanyang journey sa kanyang relasyon sa ibang tao?", A = "Mas lumalim ang kanyang koneksyon sa kanyang pamilya at kaibigan", B = "Naging mas malamig siya sa kanyang relasyon", C = "Lumayo siya sa lahat ng kanyang kaibigan", D = "Hindi ito nakaapekto sa kanyang relasyon sa iba", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang natutunan ni Zoe tungkol sa tunay na pagbabago?", A = "Ito ay nagmumula sa loob at hindi lang sa panlabas", B = "Ito ay dapat laging nakabase sa opinyon ng iba", C = "Ito ay mabilis at madali", D = "Ito ay tungkol lang sa pisikal na pagbabago", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano niya pinili ang mga bagong habits na kanyang isinama sa kanyang routine?", A = "Pinili niya ang mga bagay na makakatulong sa kanyang personal na paglago", B = "Sinunod niya ang ginagawa ng mga influencer sa social media", C = "Pinili niya ang pinakamadaling gawin", D = "Sinunod niya ang payo ng kanyang mga kaklase", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isang bagay na iniwasan ni Zoe upang mapanatili ang positibong mindset?", A = "Mga negatibong impluwensya mula sa social media", B = "Pakikipagkaibigan", C = "Pag-aaral", D = "Pag-aalaga sa sarili", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pinaka-mahalagang natutunan ni Zoe sa kanyang glow-up journey?", A = "Ang pagmamahal sa sarili ay mas mahalaga kaysa sa panlabas na anyo", B = "Ang pisikal na hitsura ang pinakamahalagang aspeto ng buhay", C = "Dapat palaging mag-adjust para sa ibang tao", D = "Mahalagang sumunod sa beauty standards", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isang pangunahing mensahe ng kwento ni Zoe?", A = "Ang pagbabago ay tungkol sa pagtanggap at pagmamahal sa sarili", B = "Mas mahalaga ang hitsura kaysa sa personalidad", C = "Mas mabuting sumunod sa trends kaysa sa sariling desisyon", D = "Dapat baguhin ang sarili upang matanggap ng iba", CorrectAnswer = "A" }

        };
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
                // Start the challenge'
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
    }
}