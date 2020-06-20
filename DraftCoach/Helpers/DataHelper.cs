using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace DraftCoach.Helpers
{
    public static class DataHelper
    {
        public static IDictionary<string, DataTable> RetrieveDataTables(string pathToFolder)
        {
            var fileNames = Directory.EnumerateFiles(pathToFolder, "*.csv");
            var dataTables = new Dictionary<string, DataTable>();

            foreach (var fileName in fileNames)
            {
                dataTables.Add(Path.GetFileNameWithoutExtension(fileName), ConvertCsvToDataTable(fileName));
            }

            return dataTables;
        }

        private static DataTable ConvertCsvToDataTable(string pathToData)
        {
            StreamReader sr = new StreamReader(pathToData);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
