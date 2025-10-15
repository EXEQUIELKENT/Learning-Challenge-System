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
    public partial class BookSummaryHard : Form
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
        public BookSummaryHard()
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
                FormTitle = "Book Summary (Hard)",
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
                FormTitle = "Book Summary (Hard)",
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
        // Friendship Story
        "                 THE BOND OF FRIENDSHIP\n\n" +

        "On a bright sunny day in the small town of Oakridge, two childhood friends, Lucas and Liam, sat on a grassy hill, " +
        "reminiscing about their adventures through the years. They had known each other since they were five, playing together " +
        "under the big oak tree near their school. Time had passed, but their friendship had never wavered, and it remained " +
        "stronger than ever. Together, they faced every challenge that life had thrown at them.\n\n" +

        "As they grew older, their paths began to diverge. Lucas was a talented musician, while Liam focused on his studies " +
        "to become a doctor. Yet, despite the differences in their lives, they still made time for one another, never allowing " +
        "distance to come between them. Whether it was a phone call or a weekend trip, their bond was unbreakable.\n\n" +

        "One day, disaster struck. Lucas's family lost everything in a fire that destroyed their home and all their possessions. " +
        "Liam was the first person Lucas called, and without hesitation, he rushed to his side. Together, they worked tirelessly " +
        "to rebuild not just Lucas’s house but also his spirit. Liam's unwavering support gave Lucas the strength to move forward.\n\n" +

        "As the months passed, the bond between Lucas and Liam only grew stronger. They celebrated each other's successes and " +
        "provided comfort during times of failure. No matter what challenges they faced, they always knew they could rely on " +
        "each other. Their friendship was a beacon of light in the darkest of times.\n\n" +

        "Eventually, Lucas's music career took off, and he found himself traveling around the world. Despite the busy schedule, " +
        "he made sure to stay connected with Liam, and whenever he was in town, they would meet and reminisce about old times. " +
        "They both realized that no matter where life took them, their friendship would remain the constant force that guided them.\n\n" +

        "As years went by, they both grew older, and the nature of their lives changed. Lucas became a famous musician, and " +
        "Liam fulfilled his dream of becoming a renowned doctor. But their bond remained untouched by fame or success.\n\n" +

        "One day, while sitting in their favorite cafe, Lucas and Liam shared a quiet moment. They reflected on how much they " +
        "had been through together and how their friendship had been the cornerstone of their lives. They smiled, knowing that " +
        "they had truly been there for each other through thick and thin.\n\n" +

        "Through all the highs and lows, Lucas and Liam knew one thing for certain: their friendship was irreplaceable. It was " +
        "the kind of bond that could never be broken, no matter how far apart they might be or how much time passed.\n\n" +

        "And so, as they grew older, their friendship remained a lasting testament to the power of loyalty, support, and love " +
        "that can only be found in the truest of friendships.",

        // Sword Art Online Story
        "                 THE WORLD OF SWORD ART\n\n" +

        "Kirito, a seasoned gamer, had been eagerly waiting for the release of Sword Art Online, a new VRMMORPG that promised " +
        "to take players into a world unlike any other. Upon logging in for the first time, he was amazed by the breathtaking " +
        "landscape and the sense of freedom the game offered. But what was supposed to be an escape from reality quickly turned " +
        "into a nightmare when the creator of the game revealed that they were trapped inside.\n\n" +

        "With no way out, players were told that the only way to escape was to clear all 100 floors of the game's massive " +
        "castle. Kirito, determined to survive, made a pact with other players, including Asuna, a skilled warrior, and " +
        "together, they fought their way through the labyrinthine levels of Aincrad, each floor more difficult than the last.\n\n" +

        "As they progressed, Kirito and Asuna grew closer, their bond strengthening with every battle they faced. They shared " +
        "moments of triumph and despair, learning to trust each other completely. Together, they made a promise to escape the " +
        "game and return to the real world, no matter what the cost.\n\n" +

        "Through the trials and challenges, Kirito's leadership and combat skills proved invaluable. His reputation grew, and " +
        "he became known as the Black Swordsman. Yet, despite his newfound fame, Kirito never lost sight of what truly mattered " +
        "– his friendship with Asuna and the promise they made to each other.\n\n" +

        "However, the journey was not without its sacrifices. Along the way, Kirito and his friends faced heart-wrenching losses " +
        "that tested their resolve. As they ascended the floors, they realized that every victory came at a price. But despite " +
        "the hardships, they never gave up.\n\n" +

        "As they reached the final floor, the battle with the game’s creator awaited them. It was the ultimate test of their " +
        "strength, resolve, and unity. With the fate of all players hanging in the balance, Kirito and Asuna fought side by " +
        "side, determined to defeat the creator and free everyone trapped in the game.\n\n" +

        "In the end, Kirito and Asuna were victorious. The game was cleared, and the players were freed from their digital " +
        "prison. However, their time in Sword Art Online had changed them forever. They had forged unbreakable bonds and " +
        "discovered the true meaning of friendship, courage, and perseverance.\n\n" +

        "Though their adventure in the virtual world ended, Kirito and Asuna's journey was far from over. They left Sword Art " +
        "Online with new strength, a deeper understanding of themselves, and a lifelong bond that would carry them through the " +
        "challenges of the real world.\n\n" +

        "Kirito and Asuna knew that no matter where life took them, their friendship and love for each other would remain their " +
        "greatest treasure.",

        // Drama Story
        "                 THE FINAL CHOICE\n\n" +

        "Leah had always been the glue that held her family together. A loving wife and mother, she spent her days caring " +
        "for her children, managing the household, and supporting her husband's career. But as time passed, Leah began to feel " +
        "a growing sense of dissatisfaction. She loved her family deeply, but she longed for something more – a sense of " +
        "purpose beyond the daily routine.\n\n" +

        "Her husband, James, had always been supportive, but he was consumed by his work. The more Leah yearned for change, " +
        "the more distant they became. She felt as if she were living a life she hadn't chosen for herself, and the weight of " +
        "this realization began to take a toll on her mental and emotional well-being.\n\n" +

        "One day, while attending a support group for women in similar situations, Leah met someone who changed everything. " +
        "Ryan, a fellow participant, had been through similar struggles and had found the courage to pursue his dreams. His " +
        "story inspired Leah, and for the first time in years, she felt a spark of hope. Ryan's encouragement gave her the " +
        "strength to start making changes in her own life.\n\n" +

        "As Leah began to explore her passions and take steps toward a new career, she found herself at a crossroads. Her " +
        "relationship with James had deteriorated, and she was forced to confront the possibility that their marriage might " +
        "becoming unrepairable. She loved him, but she also loved herself, and she needed to rediscover who she was.\n\n" +

        "The tension in their marriage reached its breaking point, and Leah made the difficult decision to seek counseling. " +
        "James, though resistant at first, eventually agreed to work on their relationship. It was a slow and painful process, " +
        "but through it, Leah began to realize that she had the power to make choices that were best for her.\n\n" +

        "Over time, Leah began to rebuild herself. She pursued her dreams and found fulfillment outside of her marriage, " +
        "while also strengthening her relationship with her children. She learned that self-care was not selfish but necessary " +
        "for her well-being.\n\n" +

        "James, too, began to change. Through the counseling, he rediscovered his love for Leah and learned how to be more " +
        "present in their relationship. Together, they worked through their issues and found a new understanding of one another.\n\n" +

        "As Leah navigated this new chapter of her life, she found peace in the realization that she had the power to shape " +
        "her future. She no longer felt trapped by circumstances but instead felt empowered to create the life she truly " +
        "wanted.\n\n" +

        "Leah's journey had been difficult, but it had led her to a place of self-discovery, love, and freedom. She had learned " +
        "that the most important relationship was the one with herself, and that only by nurturing that could she truly " +
        "nurture her family and her future."



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

                // Check for key phrases from the original story
                string[] keyPhrases = { 
                    // Love Story
                    "Lucas", "Liam", "childhood friends", "support", "adventure", "unbreakable bond", "rebuilding", "sacrifice", "fame", "memories",
                    "loyalty", "journey", "family bond", "back together", "forgiveness", "trust", "personal growth", "healing", "life-changing event",
                    // Anime/Manga Story
                    "Kirito", "Asuna", "Aincrad", "virtual world", "trapped", "freedom", "Zanpakutō", "Bankai", "Hollow", "combat",
                    "level up", "reality vs. virtual", "survival", "teamwork", "rescue", "fellow players", "boss fight", "guilds", "strategies",
                    // Personal Development
                    "Leah", "family", "change", "self-discovery", "rebuilding", "marriage", "career", "dreams", "sacrifice", "balance",
                    "emotional turmoil", "unexpected twist", "courage", "identity crisis", "redemption", "future plans", "dream fulfillment",
                    "life-altering choice", "truth revealed"
                };
                string[] criticalPhrases = {
                    "tragedy", "heartbreaking loss", "overcoming challenges", "building dreams", "strength in unity", "never giving up",
                    "long-lasting friendship", "emotional breakdown", "fateful encounter", "renewed hope", "conflict resolution", "sacrifice",
                    "loyalty tested", "life lessons learned", "family reunion", "life-or-death battle", "rescue mission", "fate of players", "overcoming fears", "ultimate sacrifice", "victory", "escape",
                    "love", "teamwork", "game-breaking event", "sacrificial decision", "final showdown", "friendship forged in battle",
                    "cliffhanger", "triumph against odds", "lost lives", "emotional reunion","life-changing decisions", "conflict", "marriage struggles", "finding purpose", "personal growth", "new beginnings",
                    "overcoming adversity", "resilience", "truths uncovered", "life-altering secrets", "decisive moments", "sacrifice for love",
                    "new chapter", "inner strength", "turning point", "catharsis", "rebuilding relationships", "life-altering risk", "emotional crossroads", "new beginnings", "journey of self-rediscovery", "life-changing revelation",
                    "hard decisions", "sacrificial love", "dream vs. reality", "personal sacrifice", "unexpected turn of events", "test of friendship", "accepting flaws", "journey of self-acceptance", "heartfelt goodbyes", "coming full circle"}; // Higher importance
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
            if (!userSummary.Contains("love") && !userSummary.Contains("teamwork") && !userSummary.Contains("growth"))
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
            if (originalStory.Contains("dramatic") && userSummary.Contains("intense") ||
                originalStory.Contains("motivational") && userSummary.Contains("uplifting"))
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

        private void BookSummaryHard_Load(object sender, EventArgs e)
        {
            InitializeContent();
        }
    }
}
