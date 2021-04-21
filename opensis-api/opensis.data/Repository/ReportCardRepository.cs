using opensis.data.Interface;
using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Repository
{
    public class ReportCardRepository : IReportCardRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public ReportCardRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

    }
}
