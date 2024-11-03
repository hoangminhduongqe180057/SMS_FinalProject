using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;

namespace StudentsManagement.Client.Services
{
    public class ExportService
    {
        // Export to CSV
        public byte[] ExportToCsv<T>(List<T> data)
        {
            var csvBuilder = new StringBuilder();
            var properties = typeof(T).GetProperties();

            // Add headers (property names)
            csvBuilder.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Add data rows
            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item)?.ToString() ?? string.Empty).ToArray();
                csvBuilder.AppendLine(string.Join(",", values));
            }

            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }

        // Export to Excel using EPPlus
        public byte[] ExportToExcel<T>(List<T> data, string sheetName)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(sheetName);
                var properties = typeof(T).GetProperties();

                // Add headers
                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[1, col + 1].Value = properties[col].Name;
                }

                // Add data rows
                for (int row = 0; row < data.Count; row++)
                {
                    for (int col = 0; col < properties.Length; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = properties[col].GetValue(data[row])?.ToString() ?? string.Empty;
                    }
                }

                return package.GetAsByteArray();
            }
        }
    }
}
