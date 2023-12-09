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
        public int[] VanFoodstuf(string ingrediance, string[] ingmass, string ID,string tabname)
        {
            int[] van = new int[ingmass.Length];
            string between = "";
            string query = $"SELECT {ingrediance} FROM {tabname} WHERE Account_van='{ID}' ";
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
            string query1 = $"Select {ingrediance} FROM Storage WHERE Id_van= '0' ";
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
        public void StorageUpDate(int[] count, string Account_van, string[] ingrediance,int[]invan)
        {
            string ing="";
            for (int i = 0; i < count.Length; i++)
            {
                ing = ing + $"{ingrediance[i]}={count[i]+invan[i]}" + ",";
            }
            string query = $"UPDATE Van SET {ing} Ordering=0 WHERE Account_van = '{Account_van}'";          
            OleDbCommand com = new OleDbCommand(query, sqlConnection);
            com.ExecuteNonQuery();
            string sto = "";
            for (int i = 0; i < count.Length; i++)
            {
                sto += $"{ingrediance[i]}={count[i]},";
            }
            sto = sto.TrimEnd(' ', ',');
            query = $"Update Storage Set {sto} Where Id_van='{Account_van}'";
            OleDbCommand comm = new OleDbCommand(query, sqlConnection);
            comm.ExecuteNonQuery();
        }
        public void AddIteminMenu(string category, string name,int price,string[] ingmass, int[] countmas)
        {
            string ingrediance="";
            string count="";
            string fullname = category+ "_"+ name;
            for (int i = 0; i < ingmass.Length; i++)
            {
                ingrediance = ingrediance + ingmass[i]+",";
                count += "'"+Convert.ToString(countmas[i])+"'"+",";
            }
            string query = $"INSERT INTO Menu ({ingrediance}Dish,Price) VALUES({count}'{fullname}','{price}')";
            OleDbCommand com = new OleDbCommand(query, sqlConnection);
            com.ExecuteNonQuery();
        }
        public List<string> Alldishes()
        {
            using (OleDbCommand com = new OleDbCommand($"Select Dish From Menu", sqlConnection))
            {
                using (OleDbDataReader reader = com.ExecuteReader())
                {
                    List<string> values = new List<string>();
                    while (reader.Read())
                    {
                        string value = reader["Dish"] != DBNull.Value ? reader["Dish"].ToString() : null;
                        values.Add(value);
                    }
                    return values;
                }
            }
        }
        public void Deleteitem(string item)
        {
            string query = $"Delete From Menu Where Dish like'{item}'";
            OleDbCommand com = new OleDbCommand(query, sqlConnection);
            com.ExecuteNonQuery();
        }

        public void CashirReport(List<string>orderlist,string role,string ingrediance, string[] ingmass)
        {
            string Dishes = "";
            var counts = orderlist.GroupBy(x => x).Select(group => new { Value = group.Key, Count = group.Count() });
            foreach (var count in counts)
            {
                int x = count.Count + inreport(count.Value, role);
                Dishes += count.Value + "=" + x +",";
            }
            Dishes = Dishes.TrimEnd(',');
            string query = $"Update Report Set {Dishes} Where N_Van='{role}'";
            OleDbCommand com = new OleDbCommand(query, sqlConnection);
            com.ExecuteNonQuery();
        }
        private int inreport(string dish,string role)
        {
            string query = $"Select {dish} From Report Where N_Van='{role}'";
            using (OleDbCommand com = new OleDbCommand(query, sqlConnection))
            {
                using (OleDbDataReader reader = com.ExecuteReader())
                {
                    int values=0;
                    while (reader.Read())
                    {
                        string value = reader[dish] != DBNull.Value ? reader[dish].ToString() : null;
                        values=Convert.ToInt32(value);
                    }
                    return values;
                }
            }
        }
    }
}
