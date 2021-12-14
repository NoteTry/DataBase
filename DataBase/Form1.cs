using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBase
{
    public partial class Form1 : Form
    {
        private DB DataBase = new DB();
       
        

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 formDb = new Form2();
            this.Hide();
            formDb.ShowDialog();
            this.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            DataBase.startDb();

            DataBase.state();

            
            // ----
            revenueBindingSource.DataSource = new List<Revenue>();

            SelectData();

            cartesianChart1.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Month",
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
            });
            cartesianChart1.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Revenue",
                LabelFormatter = value => value.ToString("C")
            });
            cartesianChart1.LegendLocation = LiveCharts.LegendLocation.Right;
            // ----
            
        }

        private void SelectData()
        {
            foreach (string[] item in DataBase.GetListCSheet("SELECT * FROM [Table]", 3))
            {
                revenueBindingSource.Add(new Revenue() { Value = int.Parse(item[2]), Month = int.Parse(item[1]), Year = int.Parse(item[0]) });
            }
        }

        private void InitData()
        {
            /**
             * Иницилизация данных в график
             **/

            cartesianChart1.Series.Clear();
           
            SeriesCollection series = new SeriesCollection();
            var years = (from o in revenueBindingSource.DataSource as List<Revenue>
                         select new { Year = o.Year }).Distinct();

            foreach (var year in years)
            {
                List<double> values = new List<double>();
                for (int month = 1; month <= 12; month++)
                {
                    double value = 0;
                    var data = from o in revenueBindingSource.DataSource as List<Revenue>
                               where o.Year.Equals(year.Year) && o.Month.Equals(month)
                               orderby o.Month ascending
                               select new { o.Value, o.Month };

                    if (data.SingleOrDefault() != null)
                        value = data.SingleOrDefault().Value;

                    values.Add(value);
                }
                series.Add(new LineSeries() { Title=year.Year.ToString(), Values=new ChartValues<double>(values) });
                
                cartesianChart1.Series = series;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InitData();
        }
    }
}
