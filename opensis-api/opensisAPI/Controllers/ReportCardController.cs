using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.ReportCard.Interfaces;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/ReportCard")]
    [ApiController]
    public class ReportCardController : ControllerBase
    {
        private IReportCardService _reportCardService;
        public ReportCardController(IReportCardService reportCardService)
        {
            _reportCardService = reportCardService;
        }

    }
}
