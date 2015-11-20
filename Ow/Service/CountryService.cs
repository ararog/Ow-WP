using Newtonsoft.Json;
using Ow.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Ow.Service
{
    public class CountryService 
    {
        private List<Country> countries;

        public CountryService()
        {
            if (countries == null)
            {
                StreamReader reader = File.OpenText("Assets/Data/countries.json");

                string json = reader.ReadToEnd();

                countries = JsonConvert
                    .DeserializeObject<List<Country>>(json);
            }
        }

        public List<Country> FindAllCountries()
        {
            return countries;
        }

        public List<Country> FindByName(string name)
        {
            List<Country> countriesFound = countries
                .Where(o => o.Name.ToLower().Contains(name.ToLower())).ToList();

            return countriesFound;
        }

        public Country FindByCode(string code)
        {
            Country country = countries.Where(o => o.Code == code)
                .SingleOrDefault();

            return country;
        }

        public string FindTranslationByCode(string code)
        {
            RegionInfo regionInfo = new RegionInfo(code);

            return regionInfo.DisplayName;
        }
    }
}
