using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.Dto
{

    public class SalesReportItemDto
    {
        public string MovieTitle { get; set; }
        public string StudioName { get; set; }
        public int TicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class SalesReportDto
    {
        public string ReportType { get; set; }
        public string Period { get; set; }
        public decimal GrandTotalRevenue { get; set; }
        public int GrandTotalTickets { get; set; }
        public List<SalesReportItemDto> Details { get; set; }
    }

}