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
using System.IO;
using DCP.Resources;
using Newtonsoft.Json;
using System.Media;


namespace DCP
{
    public partial class TableOfRecords : Form
    {
        private SoundPlayer player;
        public TableOfRecords()
        {
            InitializeComponent();

            player = new SoundPlayer(DCP.Properties.Resources.Click2);
            player.Load();

            player.Play();
            System.Threading.Thread.Sleep(10);
            player.Stop();

            //Task.Run(() => LoadDataGridViewAsync());
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
        private void TableOfRecords_Load(object sender, EventArgs e)
        {

            Task.Run(() => LoadDataGridViewAsync());
        }

        // Event handler for when the form is activated (useful if returning from another form)
        private async Task LoadDataGridViewAsync()
        {
            string username = Login.CurrentUsername;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("No user is logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string filePath = $"{username}_challenge.json";

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"No records found for {username}.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string jsonData;

            try
            {
                jsonData = File.ReadAllText(filePath); // Synchronous reading
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file: {ex.Message}", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var challengeRecords = JsonConvert.DeserializeObject<List<dynamic>>(jsonData);

            if (challengeRecords == null || challengeRecords.Count == 0)
            {
                MessageBox.Show($"No records found for {username}.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            const int chunkSize = 10; // Adjust as needed
            for (int i = 0; i < challengeRecords.Count; i += chunkSize)
            {
                var chunk = challengeRecords.Skip(i).Take(chunkSize);

                foreach (var record in chunk)
                {
                    string challenge = record.FormTitle;
                    string performance;

                    if (record.Reps != null)
                    {
                        performance = $"{record.Reps} Reps";
                    }
                    else if (record.Rot != null) // Check for "Rot" and "Glass" fields
                    {
                        performance = $"{record.Rot} Rot";
                    }
                    else if (record.Glass != null) 
                    {
                        performance = $"{record.Glass} Glass";
                    }
                    else if (record.Score != null)
                    {
                        performance = $"{record.Score} Score";
                    }
                    else if (record.Grade != null)
                    {
                        performance = $"{record.Grade} Grade";
                    }
                    else if (record.Words != null)
                    {
                        performance = $"{record.Words} Words";
                    }
                    else if (record.Challenge != null) // Fallback to "Challenge" field
                    {
                        performance = record.Challenge.ToString();
                    }
                    else
                    {
                        performance = "N/A"; // Default value if no relevant data exists
                    }

                    string time = record.Time;
                    string date = record.Date;

                    Invoke((Action)(() =>
                    {
                        dataGridView1.Rows.Add(date, challenge, performance, time);
                    }));
                }

                await Task.Delay(100); // Simulate delay for lazy loading
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            player.Play();
            // Ensure a row is selected
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one row to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string filePath = $"{Login.CurrentUsername}_challenge.json";

            // Load existing data
            var jsonData = "[]";

            if (File.Exists(filePath))
            {
#if NETCOREAPP || NET5_0_OR_GREATER
        jsonData = await File.ReadAllTextAsync(filePath);
#else
                jsonData = File.ReadAllText(filePath);
#endif
            }

            var challenges = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData);

            // Confirm deletion
            var result = MessageBox.Show("Are you sure you want to delete the selected record(s)?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Loop through selected rows and remove data
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (row.Cells["Date"].Value == null) continue;

                    string date = row.Cells["Date"].Value.ToString();
                    string formTitle = row.Cells["Challenge"].Value.ToString();
                    string performance = row.Cells["Performance"].Value.ToString();
                    string time = row.Cells["Time"].Value.ToString();

                    // Remove the matching record from the JSON list
                    challenges.RemoveAll(c =>
                        c.ContainsKey("Date") && c["Date"].ToString() == date &&
                        c.ContainsKey("FormTitle") && c["FormTitle"].ToString() == formTitle &&
                        c.ContainsKey("Time") && c["Time"].ToString() == time);

                    // Remove the row from the DataGridView
                    dataGridView1.Rows.Remove(row);
                }

                // Save the updated list back to the JSON file
                var updatedJsonData = JsonConvert.SerializeObject(challenges, Formatting.Indented);
#if NETCOREAPP || NET5_0_OR_GREATER
        await File.WriteAllTextAsync(filePath, updatedJsonData);
#else
                File.WriteAllText(filePath, updatedJsonData);
#endif

                MessageBox.Show("Selected record(s) deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
