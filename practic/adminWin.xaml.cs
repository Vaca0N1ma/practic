using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для adminWin.xaml
    /// </summary>
    public partial class adminWin : Window
    {
        Tables tables = new Tables();
        employeer selectedEmployer = null;
        public adminWin(int idEmployeer)
        {
            InitializeComponent();
            dGridService.ItemsSource = Tables.GetContext().task.ToList();
            selectedEmployer = tables.employeer.Where(employeer => employeer.id == idEmployeer).Single();
            Name.Text = "Привет, " + selectedEmployer.fullname;
            combo.Items.Add("Все");
            combo.Items.Add("Открыт");
            combo.Items.Add("В процессе");
            combo.Items.Add("Решено");
            
        }

        private void Change(object sender, DependencyPropertyChangedEventArgs
e)
        {
            if (Visibility == Visibility.Visible)

                dGridService.ItemsSource = Tables.GetContext().task.ToList();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            auth auth = new auth();
            auth.Show();
            this.Close();
        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var serviceListt = Tables.GetContext().task.ToList();
            if (combo.SelectedIndex == 1) serviceListt = serviceListt.Where(p => p.task_status.name == "Открыт").ToList();
            if (combo.SelectedIndex == 2) serviceListt = serviceListt.Where(p => p.task_status.name == "В процессе").ToList();
            if (combo.SelectedIndex == 3) serviceListt = serviceListt.Where(p => p.task_status.name == "Решено").ToList();
           
            dGridService.ItemsSource = serviceListt.ToList();
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string con = search.Text;

            var found = tables.task.Where(att => att.task_status.name.Contains(con) || att.topic.Contains(con)
                       || att.info.Contains(con) || att.employeer.fullname.Contains(con));

            dGridService.ItemsSource = found.ToList();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            // Установка контекста лицензии для библиотеки EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

            // Получаем заголовки столбцов
            for (int i = 0; i < dGridService.Columns.Count; i++)
            {
                if (dGridService.Columns[i].Header.ToString() != "Фото")
                {
                    worksheet.Cells[1, i + 1].Value = dGridService.Columns[i].Header;
                }
            }

            // Заполняем ячейки данными из DataGrid
            var dataItems = dGridService.ItemsSource as IEnumerable<task>;
            if (dataItems != null)
            {
                int rowIndex = 2;
                foreach (var item in dataItems)
                {
                    int columnIndex = 1;
                    for (int i = 0; i < dGridService.Columns.Count; i++)
                    {
                        if (dGridService.Columns[i].Header.ToString() != "Фото")
                        {
                            
                                switch (dGridService.Columns[i].Header.ToString())
                            {
                                case "Заголовок":
                                    worksheet.Cells[rowIndex, columnIndex].Value = item.topic;
                                    break;
                                case "Описание":
                                    worksheet.Cells[rowIndex, columnIndex].Value = item.info;
                                    break;
                                case "Статус задачи":
                                    worksheet.Cells[rowIndex, columnIndex].Value = item.task_status.name;
                                    break;
                                case "Руководитель":
                                    worksheet.Cells[rowIndex, columnIndex].Value = item.employeer.fullname;
                                    break;
                                case "Сотрудник":
                                    worksheet.Cells[rowIndex, columnIndex].Value = item.employeer1.fullname;
                                    break;
                                
                            }
                            columnIndex++;
                        }
                    }
                    rowIndex++;
                }
            }

            // Автонастройка размеров столбцов
            worksheet.Cells.AutoFitColumns();

            // Сохраняем файл Excel
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                var excelFile = new FileInfo(saveFileDialog.FileName);
                excelPackage.SaveAs(excelFile);
                MessageBox.Show("Данные успешно экспортированы в Excel.", "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

       
    }
}
