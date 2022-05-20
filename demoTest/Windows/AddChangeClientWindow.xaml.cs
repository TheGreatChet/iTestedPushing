using demoTest.ADO;
using demoTest.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace demoTest.Windows
{
    /// <summary>
    /// Interaction logic for AddChangeClientWindow.xaml
    /// </summary>
    public partial class AddChangeClientWindow:Window
    {
        Client Cur;
        bool isNew = true;
        byte[] imgBytes;

        public AddChangeClientWindow(Client cur)
        {
            InitializeComponent();
            Cur = cur;
            GenderCb.ItemsSource = ConnectionClass.connection.Gender.ToList();
            GenderCb.DisplayMemberPath = "Name";
            GenderCb.SelectedValuePath = "Code";
            ImageSourceConverter converter = new ImageSourceConverter();

            if (Cur != null)
            {
                isNew = false;
                NameTb.Text = Cur.FirstName;
                SurnameTb.Text = Cur.LastName;
                PatrTb.Text = Cur.Patronymic;
                EmailTb.Text = Cur.Email;
                PhoneTb.Text = Cur.Phone;
                BirthdayDp.SelectedDate = Cur.Birthday;
                GenderCb.SelectedValue = Cur.GenderCode;
                if(Cur.Photo != null) PhotoImg.Source = (ImageSource)converter.ConvertFrom(Cur.Photo);
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTb.Text) || string.IsNullOrEmpty(SurnameTb.Text) || string.IsNullOrEmpty(EmailTb.Text) || string.IsNullOrEmpty(PhoneTb.Text) || BirthdayDp.SelectedDate == null || GenderCb.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля (поле отчество не обязательное)");
                return;
            }

            if (!ConstraintsClass.EmailIsValid(EmailTb.Text))
            {
                MessageBox.Show("Некорректный формат почты (example@example.ru");
                return;
            }

            if (Cur == null)
            {
                ConnectionClass.connection.Client.Add(new Client()
                {
                    FirstName = NameTb.Text,
                    LastName = SurnameTb.Text,
                    Patronymic = PatrTb.Text,
                    Email = EmailTb.Text,
                    Phone = PhoneTb.Text,
                    Birthday = BirthdayDp.SelectedDate,
                    GenderCode = GenderCb.SelectedValue.ToString(),
                    RegistrationDate = DateTime.Now,
                    Photo = imgBytes
                });

                ConnectionClass.connection.SaveChanges();
            }
            else
            {
                Cur.FirstName = NameTb.Text;
                Cur.LastName = SurnameTb.Text;
                Cur.Patronymic = PatrTb.Text;
                Cur.Email = EmailTb.Text;
                Cur.Phone = PhoneTb.Text;
                Cur.Birthday = BirthdayDp.SelectedDate;
                Cur.GenderCode = GenderCb.SelectedValue.ToString();
                Cur.Photo = imgBytes;

                ConnectionClass.connection.SaveChanges();
            }
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            mw.NavFr.Navigate(new Pages.ClientsPage());
            mw.NavFr.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

            this.Close();

        }
        /// <summary>
        /// Input constraints
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FIO_PreviewTextInput(object sender, TextCompositionEventArgs e) => ConstraintsClass.FIOConstrain(e);

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e) => ConstraintsClass.ExceptSpace(e);

        private void EmailTb_PreviewTextInput(object sender, TextCompositionEventArgs e) => ConstraintsClass.EmailConstraint(e);

        private void PhoneTb_PreviewTextInput(object sender, TextCompositionEventArgs e) => ConstraintsClass.PhoneConstraint(e);

        private void ChoosePhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            BitmapImage img = new BitmapImage();
            if (fileDialog.ShowDialog() == true)
            {
                img = new BitmapImage(new Uri(fileDialog.FileName, UriKind.RelativeOrAbsolute));
                PhotoImg.Source = img;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(img));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    imgBytes = ms.ToArray();
                }
            }
        }
    }
}
