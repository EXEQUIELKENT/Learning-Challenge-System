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
    public partial class BookSummaryEasy : Form
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
        public BookSummaryEasy()
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
                FormTitle = "Book Summary (Easy)",
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
                FormTitle = "Book Summary (Easy)",
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
        // Love Story
        "                       A CHANCE ENCOUNTER\n\n" +

        "Samantha and Ethan’s lives couldn’t be more different. Samantha was a reserved librarian in a bustling city, " +
        "content with her peaceful routines and cherished books. Ethan, on the other hand, was an adventurous photographer " +
        "traveling the world in search of his next big story. Their worlds collided when Ethan stumbled into Samantha’s library " +
        "to escape a sudden downpour. With his camera dripping wet and a sheepish grin, he asked for a book recommendation.\n\n" +

        "Curious yet cautious, Samantha hesitantly offered a novel about adventure. Ethan, amused by the irony, " +
        "engaged her in conversation, finding her quiet passion for literature captivating. Over the weeks, Ethan kept returning " +
        "to the library, always with a new excuse to talk to Samantha. She found herself drawn to his free-spirited nature, " +
        "while he admired her intellect and warmth. Their unlikely friendship blossomed into something deeper.\n\n" +

        "When Ethan received an offer to photograph in another country, he hesitated for the first time in his career. " +
        "Unable to hide his feelings, he confessed to Samantha that he wanted her to be part of his journey. " +
        "She was torn between her comfortable life and the possibility of something extraordinary. Finally, Samantha chose " +
        "to take a leap of faith, realizing that love, like the stories she loved, was meant to be lived fully. Together, " +
        "they embarked on a new adventure, finding joy in every moment shared, proving that opposites truly attract.",

        // Anime/Manga Story
        "                       SLAM DUNK\n\n" +

        "Sakuragi Hanamichi, a notorious delinquent and self-proclaimed ladies' man, joined Shohoku High School's basketball team " +
        "for one reason: to impress the beautiful Haruko Akagi. Unaware of his raw athletic potential, Sakuragi struggled to grasp " +
        "the basics of basketball. His clumsiness on the court earned him scorn from teammates and opponents alike, but his stubborn " +
        "determination set him apart. With every mistake, he vowed to improve, driven by Haruko’s encouragement and his desire to " +
        "prove himself.\n\n" +

        "As the team prepared for the inter-high tournament, Sakuragi faced off against Kaede Rukawa, a basketball prodigy " +
        "and his rival for Haruko’s attention. Despite their animosity, the two developed a grudging respect as teammates, " +
        "pushing each other to reach new heights. Shohoku’s coach, Anzai, recognized Sakuragi’s untapped talent and began " +
        "mentoring him, instilling the discipline needed to channel his energy effectively.\n\n" +

        "Through grueling practices and intense matches, Sakuragi transformed from a novice to a formidable player. " +
        "His rebounds became legendary, earning him the nickname ‘Rebound King.’ Shohoku’s team, once an underdog, " +
        "began defeating stronger opponents. Along the way, Sakuragi learned the true meaning of teamwork, resilience, " +
        "and passion for the game. Basketball became more than a means to impress Haruko—it became his calling, " +
        "solidifying his place in the world of sports and in the hearts of his teammates.",

        // Personal Development
        "                       RISING ABOVE\n\n" +

        "John had always been an average student, blending into the background while others excelled. " +
        "He doubted his abilities and often avoided challenges, convinced that failure was inevitable. " +
        "One day, his teacher assigned the class to give a speech on their biggest dream. The thought terrified John, " +
        "but deep inside, he knew this was a chance to prove himself. Determined, he spent weeks preparing, " +
        "practicing in front of a mirror and seeking advice from supportive friends.\n\n" +

        "On the day of the presentation, John’s hands shook as he stepped onto the stage. " +
        "Taking a deep breath, he began to speak. To his surprise, his passion for his dream—becoming a writer—shone through. " +
        "The audience responded with applause, boosting his confidence. That moment marked a turning point for John. " +
        "He realized that growth required stepping out of his comfort zone and embracing challenges.\n\n" +

        "From then on, John set small, achievable goals to build his skills. He joined a writing club, " +
        "submitted essays to contests, and even started a blog. With each success, his confidence grew, and his fear of failure diminished. " +
        "John’s journey taught him that personal development isn’t about perfection but about perseverance and self-belief. " +
        "He became a testament to the idea that with determination and courage, anyone can rise above their doubts and achieve greatness."


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
                    "adventure", "librarian", "library", "Samantha", "Ethan", "books", "photographer", "free-spirited", "love", "journey",
                    // Anime/Manga Story
                    "Sakuragi", "Hanamichi", "basketball", "Haruko", "Akagi", "Shohoku", "Rukawa", "teamwork", "rebound", "Rebound King", "Anzai", "inter-high",
                    // Personal Development
                    "John", "student", "dream", "speech", "writer", "growth", "comfort zone", "writing", "club", "determination", "courage", "confidence"
                };
                string[] criticalPhrases = { "love", "teamwork", "growth", "rebound", "journey" }; // Higher importance
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
        private void BookSummaryEasy_Load(object sender, EventArgs e)
        {
            InitializeContent();
        }
    }
}
