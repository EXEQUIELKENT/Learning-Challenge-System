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
    public partial class CharacterAnalysisFilMedium : Form
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
        public CharacterAnalysisFilMedium()
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
            questions = questions.OrderBy(q => rand.Next()).Take(10).ToList();

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
                FormTitle = "Filipino Analysis (Medium)",
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
                FormTitle = "Filipino Analysis (Medium)",
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
        "                       ANG PAGLALAKBAY NI LITO\n\n" +
        "Si Lito, isang matandang jeepney driver, ay naglalakbay araw-araw sa mga abalang kalsada ng Maynila. " +
        "Marami na siyang karanasan, at ang kanyang jeepney, isang lumang modelo, ay nagsilbing saksi sa kanyang mga kwento. " +
        "Sa kabila ng kanyang edad, patuloy pa rin niyang pinapalakas ang makina ng kanyang jeep, umaasa na makapagbigay serbisyo sa mga pasahero. " +
        "Subalit, isang araw, nasira ang kanyang jeep at nahirapan siyang maghanap ng pamalit. " +
        "Napagdesisyunan niyang kumuha ng modernong jeep na may mas mataas na teknolohiya, ngunit naging hamon ito sa kanya dahil sa hindi pagkakaintindihan sa mga bagong regulasyon ng gobyerno.\n\n" +

        "Habang naghahanap siya ng solusyon, nakilala ni Lito ang mga kabataan na nais mapanatili ang tradisyon ng jeepney sa kabila ng mga pagbabago. " +
        "Sila ay nagbigay sa kanya ng ideya kung paano mapagsasama ang mga makabagong teknolohiya sa mga tradisyunal na paraan ng pagmamaneho. " +
        "Bagamat may mga kabiguan, natutunan ni Lito na hindi lang tungkol sa negosyo ang buhay, kundi ang pagtulong sa kapwa at ang pagmamahal sa kultura ng Pilipinas.\n\n" +

        "Nagkaroon si Lito ng pagkakataon upang ipakita ang kanyang jeepney sa isang lokal na festival. " +
        "Isang pagkakataon upang ipagdiwang ang kahalagahan ng jeepney sa kultura ng bansa at ang papel nito sa araw-araw na buhay ng mga tao. " +
        "Ang kanyang jeepney ay naging simbolo ng pagsusumikap at pagiging matatag sa kabila ng mga pagsubok sa buhay. " +
        "Sa kabila ng mga modernisasyon, naipakita ni Lito na may lugar pa rin ang tradisyon sa makabagong mundo.\n\n" +

        "Sa huli, napagtanto ni Lito na hindi lahat ng bagay ay kailangan palitan. " +
        "Ang kanyang jeepney ay naging isang alaala ng nakaraan na patuloy niyang isinusulong sa mga kabataan. " +
        "Nagpatuloy siya sa pagpapasada, pero ngayon ay mas maligaya at kontento sa kanyang papel sa buhay ng komunidad. " +
        "Ang jeepney na kanyang minamahal ay naging higit pa sa isang sasakyan—ito ay naging simbolo ng pagkakaisa at pagmamahal sa kultura ng Pilipino.\n\n" +

        "Hindi naging madali para kay Lito ang maglakbay sa kalsadang puno ng pagsubok, ngunit natutunan niyang ang bawat paghihirap ay may kasamang aral. " +
        "Sa bawat biyahe, natutunan niyang yakapin ang pagbabago, ngunit laging may respeto at pagpapahalaga sa kanyang mga pinagmulan.",
        //Story 2
        "                       ANG BAYANI NG EDSA\n\n" +
        "Si Marco, isang batang historyador, ay binigyan ng proyekto ng kanyang guro na magsaliksik tungkol sa mga hindi kilalang bayani " +
        "ng People Power Revolution noong 1986. Habang binabasa niya ang mga aklat at lumang dokumento, napansin niya ang pangalan ni " +
        "Isidro Salazar, isang aktibista na hindi gaanong kilala. Sa kanyang pananaliksik, natuklasan niya na si Isidro ay may malaking " +
        "papel sa pagpapabagsak ng diktadura, ngunit ang kanyang kwento ay naitago sa likod ng mga kilalang lider ng panahon.\n\n" +

        "Habang lumalalim ang kanyang pag-aaral, natutunan ni Marco na si Isidro ay naging bahagi ng mga underground na operasyon " +
        "laban sa mga pwersang militar noong panahon ng batas militar. Isa siya sa mga nag-organisa ng mga protesta at lihim na pagtulong " +
        "sa mga naapektuhan ng rehimeng Marcos. Hindi lang siya isang lider, kundi isang simbolo ng lakas ng loob ng mga ordinaryong Pilipino " +
        "na lumaban para sa kalayaan. Ang mga sakripisyo ni Isidro ay hindi naging kilala sa nakararami, ngunit siya ay patuloy na lumaban, " +
        "sa kabila ng mga panganib na dulot ng kanyang mga aksyon.\n\n" +

        "Dahil sa pagnanais na maipakilala si Isidro sa mas nakararami, nagsimula si Marco na magsagawa ng mga presentasyon at artikulo " +
        "tungkol sa kanya. Sa kanyang paghahanap ng mga dokumento at testimonya mula sa mga taong nakakilala kay Isidro, natutunan ni Marco " +
        "na ang mga bayani ay hindi laging may mga statues o pangalan sa mga pangunahing kalsada. Minsan, ang mga tunay na bayani ay mga " +
        "simpleng tao na may malalaking puso at malasakit sa kanilang bayan.\n\n" +

        "Nagdesisyon si Marco na magsagawa ng isang exhibit sa kanyang paaralan upang ipakilala ang kwento ni Isidro at ng mga hindi " +
        "kilalang bayani ng EDSA. Ang exhibit ay naging isang matagumpay na proyekto, at nakatulong ito upang mabigyan ng pansin ang " +
        "kahalagahan ng hindi nakikitang mga bayani sa kasaysayan ng bansa. Si Marco ay natutong magbigay halaga sa mga hindi naririnig " +
        "na kwento at sa mga hindi kilalang tao na nagbigay ng kanilang buhay para sa kalayaan.\n\n" +

        "Sa wakas, napagtanto ni Marco na hindi lahat ng bayani ay sikat. Ang tunay na halaga ng pagiging bayani ay ang mga maliliit " +
        "na hakbang na nagdadala ng malaking pagbabago, at ang mga kwentong hindi laging napapansin pero may malaking epekto sa kinabukasan " +
        "ng bansa. Naging inspirasyon siya sa kanyang mga kaibigan at kapwa mag-aaral na ang tunay na bayani ay matatagpuan sa mga " +
        "hindi inaasahang lugar.",
        //Story 3
        "                       ANG PAGLALAKBAY NG ISANG CHEF\n\n" +
        "Si Rafael ay isang batang chef mula sa isang maliit na bayan sa probinsya. Sa murang edad, natutunan niyang magluto mula " +
        "sa kanyang lola na may hilig sa pagluluto ng mga tradisyonal na Filipino na putahe. Puno ng pangarap si Rafael na mapansin ang " +
        "Filipino cuisine sa buong mundo, kaya't nagdesisyon siyang magtungo sa Maynila upang mag-aral at magsimula ng kanyang karera " +
        "bilang isang chef. Ngunit hindi madali ang buhay sa lungsod, at maraming pagsubok ang kanyang hinarap upang makamit ang kanyang " +
        "pangarap.\n\n" +

        "Sa Maynila, nakatagpo siya ng trabaho sa isang kilalang restaurant na nag-specialize sa international cuisines. Ngunit hindi " +
        "siya nakuntento sa pagluluto ng mga pagkain mula sa ibang bansa. Nais niyang magluto ng mga pagkaing Filipino, ngunit ang mga tao " +
        "ay tila hindi interesado. Ngunit hindi siya sumuko. Ginamit niya ang mga natutunan niya mula sa kanyang lola at nagsimula siyang " +
        "mag-eksperimento sa mga lutuing Filipino gamit ang mga makabagong teknik at mga local na sangkap. Pinaghalo niya ang mga " +
        "tradisyunal na pagkain at mga bagong ideya upang makalikha ng kakaibang lasa.\n\n" +

        "Dahil sa kanyang dedikasyon at passion sa pagluluto, natutunan ni Rafael na ang Filipino cuisine ay may kakayahang mangibabaw sa " +
        "culinary world. Lumikha siya ng kanyang sariling restaurant na nagtatampok ng modernong pagtingin sa mga pagkaing Pilipino. Ang " +
        "kanyang mga dishes, tulad ng 'Adobo de Ribs' at 'Sinigang na Salmon,' ay naging patok sa mga international food critics at mga " +
        "lokal na customer. Hindi lang siya nakilala bilang isang mahusay na chef, kundi bilang isang nagtaguyod ng kultura ng Pilipinas " +
        "sa pamamagitan ng pagkain.\n\n" +

        "Ngunit habang umaabot siya sa tagumpay, natutunan ni Rafael na hindi sapat na maging magaling sa luto lamang. Kailangan niyang " +
        "mapanatili ang koneksyon sa kanyang mga ugat at sa mga taong nakatulong sa kanya upang makarating doon. Pinili niyang bumalik sa " +
        "kanyang bayan upang magturo ng culinary skills sa mga kabataan, na nagbigay sa kanya ng kasiyahan na hindi niya nahanap sa mga " +
        "parangal at awards.\n\n" +

        "Sa huli, napagtanto ni Rafael na ang kanyang tunay na tagumpay ay hindi sa pagkuha ng mga parangal, kundi sa pagbuo ng mga ugnayan " +
        "at pagpapalaganap ng kultura ng Pilipinas sa buong mundo. Ang kanyang restaurant ay naging simbolo ng isang bagong henerasyon ng " +
        "mga chef na may malasakit sa kanilang mga pinagmulan at handang ipagmalaki ang lasa at tradisyon ng Filipino cuisine sa buong " +
        "mundo."
        };

        private List<Question> questions1 = new List<Question>
        {
            new Question { QuestionText = "Ano ang pangunahing hamon na hinarap ni Lito sa kanyang jeepney?", A = "Ang pag-aayos ng kanyang jeep", B = "Ang pagbabayad ng buwis", C = "Ang pakikisalamuha sa mga pasahero", D = "Ang pagkakaroon ng modernong jeep", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging solusyon ni Lito sa pagkasira ng kanyang jeep?", A = "Binenta niya ang jeep", B = "Pinag-isipan niyang maghanap ng mas bagong jeep", C = "Nagtayo siya ng isang bagong negosyo", D = "Nagtulungan sila ng mga kabataan upang makakuha ng bagong jeep", CorrectAnswer = "B" },
            new Question { QuestionText = "Anong papel ang ginampanan ng mga kabataan sa kwento?", A = "Nagbigay ng ideya kay Lito kung paano magsimula ng negosyo", B = "Nagbigay ng tulong pinansyal kay Lito", C = "Tinulungan si Lito sa pagpapasada", D = "Nagturo sila kay Lito ng bagong teknolohiya para sa jeep", CorrectAnswer = "D" },
            new Question { QuestionText = "Ano ang simbolo ng jeepney para kay Lito?", A = "Isang negosyo", B = "Isang alaala ng nakaraan", C = "Isang luxury vehicle", D = "Isang sasakyan na pampasigla", CorrectAnswer = "B" },
            new Question { QuestionText = "Anong uri ng festival ang sinalihan ni Lito?", A = "Food Festival", B = "Music Festival", C = "Cultural Festival", D = "Jeepney Festival", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang pangunahing mensahe ng kwento ni Lito?", A = "Ang halaga ng modernisasyon", B = "Ang importansya ng teknolohiya", C = "Ang pagmamahal sa tradisyon at kultura", D = "Ang pagbabago sa bawat henerasyon", CorrectAnswer = "C" },
            new Question { QuestionText = "Bakit hindi sumuko si Lito sa kabila ng mga pagsubok?", A = "Dahil sa kanyang malasakit sa komunidad", B = "Dahil sa takot na mawalan ng trabaho", C = "Dahil sa pagpapakita ng kanyang mga anak", D = "Dahil sa tulong mula sa gobyerno", CorrectAnswer = "A" },
            new Question { QuestionText = "Anong aspeto ng kultura ang ipinagdiwang sa kwento?", A = "Modernong teknolohiya", B = "Pagtulong sa kapwa", C = "Pagpapahalaga sa jeepney bilang bahagi ng kultura", D = "Pag-aalaga sa kalikasan", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang ginamit na simbolo upang magbigay ng halaga kay Lito?", A = "Jeepney", B = "Tricycle", C = "Taxi", D = "Bus", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano naging inspirasyon si Lito sa kanyang komunidad?", A = "Nagbigay siya ng libreng sakay", B = "Nagturo siya ng mga kasanayan sa pamamahala ng negosyo", C = "Ipinakita niya ang kahalagahan ng pagtutulungan", D = "Nagtayo siya ng isang paaralan", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang trabaho ni Lito?", A = "Bus driver", B = "Jeepney driver", C = "Taxi driver", D = "Truck driver", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging pangunahing problema ni Lito sa kanyang jeep?", A = "Nawalan siya ng mga pasahero", B = "Nasira ang kanyang jeep", C = "Naubusan siya ng gasolina", D = "Nawalan siya ng lisensya", CorrectAnswer = "B" },
            new Question { QuestionText = "Bakit mahirap para kay Lito na palitan ang kanyang jeep?", A = "Wala siyang sapat na pera", B = "Hindi niya alam kung paano bumili ng bago", C = "Hindi niya gustong baguhin ang tradisyonal na jeep", D = "Hindi siya sanay sa makabagong teknolohiya", CorrectAnswer = "D" },
            new Question { QuestionText = "Ano ang ipinakita ng mga kabataan kay Lito?", A = "Paano magmaneho ng kotse", B = "Paano pagsamahin ang tradisyon at modernong teknolohiya", C = "Paano mag-apply ng loan", D = "Paano lumipat ng ibang trabaho", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging papel ng mga kabataan sa kwento?", A = "Tinulungan si Lito na makakuha ng bagong jeep", B = "Tinuruan si Lito kung paano lumipat ng trabaho", C = "Pinayuhan si Lito na huminto sa pagmamaneho", D = "Tinulungan si Lito na makahanap ng mas maraming pasahero", CorrectAnswer = "A" },
            new Question { QuestionText = "Bakit mahalaga ang jeepney sa kultura ng Pilipinas?", A = "Dahil ito ay isang tradisyunal na sasakyan", B = "Dahil mura ang pamasahe", C = "Dahil mabilis ito kaysa sa bus", D = "Dahil mas komportable ito kaysa sa ibang sasakyan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang aral na natutunan ni Lito?", A = "Ang pagbabago ay dapat yakapin ngunit hindi kinakailangang iwanan ang tradisyon", B = "Dapat laging bumili ng bagong sasakyan", C = "Ang pagiging makabago ay mas mahalaga kaysa sa tradisyon", D = "Ang jeepney ay dapat alisin sa transportasyon", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang nangyari sa jeepney ni Lito sa dulo ng kwento?", A = "Ibenta niya ito", B = "Ginamit niya ito sa festival", C = "Pininturahan niya ito ng bago", D = "Iniwan niya ito at bumili ng kotse", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang kahulugan ng jeepney para kay Lito?", A = "Isang lumang sasakyan", B = "Isang bahagi ng kanyang buhay at kultura", C = "Isang paraan upang kumita ng pera", D = "Isang sagabal sa modernisasyon", CorrectAnswer = "B" },
            new Question { QuestionText = "Bakit sumali si Lito sa isang festival?", A = "Para ipakita ang kanyang jeepney bilang simbolo ng kultura", B = "Para manalo ng premyo", C = "Para makahanap ng bagong trabaho", D = "Para makilala ng mga tao", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangunahing tema ng kwento?", A = "Pagpapahalaga sa tradisyon sa kabila ng modernisasyon", B = "Pagpapalit ng luma sa bago", C = "Pagpapahalaga sa teknolohiya", D = "Pag-iwan sa nakaraan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging epekto ng pagtutulungan ni Lito at ng mga kabataan?", A = "Naging masaya si Lito sa kanyang trabaho", B = "Nagkaroon siya ng bagong jeep", C = "Lumipat siya sa ibang trabaho", D = "Naging sikat siya sa buong bansa", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang kahulugan ng modernisasyon sa kwento?", A = "Paggamit ng makabagong teknolohiya", B = "Pagpapalit ng lahat ng luma sa bago", C = "Pagtanggal ng jeepney sa lansangan", D = "Pagtanggal ng tradisyon", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging pakiramdam ni Lito pagkatapos ng kanyang paglalakbay?", A = "Masaya at kuntento", B = "Nalungkot at sumuko", C = "Nagalit sa mga pagbabago", D = "Umalis sa Maynila", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang nagbago sa pananaw ni Lito sa dulo ng kwento?", A = "Napagtanto niyang ang pagbabago ay hindi masama kung isasama ang tradisyon", B = "Naisip niyang dapat nang alisin ang jeepney", C = "Nagdesisyon siyang tumigil sa pamamasada", D = "Pinili niyang lumipat sa ibang bansa", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang kahalagahan ng jeepney sa mga Pilipino?", A = "Ito ay isang abot-kayang pampublikong transportasyon", B = "Ito ay isang luxury vehicle", C = "Ito ay ginagamit lamang sa mga festival", D = "Ito ay hindi na ginagamit sa kasalukuyan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ipinapakita ng kwento tungkol sa pagtutulungan?", A = "Ang pagtutulungan ay mahalaga upang mapanatili ang tradisyon", B = "Ang pagtutulungan ay hindi kailangan sa modernong panahon", C = "Ang pagtutulungan ay isang lumang paniniwala", D = "Walang epekto ang pagtutulungan sa kwento", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ipinapahiwatig ng kwento tungkol sa pagbabago?", A = "Ang pagbabago ay dapat tanggapin ngunit may paggalang sa kultura", B = "Ang pagbabago ay laging masama", C = "Ang pagbabago ay dapat laging iwasan", D = "Ang pagbabago ay dapat pilitin kahit labag sa kalooban", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang mensahe ng kwento tungkol sa kultura ng Pilipinas?", A = "Ang kultura ay dapat ipagmalaki at ipagpatuloy", B = "Ang kultura ay dapat kalimutan para sa modernisasyon", C = "Ang kultura ay hindi mahalaga sa transportasyon", D = "Ang kultura ay hindi nagbabago kailanman", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang aral na mapupulot sa kwento?", A = "Ang tradisyon at modernisasyon ay maaaring pagsamahin", B = "Ang lumang bagay ay dapat palitan agad", C = "Ang modernisasyon ay dapat unahin kaysa sa kultura", D = "Ang teknolohiya ay hindi mahalaga sa transportasyon", CorrectAnswer = "A" }
        };

        private List<Question> questions2 = new List<Question>
        {
            new Question { QuestionText = "Ano ang natuklasan ni Marco tungkol kay Isidro Salazar?", A = "Siya ay isang kilalang politiko", B = "Siya ay isang aktibista na hindi kilala", C = "Siya ay isang sundalo sa EDSA", D = "Siya ay isang pangulo", CorrectAnswer = "B" },
            new Question { QuestionText = "Anong uri ng laban ang isinulong ni Isidro Salazar?", A = "Laban para sa kalayaan", B = "Laban para sa makatarungang presyo ng langis", C = "Laban para sa edukasyon", D = "Laban para sa mas magandang sahod", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano nakatulong si Marco sa pagpapakilala kay Isidro Salazar?", A = "Bumuo siya ng isang documentary", B = "Nagsagawa siya ng isang exhibit tungkol sa EDSA", C = "Sumulat siya ng isang aklat tungkol kay Isidro", D = "Nagbigay siya ng mga libreng lecture sa mga kabataan", CorrectAnswer = "B" },
            new Question { QuestionText = "Bakit hindi naging kilala si Isidro Salazar?", A = "Dahil sa kanyang kahirapan", B = "Dahil sa hindi tamang pagpapakilala sa kanya", C = "Dahil sa kanyang pagiging tahimik at hindi naghahanap ng atensyon", D = "Dahil sa kanyang pagiging politiko", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang pangunahing layunin ni Marco sa kanyang proyekto?", A = "Makapagpasikat", B = "Maipakilala ang mga hindi kilalang bayani", C = "Makapag-aral ng kasaysayan", D = "Magkaroon ng mataas na marka", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang natutunan ni Marco mula sa buhay ni Isidro?", A = "Ang tunay na bayani ay hindi laging kilala", B = "Ang bayani ay may malaking yaman", C = "Ang bayani ay laging may kapangyarihan", D = "Ang bayani ay kilala sa buong mundo", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging epekto ng proyekto ni Marco sa kanyang komunidad?", A = "Nagbigay ito ng inspirasyon sa mga kabataan", B = "Nagkaroon ng mga libreng pagkain para sa lahat", C = "Naging kilala siya bilang isang bayani", D = "Nagkaroon ng mga pagbabago sa pamahalaan", CorrectAnswer = "A" },
            new Question { QuestionText = "Anong uri ng pagpapahalaga ang ipinakita ni Marco sa mga bayani?", A = "Pagtanggap at pagpapahalaga sa mga hindi kilalang bayani", B = "Pagtulong sa mga mahihirap", C = "Pagtulong sa mga may kapangyarihan", D = "Pagtanggap sa mga sikat na bayani", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangunahing mensahe ng kwento?", A = "Ang bawat tao ay may kakayahang maging bayani", B = "Ang mga bayani ay laging may mga statue", C = "Ang mga bayani ay hindi kailanman natatalo", D = "Ang mga bayani ay laging may malaking kapangyarihan", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano napatunayan ni Marco na ang mga hindi kilalang bayani ay may malaking halaga?", A = "Sa pamamagitan ng pag-aaral ng kasaysayan at pag-alala sa kanilang mga kwento", B = "Sa pamamagitan ng paggawa ng mga documentary", C = "Sa pamamagitan ng pagkakaroon ng mga parangal", D = "Sa pamamagitan ng pagdiriwang ng mga piyesta", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang proyekto na ibinigay kay Marco ng kanyang guro?", A = "Pagsulat ng isang libro", B = "Pagsasaliksik tungkol sa People Power Revolution", C = "Paggawa ng painting tungkol sa EDSA", D = "Pag-interview sa isang bayani", CorrectAnswer = "B" },
            new Question { QuestionText = "Sino ang hindi gaanong kilalang bayani na natuklasan ni Marco?", A = "Isidro Salazar", B = "Juan Luna", C = "Andres Bonifacio", D = "Jose Rizal", CorrectAnswer = "A" },
            new Question { QuestionText = "Anong papel ang ginampanan ni Isidro Salazar sa rebolusyon?", A = "Isang sundalo", B = "Isang lider ng protesta", C = "Isang mamamahayag", D = "Isang pulitiko", CorrectAnswer = "B" },
            new Question { QuestionText = "Bakit hindi kilala si Isidro sa kasaysayan?", A = "Hindi siya sumikat tulad ng ibang bayani", B = "Hindi siya totoo", C = "Nasa ibang bansa siya noong EDSA", D = "Isa siyang ordinaryong mamamayan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ginawa ni Isidro upang labanan ang diktadura?", A = "Sumulat ng aklat", B = "Lumaban sa sundalo", C = "Nag-organisa ng mga protesta", D = "Nagtayo ng paaralan", CorrectAnswer = "C" },
            new Question { QuestionText = "Paano natuklasan ni Marco ang tungkol kay Isidro?", A = "Sa mga lumang aklat at dokumento", B = "Sa kanyang pamilya", C = "Sa isang panaginip", D = "Sa internet", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ginawa ni Marco upang ipakilala si Isidro?", A = "Gumawa ng isang exhibit", B = "Isinulat siya sa isang dyaryo", C = "Naging guro sa kasaysayan", D = "Naglakbay sa buong bansa", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang mensahe ng kwento tungkol sa pagiging bayani?", A = "Ang tunay na bayani ay hindi palaging sikat", B = "Lahat ng bayani ay dapat kilalanin", C = "Ang bayani ay dapat may medalya", D = "Ang bayani ay dapat palaging nasa gobyerno", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangunahing aral na natutunan ni Marco?", A = "Mahalaga ang pagsasaliksik ng kasaysayan", B = "Dapat sikat ang isang bayani", C = "Masarap maging historian", D = "Mas mahirap ang pag-aaral kaysa sa iniisip", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang reaksyon ng mga kaklase ni Marco sa kanyang exhibit?", A = "Nagbigay ng inspirasyon sa kanila", B = "Hindi nila pinansin", C = "Nagalit sila", D = "Nabigo silang intindihin", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang People Power Revolution?", A = "Isang mapayapang rebolusyon laban sa diktadura", B = "Isang digmaan sa Pilipinas", C = "Isang patimpalak sa kasaysayan", D = "Isang partidong politikal", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ginawa ni Isidro upang matulungan ang mga apektado ng batas militar?", A = "Nagbigay ng tulong sa mga pamilya", B = "Nagtago sa ibang bansa", C = "Sumali sa gobyerno", D = "Nagtayo ng negosyo", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang dahilan kung bakit itinuloy ni Marco ang kanyang proyekto?", A = "Dahil nais niyang ipaalam ang kasaysayan", B = "Dahil gusto niyang maging sikat", C = "Dahil gusto niyang makakuha ng mataas na grado", D = "Dahil iniutos ng kanyang pamilya", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano naging matagumpay ang proyekto ni Marco?", A = "Dahil maraming natuto tungkol sa hindi kilalang bayani", B = "Dahil binigyan siya ng mataas na marka", C = "Dahil sumikat siya sa paaralan", D = "Dahil sumali siya sa isang TV show", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang kahulugan ng pagiging bayani batay sa kwento?", A = "Ang bayani ay maaaring isang ordinaryong tao na may malasakit sa bayan", B = "Ang bayani ay dapat maging sikat", C = "Ang bayani ay dapat maging pulitiko", D = "Ang bayani ay dapat may medalya", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ipinapakita ng kwento tungkol sa kasaysayan?", A = "May mga hindi nabibigyang pansin na bayani", B = "Lahat ng bayani ay sikat", C = "Hindi na mahalaga ang kasaysayan", D = "Dapat kalimutan ang mga lumang bayani", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang natutunan ni Marco tungkol sa mga hindi kilalang bayani?", A = "Mahalaga sila sa kasaysayan", B = "Hindi sila mahalaga", C = "Wala silang nagawang kontribusyon", D = "Hindi sila totoo", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang epekto ng exhibit ni Marco sa kanyang paaralan?", A = "Nagkaroon ng mas malalim na pag-unawa sa kasaysayan", B = "Nagkaroon ng bagong holiday", C = "Nagsimula silang kalimutan ang kasaysayan", D = "Nag-away ang mga mag-aaral tungkol dito", CorrectAnswer = "A" },
            new Question { QuestionText = "Bakit mahalaga ang pagbibigay halaga sa hindi kilalang bayani?", A = "Dahil sila rin ay nag-ambag sa kasaysayan", B = "Dahil sila ay sikat", C = "Dahil gusto nilang makilala", D = "Dahil gusto nilang magkapera", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pinakaimportanteng mensahe ng kwento?", A = "Ang tunay na bayani ay hindi kailangan maging sikat upang magbigay ng pagbabago", B = "Ang bayani ay dapat kilala ng lahat", C = "Ang bayani ay dapat may posisyon sa gobyerno", D = "Ang bayani ay dapat palaging nakikita sa TV", CorrectAnswer = "A" },
        };

        private List<Question> questions3 = new List<Question>
        {
            new Question { QuestionText = "Bakit nagdesisyon si Rafael na magtungo sa Maynila?", A = "Upang mag-aral ng international cuisines", B = "Upang magtayo ng sariling restaurant", C = "Upang mapansin ang Filipino cuisine sa buong mundo", D = "Upang makapag-aral ng Filipino culture", CorrectAnswer = "C" },
            new Question { QuestionText = "Anong mga pagkain ang ipinakilala ni Rafael sa kanyang restaurant?", A = "Western dishes", B = "Traditional Filipino dishes", C = "Modern Filipino dishes", D = "Asian fusion dishes", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang naging reaksyon ng mga international food critics sa mga pagkaing Pilipino ni Rafael?", A = "Hindi nila nagustuhan", B = "Nakipag-usap sila kay Rafael para sa mga improvements", C = "Pinuri nila ang kanyang mga pagkaing Filipino", D = "Hindi nila pinansin ang kanyang mga luto", CorrectAnswer = "C" },
            new Question { QuestionText = "Paano nakatulong si Rafael sa mga kabataan sa kanyang bayan?", A = "Nagbigay siya ng libreng pagkain", B = "Nagturo siya ng culinary skills", C = "Nagbigay siya ng scholarship", D = "Nagtayo siya ng bagong negosyo", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang natutunan ni Rafael tungkol sa tunay na tagumpay?", A = "Ito ay sa pagkakaroon ng mga parangal", B = "Ito ay sa pagbuo ng mga ugnayan at pagpapalaganap ng kultura", C = "Ito ay sa pagkakaroon ng maraming yaman", D = "Ito ay sa pagtanggap sa mga international standards", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging epekto ng Filipino cuisine ni Rafael sa mga international restaurants?", A = "Dumami ang mga nagbukas na Filipino restaurants", B = "Wala itong naging epekto", C = "Ang mga pagkain ay hindi naging sikat", D = "Nagbago ang culinary trends sa buong mundo", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano nakuha ni Rafael ang atensyon ng mga international critics?", A = "Sa pamamagitan ng pagdalo sa international competitions", B = "Sa pamamagitan ng social media", C = "Sa pamamagitan ng pagkain na may kakaibang lasa", D = "Sa pamamagitan ng pagpapakilala ng Filipino cuisine sa mga global platforms", CorrectAnswer = "D" },
            new Question { QuestionText = "Ano ang pananaw ni Rafael sa Filipino cuisine?", A = "Ito ay isang simpleng pagkain", B = "Ito ay may potensyal na maging tanyag sa buong mundo", C = "Ito ay hindi kasing tanyag ng ibang international cuisines", D = "Ito ay limitado sa Pilipinas", CorrectAnswer = "B" },
            new Question { QuestionText = "Bakit mahalaga kay Rafael ang kultura ng pagkain?", A = "Dahil ito ay isang paraan upang magkapera", B = "Dahil ito ay isang paraan upang mapanatili ang kultura", C = "Dahil ito ay isang paraan upang magtayo ng negosyo", D = "Dahil ito ay isang paraan upang makilala sa ibang bansa", CorrectAnswer = "B" },
            new Question { QuestionText = "Paano nakatulong ang Filipino cuisine sa pagpapalaki ng turismo sa Pilipinas?", A = "Sa pamamagitan ng pagpapakita ng mga bagong restaurant", B = "Sa pamamagitan ng pag-organisa ng mga food festivals", C = "Sa pamamagitan ng pagpapakilala ng bagong culinary culture", D = "Sa pamamagitan ng pagdagdag ng mga international menus sa mga restaurant", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang pangarap ni Miguel?", A = "Maging isang guro", B = "Maging isang sikat na chef", C = "Maging isang businessman", D = "Maging isang doktor", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang problema ni Miguel sa pagsisimula ng kanyang restaurant?", A = "Walang sapat na puhunan", B = "Walang sapat na talento", C = "Walang gustong kumain sa kanyang restaurant", D = "Ayaw ng kanyang pamilya", CorrectAnswer = "A" },
            new Question { QuestionText = "Saan unang nagtrabaho si Miguel?", A = "Sa isang pabrika", B = "Sa isang maliit na karinderya", C = "Sa isang five-star restaurant", D = "Sa isang tindahan ng gulay", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang espesyalidad ni Miguel sa pagluluto?", A = "Adobo", B = "Sinigang", C = "Lutong bahay na may modernong twist", D = "Mga imported na pagkain", CorrectAnswer = "C" },
            new Question { QuestionText = "Bakit umalis si Miguel sa kanyang unang trabaho?", A = "Dahil gusto niyang matuto pa", B = "Dahil mababa ang sahod", C = "Dahil nag-away sila ng kanyang boss", D = "Dahil hindi niya gusto ang pagluluto", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang dahilan kung bakit sumali si Miguel sa isang cooking competition?", A = "Para sa karangalan", B = "Para sa premyong cash na magagamit niya sa kanyang negosyo", C = "Para sumikat sa TV", D = "Para patunayan sa kanyang pamilya na kaya niya", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging inspirasyon ni Miguel sa pagluluto?", A = "Ang kanyang lola", B = "Ang mga chef na napapanood niya sa TV", C = "Ang kanyang mga kaibigan", D = "Ang mga pagkaing mula sa ibang bansa", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging resulta ng paglahok ni Miguel sa cooking competition?", A = "Nanalo siya at nagkaroon ng puhunan", B = "Natalo siya pero nakilala ang kanyang talento", C = "Hindi siya pinayagang sumali", D = "Hindi natuloy ang kompetisyon", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang natutunan ni Miguel mula sa pagkatalo sa competition?", A = "Na kailangan niya pang magsikap", B = "Na hindi siya magaling sa pagluluto", C = "Na dapat siyang sumuko", D = "Na hindi na siya dapat lumaban ulit", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano siya nakakuha ng sapat na pera upang magtayo ng restaurant?", A = "Sa isang investor na humanga sa kanya", B = "Sa kanyang pamilya", C = "Sa pagnanakaw", D = "Sa pagtaya sa lotto", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangalan ng restaurant na itinayo ni Miguel?", A = "Lutong Bahay ni Miguel", B = "Lasa at Puso", C = "Kusina ni Chef", D = "Ang Paborito", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang pangunahing layunin ni Miguel sa kanyang restaurant?", A = "Maging pinakamayaman na chef", B = "Ibahagi ang kanyang pagkaing puno ng pagmamahal", C = "Patunayan sa lahat na siya ang pinakamahusay", D = "Magbenta ng mamahaling pagkain", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang unang pagsubok na hinarap ni Miguel matapos buksan ang restaurant?", A = "Kakulangan ng customer", B = "Mataas na gastusin", C = "Mahirap humanap ng magagaling na tauhan", D = "Lahat ng nabanggit", CorrectAnswer = "D" },
            new Question { QuestionText = "Ano ang ginawa ni Miguel upang mapanatili ang kanyang restaurant?", A = "Nagbigay ng promosyon at nag-innovate sa kanyang menu", B = "Nagtaas ng presyo", C = "Ibinenta ang restaurant", D = "Isinara ang negosyo", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging reaksyon ng mga tao sa pagkain ni Miguel?", A = "Nagustuhan nila ito at naging tanyag ang kanyang restaurant", B = "Walang pumansin sa kanyang pagkain", C = "Napakamahal kaya hindi nagustuhan", D = "Hindi nila naiintindihan ang lasa", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang natutunan ni Miguel tungkol sa negosyo?", A = "Na dapat hindi sumusuko kahit may pagsubok", B = "Na dapat mabilis siyang yumaman", C = "Na mas madaling sumuko", D = "Na dapat walang magbago sa kanyang luto", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang sikreto ni Miguel sa tagumpay ng kanyang restaurant?", A = "Ang kanyang pagmamahal sa pagluluto", B = "Ang paggamit ng mamahaling sangkap", C = "Ang kanyang sikat na pangalan", D = "Ang kanyang pagiging istrikto", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang mensahe ng kwento?", A = "Ang tagumpay ay nagmumula sa sipag, tiyaga, at pagmamahal sa ginagawa", B = "Madaling yumaman sa restaurant", C = "Ang pagkain ay dapat laging mamahalin", D = "Ang pagluluto ay para lang sa may pera", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ginawa ni Miguel matapos makilala ang kanyang restaurant?", A = "Tinuruan niya ang iba tungkol sa pagluluto", B = "Itinigil niya ang pagluluto", C = "Pumunta siya sa ibang bansa", D = "Nagbenta siya ng kanyang restaurant", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ipinapakita ng kwento tungkol sa pangarap?", A = "Na hindi ito madaling matupad, pero posible kung magsisikap", B = "Na dapat lang mangarap nang walang gawa", C = "Na ang pangarap ay para lang sa mayayaman", D = "Na ang pangarap ay hindi dapat seryosohin", CorrectAnswer = "A" },
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