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
using System.Speech.Recognition; // Include this namespacee
using System.Globalization;

namespace Design
{
    public partial class HOMEPAGE : Form
    {
        private SoundPlayer player;
        private SoundPlayer randomizer;
        private SoundPlayer Openmic;
        private SoundPlayer Offmic;
        private ChallengeDescriptions challengeDescriptions; // Instance of ChallengeDescriptions class
        private Dictionary<string, Form> formMap; // Map of string identifiers to forms
        private Random random = new Random(); // Random object for shuffling
        private Timer timer; // Timer for randomization duration
        private int timeElapsed; // Track time for shuffling
        private Image currentImage; // Variable to save the randomized image
        private SpeechRecognitionEngine recognizer;
        private bool isRecognizing = false; // Flag to track the recognition state
        private int currentImageIndex = 0; // Track the current image index
        private bool isEasyMode = false;
        private bool isMediumMode = false;
        private bool isHardMode = false;
        private Dictionary<Image, string> filteredItems;

        public HOMEPAGE()
        {
            InitializeComponent();

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

            challengeDescriptions = new ChallengeDescriptions();
            // Initialize ChallengeDescriptions instance

            filteredItems = new Dictionary<Image, string>(challengeDescriptions.ImageIdentifiers);

            formMap = new Dictionary<string, Form> {

              //Math Challenges
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
              { "Addition_Easy_M", null },
              { "Addition_Medium_M", null },
              { "Addition_Hard_M", null },
              { "Subtraction_Easy_M", null },
              { "Subtraction_Medium_M", null },
              { "Subtraction_Hard_M", null },
              { "Multiplication_Easy_M", null },
              { "Multiplication_Medium_M", null },
              { "Multiplication_Hard_M", null },
              { "Division_Easy_M", null },
              { "Division_Medium_M", null },
              { "Division_Hard_M", null },
        };


            timer = new Timer();
            timer.Interval = 100; // Set the interval for timer tick (100ms)
            timer.Tick += Timer_Tick;

            random = new Random();

            this.FormClosing += Hompage_FormClosing;
            this.VisibleChanged += Hompage_VisibleChanged;
            // Attach events
            //pictureBox2.Click += RandomizerPictureBox;      // Randomizer PictureBox
            //pictureBox14.Click += CheckPictureBox;          // Check PictureBox

        }
        private void InitializeSpeechRecognition()
        {
            recognizer = new SpeechRecognitionEngine(new CultureInfo("en-US"));

            recognizer.SetInputToDefaultAudioDevice();

            // Add commands that you want to recognize
            Choices commands = new Choices();
            commands.Add(new string[] { "Start", "Play", "Stop", "Help", "Back", "Close", "Feedback", "Easy", "Medium", "Hard", "Learning", "Record", "Records", "Left", "Right", });

            Grammar grammar = new Grammar(new GrammarBuilder(commands));
            recognizer.LoadGrammar(grammar); // Load the grammar for speech recognition
            recognizer.SetInputToDefaultAudioDevice(); // Use the default microphone

            //recognizer.UpdateRecognizerSetting("CFGConfidenceRejectionThreshold", 40);
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
        private void SetDifficultyMode(string difficulty)
{
    if ((difficulty == "Easy" && isEasyMode) ||
        (difficulty == "Medium" && isMediumMode) ||
        (difficulty == "Hard" && isHardMode))
    {
        isEasyMode = isMediumMode = isHardMode = false;
        filteredItems = new Dictionary<Image, string>(challengeDescriptions.ImageIdentifiers);
        comboBoxChallenge.Items.Clear();
        foreach (var item in filteredItems.Values)
            comboBoxChallenge.Items.Add(GetChallengeWithDifficulty(item));

        MessageBox.Show("Randomization will now include all challenges.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }

    isEasyMode = isMediumMode = isHardMode = false;
    switch (difficulty)
    {
        case "Easy": isEasyMode = true; break;
        case "Medium": isMediumMode = true; break;
        case "Hard": isHardMode = true; break;
    }

    // FIXED: Support both single "_" and double "__"
    filteredItems = challengeDescriptions.ImageIdentifiers
        .Where(pair =>
            pair.Value.Contains($"__{difficulty}_") ||   // e.g. "__Easy__M"
            pair.Value.Contains($"_{difficulty}_") ||    // e.g. "_Easy_M"
            pair.Value.EndsWith($"_{difficulty}") ||     // e.g. "_Easy"
            pair.Value.Contains($"__{difficulty}")       // e.g. "__Easy"
        )
        .ToDictionary(pair => pair.Key, pair => pair.Value);

    if (filteredItems.Count == 0)
    {
        MessageBox.Show($"No '{difficulty}' challenges found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        filteredItems = new Dictionary<Image, string>(challengeDescriptions.ImageIdentifiers);
        return;
    }

    comboBoxChallenge.Items.Clear();
    foreach (var item in filteredItems.Values)
        comboBoxChallenge.Items.Add(GetChallengeWithDifficulty(item));

    MessageBox.Show($"{difficulty} challenges will now be randomized.", "Difficulty Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                case "feedback":
                    pictureBox20_Click(null, null);
                    break;
                case "easy":
                    pictureBox8_Click(null, null);
                    break;
                case "medium":
                    pictureBox9_Click(null, null);
                    break;
                case "hard":
                    pictureBox10_Click(null, null);
                    break;
                case "records":
                case "record":
                    pictureBox7_Click_1(null, null);
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

                    // Open Introduction form with Out-in effect
                    Introduction introduction = new Introduction();
                    introduction.StartPosition = FormStartPosition.CenterScreen;
                    introduction.Opacity = 0;
                    introduction.Show();

                    Timer fadeInTimer = new Timer();
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

        private void RandomizerPictureBox()
        {
            // Disable check and x picture boxes
            pictureBox7.Enabled = false;
            pictureBox2.Enabled = false;
            pictureBox14.Enabled = false; // check picture box

            pictureBox7.Image = DCP.Properties.Resources.Ex;
            pictureBox2.Image = DCP.Properties.Resources.Ex;
            pictureBox14.Image = DCP.Properties.Resources.Ex;

            // Clear the richTextBox when randomizing again
            richTextBox1.Clear();

            timeElapsed = 0; // Reset time elapsed
            timer.Start(); // Start the timer for randomization
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            timeElapsed += timer.Interval;

            if (timeElapsed < 5000) // 5 seconds duration
            {
                // Randomly select an image and update index
                var filteredImages = filteredItems.Keys.ToList();
                int newIndex = random.Next(filteredImages.Count);
                currentImage = filteredImages[newIndex];

                pictureBox6.Image = currentImage;

                if (filteredItems.TryGetValue(currentImage, out string identifier))
                {
                    label8.Text = FormatIdentifier(identifier);
                    if (challengeDescriptions.ImageDescriptions.TryGetValue(identifier, out string description))
                    {
                        richTextBox1.Text = description;
                    }
                }

                // Play the audio only once every 6 seconds or at the start of the timer
                if (timeElapsed == timer.Interval || timeElapsed % 6000 == 0)
                {
                    try
                    {
                        randomizer.Play(); // Play the randomizer sound
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error playing audio: " + ex.Message);
                    }
                }
            }
            else
            {
                timer.Stop();
                pictureBox9.Enabled = true;
                pictureBox10.Enabled = true;
                pictureBox8.Enabled = true;
                pictureBox7.Enabled = true;
                pictureBox2.Enabled = true;
                pictureBox14.Enabled = true;
                ArrowLeft.Enabled = true;
                ArrowRight.Enabled = true;
                comboBoxChallenge.Enabled = true;
                MicPictureBox.Enabled = true;

                pictureBox7.Image = DCP.Properties.Resources.Records;
                pictureBox2.Image = DCP.Properties.Resources.Random;
                pictureBox14.Image = DCP.Properties.Resources.Check;
            }
        }
        private string FormatIdentifier(string identifier)
        {
            switch (identifier)
            {
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
                case "Addition_Easy_M":
                    return "                ADDITION";
                case "Addition_Medium_M":
                    return "                ADDITION";
                case "Addition_Hard_M":
                    return "                ADDITION";
                case "Subtraction_Easy_M":
                    return "            SUBTRACTION";
                case "Subtraction_Medium_M":
                    return "            SUBTRACTION";
                case "Subtraction_Hard_M":
                    return "            SUBTRACTION";
                case "Multiplication_Easy_M":
                    return "           MULTIPLICATION";
                case "Multiplication_Medium_M":
                    return "           MULTIPLICATION";
                case "Multiplication_Hard_M":
                    return "           MULTIPLICATION";
                case "Division_Easy_M":
                    return "                DIVISION";
                case "Division_Medium_M":
                    return "                DIVISION";
                case "Division_Hard_M":
                    return "                DIVISION";
                default:
                    return identifier; // Default case for unknown identifiers
            }
        }
        private string GetChallengeWithDifficulty(string identifier)
        {
            // Remove trailing "_H" or " H" (if it exists at the end)
            if (identifier.EndsWith("_H"))
            {
                identifier = identifier.Substring(0, identifier.Length - 2); // Remove last 2 characters ("_H")
            }
            else if (identifier.EndsWith(" H"))
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
            else if (identifier.EndsWith("_E"))
            {
                identifier = identifier.Substring(0, identifier.Length - 2); // Remove last 2 characters (" H")
            }
            else if (identifier.EndsWith(" E"))
            {
                identifier = identifier.Substring(0, identifier.Length - 2); // Remove last 2 characters (" H")
            }
            else if (identifier.EndsWith("_M"))
            {
                identifier = identifier.Substring(0, identifier.Length - 2); // Remove last 2 characters (" H")
            }
            else if (identifier.EndsWith(" MA" +
                ""))
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
        private void CheckPictureBox()
        {

            if (currentImage != null && challengeDescriptions.ImageIdentifiers.ContainsKey(currentImage))
            {
                // Retrieve the identifier associated with the current image
                string identifier = challengeDescriptions.ImageIdentifiers[currentImage];

                
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
                //Math Challenges
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
                case "Addition_Easy_M": return new AdditionEasy();
                case "Addition_Medium_M": return new AdditionMedium();
                case "Addition_Hard_M": return new AdditionHard();
                case "Subtraction_Easy_M": return new SubtractionEasy();
                case "Subtraction_Medium_M": return new SubtractionMedium();
                case "Subtraction_Hard_M": return new SubtractionHard();
                case "Multiplication_Easy_M": return new MultiplicationEasy();
                case "Multiplication_Medium_M": return new MultiplicationMedium();
                case "Multiplication_Hard_M": return new MultiplicationHard();
                case "Division_Easy_M": return new DivisionEasy();
                case "Division_Medium_M": return new DivisionMedium();
                case "Division_Hard_M": return new DivisionHard();

                default:
                    MessageBox.Show("Form not defined for this challenge.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
            }
        }

        //Button Codes

        private bool isFadingIn = false; // Prevents actions during fade-in

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            player.Play();
            SetDifficultyMode("Easy");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            // Existing functionality
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            player.Play();
            // Disable check and x picture boxes
            pictureBox10.Enabled = false;
            pictureBox9.Enabled = false;
            pictureBox8.Enabled = false;
            pictureBox7.Enabled = false;
            pictureBox2.Enabled = false;
            pictureBox14.Enabled = false; // check picture box
            ArrowLeft.Enabled = false;
            ArrowRight.Enabled = false;
            comboBoxChallenge.Enabled = false; 
            MicPictureBox.Enabled  = false;

            pictureBox7.Image = DCP.Properties.Resources.Ex;
            pictureBox2.Image = DCP.Properties.Resources.Ex;
            pictureBox14.Image = DCP.Properties.Resources.Ex;

            // Clear the richTextBox when randomizing again
            richTextBox1.Clear();

            timeElapsed = 0; // Reset time elapsed
            timer.Start(); // Start the timer for randomization
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            // Existing functionality
        }
        private void pictureBox14_Click(object sender, EventArgs e)
        {
            player.Play(); // Play confirmation sound

            if (currentImage != null && challengeDescriptions.ImageIdentifiers.ContainsKey(currentImage))
            {
                // Retrieve the identifier associated with the current image
                string identifier = challengeDescriptions.ImageIdentifiers[currentImage];

                DialogResult dialogResult = MessageBox.Show("Do you want to accept the challenge?", "Challenge", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    // Ensure the correct form is opened based on the image
                    if (formMap.ContainsKey(identifier))
                    {
                        // Lazy load the form if not already created
                        if (formMap[identifier] == null)
                        {
                            formMap[identifier] = CreateFormInstance(identifier);
                        }

                        // Begin fade-out transition
                        Timer fadeOutTimer = new Timer();
                        fadeOutTimer.Interval = 10; // Adjust interval for smooth fade-out
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
                                fadeInTimer.Interval = 20; // Adjust interval for smooth fade-in
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
                MessageBox.Show("Select a challenge first.", "No Challenge Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            player.Play();
            SetDifficultyMode("Medium");
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            player.Play();
            SetDifficultyMode("Hard");
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            player.Play();
            // Code for logout confirmation and fade-out effect
            DialogResult result = MessageBox.Show("Are you sure you want to go logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                        Introduction introduction = new Introduction();
                        introduction.StartPosition = FormStartPosition.CenterScreen;
                        introduction.Opacity = 0;
                        introduction.Show();

                        Timer fadeInTimer = new Timer();
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
                        Introduction introduction = new Introduction();
                        introduction.StartPosition = FormStartPosition.CenterScreen;
                        introduction.Opacity = 0;
                        introduction.Show();

                        Timer fadeInTimer = new Timer();
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
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Existing functionality for label4 click (if any)
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
                            Introduction introduction = new Introduction();
                            introduction.StartPosition = FormStartPosition.CenterScreen;
                            introduction.Opacity = 0;
                            introduction.Show();

                            Timer fadeInTimer = new Timer();
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
            return base.ProcessCmdKey(ref msg, keyData);
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

        private LearningGuide guide = null;
        private FeedBack feedback = null;
        private LearningVC voiceCommandsForm = null;
        private bool isTransitioning = false; // Flag to prevent multiple transitions

        private void CloseAllFormsExcept(string formName)
        {
            // Close Guide if it's open and not the one being opened
            if (guide != null && !guide.IsDisposed && guide.Visible && formName != "Guide")
            {
                FadeOutAndClose(guide);
            }

            // Close Feedback if it's open and not the one being opened
            if (feedback != null && !feedback.IsDisposed && feedback.Visible && formName != "FeedBack")
            {
                FadeOutAndClose(feedback);
            }
        }

        private void FadeOutAndClose(Form form)
        {
            Timer fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 10;
            fadeOutTimer.Tick += (s, e) =>
            {
                if (form.Opacity > 0)
                {
                    form.Opacity -= 0.05;
                }
                else
                {
                    fadeOutTimer.Stop();
                    fadeOutTimer.Dispose();
                    form.Close();
                }
            };
            fadeOutTimer.Start();
        }

        // 📌 Open Guide Form (pictureBox19 Click)
        private void pictureBox19_Click(object sender, EventArgs e)
        {
            if (isTransitioning) return;
            isTransitioning = true;
            TogglePictureBoxState(false);

            if (sender != null) player.Play();

            // Close other forms before opening Guide
            CloseAllFormsExcept("Guide");

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

        // 📌 Open Feedback Form (pictureBox20 Click)
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
                    isTransitioning = false;
                    TogglePictureBoxState(true);
                }
            };
            fadeInTimer.Start();
        }

        // Utility method to toggle PictureBox state
        private void TogglePictureBoxState(bool enabled)
        {
            pictureBox19.Enabled = enabled;
            pictureBox20.Enabled = enabled;
        }

        // Utility method to end transition
        private void EndTransition()
        {
            isTransitioning = false; // Mark transition as complete
            TogglePictureBoxState(true); // Re-enable clicks on PictureBoxes
        }

        private void HOMEPAGE_Load(object sender, EventArgs e)
        {
            // Populate comboBoxChallenges with formatted challenge names + difficulty
            foreach (var challenge in challengeDescriptions.ImageIdentifiers.Values)
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

        private void ArrowLeft_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                player.Play(); // Play sound only if triggered by a click, not by voice command
            }

            var filteredImages = filteredItems.Keys.ToList();

            if (filteredImages.Count == 0) return; // Check if there are images

            currentImageIndex = (currentImageIndex - 1 + filteredImages.Count) % filteredImages.Count; // Move to the previous image

            if (filteredImages.Count == 0) return; // Check if there are images

            // Get the current image
            currentImage = filteredImages[currentImageIndex];
            pictureBox6.Image = currentImage; // Update PictureBox6

            // Update label8 text based on the selected image
            if (challengeDescriptions.ImageIdentifiers.TryGetValue(currentImage, out string identifier))
            {
                label8.Text = FormatIdentifier(identifier);

                // Automatically update the description in richTextBox1
                if (challengeDescriptions.ImageDescriptions.TryGetValue(identifier, out string description))
                {
                    richTextBox1.Text = description; // Set the description
                }
            }
        }

        private void ArrowRight_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                player.Play(); // Play sound only if triggered by a click, not by voice command
            }

            var filteredImages = filteredItems.Keys.ToList();

            if (filteredImages.Count == 0) return; // Check if there are images

            currentImageIndex = (currentImageIndex + 1) % filteredImages.Count; // Move to the next image

            if (filteredImages.Count == 0) return; // Check if there are images

            // Get the current image
            currentImage = filteredImages[currentImageIndex];
            pictureBox6.Image = currentImage; // Update PictureBox6

            // Update label8 text based on the selected image
            if (challengeDescriptions.ImageIdentifiers.TryGetValue(currentImage, out string identifier))
            {
                label8.Text = FormatIdentifier(identifier);

                // Automatically update the description in richTextBox1
                if (challengeDescriptions.ImageDescriptions.TryGetValue(identifier, out string description))
                {
                    richTextBox1.Text = description; // Set the description
                }
            }
        }

        private void comboBoxChallenge_SelectedIndexChanged(object sender, EventArgs e)
        {
            player.Play();

            if (comboBoxChallenge.SelectedItem == null) return;

            string selectedChallenge = comboBoxChallenge.SelectedItem.ToString();

            // Find the corresponding identifier
            var challengeEntry = filteredItems
                .FirstOrDefault(x => GetChallengeWithDifficulty(x.Value) == selectedChallenge);

            if (challengeEntry.Key != null) // If a match is found
            {
                currentImage = challengeEntry.Key;
                pictureBox6.Image = currentImage; // Update PictureBox

                string identifier = challengeEntry.Value;
                label8.Text = FormatIdentifier(identifier); // Update label8

                // Update description
                if (challengeDescriptions.ImageDescriptions.TryGetValue(identifier, out string description))
                {
                    richTextBox1.Text = description;
                }

                // Save current index for navigation
                currentImageIndex = challengeDescriptions.Images.IndexOf(currentImage);
            }
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
    }
}
