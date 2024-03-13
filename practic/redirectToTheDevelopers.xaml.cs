using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для redirectToTheDevelopers.xaml
    /// </summary>
    public partial class redirectToTheDevelopers : Window
    {
        Tables tables = new Tables();
        task selectedTask = null;
        task_status openStatus = null;

        public redirectToTheDevelopers(int idTask)
        {
            InitializeComponent();

            selectedTask = tables.task.Where(selTask => selTask.id == idTask).Single() ;
            openStatus = tables.task_status.Where(status => status.id == 1).Single();
            List<employeer> developersList = tables.employeer.Where(empl => empl.user_role.id == 2).ToList();

            foreach(employeer empl in developersList)
            {
                developersCB.Items.Add(empl);
            }

            topicBox.Text = selectedTask.topic;
            infoBox.Text = selectedTask.info;
        }

        private void redirectTicket(object sender, RoutedEventArgs e)
        {
            selectedTask.employeer1 = developersCB.SelectedItem as employeer;

            selectedTask.task_status = openStatus;

            tables.SaveChanges();
            MessageBox.Show("Задача успешно перенаправлена тестировщику", "Успех", MessageBoxButton.OK);
            this.Close();
        }
    }
}
