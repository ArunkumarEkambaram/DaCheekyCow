using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        }

        private void SearchMealDetails(string criteria, string value)
        {
            // Define the file path
            string filePath = "meal_transactions.txt";

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("The file does not exist.");
                return;
            }

            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Initialize variables to hold search results
            bool found = false;

            // Loop through lines to find matching entries
            for (int i = 0; i < lines.Length; i++)
            {
                if (criteria.Equals("ID", StringComparison.OrdinalIgnoreCase) && lines[i].StartsWith($"Transaction ID: {value}"))
                {
                    // Print the entry
                    PrintEntry(lines, i);
                    found = true;
                    break;
                }
                else if (criteria.Equals("Date", StringComparison.OrdinalIgnoreCase) && lines[i].StartsWith($"Date: {value}"))
                {
                    // Print the entry
                    PrintEntry(lines, i);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine("No matching entry found.");
            }
        }

        private void PrintEntry(string[] lines, int startIndex)
        {
            Console.WriteLine("Meal Details:");
            // Print the relevant entry starting from the index
            for (int i = startIndex; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    // Break on empty line (end of the entry)
                    break;
                }
                Console.WriteLine(lines[i]);
            }
            Console.WriteLine();
        }
    }
}
