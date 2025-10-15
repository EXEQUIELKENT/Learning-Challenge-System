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
    public partial class DialogAnalysisHard : Form
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
        public DialogAnalysisHard()
        {
            InitializeComponent();
            questions = new List<Question>
            {
                new Question { QuestionText = "Ano ang tamang reaksyon kapag may nagsabi sa iyo ng ‘Kamusta?’ sa isang pormal na pag-uusap?", A = "Tumawa lang", B = "Magbigay ng maikling sagot na ‘Mabuti naman’", C = "Bumangon at maglakad palayo", D = "Sagutin ng ‘Ayos lang’", CorrectAnswer = "B" },
                new Question { QuestionText = "Paano mo ipapakita ang respeto sa isang mas nakatatanda kapag ikaw ay nag-uusap?", A = "Magsalita ng mabilis at malakas", B = "Gumamit ng ‘po’ at ‘opo’", C = "Tumingin sa mata habang nagsasalita", D = "Iwasan silang tanungin ng personal na bagay", CorrectAnswer = "B" },
                new Question { QuestionText = "Anong uri ng tono ang ginagamit sa isang kaswal na usapan?", A = "Mataas at mabilis", B = "Mababa at tahimik", C = "Tuwirang at matalim", D = "Malumanay at magaan", CorrectAnswer = "D" },
                new Question { QuestionText = "Paano ipinapakita ang pagpapahalaga sa oras ng iba sa isang pormal na pag-uusap?", A = "Magsimula agad ng walang intro", B = "Maghintay ng ilang minuto bago magsimula", C = "Magsimula ng may pagpapakilala at magpasalamat sa oras", D = "Magtanong agad ng personal na tanong", CorrectAnswer = "C" },
                new Question { QuestionText = "Anong reaksyon ang nararapat kapag ikaw ay nahulog ng kaunti sa isang seryosong pag-uusap?", A = "Magbiro upang magaan ang usapan", B = "Manahimik at magpatawad sa sarili", C = "Magpatuloy sa pagsasalita at magpatawa", D = "Magbigay ng seryosong paliwanag agad", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Ano ang unang hakbang sa isang pormal na interbyu sa trabaho?", A = "Sumagot agad ng tanong", B = "Magpakilala at magpasalamat sa pagkakataon", C = "Magtanong tungkol sa sahod", D = "Makipag-usap tungkol sa iyong hilig", CorrectAnswer = "B" },
                new Question { QuestionText = "Kapag nakikipag-usap ka sa isang boss, ano ang pinakamahalagang bagay na dapat tandaan?", A = "Magsalita nang hindi tumitingin sa mata", B = "Maging magalang at sumunod sa alituntunin", C = "Magsabi ng mga biro upang gawing magaan ang usapan", D = "Laging magpakita ng kasiyahan", CorrectAnswer = "B" },
                new Question { QuestionText = "Sa isang pormal na pagpupulong, anong klaseng tanong ang pinakamainam itanong?", A = "Mga tanong na may kinalaman sa personal na buhay ng tao", B = "Mga tanong na makakatulong sa layunin ng pagpupulong", C = "Mga tanong na hindi nauugnay sa paksa", D = "Mga tanong na nagpapakita ng kritisismo", CorrectAnswer = "B" },
                new Question { QuestionText = "Paano mo ipapakita ang interes sa isang kausap na nagsasabi ng isang personal na kwento?", A = "Magbitiw ng mga tanong at hinto ng saglit", B = "Pakinggan nang maayos at magbigay ng suportang reaksyon", C = "Pumunta agad sa iba pang paksa", D = "Magsalita nang mataas at mabilis", CorrectAnswer = "B" },
                new Question { QuestionText = "Sa isang negosasyon, ano ang pinakamahalagang elemento na dapat isaalang-alang?", A = "Pagsunod sa iyong plano ng hindi nag-iisip ng opinyon ng iba", B = "Pagkilala sa mga pangangailangan at interes ng ibang partido", C = "Pagpapanatili ng lakas ng loob", D = "Pagpapakita ng kontrol sa lahat ng aspeto ng usapan", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Paano makipag-usap nang maayos sa isang kaibigan na may ibang opinyon sa isang isyu?", A = "Iwasan ang usapin at magpatuloy sa ibang paksa", B = "Makipagdebate at gawing tama ang iyong pananaw", C = "Igalang ang kanilang opinyon at pakinggan ang kanilang panig", D = "Pilitin silang baguhin ang kanilang opinyon", CorrectAnswer = "C" },
                new Question { QuestionText = "Kapag nagsisimula ng isang usapan sa isang tao na hindi mo pa kilala, anong klaseng tanong ang nararapat itanong?", A = "Mga tanong tungkol sa kanilang personal na buhay", B = "Mga tanong tungkol sa kanilang trabaho o interes", C = "Mga tanong na may kinalaman sa kanilang relihiyon", D = "Mga tanong na hindi naaangkop sa sitwasyon", CorrectAnswer = "B" },
                new Question { QuestionText = "Anong uri ng diyalogo ang kadalasang ginagamit sa isang pamilya?", A = "Mahabang talakayan tungkol sa mga negosyo", B = "Masayahing pag-uusap at pagbibiro", C = "Mga seryosong usapan tungkol sa kinabukasan", D = "Pagbabanta at pagtatalo", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang isang magandang gawain sa panahon ng isang talakayan na may kasamang mga tagapakinig?", A = "Maghintay lamang at mag-absent", B = "Magsalita nang tuloy-tuloy at hindi tumitingin sa iba", C = "Makinig ng mabuti at magbigay ng positibong puna", D = "Magsalita agad kahit hindi natapos ang tanong", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang pinakamahalagang gawin kapag ikaw ay nag-aalala sa isang pormal na pag-uusap?", A = "Iwasan ang magtanong", B = "Magbigay ng tamang pagpapaliwanag at magpakita ng pagiging handa", C = "Magsalita nang hindi maayos", D = "Magsalita nang mabilis at magulo", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Paano mo ipapakita ang pagkakaunawaan sa isang kasamahan sa trabaho?", A = "Magbigay ng hindi kinakailangang puna", B = "Pakinggan sila at magbigay ng positibong reaksyon", C = "Manahimik lamang", D = "Magbigay ng maling impormasyon", CorrectAnswer = "B" },
                new Question { QuestionText = "Sa isang seryosong pag-uusap, anong klaseng tono ang pinakaangkop?", A = "Mabilis at matalim", B = "Malumanay at tahimik", C = "Mataas at mabilis", D = "Bilog at hindi tuwid", CorrectAnswer = "B" },
                new Question { QuestionText = "Anong reaksyon ang dapat gawin kapag may nakasagutan kang hindi magkasundo?", A = "Magbigay ng mahinahong opinyon at iwasan ang matinding argumento", B = "Magalit at umalis agad", C = "Iwasan ang pag-usapan pa ang isyu", D = "Magbiro at gawing magaan ang usapan", CorrectAnswer = "A" },
                new Question { QuestionText = "Paano dapat mag-reaksyon kapag may nagsabi sa iyo ng isang masamang balita?", A = "Pagtawanan ito", B = "Magbigay ng suporta at magtanong kung paano makakatulong", C = "Magbigay ng mga hindi kailangang payo", D = "Iwasan ang pag-usapan ito", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pinakamahalagang aspeto ng epektibong komunikasyon?", A = "Malinaw at organisadong mensahe", B = "Malakas na boses", C = "Mabilis na pagsasalita", D = "Paggamit ng maraming salita", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Ano ang kahulugan ng 'active listening' sa komunikasyon?", A = "Pagtango lamang habang nakikinig", B = "Pakikinig ng may buong atensyon at pagbibigay ng tamang tugon", C = "Pagsasalita kasabay ng kausap", D = "Pagsasagot kahit hindi pa tapos ang nagsasalita", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang isang epektibong paraan upang maiwasan ang hindi pagkakaunawaan sa usapan?", A = "Pagsigaw upang malinaw ang sinasabi", B = "Paggamit ng pormal na lengguwahe sa lahat ng oras", C = "Pagtatanong kung hindi sigurado sa sinabi ng kausap", D = "Pag-iwas sa usapan", CorrectAnswer = "C" },
                new Question { QuestionText = "Paano mo ipapakita na talagang interesado ka sa sinasabi ng kausap?", A = "Panay ang tango", B = "Pakikinig nang mabuti at pag-react sa tamang paraan", C = "Pagtanong ng walang koneksyon sa usapan", D = "Pagputol sa kanilang sinasabi upang ipahayag ang sarili", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang dapat iwasan kapag may hindi pagkakaunawaan sa isang pormal na usapan?", A = "Pagpapaliwanag ng panig nang mahinahon", B = "Pagtaas ng boses at pagiging emosyonal", C = "Pakikinig sa paliwanag ng iba", D = "Pagsasabi ng 'naiintindihan kita' bago magbigay ng opinyon", CorrectAnswer = "B" },
                new Question { QuestionText = "Sa anong sitwasyon kailangang gumamit ng mas pormal na tono sa pakikipag-usap?", A = "Kapag kasama ang matagal nang kaibigan", B = "Kapag nasa opisyal na pagpupulong", C = "Kapag nagbibiro sa kaibigan", D = "Kapag nagkukwento sa pamilya", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Ano ang pinakamahusay na paraan upang ayusin ang hindi pagkakaintindihan sa isang usapan?", A = "Maghanap ng kompromiso at linawin ang mga pagkakaiba", B = "Ipilit ang sariling opinyon", C = "Iwasan ang usapan at hindi na bumalik sa isyu", D = "Gamitin ang galit upang maipakita ang punto", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang dapat gawin kapag hindi mo naintindihan ang isang pahayag sa isang seryosong pag-uusap?", A = "Magkunwaring naiintindihan", B = "Magtanong ng malinaw na paliwanag", C = "Tumango at lumipat sa ibang paksa", D = "Tumahimik na lamang", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang dapat gawin upang mapanatili ang isang makabuluhang usapan?", A = "Magbigay ng makahulugang tanong at opinyon", B = "Lumihis sa paksa at pag-usapan ang ibang bagay", C = "Maghintay lamang ng pagkakataong makapagsalita", D = "Magsalita nang tuloy-tuloy nang hindi nagbibigay ng espasyo sa iba", CorrectAnswer = "A" },
                new Question { QuestionText = "Bakit mahalaga ang 'tone of voice' sa komunikasyon?", A = "Nakakatulong ito sa pag-unawa ng tunay na damdamin ng nagsasalita", B = "Mas mahalaga ang mga salita kaysa sa tono", C = "Hindi ito nakakaapekto sa kahulugan ng mensahe", D = "Ginagamit lamang ito sa drama at teatro", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang tamang reaksyon kapag may nagsabi sa iyo ng ‘Kamusta?’ sa isang pormal na pag-uusap?", A = "Tumawa lang", B = "Magbigay ng maikling sagot na ‘Mabuti naman’", C = "Bumangon at maglakad palayo", D = "Sagutin ng ‘Ayos lang’", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Paano mo ipapakita ang respeto sa isang mas nakatatanda kapag ikaw ay nag-uusap?", A = "Magsalita ng mabilis at malakas", B = "Gumamit ng ‘po’ at ‘opo’", C = "Tumingin sa mata habang nagsasalita", D = "Iwasan silang tanungin ng personal na bagay", CorrectAnswer = "B" },
                new Question { QuestionText = "Anong uri ng tono ang ginagamit sa isang kaswal na usapan?", A = "Mataas at mabilis", B = "Mababa at tahimik", C = "Tuwirang at matalim", D = "Malumanay at magaan", CorrectAnswer = "D" },
                new Question { QuestionText = "Paano ipinapakita ang pagpapahalaga sa oras ng iba sa isang pormal na pag-uusap?", A = "Magsimula agad ng walang intro", B = "Maghintay ng ilang minuto bago magsimula", C = "Magsimula ng may pagpapakilala at magpasalamat sa oras", D = "Magtanong agad ng personal na tanong", CorrectAnswer = "C" },
                new Question { QuestionText = "Anong reaksyon ang nararapat kapag ikaw ay nahulog ng kaunti sa isang seryosong pag-uusap?", A = "Magbiro upang magaan ang usapan", B = "Manahimik at magpatawad sa sarili", C = "Magpatuloy sa pagsasalita at magpatawa", D = "Magbigay ng seryosong paliwanag agad", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang unang hakbang sa isang pormal na interbyu sa trabaho?", A = "Sumagot agad ng tanong", B = "Magpakilala at magpasalamat sa pagkakataon", C = "Magtanong tungkol sa sahod", D = "Makipag-usap tungkol sa iyong hilig", CorrectAnswer = "B" },
                
                new Question { QuestionText = "Kapag nakikipag-usap ka sa isang boss, ano ang pinakamahalagang bagay na dapat tandaan?", A = "Magsalita nang hindi tumitingin sa mata", B = "Maging magalang at sumunod sa alituntunin", C = "Magsabi ng mga biro upang gawing magaan ang usapan", D = "Laging magpakita ng kasiyahan", CorrectAnswer = "B" },
                new Question { QuestionText = "Sa isang pormal na pagpupulong, anong klaseng tanong ang pinakamainam itanong?", A = "Mga tanong na may kinalaman sa personal na buhay ng tao", B = "Mga tanong na makakatulong sa layunin ng pagpupulong", C = "Mga tanong na hindi nauugnay sa paksa", D = "Mga tanong na nagpapakita ng kritisismo", CorrectAnswer = "B" },
                new Question { QuestionText = "Paano mo ipapakita ang interes sa isang kausap na nagsasabi ng isang personal na kwento?", A = "Magbitiw ng mga tanong at hinto ng saglit", B = "Pakinggan nang maayos at magbigay ng suportang reaksyon", C = "Pumunta agad sa iba pang paksa", D = "Magsalita nang mataas at mabilis", CorrectAnswer = "B" },
                new Question { QuestionText = "Sa isang negosasyon, ano ang pinakamahalagang elemento na dapat isaalang-alang?", A = "Pagsunod sa iyong plano ng hindi nag-iisip ng opinyon ng iba", B = "Pagkilala sa mga pangangailangan at interes ng ibang partido", C = "Pagpapanatili ng lakas ng loob", D = "Pagpapakita ng kontrol sa lahat ng aspeto ng usapan", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pinakamahalagang aspeto ng epektibong komunikasyon?", A = "Malinaw at organisadong mensahe", B = "Malakas na boses", C = "Mabilis na pagsasalita", D = "Paggamit ng maraming salita", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Ano ang kahulugan ng 'active listening' sa komunikasyon?", A = "Pagtango lamang habang nakikinig", B = "Pakikinig ng may buong atensyon at pagbibigay ng tamang tugon", C = "Pagsasalita kasabay ng kausap", D = "Pagsasagot kahit hindi pa tapos ang nagsasalita", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang isang epektibong paraan upang maiwasan ang hindi pagkakaunawaan sa usapan?", A = "Pagsigaw upang malinaw ang sinasabi", B = "Paggamit ng pormal na lengguwahe sa lahat ng oras", C = "Pagtatanong kung hindi sigurado sa sinabi ng kausap", D = "Pag-iwas sa usapan", CorrectAnswer = "C" },
                new Question { QuestionText = "Ano ang dapat gawin kapag hindi mo naintindihan ang isang pahayag sa isang seryosong pag-uusap?", A = "Magkunwaring naiintindihan", B = "Magtanong ng malinaw na paliwanag", C = "Tumango at lumipat sa ibang paksa", D = "Tumahimik na lamang", CorrectAnswer = "B" },
                new Question { QuestionText = "Bakit mahalaga ang 'tone of voice' sa komunikasyon?", A = "Nakakatulong ito sa pag-unawa ng tunay na damdamin ng nagsasalita", B = "Mas mahalaga ang mga salita kaysa sa tono", C = "Hindi ito nakakaapekto sa kahulugan ng mensahe", D = "Ginagamit lamang ito sa drama at teatro", CorrectAnswer = "A" },
                new Question { QuestionText = "Ano ang dapat gawin upang mapanatili ang isang makabuluhang usapan?", A = "Magbigay ng makahulugang tanong at opinyon", B = "Lumihis sa paksa at pag-usapan ang ibang bagay", C = "Maghintay lamang ng pagkakataong makapagsalita", D = "Magsalita nang tuloy-tuloy nang hindi nagbibigay ng espasyo sa iba", CorrectAnswer = "A" },
                
                new Question { QuestionText = "Paano dapat mag-reaksyon kapag may nagsabi sa iyo ng isang masamang balita?", A = "Pagtawanan ito", B = "Magbigay ng suporta at magtanong kung paano makakatulong", C = "Magbigay ng mga hindi kailangang payo", D = "Iwasan ang pag-usapan ito", CorrectAnswer = "B" },
                new Question { QuestionText = "Ano ang pinakamahalagang gawin kapag ikaw ay nag-aalala sa isang pormal na pag-uusap?", A = "Iwasan ang magtanong", B = "Magbigay ng tamang pagpapaliwanag at magpakita ng pagiging handa", C = "Magsalita nang hindi maayos", D = "Magsalita nang mabilis at magulo", CorrectAnswer = "B" },
                new Question { QuestionText = "Paano mo ipapakita ang pagkakaunawaan sa isang kasamahan sa trabaho?", A = "Magbigay ng hindi kinakailangang puna", B = "Pakinggan sila at magbigay ng positibong reaksyon", C = "Manahimik lamang", D = "Magbigay ng maling impormasyon", CorrectAnswer = "B" },
                new Question { QuestionText = "Paano makipag-usap nang maayos sa isang kaibigan na may ibang opinyon sa isang isyu?", A = "Iwasan ang usapin at magpatuloy sa ibang paksa", B = "Makipagdebate at gawing tama ang iyong pananaw", C = "Igalang ang kanilang opinyon at pakinggan ang kanilang panig", D = "Pilitin silang baguhin ang kanilang opinyon", CorrectAnswer = "C" },               // Add more questions here...
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
                FormTitle = "Dialog Analysis (Hard)",
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
                FormTitle = "Dialog Analysis (Hard)",
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
