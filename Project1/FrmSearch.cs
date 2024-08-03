using System;
using System.IO;
using System.Windows.Forms;

namespace Project1
{
    public partial class FrmSearch : Form
    {
        public FrmSearch()
        {
            InitializeComponent();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtTransactionId.Text))
            {
                SearchMealDetailsById(TxtTransactionId.Text);
            }
            else
            {
                SearchMealDetailsByDate(DtTransDate.Text);
            }
        }

        private void SearchMealDetailsById(string transactionId)
        {
            // Define the file path
            string filePath = @"D:\DaCheekyCow.txt";

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                MessageBox.Show("The file does not exist.");
                return;
            }

            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Initialize variables to hold search results
            bool found = false;

            // Loop through lines to find matching entries
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith($"Transaction ID:{transactionId}"))
                {
                    // Print the entry
                    PrintEntry(lines, i);
                    found = true;
                    break; // Exit after finding the first match
                }
            }

            if (!found)
            {
                MessageBox.Show("No matching entry found.");
            }
        }

        public void SearchMealDetailsByDate(string dateInput)
        {
            // Define the file path
            string filePath = @"D:\DaCheekyCow.txt";

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                MessageBox.Show("The file does not exist.");
                return;
            }

            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Initialize variables to hold search results
            bool found = false;

            // Convert input date to DateTime object for comparison
            DateTime searchDate;
            if (!DateTime.TryParse(dateInput, out searchDate))
            {
                MessageBox.Show("Invalid date format.");
                return;
            }

            // Loop through lines to find matching entries
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Date:"))
                {
                    // Extract the date part from the line
                    string dateString = lines[i].Substring(6); // Skip "Date: "
                    DateTime entryDate;
                    if (DateTime.TryParse(dateString, out entryDate))
                    {
                        // Compare only the date parts
                        if (entryDate.Date == searchDate.Date)
                        {
                            // Print the entry
                            PrintEntry(lines, i);
                            found = true;
                        }
                    }
                }
            }

            if (!found)
            {
                MessageBox.Show("No matching entry found.");
            }
        }

        private void PrintEntry(string[] lines, int startIndex)
        {
            MessageBox.Show("Meal Details:");
            // Print the relevant entry starting from the index
            for (int i = startIndex; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    // Break on empty line (end of the entry)
                    break;
                }
                MessageBox.Show(lines[i]);
            }           
        }
    }
}
