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
            // If the same difficulty is already active, reset to normal mode
            if ((difficulty == "Easy" && isEasyMode) ||
                (difficulty == "Medium" && isMediumMode) ||
                (difficulty == "Hard" && isHardMode))
            {
                // Reset all flags
                isEasyMode = false;
                isMediumMode = false;
                isHardMode = false;

                // Reset to include all challenges
                filteredItems = new Dictionary<Image, string>(challengeDescriptions.ImageIdentifiers);

                // Update ComboBox with all challenges again
                comboBoxChallenge.Items.Clear();
                foreach (var item in filteredItems.Values)
                {
                    comboBoxChallenge.Items.Add(GetChallengeWithDifficulty(item));
                }

                MessageBox.Show("Randomization will now include all challenges.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Otherwise, set the new difficulty mode
            isEasyMode = isMediumMode = isHardMode = false;

            switch (difficulty)
            {
                case "Easy": isEasyMode = true; break;
                case "Medium": isMediumMode = true; break;
                case "Hard": isHardMode = true; break;
            }

            // Filter challenges by selected difficulty
            filteredItems = challengeDescriptions.ImageIdentifiers
                .Where(pair => pair.Value.Contains($"__{difficulty}_"))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            if (filteredItems.Count == 0)
            {
                MessageBox.Show($"No '{difficulty}' challenges found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                filteredItems = new Dictionary<Image, string>(challengeDescriptions.ImageIdentifiers);
                return;
            }

            // Update ComboBox with filtered items
            comboBoxChallenge.Items.Clear();
            foreach (var item in filteredItems.Values)
            {
                comboBoxChallenge.Items.Add(GetChallengeWithDifficulty(item));
            }

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
                //Fitness
                case "Push_Ups__Easy__F":
                    return "               PUSH UPS";
                case "Push_Ups__Medium_F":
                    return "               PUSH UPS";
                case "Push_Ups__Hard_F":
                    return "               PUSH UPS";
                case "Bear_Crawl__Easy_F":
                    return "             BEAR CRAWL";
                case "Bear_Crawl__Medium_F":
                    return "             BEAR CRAWL";
                case "Bear_Crawl__Hard_F":
                    return "             BEAR CRAWL";
                case "Bicycle_Crunches__Easy_F":
                    return "        BICYCLE CRUNCHES";
                case "Bicycle_Crunches__Medium_F":
                    return "        BICYCLE CRUNCHES";
                case "Bicycle_Crunches__Hard_F":
                    return "        BICYCLE CRUNCHES";
                case "Calf_Raises__Easy_F":
                    return "             CALF RAISES";
                case "Calf_Raises__Medium_F":
                    return "             CALF RAISES";
                case "Calf_Raises__Hard_F":
                    return "             CALF RAISES";
                case "Diamond_Push_Ups__Easy_F":
                    return "         DIAMOND PUSH UP";
                case "Diamond_Push_Ups__Medium_F":
                    return "         DIAMOND PUSH UP";
                case "Diamond_Push_Ups__Hard_F":
                    return "         DIAMOND PUSH UP";
                case "Glute_Bridges__Easy_F":
                    return "           GLUTE BRIDGES";
                case "Glute_Bridges__Medium_F":
                    return "           GLUTE BRIDGES";
                case "Glute_Bridges__Hard_F":
                    return "           GLUTE BRIDGES";
                case "High_Knees__Easy_F":
                    return "              HIGH KNEES";
                case "High_Knees__Medium_F":
                    return "              HIGH KNEES";
                case "High_Knees__Hard_F":
                    return "              HIGH KNEES";
                case "Jogging__Easy_F":
                    return "                JOGGING";
                case "Jogging__Medium_F":
                    return "                JOGGING";
                case "Jogging__Hard_F":
                    return "                JOGGING";
                case "Jumping_Jacks__Easy_F":
                    return "           JUMPING JACKS";
                case "Jumping_Jacks__Medium_F":
                    return "           JUMPING JACKS";
                case "Jumping_Jacks__Hard_F":
                    return "           JUMPING JACKS";
                case "Jumping_Squat__Easy_F":
                    return "              JUMP SQUAT";
                case "Jumping_Squat__Medium_F":
                    return "              JUMP SQUAT";
                case "Jumping_Squat__Hard_F":
                    return "              JUMP SQUAT";
                case "Leg_Raise__Easy_F":
                    return "                LEG RAISE";
                case "Leg_Raise__Medium_F":
                    return "                LEG RAISE";
                case "Leg_Raise__Hard_F":
                    return "                LEG RAISE";
                case "Mountain_Climbers__Easy_F":
                    return "        MOUNTAIN CLIMBERS";
                case "Mountain_Climbers__Medium_F":
                    return "        MOUNTAIN CLIMBERS";
                case "Mountain_Climbers__Hard_F":
                    return "        MOUNTAIN CLIMBERS";
                case "Planking__Easy_F":
                    return "               PLANKING";
                case "Planking__Medium_F":
                    return "               PLANKING";
                case "Planking__Hard_F":
                    return "               PLANKING";
                case "Reverse_Lunges__Easy_F":
                    return "          REVERSE LUNGES";
                case "Reverse_Lunges__Medium_F":
                    return "          REVERSE LUNGES";
                case "Reverse_Lunges__Hard_F":
                    return "          REVERSE LUNGES";
                case "Russian_Twist__Easy_F":
                    return "           RUSSIAN TWIST";
                case "Russian_Twist__Medium_F":
                    return "           RUSSIAN TWIST";
                case "Russian_Twist__Hard_F":
                    return "           RUSSIAN TWIST";
                case "Side_Lunges__Easy_F":
                    return "             SIDE LUNGES";
                case "Side_Lunges__Medium_F":
                    return "             SIDE LUNGES";
                case "Side_Lunges__Hard_F":
                    return "             SIDE LUNGES";
                case "Side_Plank__Easy_F":
                    return "              SIDE PLANK";
                case "Side_Plank__Medium_F":
                    return "              SIDE PLANK";
                case "Side_Plank__Hard_F":
                    return "              SIDE PLANK";
                case "Squat__Easy_F":
                    return "                  SQUAT";
                case "Squat__Medium_F":
                    return "                  SQUAT";
                case "Squat__Hard_F":
                    return "                  SQUAT";
                case "Standing_Side_Leg_Raises__Easy_F":
                    return "           SIDE LEG RAISE";
                case "Standing_Side_Leg_Raises__Medium__F":
                    return "           SIDE LEG RAISE";
                case "Standing_Side_Leg_Raises__Hard_F":
                    return "           SIDE LEG RAISE";
                case "Step_Up__Easy_F":
                    return "                STEP UP";
                case "Step_Up__Medium_F":
                    return "                STEP UP";
                case "Step_Up__Hard_F":
                    return "                STEP UP";
                case "Toe_Top__Easy_F":
                    return "                 TOE TOP";
                case "Toe_Top__Medium_F":
                    return "                 TOE TOP";
                case "Toe_Top__Hard_F":
                    return "                 TOE TOP";
                case "Wall_Sit__Easy_F":
                    return "                WALL SIT";
                case "Wall_Sit__Medium_F":
                    return "                WALL SIT";
                case "Wall_Sit__Hard_F":
                    return "                WALL SIT";
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
                //Fitness
                case "Push_Ups__Easy__F": return new PushUpsEasy();
                case "Push_Ups__Medium_F": return new PushUpsMedium();
                case "Push_Ups__Hard_F": return new PushUpsHard();
                case "Bear_Crawl__Easy_F": return new BearCrawlEasy();
                case "Bear_Crawl__Medium_F": return new BearCrawlMedium();
                case "Bear_Crawl__Hard_F": return new BearCrawlHard();
                case "Bicycle_Crunches__Easy_F": return new BicycleCrunchesEasy();
                case "Bicycle_Crunches__Medium_F": return new BicycleCrunchesMedium();
                case "Bicycle_Crunches__Hard_F": return new BicycleCrunchesHard();
                case "Calf_Raises__Easy_F": return new CalfRaisesEasy();
                case "Calf_Raises__Medium_F": return new CalfRaisesMedium();
                case "Calf_Raises__Hard_F": return new Calf_RaisesHard();
                case "Diamond_Push_Ups__Easy_F": return new DiamondPushUpEasy();
                case "Diamond_Push_Ups__Medium_F": return new DiamondPushUpsMedium();
                case "Diamond_Push_Ups__Hard_F": return new DiamondPushUpsHard();
                case "Glute_Bridges__Easy_F": return new GlutesBridgesEasy();
                case "Glute_Bridges__Medium_F": return new GlutesBridgesMedium();
                case "Glute_Bridges__Hard_F": return new GlutesBridgesHard();
                case "High_Knees__Easy_F": return new HighKnessEasy();
                case "High_Knees__Medium_F": return new HighKneesMedium();
                case "High_Knees__Hard_F": return new HighKneesHard();
                case "Jogging__Easy_F": return new JoggingEasy();
                case "Jogging__Medium_F": return new JoggingMedium();
                case "Jogging__Hard_F": return new JoggingHard();
                case "Jumping_Jacks__Easy_F": return new JumpingJacksEasy();
                case "Jumping_Jacks__Medium_F": return new JumpingJacksMedium();
                case "Jumping_Jacks__Hard_F": return new JumpingJacksHard();
                case "Jumping_Squat__Easy_F": return new JumpingSquatEasy();
                case "Jumping_Squat__Medium_F": return new JumpingSquatMedium();
                case "Jumping_Squat__Hard_F": return new JumpingSquatHard();
                case "Leg_Raise__Easy_F": return new LegRaisesEasy();
                case "Leg_Raise__Medium_F": return new LegRaisesMedium();
                case "Leg_Raise__Hard_F": return new LegRaisesHard();
                case "Mountain_Climbers__Easy_F": return new MountainClimbersEasy();
                case "Mountain_Climbers__Medium_F": return new MountainClimbersMedium();
                case "Mountain_Climbers__Hard_F": return new MountainClimbersHard();
                case "Planking__Easy_F": return new PlankingEasy();
                case "Planking__Medium_F": return new PlankingMedium();
                case "Planking__Hard_F": return new PlankingHard();
                case "Reverse_Lunges__Easy_F": return new ReverseLungesEasy();
                case "Reverse_Lunges__Medium_F": return new ReverseLungesMedium();
                case "Reverse_Lunges__Hard_F": return new ReverseLungesHard();
                case "Russian_Twist__Easy_F": return new RussianTwistEasy();
                case "Russian_Twist__Medium_F": return new RussianTwistMedium();
                case "Russian_Twist__Hard_F": return new RussianTwistHard();
                case "Side_Lunges__Easy_F": return new SideLungesEasy();
                case "Side_Lunges__Medium_F": return new SideLungesMedium();
                case "Side_Lunges__Hard_F": return new SideLungesHard();
                case "Side_Plank__Easy_F": return new SidePlankEasy();
                case "Side_Plank__Medium_F": return new SidePlankMedium();
                case "Side_Plank__Hard_F": return new SidePlankHard();
                case "Squat__Easy_F": return new SquatsEasy();
                case "Squat__Medium_F": return new SquatsMedium();
                case "Squat__Hard_F": return new SquatsHard();
                case "Standing_Side_Leg_Raises__Easy_F": return new StandingSideLegRaisesEasy();
                case "Standing_Side_Leg_Raises__Medium__F": return new StandingSideLegRaisesMedium();
                case "Standing_Side_Leg_Raises__Hard_F": return new StandingSideLegRaisesHard();
                case "Step_Up__Easy_F": return new StepUpEasy();
                case "Step_Up__Medium_F": return new StepUpMedium();
                case "Step_Up__Hard_F": return new StepUpHard();
                case "Toe_Top__Easy_F": return new ToeTopEasy();
                case "Toe_Top__Medium_F": return new ToeTopMedium();
                case "Toe_Top__Hard_F": return new ToeTopHard();
                case "Wall_Sit__Easy_F": return new WallSitEasy();
                case "Wall_Sit__Medium_F": return new WallSitMedium();
                case "Wall_Sit__Hard_F": return new WallSitHard();

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
