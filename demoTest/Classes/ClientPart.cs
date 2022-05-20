using demoTest.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoTest.ADO
{
    public partial class Client
    {
        public DateTime LastAppointment
        {
            get
            {
                if (ConnectionClass.connection.ClientService.Where(x => x.ClientID == ID).ToList().Any())
                    return ConnectionClass.connection.ClientService.Where(x => x.ClientID == ID).ToList().Max(c => c.StartTime);
                else
                    return new DateTime(2000, 1, 1);
            }
        }

        public int AppointmentCount
        {
            get
            {
                if (ConnectionClass.connection.ClientService.Where(x => x.ClientID == ID).ToList().Any())
                    return ConnectionClass.connection.ClientService.Where(x => x.ClientID == ID).ToList().Count();
                else
                    return 0;
            }
        }
    }
}