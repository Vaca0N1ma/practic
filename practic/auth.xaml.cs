using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для auth.xaml
    /// </summary>
    public partial class auth : Window
    {
        Tables tables = new Tables();

        public auth()
        {
            InitializeComponent();
        }

        private void authButton(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text.Trim();
            string password = passwordBox.Password.Trim();

            employeer employeer = new employeer();

            employeer = tables.employeer.Where(emp => emp.login == login && emp.password == password).FirstOrDefault();

            if (employeer != null)
            {
                MessageBox.Show("Вы вошли в систему как " + employeer.fullname + "!", "Успех!");
                switch (employeer.user_role.id)
                {
                    case 1:
                        supportWindow support = new supportWindow(employeer.id);
                        support.Show();
                        this.Close();
                        break;
                    case 2:
                        developerWindow dev = new developerWindow(employeer.id);
                        dev.Show();
                        this.Close();
                        break;
                    case 3:
                        dutyWindow duty = new dutyWindow(employeer.id);
                        duty.Show();
                        this.Close();
                        break;
                    case 4:
                        adminWin wind = new adminWin(employeer.id);
                        wind.Show();
                        //adminWindow adminWindow = new adminWindow(employeer.id);
                        //adminWindow.Show();                        
                        this.Close();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Введенной пары логин:пароль не найдено! Попробуйте снова!" + employeer, "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                loginBox.Clear();
                passwordBox.Clear();
                loginBox.Focus();
            }
        }

        private void registrationButton(object sender, RoutedEventArgs e)
        {
            registration registration = new registration();

            registration.ShowDialog();
        }

        private void openNewWindow(Window window)
        {
            window.Show();
            this.Close();
        }
    }
}
