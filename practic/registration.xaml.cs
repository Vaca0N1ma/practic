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
    /// Логика взаимодействия для registration.xaml
    /// </summary>
    public partial class registration : Window
    {

        Tables tables = new Tables();
        public registration()
        {
            InitializeComponent();

            foreach (var role in tables.user_role)
            {
                roleCB.Items.Add(role);
            }
        }

        private void regButton(object sender, RoutedEventArgs e)
        {
            if (fullnameBox.Text == "" || experienceBox.Text == "" || roleCB.SelectedIndex == -1 || loginBox.Text == "" || passwordBox.Password == "")
            {
                MessageBox.Show("Все поля должны быть заполнены! Попробуйте снова!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(System.Text.RegularExpressions.Regex.IsMatch(loginBox.Text, @"\p{IsCyrillic}") 
                || System.Text.RegularExpressions.Regex.IsMatch(passwordBox.Password, @"\p{IsCyrillic}"))
            {
                MessageBox.Show("Логин и пароль должны содержать латинские буквы или цифры! Попробуйте снова!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int.Parse(experienceBox.Text);
            } catch(Exception)
            {
                MessageBox.Show("Стаж должен быть числом! Попробуйте снова!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            employeer employeer = new employeer();

            employeer.fullname = fullnameBox.Text;
            employeer.experience = int.Parse(experienceBox.Text);
            employeer.user_role = roleCB.SelectedItem as user_role;
            employeer.login = loginBox.Text;
            employeer.password = passwordBox.Password;

            tables.employeer.Add(employeer);
            tables.SaveChanges();
            MessageBox.Show("Пользователь успешно зарегистрирован!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);

            fullnameBox.Text = "";
            experienceBox.Text = "";
            roleCB.SelectedIndex = -1;
            loginBox.Text = "";
            passwordBox.Password = "";
        }

        private void backButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
