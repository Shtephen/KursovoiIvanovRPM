using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KursovoiIvanovRPM.Application
{
    public partial class NewListOfServices : Window
    {
        private Service selectedService;

        public NewListOfServices(Service? service = null)
        {
            InitializeComponent();

            if (service != null)
            {
                selectedService = service;
                IdTextBox.Text = service.ID.ToString();
                NameTextBox.Text = service.NameList;
                DescriptionTextBox.Text = service.Description;
                CostOfWorkTextBox.Text = service.Price.ToString();
            }
        }

        private void SaveListButton_Click(object sender, RoutedEventArgs e)
        {
            string jsonfilePath = @"..\..\..\ListOfServices\ListOfServices.json";
            List<Service> services = new List<Service>();

            if(File.Exists(jsonfilePath))
            {
                string json = File.ReadAllText(jsonfilePath);
                services = JsonConvert.DeserializeObject<List<Service>>(json);
            }

            if(selectedService == null) 
            {
                Service newService = new Service
                {
                    ID = int.Parse(IdTextBox.Text),
                    NameList = NameTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    Price = decimal.Parse(CostOfWorkTextBox.Text),
                };
                services.Add(newService);
            }
            else 
            {
                selectedService.ID = int.Parse(IdTextBox.Text);
                selectedService.NameList = NameTextBox.Text;
                selectedService.Description = DescriptionTextBox.Text;
                selectedService.Price = decimal.Parse(CostOfWorkTextBox.Text);

                int index = services.FindIndex(c=> c.ID == selectedService.ID);
                if (index != -1) 
                {
                    services[index] = selectedService;
                }
            }

            string updatedJson = JsonConvert.SerializeObject(services, Formatting.Indented);
            File.WriteAllText(jsonfilePath, updatedJson);
            this.Close();
        }

    }
}
