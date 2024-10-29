using System.ComponentModel;
using System;
namespace WindowsFormsApp5
{



    public class Income
    {
        [DisplayName("ID Поступления")]
        public int ID_Income { get; set; }

        [DisplayName("ID Материала")]
        public int ID_Material { get; set; }

        [DisplayName("Количество")]
        public int Quantity { get; set; }

        [DisplayName("Дата")]
        public DateTime Date { get; set; }
    }
    public class List_portions
    {
        [DisplayName("ID Партии")]
        public int ID_Portion { get; set; }

        [DisplayName("ID Заказа по перемещение")]
        public int ID_Transfer { get; set; }
    }
    public class Material
    {
        [DisplayName("ID Материала")]
        public int ID_Material { get; set; }

        [DisplayName("Название материала")]
        public string Name { get; set; }
    }
    public class Output_fin_products
    {
        [DisplayName("ID Продукта")]
        public int ID_Material { get; set; }

        [DisplayName("ID Заказа на производство")]
        public int ID_Production { get; set; }

        [DisplayName("ID Склада")]
        public int ID_Warehouse { get; set; }

        [DisplayName("Количество")]
        public int Quantity { get; set; }

        [DisplayName("Дата")]
        public DateTime Date { get; set; }
    }
    public class Production_Application
    {
        [DisplayName("ID Заказа на производство")]
        public int ID_Production { get; set; }

        [DisplayName("ID Материала")]
        public int ID_Material { get; set; }

        [DisplayName("Количество")]
        public int Quantity { get; set; }

        [DisplayName("Дата")]
        public DateTime Date { get; set; }
    }
    public class Route_Matrix
    {
        [DisplayName("ID Маршрута")]
        public int ID_Route { get; set; }

        [DisplayName("ID Склада А")]
        public int ID_From { get; set; }

        [DisplayName("ID Склада Б")]
        public int ID_To { get; set; }

        [DisplayName("Времени на маршрут, час")]
        public int Time { get; set; }
    }
    public class Stage
    {
        [DisplayName("ID Заказа на производство")]
        public int ID_Production { get; set; }

        [DisplayName("ID Этапа")]
        public int ID_Stage { get; set; }

        [DisplayName("ID Следующего этапа")]
        public int Next { get; set; }
    }
    public class Stocks
    {
        [DisplayName("ID Материала")]
        public int ID_Material { get; set; }

        [DisplayName("ID Склада")]
        public int ID_Warehouse { get; set; }

        [DisplayName("Количество")]
        public int Quantity { get; set; }
    }
    public class Transfer
    {
        [DisplayName("ID Заказа на перемещение")]
        public int ID_Transfer { get; set; }

        [DisplayName("ID Маршрута")]
        public int ID_Route { get; set; }

        [DisplayName("Дата погрузка")]
        public string Embarkation { get; set; }

        [DisplayName("Дата отгрузки")]
        public string Shipment { get; set; }
    }
    public class Transfer_сomposition
    {
        [DisplayName("ID Партии")]
        public int ID_Portion { get; set; }

        [DisplayName("ID Материала")]
        public int ID_Material { get; set; }

        [DisplayName("Количество")]
        public int Quantity { get; set; }
    }
    public class Warehouse
    {
        [DisplayName("ID Склада")]
        public int ID_Warehouse { get; set; }

        [DisplayName("Адрес")]
        public string Address { get; set; }

        [DisplayName("Транзитный")]
        public bool Transit { get; set; }

        [DisplayName("Производственный")]
        public bool Production { get; set; }

    }

    public class Write_off
    {
        [DisplayName("ID Списания")]
        public int ID_Write_off { get; set; }

        [DisplayName("ID Заказа на производство")]
        public int ID_Production { get; set; }

        [DisplayName("ID Этапа")]
        public int ID_Stage { get; set; }

        [DisplayName("ID Материала")]
        public int ID_Material { get; set; }

        [DisplayName("Количество")]
        public int Quantity { get; set; }
    }
}