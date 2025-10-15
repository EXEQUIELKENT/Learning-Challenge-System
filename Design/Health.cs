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
    public partial class Health : Form
    {
        private SoundPlayer player;
        private SoundPlayer randomizer;
        private SoundPlayer Openmic;
        private SoundPlayer Offmic;
        private HealthDescriptios healthDescriptions; // Instance of ChallengeDescriptions class
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
        public Health(string category)
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

            healthDescriptions = new HealthDescriptios();
            // Initialize ChallengeDescriptions instance

            formMap = new Dictionary<string, Form> {

              //Health
              { "Gratitude__Easy_H", null },
              { "Gratitude__Medium_H", null },
              { "Gratitude__Hard_H", null },
              { "Hold_Your_Breath__Easy_H", null },
              { "Hold_Your_Breath__Medium_H", null },
              { "Hold_Your_Breath__Hard_H", null },
              { "Hydration__Easy_H", null },
              { "Hydration__Medium_H", null },
              { "Hydration__Hard_H", null },
              { "Mindful_Breathing__Easy_H", null },
              { "Mindful_Breathing__Medium_H", null },
              { "Mindful_Breathing__Hard_H", null },
              { "Mindful_Eating__Easy_H", null },
              { "Mindful_Eating__Medium_H", null },
              { "Mindful_Eating__Hard_H", null },
              { "No_Blinking__Easy_H", null },
              { "No_Blinking__Medium_H", null },
              { "No_Blinking__Hard_H", null },
              { "Quick_Stretching__Easy_H", null },
              { "Quick_Stretching__Medium_H", null },
              { "Quick_Stretching__Hard_H", null },
              { "Plan_your_week__Easy_H", null },
              { "Plan_your_week__Medium_H", null },
              { "Plan_your_week__Hard_H", null },
              { "Plan_Your_Week_Mental__Easy_H", null },
              { "Plan_Your_Week_Mental__Medium_H", null },
              { "Plan_Your_Week_Mental__Hard_H", null },
              { "Quiz_Mental__Easy_H", null },
              { "Quiz_Mental__Medium_H", null },
              { "Quiz_Mental__Hard_H", null },
              { "Reading_Time__Medium_H", null },
              { "Reading_Time__Hard_H", null },
              { "Reading_Time__Easy_H", null },
              { "Take_a_Cold_Shower__Easy_H", null },
              { "Take_a_Cold_Shower__Medium_H", null },
              { "Take_a_Cold_Shower__Hard_H", null },
              { "Walking_Easy_H", null },
              { "Walking_Medium_H", null },
              { "Walking_Hard_H", null },

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
                MicPictureBox.Image = DCP.Properties.Resources.Mic_On_HomePage;

                // Ask the user if they want to see the voice command list
                DialogResult result = MessageBox.Show("Do you want to see the Voice Command List?", "Voice Commands", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (voiceCommandsForm == null || voiceCommandsForm.IsDisposed)
                    {
                        voiceCommandsForm = new HealthVC();
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
            pictureBox3.Enabled = false;
            pictureBox14.Enabled = false; // check picture box

            pictureBox3.Image = DCP.Properties.Resources.Ex;
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

                if (!string.IsNullOrEmpty(selectedCategory))
                {
                    currentPool = healthDescriptions.ImageIdentifiers
                    .Where(pair =>
                        (selectedCategory == "Nutritional" && (pair.Value.Contains("Hydration") ||
                                                             pair.Value.Contains("Mindful_Eating"))) ||
                        (selectedCategory == "Mental" && (pair.Value.Contains("Gratitude") ||
                                                           pair.Value.Contains("Mindful_Breathing") ||
                                                           pair.Value.Contains("Quiz_Mental") ||
                                                           pair.Value.Contains("Plan_Your_Week_Mental") ||
                                                           pair.Value.Contains("Reading_Time"))) ||
                        (selectedCategory == "LifeStyle" && (pair.Value.Contains("Hold_Your_Breath") ||
                                                             pair.Value.Contains("No_Blinking") ||
                                                             pair.Value.Contains("Quick_Stretching") ||
                                                             pair.Value.Contains("Plan_your_week") ||
                                                             pair.Value.Contains("Take_a_Cold_Shower") ||
                                                             pair.Value.Contains("Walking")))
                    )
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
                }
                else
                {
                    // If no category is selected, include all categories
                    currentPool = new Dictionary<Image, string>(healthDescriptions.ImageIdentifiers);
                }

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



                // Check if the pool has items
                if (currentPool.Count == 0)
                {
                    timer.Stop();
                    MessageBox.Show("No challenges available for the selected mode.",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                // Randomly select an item from the current pool
                currentImage = currentPool.Keys.ElementAt(random.Next(currentPool.Count));
                pictureBox6.Image = currentImage;

                // Update the challenge identifier and description
                if (currentPool.TryGetValue(currentImage, out string identifier))
                {
                    label8.Text = FormatIdentifier(identifier);

                    if (healthDescriptions.ImageDescriptions.TryGetValue(identifier, out string description))
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
                // Stop the timer after 5 seconds
                timer.Stop();

                // Re-enable buttons and reset images
                pictureBox3.Enabled = true;
                pictureBox14.Enabled = true;
                HardPictureBox.Enabled = true;
                MediumPictureBox.Enabled = true;
                EasyPictureBox.Enabled = true;// check picture box
                ArrowLeft.Enabled = true;
                ArrowRight.Enabled = true;
                comboBoxChallenge.Enabled = true;
                MicPictureBox.Enabled = true;

                pictureBox3.Image = DCP.Properties.Resources.Random;
                pictureBox14.Image = DCP.Properties.Resources.Check;
            }
        }
        private string FormatIdentifier(string identifier)
        {
            switch (identifier)
            {
                //Health
                case "Gratitude__Easy_H":
                    return "             GRATITUDE";
                case "Gratitude__Medium_H":
                    return "             GRATITUDE";
                case "Gratitude__Hard_H":
                    return "             GRATITUDE";
                case "Hold_Your_Breath__Easy_H":
                    return "            HOLD BREATH";
                case "Hold_Your_Breath__Medium_H":
                    return "            HOLD BREATH";
                case "Hold_Your_Breath__Hard_H":
                    return "            HOLD BREATH";
                case "Hydration__Easy_H":
                    return "              HYDRATION";
                case "Hydration__Medium_H":
                    return "              HYDRATION";
                case "Hydration__Hard_H":
                    return "              HYDRATION";
                case "Mindful_Breathing__Easy_H":
                    return "        MINDFUL BREATHING";
                case "Mindful_Breathing__Medium_H":
                    return "        MINDFUL BREATHING";
                case "Mindful_Breathing__Hard_H":
                    return "        MINDFUL BREATHING";
                case "Mindful_Eating__Easy_H":
                    return "           MINDFUL EATING";
                case "Mindful_Eating__Medium_H":
                    return "           MINDFUL EATING";
                case "Mindful_Eating__Hard_H":
                    return "           MINDFUL EATING";
                case "No_Blinking__Easy_H":
                    return "             NO BLINKING";
                case "No_Blinking__Medium_H":
                    return "             NO BLINKING";
                case "No_Blinking__Hard_H":
                    return "             NO BLINKING";
                case "Quick_Stretching__Easy_H":
                    return "         QUICK STRETCHING";
                case "Quick_Stretching__Medium_H":
                    return "         QUICK STRETCHING";
                case "Quick_Stretching__Hard_H":
                    return "         QUICK STRETCHING";
                case "Plan_your_week__Easy_H":
                    return "         PLAN YOUR WEEK";
                case "Plan_your_week__Medium_H":
                    return "         PLAN YOUR WEEK";
                case "Plan_your_week__Hard_H":
                    return "          PLAN YOUR WEEK";
                case "Plan_Your_Week_Mental__Easy_H":
                    return "            MENTAL WEEK";
                case "Plan_Your_Week_Mental__Medium_H":
                    return "            MENTAL WEEK";
                case "Plan_Your_Week_Mental__Hard_H":
                    return "            MENTAL WEEK";
                case "Quiz_Mental__Easy_H":
                    return "             QUIZ MENTAL";
                case "Quiz_Mental__Medium_H":
                    return "             QUIZ MENTAL";
                case "Quiz_Mental__Hard_H":
                    return "             QUIZ MENTAL";
                case "Reading_Time__Easy_H":
                    return "            READING TIME";
                case "Reading_Time__Medium_H":
                    return "            READING TIME";
                case "Reading_Time__Hard_H":
                    return "            READING TIME";
                case "Take_a_Cold_Shower__Easy_H":
                    return "            COLD SHOWER";
                case "Take_a_Cold_Shower__Medium_H":
                    return "            COLD SHOWER";
                case "Take_a_Cold_Shower__Hard_H":
                    return "            COLD SHOWER";
                case "Walking_Easy_H":
                    return "                WALKING";
                case "Walking_Medium_H":
                    return "                WALKING";
                case "Walking_Hard_H":
                    return "                WALKING";

                default:
                    return identifier; // Default case for unknown identifiers
            }
        }
        private void CheckPictureBox()
        {

            if (currentImage != null && healthDescriptions.ImageIdentifiers.ContainsKey(currentImage))
            {
                // Retrieve the identifier associated with the current image
                string identifier = healthDescriptions.ImageIdentifiers[currentImage];


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
                //Health
                case "Gratitude__Easy_H": return new GratitudeEasy();
                case "Gratitude__Medium_H": return new GratitudeMedium();
                case "Gratitude__Hard_H": return new GratitudeHard();
                case "Hold_Your_Breath__Easy_H": return new HoldBreathEasy();
                case "Hold_Your_Breath__Medium_H": return new HoldBreathMedium();
                case "Hold_Your_Breath__Hard_H": return new HoldBreathHard();
                case "Hydration__Easy_H": return new HydrationEasy();
                case "Hydration__Medium_H": return new HydrationMedium();
                case "Hydration__Hard_H": return new HydrationHard();
                case "Mindful_Breathing__Easy_H": return new MindfulBreathingEasy();
                case "Mindful_Breathing__Medium_H": return new MindfulBreathingMedium();
                case "Mindful_Breathing__Hard_H": return new MindfulBreathingHard();
                case "Mindful_Eating__Easy_H": return new MindFulEatingEasy();
                case "Mindful_Eating__Medium_H": return new MindfulEatingMedium();
                case "Mindful_Eating__Hard_H": return new MindFulEatingHard();
                case "No_Blinking__Easy_H": return new NoBlinkingEasy();
                case "No_Blinking__Medium_H": return new NoBlinkingMedium();
                case "No_Blinking__Hard_H": return new NoBlinkingHard();
                case "Quick_Stretching__Easy_H": return new QuickStretchingEasy();
                case "Quick_Stretching__Medium_H": return new QuickStretchingMedium();
                case "Quick_Stretching__Hard_H": return new QuickStretchingHard();
                case "Plan_your_week__Easy_H": return new PlanYourWeekendEasy();
                case "Plan_your_week__Medium_H": return new PlanYourWeekendMedium();
                case "Plan_your_week__Hard_H": return new PlanYourWeekendHard();
                case "Plan_Your_Week_Mental__Easy_H": return new PlanYourWeekendMenEasy();
                case "Plan_Your_Week_Mental__Medium_H": return new PlanYourWeekendMenMedium();
                case "Plan_Your_Week_Mental__Hard_H": return new PlanYourWeekendMenHard();
                case "Quiz_Mental__Easy_H": return new QuizMentalEasy();
                case "Quiz_Mental__Medium_H": return new QuizMentalMedium();
                case "Quiz_Mental__Hard_H": return new QuizMentalHard();
                case "Reading_Time__Easy_H": return new ReadingTimeEasy();
                case "Reading_Time__Medium_H": return new ReadingTimeMedium();
                case "Reading_Time__Hard_H": return new ReadingTimeHard();
                case "Take_a_Cold_Shower__Easy_H": return new TakeAColdShowerEasy();
                case "Take_a_Cold_Shower__Medium_H": return new TakeAColdShowerMedium();
                case "Take_a_Cold_Shower__Hard_H": return new TakeAColdShowerHard();
                case "Walking_Easy_H": return new WalkingEasy();
                case "Walking_Medium_H": return new WalkingMedium();
                case "Walking_Hard_H": return new WalkingHard();

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
                            SelectionHealth selection = new SelectionHealth();
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
            if (identifier.EndsWith("_H"))
            {
                identifier = identifier.Substring(0, identifier.Length - 2); // Remove last 2 characters ("_H")
            }
            else if (identifier.EndsWith(" H"))
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
                        SelectionHealth selection = new SelectionHealth();
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
                        SelectionHealth selection = new SelectionHealth();
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

        private HealthGuide guide = null; // Class-level variable to track the Guide form
        private HealthVC voiceCommandsForm = null;
        private void pictureBox19_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                player.Play(); // Play sound only if triggered by a click, not by voice command
            }

            // Check if the Guide form is already open
            if (guide != null && !guide.IsDisposed)
            {
                guide.BringToFront();
                return;
            }

            // Create the Guide form
            guide = new HealthGuide();
            guide.FormBorderStyle = FormBorderStyle.FixedToolWindow; // Set to FixedToolWindow
            guide.StartPosition = FormStartPosition.CenterScreen;
            guide.Opacity = 0; // Start at 0 for fade-in effect

            // Add functionality to fade out and close the Guide form when the X button is clicked
            guide.FormClosing += (s, ev) =>
            {
                ev.Cancel = true; // Prevent immediate closure
                Timer fadeOutTimer = new Timer();
                fadeOutTimer.Interval = 10; // Adjust for speed of fade (lower = faster)
                fadeOutTimer.Tick += (s2, ev2) =>
                {
                    if (guide.Opacity > 0)
                    {
                        guide.Opacity -= 0.05; // Decrease opacity for fade-out
                    }
                    else
                    {
                        fadeOutTimer.Stop();
                        fadeOutTimer.Dispose();
                        guide.FormClosing -= null; // Remove event handler to avoid recursion
                        guide.Dispose(); // Dispose of the Guide form
                        guide = null; // Reset the reference
                    }
                };
                fadeOutTimer.Start();
            };

            // Show the Guide form
            guide.Show();

            // Fade-in effect for the Guide form
            Timer fadeInTimer = new Timer();
            fadeInTimer.Interval = 20;
            fadeInTimer.Tick += (s, ev) =>
            {
                if (guide.Opacity < 1)
                {
                    guide.Opacity += 0.05; // Increase opacity for fade-in
                }
                else
                {
                    fadeInTimer.Stop();
                    fadeInTimer.Dispose();
                }
            };
            fadeInTimer.Start();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
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

            pictureBox3.Image = DCP.Properties.Resources.Ex;
            pictureBox14.Image = DCP.Properties.Resources.Ex;

            // Clear the richTextBox when randomizing again
            richTextBox1.Clear();

            timeElapsed = 0; // Reset time elapsed
            timer.Start(); // Start the timer for randomization
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            player.Play();

            if (currentImage != null && healthDescriptions.ImageIdentifiers.ContainsKey(currentImage))
            {
                // Retrieve the identifier associated with the current image
                string identifier = healthDescriptions.ImageIdentifiers[currentImage];

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

        private void pictureBox7_Click(object sender, EventArgs e)
        {

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
                var easyItems = healthDescriptions.ImageIdentifiers
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

                currentItems = healthDescriptions.ImageIdentifiers;

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
                var easyItems = healthDescriptions.ImageIdentifiers
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

                currentItems = healthDescriptions.ImageIdentifiers;

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
                var easyItems = healthDescriptions.ImageIdentifiers
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

                currentItems = healthDescriptions.ImageIdentifiers;

            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Health_Load(object sender, EventArgs e)
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

        private void ArrowLeft_Click(object sender, EventArgs e)
        {
            player.Play();

            UpdateFilteredChallenges(); // Update challenge list based on filters

            if (currentItems.Count == 0) return;

            List<Image> images = currentItems.Keys.ToList();
            currentImageIndex = (currentImageIndex - 1 + images.Count) % images.Count;
            currentImage = images[currentImageIndex];

            pictureBox6.Image = currentImage;

            if (currentItems.TryGetValue(currentImage, out string identifier))
            {
                label8.Text = FormatIdentifier(identifier);
                richTextBox1.Text = healthDescriptions.ImageDescriptions.ContainsKey(identifier) ? healthDescriptions.ImageDescriptions[identifier] : "";
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

            pictureBox6.Image = currentImage;

            if (currentItems.TryGetValue(currentImage, out string identifier))
            {
                label8.Text = FormatIdentifier(identifier);
                richTextBox1.Text = healthDescriptions.ImageDescriptions.ContainsKey(identifier) ? healthDescriptions.ImageDescriptions[identifier] : "";
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
                pictureBox6.Image = currentImage;

                string identifier = challengeEntry.Value;
                label8.Text = FormatIdentifier(identifier);

                richTextBox1.Text = healthDescriptions.ImageDescriptions.ContainsKey(identifier)
                                    ? healthDescriptions.ImageDescriptions[identifier]
                                    : "";

                currentImageIndex = healthDescriptions.Images.IndexOf(currentImage);
            }
        }
        private void UpdateFilteredChallenges()
        {
            // Initialize a filtered list based on selected category and difficulty
            Dictionary<Image, string> filteredChallenges = new Dictionary<Image, string>();

            if (!string.IsNullOrEmpty(selectedCategory))
            {
                filteredChallenges = healthDescriptions.ImageIdentifiers
                    .Where(pair =>
                        (selectedCategory == "Nutritional" && (pair.Value.Contains("Hydration") || pair.Value.Contains("Mindful_Eating"))) ||
                        (selectedCategory == "Mental" && (pair.Value.Contains("Gratitude") || pair.Value.Contains("Mindful_Breathing") ||
                                                           pair.Value.Contains("Quiz_Mental") || pair.Value.Contains("Plan_Your_Week_Mental") ||
                                                           pair.Value.Contains("Reading_Time"))) ||
                        (selectedCategory == "LifeStyle" && (pair.Value.Contains("Hold_Your_Breath") || pair.Value.Contains("No_Blinking") ||
                                                             pair.Value.Contains("Quick_Stretching") || pair.Value.Contains("Plan_your_week") ||
                                                             pair.Value.Contains("Take_a_Cold_Shower") || pair.Value.Contains("Walking"))))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            else
            {
                filteredChallenges = new Dictionary<Image, string>(healthDescriptions.ImageIdentifiers);
            }

            // Apply Difficulty Sorting
            if (isEasyMode)
            {
                filteredChallenges = filteredChallenges
                    .Where(pair => pair.Value.Contains("Easy"))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            else if (isMediumMode)
            {
                filteredChallenges = filteredChallenges
                    .Where(pair => pair.Value.Contains("Medium"))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            else if (isHardMode)
            {
                filteredChallenges = filteredChallenges
                    .Where(pair => pair.Value.Contains("Hard"))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            // If no valid challenges exist, reset and show an error
            if (filteredChallenges.Count == 0)
            {
                MessageBox.Show("No challenges available for the selected category and difficulty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Assign to global variable
            currentItems = filteredChallenges;
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
