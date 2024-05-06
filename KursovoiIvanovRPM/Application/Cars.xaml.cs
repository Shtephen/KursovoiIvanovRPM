using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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

namespace KursovoiIvanovRPM.Application
{
    /// <summary>
    /// Логика взаимодействия для Cars.xaml
    /// </summary>
    public partial class Cars : Window
    {
        public Car selectedCar;
        public List<Car> CarsList { get; set; }


        public Cars()
        {
            InitializeComponent();
            string jsonFilePath = @"..\..\..\Cars\Cars.json";
            string jsonData = File.ReadAllText(jsonFilePath);

            // Преобразование JSON в список объектов Car
            CarsList = JsonConvert.DeserializeObject<List<Car>>(jsonData);

            // Привязка списка объектов к DataGrid
            CarsdataGrid.ItemsSource = CarsList;
        }

        public void CarsBackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }

        public void NewCarsButton_Click(object sender, RoutedEventArgs e)
        {
            NewCars newCarsWindow = new NewCars(null);
            newCarsWindow.ShowDialog();

            // После закрытия окна обновляем таблицу
            RefreshDataGrid();
        }

        // Метод для обновления таблицы
        public void RefreshDataGrid()
        {
            string jsonFilePath = @"..\..\..\Cars\Cars.json";
            string jsonData = File.ReadAllText(jsonFilePath);

            // Преобразование JSON в список объектов Car
            CarsList = JsonConvert.DeserializeObject<List<Car>>(jsonData);

            // Привязка списка объектов к DataGrid
            CarsdataGrid.ItemsSource = CarsList;
        }

        public void DelCarsButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarsdataGrid.SelectedItem != null)
            {
                Car selectedCar = (Car)CarsdataGrid.SelectedItem; // Получаем выбранный объект Car

                CarsList.Remove(selectedCar); // Удаляем выбранный объект из списка

                // Сохраняем обновленный список в файл JSON
                string json = JsonConvert.SerializeObject(CarsList, Formatting.Indented);
                File.WriteAllText(@"..\..\..\Cars\Cars.json", json);

                // Обновляем источник данных таблицы
                CarsdataGrid.ItemsSource = null;
                CarsdataGrid.ItemsSource = CarsList;
            }
        }
        public void RedactCarsButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarsdataGrid.SelectedItem != null)
            {
                
                Car selectedCar = (Car)CarsdataGrid.SelectedItem;

                
                NewCars newCarsWindow = new NewCars(selectedCar);
                newCarsWindow.ShowDialog();

                // Update the selected car's information in the list
                foreach (Car car in CarsList)
                {
                    if (car.ID == selectedCar.ID)
                    {
                        car.ID = selectedCar.ID;
                        car.Brand = selectedCar.Brand;
                        car.Model = selectedCar.Model;
                        car.LicensePlate = selectedCar.LicensePlate;
                        car.Year = selectedCar.Year;
                        car.Mileage = selectedCar.Mileage;
                        car.VinNumber = selectedCar.VinNumber;
                        car.StsNumber = selectedCar.StsNumber;
                        car.StsIssueDate = selectedCar.StsIssueDate;
                        break;
                    }
                }

                // Update the JSON file with the modified list
                string updatedJson = JsonConvert.SerializeObject(CarsList, Formatting.Indented);
                File.WriteAllText(@"..\..\..\Cars\Cars.json", updatedJson);

                // Refresh the DataGrid with the updated list
                CarsdataGrid.ItemsSource = null;
                CarsdataGrid.ItemsSource = CarsList;
            }
        }

        
    }
    public class Car
    {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
        public string Year { get; set; }
        public string Mileage { get; set; }
        public string VinNumber { get; set; }
        public string StsNumber { get; set; }
        public string StsIssueDate { get; set; }
    }

}

