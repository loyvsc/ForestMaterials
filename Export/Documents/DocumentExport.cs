using BuildMaterials.Extensions;
using BuildMaterials.Helpers;
using BuildMaterials.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Windows;
using TemplateEngine.Docx;

namespace BuildMaterials.Export.Documents
{
    public class DocumentExport
    {
        private readonly Window window;
        public DocumentExport(Window parent)
        {
            window = parent;
        }

        public void SaveReport(string path, Account ttn)
        {            
            try
            {
                File.WriteAllBytes(path, Properties.Resources.Account);
            }
            catch
            {                
                window.ShowDialogAsync("Во время экспорта произошла ошибка!", "Экспорт в файл");
                return;
            }

            var mainTable = new TableContent("ItemsTable");
            float finalCount = 0, finalPrice = 0, finalNdsSumm = 0, finalSumm = 0;

            foreach (var item in ttn.Contract!.Materials)
            {
                finalCount += (float)item.Material.Count;
                finalPrice += (float)item.Material.Count * (float)item.Material.Price;

                float nds = ((float)item.Material.Count * (float)item.Material.Price) * ((float)item.Material.NDS / 100);
                finalNdsSumm += nds;
                float summwithnds = ((float)item.Material.Count * (float) item.Material.Price) + finalNdsSumm;
                finalSumm += summwithnds;

                mainTable = mainTable.AddRow(
            new FieldContent("MaterialName", item.Material.Name),
            new FieldContent("MaterialCount", item.Material.Count.ToString()),
            new FieldContent("Price", item.Material.Count.ToString()),
            new FieldContent("NDS", item.Material.Price.ToString()),
            new FieldContent("NDSSumm", (item.Material.Count * item.Material.Price).ToString()),
            new FieldContent("NDSPrice", item.Material.NDS.ToString()));
            }

            IContentItem[] items =
            {                new FieldContent("Number", ttn.ID.ToString()),
                new FieldContent("Date", ttn.Date.Value.ToShortDateString()),
                new FieldContent("Seller", ttn.Contract.Seller.CompanyName),
                new FieldContent("SellerUNP", ttn.Contract.Seller.UNP),
                new FieldContent("SellerAdress", ttn.Contract.Seller.Adress),
                new FieldContent("SellerBank", $"{ttn.Contract.Seller.RascSchet}, {ttn.Contract.Seller.CBU}, {ttn.Contract.Seller.BIK}"),
                new FieldContent("Buyer", ttn.Contract.Buyer.CompanyName),
                new FieldContent("BuyerUNP", ttn.Contract.Buyer.UNP),
                new FieldContent("BuyerAdress", ttn.Buyer.Adress),
                new FieldContent("BuyerBank", $"{ttn.Contract.Seller.RascSchet}, {ttn.Contract.Seller.CBU}, {ttn.Contract.Seller.BIK}"),
                new FieldContent("BuyerAdress", ttn.Contract.Buyer.Adress),
                new FieldContent("Dogovor", ttn.Contract.ToString()),

                new FieldContent("FinalCount", finalCount.ToString()),
                new FieldContent("FilalPrice", finalPrice.ToString()),
                new FieldContent("FinalNDSSumm", finalNdsSumm.ToString()),
                new FieldContent("FinalNDSPrice", finalSumm.ToString()),
                new FieldContent("ItogoSumm", finalSumm.ToString()),
                new FieldContent("ItogoNDS", finalNdsSumm.ToString()),

                mainTable,
            };

            using (TemplateProcessor outputDocument = new TemplateProcessor(path).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(new Content(items)).SaveChanges();
                items = null!;
            }
            window.ShowDialogAsync("Экспорт в файл заверешён!", "Экспорт в файл");
        }
        public void SaveReport(string path, Contract contact)
        {
            try
            {
                File.WriteAllBytes(path, BuildMaterials.Properties.Resources.IndividualDogovor);
            }
            catch (Exception ex)
            {
                window.ShowDialogAsync("Во время экспорта произошла ошибка!" + ex.Message, "Экспорт в файл");
                return;
            }

            float summ = 0;
            string itemsDesc = string.Empty;
            contact.Materials.ForEach((item) =>
            {
                summ += (float)item.Count! * (float)item.Material!.Price;
                itemsDesc += $"{(itemsDesc != string.Empty ? ", " : "")}{item.Material.Name} {((item.Material.Shirina != string.Empty || item.Material.Dlina != string.Empty) ? $"({GetItemDescription(item.Material)})" : string.Empty)}";
            });

            string finalBuyerInfo = contact.Individual?.ID != 0 ?
                $"ФИО {contact.Individual!.FIO}\r\nВыдан {contact.Individual.Passport.IssueDate} {contact.Individual.Passport.IssuePunkt}\r\nИдентификационный номер {contact.Individual.Passport.Number}" :
                $"{contact.Buyer!.ShortCompamyName}\r\n{contact.Buyer.Adress}\r\nт/с {contact.Buyer.CurrentSchet}\r\nр/с {contact.Buyer.RascSchet}\r\nЦБУ {contact.Buyer.CBU}\r\nкод {contact.Buyer.BIK}, УНП {contact.Buyer.UNP}";

            IContentItem[] items =
            {
                new FieldContent("DayNumber", contact.Date?.Day.ToString()),
                new FieldContent("MonthName", GetRussianMonthName(contact.Date!.Value.Month)),
                new FieldContent("SellerFullName", contact.Seller.CompanyName),
                new FieldContent("BuyerType", contact.Buyer!.ID == 0 ? "физическое лицо" : "юридическое лицо"),
                new FieldContent("BuyerInfo", contact.Buyer.ID != 0 ? contact.Buyer.CompanyName : contact.Individual.FIO),
                new FieldContent("DogovorSumm", $"{summ} рублей"),
                new FieldContent("TovarsDescrption", itemsDesc),
                new FieldContent("LogisticsInfo", GetTextLogisticInfo(contact.LogisiticsType)),
                new FieldContent("FinalShortSellerName", contact.Seller.ShortCompamyName),
                new FieldContent("FinalShortSellerName", contact.Seller.ShortCompamyName),
                new FieldContent("FinalSellerAdress", contact.Seller.Adress),
                new FieldContent("SellerCurrentSchet", contact.Seller.CurrentSchet),
                new FieldContent("SellerRaschSchet", contact.Seller.RascSchet),
                new FieldContent("SellerCBU", "ЦБУ " + contact.Seller.CBU),
                new FieldContent("SellerBIK", contact.Seller.BIK),
                new FieldContent("SellerUNP", contact.Seller.UNP),
                new FieldContent("FinalBuyerInfo", finalBuyerInfo),
            };

            using (TemplateProcessor outputDocument = new TemplateProcessor(path).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(new Content(items)).SaveChanges();
                items = null!;
            }
            window.ShowDialogAsync("Экспорт в файл заверешён!", "Экспорт в файл");

            string GetRussianMonthName(int number)
            {
                switch (number)
                {
                    case 1: { return "Января"; }
                    case 2: { return "Февраль"; }
                    case 3: { return "Март"; }
                    case 4: { return "Апрель"; }
                    case 5: { return "Май"; }
                    case 6: { return "Июнь"; }
                    case 7: { return "Июль"; }
                    case 8: { return "Август"; }
                    case 9: { return "Сентябрь"; }
                    case 10: { return "Октябрь"; }
                    case 11: { return "Ноябрь"; }
                    case 12: { return "Декабрь"; }
                    default: { return string.Empty; }
                }
            }
            string GetTextLogisticInfo(string info)
            {
                switch (info.ToLower())
                {
                    case "франко-верхний лесосклад":
                        {
                            return "- на условиях франко-верхний лесосклад доставка, погрузка (разгрузка) Товара Покупателю осуществляется силами (средствами) Покупателя и за счет Покупателя.";
                        }
                    case "франко-промежуточный":
                        {
                            return "- на условиях франко-промежуточный лесосклад доставка, погрузка (разгрузка) Товара Покупателю осуществляется силами (средствами) Покупателя и за счет Покупателя.";
                        }
                    case "франко-склад":
                        {
                            return "- на условиях франко-склад организации-изготовителя (потребителя) доставка, погрузка (разгрузка) Товара Покупателю осуществляется силами (средствами) Продавца и за счет Продавца.";
                        }
                    default:
                        {
                            return string.Empty;
                        }
                }
            }
            string GetItemDescription(Material mat)
            {
                string harakt = string.Empty;
                if (mat.Dlina == string.Empty && mat.Shirina == string.Empty) return string.Empty;
                if (mat.Dlina != string.Empty)
                {
                    harakt += $"длина - " + mat.Dlina;
                }
                if (mat.Shirina != string.Empty)
                {
                    harakt += $"{(harakt != string.Empty ? ", " : string.Empty)}ширина - " + mat.Shirina;
                }
                return harakt;
            }
        }
        public void SaveReport(string path, TN contact)
        {
            try
            {
                File.WriteAllBytes(path, BuildMaterials.Properties.Resources.TN);
            }
            catch (Exception ex)
            {
                window.ShowDialogAsync("Во время экспорта произошла ошибка!\r\n" + ex.Message, "Экспорт в файл");
                return;
            }

            var mainTable = new TableContent("ItemsTable");
            float count = 0, basicPrice = 0, finalBasicSumm = 0, finalNdsSumm = 0, finalMaterialSumm = 0, finalSumm = 0;

            foreach (var item in contact.Contract!.Materials)
            {
                float thisCount = (float)item.Count!;
                count += thisCount;
                float thisPrice = (float)item.Material!.Price;
                basicPrice += thisPrice;
                float thisBasicSumm = thisCount * thisPrice;
                finalBasicSumm += thisBasicSumm;
                float thisNdsSumm = thisBasicSumm * (float)item.Material.NDS;
                finalNdsSumm += thisNdsSumm;
                finalMaterialSumm += thisBasicSumm + thisNdsSumm;
                finalSumm += finalMaterialSumm;

                mainTable = mainTable.AddRow(
            new FieldContent("MaterialName", item.Material.Name),
            new FieldContent("MaterialCountUnit", item.Material.CountUnits),
            new FieldContent("MaterialCount", thisCount.ToString()),
            new FieldContent("MaterialPrice", thisPrice.ToString()),
            new FieldContent("MaterialSumm", thisBasicSumm.ToString()),
            new FieldContent("MaterialNDS", item.Material.NDS == 0 ? "-" : item.Material.NDS.ToString()),
            new FieldContent("MaterialNDSSumm", item.Material.NDS == 0 ? "-" : (item.Material.NDS * (item.Material.Price * item.Count)).ToString()),
            new FieldContent("AllMaterialSumm", finalMaterialSumm.ToString()));
            }

            IContentItem[] items =
            {
                new FieldContent("SellerUNP", contact.Contract.Seller.UNP),
                new FieldContent("BuyerName", contact.Contract.Buyer.ID != 0 ? contact.Contract.Buyer.ShortCompamyName : contact.Contract.Individual.FIO),
                new FieldContent("Date", DateToString(DateTime.Now)),
                new FieldContent("SellerInfo", contact.Contract.Seller.ShortCompamyName),
                new FieldContent("SellerAdress", contact.Contract.Seller.Adress),
                new FieldContent("BuyerInfo", contact.Contract.Buyer.ID != 0 ? contact.Contract.Buyer.ShortCompamyName : contact.Contract.Individual.FIO),
                new FieldContent("BuyerAdress", contact.Contract.Buyer.ID != 0 ? contact.Contract.Buyer.Adress : ""),
                new FieldContent("DogovorNumber", contact.Contract.ToString()),
                new FieldContent("FinalCount", count.ToString()),
                new FieldContent("FinalBasicSumm", finalBasicSumm.ToString()),
                new FieldContent("FinalNDSSumm", finalNdsSumm == 0 ? "-" : finalNdsSumm.ToString()),
                new FieldContent("FinalMaterialSumm", finalSumm.ToString()),
                new FieldContent("LogisticsType", contact.Contract.LogisiticsType),
                new FieldContent("VsegoNDS", Сумма.Пропись(finalNdsSumm, Валюта.Рубли)),
                new FieldContent("VsegoSumm", Сумма.Пропись(finalSumm, Валюта.Рубли)),
                new FieldContent("ResponsibleEmployee", contact.ResponseEmployee!.FIO),
                new FieldContent("SdalEmployee", contact.SdalEmployee!.FIO),
                mainTable,
            };

            using (TemplateProcessor outputDocument = new TemplateProcessor(path).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(new Content(items)).SaveChanges();                
            }
            items = null!;
            window.ShowDialogAsync("Экспорт в файл заверешён!", "Экспорт в файл");

        }
        public void SaveReport(string path, TTN ttn)
        {
            try
            {
                File.WriteAllBytes(path, Properties.Resources.TTN);
            }
            catch (Exception ex)
            {
                window.ShowDialogAsync("Во время экспорта произошла ошибка!\r\n" + ex.Message, "Экспорт в файл");
                return;
            }

            var mainTable = new TableContent("ItemsTable");
            float count = 0, finalBasicSumm = 0, finalNdsSumm = 0, finalSumm = 0;

            foreach (var item in ttn.Contract!.Materials)
            {
                count += (float)item.Material.Count;
                finalBasicSumm += (float)item.Material.Count * (float)item.Material.Price;

                float nds = ((float)item.Material.Count * (float)item.Material.Price) * ((float)item.Material.NDS / 100);
                finalNdsSumm += nds;
                float summwithnds = ((float)item.Material.Count * (float)item.Material.Price) + finalNdsSumm;
                finalSumm += summwithnds;

                mainTable = mainTable.AddRow(
            new FieldContent("MaterialName", item.Material.Name),
            new FieldContent("CountUnits", item.Material.CountUnits),
            new FieldContent("Count", item.Material.Count.ToString()),
            new FieldContent("BasePrice", item.Material.Price.ToString()),
            new FieldContent("Price", (item.Material.Count * item.Material.Price).ToString()),
            new FieldContent("NDSProcent", item.Material.NDS.ToString()),
            new FieldContent("NDSSumm", item.Material.NDS == 0 ? "-" : nds.ToString()),
            new FieldContent("FinalPrice", summwithnds.ToString()));
            }

            IContentItem[] items =
            {
                new FieldContent("SellerUNP1", ttn.Contract.Seller.UNP),
                new FieldContent("SellerUNP2", ttn.Contract.Seller.UNP),
                new FieldContent("BuyerUNP", ttn.Contract.Buyer.UNP),
                new FieldContent("Date", DateToString(ttn.Date.Value)),
                new FieldContent("AutomobileNameNumber", ttn.Automobile.ToString()),
                new FieldContent("DriverFIO", ttn.Driver),
                new FieldContent("Seller", ttn.Contract.Seller.CompanyName),
                new FieldContent("Buyer", ttn.Contract.Buyer.CompanyName),
                new FieldContent("Dogovor", ttn.Contract.ToString()),
                new FieldContent("PunktPogruzki", ttn.AdresPogruzki),
                new FieldContent("PunktRazgruzki", ttn.AdresRazgruzki),
                new FieldContent("Summ", finalBasicSumm.ToString()),
                new FieldContent("FinalNDSSumm", finalNdsSumm.ToString()),
                new FieldContent("FinalNDSPrice", finalSumm.ToString()),

                new FieldContent("NDSSumm", Сумма.Пропись(finalNdsSumm, Валюта.Рубли)),
                new FieldContent("Summ", Сумма.Пропись(finalSumm, Валюта.Рубли)),
                new FieldContent("ResponseEmployee", ttn.ResponseEmployee.FIO),
                new FieldContent("SdalEmployee", ttn.SdalEmployee.FIO),
                new FieldContent("AcceptDriver", ttn.Driver),
                new FieldContent("CompanyPoruzchik", ttn.Contract.Seller.CompanyName),
                new FieldContent("MethodPogruzka", ttn.PogruzkaMethod),
                mainTable,
            };

            using (TemplateProcessor outputDocument = new TemplateProcessor(path).SetRemoveContentControls(true))
            {
                outputDocument.FillContent(new Content(items)).SaveChanges();
                items = null!;
            }
            window.ShowDialogAsync("Экспорт в файл заверешён!", "Экспорт в файл");
        }

        private string DateToString(DateTime date)
        {
            string value = date.Day + " ";

            switch (date.Month)
            {
                case 1:
                    {
                        value += "января";
                        break;
                    }
                case 2:
                    {
                        value += "февраля";
                        break;
                    }
                case 3:
                    {
                        value += "марта";
                        break;
                    }
                case 4:
                    {
                        value += "апреля";
                        break;
                    }
                case 5:
                    {
                        value += "мая";
                        break;
                    }
                case 6:
                    {
                        value += "июня";
                        break;
                    }
                case 7:
                    {
                        value += "июля";
                        break;
                    }
                case 8:
                    {
                        value += "августа";
                        break;
                    }
                case 9:
                    {
                        value += "сентября";
                        break;
                    }
                case 10:
                    {
                        value += "октября";
                        break;
                    }
                case 11:
                    {
                        value += "ноября";
                        break;
                    }
                case 12:
                    {
                        value += "декабря";
                        break;
                    }
            }
            value += $" {date.Year} г.";
            return value;
        }
    }
}
