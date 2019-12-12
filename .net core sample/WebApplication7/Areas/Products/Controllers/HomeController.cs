using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApplication7.Controllers
{
    [Area("Products")]
    public class Home1Controller : Controller
    {
        public ActionResult Index()
        {
            var Order = OrdersDetails.GetAllRecords();
            var Order1 = OrdersData.GetAllRecords();
            ViewBag.DataSource = BigData.GetAllRecords();
            ViewBag.DataSource1 = Order;
            ViewBag.DataSource2 = OrdersData.GetAllRecords();
            ViewBag.DataSource3 = from a in Order
                                  join b in Order1 on a.EmployeeID equals b.EmployeeID
                        select new { a.CustomerID, a.EmployeeID, a.OrderDate, a.OrderID, b.ShipName,
                            b.ShipAddress
                        };
            return View();
        }

        public IActionResult UrlAdaptor()
        {

            return View();
        }
        public IActionResult UrlDatasource([FromBody]DataManagerRequest dm)
        {
            IEnumerable DataSource = OrdersDetails.GetAllRecords();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<OrdersDetails>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);         //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }

        public class OrdersDetails
        {
            public static List<OrdersDetails> order = new List<OrdersDetails>();
            public OrdersDetails()
            {

            }
            public OrdersDetails(int OrderID, int EmployeeId, string CustomerId, DateTime OrderDate)
            {
                this.OrderID = OrderID;
                this.EmployeeID = EmployeeId;
                this.CustomerID = CustomerId;
                this.OrderDate = OrderDate;

            }
            public static List<OrdersDetails> GetAllRecords()
            {
                if (order.Count() == 0)
                {
                    int code = 10000;
                    for (int i = 1; i < 5; i++)
                    {
                        order.Add(new OrdersDetails(code + 1,3, "ALFKI", new DateTime(1991, 05, 15)));
                        order.Add(new OrdersDetails(code + 2,4, "ANATR", new DateTime(1990, 04, 04)));
                        order.Add(new OrdersDetails(code + 3,1, "ANTON",  new DateTime(1957, 11, 30)));
                        order.Add(new OrdersDetails(code + 4,3, "BLONP",  new DateTime(1930, 10, 22)));
                        order.Add(new OrdersDetails(code + 5, 4,"BOLID",  new DateTime(1953, 02, 18)));
                        code += 5;
                    }
                }
                return order;
            }
            public int? OrderID { get; set; }
            public int? EmployeeID { get; set; }
            public string CustomerID { get; set; }
            public DateTime OrderDate { get; set; }

        }

        public class OrdersData
        {
            public static List<OrdersData>
    order1 = new List<OrdersData>
        ();
            public OrdersData()
            {

            }
            public OrdersData(int EmployeeId, string ShipName, string ShipAddress)
            {

                this.EmployeeID = EmployeeId;
                this.ShipName = ShipName;
                this.ShipAddress = ShipAddress;
            }
            public static List<OrdersData>
                GetAllRecords()
            {
                if (order1.Count() == 0)
                {
                    int code = 10000;
                    for (int i = 1; i < 5; i++)
                    {
                        order1.Add(new OrdersData(3, " bistro", "Kirchgasse 6"));
                        order1.Add(new OrdersData(4, " Cozinha",  "Avda. Azteca 123"));
                        order1.Add(new OrdersData(1, "Frankenversand", "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                        order1.Add(new OrdersData(3, " Handel","Magazinweg 7"));
                        order1.Add(new OrdersData(4, " Carnes","1029 - 12th Ave. S."));
                        code += 5;
                    }
                }
                return order1;
            }

            public int? EmployeeID { get; set; }

            public string ShipName { get; set; }
            public string ShipAddress { get; set; }
        }

        public class BigData
        {
            public static List<BigData> order = new List<BigData>();
            public BigData()
            {

            }
            public BigData(int OrderID, string CustomerId, int EmployeeId, double Freight, bool Verified, DateTime OrderDate, string ShipCity, string ShipName, string ShipCountry, DateTime ShippedDate, string ShipAddress)
            {
                this.OrderID = OrderID;
                this.CustomerID = CustomerId;
                this.EmployeeID = EmployeeId;
                this.Freight = Freight;
                this.ShipCity = ShipCity;
                this.Verified = Verified;
                this.OrderDate = OrderDate;
                this.ShipName = ShipName;
                this.ShipCountry = ShipCountry;
                this.ShippedDate = ShippedDate;
                this.ShipAddress = ShipAddress;
            }
            public static List<BigData> GetAllRecords()
            {
                if (order.Count() == 0)
                {
                    int code = 10000;
                    for (int i = 1; i < 15; i++)
                    {
                        order.Add(new BigData(code + 1, "ALFKI", i + 0, 2.3 * i, false, new DateTime(1991, 05, 15), "Berlin", "Simons bistro", "Denmark", new DateTime(1996, 7, 16), "Kirchgasse 6"));
                        order.Add(new BigData(code + 2, "ANATR", i + 2, 3.3 * i, true, new DateTime(1990, 04, 04), "Madrid", "Queen Cozinha", "Brazil", new DateTime(1996, 9, 11), "Avda. Azteca 123"));
                        order.Add(new BigData(code + 3, "ANTON", i + 1, 4.3 * i, true, new DateTime(1957, 11, 30), "Cholchester", "Frankenversand", "Germany", new DateTime(1996, 10, 7), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                        order.Add(new BigData(code + 4, "BLONP", i + 3, 5.3 * i, false, new DateTime(1930, 10, 22), "Marseille", "Ernst Handel", "Austria", new DateTime(1996, 12, 30), "Magazinweg 7"));
                        order.Add(new BigData(code + 5, "BOLID", i + 4, 6.3 * i, true, new DateTime(1953, 02, 18), "Tsawassen", "Hanari Carnes", "Switzerland", new DateTime(1997, 12, 3), "1029 - 12th Ave. S."));
                        code += 5;
                    }
                }
                return order;
            }
            public int? OrderID { get; set; }
            public string CustomerID { get; set; }
            public int? EmployeeID { get; set; }
            public double? Freight { get; set; }
            public string ShipCity { get; set; }
            public bool Verified { get; set; }
            public DateTime OrderDate { get; set; }
            public string ShipName { get; set; }
            public string ShipCountry { get; set; }
            public DateTime ShippedDate { get; set; }
            public string ShipAddress { get; set; }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
