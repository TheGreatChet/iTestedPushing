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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace demoTest.Pages
{
    /// <summary>
    /// Interaction logic for ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage:Page
    {
        int maxPages;
        int countInPage = 10;
        int curPage = 1;

        public ClientsPage()
        {
            InitializeComponent();
            ClientsDg.ItemsSource = ConnectionClass.connection.Client.ToList();
            GenderCb.ItemsSource = ConnectionClass.connection.Gender.ToList();
            GenderCb.DisplayMemberPath = "Name";
            GenderCb.SelectedValuePath = "Code";

            PagesAmountCb.ItemsSource = new Dictionary<string, int>() { { "10", 10 }, { "50", 50 }, { "200", 200 }, { "Все", ConnectionClass.connection.Client.ToList().Count() } };
            PagesAmountCb.DisplayMemberPath = "Key";
            PagesAmountCb.SelectedValuePath = "Value";
            PagesAmountCb.SelectedValue = 10;

            Sort();
        }

        void Sort()
        {
            var list = ConnectionClass.connection.Client.ToList();
            
            if (GenderCb.SelectedValue != null)
            {
                string a = GenderCb.SelectedValue.ToString();
                list = list.Where(x => x.GenderCode == a).ToList();
            }
            if (AppointCountRb.IsChecked.Value)
            {
                list = list.OrderBy(c => c.AppointmentCount).ToList();
            }
            if (LastAppointRb.IsChecked.Value)
            {
                list = list.OrderBy(c => c.LastAppointment).ToList();
            }
            if (SurnameRb.IsChecked.Value)
            {
                list = list.OrderBy(c => c.LastName).ToList();
            }
            if (BirthChb.IsChecked.Value)
            {
                list = list.Where(c => c.Birthday.Value.Month == DateTime.Today.Month).ToList();
            }
            if (!string.IsNullOrEmpty(SearchTb.Text))
            {
                list = list.Where(c => c.Email.Contains(SearchTb.Text)
                                || c.FirstName.Contains(SearchTb.Text)
                                || c.Patronymic.Contains(SearchTb.Text)
                                || c.LastName.Contains(SearchTb.Text)
                                || c.Phone.Contains(SearchTb.Text)).ToList();
            }

            maxPages = (int)Math.Ceiling((list.Count * 1.0) / countInPage);
            list = list.Skip((curPage - 1) * countInPage).Take(countInPage).ToList();
            GenerateButtons();
            PagesTbl.Text = $"{curPage} / {maxPages}";
            CountTbl.Text = $"{list.Count} / {ConnectionClass.connection.Client.ToList().Count()}";

            ClientsDg.ItemsSource = list;
        }

        void GenerateButtons()
        {
            PageBtnsSp.Children.Clear();
            for (int i = 1; i < maxPages + 1; i++)
            {
                Button btn = new Button();
                btn.Content = i.ToString();
                btn.Height = 40;
                btn.Margin = new Thickness(5);
                btn.Click += PageBtn_Click;
                PageBtnsSp.Children.Add(btn);
            }
        }

        void PageBtn_Click(object sender, RoutedEventArgs e)
        {
            curPage = Convert.ToInt32((sender as Button).Content);
            Sort();
        }

        void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Application.Current.Windows.OfType<Window>().ToList().Where(c => c.Title == "Добавление/изменение клиента").Any())
            {
                Windows.AddChangeClientWindow w = new Windows.AddChangeClientWindow(null);
                w.Show();
            }
        }

        void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDg.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента для изменения");
                return;
            }
            Windows.AddChangeClientWindow w = new Windows.AddChangeClientWindow(ClientsDg.SelectedItem as Client);
            w.Show();
        }

        void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDg.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента для удаления");
                return;
            }

            var toDel = ClientsDg.SelectedItem as Client;

            if (ConnectionClass.connection.ClientService.Where(c => c.ClientID == toDel.ID).Any())
            {
                MessageBox.Show("У клиента есть записи, удаление запрещено");
                return;
            }

            foreach (var tag in ConnectionClass.connection.TagOfClient.Where(c => c.ClientID == toDel.ID)) 
                ConnectionClass.connection.TagOfClient.Remove(tag);

            ConnectionClass.connection.Client.Remove(toDel);

            ConnectionClass.connection.SaveChanges();

            Sort();
        }

        private void SearchSort(object sender, TextChangedEventArgs e) => Sort();

        private void GenderSort(object sender, SelectionChangedEventArgs e) => Sort();

        private void RbChbSort(object sender, RoutedEventArgs e) => Sort();

        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            curPage = 1;
            Sort();
        }

        private void PrevPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (curPage == 1)
            {
                MessageBox.Show("Первая страница");
                return;
            }

            curPage--;
            Sort();
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (curPage == maxPages)
            {
                MessageBox.Show("Последняя страница");
                return;
            }

            curPage++;
            Sort();
        }

        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            curPage = maxPages;
            Sort();
        }

        private void PagesAmountCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PagesAmountCb.SelectedItem == null)
                return;

            countInPage = Convert.ToInt32(PagesAmountCb.SelectedValue);
            
            Sort();
            
        }

        private void ClearGender_Click(object sender, RoutedEventArgs e)
        {
            GenderCb.SelectedIndex = -1;
            Sort();
        }

        private void ClearRb_Click(object sender, RoutedEventArgs e)
        {
            AppointCountRb.IsChecked = false;
            LastAppointRb.IsChecked = false;
            SurnameRb.IsChecked = false;

            Sort();
        }

        private void AppointsBtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.ClientAppointmentsWindow w = new Windows.ClientAppointmentsWindow(ClientsDg.SelectedItem as Client);
            w.Show();
        }
    }
}
