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
    public partial class QuizMentalMedium : Form
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
        public QuizMentalMedium()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "What is the main goal of Cognitive Behavioral Therapy (CBT)?", A = "To identify past trauma", B = "To change negative thought patterns", C = "To encourage medication use", D = "To analyze dreams", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is a common symptom of Generalized Anxiety Disorder (GAD)?", A = "Frequent mood swings", B = "Chronic worry and tension", C = "Sudden panic attacks", D = "Persistent sadness", CorrectAnswer = "B" },
                new Question { QuestionText = "Which mental health condition is most commonly associated with feelings of hopelessness and lack of energy?", A = "Bipolar disorder", B = "Major depressive disorder", C = "Schizophrenia", D = "Obsessive-Compulsive Disorder (OCD)", CorrectAnswer = "B" },
                new Question { QuestionText = "Which neurotransmitter is most commonly associated with depression?", A = "Dopamine", B = "Serotonin", C = "Acetylcholine", D = "Glutamate", CorrectAnswer = "B" },
                new Question { QuestionText = "What is a common side effect of antipsychotic medication?", A = "Weight loss", B = "Drowsiness", C = "Increased energy", D = "Insomnia", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is NOT a typical characteristic of someone with Post-Traumatic Stress Disorder (PTSD)?", A = "Flashbacks", B = "Avoidance of triggers", C = "Hyper-vigilance", D = "Increased desire for social activities", CorrectAnswer = "D" },
                
                new Question { QuestionText = "What is one of the first steps in the treatment of panic disorder?", A = "Hospitalization", B = "Medication only", C = "Cognitive Behavioral Therapy (CBT)", D = "Electroconvulsive therapy (ECT)", CorrectAnswer = "C" },
                new Question { QuestionText = "Which condition is primarily characterized by repetitive, intrusive thoughts and compulsive behaviors?", A = "Bipolar disorder", B = "Obsessive-Compulsive Disorder (OCD)", C = "Schizophrenia", D = "Attention-Deficit/Hyperactivity Disorder (ADHD)", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is a risk factor for developing eating disorders?", A = "High levels of social support", B = "Low levels of stress", C = "Pressure to maintain a certain body image", D = "A supportive relationship with a partner", CorrectAnswer = "C" },
                new Question { QuestionText = "Which type of therapy involves understanding past relationships to improve current emotional functioning?", A = "Psychodynamic therapy", B = "Solution-focused therapy", C = "Cognitive Behavioral Therapy (CBT)", D = "Dialectical Behavior Therapy (DBT)", CorrectAnswer = "A" },
                new Question { QuestionText = "What is one of the most effective ways to manage mild depression?", A = "Ignoring it", B = "Exercising regularly", C = "Consuming caffeine", D = "Sleeping less", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which part of the brain is most involved in processing emotions?", A = "Cerebellum", B = "Amygdala", C = "Occipital lobe", D = "Brainstem", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is NOT a symptom of anxiety disorders?", A = "Excessive worry", B = "Increased heart rate", C = "Hallucinations", D = "Restlessness", CorrectAnswer = "C" },
                new Question { QuestionText = "Which lifestyle change can positively impact mental health?", A = "Avoiding social interactions", B = "Eating a balanced diet", C = "Skipping meals", D = "Sleeping fewer hours", CorrectAnswer = "B" },
                new Question { QuestionText = "Which mental disorder involves alternating periods of extreme highs and lows?", A = "Schizophrenia", B = "Bipolar disorder", C = "Anxiety disorder", D = "Depression", CorrectAnswer = "B" },
                new Question { QuestionText = "Which type of therapy focuses on mindfulness and acceptance strategies?", A = "Cognitive Behavioral Therapy (CBT)", B = "Dialectical Behavior Therapy (DBT)", C = "Exposure therapy", D = "Psychoanalysis", CorrectAnswer = "B" },
                
                new Question { QuestionText = "What is the term for a severe episode of major depression that includes psychotic symptoms?", A = "Catatonic depression", B = "Psychotic depression", C = "Persistent depressive disorder", D = "Seasonal affective disorder", CorrectAnswer = "B" },
                new Question { QuestionText = "What is the name of the stress hormone?", A = "Serotonin", B = "Dopamine", C = "Cortisol", D = "Endorphin", CorrectAnswer = "C" },
                new Question { QuestionText = "Which therapy is primarily used to treat phobias?", A = "Psychoanalysis", B = "Cognitive Behavioral Therapy (CBT)", C = "Hypnotherapy", D = "Electroconvulsive therapy (ECT)", CorrectAnswer = "B" },
                new Question { QuestionText = "What is a common physical symptom of an anxiety disorder?", A = "Hair growth", B = "Increased heart rate", C = "Decreased appetite", D = "Sudden weight gain", CorrectAnswer = "B" },
                new Question { QuestionText = "Which mental disorder involves difficulty focusing, hyperactivity, and impulsivity?", A = "Schizophrenia", B = "Obsessive-Compulsive Disorder (OCD)", C = "Attention-Deficit/Hyperactivity Disorder (ADHD)", D = "Bipolar disorder", CorrectAnswer = "C" },
                
                new Question { QuestionText = "Which therapy involves gradually exposing a person to their fear?", A = "Exposure therapy", B = "Talk therapy", C = "Hypnotherapy", D = "Psychoanalysis", CorrectAnswer = "A" },
                new Question { QuestionText = "Which of the following is a sign of emotional well-being?", A = "Constant mood swings", B = "Persistent anxiety", C = "Ability to handle stress effectively", D = "Isolating oneself from others", CorrectAnswer = "C" },
                new Question { QuestionText = "Which mental illness is characterized by episodes of mania and depression?", A = "Anxiety disorder", B = "Bipolar disorder", C = "Schizophrenia", D = "Obsessive-Compulsive Disorder (OCD)", CorrectAnswer = "B" },
                new Question { QuestionText = "What is the purpose of antidepressant medications?", A = "To increase dopamine levels", B = "To balance brain chemicals linked to mood", C = "To cure depression immediately", D = "To cause sleepiness", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is an early sign of schizophrenia?", A = "Sudden weight loss", B = "Hallucinations", C = "Obsessive hand-washing", D = "Increased energy levels", CorrectAnswer = "B" },
                
                new Question { QuestionText = "What is one way to support someone experiencing a panic attack?", A = "Tell them to calm down", B = "Help them control their breathing", C = "Ignore them until it passes", D = "Encourage them to panic more", CorrectAnswer = "B" },
                new Question { QuestionText = "Which mental health condition involves excessive worry about having a serious illness?", A = "Schizophrenia", B = "Health anxiety (hypochondria)", C = "Panic disorder", D = "Dissociative identity disorder", CorrectAnswer = "B" },
                new Question { QuestionText = "What is one of the biggest barriers to mental health treatment?", A = "Lack of available therapists", B = "Excessive advertising for therapy", C = "Too many treatment options", D = "A strong support system", CorrectAnswer = "A" },
                new Question { QuestionText = "Which of the following is a long-term coping strategy for managing stress?", A = "Drinking alcohol", B = "Practicing mindfulness", C = "Avoiding social interactions", D = "Ignoring the problem", CorrectAnswer = "B" },
                new Question { QuestionText = "Which disorder is characterized by extreme mood swings, including episodes of mania and depression?", A = "Obsessive-Compulsive Disorder (OCD)", B = "Bipolar disorder", C = "Generalized Anxiety Disorder (GAD)", D = "Panic disorder", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which factor can contribute to developing mental health issues?", A = "Genetics", B = "Trauma", C = "Chronic stress", D = "All of the above", CorrectAnswer = "D" },
                new Question { QuestionText = "Which technique is commonly used to help people manage negative thinking patterns?", A = "Cognitive restructuring", B = "Daydreaming", C = "Ignoring negative thoughts", D = "Sleeping longer", CorrectAnswer = "A" },
                new Question { QuestionText = "What is the term for a period of intense fear that peaks within minutes?", A = "Anxiety attack", B = "Panic attack", C = "Manic episode", D = "Psychotic break", CorrectAnswer = "B" },
                new Question { QuestionText = "Which self-care activity can improve mental health?", A = "Exercising regularly", B = "Ignoring problems", C = "Spending excessive time on social media", D = "Drinking sugary drinks", CorrectAnswer = "A" },
                new Question { QuestionText = "What is the best immediate response to someone expressing suicidal thoughts?", A = "Ignore them", B = "Tell them to get over it", C = "Encourage them to talk to a mental health professional", D = "Avoid discussing it", CorrectAnswer = "C" },
                new Question { QuestionText = "Which mental health condition is characterized by persistent feelings of sadness and loss of interest?", A = "Generalized Anxiety Disorder", B = "Major Depressive Disorder", C = "Bipolar Disorder", D = "Obsessive-Compulsive Disorder", CorrectAnswer = "B" },
                new Question { QuestionText = "Which mental health condition is linked to childhood trauma?", A = "Schizophrenia", B = "Post-Traumatic Stress Disorder (PTSD)", C = "Obsessive-Compulsive Disorder (OCD)", D = "Autism Spectrum Disorder", CorrectAnswer = "B" }

                // Add more questions here...
            };

            // Initialize answeredQuestions with false values (indicating that no question has been answered yet)
            Random rand = new Random();
            questions = questions.OrderBy(q => rand.Next()).Take(10).ToList();

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
                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Medium)", score);

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
                                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Medium)", score);

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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Medium)", score);

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
                        if (score >= 8) // Check if the challenge is passed
                        {
                            success.Play();
                            SaveChallengeDataSuccess(Login.CurrentUsername, "Completed", textBox2.Text, "Quiz Mental (Medium)", score);
                            MessageBox.Show("Returning to the homepage.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            TransitionToHomePage();
                        }
                        else
                        {
                            fail.Play();
                            SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Medium)", score);
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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Medium)", score);

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

        private void QuizMentalEasy_Load(object sender, EventArgs e)
        {

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
