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
    public partial class BookSummaryMedium : Form
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
        public BookSummaryMedium()
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
                FormTitle = "Book Summary (Medium)",
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
                FormTitle = "Book Summary (Medium)",
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
            DialogResult result = MessageBox.Show("Do you want to save your book summary?", "Save File", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
        //Horror Story
        "                THE SHADOWED WOODS\n\n" +

        "Late one stormy night, Mia found herself driving through an unfamiliar road deep in the countryside. " +
        "Her car's headlights barely pierced the dense fog that cloaked the surrounding woods. Suddenly, her car sputtered " +
        "and died, leaving her stranded in the middle of nowhere. Desperate, Mia grabbed a flashlight and ventured into the woods, " +
        "hoping to find help. The eerie silence was broken only by the crunch of her footsteps on the damp ground.\n\n" +

        "As she wandered deeper, she stumbled upon an abandoned cabin. Inside, dust-covered furniture and faded photographs " +
        "hinted at a life long forgotten. Just as she turned to leave, the door slammed shut behind her. The air grew colder, and " +
        "shadows seemed to creep along the walls. Terrified, Mia tried to open the door, but it wouldn’t budge. A faint whisper " +
        "called her name, sending chills down her spine.\n\n" +

        "With no other option, Mia began searching the cabin for a way out. She found an old diary that revealed the tragic story " +
        "of a family who had lived there. They had mysteriously vanished years ago, and their restless spirits now haunted the place. " +
        "As the whispers grew louder, Mia realized she wasn’t alone. Shadowy figures emerged, their hollow eyes fixed on her. " +
        "She clutched the diary, hoping it held the key to her escape.\n\n" +

        "By piecing together the diary's cryptic entries, Mia discovered that the spirits sought closure. She recited the " +
        "final words of the diary aloud, and the shadows froze. The cabin shuddered, and the spirits dissolved into the air, " +
        "their anguished cries fading into the night. The door creaked open, and Mia stumbled out, her heart racing. " +
        "The woods seemed less foreboding as she made her way back to the car.\n\n" +

        "Though she escaped, Mia knew she would never forget the horrors she faced in the shadowed woods. " +
        "The experience left her shaken but determined to uncover the truth behind the family's disappearance, " +
        "turning her fear into a quest for justice.",

        // Bleach Anime Story
        "                 BLOOD OF THE HOLLOW\n\n" +

        "Ichigo Kurosaki had faced countless Hollows, but this one was different. A new breed of Hollow had emerged in Karakura Town, " +
        "one that was impervious to ordinary Zanpakutō attacks. As Ichigo rushed to confront the creature, he realized it was targeting " +
        "a group of civilians. With his Bankai activated, he managed to save them, but the Hollow disappeared before he could deliver " +
        "a finishing blow. He reported the incident to Soul Society, but even they seemed baffled by the Hollow's unique powers.\n\n" +

        "Meanwhile, Rukia Kuchiki uncovered an ancient scroll that hinted at the origins of these Hollows. " +
        "She discovered they were created by an exiled Soul Reaper who had experimented with merging Hollow and Shinigami " +
        "powers. Ichigo and his friends embarked on a mission to track down this rogue Soul Reaper, venturing into the Dangai " +
        "to confront him. Along the way, they encountered powerful Hollows infused with Shinigami abilities, " +
        "pushing them to their limits.\n\n" +

        "The rogue Soul Reaper, known as Kaito, revealed himself in a climactic battle. Wielding a corrupted Zanpakutō, " +
        "he demonstrated his ability to control the hybrid Hollows. Ichigo, fueled by his determination to protect his friends, " +
        "unleashed a new technique combining his Hollow and Shinigami powers. The clash between the two warriors " +
        "shook the very fabric of the spiritual world.\n\n" +

        "Despite his overwhelming strength, Kaito's madness made him vulnerable. With a final strike, Ichigo defeated him, " +
        "severing the bond between him and the hybrid Hollows. As Kaito faded away, he warned Ichigo of a greater threat looming " +
        "in Hueco Mundo. Soul Society acknowledged the need to prepare for this new enemy, and Ichigo vowed to defend " +
        "both the human and spiritual realms.\n\n" +

        "Though peace was restored for the time being, Ichigo knew that his battles were far from over. " +
        "The fight against Kaito had pushed him to evolve, but it also reminded him of the stakes involved in protecting those " +
        "he cared about. The line between Hollow and Shinigami blurred further, leaving Ichigo to question his true nature.",

        // Action Story
        "                 THE ROGUE ASSASSIN\n\n" +

        "In the sprawling metropolis of Neo-Tokyo, a rogue assassin named Kael roamed the streets, hunted by the very " +
        "organization that had trained him. Kael, once their top operative, had turned against them after uncovering their " +
        "plan to destabilize the city for profit. With a bounty on his head, Kael used his skills to stay one step ahead, " +
        "but he knew it was only a matter of time before they caught up to him.\n\n" +

        "One fateful night, Kael intercepted a secret shipment of weapons destined for the organization. " +
        "During the raid, he encountered Aya, a skilled hacker seeking revenge for her family's death at the hands of the " +
        "same group. Reluctantly, they joined forces, combining Kael's combat prowess with Aya's technological expertise. " +
        "The unlikely duo set out to expose the organization's crimes to the world.\n\n" +

        "Their mission led them to a heavily fortified skyscraper, the organization's headquarters. " +
        "Battling through waves of mercenaries, Kael and Aya faced danger at every turn. Aya hacked into their systems, " +
        "uncovering damning evidence, while Kael fought off elite assassins sent to eliminate them. The final showdown came " +
        "when Kael confronted his former mentor, Raiden, a ruthless operative who embodied everything Kael had rejected.\n\n" +

        "In a fierce battle atop the skyscraper, Kael and Raiden clashed, their skills evenly matched. " +
        "Kael's determination to protect Aya and bring justice to Neo-Tokyo gave him the edge. With a calculated move, he " +
        "defeated Raiden and secured the evidence, broadcasting it to the world. The organization crumbled under public scrutiny, " +
        "and Kael's name was cleared.\n\n" +

        "Though Kael and Aya went their separate ways, their bond remained unbroken. Kael continued to fight for justice " +
        "in the shadows, knowing that his actions had given the city a chance to heal. His journey was far from over, " +
        "but for the first time, he felt hope for a better future."


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
            // Check for length (adjust thresholds as needed)
            if (userSummary.Length < originalStory.Length * 0.3) // Too short
            {
                grade = 30;
            }
            else if (userSummary.Length > originalStory.Length * 1.2) // Too long
            {
                // Deduct points for being too long
                int excessLength = userSummary.Length - (int)(originalStory.Length * 1.2);
                grade = 60 - (excessLength / 10); // Deduct 1 point for every 10 characters over the limit
                grade = Math.Max(grade, 40); // Ensure grade doesn't go below 40 for being too long
            }
            else
            {
                grade = 60; // Start with base grade for acceptable length

                string[] keyPhrases = { 
                    // Horror Story
                    "shadowed woods", "Mia", "stormy night", "countryside", "car", "flashlight", "fog", "abandoned cabin",
                    "diary", "spirits", "shadows", "family disappearance", "closure", "escape",
    
                    // Bleach Anime Story
                    "Ichigo", "Karakura Town", "Bankai", "Hollow", "Zanpakutō", "Soul Society", "Rukia Kuchiki", "Dangai",
                    "Kaito", "hybrid Hollows", "corrupted Zanpakutō", "Hueco Mundo", "Shinigami powers",
    
                    // Action Story
                    "Kael", "Neo-Tokyo", "rogue assassin", "Aya", "hacker", "weapons shipment", "organization",
                    "skyscraper", "Raiden", "justice", "public scrutiny", "mentor", "battle"
                };

                string[] criticalPhrases = {
                    "spirits", "escape", "closure",
                    "Bankai", "Hollow", "Shinigami",
                    "justice", "organization", "mentor"
                };
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
                grade += EvaluatePassiveVoice(userSummary);
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
            if (userSummary.Contains("etc.") || userSummary.Contains("and so on")) // Vague expressions
            {
                clarityScore -= 5;
            }
            return clarityScore;
        }

        private int EvaluateGrammar(string userSummary)
        {
            // Placeholder: You can implement a grammar check here, for now, we'll assume it's ok
            int grammarScore = 0;

            // If there are any glaring grammar issues, reduce the score
            if (userSummary.Contains("it's their") || userSummary.Contains("yourself") || userSummary.Contains("me and he")) // Common mistakes
            {
                grammarScore -= 10;
            }

            return grammarScore;
        }

        private int EvaluateRelevance(string userSummary, string originalStory)
        {
            // Assess if the user stayed true to the original story and didn’t introduce irrelevant elements
            int relevanceScore = 0;

            // Simple check: If no critical phrases are used, deduct points
            if (!userSummary.Contains("horror") && !userSummary.Contains("bankai") && !userSummary.Contains("battle"))
            {
                relevanceScore -= 10;
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
            if (userSummary.StartsWith("Once upon a time") || userSummary.Contains("imaginative")) // Example for creativity detection
            {
                creativityScore += 10; // Reward creative introductions or styles
            }
            return creativityScore;
        }

        private int EvaluateToneMatching(string userSummary, string originalStory)
        {
            int toneScore = 0;
            if (originalStory.Contains("terrifying") && userSummary.Contains("intense") ||
                originalStory.Contains("battle") && userSummary.Contains("uplifting"))
            {
                toneScore += 10; // Reward matching tones between summary and original
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
        private int EvaluatePassiveVoice(string userSummary)
        {
            // Check for passive voice usage (simple heuristic-based check)
            string[] passiveVoiceIndicators = { " is being ", " are being ", " was being ", " were being ", " been ", " by " };

            int passiveCount = 0;

            foreach (var indicator in passiveVoiceIndicators)
            {
                if (userSummary.IndexOf(indicator, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    passiveCount++;
                }
            }

            // Penalize if passive voice appears more than 3 times
            if (passiveCount > 3)
            {
                return -15; // Deduct more points for overuse of passive voice
            }
            return 0;
        }

        private int EvaluateVocabularyVariation(string userSummary)
        {
            // Check for vocabulary variation (a simple approach)
            string[] commonWords = { "good", "bad", "happy", "sad", "interesting" };

            int repeatedWordCount = 0;

            foreach (var word in commonWords)
            {
                // Count occurrences of common words
                repeatedWordCount += userSummary.Split(new[] { ' ', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                                .Count(w => w.Equals(word, StringComparison.OrdinalIgnoreCase));
            }

            // Deduct points for excessive repetition of common words
            int penalty = repeatedWordCount > 2 ? -10 : 0; // Deduct 10 points if any common word is used more than twice
            return penalty;
        }
        private int EvaluateCohesion(string userSummary)
        {
            // Check if the summary has good cohesion (simple check for transitions)
            string[] transitionWords = { "however", "therefore", "in conclusion", "meanwhile", "furthermore" };

            int cohesionScore = 0;

            foreach (var word in transitionWords)
            {
                cohesionScore += userSummary.Split(new[] { ' ', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Count(w => w.Equals(word, StringComparison.OrdinalIgnoreCase));
            }

            // Penalize if there are more than 3 transition words
            if (cohesionScore > 3)
            {
                return -10; // Deduct 10 points for excessive use of transition words
            }

            return cohesionScore * 5; // Give points for cohesive transitions, but penalize overuse
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
                DialogResult result = MessageBox.Show("Are you sure with your summary?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Check if the user has entered text in the summary box
                    if (string.IsNullOrWhiteSpace(EssayrichTextBox2.Text))
                    {
                        MessageBox.Show("Please type your summary before completing the challenge.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        private InstructionsBSE InstructionsBSE = null; // Class-level variable to track the Guide form

        private void InstructionspictureBox_Click(object sender, EventArgs e)
        {
            click.Play();

            // Check if the Guide form is already open
            if (InstructionsBSE != null && !InstructionsBSE.IsDisposed)
            {
                InstructionsBSE.BringToFront();
                return;
            }

            // Create the Guide form
            InstructionsBSE = new InstructionsBSE();
            InstructionsBSE.FormBorderStyle = FormBorderStyle.FixedToolWindow; // Set to FixedToolWindow
            InstructionsBSE.StartPosition = FormStartPosition.CenterScreen;
            InstructionsBSE.Opacity = 0; // Start at 0 for fade-in effect

            // Add functionality to fade out and close the Guide form when the X button is clicked
            InstructionsBSE.FormClosing += (s, ev) =>
            {
                ev.Cancel = true; // Prevent immediate closure
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
                fadeOutTimer.Tick += (s2, ev2) =>
                {
                    if (InstructionsBSE.Opacity > 0)
                    {
                        InstructionsBSE.Opacity -= 0.05; // Decrease opacity for fade-out
                    }
                    else
                    {
                        fadeOutTimer.Stop();
                        fadeOutTimer.Dispose();
                        InstructionsBSE.FormClosing -= null; // Remove event handler to avoid recursion
                        InstructionsBSE.Dispose(); // Dispose of the Guide form
                        InstructionsBSE = null; // Reset the reference
                    }
                };
                fadeOutTimer.Start();
            };

            // Show the Guide form
            InstructionsBSE.Show();

            // Fade-in effect for the Guide form
            Timer fadeInTimer = new Timer();
            fadeInTimer.Interval = 20;
            fadeInTimer.Tick += (s, ev) =>
            {
                if (InstructionsBSE.Opacity < 1)
                {
                    InstructionsBSE.Opacity += 0.05; // Increase opacity for fade-in
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
            if (InstructionsBSE != null && !InstructionsBSE.IsDisposed)
            {
                InstructionsBSE.BringToFront();
                return;
            }

            // Create the Guide form
            InstructionsBSE = new InstructionsBSE();
            InstructionsBSE.FormBorderStyle = FormBorderStyle.FixedToolWindow; // Set to FixedToolWindow
            InstructionsBSE.StartPosition = FormStartPosition.CenterScreen;
            InstructionsBSE.Opacity = 0; // Start at 0 for fade-in effect

            // Add functionality to fade out and close the Guide form when the X button is clicked
            InstructionsBSE.FormClosing += (s, ev) =>
            {
                ev.Cancel = true; // Prevent immediate closure
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
                fadeOutTimer.Tick += (s2, ev2) =>
                {
                    if (InstructionsBSE.Opacity > 0)
                    {
                        InstructionsBSE.Opacity -= 0.05; // Decrease opacity for fade-out
                    }
                    else
                    {
                        fadeOutTimer.Stop();
                        fadeOutTimer.Dispose();
                        InstructionsBSE.FormClosing -= null; // Remove event handler to avoid recursion
                        InstructionsBSE.Dispose(); // Dispose of the Guide form
                        InstructionsBSE = null; // Reset the reference
                    }
                };
                fadeOutTimer.Start();
            };

            // Show the Guide form
            InstructionsBSE.Show();

            // Fade-in effect for the Guide form
            Timer fadeInTimer = new Timer();
            fadeInTimer.Interval = 20;
            fadeInTimer.Tick += (s, ev) =>
            {
                if (InstructionsBSE.Opacity < 1)
                {
                    InstructionsBSE.Opacity += 0.05; // Increase opacity for fade-in
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
        private void BookSummaryMedium_Load(object sender, EventArgs e)
        {
            InitializeContent();
        }
    }
}
