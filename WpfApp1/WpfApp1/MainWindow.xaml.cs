using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Item> items = new List<Item>();
        List<string> lines = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            AdminScreen.Visibility = Visibility.Collapsed;
            ClientScreen.Visibility = Visibility.Collapsed;
            CheckOutScreen.Visibility = Visibility.Collapsed;
            Selection.Visibility = Visibility.Visible;
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            Selection.Visibility = Visibility.Collapsed;
            AdminScreen.Visibility = Visibility.Visible;
            addedItems.Items.Clear();
            //Get items FROM stock
            lines = File.ReadAllLines("Stock.txt").ToList();
            foreach (var line in lines)
            {
                string[] entries = line.Split(',');

                Item newItem = new Item(entries[0], entries[1]);
                items.Add(newItem);
                //get them on the screen 
                addedItems.Items.Add($"{newItem.ItemName}-{newItem.PriceOfItem}€");
            }           
        }

        private void addItem_Click(object sender, RoutedEventArgs e)
        {
            //Add items TO stock
            Item newItem = new Item(itemName.Text, price.Text);
            File.AppendAllText("Stock.txt", $"{newItem.ItemName},{newItem.PriceOfItem}\n");
            itemName.Text = null;
            price.Text = null;
            //lisa nähtavaks lisatud itemid
            addedItems.Items.Add($"{newItem.ItemName}-{newItem.PriceOfItem}€");
        }

        private void Client_Click(object sender, RoutedEventArgs e)
        {
            Selection.Visibility = Visibility.Collapsed;
            ClientScreen.Visibility = Visibility.Visible;
            lines = File.ReadAllLines("Stock.txt").ToList();
            stuffToBuy.Items.Clear();
            foreach (var line in lines)
            {
                string[] entries = line.Split(',');

                Item newItem = new Item(entries[0], entries[1]);
                items.Add(newItem);
                //get them on the screen 
                stuffToBuy.Items.Add($"{newItem.ItemName}-{newItem.PriceOfItem}€");
            }
            costlbl.Content = "0€";
            cartlbl.Content = 0;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (ClientScreen.Visibility == Visibility.Visible)
            {
                ClientScreen.Visibility = Visibility.Collapsed;
                Selection.Visibility = Visibility.Visible;
                File.WriteAllText("Check.txt", "");
            }

            if (AdminScreen.Visibility == Visibility.Visible)
            {
                AdminScreen.Visibility = Visibility.Collapsed;
                Selection.Visibility = Visibility.Visible;
            }
        }

        int totalCost = 0;

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            string[] checkOutItem = (stuffToBuy.SelectedItem.ToString().Trim('€')).Split('-');
            File.WriteAllText("Check.txt", $"{File.ReadAllText("Check.txt")}{checkOutItem[0]},{checkOutItem[1]}\n");

            List<string> cartItems = File.ReadAllLines("Check.txt").ToList();
            cartlbl.Content = cartItems.Count;

            List<string> prices = new List<string>();

            foreach (var i in cartItems)
            {
                string[] pricesFromName = i.Split(',');
                prices.Add(pricesFromName[1]);
            }

            totalCost = 0;
            
            foreach (var price in prices)
            {
                totalCost += int.Parse(price);
            }

            costlbl.Content = totalCost + "€";

        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            ClientScreen.Visibility = Visibility.Collapsed;
            CheckOutScreen.Visibility = Visibility.Visible;

            lines = File.ReadAllLines("Check.txt").ToList();
            foreach (var checkLine in lines)
            {
                string[] entries = checkLine.Split(',');

                Item newItem = new Item(entries[0], entries[1]);
                items.Add(newItem);
                //get them on the screen 
                cartStuff.Items.Add($"{newItem.ItemName}-{newItem.PriceOfItem}€");
            }

            checkOutTotalCost.Content = totalCost + "€";

            File.WriteAllText("Check.txt", "");
        }

        private void deleteSelectedBtn_Click(object sender, RoutedEventArgs e)
        {
            string itemName = addedItems.SelectedItem.ToString();
            string[] nameAndPrice = itemName.Split('-');
            lines = File.ReadAllLines("Stock.txt").ToList();
            string lineToDel = "";
            foreach (var line in lines)
            {
                string[] oneLine = line.Split(',');
                if (oneLine[0] == nameAndPrice[0])
                {
                    lineToDel = line;
                }
            }
            lines.Remove(lineToDel);
            File.WriteAllLines("Stock.txt", lines);
            addedItems.Items.Remove(addedItems.SelectedItem);
            addedItems.UnselectAll();
            deleteSelectedBtn.IsEnabled = false;
        }

        private void deleteAllBtn_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("Stock.txt","");
            addedItems.Items.Clear();
        }

        private void addedItems_GotFocus(object sender, RoutedEventArgs e)
        {
            deleteSelectedBtn.IsEnabled = true;
        }
    }
}
