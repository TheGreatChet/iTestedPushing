using demoTest.ADO;
using demoTest.Classes;
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

namespace demoTest.Windows
{
    /// <summary>
    /// Interaction logic for ClientAppointmentsWindow.xaml
    /// </summary>
    public partial class ClientAppointmentsWindow:Window
    {
        Client Cur;
        public ClientAppointmentsWindow(Client cur)
        {
            InitializeComponent();
            Cur = cur;
            AppointsDg.ItemsSource = ConnectionClass.connection.ClientService.Where(c => c.ClientID == Cur.ID).ToList();
        }
    }
}
