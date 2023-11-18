using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using static System.Net.Mime.MediaTypeNames;

namespace systemLab5
{
    /// <summary>
    /// Логика взаимодействия для Priority.xaml
    /// </summary>
    public partial class Priority : Window
    {
        private int numTasks;
        private int numPriorities;
        private int quant;

        public Priority()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();

            numPriorities = Int32.Parse(numOfPioritiesBox.Text);
            quant = Int32.Parse(minTimeBox.Text);
            numTasks = Int32.Parse(numOfTasksBox.Text);

            for (int i = 0; i < numTasks; i++) {

                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Margin = new Thickness(3);
                stackPanel.Height = 50;

                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Priority";
                textBlock.FontSize = 10;
                textBlock.Height = 25;
                textBlock.Width = 25;
                textBlock.Margin = new Thickness(0, 0, 10, 0);

                NumericBox textBox = createBox();
                textBox.RegisterValidatorDelegate(ValidatePriority);

                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = "Time";
                textBlock1.FontSize = 10;
                textBlock1.Height = 25;
                textBlock1.Width = 25;
                textBlock1.Margin = new Thickness(10,0,10,0);

                TextBox textBox1 = createBox();

                stackPanel.Children.Add(textBlock);
                stackPanel.Children.Add(textBox);
                stackPanel.Children.Add(textBlock1);
                stackPanel.Children.Add(textBox1);

                listBox.Items.Add(stackPanel);
                
            }
            
        }

        private bool ValidatePriority(string Text) {
            if(Int32.Parse(Text) > numPriorities) return false;
            return true;
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
            PriorityThreadService.SetLogger((message) => { logger.Text += message; });
            PriorityThreadService.ChangeState();
            PriorityThreadService.SetQuantum(quant);
            List<int> time = CastValues(listBox.Items, 1);
            List<int> priorities = CastValues(listBox.Items, 3);
            List<(int, int)> initialValues = new();
            for(int i = 0; i < time.Count; i++)
            {
                initialValues.Add((time[i], priorities[i]));
            }
            PriorityThreadService.SetPriorityTasks(initialValues);
            PriorityThreadService.StartTasks();
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            PriorityThreadService.ChangeState();
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
