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
                optionsBuilder.UseMySql(connectionString.Replace("{tenant}", "opensisv2new"));
                /* ********* bob server*********
                 * 
                 string connectionString = "server=159.89.143.110;port=3306;database={tenant};user=opensisadmin;password=Opens1s@2021";
                 optionsBuilder.UseMySql(connectionString.Replace("{tenant}", "opensisv2"));*/

                //string connectionString = "server=103.230.103.93;port=3306;database={tenant};user=admin;password=methodolog1c";
                //optionsBuilder.UseMySql(connectionString.Replace("{tenant}", "opensisv2_test1"));

            }

        }
    }
}
