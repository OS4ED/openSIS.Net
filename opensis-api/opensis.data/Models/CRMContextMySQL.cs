using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace opensis.data.Models
{
    public class CRMContextMySQL : CRMContext
    {
        private DbContextOptions contextOptions;
        public CRMContextMySQL() { }
        public CRMContextMySQL(DbContextOptions options) : base(options)
        {
            this.contextOptions = options;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "server=localhost;database={tenant};user=root;password=admin@123";
                optionsBuilder.UseMySql(connectionString.Replace("{tenant}", " "));
                /* ********* bob server*********
                 * 
                 string connectionString = "server=;port=;database={tenant};user=;password=";
                 optionsBuilder.UseMySql(connectionString.Replace("{tenant}", "       "));*/

                //string connectionString = "server=;port=;database={tenant};user=;password=m";
                //optionsBuilder.UseMySql(connectionString.Replace("{tenant}", "   "));

            }

        }
    }
}
