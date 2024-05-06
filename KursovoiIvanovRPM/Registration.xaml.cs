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
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;
using static KursovoiIvanovRPM.MainWindow;
using System.Windows.Markup;

namespace KursovoiIvanovRPM
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ErrorRegTextBlock.Text = "Ошибка: Оба поля должны быть заполнены.";
            }
            else
            {
                List<User> users = JsonManager.ReadUsers();

                if (users.Any(u => u.Login == LoginTextBox.Text))
                {
                    ErrorRegTextBlock.Text = "Ошибка: Логин занят.";
                }
                else
                {
                    users.Add(new User { Login = LoginTextBox.Text, Password = PasswordBox.Password });
                    JsonManager.WriteUsers(users);
                    MessageBox.Show("Вы успешно зарегистрированы.");
                }
            }
        }
        public class JsonManager
        {
            private const string FilePath = @"..\..\..\UsersPassword\LogPas.json";

            public static List<User> ReadUsers()
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    return JsonConvert.DeserializeObject<List<User>>(json);
                }
                return new List<User>();
            }

            public static void WriteUsers(List<User> users)
            {
                string json = JsonConvert.SerializeObject(users);
                File.WriteAllText(FilePath, json);
            }
        }
    }
}

