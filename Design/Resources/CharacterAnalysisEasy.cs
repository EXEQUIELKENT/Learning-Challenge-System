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
    public partial class CharacterAnalysisEasy : Form
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
        public CharacterAnalysisEasy()
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
                FormTitle = "Character Anlysis (Easy)",
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
                FormTitle = "Character Analysis (Easy)",
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
        "                       JUJUTSU KAISEN\n\n" +
        "Yuji Itadori was an extraordinary high school student with incredible physical strength, but an ordinary " +
        "life until he discovered a cursed object that would change everything. After his grandfather’s death, " +
        "Yuji found himself in a world filled with curses, dark magic, and powerful sorcerers. His life collided with " +
        "those of Jujutsu Sorcerers, especially Satoru Gojo, who introduced him to the realm of Jujutsu Tech.\n\n" +

        "Despite his inexperience, Yuji’s ability to harness a cursed object, Sukuna's finger, made him a unique " +
        "weapon in the fight against curses. With newfound powers and the responsibility to fight evil, Yuji joined " +
        "the prestigious Jujutsu school to learn the ways of sorcery and protect others. Alongside his companions " +
        "Megumi and Nobara, Yuji navigated the complexities of this dark world.\n\n" +

        "As Yuji fought stronger enemies, the consequences of using Sukuna’s power weighed heavily on him. " +
        "Though he was determined to defeat curses, he grappled with the moral dilemmas that arose. The bonds he " +
        "formed with his friends and his unwavering sense of justice kept him grounded. In the end, Yuji realized " +
        "that true strength didn’t lie in power alone, but in the bonds we forge with others, and the courage to " +
        "make difficult choices.",
        "                       THE SUPERHERO SAGA\n\n" +
        "In the sprawling city of Metroville, there existed a hero known only as The Phantom. By day, he was " +
        "Lucas, a mild-mannered reporter, but by night, he was the city’s protector, fighting crime and injustice. " +
        "His powers, derived from an ancient relic, allowed him to manipulate shadows, moving undetected and " +
        "overcoming seemingly impossible odds.\n\n" +

        "The Phantom's most formidable challenge came in the form of his arch-nemesis, a villain named Vortex, " +
        "whose abilities to control time made him an almost unbeatable opponent. With each encounter, the stakes " +
        "grew higher, and the city’s fate hung in the balance. As the battle intensified, Lucas found himself " +
        "questioning his own humanity, wondering if he could balance his heroic duties with his personal life.\n\n" +

        "But even as Lucas struggled with his identity, he discovered that his true strength didn’t come from his " +
        "powers, but from the people around him. With the help of his loyal team, including the brilliant tech " +
        "expert Maya and the fearless fighter Kade, Lucas overcame Vortex’s manipulations of time and brought peace " +
        "to Metroville. In the end, Lucas realized that being a hero meant making sacrifices, but also standing " +
        "by those who needed him the most.",
        "                       THE WILD JOURNEY\n\n" +
        "In the heart of the jungle, a young lion named Simba faced the challenges of growing up. From a young age, " +
        "he was destined to become the leader of the Pride Lands, but his journey was fraught with danger and self-doubt. " +
        "His father, Mufasa, a wise and powerful lion, guided him through the ways of the wild, teaching him about courage, " +
        "leadership, and responsibility. Simba’s best friend, Nala, always by his side, encouraged him to embrace his destiny.\n\n" +

        "Tragedy struck when Mufasa was killed in a stampede orchestrated by Simba’s treacherous uncle, Scar. " +
        "Devastated and feeling responsible for his father's death, Simba fled into the wilderness. It was here that he " +
        "encountered new friends, Timon and Pumbaa, who taught him to live life without worrying about his responsibilities. " +
        "However, as he grew older, Simba realized that he could no longer escape his past.\n\n" +

        "With the guidance of his spirit of Mufasa and the unwavering support of Nala, Simba returned to the Pride Lands " +
        "to confront Scar. He realized that to become a true leader, he had to take responsibility for his actions and " +
        "embrace his destiny. In the end, Simba overcame his fears, defeating Scar and reclaiming his rightful place " +
        "as king, bringing peace and harmony to the Pride Lands."
        };

        private List<Question> questions1 = new List<Question>
        {
            new Question { QuestionText = "What was Yuji Itadori’s ordinary life like before discovering the cursed object?", A = "He was a Jujutsu Sorcerer", B = "He was an ordinary high school student", C = "He was a member of the Jujutsu Tech", D = "He was a sorcerer’s apprentice", CorrectAnswer = "B" },
            new Question { QuestionText = "Who introduced Yuji to the world of Jujutsu Tech?", A = "Megumi Fushiguro", B = "Satoru Gojo", C = "Nobara Kugisaki", D = "Ryomen Sukuna", CorrectAnswer = "B" },
            new Question { QuestionText = "What cursed object does Yuji harness to fight curses?", A = "A cursed sword", B = "Sukuna's finger", C = "A cursed ring", D = "A cursed talisman", CorrectAnswer = "B" },
            new Question { QuestionText = "What was Yuji’s main reason for joining Jujutsu Tech?", A = "To defeat Sukuna", B = "To learn sorcery", C = "To protect others and fight curses", D = "To become the strongest sorcerer", CorrectAnswer = "C" },
            new Question { QuestionText = "What lesson did Yuji learn by the end of his journey?", A = "True strength comes from power alone", B = "Strength is about defeating powerful enemies", C = "True strength lies in the bonds we form with others", D = "Strength is about fighting alone", CorrectAnswer = "C" },
            new Question { QuestionText = "What was Yuji Itadori’s ordinary life like before discovering the cursed object?", A = "He was a Jujutsu Sorcerer", B = "He was an ordinary high school student", C = "He was a member of the Jujutsu Tech", D = "He was a sorcerer’s apprentice", CorrectAnswer = "B" },
            new Question { QuestionText = "Who introduced Yuji to the world of Jujutsu Tech?", A = "Megumi Fushiguro", B = "Satoru Gojo", C = "Nobara Kugisaki", D = "Ryomen Sukuna", CorrectAnswer = "B" },
            new Question { QuestionText = "What cursed object does Yuji harness to fight curses?", A = "A cursed sword", B = "Sukuna's finger", C = "A cursed ring", D = "A cursed talisman", CorrectAnswer = "B" },
            new Question { QuestionText = "What was Yuji’s main reason for joining Jujutsu Tech?", A = "To defeat Sukuna", B = "To learn sorcery", C = "To protect others and fight curses", D = "To become the strongest sorcerer", CorrectAnswer = "C" },
            new Question { QuestionText = "What lesson did Yuji learn by the end of his journey?", A = "True strength comes from power alone", B = "Strength is about defeating powerful enemies", C = "True strength lies in the bonds we form with others", D = "Strength is about fighting alone", CorrectAnswer = "C" },
            new Question { QuestionText = "What happened after Yuji’s grandfather died?", A = "He left school", B = "He encountered a cursed object", C = "He moved to another city", D = "He became a Jujutsu teacher", CorrectAnswer = "B" },
            new Question { QuestionText = "Who were Yuji's closest companions?", A = "Satoru and Sukuna", B = "Gojo and Nanami", C = "Megumi and Nobara", D = "Toge and Maki", CorrectAnswer = "C" },
            new Question { QuestionText = "Why was Yuji considered unique?", A = "He could speak with spirits", B = "He had incredible speed", C = "He could control Sukuna's power", D = "He had a natural talent for cursed techniques", CorrectAnswer = "C" },
            new Question { QuestionText = "What moral dilemmas did Yuji struggle with?", A = "Killing humans possessed by curses", B = "Choosing between Megumi and Nobara", C = "Destroying Jujutsu Tech", D = "Becoming the new Sukuna", CorrectAnswer = "A" },
            new Question { QuestionText = "What does Jujutsu Tech teach students?", A = "Martial arts only", B = "Jujutsu sorcery and curse-fighting techniques", C = "The history of Sukuna", D = "How to summon spirits", CorrectAnswer = "B" },
            new Question { QuestionText = "Why did Yuji’s use of Sukuna’s power have consequences?", A = "It drained his energy", B = "It made him an outcast", C = "It risked Sukuna taking control of him", D = "It made him lose his memories", CorrectAnswer = "C" },
            new Question { QuestionText = "What was the ultimate goal of Jujutsu Sorcerers?", A = "To collect Sukuna’s fingers", B = "To eliminate all curses", C = "To protect only themselves", D = "To become stronger than Sukuna", CorrectAnswer = "B" },
            new Question { QuestionText = "How did Yuji stay grounded despite his struggles?", A = "By training harder", B = "By keeping strong bonds with his friends", C = "By seeking revenge", D = "By avoiding fights", CorrectAnswer = "B" },
            new Question { QuestionText = "Who was the first sorcerer Yuji encountered?", A = "Nobara Kugisaki", B = "Megumi Fushiguro", C = "Satoru Gojo", D = "Sukuna", CorrectAnswer = "B" },
            new Question { QuestionText = "What was Yuji’s final realization?", A = "He needed more power", B = "He should leave Jujutsu Tech", C = "True strength comes from his bonds with others", D = "He must fully embrace Sukuna’s power", CorrectAnswer = "C" },
        };

        private List<Question> questions2 = new List<Question>
        {
            new Question { QuestionText = "What is the true identity of The Phantom?", A = "Lucas, a mild-mannered reporter", B = "Kade, the fearless fighter", C = "Maya, the brilliant tech expert", D = "Vortex, the villain", CorrectAnswer = "A" },
            new Question { QuestionText = "What is The Phantom’s primary power?", A = "Time manipulation", B = "Shadow manipulation", C = "Super strength", D = "Invisibility", CorrectAnswer = "B" },
            new Question { QuestionText = "Who is The Phantom's arch-nemesis?", A = "Maya", B = "Vortex", C = "Kade", D = "The Mayor", CorrectAnswer = "B" },
            new Question { QuestionText = "What challenge did Lucas face as he fought crime?", A = "Balancing his heroic duties with his personal life", B = "Finding his true strength", C = "Defeating Vortex", D = "Gaining control of his powers", CorrectAnswer = "A" },
            new Question { QuestionText = "What did Lucas realize about being a hero?", A = "It’s about defeating the strongest enemies", B = "True strength comes from powers", C = "Being a hero means making sacrifices and supporting others", D = "It’s about standing alone", CorrectAnswer = "C" },
            new Question { QuestionText = "What is the true identity of The Phantom?", A = "Lucas, a mild-mannered reporter", B = "Kade, the fearless fighter", C = "Maya, the brilliant tech expert", D = "Vortex, the villain", CorrectAnswer = "A" },
            new Question { QuestionText = "What is The Phantom’s primary power?", A = "Time manipulation", B = "Shadow manipulation", C = "Super strength", D = "Invisibility", CorrectAnswer = "B" },
            new Question { QuestionText = "Who is The Phantom's arch-nemesis?", A = "Maya", B = "Vortex", C = "Kade", D = "The Mayor", CorrectAnswer = "B" },
            new Question { QuestionText = "What challenge did Lucas face as he fought crime?", A = "Balancing his heroic duties with his personal life", B = "Finding his true strength", C = "Defeating Vortex", D = "Gaining control of his powers", CorrectAnswer = "A" },
            new Question { QuestionText = "What did Lucas realize about being a hero?", A = "It’s about defeating the strongest enemies", B = "True strength comes from powers", C = "Being a hero means making sacrifices and supporting others", D = "It’s about standing alone", CorrectAnswer = "C" },
            new Question { QuestionText = "What was Lucas' daytime job?", A = "A detective", B = "A scientist", C = "A reporter", D = "A businessman", CorrectAnswer = "C" },
            new Question { QuestionText = "How did The Phantom receive his powers?", A = "A genetic mutation", B = "An ancient relic", C = "A scientific experiment", D = "A curse", CorrectAnswer = "B" },
            new Question { QuestionText = "What was Vortex’s power?", A = "Controlling fire", B = "Reading minds", C = "Manipulating time", D = "Becoming invisible", CorrectAnswer = "C" },
            new Question { QuestionText = "Who were Lucas’ most trusted allies?", A = "Maya and Kade", B = "Vortex and The Mayor", C = "The Phantom League", D = "His fellow reporters", CorrectAnswer = "A" },
            new Question { QuestionText = "Why was Vortex a nearly unbeatable enemy?", A = "He had an army", B = "He could control time", C = "He was physically invincible", D = "He had endless resources", CorrectAnswer = "B" },
            new Question { QuestionText = "How did Lucas ultimately defeat Vortex?", A = "By using his own shadow against him", B = "By tricking him into a time loop", C = "By stealing his powers", D = "By getting help from the government", CorrectAnswer = "B" },
            new Question { QuestionText = "What conflict did Lucas struggle with the most?", A = "Keeping his powers a secret", B = "His identity as both Lucas and The Phantom", C = "Choosing between being a hero or a villain", D = "Trusting his team", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Lucas learn about true heroism?", A = "It’s about having the most power", B = "It’s about standing alone", C = "It’s about making sacrifices and protecting others", D = "It’s about seeking revenge", CorrectAnswer = "C" },
            new Question { QuestionText = "What role did Maya play in The Phantom’s team?", A = "She was the team’s strategist", B = "She was a powerful fighter", C = "She developed tech and gadgets", D = "She was Lucas' sister", CorrectAnswer = "C" },
            new Question { QuestionText = "How did Metroville change after The Phantom defeated Vortex?", A = "It became peaceful again", B = "It fell into chaos", C = "It lost its government", D = "It became a city of heroes", CorrectAnswer = "A" },
        };

        private List<Question> questions3 = new List<Question>
        {
            new Question { QuestionText = "Who is Simba’s best friend and constant supporter?", A = "Scar", B = "Timon", C = "Mufasa", D = "Nala", CorrectAnswer = "D" },
            new Question { QuestionText = "What tragedy leads Simba to flee into the wilderness?", A = "The death of his mother", B = "The loss of the Pride Lands", C = "Mufasa’s death in a stampede", D = "Scar’s betrayal", CorrectAnswer = "C" },
            new Question { QuestionText = "Who taught Simba to live life without worrying about his responsibilities?", A = "Timon and Pumbaa", B = "Nala", C = "Mufasa", D = "Scar", CorrectAnswer = "A" },
            new Question { QuestionText = "What lesson did Simba learn as he grew older?", A = "To forget his past", B = "To embrace his destiny", C = "To avoid his responsibilities", D = "To live without fear", CorrectAnswer = "B" },
            new Question { QuestionText = "How did Simba reclaim his rightful place as king?", A = "By defeating Scar", B = "By finding a new territory", C = "By receiving guidance from Timon and Pumbaa", D = "By gaining the approval of the Pride Lands", CorrectAnswer = "A" },
            new Question { QuestionText = "Who is Simba’s best friend and constant supporter?", A = "Scar", B = "Timon", C = "Mufasa", D = "Nala", CorrectAnswer = "D" },
            new Question { QuestionText = "What tragedy leads Simba to flee into the wilderness?", A = "The death of his mother", B = "The loss of the Pride Lands", C = "Mufasa’s death in a stampede", D = "Scar’s betrayal", CorrectAnswer = "C" },
            new Question { QuestionText = "Who taught Simba to live life without worrying about his responsibilities?", A = "Timon and Pumbaa", B = "Nala", C = "Mufasa", D = "Scar", CorrectAnswer = "A" },
            new Question { QuestionText = "What lesson did Simba learn as he grew older?", A = "To forget his past", B = "To embrace his destiny", C = "To avoid his responsibilities", D = "To live without fear", CorrectAnswer = "B" },
            new Question { QuestionText = "How did Simba reclaim his rightful place as king?", A = "By defeating Scar", B = "By finding a new territory", C = "By receiving guidance from Timon and Pumbaa", D = "By gaining the approval of the Pride Lands", CorrectAnswer = "A" },
            new Question { QuestionText = "What was Mufasa’s role in Simba’s life?", A = "His brother", B = "His mentor and father", C = "His best friend", D = "His enemy", CorrectAnswer = "B" },
            new Question { QuestionText = "How did Scar take control of the Pride Lands?", A = "By convincing the lions to follow him", B = "By overthrowing Simba", C = "By manipulating Simba and causing Mufasa's death", D = "By defeating Mufasa in battle", CorrectAnswer = "C" },
            new Question { QuestionText = "What phrase did Timon and Pumbaa teach Simba?", A = "Remember who you are", B = "Hakuna Matata", C = "Be prepared", D = "Long live the king", CorrectAnswer = "B" },
            new Question { QuestionText = "What ultimately gave Simba the strength to return?", A = "Timon and Pumbaa's encouragement", B = "His dream about Mufasa", C = "Nala's insistence", D = "Scar’s growing power", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Simba learn about leadership?", A = "That he needed to be the strongest", B = "That true leaders take responsibility", C = "That he should rule alone", D = "That power comes from fear", CorrectAnswer = "B" },
            new Question { QuestionText = "How did the Pride Lands change under Scar’s rule?", A = "They became more prosperous", B = "They fell into ruin", C = "They expanded", D = "They remained unchanged", CorrectAnswer = "B" },
            new Question { QuestionText = "Who helped Simba realize that he was ready to be king?", A = "Nala", B = "Timon and Pumbaa", C = "The spirit of Mufasa", D = "Scar", CorrectAnswer = "C" },
            new Question { QuestionText = "What was Simba's final battle against Scar like?", A = "A one-sided fight", B = "A fierce battle for dominance", C = "A quick defeat of Scar", D = "A peaceful negotiation", CorrectAnswer = "B" },
            new Question { QuestionText = "What did Simba learn from his father’s teachings?", A = "That he should never return to the Pride Lands", B = "That being a king is about responsibility", C = "That revenge is the key to ruling", D = "That power means control", CorrectAnswer = "B" },
            new Question { QuestionText = "What happened after Simba reclaimed the throne?", A = "The Pride Lands flourished again", B = "The lions rejected him", C = "He ruled with fear", D = "He left and never returned", CorrectAnswer = "A" },
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

        private void CharacterAnalysisEasy_Load(object sender, EventArgs e)
        {
            
        }
    }
}
