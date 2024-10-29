using System;
using System.Data;
using System.Reflection;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public class Query
    {
        


        public static List<T> GetData<T>(string tableName) where T : new()
        {
            List<T> dataList = new List<T>();
            using (var conn = new SQLiteConnection("Data Source=kursaach.db"))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand($"SELECT * FROM {tableName}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T obj = new T();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var prop = typeof(T).GetProperty(reader.GetName(i));
                                if (prop != null)
                                {
                                    var val = reader.GetValue(i);
                                    if (val != DBNull.Value)
                                    {
                                        if (prop.PropertyType == typeof(int))
                                        {
                                            prop.SetValue(obj, Convert.ToInt32(val), null);
                                        }
                                        else if (prop.PropertyType == typeof(long))
                                        {
                                            prop.SetValue(obj, Convert.ToInt64(val), null);
                                        }
                                        else if (prop.PropertyType == typeof(double))
                                        {
                                            prop.SetValue(obj, Convert.ToDouble(val), null);
                                        }
                                        else if (prop.PropertyType == typeof(DateTime))
                                        {
                                            prop.SetValue(obj, Convert.ToDateTime(val), null);
                                        }
                                        else
                                        {
                                            prop.SetValue(obj, val, null);
                                        }
                                    }
                                }
                            }
                            dataList.Add(obj);
                        }
                    }
                }
            }
            return dataList;
        }

        public static void StocksQuantity()
        {
            using (var conn = new SQLiteConnection("Data Source=kursaach.db"))
            {
                conn.Open();
                DataTable dataTable = new DataTable();
                string selectCommand = $"SELECT * FROM Stocks";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectCommand, conn))
                {
                    adapter.Fill(dataTable);
                }
                SQLiteCommand command = new SQLiteCommand(conn);
                string trigger = @"CREATE TRIGGER IF NOT EXISTS 'AddStocks'
                                   AFTER INSERT ON 'Income'
                                   BEGIN
                                     UPDATE Stocks
                                     SET Quantity = Quantity + NEW.Quantity
                                     WHERE ID_Material = NEW.ID_Material;
                                   END;";
                command.CommandText = trigger;

            }
        }

        public static void ModifyData(string tableName, List<object> data, string ID)
        {
            string connectionString = "Data Source = kursaach.db;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Create a DataTable to hold the data
                DataTable dataTable = new DataTable();

                // Fill the DataTable with the data from the database
                string selectCommand = $"SELECT * FROM {tableName}";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(selectCommand, connection))
                {
                    adapter.Fill(dataTable);
                }

                // Bind the DataTable to a DataGridView
                DataGridView dataGridView = new DataGridView();
                dataGridView.DataSource = dataTable;
                dataGridView.Size = new Size(500, 400);
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].Tag = i;
                }

                // Show the form containing the DataGridView
                Form form = new Form();
                form.Controls.Add(dataGridView);
                form.Size = new Size(500, 300);
                form.ShowDialog();

                void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
                {
                    DataGridView dgv = (DataGridView)sender;

                    // Get the index of the clicked row from the Tag property
                    int index = (int)dgv.Rows[e.RowIndex].Tag;

                    // Retrieve the instance from the data list
                    if (index < 0 || index >= data.Count)
                    {
                        // Handle the case where the index is out of range
                        return;
                    }
                    object instance = data[index];

                    // Write code to modify the instance here...
                }

                // Update the database with the modified data
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string updateCommand = $"UPDATE {tableName} SET ";
                    List<string> columnNames = new List<string>();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        if (column.ColumnName != ID)
                        {
                            columnNames.Add(column.ColumnName);
                        }
                    }
                    for (int i = 0; i < columnNames.Count; i++)
                    {
                        if (i > 0)
                        {
                            updateCommand += ", ";
                        }
                        updateCommand += $"{columnNames[i]} = @{columnNames[i]}";
                        command.Parameters.AddWithValue($"@{columnNames[i]}", null);
                    }
                    updateCommand += $" WHERE {ID} = @ID";
                    command.Parameters.AddWithValue("@ID", null);
                    command.CommandText = updateCommand;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        for (int i = 0; i < columnNames.Count; i++)
                        {
                            if (columnNames[i] != ID)
                            {
                                command.Parameters[$"@{columnNames[i]}"].Value = row[columnNames[i]];
                            }
                        }
                        command.Parameters["@ID"].Value = row[ID];
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static string ToDateSQLite(DateTime value)
        {
            string format_date = "yyyy-MM-dd HH:mm:00";
            return value.ToString(format_date);
        }

        public static void AddData<T>(string tableName, T obj, string ID)
        {
            using (var conn = new SQLiteConnection("Data Source = kursaach.db"))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    DateTime localDate = DateTime.Now;
                    string str = ToDateSQLite(localDate);
                    Console.WriteLine(str);
                    var props = typeof(T).GetProperties().Where(p => p.CanWrite && p.Name != ID).ToList();
                    var columns = string.Join(",", props.Select(p => p.Name));
                    var values = string.Join(",", props.Select(p => p.PropertyType == typeof(DateTime) ? $"'{DBNull.Value}'" : "0"));

                    cmd.CommandText = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                    // Execute the query and get the auto-increment value
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "SELECT last_insert_rowid()";
                    var newID = cmd.ExecuteScalar();
                    typeof(T).GetProperty(ID).SetValue(obj, Convert.ToInt32(newID));
                }
            }
        }


        public static void DeleteData<T>(string tableName, int id, string ID)
        {
            using (var conn = new SQLiteConnection("Data Source=kursaach.db"))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = $"DELETE FROM {tableName} WHERE {ID} = @ID";
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();

                    // Decrease the seq value in sqlite_sequence table
                    cmd.CommandText = $"UPDATE sqlite_sequence SET seq = seq - 1 WHERE name = @tableName";
                    cmd.Parameters.AddWithValue("@tableName", tableName);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /*
        public static void DeleteData<T>(string tableName, int id, string ID)
        {
            using (var conn = new SQLiteConnection("Data Source=kursaach.db"))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = $"DELETE FROM {tableName} WHERE {ID} = @ID";
                    cmd.CommandText = $"UPDATE sqlite_sequence SET seq = @seq - 1 WHERE {tableName} = @name"; 
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }*/
    }
}