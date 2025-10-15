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
    public partial class CharacterAnalysisHard : Form
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
        public CharacterAnalysisHard()
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
                FormTitle = "Character Anlysis (Hard)",
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
                FormTitle = "Character Analysis (Hard)",
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
        // Anime Story: Kenichi, The Mightiest Disciple
        "                       THE JOURNEY OF THE MIGHTY\n\n" +
        "Kenichi Shirahama was an ordinary high school student, always bullied and weak, trying to live a peaceful life. " +
        "However, his life takes a dramatic turn when he meets Miu Furinji, the granddaughter of the master of Ryozanpaku, a dojo with a " +
        "collection of martial arts experts. Miu introduces Kenichi to the dojo, and though initially reluctant, he starts learning martial arts " +
        "from the dojo's powerful masters.\n\n" +

        "Kenichi's journey in martial arts is full of struggles as he faces enemies who test his limits. Despite being far from the best, his " +
        "determination to protect those he cares about pushes him to train harder each day. As he grows stronger, he earns the respect of his peers " +
        "and begins to rise above his former self.\n\n" +

        "The dojo is home to several powerful fighters: Akisame, the martial arts doctor, and the undefeated boxer Kensei Ma. Each master has their " +
        "own unique skills, and they guide Kenichi in their own way. Though Kenichi often doubts his abilities, his bond with Miu grows stronger, " +
        "and he starts gaining confidence.\n\n" +

        "Kenichi's growth is constantly tested when he faces the Ryozanpaku's enemies: the powerful and dangerous members of the 'Evil Fist' " +
        "group. They see Kenichi as a mere beginner, unworthy of standing against their strength. But as he endures hardship, his skills improve, and " +
        "he earns victories that showcase his growth.\n\n" +

        "Alongside his physical training, Kenichi also learns the importance of character and resilience. He comes to understand that real strength " +
        "lies not just in physical power but in the will to never give up, no matter how tough the situation becomes. His training is more than just " +
        "about fighting—it’s about his personal growth and how he becomes a protector of the people around him.\n\n" +

        "Kenichi's bond with Miu is key to his journey. Despite her incredible martial arts prowess, Miu remains humble and often acts as Kenichi's " +
        "emotional support. Together, they face countless challenges, including fierce battles against powerful enemies who threaten everything they hold dear.\n\n" +

        "As Kenichi grows stronger, he begins to confront not only his external enemies but also his internal fears and doubts. The more he " +
        "learns, the more he realizes that he must fight not just for himself but for the safety of others.\n\n" +

        "In the end, Kenichi’s perseverance and determination make him a true disciple of Ryozanpaku. He continues to grow, finding strength " +
        "through his bonds with his teachers, Miu, and the experiences that have shaped him. His journey is far from over, but Kenichi now stands " +
        "as a true martial artist, ready to face any challenge that comes his way.",

        // Revenge Story
        "                       THE REVENGE OF THE BETRAYED\n\n" +
        "Riku had always been a loyal and trustworthy member of his clan, serving his lord faithfully. But everything changed the night he was " +
        "betrayed by those closest to him. His trusted companion, Takumi, a fellow warrior, framed Riku for a crime he did not commit, causing " +
        "him to be exiled and hunted by his own people.\n\n" +

        "Riku spent years in hiding, nursing his wounds and planning his revenge. With each passing day, his desire for vengeance grew stronger. " +
        "He honed his skills in the wilderness, learning to survive and become an even greater warrior. His body became as sharp as his mind, " +
        "and his hatred for Takumi burned brighter.\n\n" +

        "After years of training, Riku was finally ready to confront the man who betrayed him. But revenge was not as simple as he had imagined. " +
        "Takumi had risen through the ranks, becoming a trusted advisor to the very lord who once believed in Riku’s loyalty. His journey to " +
        "avenge his honor was more complex than a mere battle; it was a quest for justice.\n\n" +

        "Riku began to gather allies, people who had suffered under the tyranny of Takumi and his corrupt influence. Together, they planned " +
        "an assault on the fortress where Takumi lived, knowing that it would be a battle not just of swords but of hearts and wills.\n\n" +

        "Along the way, Riku encountered numerous obstacles—traps set by Takumi’s spies, and betrayals from those he once trusted. But each " +
        "setback only fueled Riku’s determination. He realized that revenge wasn’t just about killing Takumi; it was about restoring honor and " +
        "justice to the people who had been wronged.\n\n" +

        "Riku’s quest took him through dangerous lands, where he faced dark forces and ruthless bandits. Yet, his resolve remained unshaken. " +
        "He knew that the final confrontation with Takumi would be a test of everything he had become. It wasn’t just about winning; it was " +
        "about reclaiming the life that was stolen from him.\n\n" +

        "When Riku finally confronted Takumi, the two men clashed in a brutal battle, their swords singing in the night air. Takumi, though " +
        "skilled, was not prepared for the fury of a man who had been wronged and betrayed. As the fight reached its peak, Riku found himself " +
        "questioning whether taking Takumi’s life would bring him the peace he so desperately sought.\n\n" +

        "In the end, Riku did not kill Takumi. Instead, he exposed his betrayal to the world, showing the people the truth behind Takumi’s " +
        "rise to power. Though Takumi was stripped of his status, Riku found that revenge did not offer the closure he sought. The journey had " +
        "changed him, and he realized that his true purpose was to rebuild and protect the honor of those he had lost.\n\n" +

        "Riku’s story ended not with vengeance but with redemption. His legacy lived on as a warrior who sought justice above all, and " +
        "through his actions, he restored peace to the land that had been torn apart by corruption and betrayal.",

        // The Forest Game Story
        "                       THE FOREST\n\n" +
        "The forest was a mysterious place, its depths unknown and filled with ancient legends. It was said that those who ventured too deep " +
        "could become lost forever. Yet, for the people of the nearby village, the forest was both a source of fear and fascination. Every year, " +
        "the village would send a group of brave souls into the heart of the forest to retrieve a rare flower said to possess magical powers.\n\n" +

        "Among the chosen few this year was a young adventurer named Lian, eager to prove his skills and uncover the secrets of the forest. " +
        "Though others feared the forest’s enchantments, Lian was determined to face whatever challenges awaited. He gathered his belongings, " +
        "and with his companions, set off toward the unknown.\n\n" +

        "The deeper they went into the forest, the more the atmosphere changed. The trees grew taller and thicker, their roots twisting " +
        "like serpents beneath their feet. Strange creatures watched them from the shadows, and the sounds of the forest seemed to grow louder. " +
        "At night, the group huddled together for warmth and protection, fearing the dangers that lurked in the dark.\n\n" +

        "As they ventured deeper into the forest, Lian began to notice strange occurrences. The trees seemed to shift, and whispers echoed " +
        "through the air. His companions grew uneasy, questioning whether they should turn back. But Lian, driven by his desire to succeed, " +
        "pressed forward.\n\n" +

        "Suddenly, they found themselves trapped in a clearing, surrounded by an eerie silence. It was as if the forest itself had closed in on them. " +
        "Lian was the only one who remained calm, his instincts telling him that the key to escaping lay in understanding the forest’s true nature. " +
        "He realized that the forest was not a mere collection of trees but a living entity with a will of its own.\n\n" +

        "Using his knowledge of ancient folklore, Lian deciphered the forest’s riddle and discovered that the flower they sought was hidden " +
        "within the heart of the forest, protected by its very magic. However, retrieving the flower would require them to confront their " +
        "deepest fears and test their strength.\n\n" +

        "With courage in his heart, Lian led the group to the heart of the forest, where they encountered the guardian of the flower—a " +
        "majestic creature with the body of a wolf and the wings of an eagle. It was a fierce battle, but Lian and his companions fought " +
        "valiantly, proving their worthiness.\n\n" +

        "In the end, Lian was able to retrieve the flower, but the true lesson he learned was not about the power of the flower but about " +
        "the strength within himself and his companions. The forest had tested them, but they had emerged stronger and wiser.\n\n" +

        "Returning to the village, Lian’s victory was celebrated, but he knew that the forest’s magic would forever remain a mystery. " +
        "Though they had succeeded in their quest, the forest still held secrets that no one would ever fully understand, and Lian’s journey " +
        "had just begun.",
        };

        private List<Question> questions1 = new List<Question>
        {
            new Question { QuestionText = "What was the relationship between Kenichi and Miu?", A = "They were rivals", B = "They were friends", C = "They were siblings", D = "They were enemies", CorrectAnswer = "B" },
            new Question { QuestionText = "Who taught Kenichi martial arts?", A = "Miu", B = "Higure", C = "The Dojo Masters", D = "The disciples of Ryozanpaku", CorrectAnswer = "D" },
            new Question { QuestionText = "What was Kenichi’s greatest motivation to become stronger?", A = "To avenge his parents", B = "To protect Miu", C = "To defeat his rivals", D = "To become the strongest disciple", CorrectAnswer = "B" },
            new Question { QuestionText = "Who was Kenichi’s primary rival?", A = "Takeda", B = "Berserker", C = "Furinji", D = "Shigure", CorrectAnswer = "B" },
            new Question { QuestionText = "What martial arts style did Kenichi first learn?", A = "Karate", B = "Aikido", C = "Kung Fu", D = "Kempo", CorrectAnswer = "D" },
            new Question { QuestionText = "Who was the leader of the Ryozanpaku dojo?", A = "Kenichi", B = "Miu", C = "Satsujin", D = "Furinji", CorrectAnswer = "D" },
            new Question { QuestionText = "What was Kenichi’s main flaw at the start of his journey?", A = "He was too arrogant", B = "He was overly sensitive", C = "He lacked confidence", D = "He didn’t train enough", CorrectAnswer = "C" },
            new Question { QuestionText = "What type of combatants did Kenichi have to face in the story?", A = "Superpowered fighters", B = "Criminals", C = "Powerful martial artists", D = "Monsters", CorrectAnswer = "C" },
            new Question { QuestionText = "How did Kenichi feel about Miu at the start?", A = "He was indifferent", B = "He admired her", C = "He hated her", D = "He was scared of her", CorrectAnswer = "B" },
            new Question { QuestionText = "What is the name of the group that threatens the peace in Kenichi’s world?", A = "The 12 Asura", B = "The Northern Fist", C = "The Ryozanpaku", D = "The Eight Fists of the Apocalypse", CorrectAnswer = "A" },
            new Question { QuestionText = "Which character is a weapon expert in the series?", A = "Miu", B = "Shigure", C = "Berserker", D = "Kenichi", CorrectAnswer = "B" },
            new Question { QuestionText = "What lesson did Kenichi learn from his training?", A = "Power is everything", B = "Hard work and persistence lead to growth", C = "Running away is always an option", D = "Strength is achieved through cruelty", CorrectAnswer = "B" },
            new Question { QuestionText = "What motivates Kenichi to continue his training despite the hardships?", A = "His fear of enemies", B = "His love for Miu", C = "His desire for revenge", D = "His desire to become famous", CorrectAnswer = "B" },
            new Question { QuestionText = "Who is the female martial artist who specializes in assassination techniques?", A = "Shigure", B = "Miu", C = "Satsujin", D = "Higure", CorrectAnswer = "A" },
            new Question { QuestionText = "Who is the strong student from the rival dojo?", A = "Ryozanpaku", B = "Takeda", C = "Furinji", D = "Kobushi", CorrectAnswer = "B" },
            new Question { QuestionText = "Which character was originally Kenichi’s biggest source of fear?", A = "Takeda", B = "The Dojo Masters", C = "Miu", D = "Berserker", CorrectAnswer = "D" },
            new Question { QuestionText = "What was Kenichi’s first true martial arts competition?", A = "Fight Club", B = "Rivalry Tournament", C = "The Martial Arts Competition", D = "The Masters' Test", CorrectAnswer = "C" },
            new Question { QuestionText = "What is Kenichi’s ultimate goal by the end of the story?", A = "To defeat every rival", B = "To open his own dojo", C = "To protect those he cares about", D = "To achieve legendary status", CorrectAnswer = "C" },
            new Question { QuestionText = "Who helps Kenichi develop his combat strategies?", A = "Satsujin", B = "Miu", C = "Shigure", D = "Furinji", CorrectAnswer = "D" },
            new Question { QuestionText = "What is Kenichi’s greatest strength as a martial artist?", A = "Speed", B = "Power", C = "Endurance", D = "Resilience and perseverance", CorrectAnswer = "D" },
            new Question { QuestionText = "What drives Kenichi to fight against evil organizations?", A = "His search for power", B = "His love for justice", C = "His promise to protect Miu", D = "His personal vendetta", CorrectAnswer = "B" },
            new Question { QuestionText = "What was Kenichi’s initial reaction to training at the dojo?", A = "He was excited", B = "He was reluctant", C = "He was confident", D = "He was angry", CorrectAnswer = "B" },
            new Question { QuestionText = "What is the name of the dojo where Kenichi trains?", A = "Ryozanpaku", B = "Takeda Dojo", C = "Northern Fist Dojo", D = "Martial House", CorrectAnswer = "A" },
            new Question { QuestionText = "Who was the strongest master in Ryozanpaku?", A = "Miu", B = "Shigure", C = "Akisame", D = "Hayato Furinji", CorrectAnswer = "D" },
            new Question { QuestionText = "What kind of person was Kenichi before learning martial arts?", A = "A delinquent", B = "A popular athlete", C = "A weak and bullied student", D = "A genius", CorrectAnswer = "C" },
            new Question { QuestionText = "What group constantly threatens Kenichi?", A = "The Dark Hand", B = "The Evil Fist", C = "The Red Claw", D = "The Shadow League", CorrectAnswer = "B" },
            new Question { QuestionText = "Who among the masters specializes in weapons?", A = "Kensei Ma", B = "Shigure", C = "Akisame", D = "Furinji", CorrectAnswer = "B" },
            new Question { QuestionText = "How does Kenichi’s personality change as he trains?", A = "He becomes more arrogant", B = "He gains confidence and courage", C = "He becomes more fearful", D = "He loses interest in martial arts", CorrectAnswer = "B" },
            new Question { QuestionText = "What does Kenichi fear the most?", A = "Fighting strong opponents", B = "Losing to his rivals", C = "Failing to protect his friends", D = "Disappointing Miu", CorrectAnswer = "C" },
            new Question { QuestionText = "What was Miu’s role in Kenichi’s journey?", A = "She was his rival", B = "She was his trainer", C = "She was his emotional support and inspiration", D = "She was his opponent", CorrectAnswer = "C" },
            new Question { QuestionText = "How does Kenichi earn the respect of his masters?", A = "By defeating them", B = "Through perseverance and dedication", C = "By bribing them", D = "By running away", CorrectAnswer = "B" },
            new Question { QuestionText = "Who was the first strong opponent Kenichi faced?", A = "Takeda", B = "Berserker", C = "Koga", D = "Higure", CorrectAnswer = "A" },
            new Question { QuestionText = "What was Kenichi’s main struggle while training?", A = "His lack of motivation", B = "His physical weakness", C = "His lack of discipline", D = "His inability to follow orders", CorrectAnswer = "B" },
            new Question { QuestionText = "What lesson did Akisame, the martial arts doctor, emphasize?", A = "Pain is weakness leaving the body", B = "A true martial artist protects others", C = "Power determines strength", D = "Defeat your enemies at all costs", CorrectAnswer = "B" },
            new Question { QuestionText = "How does Kenichi’s training affect his school life?", A = "He becomes a delinquent", B = "He gains confidence and new friends", C = "He drops out of school", D = "He starts bullying others", CorrectAnswer = "B" },
            new Question { QuestionText = "Who is Kenichi’s biggest challenge in the Evil Fist?", A = "Berserker", B = "Takeda", C = "Ragnarok’s Leader", D = "Shigure", CorrectAnswer = "C" },
            new Question { QuestionText = "What does Kenichi ultimately realize about martial arts?", A = "It is about strength and domination", B = "It is about discipline and protecting others", C = "It is only for self-defense", D = "It is a way to become famous", CorrectAnswer = "B" },
            new Question { QuestionText = "How do the Ryozanpaku masters view Kenichi?", A = "As a lost cause", B = "As their strongest fighter", C = "As a promising disciple", D = "As a weakling", CorrectAnswer = "C" },
            new Question { QuestionText = "What is Kenichi’s biggest accomplishment by the end of the story?", A = "Defeating the strongest enemy", B = "Overcoming his fears and growing stronger", C = "Becoming a master himself", D = "Leaving Ryozanpaku", CorrectAnswer = "B" },
            new Question { QuestionText = "What does Kenichi believe is his ultimate strength?", A = "Speed", B = "Strength", C = "Endurance", D = "His determination and will to protect others", CorrectAnswer = "D" }

        };

        private List<Question> questions2 = new List<Question>
        {
            new Question { QuestionText = "Who was the protagonist of the revenge story?", A = "Max", B = "Sam", C = "John", D = "Evan", CorrectAnswer = "A" },
            new Question { QuestionText = "What tragic event sparked the protagonist’s desire for revenge?", A = "The loss of his home", B = "The death of a loved one", C = "A betrayal", D = "Theft of his money", CorrectAnswer = "B" },
            new Question { QuestionText = "Who was responsible for the tragedy in the protagonist’s life?", A = "His best friend", B = "A criminal gang", C = "A corrupt official", D = "A rival business", CorrectAnswer = "C" },
            new Question { QuestionText = "What was the protagonist’s main goal throughout the story?", A = "To find peace", B = "To gather allies", C = "To avenge his loved one", D = "To escape his past", CorrectAnswer = "C" },
            new Question { QuestionText = "How did the protagonist prepare for his revenge?", A = "He trained physically", B = "He gathered information", C = "He used his connections", D = "He waited for the right moment", CorrectAnswer = "B" },
            new Question { QuestionText = "Who did the protagonist rely on for help?", A = "A former soldier", B = "A mysterious informant", C = "His family", D = "His old mentor", CorrectAnswer = "B" },
            new Question { QuestionText = "What was the protagonist’s ultimate weapon in the story?", A = "A firearm", B = "A hidden bomb", C = "A letter of exposure", D = "His intelligence and strategy", CorrectAnswer = "D" },
            new Question { QuestionText = "How did the protagonist feel towards his enemy?", A = "Indifferent", B = "Fearful", C = "Hate", D = "Sympathy", CorrectAnswer = "C" },
            new Question { QuestionText = "What was the final outcome of the protagonist’s revenge?", A = "He succeeded, but at a great cost", B = "He failed and was caught", C = "He realized revenge wasn’t the answer", D = "He was forgiven by his enemy", CorrectAnswer = "A" },
            new Question { QuestionText = "What lesson did the protagonist learn by the end?", A = "Revenge is sweet", B = "Forgiveness is the key", C = "Revenge doesn’t bring peace", D = "Power is everything", CorrectAnswer = "C" },
            new Question { QuestionText = "What did the protagonist regret about his pursuit of revenge?", A = "The people he hurt along the way", B = "The time he wasted", C = "Not killing his enemy sooner", D = "The alliances he made", CorrectAnswer = "A" },
            new Question { QuestionText = "Who was the protagonist’s most trusted ally during his quest?", A = "His childhood friend", B = "His lover", C = "The mysterious informant", D = "A rival gang leader", CorrectAnswer = "C" },
            new Question { QuestionText = "How did the protagonist feel after achieving his revenge?", A = "Empty and unfulfilled", B = "Relieved and happy", C = "Proud and accomplished", D = "Shocked by the outcome", CorrectAnswer = "A" },
            new Question { QuestionText = "What did the antagonist seek to gain from their actions?", A = "Power", B = "Wealth", C = "Revenge", D = "Control", CorrectAnswer = "D" },
            new Question { QuestionText = "How did the antagonist react when the protagonist finally confronted them?", A = "They were defensive", B = "They were apologetic", C = "They were angry", D = "They were indifferent", CorrectAnswer = "C" },
            new Question { QuestionText = "What happened to the protagonist at the end of the story?", A = "He became a hero", B = "He left everything behind", C = "He was consumed by his revenge", D = "He rebuilt his life", CorrectAnswer = "C" },
            new Question { QuestionText = "Who was the first person to betray the protagonist?", A = "A business partner", B = "His family", C = "A trusted friend", D = "A colleague", CorrectAnswer = "A" },
            new Question { QuestionText = "What did the protagonist use to expose his enemy?", A = "A public trial", B = "A powerful speech", C = "A secret file", D = "A video recording", CorrectAnswer = "C" },
            new Question { QuestionText = "What was Riku's role in his clan before being betrayed?", A = "A scout", B = "A warrior", C = "A healer", D = "A strategist", CorrectAnswer = "B" },
            new Question { QuestionText = "Who betrayed Riku and framed him for a crime?", A = "Takumi", B = "Hiro", C = "Jiro", D = "Daichi", CorrectAnswer = "A" },
            new Question { QuestionText = "What happened to Riku after he was framed?", A = "He was imprisoned", B = "He was executed", C = "He was exiled", D = "He was promoted", CorrectAnswer = "C" },
            new Question { QuestionText = "How did Riku spend his years in exile?", A = "He built a new life", B = "He trained in the wilderness", C = "He tried to forget his past", D = "He found another clan", CorrectAnswer = "B" },
            new Question { QuestionText = "What was Riku's ultimate goal after being betrayed?", A = "To regain his honor", B = "To kill Takumi", C = "To return to his clan", D = "To find peace", CorrectAnswer = "A" },
            new Question { QuestionText = "Who was Takumi in relation to Riku before the betrayal?", A = "His mentor", B = "His best friend", C = "His rival", D = "His younger brother", CorrectAnswer = "B" },
            new Question { QuestionText = "Why was revenge not as simple as Riku had imagined?", A = "Takumi had powerful allies", B = "Riku had lost his skills", C = "Riku had no weapons", D = "Takumi had disappeared", CorrectAnswer = "A" },
            new Question { QuestionText = "What did Riku do before confronting Takumi?", A = "He gathered allies", B = "He studied Takumi’s weaknesses", C = "He trained in secret", D = "All of the above", CorrectAnswer = "D" },
            new Question { QuestionText = "What kind of people did Riku gather to help him?", A = "Mercenaries", B = "Other betrayed warriors", C = "Villagers", D = "Former clan members", CorrectAnswer = "B" },
            new Question { QuestionText = "What obstacles did Riku face in his quest for revenge?", A = "Traps set by Takumi", B = "Betrayals from allies", C = "His own doubts", D = "All of the above", CorrectAnswer = "D" },
            new Question { QuestionText = "What lesson did Riku learn during his journey?", A = "Revenge is the only answer", B = "Strength is everything", C = "Justice is more important than vengeance", D = "Hatred makes one stronger", CorrectAnswer = "C" },
            new Question { QuestionText = "Where did Riku and Takumi have their final confrontation?", A = "In the wilderness", B = "At the fortress", C = "At the royal palace", D = "In a sacred temple", CorrectAnswer = "B" },
            new Question { QuestionText = "What was the outcome of Riku’s fight with Takumi?", A = "Riku killed him", B = "Riku lost", C = "Riku exposed his betrayal", D = "Takumi escaped", CorrectAnswer = "C" },
            new Question { QuestionText = "How did Riku expose Takumi’s betrayal?", A = "Through written proof", B = "By revealing a secret witness", C = "By showing evidence in court", D = "By forcing Takumi to confess", CorrectAnswer = "D" },
            new Question { QuestionText = "What happened to Takumi after his betrayal was exposed?", A = "He was executed", B = "He was exiled", C = "He was imprisoned and lost his status", D = "He fled the country", CorrectAnswer = "C" },
            new Question { QuestionText = "Why did Riku choose not to kill Takumi?", A = "He realized revenge wouldn’t bring him peace", B = "Takumi begged for mercy", C = "His allies stopped him", D = "He wanted Takumi to suffer", CorrectAnswer = "A" },
            new Question { QuestionText = "What was Riku’s new purpose after his quest for revenge?", A = "To become a leader", B = "To protect his people and restore justice", C = "To travel the world", D = "To live in solitude", CorrectAnswer = "B" },
            new Question { QuestionText = "What was the final lesson of Riku’s story?", A = "Power is the only way to survive", B = "Vengeance is not the path to peace", C = "Never trust anyone", D = "The strong always win", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Riku leave behind as his legacy?", A = "A sword of vengeance", B = "A restored land and a tale of justice", C = "A powerful new army", D = "A book of his experiences", CorrectAnswer = "B" },
            new Question { QuestionText = "What message does Riku’s story convey?", A = "That betrayal always leads to war", B = "That justice is more important than revenge", C = "That hatred makes a person stronger", D = "That revenge should always be taken", CorrectAnswer = "B" }

        };

        private List<Question> questions3 = new List<Question>
        {
            new Question { QuestionText = "Who was the protagonist of 'The Forest Game'?", A = "Emily", B = "Alex", C = "Jason", D = "Megan", CorrectAnswer = "B" },
            new Question { QuestionText = "What unusual event happens when Alex enters the forest?", A = "He meets a strange creature", B = "He finds a hidden treasure", C = "He gets trapped in a game", D = "He discovers a hidden village", CorrectAnswer = "C" },
            new Question { QuestionText = "What is the main goal of the game Alex is forced to play?", A = "To find the hidden treasure", B = "To escape the forest", C = "To defeat mythical creatures", D = "To survive the wilderness", CorrectAnswer = "B" },
            new Question { QuestionText = "Who does Alex meet in the forest?", A = "A mysterious guide", B = "His childhood friend", C = "A team of adventurers", D = "A talking animal", CorrectAnswer = "A" },
            new Question { QuestionText = "What is the forest game designed to do?", A = "Test the bravery of its players", B = "Punish trespassers", C = "Trap the players forever", D = "Help players find themselves", CorrectAnswer = "A" },
            new Question { QuestionText = "What happens to the players who lose the game?", A = "They are free to leave", B = "They are turned into statues", C = "They are sent to another dimension", D = "They are trapped forever", CorrectAnswer = "D" },
            new Question { QuestionText = "Who does Alex form an alliance with during the game?", A = "Another player", B = "A magical creature", C = "His sibling", D = "The mysterious guide", CorrectAnswer = "A" },
            new Question { QuestionText = "What was the ultimate challenge Alex faced in the game?", A = "Defeating a guardian beast", B = "Crossing a dangerous river", C = "Solving a series of puzzles", D = "Finding a key to the exit", CorrectAnswer = "C" },
            new Question { QuestionText = "What role does the mysterious guide play in the game?", A = "A helper who provides clues", B = "An antagonist who tries to stop Alex", C = "A neutral observer", D = "A creator of the game", CorrectAnswer = "A" },
            new Question { QuestionText = "What is the forest known for in the story?", A = "Its dangerous wildlife", B = "Its endless size", C = "Its mystical powers", D = "Its hidden treasures", CorrectAnswer = "C" },
            new Question { QuestionText = "What lesson does Alex learn by the end of the story?", A = "The importance of teamwork", B = "That no game is worth winning at all costs", C = "That trust is a powerful weapon", D = "That the forest is a place of redemption", CorrectAnswer = "B" },
            new Question { QuestionText = "Who does Alex end up saving during the game?", A = "His sibling", B = "The mysterious guide", C = "A fellow player", D = "A mystical creature", CorrectAnswer = "C" },
            new Question { QuestionText = "How does Alex escape the forest?", A = "By defeating the game master", B = "By finding the exit key", C = "By surviving until the end", D = "By solving the final puzzle", CorrectAnswer = "C" },
            new Question { QuestionText = "What strange power does the forest possess?", A = "The ability to read minds", B = "The ability to control time", C = "The ability to create illusions", D = "The ability to manipulate the weather", CorrectAnswer = "C" },
            new Question { QuestionText = "Who is the antagonist in 'The Forest Game'?", A = "The mysterious guide", B = "A rival player", C = "A forest deity", D = "The game master", CorrectAnswer = "D" },
            new Question { QuestionText = "What is the outcome of the forest game for Alex?", A = "He wins and leaves the game", B = "He is trapped forever", C = "He decides to stay in the forest", D = "He becomes the new game master", CorrectAnswer = "A" },
            new Question { QuestionText = "What challenge does Alex face near the end of the game?", A = "A final puzzle", B = "A deadly creature", C = "A betrayal from his ally", D = "A moral decision", CorrectAnswer = "D" },
            new Question { QuestionText = "How does Alex feel when he first enters the forest?", A = "Excited", B = "Fearful", C = "Indifferent", D = "Curious", CorrectAnswer = "B" },
            new Question { QuestionText = "What important skill does Alex develop during the game?", A = "Survival tactics", B = "Leadership", C = "Problem-solving", D = "Combat skills", CorrectAnswer = "C" },
            new Question { QuestionText = "Why did the village send people into the forest every year?", A = "To map its terrain", B = "To retrieve a rare flower", C = "To hunt dangerous beasts", D = "To escape from invaders", CorrectAnswer = "B" },
            new Question { QuestionText = "What was special about the flower found in the forest?", A = "It could grant eternal life", B = "It had magical powers", C = "It could summon spirits", D = "It could cure any disease", CorrectAnswer = "B" },
            new Question { QuestionText = "What was Lian’s main goal in entering the forest?", A = "To find treasure", B = "To prove his skills", C = "To become a legend", D = "To test his courage", CorrectAnswer = "B" },
            new Question { QuestionText = "How did the forest change as they went deeper?", A = "It became colder", B = "The trees grew taller and thicker", C = "It became silent", D = "The ground turned into water", CorrectAnswer = "B" },
            new Question { QuestionText = "What unusual things did Lian and his group notice in the forest?", A = "The trees seemed to shift", B = "They could hear whispers", C = "Strange creatures watched them", D = "All of the above", CorrectAnswer = "D" },
            new Question { QuestionText = "Why did Lian’s companions want to turn back?", A = "They got tired", B = "They were afraid of the strange occurrences", C = "They ran out of food", D = "They found another way home", CorrectAnswer = "B" },
            new Question { QuestionText = "What was unusual about the clearing where they got trapped?", A = "It was covered in fog", B = "It was completely silent", C = "It had glowing plants", D = "It had no trees", CorrectAnswer = "B" },
            new Question { QuestionText = "What realization helped Lian understand the forest?", A = "The forest was alive", B = "The forest had a hidden path", C = "The forest was just an illusion", D = "The forest had no exit", CorrectAnswer = "A" },
            new Question { QuestionText = "How did Lian figure out where the flower was hidden?", A = "He followed a map", B = "He used ancient folklore", C = "He asked a spirit", D = "He used a magical compass", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Lian and his companions have to face to retrieve the flower?", A = "A river of fire", B = "Their deepest fears", C = "A labyrinth", D = "A group of bandits", CorrectAnswer = "B" },
            new Question { QuestionText = "What kind of creature was guarding the flower?", A = "A dragon", B = "A wolf with eagle wings", C = "A shadow monster", D = "A giant snake", CorrectAnswer = "B" },
            new Question { QuestionText = "What proved Lian and his companions’ worthiness?", A = "Solving the forest’s riddle", B = "Defeating the guardian in battle", C = "Showing no fear", D = "Bringing a magical artifact", CorrectAnswer = "B" },
            new Question { QuestionText = "What was the true lesson Lian learned?", A = "The power of magic", B = "The strength within himself and his companions", C = "The forest could be controlled", D = "Fear is an illusion", CorrectAnswer = "B" },
            new Question { QuestionText = "What happened when they returned to the village?", A = "They were celebrated for their success", B = "The village was cursed", C = "The flower lost its magic", D = "The forest disappeared", CorrectAnswer = "A" },
            new Question { QuestionText = "What did Lian realize about the forest’s magic?", A = "It could be controlled", B = "It would always remain a mystery", C = "It had disappeared", D = "It was just an illusion", CorrectAnswer = "B" },
            new Question { QuestionText = "Why did Lian believe his journey had just begun?", A = "He wanted to explore more mysteries", B = "The village needed him for another quest", C = "The forest spoke to him", D = "The flower had more secrets", CorrectAnswer = "A" },
            new Question { QuestionText = "What was one of the major themes of the story?", A = "Greed leads to destruction", B = "Courage and teamwork lead to success", C = "Magic is the ultimate power", D = "Isolation makes one stronger", CorrectAnswer = "B" },
            new Question { QuestionText = "How did the forest test those who entered?", A = "By challenging their physical strength", B = "By making them confront their fears", C = "By leading them into endless paths", D = "By trapping them forever", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Lian feel about the forest at the end?", A = "It was a dangerous enemy", B = "It was a living mystery", C = "It was something to be feared", D = "It was an illusion", CorrectAnswer = "B" },
            new Question { QuestionText = "What made Lian different from his companions?", A = "He had magical abilities", B = "He understood the forest’s nature", C = "He was stronger than them", D = "He was fearless", CorrectAnswer = "B" }


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
                        if (score >= 15) // Check if the challenge is passed
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
