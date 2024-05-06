using NPOI.XWPF.UserModel;
using NPOI.SS.UserModel;
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
using static KursovoiIvanovRPM.Application.MenuWindow;
using NPOI.OpenXmlFormats.Wordprocessing;

namespace KursovoiIvanovRPM.Application
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private List<Order> Orders; // Объявляем переменную Orders как поле класса
        private Order selectedOrder;

        public bool? IsClosed { get; set; }

        public MenuWindow()
        {
            InitializeComponent();
            LoadDataFromJson();
        }
        


        public void Button_Click(object sender, RoutedEventArgs e)
        {
            //  
            Button button = sender as Button;
            if (button != null)
            {
                string buttonText = button.Content.ToString();
                switch (buttonText)
                {
                    case "Клиенты":
                        Klients klients = new Klients();
                        klients.Show();
                        this.Close();
                        break;
                    case "Автомобили":
                        Cars cars = new Cars();
                        cars.Show();
                        this.Close();
                        break;
                    case "Список услуг":
                        ListOfServices listOfServices = new ListOfServices();
                        listOfServices.Show();
                        this.Close();
                        break;
                    case "Сотрудники":
                        StaffWindow staffWindow = new StaffWindow();
                        staffWindow.Show();
                        this.Close();
                        break;
                    case "Запчасти":
                        Zapchasti zapchasti = new Zapchasti();
                        zapchasti.Show();
                        this.Close();
                        break;
                    default:
                        break;
                }
            }
        }
        
        public void LoadDataFromJson()
        {
            string jsonFilePath = @"..\..\..\InformationOrders\Orders.json";
            string jsonData = File.ReadAllText(jsonFilePath);
            List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(jsonData);
            Orders = orders; // Присвоить данные из локальной переменной orders списку Orders
            OrdersDataGrid.ItemsSource = Orders;
        }

        private void NewOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            NewOrderWindow newOrderWindow = new NewOrderWindow();
            newOrderWindow.Show();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem != null)
            {
                Order selectedOrder = (Order)OrdersDataGrid.SelectedItem;
                Orders.Remove(selectedOrder);
                SaveDataToJson();
                LoadDataFromJson(); // Обновить список заказов после удаления
            }
        }
        private void SaveDataToJson()
        {
            string jsonFilePath = @"..\..\..\InformationOrders\Orders.json";
            string jsonData = JsonConvert.SerializeObject(Orders);
            File.WriteAllText(jsonFilePath, jsonData);
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDataFromJson(); // Обновить таблицу заказов
        }

        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem != null)
            {

                selectedOrder = (Order)OrdersDataGrid.SelectedItem;
                NewOrderWindow newOrderWindow = new NewOrderWindow(selectedOrder);
                newOrderWindow.ShowDialog();
            }
        }

        private void ZakazNarButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную запись из DataGrid
            var selectedItem = OrdersDataGrid.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Выберите Заказ");
                return;
            }

            var orders = JsonConvert.DeserializeObject<List<Order>>(File.ReadAllText(@"..\..\..\InformationOrders\Orders.json"));
            var clients = JsonConvert.DeserializeObject<List<Client>>(File.ReadAllText(@"..\..\..\Klients\KlientsInfo.json"));
            var cars = JsonConvert.DeserializeObject<List<Car>>(File.ReadAllText(@"..\..\..\Cars\Cars.json"));
            var services = JsonConvert.DeserializeObject<List<Service>>(File.ReadAllText(@"..\..\..\ListOfServices\ListOfServices.json"));
            var staff = JsonConvert.DeserializeObject<List<Staff>>(File.ReadAllText(@"..\..\..\InfoStaff\Staff.json"));
            var zapcasti = JsonConvert.DeserializeObject<List<TechnicalFluid>>(File.ReadAllText(@"..\..\..\Zapchasti\Fluids.json"));

            int selectedID = (int)selectedItem.GetType().GetProperty("ID").GetValue(selectedItem);

            var allData = new List<object>();
            allData.AddRange(orders);
            allData.AddRange(clients);
            allData.AddRange(cars);
            allData.AddRange(services);
            allData.AddRange(staff);
            allData.AddRange(zapcasti);

            var selectedData = allData.Where(item => item.GetType().GetProperty("ID") != null && item.GetType().GetProperty("ID").GetValue(item).Equals(selectedID)).ToList();

            File.WriteAllText(@"..\..\..\ZakazNar\ZakazNar.json", JsonConvert.SerializeObject(selectedData, Formatting.Indented));
            CreateAndFillWordDocumentFromJson();
            
        }
        
        private void CreateAndFillWordDocumentFromJson()
        {
            // Получаем данные из JSON файла
            string json = File.ReadAllText(@"..\..\..\ZakazNar\ZakazNar.json");
            List<Dictionary<string, string>> data = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

            // Создание документа Word
            XWPFDocument doc = new XWPFDocument();



            XWPFParagraph title = doc.CreateParagraph();
            title.Alignment = ParagraphAlignment.CENTER;
            XWPFRun titleRun = title.CreateRun();
            titleRun.IsBold = true;
            titleRun.SetText("Заказ наряд");

            foreach (var item in data)
            {
                XWPFParagraph paragraph = doc.CreateParagraph();
                paragraph.Alignment = ParagraphAlignment.LEFT;


                if (item.ContainsKey("Name"))
                {
                    XWPFRun runName = paragraph.CreateRun();
                    runName.SetText($"Название заказа: {item["Name"]}");
                    runName.AddCarriageReturn();

                }

                if (item.ContainsKey("DateCreated"))
                {
                    XWPFRun runDateCreated = paragraph.CreateRun();
                    runDateCreated.SetText($"Дата создания заказа: {item["DateCreated"]}");
                    runDateCreated.AddCarriageReturn();
                }

                if (item.ContainsKey("DateCompleted"))
                {
                    XWPFRun runDateCompleted = paragraph.CreateRun();
                    runDateCompleted.SetText($"Дата выполнения заказа: {item["DateCompleted"]}");
                    runDateCompleted.AddCarriageReturn();
                }

                if (item.ContainsKey("LastName") && item.ContainsKey("FirstName") && item.ContainsKey("MiddleName"))
                {
                    XWPFRun runCustomer = paragraph.CreateRun();
                    runCustomer.SetText($"Заказчик: {item["LastName"]} {item["FirstName"]} {item["MiddleName"]}");
                    runCustomer.AddCarriageReturn();
                }

                if (item.ContainsKey("Email"))
                {
                    XWPFRun runEmail = paragraph.CreateRun();
                    runEmail.SetText($"Электронная почта: {item["Email"]}");
                    runEmail.AddCarriageReturn();
                }

                if (item.ContainsKey("Phone"))
                {
                    XWPFRun runPhone = paragraph.CreateRun();
                    runPhone.SetText($"Телефон: {item["Phone"]}");
                    runPhone.AddCarriageReturn();
                }

                if (item.ContainsKey("PassportData"))
                {
                    XWPFRun runPassport = paragraph.CreateRun();
                    runPassport.SetText($"Паспортные данные: {item["PassportData"]}");
                    runPassport.AddCarriageReturn();
                }

                if (item.ContainsKey("Brand") && item.ContainsKey("Model") && item.ContainsKey("LicensePlate") && item.ContainsKey("Year") &&
     item.ContainsKey("Mileage") && item.ContainsKey("VinNumber") && item.ContainsKey("StsNumber") && item.ContainsKey("StsIssueDate"))
                {
                    XWPFRun runCarInfo = paragraph.CreateRun();
                    runCarInfo.SetText($"Название машины: {item["Brand"]}");
                    runCarInfo.AddCarriageReturn();

                    XWPFRun runModel = paragraph.CreateRun();
                    runModel.SetText($"Модель: {item["Model"]}");
                    runModel.AddCarriageReturn();

                    XWPFRun runLicensePlate = paragraph.CreateRun();
                    runLicensePlate.SetText($"Гос-номер: {item["LicensePlate"]}");
                    runLicensePlate.AddCarriageReturn();

                    XWPFRun runYear = paragraph.CreateRun();
                    runYear.SetText($"Год производства: {item["Year"]}");
                    runYear.AddCarriageReturn();

                    XWPFRun runMileage = paragraph.CreateRun();
                    runMileage.SetText($"Пробег: {item["Mileage"]}");
                    runMileage.AddCarriageReturn();

                    XWPFRun runVinNumber = paragraph.CreateRun();
                    runVinNumber.SetText($"VIN: {item["VinNumber"]}");
                    runVinNumber.AddCarriageReturn();

                    XWPFRun runStsNumber = paragraph.CreateRun();
                    runStsNumber.SetText($"STS: {item["StsNumber"]}");
                    runStsNumber.AddCarriageReturn();

                    XWPFRun runStsIssueDate = paragraph.CreateRun();
                    runStsIssueDate.SetText($"Дата выдачи STS: {item["StsIssueDate"]}");
                    runStsIssueDate.AddCarriageReturn();
                }


                if (item.ContainsKey("NameList") && item.ContainsKey("Description") && item.ContainsKey("Price"))
                {
                    XWPFRun runServiceInfo = paragraph.CreateRun();
                    runServiceInfo.SetText($"Название услуги: {item["NameList"]}");
                    runServiceInfo.AddCarriageReturn();

                    XWPFRun runDescription = paragraph.CreateRun();
                    runDescription.SetText($"Описание: {item["Description"]}");
                    runDescription.AddCarriageReturn();

                    XWPFRun runPrice = paragraph.CreateRun();
                    runPrice.SetText($"Цена: {item["Price"]} руб");
                    runPrice.AddCarriageReturn();
                }

                if (item.ContainsKey("Surname") && item.ContainsKey("NameStaff") && item.ContainsKey("Patronumic") && item.ContainsKey("Post") &&
                    item.ContainsKey("Telephone") && item.ContainsKey("WorkExperience"))
                {
                    XWPFRun runWorkerInfo = paragraph.CreateRun();
                    runWorkerInfo.SetText($"Работу выполнил: {item["Surname"]} {item["NameStaff"]} {item["Patronumic"]}");
                    runWorkerInfo.AddCarriageReturn();

                    XWPFRun runPost = paragraph.CreateRun();
                    runPost.SetText($"Должность: {item["Post"]}");
                    runPost.AddCarriageReturn();

                    XWPFRun runTelephone = paragraph.CreateRun();
                    runTelephone.SetText($"Телефон: {item["Telephone"]}");
                    runTelephone.AddCarriageReturn();

                    XWPFRun runWorkExperience = paragraph.CreateRun();
                    runWorkExperience.SetText($"Стаж: {item["WorkExperience"]} годам");
                    runWorkExperience.AddCarriageReturn();
                }

                if (item.ContainsKey("NameZap") && item.ContainsKey("Price") && item.ContainsKey("FluidType") &&
                    item.ContainsKey("CountryOfOrigin") && item.ContainsKey("ColvoFluids"))
                {
                    XWPFRun runPartInfo = paragraph.CreateRun();
                    runPartInfo.SetText($"Название: {item["NameZap"]}");
                    runPartInfo.AddCarriageReturn();

                    XWPFRun runPrice = paragraph.CreateRun();
                    runPrice.SetText($"Цена: {item["Price"]} руб");
                    runPrice.AddCarriageReturn();

                    XWPFRun runFluidType = paragraph.CreateRun();
                    runFluidType.SetText($"Тип запчасти\\Тех.Жидкости: {item["FluidType"]}");
                    runFluidType.AddCarriageReturn();

                    XWPFRun runCountryOfOrigin = paragraph.CreateRun();
                    runCountryOfOrigin.SetText($"Страна производства: {item["CountryOfOrigin"]}");
                    runCountryOfOrigin.AddCarriageReturn();

                    XWPFRun runColvoFluids = paragraph.CreateRun();
                    runColvoFluids.SetText($"Количество: {item["ColvoFluids"]}");

                }



                // Сохранение документа
                using (var fs = new FileStream(@"..\..\..\NewWordFile\newFile.docx", FileMode.Create, FileAccess.Write))
                {
                    doc.Write(fs);
                }

                
            }

        }
    }
    public class Order
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public DateTime DateCompleted { get; set; }
    }
}

    




    


