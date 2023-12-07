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
            using (OleDbCommand com = new OleDbCommand($"Select Account_van From Van", sqlConnection))
            {
                using (OleDbDataReader reader = com.ExecuteReader())
                {
                    List<string> values = new List<string>();
                    while (reader.Read())
                    {
                        string value = reader["Account_van"] != DBNull.Value ? reader["Account_van"].ToString() : null;
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
        public int[] VanFoodstuf(string ingrediance, string[] ingmass, string ID)
        {
            int[] van = new int[ingmass.Length];
            string between = "";
            string query = $"SELECT {ingrediance} FROM Van WHERE Account_van='{ID}' ";
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
                    }
                }
            }
            return van;
        }
        public int[] HaveToBeAdd(string[] ingmass,string ingrediance,int[] van)
        {
            int[] HaveToBe = new int[van.Length];
            string between = "";
            int[] Base = new int[ingmass.Length];
            string query1 = $"Select {ingrediance} FROM Storage WHERE Id_van= 0 ";
            using (OleDbCommand com = new OleDbCommand(query1, sqlConnection))
            {
                using (OleDbDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int j = 0; j < ingmass.Length; j++)
                        {
                            between = reader[$"{ingmass[j]}"] != DBNull.Value ? reader[$"{ingmass[j]}"].ToString() : null;
                            Base[j] = Base[j] + Convert.ToInt32(between);
                        }
                    }
                }
            }
            for (int i = 0; i < ingmass.Length; i++)
            {
                HaveToBe[i] = Base[i] - van[i];
            }
            return HaveToBe;
        }      
        public void StorageUpDate(int[] count, string IDVAN, string[] ingrediance,int[]invan)
        {
            string ing="";
            for (int i = 0; i < count.Length; i++)
            {
                ing = ing + $"{ingrediance[i]}={count[i]+invan[i]}" + ",";
            }
            string query = $"UPDATE Van SET {ing} Ordering=0 WHERE Account_van = '{IDVAN}'";          
            OleDbCommand com = new OleDbCommand(query, sqlConnection);
            com.ExecuteNonQuery();
        }
    }
}
