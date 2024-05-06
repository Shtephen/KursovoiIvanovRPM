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
    /// Логика взаимодействия для NewFluids.xaml
    /// </summary>
    public partial class NewFluids : Window
    {
        private TechnicalFluid selectedFluid;
        public NewFluids(TechnicalFluid? technicalFluid = null )
        {
            InitializeComponent();

            if ( technicalFluid != null ) 
            {
                selectedFluid = technicalFluid;
                FluidsID.Text = technicalFluid.ID.ToString();
                FluidsName.Text = technicalFluid.NameZap;
                FluidsPrice.Text = technicalFluid.Price.ToString();
                FluidsType.Text = technicalFluid.FluidType;
                FluidsCountry.Text = technicalFluid.CountryOfOrigin;
                FluidsColvo.Text = technicalFluid.ColvoFluids;
            }
        }

        private void NewFluidsSaveButton_Click ( object sender, RoutedEventArgs e )
        {
            string jsonFilePath = "D:\\Весенний семестр\\Курсовой РПМ\\KursovoiIvanovRPM\\KursovoiIvanovRPM\\Zapchasti\\Fluids.json";
            List<TechnicalFluid> technicalFluids = new List<TechnicalFluid>();

            if(File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                technicalFluids = JsonConvert.DeserializeObject<List<TechnicalFluid>>(json);
            }
            
            if (selectedFluid == null)
            {
                TechnicalFluid newFluids = new TechnicalFluid
                {
                    ID = int.Parse(FluidsID.Text),
                    NameZap = FluidsName.Text,
                    Price = double.Parse(FluidsPrice.Text),
                    FluidType = FluidsType.Text,
                    CountryOfOrigin = FluidsCountry.Text,
                    ColvoFluids = FluidsColvo.Text
                };
                technicalFluids.Add(newFluids);
            }
            else
            {
                selectedFluid.ID = int.Parse(FluidsID.Text);
                selectedFluid.NameZap = FluidsName.Text;
                selectedFluid.Price = double.Parse(FluidsPrice.Text);
                selectedFluid.FluidType = FluidsType.Text;
                selectedFluid.CountryOfOrigin = FluidsCountry.Text;
                selectedFluid.ColvoFluids = FluidsColvo.Text;

                int index = technicalFluids.FindIndex(c => c.ID == selectedFluid.ID);
                if (index != -1) 
                {
                    technicalFluids[index] = selectedFluid;              
                }
            }

            string updateJson = JsonConvert.SerializeObject(technicalFluids, Formatting.Indented);
            File.WriteAllText(jsonFilePath, updateJson);
            this.Close();

        }
    }
}
