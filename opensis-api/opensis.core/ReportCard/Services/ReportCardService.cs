using opensis.core.ReportCard.Interfaces;
using opensis.data.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.ReportCard.Services
{
    public class ReportCardService : IReportCardService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IReportCardRepository reportCardRepository;
        public ReportCardService(IReportCardRepository reportCardRepository)
        {
            this.reportCardRepository = reportCardRepository;
        }

    }
}
