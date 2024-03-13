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
    /// Логика взаимодействия для dutyWindow.xaml
    /// </summary>
    public partial class dutyWindow : Window
    {
        employeer selectedEmployer = null;
        Tables tables = new Tables();
        task_status openStatus = null;
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        public dutyWindow(int idEmployeer)
        {
            InitializeComponent();
            selectedEmployer = tables.employeer.Where(employeer => employeer.id == idEmployeer).Single();

            List<employeer> supportsList = tables.employeer.Where(support => support.user_role.id == 1).ToList();
            openStatus = tables.task_status.Where(status => status.id == 1).Single();
            userNameLabel.Text = "Привет, " + selectedEmployer.fullname;

            foreach (var support in supportsList)
            {
                dutyCB.Items.Add(support);
            }
        }

        private void createTicket(object sender, RoutedEventArgs e)
        {
            if (topicBox.Text == "" || infoBox.Text == "" || dutyCB.SelectedIndex == -1)
            {
                MessageBox.Show("Все поля должны быть заполнены! Попробуйте снова!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            task task = new task();

            task.topic = topicBox.Text;
            task.info = infoBox.Text;
            task.employeer = selectedEmployer;
            task.employeer1 = dutyCB.SelectedItem as employeer;
            task.task_status = openStatus;

            if(MessageBox.Show("Хотите приложить к проекту изображение?", "Добавить изображение",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
                fileDialog.InitialDirectory = projectDirectory + "\\images\\";

                if (fileDialog.ShowDialog() == true)
                {
                  
                    string imageName = "images\\" + fileDialog.SafeFileName.ToString();
                    task.image = imageName;
                    errorScreenshot.Source = new BitmapImage(new Uri(projectDirectory + "\\" + task.image));
                }
            } else
            {
                task.image = "images/nonImage.jpg";
            }

            tables.task.Add(task);
            tables.SaveChanges();

            MessageBox.Show("Обращение успешно создано!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);

            topicBox.Text = "";
            infoBox.Text = "";
            errorScreenshot.Source = null;
            dutyCB.SelectedIndex = -1;
        }

        private void logout(object sender, RoutedEventArgs e)
        {
            auth auth = new auth();
            auth.Show();
            this.Close();
        }
    }
}
