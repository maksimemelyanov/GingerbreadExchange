using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

namespace GingerbreadsExchange.Models
{
    public class Currency
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public decimal Value { get; protected set; }

        public Currency(string name, decimal value)
        {
            Name = name;
            Value = value;
        }

        public Currency()
        {

        }

        public void ChangeValue(decimal value)
        {
            Value = value;
        }
    }

    public class CBReader
    {
        public class ValCurs
        {
            [XmlElementAttribute("Valute")]
            public ValCursValute[] ValuteList;
        }

        public class ValCursValute
        {
            [XmlElementAttribute("CharCode")]
            public string ValuteName;

            [XmlElementAttribute("Value")]
            public string ExchangeRate;
        }

        public List<Currency> GetExchangeRates()
        {
            List<Currency> result = new List<Currency>();
            XmlSerializer xs = new XmlSerializer(typeof(ValCurs));
            XmlReader xr = new XmlTextReader("http://www.cbr.ru/scripts/XML_daily.asp");
            foreach (ValCursValute valute in ((ValCurs)xs.Deserialize(xr)).ValuteList)
            {
                result.Add(new Currency(name: valute.ValuteName, value: Convert.ToDecimal(valute.ExchangeRate)));
            }
            return result;
        }
    }

}