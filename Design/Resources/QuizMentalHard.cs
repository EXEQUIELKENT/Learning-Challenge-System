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
    public partial class QuizMentalHard : Form
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
        public QuizMentalHard()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "Which of the following is a potential side effect of long-term use of benzodiazepines?", A = "Improved memory", B = "Increased energy", C = "Dependence and withdrawal", D = "Weight gain", CorrectAnswer = "C" },
                new Question { QuestionText = "What is the primary distinction between Bipolar I and Bipolar II disorder?", A = "Bipolar I involves hypomania, Bipolar II involves mania", B = "Bipolar I involves severe manic episodes, Bipolar II involves hypomania", C = "Bipolar II is more common", D = "Bipolar I requires no treatment", CorrectAnswer = "B" },
                new Question { QuestionText = "Which brain region is most often linked to the regulation of mood and is involved in disorders like depression and bipolar disorder?", A = "Frontal cortex", B = "Amygdala", C = "Hypothalamus", D = "Prefrontal cortex", CorrectAnswer = "D" },
                new Question { QuestionText = "What is the primary purpose of dialectical behavior therapy (DBT)?", A = "To treat obsessive-compulsive disorder", B = "To address the challenges of self-harm and emotional regulation", C = "To alleviate symptoms of schizophrenia", D = "To focus on interpersonal therapy", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is a known biological risk factor for developing schizophrenia?", A = "Low serotonin levels", B = "Increased dopamine activity", C = "Chronic stress", D = "High levels of cortisol", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which of the following mental health conditions is most strongly linked to hereditary factors?", A = "Post-Traumatic Stress Disorder (PTSD)", B = "Autism spectrum disorder", C = "Obsessive-Compulsive Disorder (OCD)", D = "Bipolar disorder", CorrectAnswer = "D" },
                new Question { QuestionText = "Which condition is primarily treated using the technique of Exposure and Response Prevention (ERP)?", A = "Generalized Anxiety Disorder", B = "Post-Traumatic Stress Disorder", C = "Obsessive-Compulsive Disorder", D = "Borderline Personality Disorder", CorrectAnswer = "C" },
                new Question { QuestionText = "What is the main difference between Major Depressive Disorder and Persistent Depressive Disorder (Dysthymia)?", A = "MDD lasts longer than Dysthymia", B = "Dysthymia is a milder, but longer-lasting condition", C = "MDD has no physical symptoms", D = "Dysthymia is associated with manic episodes", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is the primary feature of Narcissistic Personality Disorder?", A = "Excessive dependence on others", B = "Pattern of grandiosity and lack of empathy", C = "Excessive fear of abandonment", D = "Difficulty controlling anger", CorrectAnswer = "B" },
                new Question { QuestionText = "Which factor is most likely to increase the risk of developing an anxiety disorder?", A = "Having a supportive family environment", B = "Experiencing childhood trauma or stress", C = "Consuming a balanced diet", D = "Being naturally optimistic", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which neurotransmitter imbalance is most closely associated with the symptoms of schizophrenia?", A = "Serotonin", B = "Dopamine", C = "Glutamate", D = "Norepinephrine", CorrectAnswer = "B" },
                new Question { QuestionText = "What is the primary focus of Acceptance and Commitment Therapy (ACT)?", A = "To focus on identifying irrational thoughts", B = "To encourage acceptance of thoughts and emotions without judgment", C = "To change negative behaviors through reward systems", D = "To analyze past trauma and unresolved issues", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is a common symptom of Borderline Personality Disorder?", A = "Extreme fear of rejection", B = "Increased energy levels", C = "Intense self-esteem", D = "Constant high mood", CorrectAnswer = "A" },
                new Question { QuestionText = "Which therapeutic approach is most commonly used to treat individuals with Borderline Personality Disorder?", A = "Cognitive Behavioral Therapy (CBT)", B = "Dialectical Behavior Therapy (DBT)", C = "Psychodynamic therapy", D = "Hypnotherapy", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following conditions is associated with the highest risk of suicide?", A = "Post-Traumatic Stress Disorder (PTSD)", B = "Bipolar disorder", C = "Schizophrenia", D = "Major depressive disorder", CorrectAnswer = "D" },
                
                new Question { QuestionText = "Which of the following mental health conditions is often characterized by a person experiencing delusions and hallucinations?", A = "Obsessive-Compulsive Disorder (OCD)", B = "Anxiety disorder", C = "Schizophrenia", D = "Post-Traumatic Stress Disorder", CorrectAnswer = "C" },
                new Question { QuestionText = "Which of the following is a primary risk factor for developing alcohol use disorder?", A = "Having supportive relationships", B = "History of trauma or abuse", C = "Maintaining a healthy diet", D = "Being highly extroverted", CorrectAnswer = "B" },
                new Question { QuestionText = "What is the primary treatment for Attention-Deficit/Hyperactivity Disorder (ADHD) in adults?", A = "Antidepressant medication", B = "Stimulant medication and behavioral therapy", C = "Electroconvulsive therapy", D = "Deep brain stimulation", CorrectAnswer = "B" },
                new Question { QuestionText = "Which personality disorder is most commonly associated with manipulative behavior, lack of remorse, and disregard for others' rights?", A = "Antisocial Personality Disorder", B = "Narcissistic Personality Disorder", C = "Histrionic Personality Disorder", D = "Avoidant Personality Disorder", CorrectAnswer = "A" },
                new Question { QuestionText = "Which brain structure is primarily responsible for regulating emotional responses, particularly fear and aggression?", A = "Hippocampus", B = "Amygdala", C = "Thalamus", D = "Cerebellum", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which of the following is NOT a diagnostic criterion for Schizophrenia according to the DSM-5?", A = "Delusions", B = "Hallucinations", C = "Periods of mania", D = "Disorganized speech", CorrectAnswer = "C" },
                new Question { QuestionText = "Which neurotransmitter is most commonly targeted by medications for treating anxiety disorders?", A = "Serotonin", B = "Dopamine", C = "Glutamate", D = "Acetylcholine", CorrectAnswer = "A" },
                new Question { QuestionText = "Which cognitive distortion involves seeing things in only black-and-white terms, without recognizing middle ground?", A = "Overgeneralization", B = "All-or-nothing thinking", C = "Catastrophizing", D = "Mind reading", CorrectAnswer = "B" },
                new Question { QuestionText = "What is the primary characteristic of Dissociative Identity Disorder (DID)?", A = "Persistent fear of social situations", B = "Presence of two or more distinct personality states", C = "Obsessive need for symmetry", D = "Inability to experience pleasure", CorrectAnswer = "B" },
                new Question { QuestionText = "Which neurotransmitter imbalance is primarily linked to symptoms of Obsessive-Compulsive Disorder (OCD)?", A = "Serotonin", B = "Dopamine", C = "Norepinephrine", D = "GABA", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Which of the following disorders is most commonly associated with persistent, intrusive thoughts that lead to compulsive behaviors?", A = "Generalized Anxiety Disorder", B = "Obsessive-Compulsive Disorder", C = "Post-Traumatic Stress Disorder", D = "Borderline Personality Disorder", CorrectAnswer = "B" },
                new Question { QuestionText = "Which part of the brain is most responsible for regulating impulsive behaviors?", A = "Prefrontal cortex", B = "Cerebellum", C = "Hippocampus", D = "Medulla oblongata", CorrectAnswer = "A" },
                new Question { QuestionText = "What is the term for a false sensory perception that occurs without an external stimulus?", A = "Delusion", B = "Hallucination", C = "Disorganized thought", D = "Paranoia", CorrectAnswer = "B" },
                new Question { QuestionText = "Which class of drugs is most commonly prescribed for treating severe bipolar disorder?", A = "Selective Serotonin Reuptake Inhibitors (SSRIs)", B = "Mood stabilizers", C = "Beta-blockers", D = "Antihistamines", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is a hallmark symptom of psychosis?", A = "Obsessive worry", B = "Hallucinations or delusions", C = "Mood swings", D = "Compulsive behavior", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which psychological disorder is characterized by an extreme fear of social interactions?", A = "Agoraphobia", B = "Social Anxiety Disorder", C = "Panic Disorder", D = "Generalized Anxiety Disorder", CorrectAnswer = "B" },
                new Question { QuestionText = "Which term describes the coexistence of two or more disorders in the same individual?", A = "Dual diagnosis", B = "Comorbidity", C = "Dissociation", D = "Psychopathology", CorrectAnswer = "B" },
                new Question { QuestionText = "Which type of therapy is specifically designed to treat individuals with severe emotional dysregulation and self-harming behaviors?", A = "Cognitive Behavioral Therapy (CBT)", B = "Dialectical Behavior Therapy (DBT)", C = "Exposure Therapy", D = "Interpersonal Therapy", CorrectAnswer = "B" },
                new Question { QuestionText = "Which neurotransmitter is most associated with reward and motivation?", A = "Dopamine", B = "Serotonin", C = "GABA", D = "Acetylcholine", CorrectAnswer = "A" },
                new Question { QuestionText = "Which of the following is a key symptom of a manic episode in Bipolar Disorder?", A = "Excessive sleep", B = "Increased energy and impulsivity", C = "Low self-esteem", D = "Avoidance of social interaction", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which term describes the belief that one's thoughts are being broadcast to others?", A = "Delusion of control", B = "Thought broadcasting", C = "Magical thinking", D = "Hallucination", CorrectAnswer = "B" },
                new Question { QuestionText = "What is the term for an extreme loss of interest or pleasure in activities once enjoyed?", A = "Anhedonia", B = "Catatonia", C = "Derealization", D = "Somatization", CorrectAnswer = "A" },
                new Question { QuestionText = "Which personality disorder is most closely linked to a lack of empathy and manipulation?", A = "Histrionic Personality Disorder", B = "Borderline Personality Disorder", C = "Narcissistic Personality Disorder", D = "Antisocial Personality Disorder", CorrectAnswer = "D" },
                new Question { QuestionText = "Which of the following best describes the negative symptoms of schizophrenia?", A = "Hallucinations and delusions", B = "Lack of motivation and social withdrawal", C = "Disorganized speech", D = "Hyperactivity and excessive energy", CorrectAnswer = "B" },
                new Question { QuestionText = "Which disorder involves recurrent episodes of binge eating followed by compensatory behaviors such as purging?", A = "Anorexia Nervosa", B = "Bulimia Nervosa", C = "Binge-Eating Disorder", D = "Avoidant/Restrictive Food Intake Disorder", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Which mental health condition is characterized by alternating states of extreme elation and severe depression?", A = "Schizophrenia", B = "Bipolar Disorder", C = "Borderline Personality Disorder", D = "Generalized Anxiety Disorder", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is a major risk factor for developing Post-Traumatic Stress Disorder (PTSD)?", A = "High levels of social support", B = "Experiencing a traumatic event", C = "Genetics only", D = "Low levels of stress", CorrectAnswer = "B" },
                new Question { QuestionText = "Which disorder is characterized by an excessive need to be taken care of, leading to submissive and clinging behavior?", A = "Obsessive-Compulsive Personality Disorder", B = "Dependent Personality Disorder", C = "Avoidant Personality Disorder", D = "Schizoid Personality Disorder", CorrectAnswer = "B" },
                new Question { QuestionText = "What is the term for the condition where a person fakes or exaggerates symptoms of illness to gain medical attention?", A = "Somatic Symptom Disorder", B = "Factitious Disorder", C = "Illness Anxiety Disorder", D = "Conversion Disorder", CorrectAnswer = "B" },
                new Question { QuestionText = "Which of the following is a symptom of Catatonia?", A = "Excessive energy", B = "Extreme stillness and lack of movement", C = "Compulsive eating", D = "Panic attacks", CorrectAnswer = "B" }

                // Add more questions here...
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
                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Hard)", score);

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
                                SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Hard)", score);

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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Hard)", score);

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
                            SaveChallengeDataSuccess(Login.CurrentUsername, "Completed", textBox2.Text, "Quiz Mental (Hard)", score);
                            MessageBox.Show("Returning to the homepage.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            TransitionToHomePage();
                        }
                        else
                        {
                            fail.Play();
                            SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Hard)", score);
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
                        SaveChallengeDataFailed(Login.CurrentUsername, textBox2.Text, "Quiz Mental (Hard)", score);

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
