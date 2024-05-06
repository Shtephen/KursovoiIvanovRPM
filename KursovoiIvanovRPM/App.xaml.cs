using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KursovoiIvanovRPM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Очистка ресурсов при запуске приложения
            Resources.Clear();
        }
    }
}
