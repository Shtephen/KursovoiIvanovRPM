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
    /// Логика взаимодействия для ListOfServices.xaml
    /// </summary>
    public partial class ListOfServices : Window
    {
        public ListOfServices()
        {
           InitializeComponent();
            string filePath = @"..\..\..\ListOfServices\ListOfServices.json";
            List <Service> services = LoadDataFromJson(filePath);
            DataContext = new { Services = services };
        }

        public List <Service> LoadDataFromJson (string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Service>>(json);       
        }
        private void ServicesBackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }
        private void NewListServ (object sender, RoutedEventArgs e)
        {
           NewListOfServices newListOfServices = new NewListOfServices();
           newListOfServices.ShowDialog();

            string filePath = @"..\..\..\ListOfServices\ListOfServices.json";
            List<Service> services = LoadDataFromJson(filePath);
            ServicesDataGrid.ItemsSource = services;

        }

        private void DeliteListButton_Click (object sender, RoutedEventArgs e)
        {
            if(ServicesDataGrid.SelectedItems.Count > 0) 
            {
                List<Service> services = ((IEnumerable<Service>)ServicesDataGrid.ItemsSource).ToList();

                foreach (Service service in ServicesDataGrid.SelectedItems) 
                { 
                    services.Remove(service);
                }

                string filePath = @"..\..\..\ListOfServices\ListOfServices.json";
                SaveListOfItemToJson(filePath, services);

                ServicesDataGrid.ItemsSource = services;
            }
        }

        public void SaveListOfItemToJson (string filePath, List<Service> services)
        {
            string json = JsonConvert.SerializeObject(services, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private void RedaktListButton_Click(object sender, RoutedEventArgs e)
        {
            if(ServicesDataGrid.SelectedItems.Count > 0)
            {
                Service selectedService = (Service)ServicesDataGrid.SelectedItem;
                NewListOfServices newListOfServices = new NewListOfServices(selectedService);
                newListOfServices.Closed += (s, args) =>
                {
                    string filePath = @"..\..\..\ListOfServices\ListOfServices.json";
                    List<Service> services = LoadDataFromJson(filePath);
                    ServicesDataGrid.ItemsSource = services;
                };
                newListOfServices.ShowDialog();
            }
        }
    }

    public class Service
    {
        public int ID { get; set; }
        public string NameList { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
