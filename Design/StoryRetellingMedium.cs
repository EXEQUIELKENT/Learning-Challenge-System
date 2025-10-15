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
    public partial class StoryRetellingMedium : Form
    {
        private Timer timer;
        private int progressDuration = 1800; // total time for progress bar in seconds
        private int timeRemaining;
        private SoundPlayer soundPlayer;
        private SoundPlayer click;
        private SoundPlayer fail;
        private SoundPlayer success;
        private SoundPlayer count;
        private bool isEnterKeyDisabled = false;
        private int grade = 0;
        public StoryRetellingMedium()
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
                FormTitle = "Story Retelling (Medium)",
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
                FormTitle = "Story Retelling (Medium)",
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
        // Action Story  
        "                       ANG BAYANING WALANG TAKOT\n\n" +

        "Sa gitna ng isang maliit na bayan sa kabundukan, nakilala si Elias, isang dating sundalo na nagdesisyong " +
        "mamuhay ng tahimik matapos ang maraming taon ng digmaan. Ngunit ang katahimikan ng kanilang lugar ay " +
        "nabali nang dumating ang isang grupo ng mga tulisan na nagdulot ng takot at kaguluhan sa mga tao. Hindi " +
        "kinaya ni Elias na magpabaya, kaya't muli niyang isinuot ang kanyang lumang uniporme upang ipaglaban ang " +
        "kanyang bayan.\n\n" +

        "Sa unang gabi ng kanyang paglaban, pinuntirya ni Elias ang kampo ng mga tulisan. Gumamit siya ng " +
        "kanyang kaalaman sa estratehiya upang magtago sa dilim at iligtas ang ilang bihag. Dahil sa kanyang tapang, " +
        "unti-unti siyang nakilala bilang 'Bayani ng Gabi.' Ngunit habang lumalaban, nalaman niyang ang lider ng mga tulisan " +
        "ay si Arnel, isang dati niyang kaibigan mula sa hukbo.\n\n" +

        "Nagharap sina Elias at Arnel sa isang matinding labanan. Habang umiwas sa mga patibong ni Arnel, " +
        "sinubukan ni Elias na kumbinsihin ang dating kaibigan na itigil ang kasamaan. Ngunit tumanggi si Arnel, " +
        "pinipiling ipagpatuloy ang kanyang paniniwala na ang kaguluhan ay ang tanging sagot sa kanilang kahirapan. " +
        "Sa huli, napilitang pabagsakin ni Elias ang kaibigan upang iligtas ang bayan.\n\n" +

        "Sa kabila ng tagumpay, mabigat ang loob ni Elias. Hindi niya inakala na ang kanyang laban ay magdudulot ng " +
        "pagkawala ng isang mahalagang kaibigan. Gayunpaman, naging inspirasyon ang kanyang kwento para sa mga tao " +
        "na lumaban para sa kanilang bayan at manindigan sa tama.\n\n" +

        "Ang bayan ay muling bumangon mula sa takot, salamat sa kabayanihan ni Elias. Naging simbolo siya ng pag-asa, " +
        "at kahit tahimik na bumalik sa kanyang simpleng buhay, nanatili sa puso ng lahat ang alaala ng 'Bayani ng Gabi.'",  


        // Mermaid Story  
        "                       ANG LIHIM NG SIRENA\n\n" +

        "Sa ilalim ng mala-kristal na dagat ng Isla Sirena, nakatira si Mira, isang sirena na pinagbawalang " +
        "lumapit sa mundo ng mga tao. Lumaki siyang palaisip kung bakit kailangang iwasan ang lupa, ngunit " +
        "isang araw, hindi na niya napigilan ang kanyang pag-usisa. Sa kanyang pagtakas mula sa palasyo ng kanyang " +
        "ama, napadpad siya sa dalampasigan at nakita si Leo, isang mangingisda na may mabuting puso.\n\n" +

        "Habang nagtatago, sinubaybayan ni Mira si Leo mula sa malayo. Nakita niya kung paano ito tumutulong " +
        "sa mga kababayan nito, lalo na sa panahon ng taggutom. Sa kabila ng utos ng kanyang ama, hindi napigilan ni Mira " +
        "na lapitan si Leo upang magpakilala. Sa kanilang pag-uusap, natuklasan ni Mira ang kabutihan ng tao, " +
        "habang si Leo ay namangha sa kagandahan at pagiging espesyal ng sirena.\n\n" +

        "Ngunit dumating ang isang araw na nahuli si Mira ng ibang tao. Pinagtangkaan siyang gawing atraksyon " +
        "ng mga negosyante. Ginamit ni Leo ang kanyang tapang upang iligtas si Mira mula sa masamang balak. " +
        "Sa kanilang pagtakas, napagtanto ni Mira na hindi lahat ng tao ay mapanganib, ngunit hindi rin lahat " +
        "ay mabuti.\n\n" +

        "Binalikan ni Mira ang kanyang kaharian at ipinagtapat sa kanyang ama ang lahat. Sa halip na magalit, " +
        "naunawaan ng kanyang ama ang punto ng anak. Pinayagan niya si Mira na bumalik sa dalampasigan paminsan-minsan " +
        "upang magdala ng tulong sa mga tao. Kasabay nito, natutunan din ni Leo ang kahalagahan ng kalikasan " +
        "at pagprotekta sa dagat.\n\n" +

        "Ang kwento ni Mira at Leo ay naging alamat sa Isla Sirena, isang paalala na ang pagkakaibigan ay hindi " +
        "nalilimitahan ng karagatan. Sa tuwing may makikitang bahaghari sa dagat, sinasabi ng mga tao na iyon " +
        "ay alaala ng kanilang pagkakaibigan.",  


        // Horror Story  
        "                       ANG KABABALAGHAN SA BAHAY NA BATO\n\n" +

        "Sa lumang baryo ng San Mateo, nakatayo ang isang bahay na bato na kinatatakutan ng mga tao. " +
        "Ayon sa kwento, sa tuwing magtatakip-silim, may naririnig na mga iyak at halakhak mula sa loob ng bahay. " +
        "Isang gabi, nagpasya si Marco, isang batang manunulat na naghahanap ng inspirasyon, na mag-imbestiga " +
        "kasama ang kanyang kaibigang si Lito.\n\n" +

        "Pagpasok nila sa bahay, nakaramdam sila ng bigat ng hangin. Ang bawat hakbang ay parang may kasamang " +
        "bulong mula sa mga pader. Sa isang silid, natagpuan nila ang isang lumang diary na nagsasabing ang " +
        "may-ari ng bahay ay isang albularyo na nasawi dahil sa isang ritwal na hindi natapos. Habang binabasa " +
        "ang diary, biglang sumara ang pinto at naramdaman nilang may malamig na kamay na dumampi sa kanilang mga balikat.\n\n" +

        "Nagdesisyon silang tumakbo palabas, ngunit para bang naliligaw sila sa isang bahay na tila lumalaki. " +
        "Ang bawat pintuan na kanilang buksan ay bumabalik lamang sa parehong silid. Nang mawalan na sila ng pag-asa, " +
        "napansin ni Marco na ang diary ay may mga pahinang nabahiran ng dugo. Binasa niya ang huling bahagi nito at " +
        "natuklasang kailangan nilang tawagin ang pangalan ng albularyo upang makalabas.\n\n" +

        "Pagkatapos nilang banggitin ang pangalan, huminto ang lahat ng kakaibang nangyayari. Ngunit sa kanilang " +
        "paglabas, nakita nila ang isang anino sa bintana, na para bang nagbabantay pa rin ang espiritu ng albularyo. " +
        "Sa kanilang pag-uwi, nagdesisyon si Marco na isulat ang kwento, ngunit sa bawat pagpatak ng tinta sa papel, " +
        "nakakarinig siya ng mahihinang boses na umaawit mula sa bahay na bato.\n\n" +

        "Hanggang ngayon, nananatiling palaisipan kung ang bahay na bato ay talaga bang pinamumugaran ng mga espiritu. " +
        "Ang kwento ni Marco at Lito ay naging babala sa mga tao na huwag magpunta sa bahay tuwing gabi. Ngunit may " +
        "mga nagsasabing naririnig pa rin ang halakhak mula sa malayo.",



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
                    // Action Story
                    "kalsada", "paghabol", "mandirigma", "bagyo", "labu-labo", "aliado", "kaligtasan", "plano", "kalaban", "kaguluhan",
                    // Mermaid Story
                    "dagat", "sirena", "kaibigan", "sireno", "kabutihan", "kaharian", "perlas", "trono", "tubig", "magical",
                    // Horror Story
                    "multo", "maligno", "lumang bahay", "madilim", "sigaw", "aparador", "salamin", "puno", "panaginip", "tulungan"
                };
                string[] criticalPhrases = { "kaligtasan", "kabutihan", "perlas", "multo", "mandirigma" }; // Higher importance
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
            if (userSummary.Contains("hindi malinaw") || userSummary.Contains("at kung anu-ano pa"))
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
            if (userSummary.Contains("ang dagat ay nalulunod") ||
                userSummary.Contains("siya sila ito") ||
                userSummary.Contains("ito at iyon"))
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
            string[] criticalPhrases = { "kaligtasan", "kabutihan", "perlas", "multo", "mandirigma" };

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
            if (userSummary.StartsWith("Sa gitna ng kaguluhan") ||
                userSummary.Contains("mga mahiwagang kwento ng dagat") ||
                userSummary.Contains("nakakatakot na kwento"))
            {
                creativityScore += 10;
            }
            return creativityScore;
        }

        private int EvaluateToneMatching(string userSummary, string originalStory)
        {
            int toneScore = 0;

            // Match tones between summary and original
            if ((originalStory.Contains("kaguluhan") && userSummary.Contains("mapanganib")) ||
                (originalStory.Contains("sirena") && userSummary.Contains("kamangha-mangha")) ||
                (originalStory.Contains("multo") && userSummary.Contains("nakakatakot")))
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
                return -60; // Heavy penalty for exact copy
            }

            // Check for percentage of matching content
            double similarity = CalculateSimilarity(userSummary, originalStory);
            if (similarity > 0.5) // More than 50% match
            {
                return -60; // Penalize heavily for high similarity
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
            string[] commonWords = { "maganda", "mahirap", "masaya", "nakakatakot", "mapanganib" };

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
            string[] transitionWords = { "gayunpaman", "samantala", "sa kabilang banda", "bilang pagtatapos", "samakatuwid" };

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
        private void StoryRetellingMedium_Load(object sender, EventArgs e)
        {
            InitializeContent();
        }
    }
}
