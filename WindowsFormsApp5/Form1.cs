using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Production_Application> dataProd; // Заказ на производство
        List<Stage> dataStage; // Этапы
        List<Income> dataIncome; // Заказ на закупку
        List<Material> dataMaterial; // Материал
        List<Stocks> dataStocks; // Запас
        List<Output_fin_products> dataOutputFin; // Выпуск готового продукта
        List<Write_off> dataWriteOff; // Списание
        List<Route_Matrix> dataRoute; // Матрица маршрутов
        List<Transfer> dataTransfer; // Заказ на перемещение
        List<List_portions> dataPortion; // Партия
        List<Transfer_сomposition> dataTranComp; // Состав партии
        List<Warehouse> dataWarehouse; //Склад

        private void listRole_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            listBox1.Items.Clear();
            label1.Visible = true;
            string s = this.listRole.Text;
            dataGridView1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;

            if (s == "Руководитель производства")
            {
                listBox1.Items.Add("Заказов на производство");
                listBox1.Items.Add("Этапов производства");
                listBox1.Items.Add("Списания");
                listBox1.Items.Add("Готовой продукции");
                listBox1.Items.Add("Запасов");
                listBox1.Items.Add("Материалов");
                listBox1.Items.Add("Складов");
            }
            if (s == "Менеджер по закупке")
            {
                listBox1.Items.Add("Закупок");
                listBox1.Items.Add("Материалов");
                listBox1.Items.Add("Запасов");
            }
            if (s == "Менеджер по работе с клиентами")
            {
                listBox1.Items.Add("Заказов на производство");
                listBox1.Items.Add("Готовой продукции");
                listBox1.Items.Add("Материалов");
                listBox1.Items.Add("Запасов");
            }
            if (s == "Логист")
            {
                listBox1.Items.Add("Маршрутов");
                listBox1.Items.Add("Заказов на перемещение");
                listBox1.Items.Add("Партий");
                listBox1.Items.Add("Состава партии");
                listBox1.Items.Add("Складов");
            }
            if (s == "Кладовщик")
            {
                listBox1.Items.Add("Заказов на перемещение");
                listBox1.Items.Add("Запасов");
                listBox1.Items.Add("Списания");
                listBox1.Items.Add("Складов");
            }
        }

        public static void liBox_Mouse1<T>(string cclass, ref List<T> obj, DataGridView dataGridView1, Button button1) where T : new()
        {
            button1.Visible = true;
            obj = Query.GetData<T>($"{cclass}");
            dataGridView1.DataSource = obj;
        }

        public static void liBox_Mouse3<T>(string cclass, ref List<T> obj, DataGridView dataGridView1, Button button1, Button button2, Button button3) where T : new()
        {
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            obj = Query.GetData<T>($"{cclass}");
            dataGridView1.DataSource = obj;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button1_Click(sender, e);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string s = this.listBox1.Text;
            string ss = this.listRole.Text;
            if (s == "Заказов на производство") { Query.ModifyData("Production_Application", dataProd.Cast<object>().ToList(), "ID_Production"); }
            if (s == "Закупок") { Query.ModifyData("Income", dataIncome.Cast<object>().ToList(), "ID_Income"); }
            if (s == "Запасов" && ss == "Кладовщик") { Query.ModifyData("Stocks", dataStocks.Cast<object>().ToList(), "ID_Material"); }
            if (s == "Материалов" && ss == "Менеджер по закупке") { Query.ModifyData("Material", dataMaterial.Cast<object>().ToList(), "ID_Material"); }
            if (s == "Маршрутов") { Query.ModifyData("Route_Matrix", dataRoute.Cast<object>().ToList(), "ID_Route"); }
            if (s == "Заказов на перемещение") { Query.ModifyData("Transfer", dataTransfer.Cast<object>().ToList(), "ID_Transfer"); }
            if (s == "Складов") { Query.ModifyData("Warehouse", dataWarehouse.Cast<object>().ToList(), "ID_Warehouse"); }
            if (s == "Партий") { Query.ModifyData("List_portions", dataPortion.Cast<object>().ToList(), "ID_Portion"); }
            if (s == "Состава партии") { Query.ModifyData("Transfer_сomposition", dataTranComp.Cast<object>().ToList(), "ID_Portion"); } 
            if (s == "Списания") { Query.ModifyData("Write_off", dataWriteOff.Cast<object>().ToList(), "ID_Write_off"); } 
            if (s == "Готовой продукции") { Query.ModifyData("Output_fin_products", dataOutputFin.Cast<object>().ToList(), "ID_Material"); }
            if (s == "Этапов производства") { Query.ModifyData("Stage", dataStage.Cast<object>().ToList(), "ID_Stage"); }
            button1_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            dataGridView1.Visible = true;
            string s = this.listBox1.Text;
            string ss = this.listRole.Text;
            if (s == "Заказов на производство") { liBox_Mouse3("Production_Application", ref dataProd, dataGridView1, button1, button2, button3); }
            if (s == "Закупок") { liBox_Mouse3("Income", ref dataIncome, dataGridView1, button1, button2, button3); }
            if (s == "Материалов" && ss != "Менеджер по закупке") { liBox_Mouse1("Material", ref dataMaterial, dataGridView1, button1); }
            if (s == "Материалов" && ss == "Менеджер по закупке") { liBox_Mouse3("Material", ref dataMaterial, dataGridView1, button1, button2, button3); }
            if (s == "Запасов" && ss != "Кладовщик") { liBox_Mouse1("Stocks", ref dataStocks, dataGridView1, button1); }
            if (s == "Запасов" && ss == "Кладовщик") { liBox_Mouse3("Stocks", ref dataStocks, dataGridView1, button1, button2, button3); }
            if (s == "Маршрутов") { liBox_Mouse3("Route_Matrix", ref dataRoute, dataGridView1, button1, button2, button3); }
            if (s == "Заказов на перемещение") { liBox_Mouse3("Transfer", ref dataTransfer, dataGridView1, button1, button2, button3); }
            if (s == "Складов") { liBox_Mouse3("Warehouse", ref dataWarehouse, dataGridView1, button1, button2, button3); }
            if (s == "Партий") { liBox_Mouse3("List_portions", ref dataPortion, dataGridView1, button1, button2, button3); }
            if (s == "Состава партии") { liBox_Mouse3("Transfer_сomposition", ref dataTranComp, dataGridView1, button1, button2, button3); }
            if (s == "Списания") { liBox_Mouse3("Write_off", ref dataWriteOff, dataGridView1, button1, button2, button3); }
            if (s == "Готовой продукции") { liBox_Mouse3("Output_fin_products", ref dataOutputFin, dataGridView1, button1, button2, button3); }
            if (s == "Этапов производства") { liBox_Mouse3("Stage", ref dataStage, dataGridView1, button1, button2, button3); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s = this.listBox1.Text;
            int rowCount = dataGridView1.RowCount;
            if (s == "Заказов на производство") { Query.AddData("Production_Application", dataProd[rowCount - 1], "ID_Production"); }
            if (s == "Закупок") { Query.AddData("Income", dataIncome[rowCount - 1], "ID_Income"); }
            if (s == "Материалов") { Query.AddData("Material", dataMaterial[rowCount - 1], "ID_Material"); }
            if (s == "Запасов") { Query.AddData("Stocks", dataStocks[rowCount - 1], "ID_Material"); } 
            if (s == "Маршрутов") { Query.AddData("Route_Matrix", dataRoute[rowCount - 1], "ID_Route"); }
            if (s == "Заказов на перемещение") { Query.AddData("Transfer", dataTransfer[rowCount - 1], "ID_Transfer"); }
            if (s == "Складов") { Query.AddData("Warehouse", dataWarehouse[rowCount - 1], "ID_Warehouse"); }
            if (s == "Партий") { Query.AddData("List_portions", dataPortion[rowCount - 1], "ID_Portion"); }
            if (s == "Состава партии") { Query.AddData("Transfer_сomposition", dataTranComp[rowCount - 1], "ID_Portion"); }
            if (s == "Списания") { Query.AddData("Write_off", dataWriteOff[rowCount - 1], "ID_Write_off"); }
            if (s == "Готовой продукции") { Query.AddData("Output_fin_products", dataOutputFin[rowCount - 1], "ID_Material"); }
            if (s == "Этапов производства") { Query.AddData("Stage", dataStage[rowCount - 1], "ID_Stage"); }
            button1_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string s = this.listBox1.Text;
            int row = dataGridView1.CurrentCell.RowIndex;
            int index = (int)dataGridView1.Rows[row].Cells[0].Value;
            if (s == "Заказов на производство") { Query.DeleteData<Production_Application>("Production_Application", dataProd[index-1].ID_Production, "ID_Production"); }
            if (s == "Закупок") { Query.DeleteData<Income>("Income", dataIncome[index - 1].ID_Income, "ID_Income"); }
            if (s == "Материалов") { Query.DeleteData<Material>("Material", dataMaterial[index - 1].ID_Material, "ID_Material"); }
            if (s == "Запасов") { Query.DeleteData<Stocks>("Stocks", dataStocks[index - 1].ID_Material, "ID_Material"); }
            if (s == "Маршрутов") { Query.DeleteData<Route_Matrix>("Route_Matrix", dataRoute[index - 1].ID_Route, "ID_Route"); }
            if (s == "Заказов на перемещение") { Query.DeleteData<Transfer>("Transfer", dataTransfer[index - 1].ID_Transfer, "ID_Transfer"); }
            if (s == "Складов") { Query.DeleteData<Warehouse>("Warehouse", dataWarehouse[index - 1].ID_Warehouse, "ID_Warehouse"); }
            if (s == "Партий") { Query.DeleteData<List_portions>("List_portions", dataPortion[index - 1].ID_Portion, "ID_Portion"); }
            if (s == "Состава партии") { Query.DeleteData<Transfer_сomposition>("Transfer_сomposition", dataTranComp[index - 1].ID_Portion, "ID_Portion"); }
            if (s == "Списания") { Query.DeleteData<Write_off>("Write_off", dataWriteOff[index - 1].ID_Write_off, "ID_Write_off"); }
            if (s == "Готовой продукции") { Query.DeleteData<Output_fin_products>("Output_fin_products", dataOutputFin[index - 1].ID_Material, "ID_Material"); }
            if (s == "Этапов производства") { Query.DeleteData<Stage>("Stage", dataStage[index - 1].ID_Stage, "ID_Stage"); }

            button1_Click(sender, e);
        }
    }
}
