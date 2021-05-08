using EbuyNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbuyNetwork.Controllers
{
    public class MyOrdersController : Controller
    {
        // GET: MyOrders
        public ActionResult Index(string keyword,int? status, string startDate,string endDate,int? page)
        {
            if (Session["loggedUser"] == null) RedirectToAction("Index", "Login");
            if (page == null)
                page = 0;
            ViewBag.page = page;

            string filter = "";


            if (keyword != null)
            {
                ViewBag.keyword = keyword;
                filter += " AND [Item].name LIKE '%" + keyword + "%'";
            }

            ViewBag.status = status;
            if (status == 2)
            {
                filter += "  AND [Item].delivered=0 ";
            }
            else if (status == 3)
            {
                filter += " ";
            }

            if ((startDate!= "" && endDate != "") && (startDate != null && endDate != null))
            {
                ViewBag.startDate = startDate;
                ViewBag.endDate = endDate;
                filter += " AND CAST([Order].ordered_time AS DATE)>='" + startDate + "' AND CAST([Order].ordered_time AS DATE)<='" + endDate + "'";
            }


            User user = (User)Session["loggedUser"];
            SqlConnection con = SQLCon.getConnection();


            SqlCommand qTotal = con.CreateCommand();
            qTotal.CommandType = CommandType.Text;
            qTotal.CommandText = "SELECT COUNT([Order].id) AS total FROM [Order] LEFT JOIN [Item] ON [Order].item_id=[Item].id LEFT JOIN [Category] ON [Item].category_id=[Category].id WHERE [Order].user_id=" + user.id + "" + filter;

            SqlDataReader resultTotal = qTotal.ExecuteReader();
            resultTotal.Read();

            int totalResults = Int32.Parse(resultTotal["total"].ToString());
            int totalPages = (int)Math.Ceiling((double)totalResults / 16);
            ViewBag.totalResult = totalResults;
            ViewBag.totalPages = totalPages;



            resultTotal.Close();



            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;
            q.CommandText = "SELECT [Order].ordered_time AS ordered_time,[Order].user_id AS bid,[Category].id AS category_id, [Category].item_count AS category_item_count, [Category].name AS category_name, [Order].payment_code AS payment_code, [Item].* FROM [Order] LEFT JOIN [Item] ON [Order].item_id=[Item].id LEFT JOIN [Category] ON [Item].category_id=[Category].id WHERE [Order].user_id=" + user.id+""+filter+ " ORDER BY [Order].id DESC OFFSET " + page*16 + " ROWS FETCH NEXT 16 ROWS ONLY";
            SqlDataReader result = q.ExecuteReader();
            List<Order> orders = new List<Order>();
            while (result.Read())
            {
                orders.Add(new Order {
                    item = new Item
                    {
                        id = Int32.Parse(result["id"].ToString()),
                        category = new Category
                        {
                            id = Int32.Parse(result["category_id"].ToString()),
                            item_count = Int32.Parse(result["category_item_count"].ToString()),
                            name = result["category_name"].ToString()

                        },
                        userId = Int32.Parse(result["user_id"].ToString()),
                        isSold = false,
                        isDelivered = false,
                        price = Double.Parse(result["price"].ToString()),
                        name = result["name"].ToString(),
                        imageFile = result["imagefile"].ToString(),
                        description = result["description"].ToString()

                    },
                    paymentCode = "15548",
                    orderedTime = DateTime.Parse(result["ordered_time"].ToString()),
                    buyerId = Int32.Parse(result["bid"].ToString())

                });
            }
            ViewBag.orders=orders;
            ViewBag.item_count = totalResults;


            /*pages */
            string longurl = Request.Url.AbsoluteUri;
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["page"] = "pageNumber";

            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            ViewBag.qurl = longurl;

            return View();
        }
    }
}