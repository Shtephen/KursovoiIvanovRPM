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
    /// Логика взаимодействия для NewStaffWindow.xaml
    /// </summary>
    public partial class NewStaffWindow : Window
    {

        private Staff selectedStaff;
        public NewStaffWindow(Staff staff = null)
        {
            InitializeComponent();

          
            if (staff != null) 
            {
                selectedStaff = staff;
                StaffID.Text = staff.ID.ToString();
                StaffSurname.Text = staff.Surname;
                StaffName.Text = staff.NameStaff;
                StaffPatronumic.Text = staff.Patronumic;
                StaffPost.Text = staff.Post;
                StaffPhone.Text = staff.Telephone;
                StaffWorkExperience.Text = staff.WorkExperience;    
            }
           
        }

        private void NewStaffSaveButton_Click(object sender, RoutedEventArgs e) 
        {
           string jsonFile = @"..\..\..\InfoStaff\Staff.json";
          List<Staff> staffs = new List<Staff>();
            
            if (File.Exists(jsonFile)) 
            {
                 string json = File.ReadAllText(jsonFile);
                 staffs = JsonConvert.DeserializeObject<List<Staff>>(json);
            }

            if (selectedStaff == null)
            {
                Staff newStaff = new Staff
                {
                    ID = int.Parse(StaffID.Text),
                    Surname = StaffSurname.Text,
                    NameStaff = StaffName.Text,
                    Patronumic = StaffPatronumic.Text,
                    Post = StaffPost.Text,
                    Telephone = StaffPhone.Text,
                    WorkExperience = StaffWorkExperience.Text
                };
                staffs.Add(newStaff);
            }
            else 
            {
                selectedStaff.ID = int.Parse(StaffID.Text);
                selectedStaff.Surname = StaffSurname.Text;
                selectedStaff.NameStaff = StaffName.Text;
                selectedStaff.Patronumic = StaffPatronumic.Text;
                selectedStaff.Post = StaffPost.Text;
                selectedStaff.Telephone = StaffPhone.Text;
                selectedStaff.WorkExperience = StaffWorkExperience.Text;

                int index = staffs.FindIndex (c => c.ID == selectedStaff.ID);
                if(index != -1)
                {
                    staffs[index] = selectedStaff;
                }
            }

            string updateJson = JsonConvert.SerializeObject(staffs, Formatting.Indented);
            File.WriteAllText(jsonFile, updateJson);
            this.Close();
        }
    }
}
