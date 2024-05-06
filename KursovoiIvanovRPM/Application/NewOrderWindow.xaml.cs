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

namespace KursovoiIvanovRPM.Application
{
    /// <summary>
    /// Логика взаимодействия для NewOrderWindow.xaml
    /// </summary>
    public partial class NewOrderWindow : Window
    {
        private int orderIdCounter = 1;
        private List<Order> orders = new List<Order>();

        private Order selectedOrder;

        public NewOrderWindow(Order order = null)
        {
            InitializeComponent();

            LoadOrders();

            if (order != null)
            {
                selectedOrder = orders.FirstOrDefault(o => o.ID == order.ID);
                if (selectedOrder != null)
                {
                    FillFieldsWithSelectedOrder();
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(StatusTextBox.Text))
            {
                return;
            }

            if (selectedOrder != null) // Редактирование существующего заказа
            {
                selectedOrder.Name = NameTextBox.Text;
                selectedOrder.DateCreated = DateCreatedDatePicker.SelectedDate?.Date ?? DateTime.Today;
                selectedOrder.Status = StatusTextBox.Text;
                selectedOrder.DateCompleted = DateCompletedDatePicker.SelectedDate?.Date ?? DateTime.Today;
            }
            else // Создание нового заказа
            {
                Order newOrder = new Order
                {
                    ID = orderIdCounter,
                    Name = NameTextBox.Text,
                    DateCreated = DateCreatedDatePicker.SelectedDate?.Date ?? DateTime.Today,
                    Status = StatusTextBox.Text,
                    DateCompleted = DateCompletedDatePicker.SelectedDate?.Date ?? DateTime.Today
                };

                orders.Add(newOrder);
                orderIdCounter++;
            }

            string json = JsonConvert.SerializeObject(orders);
            string filePath = @"..\..\..\InformationOrders\Orders.json";
            File.WriteAllText(filePath, json);

            // Обновляем список заказов
            LoadOrders();
            this.Close();
        }

        private void LoadOrders()
        {
            string filePath = @"..\..\..\InformationOrders\Orders.json";

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                orders = JsonConvert.DeserializeObject<List<Order>>(json);

                // Находим максимальный Id среди загруженных заказов
                orderIdCounter = orders.Count > 0 ? orders.Max(o => o.ID) + 1 : 1;
            }
            else
            {
                orders = new List<Order>(); 
            }
        }
        private void FillFieldsWithSelectedOrder()
        {
            NameTextBox.Text = selectedOrder.Name;
            DateCreatedDatePicker.SelectedDate = selectedOrder.DateCreated;
            StatusTextBox.Text = selectedOrder.Status;
            DateCompletedDatePicker.SelectedDate = selectedOrder.DateCompleted;
        }


    }
}