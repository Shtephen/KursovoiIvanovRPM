using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Логика взаимодействия для NewKlients.xaml
    /// </summary>
    public partial class NewKlients : Window
    {
        public Client selectedClient;
        

        public NewKlients(Client? client = null)
        {
            InitializeComponent();

            if (client != null)
            {
                selectedClient = client;
                txtID.Text = client.ID.ToString();
                txtLastName.Text = client.LastName;
                txtFirstName.Text = client.FirstName;
                txtMiddleName.Text = client.MiddleName;
                txtEmail.Text = client.Email;
                txtPhone.Text = client.Phone;
                txtPassportData.Text = client.PassportData;
            }
        }

        public void NewKlientsSaveButton_Click(object sender, RoutedEventArgs e)
        {
            string jsonFilePath = @"..\..\..\Klients\KlientsInfo.json";
            List<Client> clients = new List<Client>();

            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                clients = JsonConvert.DeserializeObject<List<Client>>(json);
            }

            if (selectedClient == null) // Добавление новой записи
            {
                Client newClient = new Client
                {
                    ID = int.Parse(txtID.Text),
                    LastName = txtLastName.Text,
                    FirstName = txtFirstName.Text,
                    MiddleName = txtMiddleName.Text,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text,
                    PassportData = txtPassportData.Text
                };

                clients.Add(newClient);
            }
            else // Редактирование существующей записи
            {
                selectedClient.ID = int.Parse(txtID.Text);
                selectedClient.LastName = txtLastName.Text;
                selectedClient.FirstName = txtFirstName.Text;
                selectedClient.MiddleName = txtMiddleName.Text;
                selectedClient.Email = txtEmail.Text;
                selectedClient.Phone = txtPhone.Text;
                selectedClient.PassportData = txtPassportData.Text;

                int index = clients.FindIndex(c => c.ID == selectedClient.ID);
                if (index != -1)
                {
                    clients[index] = selectedClient;
                }
            }

            string updatedJson = JsonConvert.SerializeObject(clients, Formatting.Indented);
            File.WriteAllText(jsonFilePath, updatedJson);
            this.Close();
        }
    }
}
