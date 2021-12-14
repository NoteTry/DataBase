using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace DataBase
{
    public partial class Form2 : Form
    {
        private DB DataBase = new DB();

        private int DataIdCustomer;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DataBase.startDb();

            DataBase.state();

            edit();

            LoadDataCSheet();

            AddListBoxServices();
            AddListEmployees();
            AddListBoxCalculationSheet();

            ListBoxCustomerSelect();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                DataBase.command(
                    $"INSERT INTO Сustomer (Name, Date) VALUES (N'{textBox4.Text}', '{DateTime.Parse(dateTimePicker1.Text.ToString())}')"
                    ).ExecuteNonQuery();

                SqlCommand commandLastId = DataBase.command(
                    "SELECT @@IDENTITY"
                    );

                int lastId = int.Parse(commandLastId.ExecuteScalar().ToString());

                DataBase.command($"INSERT INTO [CalculationSheet] (Id_Customer, Id_Employees, Id_Services) VALUES ('{lastId}', '{listBox3.SelectedValue}', '{listBox4.SelectedValue}')").ExecuteNonQuery();

                MessageBox.Show("Запись добавлена успешно!!");
            }
        }

        private void AddListBoxCalculationSheet()
        {
            ArrayList allDataSheet = new ArrayList();

            SqlCommand data = DataBase.command("SELECT * FROM CalculationSheet");

            SqlDataReader SDR = data.ExecuteReader();

            foreach (DbDataRecord rezult in SDR)
            {
                allDataSheet.Add(rezult);
            }

            SDR.Close();

            if (allDataSheet.Count != 0) {
                listBox2.DataSource = allDataSheet;
                listBox2.DisplayMember = "Id";
                listBox2.ValueMember = "Id";

            } 
        }

        private void AddListBoxServices()
        {
            ArrayList allData = new ArrayList();

            SqlCommand data = DataBase.command("SELECT * FROM Services");
            SqlDataReader SDR = data.ExecuteReader();

            foreach (DbDataRecord rezult in SDR)
            {
                allData.Add(rezult);
            }

            SDR.Close();

            listBox4.DataSource = allData;
            listBox4.DisplayMember = "Name";
            listBox4.ValueMember = "Id";

            listBox5.DataSource = allData;
            listBox5.DisplayMember = "Name";
            listBox5.ValueMember = "Id";
        }

        private void AddListEmployees()
        {
            ArrayList allDataEm = new ArrayList();

            SqlCommand data = DataBase.command("SELECT * FROM Employees");
            SqlDataReader SDR = data.ExecuteReader();

            foreach (DbDataRecord rezult in SDR)
            {
                allDataEm.Add(rezult);
            }
            SDR.Close();

            listBox3.DataSource = allDataEm;
            listBox3.DisplayMember = "Name";
            listBox3.ValueMember = "Id";

            listBox6.DataSource = allDataEm;
            listBox6.DisplayMember = "Name";
            listBox6.ValueMember = "Id";
        }

        private void LoadDataCSheet()
        {
            foreach (string[] item in DataBase.GetListCSheet("SELECT * FROM CalculationSheet"))
            {
                item[1] = GetNameСustomer(int.Parse(item[1]));
                item[2] = GetEmployees(int.Parse(item[2]));
                item[3] = GetServices(int.Parse(item[3]));

                dataGridView2.Rows.Add(item);
            }
        }

        private void ListBoxCustomerSelect()
        {
            ArrayList SheelDataList = new ArrayList();

            foreach (string[] item in DataBase.GetListCSheet("SELECT * FROM CalculationSheet"))
            {
                SheelDataList.Add(new CreateListBox(item[0].ToString(), GetNameСustomer(int.Parse(item[1]))));
            }

            listBox8.DataSource = SheelDataList;
            listBox8.DisplayMember = "GetName";
            listBox8.ValueMember = "GetId";
        }

        private string GetNameСustomer(int id)
        {
            SqlCommand data = DataBase.command("SELECT Name FROM Сustomer WHERE Id = '" + id + "'");
            SqlDataReader reader = data.ExecuteReader();

            reader.Read();
            string NameCustomer = reader[0].ToString();
            reader.Close();

            return NameCustomer;
        }

        private string GetEmployees(int id)
        {
            SqlCommand data = DataBase.command("SELECT Name FROM Employees WHERE Id = '" + id + "'");
            SqlDataReader reader = data.ExecuteReader();

            reader.Read();
            string Employees = reader[0].ToString();
            reader.Close();

            return Employees;
        }

        private string GetServices(int id)
        {
            SqlCommand data = DataBase.command("SELECT Name FROM Services WHERE Id = '" + id + "'");
            SqlDataReader reader = data.ExecuteReader();

            reader.Read();
            string Services = reader[0].ToString();
            reader.Close();

            return Services;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlCommand data = DataBase.command($"SELECT * FROM CalculationSheet WHERE Id = '{listBox2.SelectedValue}'");
            SqlDataReader reader = data.ExecuteReader();

            reader.Read();
           
                int IdCustomer = int.Parse(reader[1].ToString());
                int IdCSheel = int.Parse(reader[0].ToString());

            reader.Close();
            
            DataBase.command($"DELETE FROM Сustomer WHERE Id = '" + IdCustomer + "'").ExecuteNonQuery();
            DataBase.command($"DELETE FROM [CalculationSheet] WHERE Id = '" + IdCSheel + "'").ExecuteNonQuery();

            MessageBox.Show("Запись с клиентом удалина!");
        }

        private void edit()
        {
            ArrayList SheelDataList = new ArrayList();

            foreach (string[] item in DataBase.GetListCSheet("SELECT * FROM CalculationSheet"))
            {
                SheelDataList.Add(new CreateListBox(item[0].ToString(), GetNameСustomer(int.Parse(item[1]))));
            }

            listBox7.DataSource = SheelDataList;
            listBox7.DisplayMember = "GetName";
            listBox7.ValueMember = "GetId";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();

            foreach (string[] item in DataBase.GetListCSheet($"SELECT * FROM [CalculationSheet] WHERE Id = '{listBox7.SelectedValue}'"))
            {
                DataIdCustomer = int.Parse(item[1]);

                item[1] = GetNameСustomer(int.Parse(item[1]));
                item[2] = GetEmployees(int.Parse(item[2]));
                item[3] = GetServices(int.Parse(item[3]));
                
                textBox5.Text = item[1];
                dataGridView3.Rows.Add(item);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox5.Text != "")
            {
                DataBase.command($"UPDATE [CalculationSheet] SET Id_Employees = '{listBox6.SelectedValue}', Id_Services = '{listBox5.SelectedValue}' WHERE Id = '{listBox7.SelectedValue}'").ExecuteNonQuery();
                

                DataBase.command($"UPDATE [Сustomer] SET Name = '{textBox5.Text}' WHERE Id = '{DataIdCustomer}'").ExecuteNonQuery();
                
                MessageBox.Show("Данные обновлены!");
            }
        }
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = null;

            ArrayList SheelDataList = new ArrayList();

            foreach (string[] item in DataBase.GetListCSheet("SELECT * FROM CalculationSheet"))
            {
                SheelDataList.Add(new CreateListBox(item[0].ToString(), GetNameСustomer(int.Parse(item[1])) + "\t" + GetEmployees(int.Parse(item[2])) + "\t" + GetServices(int.Parse(item[3]))));
            }

            listBox1.DataSource = SheelDataList;
            listBox1.DisplayMember = "GetName";
            listBox1.ValueMember = "GetId";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = null;

            ArrayList SheelDataList = new ArrayList();

            foreach (string[] item in DataBase.GetListCSheet($"SELECT * FROM CalculationSheet WHERE Id = '{listBox8.SelectedValue}'"))
            {
                SheelDataList.Add(new CreateListBox(item[0].ToString(), GetNameСustomer(int.Parse(item[1])) + "\t" + GetEmployees(int.Parse(item[2])) + "\t" + GetServices(int.Parse(item[3]))));
            }

            listBox1.DataSource = SheelDataList;
            listBox1.DisplayMember = "GetName";
            listBox1.ValueMember = "GetId";
        }

        
    }
}
