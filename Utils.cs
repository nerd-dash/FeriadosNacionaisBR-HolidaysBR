using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static LookUp.GoogleMaps;

namespace LookUp.Business
{
    public static class Utils
    {

        public static string GetAddress(double lat, double lng)
        {
            try
            {
                //Pass request to google api with orgin and destination details
                HttpWebRequest request =
                    (HttpWebRequest)WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/xml?latlng="
                    + lat.ToString().Replace(",", ".") + "," + lng.ToString().Replace(",", ".") + "&language=pt-br");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    if (!string.IsNullOrEmpty(result))
                    {
                        string xml = result;
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(xml);
                        //var node = doc.SelectNodes("GeocodeResponse/result/formatted_address");
                        //var v = node[0].InnerText;
                        var v = doc.ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText;
                        return v;
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static async Task<decimal> GetKMCalculated(double initialLat, double initialLng, double finalLat, double finalLng)
        {

            if (initialLat == 0 || initialLng == 0 || finalLat == 0 || finalLng == 0)
                return 0;

            try
            {
                DistanceResult t = await GoogleMaps.GetDistanceAsync(initialLat.ToString().Replace(",", ".") + "," + initialLng.ToString().Replace(",", "."), finalLat.ToString().Replace(",", ".") + "," + finalLng.ToString().Replace(",", "."));

                var distance = t.rows.Select(x => x.elements).FirstOrDefault().Select(x => x.distance).Select(x => x.value).FirstOrDefault();

                return distance > 0 ? distance / 1000 : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static bool StringIsNumber(string number)
        {
            return int.TryParse(number, out int n);
        }

        public static IList<DateTime> GetHolidaysByCurrentYear(int? yearParameter = null)
        {
            var holidayList = new List<DateTime>();
            var year = DateTime.Now.Year;

            if (yearParameter != null)
                year = yearParameter.Value;

            holidayList.Add(new DateTime(year, 1, 1)); //Ano novo 
            holidayList.Add(new DateTime(year, 4, 21));  //Tiradentes
            holidayList.Add(new DateTime(year, 5, 1)); //Dia do trabalho
            holidayList.Add(new DateTime(year, 7, 9)); //Dia da Independência do Brasil
            holidayList.Add(new DateTime(year, 10, 12));  //Nossa Senhora Aparecida
            holidayList.Add(new DateTime(year, 11, 2)); //Finados
            holidayList.Add(new DateTime(year, 11, 15)); //Proclamação da República
            holidayList.Add(new DateTime(year, 12, 25)); //Natal

            #region FeriadosMóveis

            int x, y;
            int a, b, c, d, e;
            int day, month;

            if (year >= 1900 & year <= 2099)
            {
                x = 24;
                y = 5;
            }
            else
                if (year >= 2100 & year <= 2199)
            {
                x = 24;
                y = 6;
            }
            else
                    if (year >= 2200 & year <= 2299)
            {
                x = 25;
                y = 7;
            }
            else
            {
                x = 24;
                y = 5;
            }

            a = year % 19;
            b = year % 4;
            c = year % 7;
            d = (19 * a + x) % 30;
            e = (2 * b + 4 * c + 6 * d + y) % 7;

            if ((d + e) > 9)
            {
                day = (d + e - 9);
                month = 4;
            }

            else
            {
                day = (d + e + 22);
                month = 3;
            }

            var pascoa = new DateTime(year, month, day);
            var sextaSanta = pascoa.AddDays(-2);
            var carnaval = pascoa.AddDays(-47);
            var corpusChristi = pascoa.AddDays(60);

            holidayList.Add(pascoa);
            holidayList.Add(sextaSanta);
            holidayList.Add(carnaval);
            holidayList.Add(corpusChristi);

            #endregion

            return holidayList;
        }
    }
}
