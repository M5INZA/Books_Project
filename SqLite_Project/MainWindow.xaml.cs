using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.Sqlite;

namespace SqLite_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Main varibles
        SqliteConnection conn = new SqliteConnection(@"Data Source=BookDatabase.db");
        SqliteCommand cmd;

        public MainWindow()
        {
            InitializeComponent();
        }

        //get data from book table
        private void loaddata()
        {
            cmd = new SqliteCommand("SELECT * FROM Book;", conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            Books_Grid.ItemsSource = dt.DefaultView;
            Books_Grid.SelectedItem = null;
        }

        private void Main_Win_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                loaddata();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption: " + ex.Message);
            }
        }

        private void Btn_New_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Txt_Id.Focus();
                Txt_Id.Text = "";
                Txt_Name.Text = "";
                Txt_Price.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption: " + ex.Message);
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(Txt_Id.Text!=""&&Txt_Name.Text != "" &&Txt_Price.Text != "")
                {
                    string addCommand = $"Insert into Book Values({Txt_Id.Text},'{Txt_Name.Text}',{Txt_Price.Text});";
                    cmd = new SqliteCommand(addCommand, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Add Sucsesfoly", "Program", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Information is empty!!\nEnter the information.", "Program", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                loaddata();
            }
            catch (Exception ex)
            {
                if (ex.Message == "SQLite Error 19: 'UNIQUE constraint failed: Book.Id'.")
                {
                    MessageBox.Show("This ID is already assigned to another book!\nEnter another ID.", "Program", MessageBoxButton.OK, MessageBoxImage.Stop);
                    Txt_Id.Focus();
                    Txt_Id.SelectAll();
                }
                else
                {
                    MessageBox.Show("Exeption: " + ex.Message);
                }
            }
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Books_Grid.SelectedItem != null && Txt_Id.Text != "" && Txt_Name.Text != "" && Txt_Price.Text != "")
                {
                    DataRowView drv = Books_Grid.SelectedItem as DataRowView;
                    if (drv[0].ToString() == Txt_Id.Text && drv[1].ToString() == Txt_Name.Text && drv[2].ToString() == Txt_Price.Text)
                    {
                        MessageBox.Show("Nothing has changed", "Program", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        string addCommand = $"Update Book Set Id={Txt_Id.Text},Name='{Txt_Name.Text}',Price={Txt_Price.Text} WHERE id={drv[0]};";
                        cmd = new SqliteCommand(addCommand, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Edit Successfully", "Program", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("No item selected!\nSelect the item.", "Program", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                loaddata();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption: " + ex.Message);
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Books_Grid.SelectedItem != null)
                {
                    DataRowView drv = Books_Grid.SelectedItem as DataRowView;
                    if (MessageBox.Show($"Are you sure delete({drv[0]} - {drv[1]} - {drv[2]})?", "Program", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        cmd = new SqliteCommand($"Delete from book where id={drv[0]};", conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    MessageBox.Show("No item selected!\nSelect the item.", "Program", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                loaddata();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption: " + ex.Message);
            }
        }

        private void Books_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Books_Grid.SelectedItem != null)
                {
                    DataRowView drv = Books_Grid.SelectedItem as DataRowView;
                    Txt_Id.Text = drv[0].ToString();
                    Txt_Name.Text = drv[1].ToString();
                    Txt_Price.Text = drv[2].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption: " + ex.Message);
            }
        }

        private void Txt_Price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Rx1 = new Regex(@"^\d{1,4}([.]\d{1,2})?$");
            e.Handled = !Rx1.IsMatch(e.Text);
        }

        private void Txt_Id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex Rx1 = new Regex(@"^\d{1,4}([.]\d{1,2})?$");
            e.Handled = !Rx1.IsMatch(e.Text);
        }

        private void English_ch_Checked(object sender, RoutedEventArgs e)
        {
            Add_Grid.FlowDirection = FlowDirection.LeftToRight;
            Books_Grid.FlowDirection = FlowDirection.LeftToRight;
            Add_group.Header = "Add Form";
            Id_lable.Text = "Id:";
            Name_lable.Text = "Name:";
            Price_lable.Text = "Price:";
            Btn_Add.Content = "Add";
            Btn_New.Content = "*N";
            Btn_Edit.Content = "Edit";
            Btn_Delete.Content = "Delete";
            Id_Clmn.Header = "Id";
            Name_Clmn.Header = "Name";
            Price_Clmn.Header = "Price";
        }

        private void Arabic_ch_Checked(object sender, RoutedEventArgs e)
        {
            Add_Grid.FlowDirection = FlowDirection.RightToLeft;
            Books_Grid.FlowDirection = FlowDirection.RightToLeft;
            Add_group.Header = "مكان الاضافة";
            Id_lable.Text = "الرمز:";
            Name_lable.Text = "الاسم:";
            Price_lable.Text = "السعر:";
            Btn_Add.Content = "اضافة";
            Btn_New.Content = "*N";
            Btn_Edit.Content = "تعديل";
            Btn_Delete.Content = "حذف";
            Id_Clmn.Header = "الرمز";
            Name_Clmn.Header = "الاسم";
            Price_Clmn.Header = "السعر";
        }

        private void Main_Win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            conn.Close();
        }
    }
}
