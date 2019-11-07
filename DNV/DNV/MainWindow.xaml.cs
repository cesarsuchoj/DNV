using System;
using System.Collections.Generic;
using System.Data;
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
using DNVDO.Model;
using DNVDO.Model.Data;

namespace DNVDO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataSet dataSet;
        public List<Registro> registros;

        public MainWindow()
        {
            InitializeComponent();
            dataSet = new DataSet();
            UpdateLists();
            dataGrid.ItemsSource = registros;
            List<string> books = new List<string>();
            books.Add(Books.DNV.value.ToString());
            books.Add(Books.DO.value.ToString());
            cmbBook.ItemsSource = books;
            List<string> Sexes = new List<string>();
            Sexes.Add(Sex.MALE.value.ToString());
            Sexes.Add(Sex.FEMALE.value.ToString());
            cmbSex.ItemsSource = Sexes;
        }

        private void UpdateLists()
        {
            registros = DataAccessLayer.ReadRegistry();
        }
        private void ClearFields()
        {
            txtRegistry.Text = "";
            cmbBook.Text = "";
            txtBookNumber.Text = "";
            txtPage.Text = "";
            dtpRegistryDate.SelectedDate = DateTime.Now;
            txtName.Text = "";
            cmbSex.Text = "";
            dtpBirthDate.SelectedDate = DateTime.Now;
            dtpBirthHour.Text = DateTime.Now.TimeOfDay.ToString();
            txtFatherName.Text = "";
            dtpFatherBirthDate.SelectedDate = DateTime.Now;
            lblFatherAge.Content = "";
            txtFatherCity.Text = "";
            txtMotherName.Text = "";
            dtpMotherBirthDate.SelectedDate = DateTime.Now;
            lblMotherAge.Content = "";
            txtMotherCity.Text = "";
            txtDocument.Text = "";
            ckbIsOnDeadline.IsChecked = false;
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DataAccessLayer.InputRegistry(new Registro(Int32.Parse(txtRegistry.Text), cmbBook.Text, Int32.Parse(txtBookNumber.Text), Int32.Parse(txtPage.Text), dtpRegistryDate.SelectedDate.ToString(), txtName.Text, cmbSex.Text, dtpBirthDate.SelectedDate.ToString(), dtpBirthHour.Text, txtFatherName.Text, dtpFatherBirthDate.SelectedDate.ToString(), txtFatherCity.Text, txtMotherName.Text, dtpMotherBirthDate.SelectedDate.ToString(), txtMotherCity.Text, txtDocument.Text, ((bool)ckbIsOnDeadline.IsChecked ? 1 : 0)));
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
        private void TxtDocument_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtDocument.Text.Equals(""))
                if (cmbBook.SelectedItem.ToString().Equals("A"))
                {
                    if (!BusinessLogicalLayer.DNVValidate(txtDocument.Text))
                        MessageBox.Show("Numero de documento invalido, por favor verificar o numero digitado.", "Numero invalido", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    if (!BusinessLogicalLayer.DOValidate(txtDocument.Text))
                        MessageBox.Show("Numero de documento invalido, por favor verificar o numero digitado.", "Numero invalido", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
        }
        private void DtpFatherBirthDate_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                lblFatherAge.Content = BusinessLogicalLayer.IdadePai(dtpFatherBirthDate.SelectedDate.Value);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Idade invalida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
        }
        private void DtpMotherBirthDate_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                lblMotherAge.Content = BusinessLogicalLayer.IdadeMae(dtpMotherBirthDate.SelectedDate.Value, dtpRegistryDate.SelectedDate.Value, ckbIsOnDeadline.IsChecked.Value);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Idade invalida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void txtbusca_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void Txtbusca_KeyDwon(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                BtnBusca_Click(sender, new RoutedEventArgs());
            }
        }
        private void BtnBusca_Click(object sender, RoutedEventArgs e)
        {
            DataAccessLayer.ReadRegistry(int.Parse(txtBusca.Text));
        }

    }
}
