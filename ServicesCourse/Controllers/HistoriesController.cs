using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicesCourse.Models;

namespace ServicesCourse.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Histories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.History.Include(h => h.Service).Include(h => h.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Histories/Details/5
        public async Task<IActionResult> Details(string login, DateTime accessTime)
        {

            var history = await _context.History
                .Include(h => h.Service)
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.Login == login &&
                    m.AccessTime.Year == accessTime.Year &&
                    m.AccessTime.Month == accessTime.Month &&
                    m.AccessTime.Day == accessTime.Day &&
                    m.AccessTime.Hour == accessTime.Hour &&
                    m.AccessTime.Minute == accessTime.Minute &&
                    m.AccessTime.Second == accessTime.Second
                );

            if (history == null)
            {
                return NotFound();
            }

            return View(history);
        }

        public IActionResult ActiveServicesToExcel()
        {
            DataTable dt = new DataTable("Активные сервисы");
            dt.Columns.AddRange(new DataColumn[5]
            {
                new DataColumn("Код"),
                new DataColumn("Название"),
                new DataColumn("Раздел"),
                new DataColumn("Подраздел"),
                new DataColumn("Версия"),
            });

            var data = _context.Service
                .Include(s => s.Subsection)
                .ThenInclude(s => s.Section)
                .Where(s => s.ActivityStatus);

            foreach (Service service in data)
            {
                dt.Rows.Add(service.Id, service.ServiceName, service.Subsection.Section.SectionName,
                    service.Subsection.SubscetionName, service.Version);
            }

            using (XLWorkbook wb = new())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Список активных сервисов системы.xlsx");
                }
            }

        }
        public IActionResult ActiveServicesBySectionToExcel()
        {
            DataTable dt = new DataTable("Активные сервисы разделам");
            dt.Columns.AddRange(new DataColumn[3]
            {
                new DataColumn("Код раздела"),
                new DataColumn("Раздел"),
                new DataColumn("Количество сервисов")
            });

            var data = _context.Service
                .Include(s => s.Subsection)
                .ThenInclude(s => s.Section)
                .GroupBy(s => new { s.Subsection.Section.Id, s.Subsection.Section.SectionName })
                .Select(a => new { Name = a.Key, Amount = a.Sum(b => Convert.ToInt32(b.ActivityStatus)) });

            foreach (var group in data)
            {
                dt.Rows.Add(group.Name.Id, group.Name.SectionName, group.Amount);
            };

            using (XLWorkbook wb = new())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Количество активных сервисов по разделам.xlsx");
                }
            }
        }

        public IActionResult AccessHistoryToExcel()
        {
            DataTable dt = new DataTable("История посещений");
            dt.Columns.AddRange(new DataColumn[7]
            {
                new DataColumn("Логин"),
                new DataColumn("Код сервиса"),
                new DataColumn("Название сервиса"),
                new DataColumn("Время посещения"),
                new DataColumn("Раздел"),
                new DataColumn("Подраздел"),
                new DataColumn("Версия")
            });

            var data = _context.History
                .Include(s => s.User)
                .Include(s => s.Service)
                .ThenInclude(s => s.Subsection)
                .ThenInclude(s => s.Section);

            foreach (var history in data)
            {
                dt.Rows.Add(history.Login, history.ServiceId, history.Service.ServiceName, history.AccessTime,
                    history.Service.Subsection.Section.SectionName, history.Service.Subsection.SubscetionName,
                    history.Service.Version);
            };

            using (XLWorkbook wb = new())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "История посещений.xlsx");
                }
            }
        }

        public IActionResult TopAccessServicesToExcel()
        {
            DataTable dt = new DataTable("Самые посещаемые сервисы");
            dt.Columns.AddRange(new DataColumn[3]
            {
                new DataColumn("Код"),
                new DataColumn("Сервис"),
                new DataColumn("Количество посещений")
            });

            var data = _context.History
                .Include(s => s.Service)
                .ThenInclude(s => s.Subsection)
                .ThenInclude(s => s.Section)
                .GroupBy(s => new { s.ServiceId, s.Service.ServiceName })
                .Select(a => new { Name = a.Key, Amount = a.Count() })
                .OrderBy(s => s.Amount)
                .Take(5);

            foreach (var group in data)
            {
                dt.Rows.Add(group.Name.ServiceId, group.Name.ServiceName, group.Amount);
            };

            using (XLWorkbook wb = new())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Самые посещаемые сервисы.xlsx");
                }
            }
        }

        public IActionResult NotVisitedServicesToExcel()
        {
            DataTable dt = new DataTable("Непосещенные сервисы");
            dt.Columns.AddRange(new DataColumn[5]
            {
                new DataColumn("Код"),
                new DataColumn("Название"),
                new DataColumn("Раздел"),
                new DataColumn("Подраздел"),
                new DataColumn("Версия")
            });

            var all_data = _context.Service.Where(s => s.ActivityStatus).Distinct();

            int hours = 24;
            DbFunctions dbFunctions = null;

            var g_data = _context.History
                .Include(s => s.Service)
                .Where(s => dbFunctions.DateDiffHour(s.AccessTime, DateTime.UtcNow) <= hours)
                .Select(s => s.Service)
                .Distinct();


            var data = all_data.Except(g_data).Include(s => s.Subsection).ThenInclude(s => s.Section);

            foreach (Service service in data)
            {
                dt.Rows.Add(service.Id, service.ServiceName, service.Subsection.Section.SectionName,
                    service.Subsection.SubscetionName, service.Version);
            };

            using (XLWorkbook wb = new())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Непосещенные сервисы.xlsx");
                }
            }
        }
    }
}
