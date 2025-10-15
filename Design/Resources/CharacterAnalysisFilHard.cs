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
    public partial class CharacterAnalysisFilHard : Form
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
        public CharacterAnalysisFilHard()
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
            questions = questions.OrderBy(q => rand.Next()).Take(20).ToList();

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
                FormTitle = "Filipino Analysis (Hard)",
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
                FormTitle = "Filipino Analysis (Hard)",
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
         "                       ANG PAGLALAKBAY NI LOLA\n\n" +
        "Si Lola Inda ay isang matandang magsasaka na buong buhay na umasa sa kanyang maliit na lupa para mabuhay. Ang kanyang mga " +
        "pananim, mula palay hanggang gulay, ay naging kanyang kabuhayan at pagmamahal sa buhay. Ang lupaing ito ay minana pa niya sa " +
        "kanyang mga magulang at simbolo ng kanyang pamilya at kasaysayan.\n\n" +

        "Isang araw, dumating ang balitang bibilhin ang kanyang lupa para gawing subdivision. Nalungkot si Lola Inda, hindi dahil mawawalan " +
        "siya ng kabuhayan, kundi dahil sa posibilidad na mawala ang isang yaman na minahal niya ng buong puso. Para sa kanya, ang lupa " +
        "ay hindi lamang ari-arian kundi isang kayamanan ng kasaysayan.\n\n" +

        "Hinimok ng kanyang pamilya si Lola Inda na tanggapin na ang alok ng mga developer dahil malaking halaga ang kapalit. Ngunit tumanggi " +
        "siya. Para kay Lola Inda, ang pera ay panandalian lamang, ngunit ang lupa ay kayamanan ng susunod na henerasyon. Hindi madaling " +
        "tumayo laban sa mga tao na may kapangyarihan, ngunit hindi siya nawalan ng loob.\n\n" +

        "Nag-umpisa siyang humingi ng suporta mula sa kanyang mga kapitbahay at iba pang magsasaka sa kanilang lugar. Sa tulong ng mga NGO " +
        "at lokal na opisyal, nagkaroon sila ng mga forum upang pag-usapan ang kanilang mga karapatan bilang magsasaka. Ang laban na ito " +
        "ay nagbigay lakas kay Lola Inda at sa iba pa na ipaglaban ang kanilang karapatan sa lupa.\n\n" +

        "Habang nagpapatuloy ang kanilang kampanya, nagkaroon ng sigalot sa pagitan ng kanyang pamilya. Ang kanyang mga anak ay nais ng " +
        "mas magandang kinabukasan gamit ang perang maibibigay mula sa lupa. Ngunit ipinaliwanag ni Lola Inda na ang tunay na yaman ay " +
        "hindi nasusukat sa pera kundi sa dignidad at pagmamahal sa lupaing iniwan ng kanilang mga ninuno.\n\n" +

        "Sa kabila ng mga hamon, nanatiling matatag si Lola Inda. Lumapit siya sa mga media outlets upang maipakita ang kanilang sitwasyon. " +
        "Ang kwento niya ay nag-viral, at marami ang sumuporta sa kanilang laban. Unti-unting narinig ang kanilang boses, at sinimulan " +
        "ng gobyerno ang pagsusuri sa mga planong gagawin sa lupaing iyon.\n\n" +

        "Isang araw, natanggap ni Lola Inda ang balitang mananatili sa kanila ang lupa. Bagamat nagkaroon ng kompromiso, napanatili nilang " +
        "mga magsasaka ang karapatan sa lupa, at isinulong ang sustainable farming practices upang mapakinabangan pa ng maraming taon.\n\n" +

        "Sa huli, napagtanto ni Lola Inda na ang tunay na tagumpay ay hindi lamang ang pagtatagumpay sa laban kundi ang pagkakaisa ng " +
        "komunidad para sa isang mas mataas na layunin. Ang kanyang pagmamahal sa lupa ay nagbigay inspirasyon sa marami upang ipaglaban " +
        "ang karapatan ng mga maliliit na magsasaka.\n\n" +

        "Si Lola Inda ay nanatili sa kanyang lupa, nagtanim ng mas maraming pananim, at tinuruan ang mga kabataan sa kanilang lugar na " +
        "pahalagahan ang kalikasan at ang kanilang pinagmulan. Ang kanyang kwento ay naging inspirasyon sa marami na ipaglaban ang kung " +
        "ano ang tama at mahalaga.",
        //Story 2
        "                       ANG LARUAN NI NICO\n\n" +
        "Si Nico ay isang masayahing bata mula sa isang simpleng pamilya. Sa kabila ng kanilang kakulangan sa materyal na bagay, palaging " +
        "masaya si Nico sa piling ng kanyang mga magulang. Isang Pasko, natanggap ni Nico ang pinakamimithing laruan na matagal na niyang hinihiling.\n\n" +

        "Ang laruan ay isang makulay na kotse na may remote control, isang bagay na matagal niyang pinagdarasal. Napakasaya niya at buong " +
        "araw niyang nilaro ito. Ngunit nang lumabas siya sa kanilang bakuran, nakita niya ang mga kapitbahay niyang bata na naglalaro ng " +
        "mga sirang laruan o simpleng stick at lata.\n\n" +

        "Habang naglalaro, napansin ni Nico na maraming bata ang tumititig sa kanyang bagong laruan. Naramdaman niya ang saya sa kanyang " +
        "puso ngunit hindi niya maiwasang isipin ang kalungkutan ng mga batang walang ganoong laruan. Nagkaroon siya ng ideya na paglaruan " +
        "ito kasama nila.\n\n" +

        "Una, nahihiya ang ibang bata na lumapit. Ngunit sa mapilit na pag-anyaya ni Nico, unti-unti nilang sinubukang hawakan at gamitin " +
        "ang laruan. Ang saya at tawa ng mga bata ay naging musika sa tenga ni Nico, at naramdaman niya ang kakaibang ligaya sa pagbabahagi.\n\n" +

        "Isang araw, naisip ni Nico na dalhin ang kanyang laruan sa barangay upang ipahiram sa mas maraming bata. Dito, nag-organisa sila " +
        "ng simpleng salu-salo, kung saan ang bawat bata ay nagkaroon ng pagkakataon na maglaro ng laruan ni Nico. Ang saya ay naging " +
        "inspirasyon upang simulan ang isang donation drive para sa mga laruan.\n\n" +

        "Sa tulong ng kanyang mga magulang, nagsimula si Nico na mangalap ng mga lumang laruan mula sa kanilang mga kapitbahay. Maraming " +
        "tao ang nagbigay, at nakapamahagi sila ng laruan sa maraming bata sa kanilang lugar. Ang simple niyang laruan ay naging simula " +
        "ng isang mas malaking layunin.\n\n" +

        "Hindi naging madali ang lahat. May mga pagkakataong may hindi pagkakaintindihan sa mga bata sa pag-aagawan sa laruan. Ngunit " +
        "natutunan ni Nico na ang pagbabahagi ay hindi lamang tungkol sa materyal na bagay kundi sa pagtuturo ng pagkakaisa at pagkakaintindihan.\n\n" +

        "Sa huli, naging inspirasyon si Nico sa kanyang komunidad. Ang kanyang kwento ay lumaganap at nagbigay ng inspirasyon sa iba na " +
        "gumawa ng mabuti. Ang kanyang laruan ay naging simbolo ng pagkakaisa at pagmamahal sa kapwa.\n\n" +

        "Si Nico ay lumaki na may mas malawak na pang-unawa sa kahalagahan ng pagbibigay at pagbabahagi. Ang kanyang simpleng aksyon " +
        "noong bata pa siya ay nag-iwan ng malalim na epekto sa kanyang komunidad at sa kanyang sarili.",
        //Story 3
        "                       ANG MANGINGISDA\n\n" +
        "Si Mang Ben ay isang mangingisda sa isang maliit na bayan sa baybayin. Sa loob ng maraming taon, ang kanyang hanapbuhay ay " +
        "nakadepende sa yamang-dagat ng kanilang lugar. Ngunit sa paglipas ng panahon, napansin niyang unti-unting nababawasan ang " +
        "kanilang huli dahil sa polusyon sa dagat.\n\n" +

        "Habang tumatagal, naramdaman ni Mang Ben ang epekto ng pagkasira ng kalikasan hindi lamang sa kanilang hanapbuhay kundi sa kabuuang " +
        "komunidad. Nakita niya ang mga sirang lambat, patay na isda, at basura sa paligid ng kanilang baybayin. Napaisip siya kung paano " +
        "nila maibabalik ang sigla ng kanilang dagat.\n\n" +

        "Nagdesisyon si Mang Ben na kumilos. Nagsimula siyang mangolekta ng basura sa kanilang baybayin tuwing madaling araw bago pumalaot. " +
        "Sa simula, mag-isa lamang siya. Ngunit kalaunan, napansin siya ng kanyang mga kapitbahay at sinamahan siya sa kanyang layunin.\n\n" +

        "Nag-organisa sila ng malakihang coastal clean-up drive, kung saan ang mga residente ng kanilang lugar ay nagkaisa upang linisin ang " +
        "kanilang baybayin. Nagdala rin sila ng mga poster at impormasyon upang ipaliwanag sa komunidad ang kahalagahan ng pangangalaga " +
        "sa kalikasan.\n\n" +

        "Sa tulong ng mga NGO at lokal na opisyal, nabigyan sila ng pondo upang magpatupad ng mas sustainable na pamamaraan ng pangingisda. " +
        "Natutunan ni Mang Ben at ng kanyang mga kasama ang paggamit ng eco-friendly na mga lambat at ang kahalagahan ng marine sanctuaries.\n\n" +

        "Ang kanilang pagsisikap ay nagdala ng resulta. Unti-unting bumalik ang sigla ng kanilang dagat, at dumami muli ang kanilang huli. " +
        "Ang mga bata sa kanilang lugar ay naging bahagi rin ng kampanya sa pamamagitan ng pagtuturo sa paaralan tungkol sa kalikasan.\n\n" +

        "Sa kabila ng tagumpay, patuloy na naging mapagmatyag si Mang Ben. Alam niyang ang laban para sa kalikasan ay hindi natatapos, kaya " +
        "patuloy niyang isinulong ang edukasyon at pagkakaisa sa kanilang lugar. Naging inspirasyon siya hindi lamang sa kanyang bayan " +
        "kundi sa buong rehiyon.\n\n" +

        "Sa huli, si Mang Ben ay naging simbolo ng pagmamalasakit sa kalikasan. Ang kanyang simpleng pagsisimula ay nagdala ng malaking " +
        "pagbabago sa kanilang dagat at sa kanilang komunidad. Siya ay nagpapatunay na kahit sa maliit na paraan, maaaring makapag-ambag " +
        "sa mas magandang kinabukasan.\n\n" +

        "Ang kanyang kwento ay nagbigay inspirasyon sa marami na alagaan ang kalikasan at ipakita na ang pagkilos ay nagsisimula sa " +
        "simpleng hakbang. Si Mang Ben ay nanatiling mangingisda, ngunit ngayon ay may mas malawak na layunin: ang panatilihing buhay " +
        "ang dagat para sa susunod na henerasyon."
    };

        private List<Question> questions1 = new List<Question>
        {
            new Question { QuestionText = "Bakit mahalaga kay Lola Inda ang lupaing minana niya?", A = "Dahil ito ang kanyang kabuhayan", B = "Dahil ito ang simbolo ng kasaysayan at pamilya", C = "Dahil ito ang gusto ng kanyang pamilya", D = "Dahil ito ay may mataas na halaga", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging balak ng mga developer sa lupa ni Lola Inda?", A = "Gawing mall", B = "Gawing subdivision", C = "Gawing taniman", D = "Gawing parke", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging unang reaksyon ng pamilya ni Lola Inda?", A = "Nagpursige siyang huwag ibenta ang lupa", B = "Hinimok siyang ibenta ang lupa", C = "Pinayuhan siyang humingi ng tulong", D = "Sumuporta sa kanyang desisyon", CorrectAnswer = "B" },
            new Question { QuestionText = "Paano sinimulan ni Lola Inda ang kanyang laban?", A = "Humingi ng suporta sa kapitbahay at NGO", B = "Nagprotesta sa harap ng developers", C = "Nagsampa ng kaso sa korte", D = "Lumapit sa media agad-agad", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangunahing dahilan ng pagtutol ni Lola Inda sa pagbebenta ng lupa?", A = "Dahil hindi sapat ang pera", B = "Dahil gusto niyang ipamana ito sa susunod na henerasyon", C = "Dahil gusto niyang magtanim ng mas maraming pananim", D = "Dahil ayaw niyang maalis sa kanilang baryo", CorrectAnswer = "B" },
            new Question { QuestionText = "Anong mga hakbang ang ginawa ng komunidad upang suportahan si Lola Inda?", A = "Nagbigay ng legal na suporta", B = "Nag-organisa ng forum at mga aktibidad", C = "Nagprotesta sa harap ng gobyerno", D = "Nag-alok ng kanilang sariling lupa", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging epekto ng media sa laban ni Lola Inda?", A = "Napansin ng gobyerno ang kanilang sitwasyon", B = "Naging sikat si Lola Inda sa kanilang lugar", C = "Na-pressure ang mga developer na umalis", D = "Hindi naging epektibo ang media coverage", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging desisyon ng gobyerno sa lupa ni Lola Inda?", A = "Ipagbili ito sa developer", B = "Manatili ito sa mga magsasaka", C = "Gawing pampublikong lupa", D = "Gawing taniman ng gobyerno", CorrectAnswer = "B" },
            new Question { QuestionText = "Anong uri ng pagsasaka ang isinulong ni Lola Inda?", A = "Sustainable farming practices", B = "Modern mechanized farming", C = "Organic farming lamang", D = "Traditional farming practices", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang natutunan ni Lola Inda tungkol sa tunay na tagumpay?", A = "Ito ay sa pagkakaisa ng komunidad", B = "Ito ay sa pagtuturo ng modernong teknolohiya", C = "Ito ay sa pagkakaroon ng sariling negosyo", D = "Ito ay sa pagtanggap sa pagbabago", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang simbolo ng lupa para kay Lola Inda?", A = "Simbolo ng kayamanan", B = "Simbolo ng dignidad at kasaysayan", C = "Simbolo ng pagsasaka", D = "Simbolo ng modernisasyon", CorrectAnswer = "B" },
            new Question { QuestionText = "Paano isinulong ni Lola Inda ang edukasyon sa kabataan tungkol sa kalikasan?", A = "Nagturo ng sustainable farming sa kabataan", B = "Nagbigay ng mga scholarship", C = "Nagturo ng environmental laws", D = "Nag-organisa ng clean-up drives", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano naapektuhan ang pamilya ni Lola Inda sa kanyang laban?", A = "Naging mas malapit sila", B = "Nagkaroon ng sigalot sa kanilang pamilya", C = "Sumuporta ang buong pamilya", D = "Naghiwalay ang pamilya", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang ipinakita ni Lola Inda sa kanyang pakikibaka?", A = "Kakayahan ng maliliit na tao na ipaglaban ang kanilang karapatan", B = "Ang kahalagahan ng suporta ng gobyerno", C = "Ang lakas ng modernisasyon", D = "Ang kahalagahan ng pera sa laban", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano nagbigay inspirasyon si Lola Inda sa iba?", A = "Sa pamamagitan ng kanyang determinasyon", B = "Sa kanyang yaman at tagumpay", C = "Sa kanyang simpleng pamumuhay", D = "Sa kanyang pagiging masayahin", CorrectAnswer = "A" },
            new Question { QuestionText = "Anong kasanayan ang itinuro ni Lola Inda sa kabataan?", A = "Pag-aalaga ng hayop", B = "Pagtatanim at sustainable farming", C = "Pag-aayos ng mga makinarya", D = "Pagsulat ng panukala sa gobyerno", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang ipinakita ng laban ni Lola Inda sa mga developers?", A = "Ang kahalagahan ng pagkakaisa", B = "Ang kahalagahan ng pera sa laban", C = "Ang kahalagahan ng modernisasyon", D = "Ang kahalagahan ng negosyo", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging epekto ng laban ni Lola Inda sa komunidad?", A = "Naging mas malapit sila sa kalikasan", B = "Naging malaya sila sa modernisasyon", C = "Naging mas maayos ang kanilang pamayanan", D = "Naging inspirasyon sila sa iba pang baryo", CorrectAnswer = "D" },
            new Question { QuestionText = "Paano ipinakita ni Lola Inda ang pagmamahal sa kanyang pinagmulan?", A = "Sa pagtuturo ng kasaysayan ng kanilang lupa", B = "Sa pagpapamana ng lupa sa pamilya", C = "Sa paglaban para sa kanilang karapatan sa lupa", D = "Sa pagtanggi sa mga modernong ideya", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang mensahe ng kwento ni Lola Inda?", A = "Ang yaman ay nasa lupa at kasaysayan, hindi lamang sa pera", B = "Ang pagbabago ay hindi maiiwasan", C = "Ang lupa ay dapat laging ipagbili kung may malaking halaga", D = "Ang gobyerno ay dapat palaging nakikialam", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangunahing pinagkukunan ng kabuhayan ni Lola Inda?", A = "Pagtitinda sa palengke", B = "Pagsasaka", C = "Pangingisda", D = "Pagtatrabaho sa lungsod", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naramdaman ni Lola Inda nang malaman niyang may gustong bumili ng kanyang lupa?", A = "Natuwa", B = "Nalungkot", C = "Walang pakialam", D = "Nagdalawang-isip", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang dahilan kung bakit gusto ng kanyang pamilya na ibenta ang lupa?", A = "Dahil mahirap nang magtanim", B = "Dahil gusto nilang lumipat sa lungsod", C = "Dahil malaki ang halaga ng alok", D = "Dahil may utang sila", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang kahulugan ng lupa para kay Lola Inda?", A = "Isang ari-arian lamang", B = "Isang yaman ng kasaysayan", C = "Isang pansamantalang kabuhayan", D = "Isang hadlang sa pag-unlad", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging unang hakbang ni Lola Inda upang ipaglaban ang kanyang lupa?", A = "Naghanap ng abogado", B = "Humingi ng suporta sa kapitbahay", C = "Nagsampa ng reklamo sa barangay", D = "Nagpa-interview sa media agad-agad", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang ginamit nilang paraan upang maipakita ang kanilang karapatan sa lupa?", A = "Pagsasagawa ng forum", B = "Pagtatanim ng maraming gulay", C = "Pagpapadala ng sulat sa presidente", D = "Pagkakampanya sa social media", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangunahing layunin ng mga NGO na sumuporta kay Lola Inda?", A = "Upang tulungan siyang ibenta ang lupa", B = "Upang protektahan ang karapatan ng mga magsasaka", C = "Upang bigyan siya ng trabaho sa lungsod", D = "Upang hikayatin siyang lumipat sa ibang bayan", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging epekto ng hindi pagsang-ayon ng pamilya ni Lola Inda sa kanyang desisyon?", A = "Nagkaroon ng hidwaan sa pamilya", B = "Sumuporta pa rin sila sa kanya", C = "Lumipat ang kanyang mga anak sa ibang lugar", D = "Nagbigay sila ng alternatibong solusyon", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging papel ng media sa laban ni Lola Inda?", A = "Ipinalabas ang kanyang kwento sa publiko", B = "Nagbigay ng pera para hindi na ibenta ang lupa", C = "Nagpakalat ng maling impormasyon", D = "Walang naging epekto", CorrectAnswer = "A" },
            new Question { QuestionText = "Bakit nagdesisyon ang gobyerno na panatilihin ang lupa sa mga magsasaka?", A = "Dahil sa lumalakas na suporta ng publiko", B = "Dahil mas mahalaga ang agrikultura kaysa negosyo", C = "Dahil natakot sila sa protesta", D = "Dahil inutusan sila ng presidente", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang kompromisong naganap upang mapanatili ng mga magsasaka ang kanilang lupa?", A = "Pagtanggap ng tulong mula sa gobyerno", B = "Pagsusulong ng sustainable farming", C = "Pagsuko ng kalahati ng lupa sa developers", D = "Pagpapabago sa batas ukol sa lupa", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang pangunahing aral na matutunan mula kay Lola Inda?", A = "Ang tunay na yaman ay nasa lupa at kasaysayan", B = "Mas mabuting ibenta ang lupa para sa kinabukasan", C = "Ang pera ay mas mahalaga kaysa prinsipyo", D = "Mahirap labanan ang mga mayayaman", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging reaksyon ng komunidad matapos magtagumpay si Lola Inda?", A = "Naging inspirasyon sa iba pang magsasaka", B = "Nagsimula silang magbenta ng lupa", C = "Lumipat sila sa ibang bayan", D = "Humingi sila ng mas malaking bayad", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isa pang naging epekto ng tagumpay ni Lola Inda?", A = "Mas maraming tao ang nagpahalaga sa sustainable farming", B = "Mas maraming lupa ang nabili ng mga developers", C = "Nawala ang suporta ng gobyerno", D = "Lumipat ang kanyang pamilya sa ibang bansa", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang itinuro ni Lola Inda sa mga kabataan sa kanilang lugar?", A = "Pagpapahalaga sa kalikasan at agrikultura", B = "Pagnenegosyo ng lupa", C = "Paglipat sa siyudad para sa mas magandang oportunidad", D = "Pag-iinvest sa real estate", CorrectAnswer = "A" },
            new Question { QuestionText = "Bakit naging mas mahirap para kay Lola Inda na ipaglaban ang lupa?", A = "Dahil sa hindi pagsuporta ng kanyang pamilya", B = "Dahil wala siyang sapat na pera", C = "Dahil sa takot sa gobyerno", D = "Dahil sa kawalan ng interes ng mga tao", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isa sa mga paraan na ginamit ng gobyerno upang suportahan ang magsasaka?", A = "Pagbibigay ng edukasyon tungkol sa sustainable farming", B = "Pagbili ng kanilang lupa", C = "Pagpapahintulot sa developers", D = "Pagbabawas ng kanilang buwis", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang nagbigay inspirasyon kay Lola Inda na ipaglaban ang kanyang lupa?", A = "Ang kanyang pagmamahal sa kasaysayan at kalikasan", B = "Ang pagnanais niyang yumaman", C = "Ang pangarap niyang magtayo ng negosyo", D = "Ang kanyang takot na mawalan ng trabaho", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano naapektuhan ng laban ni Lola Inda ang ibang magsasaka?", A = "Nabigyan sila ng lakas ng loob upang ipaglaban ang kanilang karapatan", B = "Napilitan silang ibenta ang kanilang lupa", C = "Umalis sila sa kanilang baryo", D = "Sumuporta sila sa developers", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang nais iparating ng kwento ni Lola Inda?", A = "Ang kahalagahan ng prinsipyo, pagkakaisa, at pagmamahal sa lupa", B = "Ang kahalagahan ng pagbebenta ng lupa sa tamang halaga", C = "Ang pagiging bukas sa modernisasyon", D = "Ang pagsunod sa kagustuhan ng mayayaman", CorrectAnswer = "A" }
        };

        private List<Question> questions2 = new List<Question>
        {
            new Question { QuestionText = "Ano ang natanggap ni Nico noong Pasko?", A = "Bagong damit", B = "Makulay na kotse na may remote control", C = "Bagong sapatos", D = "Isang malaking stuffed toy", CorrectAnswer = "B" },
            new Question { QuestionText = "Bakit espesyal ang laruan ni Nico?", A = "Dahil ito ay mamahalin", B = "Dahil ito ang matagal na niyang hiniling", C = "Dahil ito ay ibinigay ng kapitbahay", D = "Dahil ito ay kakaiba", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang nakita ni Nico habang naglalaro sa labas?", A = "Mga batang masaya sa kanilang mga bagong laruan", B = "Mga batang naglalaro ng sirang laruan", C = "Mga batang nag-aaway", D = "Mga batang walang interes sa kanyang laruan", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang ginawa ni Nico nang mapansin ang mga batang tumititig sa kanyang laruan?", A = "Nagtago siya ng laruan", B = "Inalok niya silang maglaro kasama siya", C = "Ipinaalala niyang kanya lang iyon", D = "Pinasara niya ang kanyang bakuran", CorrectAnswer = "B" },
            new Question { QuestionText = "Paano nag-umpisa ang pagbabahagi ni Nico ng kanyang laruan?", A = "Pinautang niya ang kanyang laruan sa mga bata", B = "Inimbitahan niyang maglaro ang mga bata sa kanilang bakuran", C = "Ipinadala niya ang laruan sa barangay", D = "Ibinenta niya ang laruan upang bumili ng marami pang laruan", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging reaksyon ng mga bata noong una?", A = "Nahihiya silang lumapit", B = "Agad nilang hinawakan ang laruan", C = "Nag-alok sila ng palitan para sa laruan", D = "Naging agresibo sila", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano naging inspirasyon si Nico sa kanyang barangay?", A = "Sa pamamagitan ng pagdala ng laruan sa barangay", B = "Sa pamamagitan ng pagbebenta ng laruan", C = "Sa pag-aalok ng libreng pagkain", D = "Sa paggawa ng bagong laruan", CorrectAnswer = "A" },
            new Question { QuestionText = "Anong kampanya ang sinimulan ni Nico at ng kanyang pamilya?", A = "Pagtitipon ng lumang laruan para sa mga bata", B = "Pagbebenta ng mga bagong laruan", C = "Pagbibigay ng pera sa mahihirap na bata", D = "Paggawa ng bagong proyekto para sa mga kabataan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang layunin ng donation drive ni Nico?", A = "Upang makapagbigay ng mas maraming laruan sa mga bata", B = "Upang kumita para sa kanilang pamilya", C = "Upang mag-organisa ng malalaking laro", D = "Upang makilala ng barangay ang kanilang pamilya", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang natutunan ni Nico mula sa kanyang pagbabahagi?", A = "Ang halaga ng pagkakaisa at pagkakaintindihan", B = "Ang halaga ng pagkakaroon ng maraming laruan", C = "Ang halaga ng pagiging popular", D = "Ang halaga ng pagtitipid ng laruan", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano nasolusyunan ni Nico ang mga hindi pagkakaintindihan ng mga bata?", A = "Sa pamamagitan ng pag-organisa ng palaro", B = "Sa pagtuturo ng pagbabahagi at pagkakaisa", C = "Sa pagbili ng mas maraming laruan", D = "Sa pagtatago ng kanyang laruan", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging epekto ng pagbabahagi ni Nico sa komunidad?", A = "Nabawasan ang away sa mga bata", B = "Naging inspirasyon siya sa iba na tumulong", C = "Naging sikat siya sa barangay", D = "Nagkaroon ng maraming laruan ang lahat", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging simbolo ng laruan ni Nico?", A = "Simbolo ng kayamanan", B = "Simbolo ng pagmamahal sa kapwa", C = "Simbolo ng kasiyahan sa laruan", D = "Simbolo ng magandang Pasko", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang itinuro ng kwento ni Nico?", A = "Ang kahalagahan ng pagbabahagi at pagtutulungan", B = "Ang kahalagahan ng pagkakaroon ng laruan", C = "Ang kahalagahan ng pagbibigay ng regalo", D = "Ang kahalagahan ng pagkakaroon ng Pasko", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano nakatulong ang donation drive ni Nico?", A = "Nakabili ito ng mas mahal na laruan para sa kanya", B = "Nakagawa ito ng bagong grupo ng mga bata", C = "Napakaraming bata ang natulungan sa kanilang lugar", D = "Nagbigay ito ng ideya para sa negosyo", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang natutunan ng mga bata mula kay Nico?", A = "Pagpapahalaga sa mga materyal na bagay", B = "Pagpapahalaga sa pagkakaisa at pagbabahagi", C = "Pagpapahalaga sa laruan", D = "Pagpapahalaga sa mas magagandang regalo", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang pangunahing mensahe ng kwento ni Nico?", A = "Ang kasiyahan ay mas nararamdaman kapag ibinabahagi", B = "Mas masaya kapag maraming laruan", C = "Ang pagbibigay ay para lamang sa Pasko", D = "Ang bawat bata ay dapat magkaroon ng laruan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging inspirasyon ni Nico para magbigay?", A = "Ang kasiyahan ng iba sa kanyang laruan", B = "Ang kagustuhan niyang maging sikat", C = "Ang kagustuhan niyang magkaroon ng negosyo", D = "Ang kagustuhan niyang magkapera", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano ipinakita ni Nico ang pagiging mapagbigay?", A = "Pag-aalay ng kanyang laruan sa ibang bata", B = "Pag-aalok ng mga libreng pagkain", C = "Pagbili ng bagong laruan para sa iba", D = "Pagbibigay ng pera sa barangay", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang simbolo ng kwento ni Nico para sa komunidad?", A = "Pagkakaisa at malasakit sa kapwa", B = "Pagpapahalaga sa mga materyal na bagay", C = "Pagpapalaganap ng negosyo", D = "Pag-iingat sa mga laruan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naramdaman ni Nico nang matanggap niya ang kanyang laruan?", A = "Lungkot", B = "Saya", C = "Pagkagulat", D = "Panghihinayang", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang napansin ni Nico sa mga kapitbahay niyang bata?", A = "Mas magaganda ang kanilang laruan", B = "Wala silang laruan o may sirang laruan", C = "Hindi sila mahilig maglaro", D = "Ayaw nilang makipaglaro sa kanya", CorrectAnswer = "B" },
            new Question { QuestionText = "Bakit nahihiya ang ibang bata noong una?", A = "Dahil ayaw nilang maglaro", B = "Dahil hindi nila alam paano gamitin ang laruan", C = "Dahil takot silang masira ito", D = "Dahil hindi nila gusto si Nico", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang ginawa ni Nico para mahikayat ang mga bata na makilaro?", A = "Nagbigay siya ng kendi", B = "Pinilit niya silang lumapit", C = "Matiyagang inanyayahan silang maglaro", D = "Itinago niya ang laruan", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang naging epekto ng laruan ni Nico sa kanyang mga kapitbahay?", A = "Nagkaroon sila ng bagong pag-asa", B = "Nagkaroon sila ng away", C = "Nagkaroon ng inggitan", D = "Wala silang interes dito", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang sumunod na ginawa ni Nico pagkatapos niyang ipalaro ang laruan sa kanyang kapitbahay?", A = "Dinala niya ito sa barangay upang ipalaro sa mas maraming bata", B = "Itinago niya ito sa loob ng bahay", C = "Ibinenta niya ang laruan", D = "Sinira niya ang laruan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging reaksyon ng mga bata sa barangay nang dalhin ni Nico ang kanyang laruan?", A = "Hindi sila interesado", B = "Masaya silang nakisali sa laro", C = "Nag-aaway sila dahil gusto nilang kunin ang laruan", D = "Naglaro sila pero hindi natuwa", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging inspirasyon ni Nico upang simulan ang donation drive?", A = "Ang saya ng mga batang nakalaro niya", B = "Ang hiling ng kanyang mga magulang", C = "Ang utos ng barangay", D = "Ang pangarap niyang magnegosyo", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ginawa ng mga tao sa kanilang komunidad nang marinig ang donation drive?", A = "Nagbigay sila ng mga lumang laruan", B = "Nagtipon sila para mag-party", C = "Nagbigay sila ng pera kay Nico", D = "Hindi sila tumulong", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangunahing aral na natutunan ni Nico mula sa kanyang karanasan?", A = "Mas masaya ang magbahagi kaysa magtago", B = "Mas mabuting itago ang mga laruan upang hindi masira", C = "Hindi lahat ng bata ay mahilig maglaro", D = "Mas mabuting hindi ipahiram ang laruan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging suliranin sa pagbabahagi ng laruan ni Nico?", A = "May mga batang nag-aagawan sa laruan", B = "Walang gustong maglaro", C = "Nasira agad ang laruan", D = "Hindi na niya nagustuhan ang laruan", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano nasolusyunan ni Nico ang mga hindi pagkakaunawaan sa pagitan ng mga bata?", A = "Sa pamamagitan ng patas na pagpapahiram ng laruan", B = "Sa pamamagitan ng pagsigaw sa kanila", C = "Sa pamamagitan ng hindi na pagbibigay ng laruan", D = "Sa pamamagitan ng pagbili ng bagong laruan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naramdaman ni Nico nang makita ang tuwa ng mga bata?", A = "Saya at kasiyahan", B = "Inggit", C = "Pagkabagot", D = "Takot", CorrectAnswer = "A" },
            new Question { QuestionText = "Bakit patuloy na lumaganap ang kwento ni Nico?", A = "Dahil sa inspirasyong hatid ng kanyang kabutihang-loob", B = "Dahil sa kanyang kasikatan", C = "Dahil sa kanyang bagong laruan", D = "Dahil sa utos ng kanyang magulang", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang nagpatunay na hindi lang tungkol sa laruan ang kwento ni Nico?", A = "Dahil natutunan niya ang halaga ng pagbabahagi at pagtutulungan", B = "Dahil hindi siya bumili ng bagong laruan", C = "Dahil nagalit siya sa ibang bata", D = "Dahil itinago niya ang laruan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging epekto ng kabutihang-loob ni Nico sa ibang tao?", A = "Naging inspirasyon sila na tumulong", B = "Nagkaroon ng inggitan", C = "Hindi nila pinansin ang kanyang ginawa", D = "Nagkaroon ng away sa komunidad", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang nais iparating ng kwento sa mambabasa?", A = "Ang tunay na kasiyahan ay nasa pagbibigay at hindi sa pag-aari", B = "Mas mahalaga ang magkaroon ng maraming laruan", C = "Mas mabuti ang hindi magpahiram ng laruan", D = "Mas magandang magtago ng laruan upang hindi masira", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang nagbago sa pananaw ni Nico tungkol sa laruan?", A = "Hindi ito kasing halaga ng pagbabahagi ng kasiyahan", B = "Mas gusto niyang mag-ipon ng mas maraming laruan", C = "Mas gusto niyang maglaro mag-isa", D = "Nais niyang bumili ng mas mahal na laruan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangunahing naging resulta ng ginawang donation drive?", A = "Mas maraming bata ang nagkaroon ng laruan", B = "Nawala ang kanyang laruan", C = "Naging mas sikat si Nico", D = "Nagkaroon ng away sa barangay", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pinakamalaking aral na maaaring matutunan mula sa kwento?", A = "Ang pagmamahal sa kapwa ay mas mahalaga kaysa materyal na bagay", B = "Mas mainam na magtago ng laruan", C = "Dapat laging bumili ng bagong laruan", D = "Mas mabuting hindi makipaglaro sa iba", CorrectAnswer = "A" }
        };

        private List<Question> questions3 = new List<Question>
        {
            new Question { QuestionText = "Ano ang pangunahing ikinabubuhay ni Mang Ben?", A = "Pagtatanim", B = "Pangingisda", C = "Pagbibilad ng asin", D = "Paggawa ng bangka", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang problema sa dagat na ikinalungkot ni Mang Ben?", A = "Kakulangan ng isda", B = "Polusyon sa dagat", C = "Pag-aagawan ng teritoryo", D = "Bagyong paparating", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging reaksyon ng ibang mangingisda tungkol sa sitwasyon ng dagat?", A = "Tumigil sila sa pangingisda", B = "Patuloy nilang inabuso ang dagat", C = "Nagtulungan sila upang linisin ang dagat", D = "Lumipat sila sa ibang lugar", CorrectAnswer = "B" },
            new Question { QuestionText = "Anong ideya ang naisip ni Mang Ben upang masolusyunan ang problema?", A = "Mag-organisa ng coastal cleanup", B = "Mag-imbita ng tulong mula sa gobyerno", C = "Lumipat sa ibang hanapbuhay", D = "Ituro ang sustainable fishing sa komunidad", CorrectAnswer = "D" },
            new Question { QuestionText = "Ano ang una niyang ginawa upang simulan ang solusyon?", A = "Lumapit sa lokal na pamahalaan", B = "Nagtayo ng environmental group", C = "Nagdaos ng meeting sa mga kapwa mangingisda", D = "Nagtipon ng pondo para sa proyekto", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang naging reaksyon ng iba pang mangingisda sa panukala ni Mang Ben?", A = "Sumuporta agad sila", B = "Nagkaroon ng pag-aalinlangan", C = "Tumanggi silang makiisa", D = "Nagkaroon ng sigalot sa grupo", CorrectAnswer = "B" },
            new Question { QuestionText = "Paano nakumbinsi ni Mang Ben ang komunidad na makiisa sa proyekto?", A = "Sa pamamagitan ng mga seminar at workshops", B = "Sa pagbibigay ng mga libreng kagamitan", C = "Sa panghihikayat gamit ang halimbawa ng ibang baryo", D = "Sa pagbigay ng insentibo mula sa gobyerno", CorrectAnswer = "A" },
            new Question { QuestionText = "Anong prinsipyo ang itinuro ni Mang Ben tungkol sa pangingisda?", A = "Paggamit ng modernong teknolohiya", B = "Pagpapatigil ng pangingisda tuwing taglamig", C = "Pag-iwas sa overfishing at pagpapahalaga sa ekosistema", D = "Pagpapalago ng negosyo sa pangingisda", CorrectAnswer = "C" },
            new Question { QuestionText = "Anong hakbang ang ginawa ng grupo upang linisin ang dagat?", A = "Nagsagawa ng coastal clean-up drive", B = "Naglagay ng mga lambat sa paligid ng dagat", C = "Nagtanim ng bakawan", D = "Naglagay ng warning signs sa dagat", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ginawa ng gobyerno upang suportahan ang inisyatibo?", A = "Nagbigay ng bagong kagamitan", B = "Nagbigay ng pondo at edukasyon tungkol sa kalikasan", C = "Naglagay ng batas laban sa pangingisda", D = "Nagbukas ng trabaho sa ibang sektor", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang simbolo ng dagat para kay Mang Ben?", A = "Simbolo ng kayamanan", B = "Simbolo ng buhay at likas na yaman", C = "Simbolo ng kasaysayan", D = "Simbolo ng trabaho at pagkakakitaan", CorrectAnswer = "B" },
            new Question { QuestionText = "Paano nagbago ang komunidad dahil sa proyekto ni Mang Ben?", A = "Nabawasan ang hanapbuhay sa pangingisda", B = "Mas naging maayos ang kanilang relasyon sa dagat", C = "Nagkaroon ng sigalot sa pagitan ng mga grupo", D = "Tumaas ang demand para sa seafood", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang naging epekto ng pagtatanim ng bakawan?", A = "Mas maraming isda ang dumami sa kanilang lugar", B = "Nagkaroon ng bagong lugar para sa negosyo", C = "Nabawasan ang bagyo sa kanilang lugar", D = "Nawala ang polusyon sa tubig", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang natutunan ng mga tao mula sa inisyatibo ni Mang Ben?", A = "Ang kahalagahan ng pangangalaga sa kalikasan", B = "Ang kahalagahan ng paglipat ng hanapbuhay", C = "Ang kahalagahan ng pagsunod sa batas", D = "Ang kahalagahan ng pag-aaral ng negosyo", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang simbolo ng tagumpay ng proyekto ni Mang Ben?", A = "Pagdami ng huli ng isda", B = "Paglilinis ng baybayin", C = "Pagkakaisa ng komunidad", D = "Pagiging tanyag ng kanilang baryo", CorrectAnswer = "C" },
            new Question { QuestionText = "Anong aral ang itinuro ng kwento ni Mang Ben?", A = "Pagpapanatili ng likas-yaman ay responsibilidad ng lahat", B = "Ang kayamanan ay nagmumula sa negosyo", C = "Mahalaga ang tulong ng gobyerno sa lahat ng oras", D = "Ang teknolohiya ay susi sa tagumpay", CorrectAnswer = "A" },
            new Question { QuestionText = "Paano nagbigay inspirasyon si Mang Ben sa ibang baryo?", A = "Sa paggawa ng documentary tungkol sa kanyang proyekto", B = "Sa pagsasalita sa mga seminar sa ibang lugar", C = "Sa pag-imbita ng mga tao mula sa ibang lugar", D = "Sa pagpapakita ng tagumpay ng kanilang baryo", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang ipinakita ni Mang Ben sa kanyang laban para sa dagat?", A = "Ang pagkakaroon ng tamang kaalaman ay mahalaga", B = "Ang pagtutulungan ay mahalaga sa tagumpay", C = "Ang kalikasan ay dapat pangalagaan sa kahit anong paraan", D = "Lahat ng sagot ay tama", CorrectAnswer = "D" },
            new Question { QuestionText = "Ano ang epekto ng proyekto ni Mang Ben sa kalikasan?", A = "Tumaas ang biodiversity sa lugar", B = "Nawala ang polusyon", C = "Naging malinis ang tubig sa lahat ng parte", D = "Tumaas ang kita ng mga mangingisda", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang mensahe ng kwento ni Mang Ben?", A = "Ang tao at kalikasan ay dapat magkasama para sa tagumpay", B = "Ang kalikasan ay mahalaga kaysa sa lahat", C = "Ang kita ay mahalaga kaysa sa pangangalaga", D = "Ang tao ay dapat laging nakadepende sa gobyerno", CorrectAnswer = "A" },
            new Question { QuestionText = "Bakit nabawasan ang huli ni Mang Ben sa paglipas ng panahon?", A = "Dahil sa pagbabago ng klima", B = "Dahil sa polusyon sa dagat", C = "Dahil sa paglipat ng mga isda", D = "Dahil sa pagdami ng mangingisda", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang ginawa ni Mang Ben tuwing madaling araw bago pumalaot?", A = "Nagpapahinga sa bahay", B = "Nanghuli ng isda", C = "Nangolekta ng basura", D = "Nagturo ng pangingisda", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang unang napansin ni Mang Ben sa kanilang baybayin?", A = "Dumaraming isda", B = "Malinis na tubig", C = "Sirang lambat at patay na isda", D = "Mataas na alon", CorrectAnswer = "C" },
            new Question { QuestionText = "Ano ang naging reaksyon ng kanyang kapitbahay sa una niyang ginawa?", A = "Walang pakialam", B = "Sumama sa kanya", C = "Pinagtawanan siya", D = "Hinirapan siya", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang layunin ng coastal clean-up drive na inorganisa nila?", A = "Upang gawing tourist spot ang baybayin", B = "Upang mapanatili ang kalinisan ng dagat", C = "Upang itigil ang pangingisda", D = "Upang makahuli ng mas maraming isda", CorrectAnswer = "B" },
            new Question { QuestionText = "Ano ang ginawa nila upang ipaalam ang kahalagahan ng pangangalaga sa kalikasan?", A = "Nagbigay ng poster at impormasyon", B = "Nagbigay ng libreng kagamitan", C = "Nagbigay ng libreng isda", D = "Nagsagawa ng rally", CorrectAnswer = "A" },
            new Question { QuestionText = "Anong institusyon ang tumulong sa kanila upang magkaroon ng pondo?", A = "Mga NGO at lokal na opisyal", B = "Mga pribadong kumpanya", C = "Mga paaralan", D = "Ibang bayan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isa sa mga natutunan nila upang mapanatili ang kalikasan?", A = "Paggamit ng eco-friendly na lambat", B = "Pagpaparami ng barko sa dagat", C = "Pagpaparami ng tao sa pangingisda", D = "Paggamit ng mas malalaking lambat", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isa sa mga resulta ng kanilang pagsisikap?", A = "Unti-unting bumalik ang sigla ng dagat", B = "Nawala ang hanapbuhay ng mangingisda", C = "Lumipat sila sa ibang bayan", D = "Nagkaroon ng bagong batas laban sa pangingisda", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ginawa ng mga bata sa kanilang lugar upang tumulong?", A = "Naging bahagi ng kampanya sa pamamagitan ng pagtuturo sa paaralan", B = "Sumama sa pangingisda", C = "Nag-organisa ng pagdiriwang", D = "Nagbigay ng pondo sa proyekto", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pananaw ni Mang Ben tungkol sa laban para sa kalikasan?", A = "Ito ay isang patuloy na laban", B = "Matatapos ito kapag malinis na ang dagat", C = "Hindi ito mahalaga", D = "Ito ay trabaho lang ng gobyerno", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging epekto ng proyektong ito sa buong rehiyon?", A = "Naging inspirasyon ito sa iba pang bayan", B = "Nabawasan ang bilang ng mangingisda", C = "Nawala ang industriya ng pangingisda", D = "Walang epekto sa ibang bayan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ipinapakita ng kwento ni Mang Ben tungkol sa pagbabago?", A = "Magsisimula ito sa simpleng hakbang", B = "Kailangan ito ng malaking puhunan", C = "Hindi ito posible", D = "Nakadepende ito sa gobyerno", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging mas malawak na layunin ni Mang Ben sa bandang huli?", A = "Panatilihing buhay ang dagat para sa susunod na henerasyon", B = "Palawakin ang kanyang negosyo", C = "Lumipat sa ibang trabaho", D = "Gawing tourist spot ang kanilang lugar", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ipinakita ni Mang Ben sa kanyang pagsisikap?", A = "Ang maliit na pagkilos ay maaaring magdulot ng malaking pagbabago", B = "Ang pagbabago ay imposible", C = "Ang pangingisda ay dapat itigil", D = "Dapat palaging umasa sa tulong ng gobyerno", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang isang solusyon na itinuro ng mga NGO sa kanila?", A = "Pagpapatupad ng marine sanctuaries", B = "Pagpaparami ng barko", C = "Pagbebenta ng lupa sa dayuhan", D = "Pag-aangkat ng isda mula sa ibang bayan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang pangunahing dahilan kung bakit sumama ang ibang residente sa coastal clean-up?", A = "Nakita nila ang dedikasyon ni Mang Ben", B = "Pinilit sila ng gobyerno", C = "May pera kapalit ng kanilang pagsali", D = "Ginawa ito bilang isang proyekto sa paaralan", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang ipinapahiwatig ng kwento tungkol sa pagkakaisa?", A = "Ang sama-samang pagkilos ay may positibong epekto", B = "Hindi ito mahalaga", C = "Mahirap itong makamit", D = "Mas maganda ang pagtatrabaho nang mag-isa", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang naging epekto ng paggamit ng eco-friendly na lambat?", A = "Dumami ang kanilang huli nang hindi sinisira ang dagat", B = "Tumaas ang presyo ng isda", C = "Mas mahirap nang mangisda", D = "Nabawasan ang mga isda", CorrectAnswer = "A" },
            new Question { QuestionText = "Ano ang maaaring gawin ng iba pang bayan batay sa karanasan ni Mang Ben?", A = "Magsimula ng sariling inisyatibo upang protektahan ang kalikasan", B = "Umasa sa tulong ng gobyerno", C = "Itigil ang pangingisda", D = "Huwag pansinin ang problema", CorrectAnswer = "A" }

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

        private void CharacterAnalysisFilHard_Load(object sender, EventArgs e)
        {

        }
    }
}