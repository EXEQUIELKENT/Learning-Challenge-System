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
    public partial class PatternRecognitionHard : Form
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
        public class Question
        {
            public string QuestionText { get; set; }
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string CorrectAnswer { get; set; }
        }
        public PatternRecognitionHard()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "If the pattern follows: Apple, Banana, Cherry, Dragonfruit, ?, what comes next?", A = "Elderberry", B = "Fig", C = "Grape", D = "Guava", CorrectAnswer = "A" },
                new Question { QuestionText = "Which name logically follows this sequence: Albert, Benjamin, Charles, Daniel, ?", A = "Edward", B = "Francis", C = "George", D = "Ethan", CorrectAnswer = "A" },
                new Question { QuestionText = "Which place follows in this pattern: Manila, Bangkok, Tokyo, Beijing, ?", A = "Jakarta", B = "Seoul", C = "Hanoi", D = "Taipei", CorrectAnswer = "B" },
                new Question { QuestionText = "If A = 1, B = 3, C = 6, D = 10, then what is F?", A = "15", B = "21", C = "28", D = "36", CorrectAnswer = "C" },
                new Question { QuestionText = "In a pattern of professions: Doctor, Engineer, Lawyer, Teacher, ?, what comes next?", A = "Pilot", B = "Scientist", C = "Architect", D = "Nurse", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Find the next in the sequence: Tesla, Edison, Newton, Einstein, ?", A = "Hawking", B = "Galileo", C = "Curie", D = "Faraday", CorrectAnswer = "B" },
                new Question { QuestionText = "Which word logically follows this pattern: Hot, Warm, Cool, ?", A = "Boiling", B = "Freezing", C = "Cold", D = "Chilly", CorrectAnswer = "C" },
                new Question { QuestionText = "Which number logically follows the pattern: 19, 37, 79, 163, ?", A = "247", B = "329", C = "303", D = "331", CorrectAnswer = "D" },
                new Question { QuestionText = "What city follows in this pattern: Paris, Berlin, Madrid, Rome, ?", A = "Vienna", B = "Lisbon", C = "London", D = "Athens", CorrectAnswer = "C" },
                new Question { QuestionText = "What is the next logical letter in the sequence: C, G, L, R, ?", A = "T", B = "W", C = "X", D = "Z", CorrectAnswer = "B" },
                
                new Question { QuestionText = "If the sequence follows: Mercury, Venus, Earth, Mars, ?, what comes next?", A = "Jupiter", B = "Saturn", C = "Neptune", D = "Uranus", CorrectAnswer = "A" },
                new Question { QuestionText = "If 2, 6, 12, 20, ?, what comes next?", A = "26", B = "30", C = "32", D = "36", CorrectAnswer = "C" },
                new Question { QuestionText = "Which book logically follows: The Fellowship of the Ring, The Two Towers, ?", A = "The Hobbit", B = "The Return of the King", C = "The Silmarillion", D = "Harry Potter", CorrectAnswer = "B" },
                new Question { QuestionText = "What country follows this pattern: USA, Canada, Mexico, Guatemala, ?", A = "Panama", B = "El Salvador", C = "Honduras", D = "Costa Rica", CorrectAnswer = "C" },
                new Question { QuestionText = "What profession follows this pattern: Pilot, Captain, Commander, ?", A = "Lieutenant", B = "General", C = "Marshal", D = "Admiral", CorrectAnswer = "D" },
                
                new Question { QuestionText = "Which Philippine historical figure follows: Rizal, Bonifacio, Del Pilar, Mabini, ?", A = "Aguinaldo", B = "Luna", C = "Quezon", D = "Osmeña", CorrectAnswer = "B" },
                new Question { QuestionText = "Which historical event follows: American Revolution, French Revolution, Industrial Revolution, ?", A = "Renaissance", B = "Civil Rights Movement", C = "World War I", D = "Russian Revolution", CorrectAnswer = "D" },
                new Question { QuestionText = "Which TV show follows: Breaking Bad, Game of Thrones, The Walking Dead, ?", A = "Stranger Things", B = "House of Cards", C = "Westworld", D = "Sherlock", CorrectAnswer = "C" },
                new Question { QuestionText = "What is the next famous inventor in this pattern: Galileo, Newton, Edison, Tesla, ?", A = "Faraday", B = "Curie", C = "Einstein", D = "Hawking", CorrectAnswer = "C" },
                new Question { QuestionText = "Which Disney movie follows: Snow White, Cinderella, Sleeping Beauty, The Little Mermaid, ?", A = "Beauty and the Beast", B = "Mulan", C = "Aladdin", D = "Frozen", CorrectAnswer = "A" },
                
                new Question { QuestionText = "What game title follows: Super Mario Bros, The Legend of Zelda, Metroid, ?", A = "Kirby", B = "Donkey Kong", C = "Mega Man", D = "Final Fantasy", CorrectAnswer = "B" },
                new Question { QuestionText = "Which mobile brand follows: Nokia, BlackBerry, Apple, Samsung, ?", A = "Sony", B = "Xiaomi", C = "Huawei", D = "Motorola", CorrectAnswer = "C" },
                new Question { QuestionText = "What follows this sequence: January, March, May, July, ?", A = "September", B = "October", C = "November", D = "August", CorrectAnswer = "C" },
                new Question { QuestionText = "Which fast-food chain follows: McDonald's, Burger King, Wendy's, KFC, ?", A = "Subway", B = "Pizza Hut", C = "Taco Bell", D = "Domino's", CorrectAnswer = "C" },
                new Question { QuestionText = "Which Pokémon follows this evolution pattern: Charmander, Charmeleon, ?", A = "Charizard", B = "Dragonite", C = "Arcanine", D = "Gyarados", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Which number follows this sequence: 2, 5, 10, 17, ?", A = "25", B = "26", C = "28", D = "30", CorrectAnswer = "B" },
                new Question { QuestionText = "Which planet follows: Jupiter, Saturn, Uranus, ?", A = "Earth", B = "Neptune", C = "Venus", D = "Mars", CorrectAnswer = "B" },
                new Question { QuestionText = "Which singer follows: Michael Jackson, Elvis Presley, Madonna, ?", A = "Whitney Houston", B = "Beyoncé", C = "Freddie Mercury", D = "Adele", CorrectAnswer = "C" },
                new Question { QuestionText = "Which sport follows: Football, Basketball, Baseball, Tennis, ?", A = "Soccer", B = "Golf", C = "Cricket", D = "Hockey", CorrectAnswer = "D" },
                new Question { QuestionText = "If the pattern follows: Shakespeare, Dickens, Hemingway, Orwell, ?, who comes next?", A = "Austen", B = "Tolkien", C = "Fitzgerald", D = "Poe", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Which event follows: World War I, World War II, Cold War, ?", A = "Industrial Revolution", B = "Vietnam War", C = "Moon Landing", D = "9/11", CorrectAnswer = "B" },
                new Question { QuestionText = "Which scientist follows: Galileo, Newton, Einstein, Hawking, ?", A = "Bohr", B = "Curie", C = "Tesla", D = "Darwin", CorrectAnswer = "A" },
                new Question { QuestionText = "Which social media platform follows: Friendster, MySpace, Facebook, Instagram, ?", A = "Snapchat", B = "TikTok", C = "Twitter", D = "Reddit", CorrectAnswer = "B" },
                new Question { QuestionText = "If the sequence follows: iPhone 4, iPhone 5, iPhone 6, iPhone 7, ?, what comes next?", A = "iPhone 8", B = "iPhone X", C = "iPhone SE", D = "iPhone XR", CorrectAnswer = "A" },
                new Question { QuestionText = "If 5, 12, 24, 41, ?, what comes next?", A = "60", B = "63", C = "65", D = "70", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which Olympic host city follows: Athens, Beijing, London, Rio, ?", A = "Tokyo", B = "Paris", C = "Los Angeles", D = "Berlin", CorrectAnswer = "A" },
                new Question { QuestionText = "If the pattern follows: New York, Los Angeles, Chicago, Houston, ?, which city comes next?", A = "Philadelphia", B = "San Francisco", C = "Phoenix", D = "Dallas", CorrectAnswer = "C" },
                new Question { QuestionText = "Which software company follows: Microsoft, Apple, Google, Amazon, ?", A = "Facebook", B = "IBM", C = "Netflix", D = "Adobe", CorrectAnswer = "A" },
                new Question { QuestionText = "Which car brand follows: Toyota, Honda, Ford, BMW, ?", A = "Mercedes", B = "Nissan", C = "Chevrolet", D = "Tesla", CorrectAnswer = "D" },
                new Question { QuestionText = "Which chess champion follows: Fischer, Kasparov, Kramnik, Carlsen, ?", A = "Nakamura", B = "Ding Liren", C = "Caruana", D = "Anand", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which cryptocurrency follows: Bitcoin, Ethereum, Litecoin, Dogecoin, ?", A = "Cardano", B = "Solana", C = "Polkadot", D = "Ripple", CorrectAnswer = "B" },
                new Question { QuestionText = "Which video game follows: Counter-Strike, Call of Duty, Battlefield, Halo, ?", A = "Valorant", B = "Overwatch", C = "Apex Legends", D = "Destiny", CorrectAnswer = "A" },
                new Question { QuestionText = "If the pattern follows: Tiger, Elephant, Lion, Giraffe, ?, what comes next?", A = "Cheetah", B = "Zebra", C = "Rhino", D = "Bear", CorrectAnswer = "B" },
                new Question { QuestionText = "Which YouTuber follows: PewDiePie, MrBeast, Markiplier, Jacksepticeye, ?", A = "Dream", B = "Ninja", C = "Logan Paul", D = "Corpse", CorrectAnswer = "A" },
                new Question { QuestionText = "Which anime follows: Naruto, One Piece, Bleach, Dragon Ball, ?", A = "Demon Slayer", B = "Attack on Titan", C = "Fairy Tail", D = "Death Note", CorrectAnswer = "C" },
                
                new Question { QuestionText = "If the sequence follows: Rock, Paper, Scissors, ?, what comes next?", A = "Lizard", B = "Spock", C = "Both A and B", D = "None", CorrectAnswer = "C" },
                new Question { QuestionText = "Which mobile app follows: WhatsApp, Messenger, Telegram, Signal, ?", A = "Viber", B = "WeChat", C = "Snapchat", D = "Discord", CorrectAnswer = "B" },
                new Question { QuestionText = "Which horror movie follows: The Exorcist, The Shining, A Nightmare on Elm Street, The Conjuring, ?", A = "Hereditary", B = "It", C = "Insidious", D = "Paranormal Activity", CorrectAnswer = "B" },
                new Question { QuestionText = "If the sequence follows: 4, 8, 15, 16, 23, ?, what comes next?", A = "42", B = "50", C = "30", D = "33", CorrectAnswer = "A" },
                new Question { QuestionText = "Which Marvel superhero follows: Iron Man, Captain America, Thor, Hulk, ?", A = "Black Widow", B = "Doctor Strange", C = "Spider-Man", D = "Hawkeye", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Which Game of Thrones house follows: Stark, Lannister, Baratheon, Targaryen, ?", A = "Tully", B = "Martell", C = "Greyjoy", D = "Bolton", CorrectAnswer = "C" },
                new Question { QuestionText = "If the pattern follows: Red, Orange, Yellow, Green, Blue, ?, what comes next?", A = "Indigo", B = "Violet", C = "Pink", D = "Cyan", CorrectAnswer = "A" },
                new Question { QuestionText = "Which programming language follows: C, Java, Python, JavaScript, ?", A = "Swift", B = "Kotlin", C = "Ruby", D = "C#", CorrectAnswer = "D" },
                new Question { QuestionText = "Which fast food chain follows: Jollibee, McDonald’s, KFC, Burger King, ?", A = "Wendy’s", B = "Chowking", C = "Shakey’s", D = "Pizza Hut", CorrectAnswer = "A" },
                new Question { QuestionText = "Which K-pop group follows: BTS, Blackpink, EXO, TWICE, ?", A = "ITZY", B = "Stray Kids", C = "Red Velvet", D = "SEVENTEEN", CorrectAnswer = "D" },
                
                new Question { QuestionText = "Which NBA team follows: Lakers, Bulls, Celtics, Warriors, ?", A = "Heat", B = "Suns", C = "Mavericks", D = "Bucks", CorrectAnswer = "A" },
                new Question { QuestionText = "Which game console follows: PlayStation 1, PlayStation 2, PlayStation 3, ?", A = "PlayStation 4", B = "PlayStation 5", C = "Xbox 360", D = "Nintendo Switch", CorrectAnswer = "A" },
                new Question { QuestionText = "Which car model follows: Mustang, Camaro, Challenger, Corvette, ?", A = "Lamborghini", B = "Ferrari", C = "Dodge Charger", D = "Tesla Model S", CorrectAnswer = "C" },
                new Question { QuestionText = "Which element follows: Hydrogen, Helium, Lithium, Beryllium, ?", A = "Carbon", B = "Nitrogen", C = "Boron", D = "Oxygen", CorrectAnswer = "C" },
               };

            // Initialize answeredQuestions with false values (indicating that no question has been answered yet)
            Random rand = new Random();
            questions = questions.OrderBy(q => rand.Next()).Take(20).ToList();

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
                FormTitle = "Pattern Recognition (Hard)",
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
                FormTitle = "Pattern Recognition (Hard)",
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
                // Start the challenge
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

        private void GrammarEasy_Load(object sender, EventArgs e)
        {

        }
    }
}
