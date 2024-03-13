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
    /// Логика взаимодействия для supportWindow.xaml
    /// </summary>
    public partial class supportWindow : Window
    {
        Tables tables = new Tables();
        task_status inProcess = null;
        task_status finished = null;
        employeer selectedEmployer = null;
        int employerId = 0;
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;


        public supportWindow(int idEmployeer)
        {
            InitializeComponent();

            employerId = idEmployeer;
            List<task> tickets = tables.task.Where(task => task.employeer1.id == employerId && task.task_status.id != 3).ToList();
            inProcess = tables.task_status.Where(status => status.id == 2).Single();
            finished = tables.task_status.Where(status => status.id == 3).Single();
            selectedEmployer = tables.employeer.Where(employeer => employeer.id == idEmployeer).Single();
            userNameLabel.Text = "Привет, " + selectedEmployer.fullname;

            foreach (task task in tickets)
            {
                ticketsLB.Items.Add(task);
            }
            listOfTicketsTitle.Content = $"Список задач - {tickets.Count()} штук";
        }

        private void sendToTheDevelopers(object sender, RoutedEventArgs e)
        {
            task selectedTask = ticketsLB.SelectedItem as task;

            if (selectedTask != null)
            {
                redirectToTheDevelopers redirectToTheDevelopers = new redirectToTheDevelopers(selectedTask.id);
                redirectToTheDevelopers.ShowDialog();
                ticketsLB.Items.Clear();
                List<task> tickets = tables.task.Where(task => task.employeer1.id == employerId && task.task_status.id != 3).ToList();
                foreach (task task in tickets)
                {
                    ticketsLB.Items.Add(task);
                }
                listOfTicketsTitle.Content = $"Список задач - {tickets.Count()} штук";
                titleBox.Text = "";
                infoBox.Text = "";
                taskStatusBox.Text = "";
            } else
            {
                MessageBox.Show("Выберите задачу, прежде чем выполнить действие!", "Попробуйте снова");
            }
        }

        private void tryToFix(object sender, RoutedEventArgs e)
        {
            task selectedTask = ticketsLB.SelectedItem as task;

            if (selectedTask != null)
            {
                selectedTask.task_status = inProcess;
                tables.SaveChanges();
                taskStatusBox.Text = selectedTask.task_status.name;
                MessageBox.Show("Вы начали выполнение задачи.", "Начато выполнение задачи.", MessageBoxButton.OK);
            } else
            {
                MessageBox.Show("Выберите задачу, прежде чем выполнить действие!", "Попробуйте снова");
            }
        }

        private void taskFixed(object sender, RoutedEventArgs e)
        {
            task selectedTask = ticketsLB.SelectedItem as task;

            if(selectedTask != null)
            {
                selectedTask.task_status = finished;

                ticketsLB.Items.Remove(selectedTask);
                titleBox.Text = "";
                infoBox.Text = "";
                taskStatusBox.Text = "";
                tables.SaveChanges();
                MessageBox.Show("Задача успешно завершена!", "Успех!", MessageBoxButton.OK);
            }
            else if(selectedTask.task_status.ToString() == "Открыто")
            {
                MessageBox.Show("Приступите к началу проекта, чтобы его завершить");
            }
            else
            {
                MessageBox.Show("Выберите задачу, прежде чем выполнить действие!", "Попробуйте снова");
            }

            List<task> tickets = tables.task.Where(task => task.employeer1.id == employerId && task.task_status.id != 3).ToList();
            listOfTicketsTitle.Content = $"Список задач - {tickets.Count()} штук";
        }

        private void ticketsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            task task = ticketsLB.SelectedItem as task;

            if (task != null)
            {
                titleBox.Text = task.topic;
                infoBox.Text = task.info;
                taskStatusBox.Text = task.task_status.name;
                if (task.image != null)
                {
                    errorScreenshot.Source = new BitmapImage(new Uri(projectDirectory + "\\" + task.image));
                }
                else
                {
                    errorScreenshot.Source = null;
                }
            }
            else
            {
                titleBox.Text = "";
                infoBox.Text = "";
                taskStatusBox.Text = "";
                errorScreenshot.Source = null;
            }
        }

        private void logout(object sender, RoutedEventArgs e)
        {
            auth auth = new auth();
            auth.Show();
            this.Close();
        }
    }
}
