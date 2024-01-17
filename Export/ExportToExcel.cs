using BuildMaterials.Extensions;
using Castle.Core.Internal;
using OfficeOpenXml;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows;

namespace BuildMaterials.Export
{
    public class ExportToExcel
    {
        public static void ExportFromDataGrid<T>(string filename, FilterDataGrid.FilterDataGrid datagrid, Window parent)
        {
            ExportFromArray<T>(filename, datagrid.CollectionViewSource.Cast<T>().ToArray(),parent);
        }

        public static void ExportFromArray<T>(string filename, T[] items, Window parent)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (ExcelPackage document = new ExcelPackage())
            {
                var mainsheet = document.Workbook.Worksheets.Add("Экспортированная информация");

                //получаем заголовки и заполняем таблицу
                string[] header = GetPropertyNames<T>(items);
                for (int ind = 1; ind < header.Length + 1; ind++)
                {
                    mainsheet.Cells[1, ind].Value = header[ind - 1];
                }

                //получаем количество значений и начинаем заполнять таблицу                
                for (int i = 2; i < items.Length + 2; i++) //строки
                {
                    //получаем значения и заполняем
                    string[] values = GetPropertyValues(items[i - 2]);
                    for (int j = 0; j < values.Length; j++)
                    {
                        mainsheet.Cells[i, j + 1].Value = values[j];
                    }
                }

                var range = mainsheet.Cells[1, 1, items.Length + 2, header.Length + 1];
                //стилизуем
                range.AutoFitColumns();
                range.Style.Numberformat.Format = "yyyy";
                range.Style.Numberformat.Format = "### ### ### ##0";
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                var headerRange = mainsheet.Cells[1, 1, 1, header.Length];
                headerRange.AutoFilter = true;

                Save(filename, document.GetAsByteArray(), parent); //сохранение файла
            }
        }

        private static void Save(string path, byte[] bytes, Window parent)
        {
            try
            {
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception ex)
            {
                parent.ShowDialogAsync("При сохранении файла произошла ошибка: " + ex.Message, "Экспорт в Excel");
            }
        }

        private static string[] GetPropertyNames<T>(object obj)
        {
            List<string> names = new List<string>();
            if (obj == null) return names.ToArray();

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            names = new List<string>();

            for (int i = 0; i < properties.Length; i++)
            {
                var attr = properties[i].GetAttribute<ExportColumnNameAttribute>();
                var ignoreAttr = properties[i].GetAttribute<IgnorePropertyAttribute>();
                if (ignoreAttr != null) continue;
                names.Add(attr != null ? attr.Name : properties[i].Name);
            }
            return names.ToArray();
        }

        private static string[] GetPropertyValues(object obj)
        {
            List<string> values = new List<string>();
            if (obj == null) return values.ToArray();

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            values = new List<string>();

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].PropertyType.Name.Contains("List"))
                {
                    values.Add(((IList)properties[i].GetValue(obj)).AsString());
                    continue;
                }

                var ignoreAttr = properties[i].GetAttribute<IgnorePropertyAttribute>();
                if (ignoreAttr != null) continue;

                var boolAttr = properties[i].GetAttribute<BooleanValueAttribute>();
                if (boolAttr != null)
                {
                    bool blvalue = (bool)properties[i].GetValue(obj);
                    values.Add(boolAttr.GetValue(blvalue));
                    continue;
                }

                string value = "";

                if (properties[i].PropertyType == typeof(DateTime))
                {
                    value = ((DateTime)properties[i].GetValue(obj)).ToShortDateString();
                }
                else
                {
                    value = properties[i].GetValue(obj).ToString();
                }

                values.Add(value);
            }
            return values.ToArray();
        }
    }

    public static class Extensions
    {
        public static string AsString(this IList list)
        {
            string val = "";
            foreach (var item in list)
            {
                val += item.ToString();
            }
            return val;
        }
    }
}
