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
using DCP.Resources;

namespace DCP
{
    public partial class StoryRetellingHard : Form
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
        private int grade = 0;
        public StoryRetellingHard()
        {
            InitializeComponent();

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
            EssayrichTextBox2.Enabled = false;
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
                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, grade);

                MessageBox.Show("Challenge not completed. Returning to the homepage.", "Challenge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TransitionToHomePage();
            }
        }
        public void SaveChallengeDataSuccess(string username, string time, int grade)
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
                FormTitle = "Story Retelling (Hard)",
                Grade = $"Completed {grade}", // New property for jogging
                Time = time
            };

            challengeList.Add(newChallengeData);

            // Save back to the JSON file
            var updatedChallengeData = JsonConvert.SerializeObject(challengeList, Formatting.Indented);
            File.WriteAllText(challengeFilePath, updatedChallengeData);
        }
        public void SaveChallengeDataFailed(string username, string time, int grade)
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
                FormTitle = "Story Retelling (Hard)",
                Grade = $"Failed {grade}", // New property for jogging
                Time = time
            };

            challengeList.Add(newChallengeData);

            // Save back to the JSON file
            var updatedChallengeData = JsonConvert.SerializeObject(challengeList, Formatting.Indented);
            File.WriteAllText(challengeFilePath, updatedChallengeData);
        }
        private void SaveEssayContent(string essayText)
        {
            DialogResult result = MessageBox.Show("Do you want to save your retell story", "Save File", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Prompt for the title of the story
                string storyTitle = Microsoft.VisualBasic.Interaction.InputBox("Enter the title of the story:", "Story Title", "Default Title");

                // Initialize SaveFileDialog
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt", // Set file type filter
                };

                // Show the SaveFileDialog
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    // Check if the file already exists
                    int fileCount = 1;
                    while (File.Exists(filePath))
                    {
                        // If the file exists, create a new name with an incremented number
                        filePath = Path.Combine(Path.GetDirectoryName(filePath),
                            Path.GetFileNameWithoutExtension(filePath) + $" ({fileCount})" + Path.GetExtension(filePath));
                        fileCount++;
                    }

                    // Combine the title with the essay text
                    string contentToSave = $"Title: {storyTitle}\n\n{essayText}";

                    // Save the essay content with title to the file
                    File.WriteAllText(filePath, contentToSave);
                    MessageBox.Show($"Your book summary has been saved as a text file: {filePath}", "File Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
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
            button1.Enabled = true;
            textBox2.Enabled = true;
            pictureBox11.Enabled = true;
            button4.Enabled = true;
            isEnterKeyDisabled = false;
            EssayrichTextBox2.Enabled = true;

            // Initialize progress bar and timer
            progressBar1.Value = 0;
            progressBar1.Maximum = progressDuration - 1; // Adjusted to fill fully at end
            timeRemaining = progressDuration;

            // Start timer and update time textbox
            DisplayRandomContent(); // Call the method to display a random story
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
                                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, grade);

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
        private void DisplayRandomContent()
        {
            Random random = new Random();
            currentIndex = random.Next(0, contentList.Count); // Generate a random index
            DisplayContent(currentIndex); // Display the selected story in EssayrichTextBox1
        }
        private void DisplayContent(int index)
        {
            if (index >= 0 && index < contentList.Count)
            {
                EssayrichTextBox1.Text = contentList[index];
            }
        }
        private List<string> contentList;
        private int currentIndex = 0;

        private void InitializeContent()
        {
            contentList = new List<string>
        {
        // Noli Me Tangere Story  
        "                       ANG HINAGPIS NG KABAYANAN\n\n" +

        "Sa bayan ng San Diego, si Crisostomo Ibarra, isang binatang nag-aral sa Europa, ay bumalik upang ipagpatuloy " +
        "ang nasimulan ng kanyang ama. Ang kanyang layunin ay buuin ang isang paaralan upang mabigyan ng magandang " +
        "kinabukasan ang mga kabataan sa kanilang lugar. Subalit, hindi naging madali ang kanyang misyon dahil sa mga " +
        "hadlang na dala ng katiwalian at kasakiman ng mga nasa kapangyarihan.\n\n" +

        "Sa kanyang pagbabalik, muling nagtagpo ang landas nila ni Maria Clara, ang kanyang kababata at kasintahan. " +
        "Sa kabila ng pagmamahalan, nalaman nilang maraming lihim ang nag-uugnay sa kanilang mga pamilya, " +
        "at ang mga lihim na ito ang naging dahilan ng kanilang pagsubok. Isa na rito ang katotohanan tungkol sa " +
        "pagkamatay ng ama ni Ibarra na si Don Rafael, na ginawang biktima ng hindi makatarungang batas.\n\n" +

        "Sa gitna ng kanyang layunin, hinarap ni Ibarra si Padre Damaso, isang paring may malaking impluwensya sa bayan. " +
        "Pinilit niyang ilantad ang katiwalian nito, ngunit nagdulot lamang ito ng higit pang galit at tensyon. " +
        "Sa isang pagtitipon, naging sanhi ng away ang kanilang alitan, dahilan upang si Ibarra ay ituring na kaaway " +
        "ng Simbahan at ng estado.\n\n" +

        "Habang tumatagal, nadagdagan ang mga problema ni Ibarra. Ang mga kasamahang inaasahan niyang susuporta sa kanya " +
        "ay napilitang umatras dahil sa takot sa mga makapangyarihan. Sa kabila nito, patuloy niyang ipinaglaban ang " +
        "karapatan ng kanyang mga kababayan, lalo na ang mga mahihirap at naaapi.\n\n" +

        "Naging malapit si Ibarra kay Pilosopo Tasyo, isang matandang pantas na nagbigay sa kanya ng inspirasyon " +
        "at lakas ng loob. Subalit, sa kabila ng mga payo, napagtanto niyang hindi sapat ang pagiging makatao " +
        "kung ang kalaban ay sistemang likas na mapanlinlang.\n\n" +

        "Nang mapagbintangan si Ibarra ng paghihimagsik, napilitan siyang tumakas. Sa tulong ni Elias, isang misteryosong " +
        "binata na nagmula sa mas mababang antas ng lipunan, nakaligtas si Ibarra sa tiyak na kamatayan. " +
        "Nagpatuloy ang kanilang laban sa katarungan, kahit na naging mahirap ito dahil sa kakulangan ng suporta.\n\n" +

        "Sa paglipas ng panahon, unti-unting nawala ang pag-asa ni Ibarra na makamit ang kanyang mga pangarap. " +
        "Ang mga planong kanyang inihanda ay nauwi sa kabiguan dahil sa pagkamatay ng kanyang mga kaalyado " +
        "at sa pagkalayo nila ni Maria Clara. Ang kanilang pagmamahalan ay tuluyang sinira ng pangingibabaw ng " +
        "kapangyarihan ng Simbahan at ng kolonyal na pamahalaan.\n\n" +

        "Sa huli, pinili ni Maria Clara na magmadre upang takasan ang realidad ng kanilang sitwasyon. Si Ibarra naman " +
        "ay muling naglaho sa dilim, dala ang kanyang mga sugat at hinanakit. Ang bayan ng San Diego ay nanatiling " +
        "nakalubog sa sistemang mapang-api, ngunit naiwan sa kanilang alaala ang tapang at layunin ng isang Crisostomo Ibarra.\n\n" +

        "Ang kwento ng Noli Me Tangere ay isang paalala ng kahalagahan ng kalayaan at katarungan, at ng sakripisyong " +
        "handang gawin ng mga taong tunay na nagmamahal sa kanilang bayan.",

        // El Filibusterismo Story
        "                       ANG SIMULA NG HIMAGSIKAN\n\n" +

        "Makaraan ang labintatlong taon, si Simoun, isang misteryosong alahero na puno ng kayamanan, ay dumating " +
        "sa Maynila. Siya ay si Crisostomo Ibarra na nagbabalik sa ibang katauhan. Ang kanyang layunin ay " +
        "ang maghiganti sa mga nagpahirap sa kanya at sa mga kababayan niyang inaapi. Sa kanyang anyo bilang " +
        "alahero, ginamit niya ang kanyang impluwensya upang pasiklabin ang galit ng mga tao laban sa mga makapangyarihan.\n\n" +

        "Pinili ni Simoun na magtrabaho kasama ang mga lider ng pamahalaan, tulad ng Kapitan Heneral. " +
        "Sa pamamagitan nito, mabilis siyang nagkaroon ng koneksyon sa mga elitista. Habang ginagawa niya ito, " +
        "lihim niyang inaabot ang mga karaniwang mamamayan upang palakasin ang kanilang damdaming makabayan.\n\n" +

        "Si Basilio, isang batang saksi sa kawalang-katarungan ng nakaraan, ay muling nagpakita sa kwento. " +
        "Ngayon ay isang mag-aaral na ng medisina, nalaman niyang si Simoun ay si Crisostomo Ibarra. " +
        "Hinimok siya ni Simoun na sumama sa kanyang layunin, ngunit nag-alinlangan si Basilio dahil sa takot " +
        "na maulit ang mga nangyari noong una.\n\n" +

        "Ang lihim na plano ni Simoun ay kasangkot ang isang engrandeng pagsabog na magaganap sa isang piging " +
        "ng mga elitista. Dito, plano niyang lipulin ang mga mapagsamantala sa lipunan. Subalit, hindi inaasahang " +
        "pinili ni Basilio na ipaalam ang plano kay Isagani, isang dating kaibigan, dahil sa pangambang maraming " +
        "inosenteng tao ang madadamay.\n\n" +

        "Sa araw ng piging, tumanggi si Isagani na hayaan ang plano ni Simoun. Inagaw niya ang lampara na puno " +
        "ng pampasabog at itinapon ito sa ilog bago pa man sumiklab ang trahedya. Ang ginawa ni Isagani ay " +
        "nagdulot ng pagkabigo kay Simoun, at lalong nagpuno ng galit sa kanyang puso.\n\n" +

        "Sa gitna ng kanyang pagkabigo, tumakas si Simoun sa bundok. Sa tulong ni Padre Florentino, isang pari " +
        "na kanyang pinagkatiwalaan, nagkaroon siya ng pagkakataon na pagnilayan ang kanyang mga ginawa. " +
        "Pinili ni Simoun na tapusin ang kanyang buhay, dala ang kanyang hinanakit at pagsisisi.\n\n" +

        "Sa huli, ang kayamanan at mga alahas na iniwan ni Simoun ay inialay ni Padre Florentino sa dagat, " +
        "bilang simbolo ng pag-asa na maghahatid ng mas magandang bukas para sa bayan.\n\n" +

        "Ang kwento ng El Filibusterismo ay patunay ng sakripisyo ng isang tao para sa pagbabago. Sa kabila " +
        "ng mga pagkakamali at pagkabigo, naipakita nito ang pag-asa at pananampalataya sa pagkakaisa ng mga Pilipino.",

        // Ibong Adarna Story
        "                       ANG PAGSUBOK NG MGA PRINSIPE\n\n" +

        "Sa kaharian ng Berbanya, si Haring Fernando ay nagkasakit dahil sa isang bangungot na hindi maipaliwanag. " +
        "Ang tanging lunas na nalaman ng kanyang mga doktor ay ang awit ng mahiwagang Ibong Adarna. Dahil dito, " +
        "iniutos niya sa kanyang tatlong anak na hanapin ang ibon upang maibalik ang kanyang kalusugan.\n\n" +

        "Ang panganay na anak na si Don Pedro ang unang naglakbay. Subalit, sa kabila ng kanyang tapang, siya " +
        "ay nabigo dahil sa kanyang kayabangan. Nakakita siya ng mahiwagang puno ng Piedras Platas, ngunit " +
        "siya ay natulog sa ilalim nito at naging bato.\n\n" +

        "Sumunod na naglakbay si Don Diego, ang pangalawang anak. Katulad ng kanyang kapatid, siya rin ay " +
        "nabigo. Napagod siya at natulog sa ilalim ng puno, at tulad ni Don Pedro, siya rin ay naging bato.\n\n" +

        "Sa wakas, si Don Juan, ang bunsong anak, ay naglakas-loob na hanapin ang ibon. Sa kanyang biyahe, " +
        "nakatagpo siya ng isang ermitanyo na nagbigay sa kanya ng payo at mahikang tubig. Sinunod niya ang payo, " +
        "at sa tulong ng ermitanyo, nakayanan niyang makuha ang Ibong Adarna.\n\n" +

        "Sa kanyang pagbabalik, sinalba niya ang kanyang mga kapatid mula sa kanilang sumpa gamit ang mahiwagang tubig. " +
        "Ngunit, dahil sa inggit, binalak nina Don Pedro at Don Diego na pagtaksilan si Don Juan. Binugbog nila " +
        "siya at iniwan sa gubat upang sila ang magdala ng Ibong Adarna sa kanilang ama.\n\n" +

        "Subalit, hindi nagtagumpay ang kanilang panloloko. Ang Ibong Adarna ay tumangging umawit at naging dahilan " +
        "ng muling pagkakasakit ng hari. Sa tulong ng mahiwagang ermitanyo, nakabalik si Don Juan sa palasyo " +
        "at nalaman ng hari ang totoo.\n\n" +

        "Pinatawad ni Don Juan ang kanyang mga kapatid sa kabila ng kanilang kasamaan. Sa kanyang pagiging mabait, " +
        "siya ay ginawaran ng parangal at itinanghal na tagapagmana ng trono.\n\n" +

        "Ang kwento ng Ibong Adarna ay isang paalala ng kabutihan, tapang, at pagpapakumbaba. Sa kabila ng mga pagsubok, " +
        "ang tagumpay ay laging nakakamit ng mga may malinis na puso.",



        };


        }
        private int EvaluateSummary(string userSummary, string originalStory)
        {
            grade = 0;

            // Handle edge case: single character or extremely short input
            if (userSummary.Trim().Length <= 1)
            {
                return 0; // Automatically fail for single-character or trivial input
            }
            else
            {
                grade = 60; // Start with base grade for acceptable length

                // Check for key phrases from the original story
                string[] keyPhrases = { 
                    // El Filibusterismo
                    "rebolusyon", "kayumanggi", "nasyonalismo", "katarungan", "independensya", "katipunan", "pag-aalsa", "Juan Crisostomo Ibarra", "Pilosopo Tasyo",
                    "rebolusyonaryong kilusan", "Simoun", "Kaharian ng Espanya", "mga prinsipe", 
                    // Ibong Adarna
                    "Ibong Adarna", "tigre", "nanghihina", "kaharian", "paghihiganti", "pagmamahal sa pamilya", "tahanan", "Panginoon", "paghahanap", "pagsasakripisyo",
                    // Noli Me Tangere
                    "noli me tangere", "social reform", "pagsisisi", "fathers and sons", "perseverance", "Maria Clara", "Padre Damaso", "pamilya", "Luneta", "oppression"
                };

                string[] criticalPhrases = {
                "rebolusyon", "independensya", "paghihiganti", "katarungan", "pagmamahal sa pamilya", "perseverance", "pagsisisi" }; // Higher importance
                foreach (var phrase in keyPhrases)
                {
                    if (userSummary.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        grade += criticalPhrases.Contains(phrase) ? 15 : 10; // Extra points for critical phrases
                    }
                }

                grade += EvaluateClarity(userSummary);
                grade += EvaluateGrammar(userSummary);
                grade += EvaluateRelevance(userSummary, originalStory);
                grade += EvaluateConciseness(userSummary, originalStory);
                grade += EvaluateCreativity(userSummary, originalStory);
                grade += EvaluateToneMatching(userSummary, originalStory);
                grade += EvaluateOriginality(userSummary, originalStory);
                grade += DetectCopyPaste(userSummary, originalStory);
                grade += EvaluateVocabularyVariation(userSummary);
                grade += EvaluateCohesion(userSummary);


                // Ensure grade doesn't exceed 100
                grade = Math.Min(100, grade);
            }

            return grade;
        }
        private int EvaluateClarity(string userSummary)
        {
            // Simple example: Deduct points if there are long, unclear sentences or lack of coherence
            int clarityScore = 0;
            if (userSummary.Split('.').Length < 3) // Too few sentences
            {
                clarityScore -= 10;
            }
            if (userSummary.Contains("hindi malinaw") || userSummary.Contains("at iba pa")) // Vague expressions
            {
                clarityScore -= 5;
            }
            return clarityScore;
        }

        private int EvaluateGrammar(string userSummary)
        {
            // Placeholder: Implement grammar checks. For now, we'll assume basic validation
            int grammarScore = 0;

            // If there are glaring grammar issues, reduce the score
            if (userSummary.Contains("ito ay mayroon") || userSummary.Contains("siya at sila") || userSummary.Contains("ako at siya")) // Common mistakes
            {
                grammarScore -= 10;
            }

            return grammarScore;
        }

        private int EvaluateRelevance(string userSummary, string originalStory)
        {
            // Assess if the user stayed true to the original story and didn’t introduce irrelevant elements
            int relevanceScore = 0;

            // Check if critical phrases are present for each story
            string[] criticalPhrases = { "rebolusyon", "independensya", "paghihiganti", "katarungan", "pagmamahal sa pamilya", "perseverance", "pagsisisi" };

            bool containsCriticalPhrase = criticalPhrases.Any(phrase =>
                userSummary.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0);

            if (!containsCriticalPhrase)
            {
                relevanceScore -= 10; // Deduct points if no critical phrases are used
            }

            return relevanceScore;
        }

        private int EvaluateConciseness(string userSummary, string originalStory)
        {
            // Reward concise summaries that retain important details
            int concisenessScore = 0;

            // For every 50 characters less than the original, award 5 points (encouraging brevity)
            if (userSummary.Length < originalStory.Length)
            {
                concisenessScore += 5;
            }

            return concisenessScore;
        }
        private int EvaluateCreativity(string userSummary, string originalStory)
        {
            int creativityScore = 0;

            // Reward creative introductions or styles
            if (userSummary.StartsWith("Noong unang panahon") ||
                userSummary.Contains("makulay na kwento") ||
                userSummary.Contains("puno ng inspirasyon"))
            {
                creativityScore += 10;
            }
            return creativityScore;
        }

        private int EvaluateToneMatching(string userSummary, string originalStory)
        {
            int toneScore = 0;

            // Match tones between summary and original
            if (originalStory.Contains("romansa") && userSummary.Contains("mapagmahal") ||
                originalStory.Contains("drama") && userSummary.Contains("emosyonal") ||
                originalStory.Contains("aral") && userSummary.Contains("nakakatuwa"))
            {
                toneScore += 10;
            }
            else
            {
                toneScore -= 5; // Deduct points for mismatched tone
            }

            return toneScore;
        }

        private int EvaluateOriginality(string userSummary, string originalStory)
        {
            if (userSummary.Equals(originalStory, StringComparison.OrdinalIgnoreCase))
            {
                return -150; // Heavy penalty for exact copy
            }

            // Check for percentage of matching content
            double similarity = CalculateSimilarity(userSummary, originalStory);
            if (similarity > 0.5) // More than 50% match
            {
                return -150; // Penalize heavily for high similarity
            }

            return 10; // Reward originality
        }
        private double CalculateSimilarity(string text1, string text2)
        {
            // Simple similarity calculation based on common word overlap
            var words1 = text1.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(w => w.ToLower())
                              .ToHashSet();
            var words2 = text2.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(w => w.ToLower())
                              .ToHashSet();

            int commonWords = words1.Intersect(words2).Count();
            int totalWords = Math.Max(words1.Count, words2.Count);

            return (double)commonWords / totalWords;
        }

        private int DetectCopyPaste(string userSummary, string originalStory)
        {
            // Split the story and summary into sentences
            var originalSentences = originalStory.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(s => s.Trim()).ToList();
            var userSentences = userSummary.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                           .Select(s => s.Trim()).ToList();

            int copyPenalty = 0;

            foreach (string userSentence in userSentences)
            {
                foreach (string originalSentence in originalSentences)
                {
                    // Exact match (case-insensitive)
                    if (string.Equals(userSentence, originalSentence, StringComparison.OrdinalIgnoreCase))
                    {
                        copyPenalty -= 50; // Deduct points for an exact match
                    }
                    else
                    {
                        // Check for high similarity using substring matching
                        double similarity = CalculateSimilarity(userSentence, originalSentence);
                        if (similarity > 0.8) // If similarity is above 80%
                        {
                            copyPenalty -= 50; // Deduct points for highly similar sentences
                        }
                    }
                }
            }

            // Ensure the penalty does not drop the grade below 0
            return Math.Max(copyPenalty, -100);
        }

        private int EvaluateVocabularyVariation(string userSummary)
        {
            string[] commonWords = { "mabuti", "masama", "masaya", "malungkot", "mahalaga" };

            int repeatedWordCount = 0;

            foreach (var word in commonWords)
            {
                repeatedWordCount += userSummary.Split(new[] { ' ', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                                .Count(w => w.Equals(word, StringComparison.OrdinalIgnoreCase));
            }

            int penalty = repeatedWordCount > 2 ? -10 : 0;
            return penalty;
        }
        private int EvaluateCohesion(string userSummary)
        {
            string[] transitionWords = { "gayunpaman", "samakatuwid", "bilang pagtatapos", "habang", "bukod pa rito" };

            int cohesionScore = 0;

            foreach (var word in transitionWords)
            {
                cohesionScore += userSummary.Split(new[] { ' ', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Count(w => w.Equals(word, StringComparison.OrdinalIgnoreCase));
            }

            if (cohesionScore > 3)
            {
                return -10; // Deduct for excessive transitions
            }

            return cohesionScore * 5;
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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, grade);

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
                DialogResult result = MessageBox.Show("Are you sure with your story retelling?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Check if the user has entered text in the summary box
                    if (string.IsNullOrWhiteSpace(EssayrichTextBox2.Text))
                    {
                        MessageBox.Show("Please type your retell story before completing the challenge.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Exit to prevent further execution
                    }
                    // Challenge completed successfully
                    timer.Stop();
                    progressBar1.Value = progressBar1.Maximum; // Set progress bar to max
                    textBox2.Text = TimeSpan.FromSeconds(progressDuration - timeRemaining).ToString("hh\\:mm\\:ss"); // Update final time

                    // Evaluate the summary
                    string userSummary = EssayrichTextBox2.Text;
                    string originalStory = EssayrichTextBox1.Text;
                    EvaluateSummary(userSummary, originalStory);

                    // Display the grade
                    MessageBox.Show($"Your summary grade is {grade}/100.", "Grade", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Save the data to JSON
                    if (grade >= 50)
                    {
                        SaveEssayContent(EssayrichTextBox2.Text);
                        success.Play();
                        SaveChallengeDataSuccess(Login.CurrentUsername, textBox2.Text, grade);
                        MessageBox.Show("Challenge Complete. Returning to the homepage.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TransitionToHomePage();
                    }
                    else
                    {
                        fail.Play();
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, grade);
                        MessageBox.Show("Challenge not completed. Returning to the homepage.", "Challenge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TransitionToHomePage();
                    }
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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, grade);

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
        private InstructionsSRE InstructionsSRE = null; // Class-level variable to track the Guide form

        private void InstructionspictureBox_Click(object sender, EventArgs e)
        {
            click.Play();

            // Check if the Guide form is already open
            if (InstructionsSRE != null && !InstructionsSRE.IsDisposed)
            {
                InstructionsSRE.BringToFront();
                return;
            }

            // Create the Guide form
            InstructionsSRE = new InstructionsSRE();
            InstructionsSRE.FormBorderStyle = FormBorderStyle.FixedToolWindow; // Set to FixedToolWindow
            InstructionsSRE.StartPosition = FormStartPosition.CenterScreen;
            InstructionsSRE.Opacity = 0; // Start at 0 for fade-in effect

            // Add functionality to fade out and close the Guide form when the X button is clicked
            InstructionsSRE.FormClosing += (s, ev) =>
            {
                ev.Cancel = true; // Prevent immediate closure
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
                fadeOutTimer.Tick += (s2, ev2) =>
                {
                    if (InstructionsSRE.Opacity > 0)
                    {
                        InstructionsSRE.Opacity -= 0.05; // Decrease opacity for fade-out
                    }
                    else
                    {
                        fadeOutTimer.Stop();
                        fadeOutTimer.Dispose();
                        InstructionsSRE.FormClosing -= null; // Remove event handler to avoid recursion
                        InstructionsSRE.Dispose(); // Dispose of the Guide form
                        InstructionsSRE = null; // Reset the reference
                    }
                };
                fadeOutTimer.Start();
            };

            // Show the Guide form
            InstructionsSRE.Show();

            // Fade-in effect for the Guide form
            Timer fadeInTimer = new Timer();
            fadeInTimer.Interval = 20;
            fadeInTimer.Tick += (s, ev) =>
            {
                if (InstructionsSRE.Opacity < 1)
                {
                    InstructionsSRE.Opacity += 0.05; // Increase opacity for fade-in
                }
                else
                {
                    fadeInTimer.Stop();
                    fadeInTimer.Dispose();
                }
            };
            fadeInTimer.Start();
        }
        private void label5_Click(object sender, EventArgs e)
        {
            click.Play();

            // Check if the Guide form is already open
            if (InstructionsSRE != null && !InstructionsSRE.IsDisposed)
            {
                InstructionsSRE.BringToFront();
                return;
            }

            // Create the Guide form
            InstructionsSRE = new InstructionsSRE();
            InstructionsSRE.FormBorderStyle = FormBorderStyle.FixedToolWindow; // Set to FixedToolWindow
            InstructionsSRE.StartPosition = FormStartPosition.CenterScreen;
            InstructionsSRE.Opacity = 0; // Start at 0 for fade-in effect

            // Add functionality to fade out and close the Guide form when the X button is clicked
            InstructionsSRE.FormClosing += (s, ev) =>
            {
                ev.Cancel = true; // Prevent immediate closure
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
                fadeOutTimer.Tick += (s2, ev2) =>
                {
                    if (InstructionsSRE.Opacity > 0)
                    {
                        InstructionsSRE.Opacity -= 0.05; // Decrease opacity for fade-out
                    }
                    else
                    {
                        fadeOutTimer.Stop();
                        fadeOutTimer.Dispose();
                        InstructionsSRE.FormClosing -= null; // Remove event handler to avoid recursion
                        InstructionsSRE.Dispose(); // Dispose of the Guide form
                        InstructionsSRE = null; // Reset the reference
                    }
                };
                fadeOutTimer.Start();
            };

            // Show the Guide form
            InstructionsSRE.Show();

            // Fade-in effect for the Guide form
            Timer fadeInTimer = new Timer();
            fadeInTimer.Interval = 20;
            fadeInTimer.Tick += (s, ev) =>
            {
                if (InstructionsSRE.Opacity < 1)
                {
                    InstructionsSRE.Opacity += 0.05; // Increase opacity for fade-in
                }
                else
                {
                    fadeInTimer.Stop();
                    fadeInTimer.Dispose();
                }
            };
            fadeInTimer.Start();
        }
        private void EssayrichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void EssayrichTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void StoryRetellingHard_Load(object sender, EventArgs e)
        {
            InitializeContent();
        }
    }
}
