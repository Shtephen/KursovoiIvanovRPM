using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using KursovoiIvanovRPM.Application;

namespace KursovoiIvanovRPM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<User> users;
        public MainWindow()
        {
            InitializeComponent();
            LoadUsersFromJson();
        }

        private void LoadUsersFromJson()
        {
            string path = @"..\..\..\UsersPassword\LogPas.json";


            string json = File.ReadAllText(path);
            users = JsonConvert.DeserializeObject<List<User>>(json);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;

            User user = users.Find(u => u.Login == login && u.Password == password);

            if (user != null)
            {
                MenuWindow menuWindow = new MenuWindow();
                menuWindow.Show();
                this.Close();
            }
            else
            {
                ErrorTextBlock.Text = "Логин или пароль неверны!";
            }
        }

        public void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.ShowDialog();
        }
        public class User
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }
    }
}
