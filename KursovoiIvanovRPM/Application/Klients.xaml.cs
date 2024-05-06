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
    public class Client
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PassportData { get; set; }
    }

    public partial class Klients : Window
    {
        public Klients()
        {
            InitializeComponent();
            string filePath = @"D:..\..\..\Klients\KlientsInfo.json"; // Путь к вашему JSON файлу
            List<Client> clients = LoadClientsFromJson(filePath);
            DataContext = new { Clients = clients };
        }

        private void KlientsBackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }
        public void NewKlientsButton_Click (object sender, RoutedEventArgs e)
        {
            NewKlients newKlients = new NewKlients();
            newKlients.ShowDialog();

            string filePath = @"..\..\..\Klients\KlientsInfo.json";
            List<Client> clients = LoadClientsFromJson(filePath);
            dataGrid.ItemsSource = clients;
        }

        public List<Client> LoadClientsFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Client>>(json);
        }
        private void DeleteKlientsButton_Click (Object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count > 0)
            {
                List<Client> clients = ((IEnumerable<Client>)dataGrid.ItemsSource).ToList();
                foreach (Client client in dataGrid.SelectedItems)
                {
                    clients.Remove(client);
                }

                // Сохраняем обновленный список клиентов в файл
                string filePath = @"..\..\..\Klients\KlientsInfo.json";
                SaveClientsToJson(filePath, clients);

                // Обновляем данные в DataGrid
                dataGrid.ItemsSource = clients;
            }
        }
        public void SaveClientsToJson(string filePath, List<Client> clients)
        {
            string json = JsonConvert.SerializeObject(clients, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void RedaktKlientsButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count > 0)
            {
                Client selectedClient = (Client)dataGrid.SelectedItem;
                NewKlients newKlientsWindow = new NewKlients(selectedClient);
                newKlientsWindow.Closed += (s, args) =>
                {
                    string filePath = @"..\..\..\Klients\KlientsInfo.json";
                    List<Client> clients = LoadClientsFromJson(filePath);
                    dataGrid.ItemsSource = clients;
                };
                newKlientsWindow.ShowDialog();
            }
        }
    }
}
