using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Project1
{
    public partial class FrmOrderPage : Form
    {
        public FrmOrderPage()
        {
            InitializeComponent();
        }

        class MealDetails
        {
            public string MealName { get; set; }
            public string ServingSize { get; set; }
            public float Price { get; set; }
            public int Stock { get; set; }
        }

        //Store Stock available as per the price
        private int availableStock;

        private static int counter = 0;

        //New Order
        List<MealDetails> NewOrder { get; set; } = new List<MealDetails>();

        // Define meals and serving sizes
        string[] meals = new string[]
        {
            "Full Irish", "Irish Grill", "Belmullet Grill", "Curry Special", "Irish Stew",
            "Student Stew", "Bacon & Cabbage", "Colcannon", "Boxty Special",
            "Atlantic Way", "Coddle", "Snack Box"
        };

        string[] sizes = new string[] { "Leprechaun", "Child", "Adult", "Student", "Cuchulainn" };

        // Define a 2D array for prices and stock levels
        // Format: {"price:stock"}
        string[,] mealDetails = new string[,]
        {
            // Leprechaun    Child       Adult        Student      Cuchulainn
            {"7.99:20",    "8.99:15",    "9.99:10",    "11.99:5",     "13.99:3"    }, // Full Irish
            {"8.99:25",    "9.99:20",    "10.99:15",   "11.99:10",    "12.99:5"    }, // Irish Grill
            {"9.99:30",    "10.99:25",   "11.99:20",   "12.99:15",    "13.99:10"   }, // Belmullet Grill
            {"6.99:15",    "7.99:10",    "8.99:5",     "9.99:3",      "10.99:2"    }, // Curry Special
            {"5.99:35",    "6.99:30",    "7.99:25",    "8.99:20",     "9.99:15"    }, // Irish Stew
            {"4.99:10",    "5.99:8",     "6.99:6",     "7.99:4",      "8.99:2"     }, // Student Stew
            {"8.99:40",    "9.99:35",    "10.99:30",   "11.99:25",    "12.99:20"   }, // Bacon & Cabbage
            {"2.99:50",    "3.99:45",    "4.99:40",    "5.99:35",     "6.99:30"    }, // Colcannon
            {"9.99:15",    "10.99:12",   "11.99:10",   "12.99:8",     "13.99:6"    }, // Boxty Special
            {"7.99:20",    "8.99:18",    "9.99:15",    "10.99:12",    "11.99:10"   }, // Atlantic Way
            {"6.99:10",    "7.99:8",     "8.99:6",     "9.99:5",      "10.99:3"    }, // Coddle
            {"5.99:25",    "6.99:20",    "7.99:15",    "8.99:10",     "9.99:5"     }  // Snack Box
        };

        //Generate New transaction Id
        static string GenerateUniqueId()
        {
            counter++;
            return $"{DateTime.Now:yyMMddHHmmss}-{counter:D4}";
        }

        //Search Meals beased on Meals and Serving Size
        public void SearchMeal(string[] meals, string[] sizes, string[,] mealDetails, string mealName, string sizeName)
        {
            int mealIndex = Array.IndexOf(meals, mealName);
            int sizeIndex = Array.IndexOf(sizes, sizeName);

            if (mealIndex == -1 || sizeIndex == -1)
            {
                MessageBox.Show("Meal or size not found.");
            }
            else
            {
                var details = mealDetails[mealIndex, sizeIndex].Split(':');
                TxtPrice.Text = details[0];
                availableStock = int.Parse(details[1]);
            }
        }

        //Update Stock after each order completed
        public void UpdateStock(string[] meals, string[] sizes, string[,] mealDetails, string mealName, string sizeName, string newStock)
        {
            int mealIndex = Array.IndexOf(meals, mealName);
            int sizeIndex = Array.IndexOf(sizes, sizeName);

            if (mealIndex == -1 || sizeIndex == -1)
            {
                MessageBox.Show("Meal or size not found.");
            }
            else
            {
                var details = mealDetails[mealIndex, sizeIndex].Split(':');
                mealDetails[mealIndex, sizeIndex] = $"{details[0]}:{newStock}";
            }
        }

        private void FrmOrderPage_Load(object sender, EventArgs e)
        {
            DdlMeals.Items.AddRange(meals);
            DdlSize.Items.AddRange(sizes);
        }

        private void DdlSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlMeals.SelectedIndex != -1 && DdlSize.SelectedIndex != -1)
            {
                string mealName = DdlMeals.SelectedItem.ToString();
                string sizeName = DdlSize.SelectedItem.ToString();
                SearchMeal(meals, sizes, mealDetails, mealName, sizeName);
            }
        }

        private void DdlMeals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlMeals.SelectedIndex != -1 && DdlSize.SelectedIndex != -1)
            {
                string mealName = DdlMeals.SelectedItem.ToString();
                string sizeName = DdlSize.SelectedItem.ToString();
                SearchMeal(meals, sizes, mealDetails, mealName, sizeName);
            }
        }

        //Add Order
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (DdlMeals.SelectedIndex > 0 && DdlSize.SelectedIndex > 0)
            {
                string mealName = DdlMeals.SelectedItem.ToString();
                string sizeName = DdlSize.SelectedItem.ToString();

                int.TryParse(TxtStock.Text, out int enteredStock);
                if (enteredStock > availableStock)
                {
                    if (MessageBox.Show($"Insufficient Stock available, Current available stocks are {availableStock} ") == DialogResult.OK)
                    {
                        TxtStock.Text = availableStock.ToString();
                    }
                }
                else
                {
                    string newStock = (availableStock - enteredStock).ToString();
                    UpdateStock(meals, sizes, mealDetails, mealName, sizeName, newStock);
                }
                NewOrder.Add(new MealDetails { MealName = mealName, ServingSize = sizeName, Price = float.Parse(TxtPrice.Text), Stock = int.Parse(TxtStock.Text) });
            }
            else
            {
                MessageBox.Show("Please select Meal Name and Serving Size, before placing order");
            }
        }

        private void BtnCompleteOrder_Click(object sender, EventArgs e)
        {
            // Generate transaction ID
            string transactionId = GenerateUniqueId();

            // Get current date and time
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            StringBuilder orders = new StringBuilder();
            foreach (var item in NewOrder)
            {
                orders.AppendLine($"Transaction ID: {transactionId}\n" +
                      $"Date: {date}\n" +
                      $"Meal: {item.MealName}\n" +
                      $"Size: {item.ServingSize}\n" +
                      $"Price: {item.Price:C}\n" +
                      $"Stock: {item.Stock}\n" +
                      "----------------------------------------");
            }

            MessageBox.Show(orders.ToString());
            File.AppendAllText(@"D:\DaCheekyCow.txt", orders.ToString() + Environment.NewLine);

            //Clear all after each order completed
            Clear();
            NewOrder.Clear();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            DdlMeals.SelectedIndex = -1;
            DdlSize.SelectedIndex = -1;
            TxtPrice.Text = string.Empty;
            TxtStock.Text = string.Empty;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            FrmSearch frmSearch = new FrmSearch();
            frmSearch.Show();
        }
    }
}