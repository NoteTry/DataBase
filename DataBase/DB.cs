using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataBase
{
    class DB
    {
        private SqlConnection sqlConnection = null;

        public void startDb()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_db"].ConnectionString);

            sqlConnection.Open();
        }

        public SqlCommand command(string value)
        {
            SqlCommand command = new SqlCommand(value, sqlConnection);

            return command;
        }

        public void state()
        {
            if (this.getSqlConnection().State != ConnectionState.Open)
                MessageBox.Show("Ошибка подключения!!!");
        }

        public SqlConnection getSqlConnection()
        {
            return sqlConnection;
        }

        public List<string[]> GetListCSheet(string value, int num = 4)
        {
            SqlCommand data = command(value);
            SqlDataReader reader = data.ExecuteReader();

            List<string[]> SheelData = new List<string[]>();

            while (reader.Read())
            {
                SheelData.Add(new string[4]);

                for (int i = 0; i< num; i++)
                    SheelData[SheelData.Count - 1][i] = reader[i].ToString();
            }
            reader.Close();

            return SheelData;
        }
    }
}
