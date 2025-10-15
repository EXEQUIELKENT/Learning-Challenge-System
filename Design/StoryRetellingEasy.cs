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
    public partial class StoryRetellingEasy : Form
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
        private int grade = 0;
        public StoryRetellingEasy()
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
                FormTitle = "Story Retelling (Easy)",
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
                FormTitle = "Story Retelling (Easy)",
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
        // Love Story
        "                       PAG-IBIG SA LUGAR NG BITUIN\n\n" +

        "Si Maria, isang simpleng guro sa baryo, ay laging humahanga sa mga bituin tuwing gabi. Para sa kanya, " +
        "ang kalangitan ay puno ng mga kwento ng pag-asa at pangarap. Isang gabi, habang nagmamasid siya sa mga tala, " +
        "nakilala niya si Miguel, isang astropisiko na bumalik sa kanilang baryo para magpahinga mula sa maingay na lungsod. " +
        "Pareho nilang napansin ang kakaibang ningning ng mga bituin, at nag-umpisa silang mag-usap tungkol sa kanilang mga pangarap.\n\n" +

        "Habang tumatagal ang kanilang pagkakakilala, natutunan nila ang mga pagkakaiba at pagkakapareho nila. " +
        "Si Maria ay nananatili sa baryo dahil sa pagmamahal sa pagtuturo, samantalang si Miguel ay naglalakbay upang maabot " +
        "ang pinakamalayong bituin gamit ang siyensya. Sa bawat gabi ng pag-uusap, unti-unti silang nahulog sa isa't isa. " +
        "Sa isang malamig na gabi, naglakas-loob si Miguel na sabihin kay Maria na ang kanyang pinakamalaking pangarap ay " +
        "makasama siya sa lahat ng tagumpay at kabiguan.\n\n" +

        "Ngunit kailangang pumili si Maria: manatili sa komportable niyang mundo o samahan si Miguel sa malalayong lugar " +
        "habang hinahabol ang mga bituin. Sa huli, napagtanto niya na ang tunay na pag-ibig ay nangangailangan ng tapang. " +
        "Kasama si Miguel, iniwan niya ang baryo at sinimulan ang bagong yugto ng kanilang buhay, na puno ng pag-asa at " +
        "pagmamahal sa bawat hakbang sa ilalim ng mga bituin.",

        // Pagong at Matsing Story
        "                       PAGONG AT NI MATSING\n\n" +

        "Sa isang maliit na baryo sa tabi ng ilog, namumuhay si Pagong at si Matsing. Si Pagong ay mabagal ngunit " +
        "matiyaga, samantalang si Matsing ay mabilis ngunit laging nagmamadali. Sa araw-araw nilang pagsasama, " +
        "madalas silang nagkukwentuhan sa ilalim ng punong saging. Minsan, naisipan nilang magtanim ng punong saging " +
        "upang magamit ang bunga nito para sa kanilang pangangailangan.\n\n" +

        "Nang dumating ang araw ng pagtatanim, nagkaroon sila ng alitan. Si Matsing ay gusto ang itaas ng puno " +
        "dahil naniniwala siyang mabilis itong magbubunga, samantalang si Pagong ay pinili ang ugat dahil alam niyang " +
        "doon nagmumula ang tunay na sustansya. Sa kabila ng kanilang pagtatalo, pinagbigyan ni Matsing si Pagong at iniwan " +
        "ang ugat sa kanya. Paglipas ng mga buwan, nakita ni Matsing na ang kanyang itaas ng puno ay natuyo, samantalang " +
        "ang ugat ni Pagong ay lumago ng matayog at may bunga na.\n\n" +

        "Nakita ni Matsing ang kahalagahan ng pagtitiyaga at tamang desisyon. Humingi siya ng tawad kay Pagong, " +
        "at nagdesisyon silang magsama-sama upang tamasahin ang bunga ng kanilang tanim. Sa huli, natutunan nila " +
        "na ang pagkakaibigan ay hindi tungkol sa pagiging tama, kundi sa pagsuporta sa isa’t isa sa bawat hakbang.",

        // Drama Story
        "                       ANG PAGPAPATAWAD NI CLARA\n\n" +

        "Si Clara ay laging nasa gitna ng kanyang pamilya—isang panganay na inaasahang magsakripisyo para sa lahat. " +
        "Nang dumating ang isang pagkakataon na mag-aaral siya sa ibang bansa, nagkaroon ng tensyon sa kanilang pamilya. " +
        "Ang kanyang ina, si Aling Teresa, ay nagalit dahil sa pangambang iiwan sila ni Clara. Sa gitna ng mga sigawan, " +
        "nagdesisyon si Clara na isantabi ang kanyang pangarap upang manatili sa kanyang pamilya.\n\n" +

        "Ngunit habang lumilipas ang mga taon, unti-unting naramdaman ni Clara ang lungkot at pagsisisi. " +
        "Nagkakasakit na siya dahil sa bigat ng loob, ngunit hindi niya kayang sabihin ang kanyang nararamdaman sa ina. " +
        "Sa isang gabi ng kanilang simpleng salu-salo, nabasag ang katahimikan nang sabihin ni Clara ang kanyang " +
        "dinaramdam. Naluha si Aling Teresa at inamin niyang natakot lamang siyang maiwan ng kanyang anak. " +
        "Hiningi niya ang tawad ni Clara at sinabing hindi niya dapat hinadlangan ang pangarap nito.\n\n" +

        "Sa wakas, natutunan nilang magpatawad at magpahalaga sa isa’t isa. Sa suporta ng pamilya, muling binuhay " +
        "ni Clara ang kanyang pangarap. Lumipad siya sa ibang bansa upang mag-aral, ngunit sa bawat tagumpay, " +
        "hindi niya kinalimutan ang kanyang pamilya. Ang kanilang kwento ay naging patunay na ang pagmamahal at " +
        "pagpapatawad ay laging may lugar sa ating buhay, gaano man kahirap ang mga hamon."



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
                    // Love Story
                    "pag-ibig", "librarian", "library", "Samantha", "Ethan", "libro", "photographer", "adventure", "romansa", "pagsasama",
                    // Pagong at Si Matsing
                    "Pagong", "Matsing", "saging", "kawayan", "tanim", "tapat", "panloloko", "pagtutulungan", "magkaibigan", "aral",
                    // Drama Story
                    "trahedya", "pamilya", "sakripisyo", "tagumpay", "pagkakaibigan", "pangarap", "pag-asa", "desisyon", "emotional", "drama"
                };
                string[] criticalPhrases = { "pag-ibig", "aral", "pangarap", "sakripisyo", "pagsasama" }; // Higher importance
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
            string[] criticalPhrases = { "pag-ibig", "aral", "pangarap", "sakripisyo", "pagsasama" };

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
                return -50; // Heavy penalty for exact copy
            }

            // Check for percentage of matching content
            double similarity = CalculateSimilarity(userSummary, originalStory);
            if (similarity > 0.5) // More than 50% match
            {
                return -50; // Penalize heavily for high similarity
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
        private void StoryRetellingEasy_Load(object sender, EventArgs e)
        {
            InitializeContent();
        }
    }
}
