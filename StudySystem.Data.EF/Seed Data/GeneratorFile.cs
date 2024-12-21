using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using StudySystem.Infrastructure.CommonConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Seed_Data
{
    public class GeneratorFile
    {
        /// <summary>
        /// Read Csv file using Csv Helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TClassMap"></typeparam>
        /// <param name="fileName"></param>
        /// <returns>List <typeparamref name="T"/></returns>
        /// <exception cref="Exception"></exception>
        public List<T> CsvDataGenerator<T, TClassMap>(string fileName) where TClassMap : ClassMap<T>
        {
            var fileStream = new FileStream($"{CommonConstant.PathFolderCsv}{fileName}{CommonConstant.TypeFileCsv}", FileMode.Open, FileAccess.Read);
            List<T> data = new List<T>();
            try
            {
                using(var stream = new StreamReader(fileStream, Encoding.UTF8))
                {
                    using (var csv = new CsvReader(stream, System.Globalization.CultureInfo.InvariantCulture))
                    {
                        var flag = true;
                        csv.Context.RegisterClassMap<TClassMap>();
                        while(csv.Read())
                        {
                            if (flag)
                            {
                                csv.ReadHeader();
                                flag = false;
                                continue;
                            }
                            data.Add(csv.GetRecord<T>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                fileStream.Close();
            }
            return data;
        }
    }
}
