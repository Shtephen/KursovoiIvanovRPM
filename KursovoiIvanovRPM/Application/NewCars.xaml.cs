using Newtonsoft.Json;
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


namespace KursovoiIvanovRPM.Application
{
    /// <summary>
    /// Логика взаимодействия для NewCars.xaml
    /// </summary>
    public partial class NewCars : Window
    {
        public List<Car> CarsList = new List<Car>();

        public Car selectedCar;



        public NewCars(Car selectedCar)
        {
            InitializeComponent();

            
            this.selectedCar = selectedCar;

            
            if (selectedCar != null)
            {
                IdTextBox.Text = selectedCar.ID.ToString();
                BrandTextBox.Text = selectedCar.Brand;
                ModelTextBox.Text = selectedCar.Model;
                LicensePlateTextBox.Text = selectedCar.LicensePlate;
                YearTextBox.Text = selectedCar.Year;
                MileageTextBox.Text = selectedCar.Mileage;
                VinNumberTextBox.Text = selectedCar.VinNumber;
                StsNumberTextBox.Text = selectedCar.StsNumber;
              
                StsIssueDateDatePicker.SelectedDate = DateTime.Parse(selectedCar.StsIssueDate);
            }
        }

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCar == null)
            {
                // Добавление новой записи

                Car newCar = new Car
                {
                    ID = int.Parse(IdTextBox.Text),
                    Brand = BrandTextBox.Text,
                    Model = ModelTextBox.Text,
                    LicensePlate = LicensePlateTextBox.Text,
                    Year = YearTextBox.Text,
                    Mileage = MileageTextBox.Text,
                    VinNumber = VinNumberTextBox.Text,
                    StsNumber = StsNumberTextBox.Text,
                    StsIssueDate = StsIssueDateDatePicker.SelectedDate.ToString()
                };

                // Загружаем существующий список из файла
                string json = File.ReadAllText(@"..\..\..\Cars\Cars.json");
                CarsList = JsonConvert.DeserializeObject<List<Car>>(json);

                // Добавляем новый объект Car в список
                CarsList.Add(newCar);
            }
            else
            {
                selectedCar.ID = int.Parse(IdTextBox.Text);
                selectedCar.Brand = BrandTextBox.Text;
                selectedCar.Model = ModelTextBox.Text;
                selectedCar.LicensePlate = LicensePlateTextBox.Text;
                selectedCar.Year = YearTextBox.Text;
                selectedCar.Mileage = MileageTextBox.Text;
                selectedCar.VinNumber = VinNumberTextBox.Text;
                selectedCar.StsNumber = StsNumberTextBox.Text;
                selectedCar.StsIssueDate = StsIssueDateDatePicker.SelectedDate.ToString();

                
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
            }

            
            string updatedJson = JsonConvert.SerializeObject(CarsList, Formatting.Indented);

            
            File.WriteAllText(@"..\..\..\Cars\Cars.json", updatedJson);
            
            this.Close();
        }

    }
}
