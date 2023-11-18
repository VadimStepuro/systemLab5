using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using systemLab5.models;
using systemLab5.services;

namespace systemLab5
{
    public partial class Guarantee : Window
    {
        private int numTasks;
        private int quant;
            
        public Guarantee()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();

            quant = Int32.Parse(minTimeBox.Text);
            numTasks = Int32.Parse(numOfTasksBox.Text);

            for (int i = 0; i < numTasks; i++)
            {

                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Margin = new Thickness(3);
                stackPanel.Height = 50;

                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = "Time";
                textBlock1.FontSize = 10;
                textBlock1.Height = 25;
                textBlock1.Width = 25;
                textBlock1.Margin = new Thickness(10, 0, 10, 0);

                TextBox textBox1 = createBox();

                stackPanel.Children.Add(textBlock1);
                stackPanel.Children.Add(textBox1);

                listBox.Items.Add(stackPanel);

            }

        }

        public static NumericBox createBox()
        {
            NumericBox box = new NumericBox();
            box.Width = 50;
            box.Height = 25;
            return box;
        }

        public static List<int> CastValues(IEnumerable inputData, int itemId)
        {
            return inputData.Cast<StackPanel>()
                            .Select(i => int.Parse(((TextBox)i.Children[itemId]).Text))
                            .ToList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GuaranteeTaskService.SetLogger((message) => { logger.Text += message; });
            GuaranteeTaskService.ChangeState();
            GuaranteeTaskService.SetQuant(quant);
            List<int> time = CastValues(listBox.Items, 1);

            GuaranteeTaskService.SetTasks(time);
            GuaranteeTaskService.StartTasks();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GuaranteeTaskService.ChangeState();
        }

        private static void validateTextBox(TextBox textBox)
        {
            if (!int.TryParse(textBox.Text, out _))
            {
                if (textBox.Text.Length > 1)
                    textBox.Text = textBox.Text[..^1];
                else
                    textBox.Text = "";
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            validateTextBox((TextBox)sender);
        }
    }
}
