using demoTest.ADO;
using demoTest.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace demoTest.Classes
{
    public static class ConstraintsClass
    {
        public static void FIOConstrain(TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"[аА-яЯ -]"))
                e.Handled = true;
        }

        public static void EmailConstraint(TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"[aA-zZ0-9@.]"))
                e.Handled = true;
        }

        public static void PhoneConstraint(TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"[0-9]"))
                e.Handled = true;
        }

        public static bool EmailIsValid(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return string.IsNullOrEmpty(mail.DisplayName);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void ExceptSpace(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}
