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
using ClosedXML.Excel;


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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            player.Play();

            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("There are no records to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Do you want to save your records as an Excel file?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
            {
                MessageBox.Show("Export canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Save Records to Excel";
                    saveFileDialog.FileName = $"{Login.CurrentUsername}_Records.xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        using (var workbook = new ClosedXML.Excel.XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Challenge Records");

                            // 🔹 Title
                            worksheet.Cell("A1").Value = $"{Login.CurrentUsername}'s Challenge Records";
                            worksheet.Range("A1:D1").Merge();
                            worksheet.Row(1).Height = 30;
                            worksheet.Cell("A1").Style.Font.Bold = true;
                            worksheet.Cell("A1").Style.Font.FontSize = 16;
                            worksheet.Cell("A1").Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                            worksheet.Cell("A1").Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;
                            worksheet.Cell("A1").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LimeGreen;
                            worksheet.Cell("A1").Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                            // 🔹 Column Headers
                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                worksheet.Cell(2, i + 1).Value = dataGridView1.Columns[i].HeaderText;
                            }

                            var headerRange = worksheet.Range(2, 1, 2, dataGridView1.Columns.Count);
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("#98FB98"); // Pale Green
                            headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                            headerRange.Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                            headerRange.Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;

                            // 🔹 Data Rows
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                                {
                                    worksheet.Cell(i + 3, j + 1).Value = dataGridView1.Rows[i].Cells[j].Value?.ToString();
                                }
                            }

                            // 🔹 Data Styling
                            var dataRange = worksheet.Range(3, 1, dataGridView1.Rows.Count + 2, dataGridView1.Columns.Count);
                            dataRange.Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                            dataRange.Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                            dataRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                            dataRange.Style.Alignment.Vertical = ClosedXML.Excel.XLAlignmentVerticalValues.Center;

                            // 🔹 Alternating Row Colors (Pale Green and White)
                            for (int row = 3; row <= dataGridView1.Rows.Count + 2; row++)
                            {
                                if (row % 2 == 0)
                                    worksheet.Row(row).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("#98FB98"); // Pale Green
                                else
                                    worksheet.Row(row).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.White;
                            }

                            // 🔹 Auto-fit + Make columns slightly wider
                            worksheet.Columns().AdjustToContents();
                            foreach (var column in worksheet.Columns())
                            {
                                column.Width += 20; // add extra width for better spacing
                            }

                            // 🔹 Freeze header row
                            worksheet.SheetView.FreezeRows(2);

                            // 🔹 Optional: timestamp
                            worksheet.Cell(dataGridView1.Rows.Count + 4, 1).Value =
                                $"Exported on: {DateTime.Now:MMMM dd, yyyy - hh:mm tt}";
                            worksheet.Cell(dataGridView1.Rows.Count + 4, 1).Style.Font.Italic = true;
                            worksheet.Cell(dataGridView1.Rows.Count + 4, 1).Style.Font.FontColor = ClosedXML.Excel.XLColor.Gray;
                            worksheet.Range(dataGridView1.Rows.Count + 4, 1, dataGridView1.Rows.Count + 4, dataGridView1.Columns.Count)
                                .Merge();

                            workbook.SaveAs(filePath);
                        }

                        MessageBox.Show("Records have been successfully saved to Excel.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving Excel file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
