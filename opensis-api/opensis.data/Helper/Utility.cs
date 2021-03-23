using opensis.data.Models;
using opensis.data.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace opensis.data.Helper
{
    
    public static class Utility
    {
        /// <summary>
        /// This method returns a int primarykeyId  for an entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cRMContext"></param>
        /// <param name="columnSelector"></param>
        /// <returns></returns>
        public static int? GetMaxPK<TEntity>(CRMContext cRMContext, Func<TEntity, int> columnSelector) where TEntity : class
        {
            int? GetMaxId = 0;
           
                
                var entityClass = cRMContext?.Set<TEntity>();
                if (entityClass.Count() == 0)
                {
                    GetMaxId = 1;
                }
                else
                {
                    GetMaxId = cRMContext?.Set<TEntity>().Max(columnSelector);
                    if (GetMaxId == null || GetMaxId <= 0)
                    {
                        GetMaxId = 1;
                    }
                    else
                    {
                        GetMaxId = GetMaxId + 1;
                    }
                }


            return GetMaxId;
        }


        //public static int? GetIntMaxPK<TEntity>(CRMContext cRMContext, Func<TEntity, int> columnSelector, string tenantid, string schoolid) where TEntity : class
        //{
        //    int? GetMaxId = 0;

        //    List<FilterParams> filters = new List<FilterParams>();

        //    FilterParams param1 = new FilterParams();
        //    param1.ColumnName = "TenantId";
        //    param1.FilterValue = tenantid;
        //    param1.JoinCondition = "AND";
        //    param1.FilterOption = FilterOptions.IsEqualTo;

        //    FilterParams param2 = new FilterParams();
        //    param2.ColumnName = "SchoolId";
        //    param2.FilterValue = schoolid;
        //    param2.FilterOption = FilterOptions.IsEqualTo;


        //    filters.Add(param1);
        //    filters.Add(param2);

        //    var entityClass = cRMContext?.Set<TEntity>().AsQueryable();
            
        //    entityClass = entityClass.Where(filters);

        //    if (entityClass.Count() == 0)
        //    {               
        //            GetMaxId = 1;                
        //    }
        //    else
        //    {
        //        GetMaxId = cRMContext?.Set<TEntity>().AsQueryable().Where(filters).Max(columnSelector);

        //        if (GetMaxId == null || GetMaxId <= 0)
        //        {

        //            GetMaxId = 1;
        //        }
        //        else
        //        {
        //            GetMaxId = GetMaxId + 1;
        //        }
        //    }


        //    return GetMaxId;
        //}


        public static long? GetMaxLongPK<TEntity>(CRMContext cRMContext, Func<TEntity, long> columnSelector) where TEntity : class
        {
            long? GetMaxId = 0;


            var entityClass = cRMContext?.Set<TEntity>();
            if (entityClass.Count() == 0)
            {
                GetMaxId = 1;
            }
            else
            {
                GetMaxId = cRMContext?.Set<TEntity>().Max(columnSelector);
                if (GetMaxId == null || GetMaxId <= 0)
                {
                    GetMaxId = 1;
                }
                else
                {
                    GetMaxId = GetMaxId + 1;
                }
            }


            return GetMaxId;
        }

        /// <summary>
        /// This method returns a decrypt string for a password.
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            string passwordKey = "oPen$!$.b14Ca5898a4e4133b!";
            byte[] cipherBytes = Convert.FromBase64String(cipherText); using (Aes encryptor = Aes.Create())
            {
                var salt = cipherBytes.Take(16).ToArray();
                var iv = cipherBytes.Skip(16).Take(16).ToArray();
                var encrypted = cipherBytes.Skip(32).ToArray();
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(passwordKey, salt, 100); encryptor.Key = pdb.GetBytes(32);
                encryptor.Padding = PaddingMode.PKCS7;
                encryptor.Mode = CipherMode.CBC;
                encryptor.IV = iv; using (MemoryStream ms = new MemoryStream(encrypted))
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cs, Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method returns a hashed string for a password.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string GetHashedPassword(string Input)
        {
            string passwordSecurityKey = "oPenSIsV2.0lOGinS1c0R3t8K61";
            var sha1 = System.Security.Cryptography.SHA256.Create();
            var inputBytes = Encoding.ASCII.GetBytes(Input + passwordSecurityKey);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string sortBy, string sortDirection)
        {
            var param = Expression.Parameter(typeof(T), "item");

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            switch (sortDirection.ToLower())
            {
                case "asc":
                    return source.AsQueryable<T>().OrderBy<T, object>(sortExpression);
                default:
                    return source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);
            }
        }


        public static IEnumerable<T> FilteredData<T>(List<FilterParams> filterParams, IEnumerable<T> data)
        {

            IEnumerable<string> distinctColumns = filterParams.Where(x => !String.IsNullOrEmpty(x.ColumnName)).Select(x => x.ColumnName).Distinct();

            foreach (string colName in distinctColumns)
            {
                var filterColumn = typeof(T).GetProperty(colName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (filterColumn != null)
                {
                    IEnumerable<FilterParams> filterValues = filterParams.Where(x => x.ColumnName.Equals(colName)).Distinct();

                    if (filterValues.Count() > 1)
                    {
                        IEnumerable<T> sameColData = Enumerable.Empty<T>();

                        foreach (var val in filterValues)
                        {
                            sameColData = sameColData.Concat(FilterData(val.FilterOption, data, filterColumn, val.FilterValue));
                        }

                        data = data.Intersect(sameColData);
                    }
                    else
                    {
                        data = FilterData(filterValues.FirstOrDefault().FilterOption, data, filterColumn, filterValues.FirstOrDefault().FilterValue);
                    }
                }
            }
            return data;
        }
        private static IEnumerable<T> FilterData<T>(FilterOptions filterOption, IEnumerable<T> data, PropertyInfo filterColumn, string filterValue)
        {
            int outValue;
            DateTime dateValue;
            switch (filterOption)
            {
                #region [StringDataType]  

                case FilterOptions.StartsWith:
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().StartsWith(filterValue.ToString().ToLower())).ToList();
                    break;
                case FilterOptions.EndsWith:
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().EndsWith(filterValue.ToString().ToLower())).ToList();
                    break;
                case FilterOptions.Contains:
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().Contains(filterValue.ToString().ToLower())).ToList();
                    break;
                case FilterOptions.DoesNotContain:
                    data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                     (filterColumn.GetValue(x, null) != null && !filterColumn.GetValue(x, null).ToString().ToLower().Contains(filterValue.ToString().ToLower()))).ToList();
                    break;
                case FilterOptions.IsEmpty:
                    data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                     (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString() == string.Empty)).ToList();
                    break;
                case FilterOptions.IsNotEmpty:
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString() != string.Empty).ToList();
                    break;
                #endregion

                #region [Custom]  

                case FilterOptions.IsGreaterThan:
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) > outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) > dateValue).ToList();

                    }
                    break;

                case FilterOptions.IsGreaterThanOrEqualTo:
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) >= outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) >= dateValue).ToList();
                        break;
                    }
                    break;

                case FilterOptions.IsLessThan:
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) < outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) < dateValue).ToList();
                        break;
                    }
                    break;

                case FilterOptions.IsLessThanOrEqualTo:
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) <= outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) <= dateValue).ToList();
                        break;
                    }
                    break;

                case FilterOptions.IsEqualTo:
                    if (filterValue == string.Empty)
                    {
                        data = data.Where(x => filterColumn.GetValue(x, null) == null
                                        || (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() == string.Empty)).ToList();
                    }
                    else
                    {
                        if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                        {
                            data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) == outValue).ToList();
                        }
                        else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                        {
                            data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) == dateValue).ToList();
                            break;
                        }
                        else
                        {
                            data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() == filterValue.ToLower()).ToList();
                        }
                    }
                    break;

                case FilterOptions.IsNotEqualTo:
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) != outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) != dateValue).ToList();
                        break;
                    }
                    else
                    {
                        data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                         (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() != filterValue.ToLower())).ToList();
                    }
                    break;
                    #endregion
            }
            return data;
        }

        public static DataTable ToPivotTable<T, TColumn, TRow, TData>(
         this IEnumerable<T> source,
         Func<T, TColumn> columnSelector,
         Expression<Func<T, TRow>> rowSelector,
         Func<IEnumerable<T>, TData> dataSelector)
        {
            DataTable table = new DataTable();
            var rowsName = ((NewExpression)rowSelector.Body).Members.Select(s => s).ToList();
            foreach (var row in rowsName)
            {
                var name = row.Name;
                table.Columns.Add(new DataColumn(name));
            }
            var columns = source.Select(columnSelector).Distinct();
            foreach (var column in columns)
                table.Columns.Add(new DataColumn(column.ToString()));
            var rows = source.GroupBy(rowSelector.Compile())
                             .Select(rowGroup => new
                             {
                                 Key = rowGroup.Key,
                                 Values = columns.GroupJoin(
                                     rowGroup,
                                     c => c,
                                     r => columnSelector(r),
                                     (c, columnGroup) => dataSelector(columnGroup))
                             });

            foreach (var row in rows)
            {
                var dataRow = table.NewRow();
                var items = row.Values.Cast<object>().ToList();
                string[] keyRow = row.Key.ToString().Split(',');
                int index = 0;
                foreach (var key in keyRow)
                {
                    string keyValue = key.Replace("}", "").Split('=')[1].Trim();
                    items.Insert(index, keyValue);
                    index++;
                }
                dataRow.ItemArray = items.ToArray();
                table.Rows.Add(dataRow);
            }
            return table;
        }
    }
}
