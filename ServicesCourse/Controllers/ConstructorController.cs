using Microsoft.AspNetCore.Mvc;
using DevExpress.XtraReports.Web.QueryBuilder.Services;
using Microsoft.AspNetCore.Authorization;
using DevExpress.DataAccess.Web.QueryBuilder;
using DevExpress.DataAccess.Sql;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using System;

namespace ServicesCourse.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConstructorController : Controller
    {
        static string SQL; 
        public IActionResult Index(
            [FromServices] IQueryBuilderClientSideModelGenerator queryBuilderClientSideModelGenerator
        )
        {
            var newDataConnectionName = "QueryBuilderDB";
            var queryBuilderModel = queryBuilderClientSideModelGenerator.GetModel(newDataConnectionName);
            return View(queryBuilderModel);
        }

        [HttpPost]
        public IActionResult SaveQuery(
            [FromServices] IQueryBuilderInputSerializer queryBuilderInputSerializer,
            [FromForm] DevExpress.DataAccess.Web.QueryBuilder.DataContracts.SaveQueryRequest saveQueryRequest)
        {
            try
            {
                var queryBuilderInput = queryBuilderInputSerializer.DeserializeSaveQueryRequest(saveQueryRequest);
                SelectQuery resultingQuery = queryBuilderInput.ResultQuery;
                SQL = queryBuilderInput.SelectStatement;
                return new RedirectToActionResult("DownloadQuery", "Constructor", null);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public ActionResult DownloadQuery()
        {
            var sql = SQL; 
            var conStr = "Server=(localdb)\\MSSQLLocalDB;Database=DB;Trusted_Connection=True;MultipleActiveResultSets=true";
            var dt = new DataTable("QueryResult");
            using (var connection = new SqlConnection(conStr))
            {
                var cmd = new SqlCommand(sql, connection);
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                adapter.Dispose();
            }
            using (XLWorkbook wb = new())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Noname.xlsx");
                }
            }
        }
    }
}


