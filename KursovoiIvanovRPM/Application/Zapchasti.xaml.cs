using Microsoft.Win32;
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
    /// Логика взаимодействия для Zapchasti.xaml
    /// </summary>
    public partial class Zapchasti : Window
    {
        public Zapchasti()
        {
            InitializeComponent();
            
            string filePathFluids = @"..\..\..\Zapchasti\Fluids.json";
            List<TechnicalFluid> technicalFluids = LoadFluidsFromJson(filePathFluids);
            DataContext = new { Zapchast = technicalFluids};
            

        }
        private void BackToMenuZapButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }
        
        private void NewFluidsButton_Click (object sender, RoutedEventArgs e)
        {
            NewFluids newFluids = new NewFluids();
            newFluids.ShowDialog();

            string filePathFluids = @"..\..\..\Zapchasti\Fluids.json";
            List<TechnicalFluid> technicalFluids = LoadFluidsFromJson(filePathFluids);
            DataGridZapchast.ItemsSource = technicalFluids;
        }

        public List <TechnicalFluid> LoadFluidsFromJson (string filePathFluids)
        {
            string jsonFluids = File.ReadAllText(filePathFluids);
            return JsonConvert.DeserializeObject<List<TechnicalFluid>>(jsonFluids);
        }

        private void DelFluidsButton_Click (Object sender, RoutedEventArgs e)
        {
            if (DataGridZapchast.SelectedItems.Count > 0)
            {
                List<TechnicalFluid> technicalFluids = ((IEnumerable<TechnicalFluid>)DataGridZapchast.ItemsSource).ToList();
                foreach (TechnicalFluid technicalFluid in DataGridZapchast.SelectedItems)
                {
                    technicalFluids.Remove(technicalFluid);
                }
               string filePathFluids = @"..\..\..\Zapchasti\Fluids.json";
                SaveFluidsToJson (filePathFluids, technicalFluids);

                DataGridZapchast.ItemsSource = technicalFluids;
            }
        }
        public void SaveFluidsToJson(string filePathFluids, List<TechnicalFluid> technicalFluids)
        {
            string jsonFluids = JsonConvert.SerializeObject(technicalFluids, Formatting.Indented);
            File.WriteAllText(filePathFluids, jsonFluids);
        }

        public void RedactFluidsButton_Click (object sender, RoutedEventArgs e)
        {
            if (DataGridZapchast.SelectedItems.Count > 0)
            {
                TechnicalFluid selectedFluids = (TechnicalFluid)DataGridZapchast.SelectedItem;
                NewFluids newFluidsWindow = new NewFluids(selectedFluids);
                newFluidsWindow.Closed += (s, args) =>
                {
                    string filePathFluids = @"..\..\..\Zapchasti\Fluids.json";
                    List<TechnicalFluid> technicalFluids = LoadFluidsFromJson(filePathFluids);
                    DataGridZapchast.ItemsSource = technicalFluids;
                };
                newFluidsWindow.ShowDialog();
            }
        }
    }
    public class TechnicalFluid
    {
        public int ID { get; set; }
        public string NameZap { get; set; }
        public double Price { get; set; }
        public string FluidType { get; set; }
        public string CountryOfOrigin { get; set; }
        public string ColvoFluids { get; set; }
    }

}
