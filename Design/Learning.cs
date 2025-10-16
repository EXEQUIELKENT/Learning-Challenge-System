using DCP;
using DCP.Properties;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static Design.HOMEPAGE;
using System.IO;
using DCP.Resources;
using System.Speech.Recognition; // Include this namespace
using System.Globalization;

namespace DCP
{
    public partial class Learning : Form
    {
        private SoundPlayer player;
        private SoundPlayer randomizer;
        private SoundPlayer Openmic;
        private SoundPlayer Offmic;
        private LearningDescriptions learningDescriptions; // Instance of ChallengeDescriptions class
        private Dictionary<string, Form> formMap; // Map of string identifiers to forms
        private Random random = new Random(); // Random object for shuffling
        private Timer timer; // Timer for randomization duration
        private int timeElapsed; // Track time for shuffling
        private Image currentImage; // Variable to save the randomized image
        private SpeechRecognitionEngine recognizer;
        private bool isRecognizing = false; // Flag to track the recognition state
        private bool isEasyMode = false;
        private bool isMediumMode = false;
        private bool isHardMode = false;// Flag to track if "Easy" mode is enabled
        private Dictionary<Image, string> currentItems; // Current items being randomized
        private string selectedCategory;
        private int currentImageIndex = 0; // Track the current image index
        public Learning(string category)
        {
            InitializeComponent();

            selectedCategory = category;

            InitializeSpeechRecognition();

            Openmic = new SoundPlayer(DCP.Properties.Resources.OpenMic);
            Openmic.Load();

            Offmic = new SoundPlayer(DCP.Properties.Resources.OffMic);
            Offmic.Load();

            randomizer = new SoundPlayer(DCP.Properties.Resources.Randomizer6s);
            randomizer.Load();

            player = new SoundPlayer(DCP.Properties.Resources.Click2);
            player.Load();

            Offmic.Play();
            Openmic.Play();
            player.Play();
            System.Threading.Thread.Sleep(10);
            player.Stop();
            randomizer.Stop();
            Offmic.Stop();
            Openmic.Stop();

            label4.TextAlign = ContentAlignment.MiddleCenter;
            this.StartPosition = FormStartPosition.CenterScreen;

            learningDescriptions = new LearningDescriptions();
            // Initialize ChallengeDescriptions instance

            formMap = new Dictionary<string, Form> {

              //Learning
              { "Budget_Problem__Easy_M", null },
              { "Budget_Problem__Medium_M", null },
              { "Budget_Problem__Hard_M", null },
              { "Pattern_Recognition__Easy_M", null },
              { "Pattern_Recognition__Medium_M", null },
              { "Pattern_Recognition__Hard_M", null },
              { "Real_Life_Application__Easy_M", null },
              { "Real_Life_Application__Medium_M", null },
              { "Real_Life_Application__Hard_M", null },
              { "Math_Puzzle__Easy_M", null },
              { "Math_Puzzle__Medium_M", null },
              { "Math_Puzzle__Hard_M", null },
              { "Time_Challenge__Easy_M", null },
              { "Time_Challenge__Medium_M", null },
              { "Time_Challenge__Hard_M", null },

              };

            timer = new Timer();
            timer.Interval = 100; // Set the interval for timer tick (100ms)
            timer.Tick += Timer_Tick;

            random = new Random();

            this.FormClosing += Hompage_FormClosing;
            this.VisibleChanged += Hompage_VisibleChanged;
        }
        private void InitializeSpeechRecognition()
        {
            recognizer = new SpeechRecognitionEngine(new CultureInfo("en-US"));

            recognizer.SetInputToDefaultAudioDevice();

            // Add commands that you want to recognize
            Choices commands = new Choices();
            commands.Add(new string[] { "Start", "Play", "Stop", "Help", "Back", "Close", "Easy", "Medium", "Hard", "Left", "Right"});

            Grammar grammar = new Grammar(new GrammarBuilder(commands));
            recognizer.LoadGrammar(grammar); // Load the grammar for speech recognition
            recognizer.SetInputToDefaultAudioDevice(); // Use the default microphone

            // Event Handlers
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        }
        private void StartVoiceRecognition()
        {
            try
            {
                MicPictureBox.Enabled = false;
                MicPictureBox.Image = DCP.Properties.Resources.Mic_On_Learning;

                // Ask the user if they want to see the voice command list
                DialogResult result = MessageBox.Show("Do you want to see the Voice Command List?", "Voice Commands", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (voiceCommandsForm == null || voiceCommandsForm.IsDisposed)
                    {
                        voiceCommandsForm = new LearningVC();
                        voiceCommandsForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                        voiceCommandsForm.StartPosition = FormStartPosition.CenterScreen;
                        voiceCommandsForm.Opacity = 0;
                        voiceCommandsForm.FormClosing += (s, ev) => { FadeOutAndClose(voiceCommandsForm); };

                        voiceCommandsForm.Show();
                        FadeInForm(voiceCommandsForm);
                    }
                    else
                    {
                        voiceCommandsForm.BringToFront();
                    }
                }

                // Play the audio
                Openmic.Play();

                // Delay the start of the voice recognition until the audio has finished
                Timer audioTimer = new Timer();
                audioTimer.Interval = 3000; // 3 seconds delay
                audioTimer.Tick += (s, args) =>
                {
                    audioTimer.Stop();
                    audioTimer.Dispose();

                    // Start the voice recognition after the audio finishes
                    recognizer.RecognizeAsync(RecognizeMode.Multiple); // Start continuous recognition
                    isRecognizing = true; // Set the state to recognizing

                    MicPictureBox.Enabled = true;
                };

                audioTimer.Start();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Voice Command Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MicPictureBox.Enabled = true;
            }
        }
        private void StopVoiceRecognition()
        {
            try
            {
                MicPictureBox.Image = DCP.Properties.Resources.Mic_Off;
                MicPictureBox.Enabled = false;

                // Stop voice recognition safely
                if (recognizer != null)
                {
                    recognizer.RecognizeAsyncStop();
                    isRecognizing = false;
                }

                // Check if voiceCommandsForm is open before closing
                if (voiceCommandsForm != null && !voiceCommandsForm.IsDisposed)
                {
                    FadeOutAndClose(voiceCommandsForm);
                }
                // Play the audio
                Offmic.Play();

                // Delay stopping the voice recognition until the audio has finished
                Timer audioTimer = new Timer();
                audioTimer.Interval = 3000; // 3 seconds delay
                audioTimer.Tick += (s, args) =>
                {
                    audioTimer.Stop();
                    audioTimer.Dispose();

                    // Stop the voice recognition after the audio finishes
                    recognizer.RecognizeAsyncStop(); // Stop recognition
                    isRecognizing = false; // Set the state to not recognizing

                    MicPictureBox.Enabled = true;
                };
                audioTimer.Start();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Voice Command Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MicPictureBox.Enabled = true;
            }
        }
        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < 0.85)
            {
                return;
            }

            string command = e.Result.Text.ToLower(); // Normalize input to lowercase for easier comparison

            switch (command)
            {
                case "start":
                    RandomizerPictureBox();
                    break;
                case "play":
                    CheckPictureBox();
                    recognizer.RecognizeAsyncStop(); // Stop recognition after accepting
                    break;
                case "help":
                    pictureBox19_Click(null, null);
                    break;
                case "easy":
                    EasyPictureBox_Click(null, null);
                    break;
                case "medium":
                    MediumPictureBox_Click(null, null);
                    break;
                case "hard":
                    HardPictureBox_Click(null, null);
                    break;
                case "back":
                case "close":
                    Back();
                    break;
                case "left":
                    ArrowLeft_Click(null, null);
                    break;
                case "right":
                    ArrowRight_Click(null, null);
                    break;
                case "stop":
                    StopVoiceRecognition();
                    break;
                default:
                    MessageBox.Show($"Unknown Command: {command}", "Voice Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }
        private void Hompage_FormClosing(object sender, FormClosingEventArgs e) => StopVoiceRecognition();
        private void Hompage_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
                try
                {

                    // Delay stopping the voice recognition until the audio has finished
                    Timer audioTimer = new Timer();
                    audioTimer.Interval = 3000; // 3 seconds delay
                    audioTimer.Tick += (s, args) =>
                    {
                        audioTimer.Stop();
                        audioTimer.Dispose();

                        // Stop the voice recognition after the audio finishes
                        recognizer.RecognizeAsyncStop(); // Stop recognition
                        isRecognizing = false; // Set the state to not recognizing

                    };
                    audioTimer.Start();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Voice Command Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

        }
        private void FadeOutAndClose(Form form)
        {
            Timer fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 10;
            fadeOutTimer.Tick += (s, ev) =>
            {
                if (form.Opacity > 0)
                {
                    form.Opacity -= 0.05;
                }
                else
                {
                    fadeOutTimer.Stop();
                    form.Close();
                    isTransitioning = false; // Reset the flag
                    TogglePictureBoxState(true); // Re-enable picture boxes
                }
            };
            fadeOutTimer.Start();
        }
        private void FadeInForm(Form form)
        {
            Timer fadeInTimer = new Timer();
            fadeInTimer.Interval = 20;
            fadeInTimer.Tick += (s, e) =>
            {
                if (form.Opacity < 1)
                {
                    form.Opacity += 0.05;
                }
                else
                {
                    fadeInTimer.Stop();
                    fadeInTimer.Dispose();
                }
            };
            fadeInTimer.Start();
        }
        // Stop recognition when complete
        private void Recognizer_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            MessageBox.Show("Recognition session ended.", "Voice Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Back()
        {
            recognizer.RecognizeAsyncStop(); // Stop recognition
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
                    SelectionFitness selectionFitness = new SelectionFitness();
                    selectionFitness.StartPosition = FormStartPosition.CenterScreen;
                    selectionFitness.Opacity = 0;
                    selectionFitness.Show();

                    Timer fadeInTimer = new Timer();
                    fadeInTimer.Interval = 20;
                    fadeInTimer.Tick += (s2, ev2) =>
                    {
                        if (selectionFitness.Opacity < 1)
                        {
                            selectionFitness.Opacity += 0.05;
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

        private void RandomizerPictureBox()
        {
            // Disable check and x picture boxes
            pictureBox6.Enabled = false;
            pictureBox14.Enabled = false; // check picture box

            pictureBox6.Image = DCP.Properties.Resources.Ex;
            pictureBox14.Image = DCP.Properties.Resources.Ex;

            // Clear the richTextBox when randomizing again
            richTextBox1.Clear();

            timeElapsed = 0; // Reset time elapsed
            timer.Start(); // Start the timer for randomization
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            timeElapsed += timer.Interval;

            if (timeElapsed < 5000) // 5000ms = 5 seconds
            {
                // Determine the current pool based on the active mode
                // Determine the current pool based on the selected category and difficulty
                Dictionary<Image, string> currentPool = new Dictionary<Image, string>();

                if (isEasyMode)
                {
                    currentPool = currentPool
                        .Where(pair => pair.Value.Contains("Easy"))
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
                }
                else if (isMediumMode)
                {
                    currentPool = currentPool
                        .Where(pair => pair.Value.Contains("Medium"))
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
                }
                else if (isHardMode)
                {
                    currentPool = currentPool
                        .Where(pair => pair.Value.Contains("Hard"))
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
                }

                // Randomly select an item from the current pool
                currentImage = currentPool.Keys.ElementAt(random.Next(currentPool.Count));
                pictureBox8.Image = currentImage;

                // Update the challenge identifier and description
                if (currentPool.TryGetValue(currentImage, out string identifier))
                {
                    label8.Text = FormatIdentifier(identifier);

                    if (learningDescriptions.ImageDescriptions.TryGetValue(identifier, out string description))
                    {
                        richTextBox1.Text = description;
                    }
                }

                // Play sound at intervals
                if (timeElapsed == timer.Interval || timeElapsed % 6000 == 0)
                {
                    try
                    {
                        randomizer.Play();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error playing audio: " + ex.Message);
                    }
                }
            }
            else
            {
                // Stop the timer after 5 seconds
                timer.Stop();

                // Re-enable buttons and reset images
                pictureBox6.Enabled = true;
                pictureBox14.Enabled = true;
                HardPictureBox.Enabled = true;
                MediumPictureBox.Enabled = true;
                EasyPictureBox.Enabled = true;// check picture box
                ArrowLeft.Enabled = true;
                ArrowRight.Enabled = true;
                comboBoxChallenge.Enabled = true;
                MicPictureBox.Enabled = true;

                pictureBox6.Image = DCP.Properties.Resources.Random;
                pictureBox14.Image = DCP.Properties.Resources.Check;
            }
        }
        private string FormatIdentifier(string identifier)
        {
            switch (identifier)
            {
                //Learning
                case "Book_Summary__Easy_E":
                    return "           BOOK SUMMARY";
                case "Book_Summary__Medium_E":
                    return "           BOOK SUMMARY";
                case "Book_Summary__Hard_E":
                    return "           BOOK SUMMARY";
                case "Character_Analysis_English__Easy_E":
                    return "       CHARACTER ANALYSIS";
                case "Character_Analysis_English_Medium_E":
                    return "       CHARACTER ANALYSIS";
                case "Character_Analysis_English__Hard_E":
                    return "       CHARACTER ANALYSIS";
                case "Characteristic_Analysis_Filipino__Easy_F":
                    return "       CHARACTER ANALYSIS";
                case "Characteristic_Analysis_Filipino___Medium_F":
                    return "       CHARACTER ANALYSIS";
                case "Characteristic_Analysis_Filipino___Hard_F":
                    return "       CHARACTER ANALYSIS";
                case "Grammar__Easy_E":
                    return "    GRAMMAR QUESTION ENG";
                case "Grammar__Medium_E":
                    return "    GRAMMAR QUESTION ENG";
                case "Grammar__Hard_E":
                    return "    GRAMMAR QUESTION ENG";
                case "Vocabulary_Challenge__Easy_E":
                    return "         VOCABULARY ENG";
                case "Vocabulary_Challenge__Medium_E":
                    return "         VOCABULARY ENG";
                case "Vocabulary_Challenge__Hard_E":
                    return "         VOCABULARY ENG";
                case "Word_Count_Challenge_Easy_E":
                    return "            WORD COUNT";
                case "Word_Count_Challenge_Medium_E":
                    return "            WORD COUNT";
                case "Word_Count_Challenge_Hard_E":
                    return "            WORD COUNT";
                case "Dialog_Analysis__Easy_F":
                    return "       DIALOG ANALYSIS FIL";
                case "Dialog_Analysis__Medium_F":
                    return "       DIALOG ANALYSIS FIL";
                case "Dialog_Analysis__Hard_F":
                    return "       DIALOG ANALYSIS FIL";
                case "Filipino_Quiz__Easy_F":
                    return "           FILIPINO QUIZ";
                case "Filipino_Quiz__Medium_F":
                    return "           FILIPINO QUIZ";
                case "Filipino_Quiz__Hard_F":
                    return "           FILIPINO QUIZ";
                case "Poetry_Challenge__Easy_F":
                    return "              POETRY FIL";
                case "Poetry_Challenge__Medium_F":
                    return "              POETRY FIL";
                case "Poetry_Challenge__Hard_F":
                    return "              POETRY FIL";
                case "Story_Retelling__Easy_F":
                    return "       STORY RETELLING FIL";
                case "Story_Retelling__Medium_F":
                    return "       STORY RETELLING FIL";
                case "Story_Retelling__Hard_F":
                    return "       STORY RETELLING FIL";
                case "Budget_Problem__Easy_M":
                    return "         PROBLEM SOLVING";
                case "Budget_Problem__Medium_M":
                    return "         PROBLEM SOLVING";
                case "Budget_Problem__Hard_M":
                    return "         PROBLEM SOLVING";
                case "Pattern_Recognition__Easy_M":
                    return "      PATTERN RECOGNITION";
                case "Pattern_Recognition__Medium_M":
                    return "      PATTERN RECOGNITION";
                case "Pattern_Recognition__Hard_M":
                    return "      PATTERN RECOGNITION";
                case "Real_Life_Application__Easy_M":
                    return "          REAL APPLICATION";
                case "Real_Life_Application__Medium_M":
                    return "          REAL APPLICATION";
                case "Real_Life_Application__Hard_M":
                    return "          REAL APPLICATION";
                case "Math_Puzzle__Easy_M":
                    return "            MATH PUZZLE";
                case "Math_Puzzle__Medium_M":
                    return "            MATH PUZZLE";
                case "Math_Puzzle__Hard_M":
                    return "            MATH PUZZLE";
                case "Time_Challenge__Easy_M":
                    return "          TIME CHALLANGE";
                case "Time_Challenge__Medium_M":
                    return "          TIME CHALLANGE";
                case "Time_Challenge__Hard_M":
                    return "          TIME CHALLANGE";

                default:
                    return identifier; // Default case for unknown identifiers
            }
        }

        private void CheckPictureBox()
        {

            if (currentImage != null && learningDescriptions.ImageIdentifiers.ContainsKey(currentImage))
            {
                // Retrieve the identifier associated with the current image
                string identifier = learningDescriptions.ImageIdentifiers[currentImage];


                // Check if the identifier exists in the form map
                if (formMap.ContainsKey(identifier))
                {
                    // Lazy load the form if not already created
                    if (formMap[identifier] == null)
                    {
                        formMap[identifier] = CreateFormInstance(identifier);
                    }

                    // Begin fade-out transition
                    Timer fadeOutTimer = new Timer();
                    fadeOutTimer.Interval = 10; // Adjust interval for smoothness of fade-out
                    fadeOutTimer.Tick += (s, ev) =>
                    {
                        if (this.Opacity > 0)
                        {
                            this.Opacity -= 0.05; // Reduce opacity gradually
                        }
                        else
                        {
                            fadeOutTimer.Stop();
                            this.Hide(); // Hide the current form after fade-out

                            // Open the associated form with fade-in effect
                            Form associatedForm = formMap[identifier];
                            associatedForm.StartPosition = FormStartPosition.CenterScreen;
                            associatedForm.Opacity = 0; // Set initial opacity to 0
                            associatedForm.Show();

                            // Begin fade-in transition for the new form
                            Timer fadeInTimer = new Timer();
                            fadeInTimer.Interval = 20; // Adjust interval for smoothness of fade-in
                            fadeInTimer.Tick += (s2, ev2) =>
                            {
                                if (associatedForm.Opacity < 1)
                                {
                                    associatedForm.Opacity += 0.05; // Increase opacity gradually
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
                else
                {
                    MessageBox.Show("The challenge form could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Randomize a challenge first.", "No Challenge", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private Form CreateFormInstance(string formKey)
        {
            switch (formKey)
            {
                //Learning
                case "Book_Summary__Easy_E": return new BookSummaryEasy();
                case "Book_Summary__Medium_E": return new BookSummaryMedium();
                case "Book_Summary__Hard_E": return new BookSummaryHard();
                case "Character_Analysis_English__Easy_E": return new CharacterAnalysisEasy();
                case "Character_Analysis_English__Medium_E": return new CharacterAnalysisMedium();
                case "Character_Analysis_English__Hard_E": return new CharacterAnalysisHard();
                case "Characteristic_Analysis_Filipino__Easy_F": return new CharacterAnalysisFilEasy();
                case "Characteristic_Analysis_Filipino__Medium_F": return new CharacterAnalysisFilMedium();
                case "Characteristic_Analysis_Filipino__Hard_F": return new CharacterAnalysisFilHard();
                case "Grammar__Easy_E": return new GrammarEasy();
                case "Grammar__Medium_E": return new GrammarMedium();
                case "Grammar__Hard_E": return new GrammarHard();
                case "Vocabulary_Challenge__Easy_E": return new VocabularyEasy();
                case "Vocabulary_Challenge__Medium_E": return new VocabularyMedium();
                case "Vocabulary_Challenge__Hard_E": return new VocabularyHard();
                case "Word_Count_Challenge_Easy_E": return new WordCountEasy();
                case "Word_Count_Challenge_Medium_E": return new WordCountMedium();
                case "Word_Count_Challenge_Hard_E": return new WordCountHard();
                case "Dialog_Analysis__Easy_F": return new DialogAnalysisEasy();
                case "Dialog_Analysis__Medium_F": return new DialogAnalysisMedium();
                case "Dialog_Analysis__Hard_F": return new DialogAnalysisHard();
                case "Filipino_Quiz__Easy_F": return new FilipinoQuizEasy();
                case "Filipino_Quiz__Medium_F": return new FilipinoQuizMedium();
                case "Filipino_Quiz__Hard_F": return new FilipinoQuizHard();
                case "Poetry_Challenge__Easy_F": return new PoetryEasy();
                case "Poetry_Challenge__Medium_F": return new PoetryMedium();
                case "Poetry_Challenge__Hard_F": return new PoetryHard();
                case "Story_Retelling__Easy_F": return new StoryRetellingEasy();
                case "Story_Retelling__Medium_F": return new StoryRetellingMedium();
                case "Story_Retelling__Hard_F": return new StoryRetellingHard();
                case "Budget_Problem__Easy_M": return new BudjetProblemEasy();
                case "Budget_Problem__Medium_M": return new BudjetProblemMedium();
                case "Budget_Problem__Hard_M": return new BudjetProblemHard();
                case "Pattern_Recognition__Easy_M": return new PatternRecognitionEasy();
                case "Pattern_Recognition__Medium_M": return new PatternRecognitionMedium();
                case "Pattern_Recognition__Hard_M": return new PatternRecognitionHard();
                case "Real_Life_Application__Easy_M": return new RealLifeApplicationEasy();
                case "Real_Life_Application__Medium_M": return new RealLifeApplicationMedium();
                case "Real_Life_Application__Hard_M": return new RealLifeApplicationHard();
                case "Math_Puzzle__Easy_M": return new MathPuzzleEasy();
                case "Math_Puzzle__Medium_M": return new MathPuzzleMedium();
                case "Math_Puzzle__Hard_M": return new MathPuzzleHard();
                case "Time_Challenge__Easy_M": return new TimeChallengeEasy();
                case "Time_Challenge__Medium_M": return new TimeChallengeMedium();
                case "Time_Challenge__Hard_M": return new TimeChallengeHard();

                default:
                    MessageBox.Show("Form not defined for this challenge.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.V) // Check if the "V" key is pressed
            {
                if (isRecognizing)
                {
                    StopVoiceRecognition(); // Stop recognition if it's currently active
                }
                else
                {
                    StartVoiceRecognition(); // Start recognition if it's not active
                }
                return true;
            }
            if (keyData == Keys.Back)
            {
                player.Play();

                var result = MessageBox.Show("Do you want to go back?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                if (result == DialogResult.Yes)
                {
                    Timer fadeOutTimer = new Timer();
                    fadeOutTimer.Interval = 20; // Interval in milliseconds
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
                            SelectionLearning selection = new SelectionLearning();
                            selection.StartPosition = FormStartPosition.CenterScreen;
                            selection.Opacity = 0;
                            selection.Show();

                            Timer fadeInTimer = new Timer();
                            fadeInTimer.Interval = 20;
                            fadeInTimer.Tick += (s2, ev2) =>
                            {
                                if (selection.Opacity < 1)
                                {
                                    selection.Opacity += 0.05;
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
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private string GetChallengeWithDifficulty(string identifier)
        {
            // Remove trailing "_H" or " H" (if it exists at the end)
            if (identifier.EndsWith("_E"))
            {
                identifier = identifier.Substring(0, identifier.Length - 2); // Remove last 2 characters ("_H")
            }
            else if (identifier.EndsWith(" E"))
            {
                identifier = identifier.Substring(0, identifier.Length - 2); // Remove last 2 characters (" H")
            }
            else if (identifier.EndsWith("_F"))
            {
                identifier = identifier.Substring(0, identifier.Length - 2); // Remove last 2 characters (" H")
            }
            else if (identifier.EndsWith(" F"))
            {
                identifier = identifier.Substring(0, identifier.Length - 2); // Remove last 2 characters (" H")
            }
            else if (identifier.EndsWith("_M"))
            {
                identifier = identifier.Substring(0, identifier.Length - 2); // Remove last 2 characters (" H")
            }
            else if (identifier.EndsWith(" M"))
            {
                identifier = identifier.Substring(0, identifier.Length - 2); // Remove last 2 characters (" H")
            }

            // Replace underscores with spaces and convert to uppercase
            string formatted = identifier.Replace("_", " ").ToUpper();

            // Identify difficulty level
            string difficulty = "";
            if (formatted.Contains(" EASY"))
                difficulty = " (E)";
            else if (formatted.Contains(" MEDIUM"))
                difficulty = " (M)";
            else if (formatted.Contains(" HARD"))
                difficulty = " (H)";

            // Remove extra difficulty text from the middle
            formatted = formatted.Replace(" EASY", "").Replace(" MEDIUM", "").Replace(" HARD", "");

            // Return formatted challenge name
            return formatted + difficulty;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            player.Play();
            // Code for close confirmation and fade-out effect
            DialogResult result = MessageBox.Show("Are you sure you want to go close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
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

                        // Open Introduction form with fade-in effect
                        SelectionLearning selection = new SelectionLearning();
                        selection.StartPosition = FormStartPosition.CenterScreen;
                        selection.Opacity = 0;
                        selection.Show();

                        Timer fadeInTimer = new Timer();
                        fadeInTimer.Interval = 20;
                        fadeInTimer.Tick += (s2, ev2) =>
                        {
                            if (selection.Opacity < 1)
                            {
                                selection.Opacity += 0.05;
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
       
        private void Learning_Load(object sender, EventArgs e)
        {
            UpdateFilteredChallenges(); // Siguraduhing na-update ang `currentItems` bago punan ang ComboBox

            // Populate comboBoxChallenges with formatted challenge names + difficulty
            comboBoxChallenge.Items.Clear(); // I-clear para maiwasan ang duplicate entries
            foreach (var challenge in currentItems.Values)
            {
                string formattedChallenge = GetChallengeWithDifficulty(challenge);
                if (!comboBoxChallenge.Items.Contains(formattedChallenge))
                {
                    comboBoxChallenge.Items.Add(formattedChallenge);
                }
            }

            // Subscribe to event
            comboBoxChallenge.SelectedIndexChanged += comboBoxChallenge_SelectedIndexChanged;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            player.Play();
            // Code for logout confirmation and fade-out effect
            DialogResult result = MessageBox.Show("Are you sure you want to go back?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
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

                        // Open Introduction form with fade-in effect
                        SelectionLearning selection = new SelectionLearning();
                        selection.StartPosition = FormStartPosition.CenterScreen;
                        selection.Opacity = 0;
                        selection.Show();

                        Timer fadeInTimer = new Timer();
                        fadeInTimer.Interval = 20;
                        fadeInTimer.Tick += (s2, ev2) =>
                        {
                            if (selection.Opacity < 1)
                            {
                                selection.Opacity += 0.05;
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

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            player.Play();
            // Disable check and x picture boxes
            pictureBox2.Enabled = false;
            pictureBox14.Enabled = false;
            HardPictureBox.Enabled = false;
            MediumPictureBox.Enabled = false;
            EasyPictureBox.Enabled = false;// check picture box
            ArrowLeft.Enabled = false;
            ArrowRight.Enabled = false;
            comboBoxChallenge.Enabled = false;
            MicPictureBox.Enabled = false;

            pictureBox6.Image = DCP.Properties.Resources.Ex;
            pictureBox14.Image = DCP.Properties.Resources.Ex;

            // Clear the richTextBox when randomizing again
            richTextBox1.Clear();

            timeElapsed = 0; // Reset time elapsed
            timer.Start(); // Start the timer for randomization
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            player.Play();

            if (currentImage != null && learningDescriptions.ImageIdentifiers.ContainsKey(currentImage))
            {
                // Retrieve the identifier associated with the current image
                string identifier = learningDescriptions.ImageIdentifiers[currentImage];

                DialogResult dialogResult = MessageBox.Show("Do you want to accept the challenge?", "Challenge", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    // Check if the identifier exists in the form map
                    if (formMap.ContainsKey(identifier))
                    {
                        // Lazy load the form if not already created
                        if (formMap[identifier] == null)
                        {
                            formMap[identifier] = CreateFormInstance(identifier);
                        }

                        // Begin fade-out transition
                        Timer fadeOutTimer = new Timer();
                        fadeOutTimer.Interval = 10; // Adjust interval for smoothness of fade-out
                        fadeOutTimer.Tick += (s, ev) =>
                        {
                            if (this.Opacity > 0)
                            {
                                this.Opacity -= 0.05; // Reduce opacity gradually
                            }
                            else
                            {
                                fadeOutTimer.Stop();
                                this.Hide(); // Hide the current form after fade-out

                                // Open the associated form with fade-in effect
                                Form associatedForm = formMap[identifier];
                                associatedForm.StartPosition = FormStartPosition.CenterScreen;
                                associatedForm.Opacity = 0; // Set initial opacity to 0
                                associatedForm.Show();

                                // Begin fade-in transition for the new form
                                Timer fadeInTimer = new Timer();
                                fadeInTimer.Interval = 20; // Adjust interval for smoothness of fade-in
                                fadeInTimer.Tick += (s2, ev2) =>
                                {
                                    if (associatedForm.Opacity < 1)
                                    {
                                        associatedForm.Opacity += 0.05; // Increase opacity gradually
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
                    else
                    {
                        MessageBox.Show("The challenge form could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Randomize a challenge first.", "No Challenge", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EasyPictureBox_Click(object sender, EventArgs e)
        {
            player.Play();

            if (!isEasyMode)
            {
                // Reset all other modes
                isMediumMode = false;
                isHardMode = false;

                // Enable "Easy" mode
                isEasyMode = true;

                MessageBox.Show("Easy challenges will now be randomized.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Filter identifiers containing "Easy"
                var easyItems = learningDescriptions.ImageIdentifiers
                    .Where(pair => pair.Value.Contains("Easy")) // Filter by "Easy"
                    .ToDictionary(pair => pair.Key, pair => pair.Value); // Create a new dictionary

                if (easyItems.Count == 0)
                {
                    MessageBox.Show("No 'Easy' items found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isEasyMode = false; // Reset mode
                    return;
                }

            }
            else
            {
                // Disable "Easy" mode and reset to normal
                isEasyMode = false;

                MessageBox.Show("Randomization will now include all challenges.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                currentItems = learningDescriptions.ImageIdentifiers;

            }
        }

        private void MediumPictureBox_Click(object sender, EventArgs e)
        {
            player.Play();

            if (!isMediumMode)
            {
                // Reset all other modes
                isEasyMode = false;
                isHardMode = false;

                // Enable "Medium" mode
                isMediumMode = true;

                MessageBox.Show("Medium challenges will now be randomized.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Filter identifiers containing "Medium"
                var easyItems = learningDescriptions.ImageIdentifiers
                    .Where(pair => pair.Value.Contains("Medium")) // Filter by "Medium"
                    .ToDictionary(pair => pair.Key, pair => pair.Value); // Create a new dictionary

                if (easyItems.Count == 0)
                {
                    MessageBox.Show("No 'Medium' items found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isMediumMode = false; // Reset mode
                    return;
                }

            }
            else
            {
                // Disable "Medium" mode and reset to normal
                isMediumMode = false;

                MessageBox.Show("Randomization will now include all challenges.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                currentItems = learningDescriptions.ImageIdentifiers;

            }
        }

        private void HardPictureBox_Click(object sender, EventArgs e)
        {
            player.Play();

            if (!isHardMode)
            {
                // Reset all other modes
                isEasyMode = false;
                isMediumMode = false;

                // Enable "Hard" mode
                isHardMode = true;

                MessageBox.Show("Hard challenges will now be randomized.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Filter identifiers containing "Hard"
                var easyItems = learningDescriptions.ImageIdentifiers
                    .Where(pair => pair.Value.Contains("Hard")) // Filter by "Hard"
                    .ToDictionary(pair => pair.Key, pair => pair.Value); // Create a new dictionary

                if (easyItems.Count == 0)
                {
                    MessageBox.Show("No 'Hard' items found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isHardMode = false; // Reset mode
                    return;
                }

            }
            else
            {
                // Disable "Hard" mode and reset to normal
                isHardMode = false;

                MessageBox.Show("Randomization will now include all challenges.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                currentItems = learningDescriptions.ImageIdentifiers;

            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void ArrowLeft_Click(object sender, EventArgs e)
        {
            player.Play();

            UpdateFilteredChallenges(); // Update challenge list based on filters

            if (currentItems.Count == 0) return;

            List<Image> images = currentItems.Keys.ToList();
            currentImageIndex = (currentImageIndex - 1 + images.Count) % images.Count;
            currentImage = images[currentImageIndex];

            pictureBox8.Image = currentImage;

            if (currentItems.TryGetValue(currentImage, out string identifier))
            {
                label8.Text = FormatIdentifier(identifier);
                richTextBox1.Text = learningDescriptions.ImageDescriptions.ContainsKey(identifier) ? learningDescriptions.ImageDescriptions[identifier] : "";
            }
        }

        private void ArrowRight_Click(object sender, EventArgs e)
        {
            player.Play();

            UpdateFilteredChallenges(); // Update challenge list based on filters

            if (currentItems.Count == 0) return;

            List<Image> images = currentItems.Keys.ToList();
            currentImageIndex = (currentImageIndex + 1) % images.Count;
            currentImage = images[currentImageIndex];

            pictureBox8.Image = currentImage;

            if (currentItems.TryGetValue(currentImage, out string identifier))
            {
                label8.Text = FormatIdentifier(identifier);
                richTextBox1.Text = learningDescriptions.ImageDescriptions.ContainsKey(identifier) ? learningDescriptions.ImageDescriptions[identifier] : "";
            }
        }

        private void comboBoxChallenge_SelectedIndexChanged(object sender, EventArgs e)
        {
            player.Play();

            UpdateFilteredChallenges(); // Update challenge list based on filters

            if (comboBoxChallenge.SelectedItem == null) return;

            string selectedChallenge = comboBoxChallenge.SelectedItem.ToString();

            // Hanapin ang challenge sa na-filter na listahan
            var challengeEntry = currentItems
                .FirstOrDefault(x => GetChallengeWithDifficulty(x.Value) == selectedChallenge);

            if (challengeEntry.Key != null)
            {
                currentImage = challengeEntry.Key;
                pictureBox8.Image = currentImage;

                string identifier = challengeEntry.Value;
                label8.Text = FormatIdentifier(identifier);

                richTextBox1.Text = learningDescriptions.ImageDescriptions.ContainsKey(identifier)
                                    ? learningDescriptions.ImageDescriptions[identifier]
                                    : "";

                currentImageIndex = learningDescriptions.Images.IndexOf(currentImage);
            }
        }
        private void UpdateFilteredChallenges()
        {
            
        }

        private void MicPictureBox_Click(object sender, EventArgs e)
        {
            if (isRecognizing)
            {
                StopVoiceRecognition();
            }
            else
            {
                StartVoiceRecognition();
            }
        }

        private LearningGuide guide = null; // Class-level variable to track the Guide form
        private LearningVC voiceCommandsForm = null;
        private FeedBack feedback = null;
        private bool isTransitioning = false; // Flag to prevent multiple transitions
        private void pictureBox19_Click(object sender, EventArgs e)
        {
            if (isTransitioning) return;
            isTransitioning = true;
            TogglePictureBoxState(false);

            if (sender != null) player.Play();

            // Close other forms before opening Guide
            CloseAllFormsExcept("LearningGuide");

            // Open Guide with fade-in effect
            if (guide == null || guide.IsDisposed)
            {
                guide = new LearningGuide();
                guide.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                guide.StartPosition = FormStartPosition.CenterScreen;
                guide.Opacity = 0;

                guide.FormClosing += (s, ev) => { FadeOutAndClose(guide); };

                guide.Show();
                FadeInForm(guide);
            }
            else
            {
                guide.BringToFront();
            }
        }

        // Utility method to toggle PictureBox state
        private void TogglePictureBoxState(bool enabled)
        {
            pictureBox19.Enabled = enabled;
            pictureBox20.Enabled = enabled;
        }
        private void CloseAllFormsExcept(string formName)
        {
            // Close Guide if it's open and not the one being opened
            if (guide != null && !guide.IsDisposed && guide.Visible && formName != "LearningGuide")
            {
                FadeOutAndClose(guide);
            }

            // Close Feedback if it's open and not the one being opened
            if (feedback != null && !feedback.IsDisposed && feedback.Visible && formName != "FeedBack")
            {
                FadeOutAndClose(feedback);
            }
        }
        private void pictureBox20_Click(object sender, EventArgs e)
        {
            if (isTransitioning) return;
            isTransitioning = true;
            TogglePictureBoxState(false);

            if (sender != null) player.Play();

            // Close other forms before opening Feedback
            CloseAllFormsExcept("FeedBack");

            // Open Feedback with fade-in effect
            if (feedback == null || feedback.IsDisposed)
            {
                feedback = new FeedBack();
                feedback.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                feedback.StartPosition = FormStartPosition.CenterScreen;
                feedback.Opacity = 0;

                feedback.FormClosing += (s, ev) => { FadeOutAndClose(feedback); };

                feedback.Show();
                FadeInForm(feedback);
            }
            else
            {
                feedback.BringToFront();
            }
        }

        private void pictureBox7_Click_1(object sender, EventArgs e)
        {
            if (sender != null)
            {
                player.Play(); // Play sound only if triggered by a click, not by voice command
            }

            Timer fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
            fadeOutTimer.Tick += (s, ev) =>
            {
                if (this.Opacity > 0)
                {
                    this.Opacity -= 0.05; // Decrease opacity for fade-out
                }
                else
                {

                    fadeOutTimer.Stop();
                    this.Hide();

                    // Start the new form with fade-in
                    TableOfRecords tableOfRecords = new TableOfRecords();
                    tableOfRecords.StartPosition = FormStartPosition.CenterScreen;
                    tableOfRecords.Opacity = 0; // Start at 0 for fade-in effect
                    tableOfRecords.Show();

                    // Fade in the new form
                    Timer fadeInTimer = new Timer();
                    fadeInTimer.Interval = 20;
                    fadeInTimer.Tick += (s2, ev2) =>
                    {
                        if (tableOfRecords.Opacity < 1)
                        {
                            tableOfRecords.Opacity += 0.05; // Increase opacity for fade-in
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
}
