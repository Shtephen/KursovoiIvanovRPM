using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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
    public class Staff
    {
        public int ID { get; set; }
        public string Surname { get; set; }
        public string NameStaff { get; set; }
        public string Patronumic { get; set; }
        public string Post { get; set; }
        public string Telephone { get; set; }
        public string WorkExperience { get; set; }
    }

    public partial class StaffWindow : Window
    {
        public StaffWindow()
        {
            InitializeComponent();
            string filePath = @"..\..\..\InfoStaff\Staff.json";
            List<Staff> staffs = LoadStaffFromJson(filePath);
            DataContext = new { Staff = staffs };
        }

        private void StaffBackToMenu_Click (object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }

        private void NewStaffButton_Click (object sender, RoutedEventArgs e)
        {
            NewStaffWindow newStaffWindow = new NewStaffWindow();
            newStaffWindow.ShowDialog();

            string filePath = @"..\..\..\InfoStaff\Staff.json";
            List<Staff> staff = LoadStaffFromJson(filePath);
            StaffDataGrid.ItemsSource = staff;
        }

        public List<Staff> LoadStaffFromJson (string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Staff>>(json);
        }

        private void DeleteKlientsButton_Click(object sender, RoutedEventArgs e)
        {
            if(StaffDataGrid.SelectedItems.Count > 0) 
            {
                List<Staff> staff = ((IEnumerable<Staff>)StaffDataGrid.ItemsSource).ToList();
                foreach (Staff staff1 in StaffDataGrid.SelectedItems) 
                {
                    staff.Remove(staff1);
                }

                string filePath = @"..\..\..\InfoStaff\Staff.json";
                SaveStaffToJson(filePath, staff);

                StaffDataGrid.ItemsSource = staff;
            }
        }

        public void SaveStaffToJson (string filePath, List<Staff> staff)
        {
            string json = JsonConvert.SerializeObject(staff, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void RedaktStaffButton_Click (object sender, RoutedEventArgs e) 
        {
            if (StaffDataGrid.SelectedItems.Count > 0)
            {
                Staff selectedStaff = (Staff)StaffDataGrid.SelectedItem;
                NewStaffWindow newStaffWindow = new NewStaffWindow(selectedStaff);
                newStaffWindow.Closed += (s, args) =>
                {
                    string filePath = @"..\..\..\InfoStaff\Staff.json";
                    List<Staff> staff = LoadStaffFromJson(filePath);
                    StaffDataGrid.ItemsSource = staff;
                };
                newStaffWindow.ShowDialog();
            }
        }
    }
}
