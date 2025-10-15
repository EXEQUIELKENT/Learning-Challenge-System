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
    public partial class QuizMentalEasy : Form
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
        public class Question
        {
            public string QuestionText { get; set; }
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string CorrectAnswer { get; set; }
        }
        public QuizMentalEasy()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "What is the most common mental health disorder?", A = "Bipolar Disorder", B = "Depression", C = "Anxiety", D = "Schizophrenia", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is a symptom of depression?", A = "Excessive happiness", B = "Persistent sadness", C = "Increased energy", D = "High self-esteem", CorrectAnswer = "B" },
                new Question { QuestionText = "What does PTSD stand for?", A = "Post-Therapy Stress Disorder", B = "Personal Therapy Stress Disorder", C = "Psychological Tension Stress Disorder", D = "Post-Traumatic Stress Disorder", CorrectAnswer = "D" },
                new Question { QuestionText = "Which of the following is a risk factor for developing mental health disorders?", A = "Experiencing childhood trauma", B = "Having supportive relationships", C = "Being well-rested", D = "Regular physical exercise", CorrectAnswer = "A" },
                new Question { QuestionText = "What is one of the best ways to help a friend who is struggling with mental health issues?", A = "Tell them to just \"snap out of it\"", B = "Keep them isolated from others", C = "Encourage them to seek professional help", D = "Ignore their feelings", CorrectAnswer = "C" },

                new Question { QuestionText = "What is the term for a mental health condition involving extreme mood swings?", A = "Bipolar Disorder", B = "OCD", C = "Schizophrenia", D = "PTSD", CorrectAnswer = "A" },
                new Question { QuestionText = "Which mental illness is characterized by persistent intrusive thoughts and repetitive behaviors?", A = "Generalized Anxiety Disorder", B = "Obsessive-Compulsive Disorder", C = "Bipolar Disorder", D = "PTSD", CorrectAnswer = "B" },
                new Question { QuestionText = "What is the name of the natural 'feel-good' chemicals in the brain?", A = "Cortisol", B = "Endorphins", C = "Testosterone", D = "Adrenaline", CorrectAnswer = "B" },
                new Question { QuestionText = "What is the best way to support someone with a mental health disorder?", A = "Judging them", B = "Listening and offering support", C = "Ignoring them", D = "Telling them to 'just be happy'", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is NOT a symptom of anxiety?", A = "Rapid heartbeat", B = "Excessive worrying", C = "Hallucinations", D = "Difficulty sleeping", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Which therapy is commonly used to treat depression and anxiety?", A = "Cognitive Behavioral Therapy (CBT)", B = "Electroshock Therapy", C = "Dream Analysis", D = "None of the above", CorrectAnswer = "A" },
                new Question { QuestionText = "Which of the following is a sign of burnout?", A = "Feeling energized", B = "Emotional exhaustion", C = "Increased productivity", D = "None of the above", CorrectAnswer = "B" },
                new Question { QuestionText = "Which activity can help improve mental well-being?", A = "Socializing", B = "Meditation", C = "Exercise", D = "All of the above", CorrectAnswer = "D" },
                new Question { QuestionText = "What does ADHD affect the most?", A = "Memory only", B = "Attention, impulsivity, and hyperactivity", C = "Speech", D = "Digestion", CorrectAnswer = "B" },
                new Question { QuestionText = "Which mental disorder involves extreme fear or panic attacks?", A = "Depression", B = "Panic Disorder", C = "ADHD", D = "Schizophrenia", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which of these can contribute to better sleep and mental health?", A = "Caffeine before bed", B = "Regular sleep schedule", C = "Using your phone in bed", D = "Sleeping only 3 hours", CorrectAnswer = "B" },
                new Question { QuestionText = "What is a healthy way to deal with stress?", A = "Ignoring it", B = "Using alcohol", C = "Exercise and relaxation techniques", D = "Working non-stop", CorrectAnswer = "C" },
                new Question { QuestionText = "What is the first step in seeking mental health support?", A = "Hiding your feelings", B = "Talking to a trusted person or professional", C = "Avoiding the topic", D = "Self-diagnosing", CorrectAnswer = "B" },
                new Question { QuestionText = "Which mental health condition is characterized by a lack of interest in daily activities?", A = "Depression", B = "Schizophrenia", C = "OCD", D = "Panic Disorder", CorrectAnswer = "A" },
                new Question { QuestionText = "What does exposure therapy help treat?", A = "Anxiety disorders and phobias", B = "ADHD", C = "Bipolar Disorder", D = "None of the above", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Which of the following is NOT a cause of mental illness?", A = "Genetics", B = "Brain chemistry", C = "Superstitions", D = "Trauma", CorrectAnswer = "C" },
                new Question { QuestionText = "What is a common stigma about mental illness?", A = "It is just in a person's head", B = "People can 'snap out of it'", C = "Only weak people have it", D = "All of the above", CorrectAnswer = "D" },
                new Question { QuestionText = "What is self-harm often a sign of?", A = "Attention-seeking", B = "Serious emotional distress", C = "Happiness", D = "Good mental health", CorrectAnswer = "B" },
                new Question { QuestionText = "What is emotional intelligence?", A = "Suppressing emotions", B = "Recognizing and managing emotions effectively", C = "Being unemotional", D = "A mental disorder", CorrectAnswer = "B" },
                new Question { QuestionText = "What is one key sign of a healthy mind?", A = "Ignoring emotions", B = "Being able to cope with stress and change", C = "Never feeling sad", D = "Always avoiding problems", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which of these is an effective way to prevent mental health issues?", A = "Building a strong support system", B = "Avoiding all social interactions", C = "Bottling up emotions", D = "Overworking yourself", CorrectAnswer = "A" },
                new Question { QuestionText = "How does gratitude impact mental health?", A = "It has no effect", B = "It can increase happiness and life satisfaction", C = "It causes anxiety", D = "It is only useful for children", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following can increase resilience?", A = "Avoiding challenges", B = "Adopting a positive mindset and problem-solving skills", C = "Blaming others", D = "Ignoring difficulties", CorrectAnswer = "B" },
                new Question { QuestionText = "Which neurotransmitter is most associated with feelings of happiness?", A = "Serotonin", B = "Adrenaline", C = "Cortisol", D = "Glutamate", CorrectAnswer = "A" },
                new Question { QuestionText = "What is a common treatment for anxiety disorders?", A = "Meditation", B = "Exposure therapy", C = "Medication", D = "All of the above", CorrectAnswer = "D" },
                
                new Question { QuestionText = "Which age group is most affected by mental health disorders?", A = "Children", B = "Teenagers and young adults", C = "Middle-aged adults", D = "Elderly", CorrectAnswer = "B" },
                new Question { QuestionText = "What is one of the first signs of burnout?", A = "Increased energy", B = "Feeling exhausted all the time", C = "Improved focus", D = "More motivation", CorrectAnswer = "B" },
                new Question { QuestionText = "How does social media affect mental health?", A = "Can increase anxiety and depression", B = "Has no effect", C = "Always improves mental health", D = "Prevents loneliness", CorrectAnswer = "A" },
                new Question { QuestionText = "What is mindfulness?", A = "Being aware of the present moment", B = "Always being busy", C = "Ignoring emotions", D = "A type of diet", CorrectAnswer = "A" },
                new Question { QuestionText = "Which mental health disorder involves repetitive behaviors?", A = "Depression", B = "Obsessive-Compulsive Disorder (OCD)", C = "ADHD", D = "Schizophrenia", CorrectAnswer = "B" },
                
                new Question { QuestionText = "What can help improve sleep quality and mental health?", A = "Caffeine before bed", B = "A regular sleep schedule", C = "Sleeping with the TV on", D = "Staying up all night", CorrectAnswer = "B" },
                new Question { QuestionText = "What does therapy help with?", A = "Coping strategies", B = "Understanding emotions", C = "Reducing stress", D = "All of the above", CorrectAnswer = "D" },
                new Question { QuestionText = "What does ADHD affect?", A = "Memory", B = "Attention and hyperactivity", C = "Speech", D = "Digestion", CorrectAnswer = "B" },
                new Question { QuestionText = "What is a major warning sign of suicide?", A = "Talking about feeling hopeless", B = "Sudden mood improvements", C = "Giving away possessions", D = "All of the above", CorrectAnswer = "D" },
                new Question { QuestionText = "Which of these is a type of anxiety disorder?", A = "Bipolar disorder", B = "Panic disorder", C = "Dementia", D = "ADHD", CorrectAnswer = "B" },
                
                new Question { QuestionText = "What is a benefit of journaling for mental health?", A = "It helps process emotions", B = "It reduces stress", C = "It improves self-awareness", D = "All of the above", CorrectAnswer = "D" },
                new Question { QuestionText = "What is a symptom of schizophrenia?", A = "Hallucinations", B = "Increased focus", C = "High energy", D = "Improved mood", CorrectAnswer = "A" },
                new Question { QuestionText = "Which profession specializes in mental health treatment?", A = "Cardiologist", B = "Psychiatrist", C = "Dentist", D = "Neurologist", CorrectAnswer = "B" },
                new Question { QuestionText = "How does deep breathing help with anxiety?", A = "It increases oxygen flow", B = "It slows the heart rate", C = "It promotes relaxation", D = "All of the above", CorrectAnswer = "D" },
                new Question { QuestionText = "What is postnatal depression?", A = "Depression after childbirth", B = "Depression in the elderly", C = "A form of seasonal depression", D = "A childhood disorder", CorrectAnswer = "A" },
                
                new Question { QuestionText = "What role does exercise play in mental health?", A = "Releases stress-reducing hormones", B = "Improves mood", C = "Boosts self-esteem", D = "All of the above", CorrectAnswer = "D" },
                new Question { QuestionText = "What can trigger a panic attack?", A = "Stress", B = "Caffeine", C = "Overthinking", D = "All of the above", CorrectAnswer = "D" },
                new Question { QuestionText = "Which of these is a symptom of bipolar disorder?", A = "Extreme mood swings", B = "Hearing voices", C = "Memory loss", D = "Fear of crowds", CorrectAnswer = "A" },
                new Question { QuestionText = "What is the fear of social situations called?", A = "Agoraphobia", B = "Social Anxiety Disorder", C = "Panic Disorder", D = "Claustrophobia", CorrectAnswer = "B" },
                new Question { QuestionText = "What is one way to help a friend with anxiety?", A = "Reassure them", B = "Encourage them to breathe slowly", C = "Be patient and understanding", D = "All of the above", CorrectAnswer = "D" },
                
                new Question { QuestionText = "What is the first step in overcoming stigma around mental health?", A = "Ignoring it", B = "Educating yourself and others", C = "Avoiding conversations about it", D = "Telling people to 'just deal with it'", CorrectAnswer = "B" },
                new Question { QuestionText = "Which part of the brain is most involved in regulating emotions?", A = "Cerebellum", B = "Amygdala", C = "Brainstem", D = "Occipital lobe", CorrectAnswer = "B" },
                new Question { QuestionText = "What is seasonal affective disorder (SAD)?", A = "A type of anxiety disorder", B = "Depression linked to seasonal changes", C = "A form of OCD", D = "A sleep disorder", CorrectAnswer = "B" },
                new Question { QuestionText = "Which activity is scientifically proven to reduce stress?", A = "Listening to calming music", B = "Spending time in nature", C = "Practicing mindfulness", D = "All of the above", CorrectAnswer = "D" },
                new Question { QuestionText = "What is one common myth about mental health?", A = "Only weak people have mental health issues", B = "Mental illnesses can be treated", C = "Anyone can experience mental health struggles", D = "Seeking help is a sign of strength", CorrectAnswer = "A" }
            };

            // Initialize answeredQuestions with false values (indicating that no question has been answered yet)
            Random rand = new Random();
            questions = questions.OrderBy(q => rand.Next()).Take(5).ToList();

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
                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Easy)", score);

                MessageBox.Show("Challenge not completed. Returning to the homepage.", "Challenge Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TransitionToHomePage();
            }
        }
        public void SaveChallengeDataSuccess(string username, string status, string time, string formTitle, int score)
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
                FormTitle = formTitle,
                Score = $"Completed {score}", // New property for jogging
                Time = time
            };

            challengeList.Add(newChallengeData);

            // Save back to the JSON file
            var updatedChallengeData = JsonConvert.SerializeObject(challengeList, Formatting.Indented);
            File.WriteAllText(challengeFilePath, updatedChallengeData);
        }
        public void SaveChallengeDataFailed(string username, string time, string formTitle, int score)
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
                FormTitle = formTitle,
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
                                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Easy)", score);

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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Easy)", score);

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
                        if (score >= 3) // Check if the challenge is passed
                        {
                            success.Play();
                            SaveChallengeDataSuccess(Login.CurrentUsername, "Completed", textBox2.Text, "Quiz Mental (Easy)", score);
                            MessageBox.Show("Returning to the homepage.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            TransitionToHomePage();
                        }
                        else
                        {
                            fail.Play();
                            SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Easy)", score);
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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Easy)", score);

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
        private void QuizMentalEasy_Load(object sender, EventArgs e)
        {

        }
    }
}
