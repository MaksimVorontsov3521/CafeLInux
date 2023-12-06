using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Text.RegularExpressions;


namespace Fuck
{
    class DishesFromMenu
    {
        private OleDbConnection sqlConnection = null;
        public DishesFromMenu()
        {
            sqlConnection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Sqlcon"].ConnectionString);            
            sqlConnection.Open();
        }
        // Заполнение comboBox товарами по категориям
        public List<string> FillBox(string something,string what,int n)
        {            

            using (OleDbCommand com = new OleDbCommand($"Select {something} From Menu where Dish Like '{what}%'", sqlConnection))
            {
                using (OleDbDataReader reader = com.ExecuteReader())
                {
                    List<string> values = new List<string>();
                    while (reader.Read())
                    {
                        string value = reader[something] != DBNull.Value ? reader[something].ToString() : null;
                        values.Add(value);
                    }
                    for (int i = 0; i < values.Count; i++)
                    {// убрать название категории
                        values[i] = values[i].Remove(0, n);
                    }
                    return values;
                }
            }
        }
        // Поиск всех фургонов
        public List<string> FillVans()
        {
            using (OleDbCommand com = new OleDbCommand($"Select Id_van From Van", sqlConnection))
            {
                using (OleDbDataReader reader = com.ExecuteReader())
                {
                    List<string> values = new List<string>();
                    while (reader.Read())
                    {
                        string value = reader["Id_van"] != DBNull.Value ? reader["Id_van"].ToString() : null;
                        values.Add(value);
                    }
                    return values;
                }
            }
        }
        // Получение названий ингредиентов
        //
        public string COLUMN_NAME(string tableName)
        {
            DataTable schemaTable = sqlConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });
            
            string columnsNames = "";

            foreach (DataRow row in schemaTable.Rows)
            {
                string columnName = row["COLUMN_NAME"].ToString();
                columnsNames += columnName + ", ";
            }

            // Удаление последней запятой и пробела, если они есть
            columnsNames = columnsNames.TrimEnd(' ', ',');
            return columnsNames;
        }
        public string Ingrediance(string columnsNames)
        { 
            // Слова, которые нужно вырезать
            string[] wordsToRemove = {"Dish,"," Price,"," "};
            foreach (var word in wordsToRemove)
            {
                columnsNames = columnsNames.Replace(word, "");
            }
            columnsNames = columnsNames.TrimEnd(' ', ',');
            return columnsNames;
        }
        //
        // Продукты одного фургона
        public int[] VanFoodstuf(string ingrediance ,string[] ingmass,string ID)
        {
            int[] van = new int[ingmass.Length];
            string between = "";
            string query = $"SELECT {ingrediance} FROM Van WHERE Id_van='{ID}' ";
            using (OleDbCommand com = new OleDbCommand(query, sqlConnection))
            {
                using (OleDbDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int j = 0; j < ingmass.Length; j++)
                        {
                            between = reader[$"{ingmass[j]}"] != DBNull.Value ? reader[$"{ingmass[j]}"].ToString() : null;
                            van[j] = van[j] + Convert.ToInt32(between);
                        }
                        //between = reader[$"Ordering"] != DBNull.Value ? reader[$"Ordering"].ToString() : null;                       
                    }
                }
            }
            return van;
        }
    }
}
