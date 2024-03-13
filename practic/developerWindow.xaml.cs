using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace practic
{
    /// <summary>
    /// Логика взаимодействия для developerWindow.xaml
    /// </summary>
    public partial class developerWindow : Window
    {
        Tables tables = new Tables();
        task_status inProcess = null;
        task_status finished = null;
        employeer selectedEmployer = null;
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        int employeerId = 0;

        public developerWindow(int idEmployeer)
        {
            InitializeComponent();

            List<task> tickets = tables.task.Where(task => task.employeer1.id == idEmployeer && task.task_status.id != 3).ToList();
            inProcess = tables.task_status.Where(status => status.id == 2).Single();
            finished = tables.task_status.Where(status => status.id == 3).Single();
            selectedEmployer = tables.employeer.Where(employeer => employeer.id == idEmployeer).Single();
            userNameLabel.Text = "Привет, " + selectedEmployer.fullname;
            employeerId = idEmployeer;

            foreach (task task in tickets)
            {
                ticketsLB.Items.Add(task);
            }

            listOfTicketsTitle.Content = $"Список задач - {tickets.Count()} штук";
        }

        private void ticketsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            task task = ticketsLB.SelectedItem as task;

            if (task != null)
            {
                if (task.image != null)
                {
                    errorScreenshot.Source = new BitmapImage(new Uri(projectDirectory + "\\" + task.image));
                }
                else
                {
                    errorScreenshot.Source = null;
                }


                titleBox.Text = task.topic;
                infoBox.Text = task.info;
                taskStatusBox.Text = task.task_status.name;
            }
            else
            {
                titleBox.Text = "";
                infoBox.Text = "";
                taskStatusBox.Text = "";
                errorScreenshot.Source = null;
            }
        }

        private void tryToFix(object sender, RoutedEventArgs e)
        {
            task selectedTask = ticketsLB.SelectedItem as task;

            if(selectedTask == null)
            {
                MessageBox.Show("Выберите заявку, чтобы начать ее выполнение!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            selectedTask.task_status = inProcess;

            tables.SaveChanges();
            taskStatusBox.Text = selectedTask.task_status.name;
            MessageBox.Show("Вы начали выполнение задачи.", "Начато выполнение задачи.", MessageBoxButton.OK);
        }

        private void taskFixed(object sender, RoutedEventArgs e)
        {
            task selectedTask = ticketsLB.SelectedItem as task;

            if (selectedTask == null)
            {
                MessageBox.Show("Выберите заявку, чтобы завершить ее выполнение!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            selectedTask.task_status = finished;

            ticketsLB.Items.Remove(selectedTask);
            titleBox.Text = "";
            infoBox.Text = "";
            taskStatusBox.Text = "";
            errorScreenshot.Source = null;
            tables.SaveChanges();
            MessageBox.Show("Задача успешно завершена!", "Успех!", MessageBoxButton.OK);
            List<task> tickets = tables.task.Where(task => task.employeer1.id == employeerId && task.task_status.id != 3).ToList();
            listOfTicketsTitle.Content = $"Список задач - {tickets.Count()} штук";
        }

        private void logout(object sender, RoutedEventArgs e)
        {
            auth auth = new auth();
            auth.Show();
            this.Close();
        }
    }
}
