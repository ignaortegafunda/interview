
using Interview.WebServiceAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace Interview.Service
{
    public static class FundaService
    {
        public static IDictionary<string, int> GetTopMekelaarWithFilters(string filtersParam, int top)
        {
            if (string.IsNullOrEmpty(filtersParam))
            {
                throw new ArgumentException("Parameter 'filtersParam' must not be empty");
            }

            string filtersParamRegex = @"^[a-zA-Z0-9]+[/[a-zA-Z0-9][a-zA-Z0-9]+]*";

            Match match = Regex.Match(filtersParam, filtersParamRegex, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                throw new ArgumentException("Parameter 'filtersParam' is in the incorrect format");
            }

            if(top < 0)
            {
                throw new ArgumentException("Parameter 'top' must be a possitive integer");
            }

            IDictionary<string, int> propertiesByMekelaar = new Dictionary<string, int>();

            string serviceJsonUrl = ConfigurationManager.AppSettings["serviceJsonUrl"];
            string keyParam = ConfigurationManager.AppSettings["keyParam"];

            string serviceUrl = serviceJsonUrl.Replace("keyParam", keyParam).Replace("filtersParam", filtersParam);

            WebServiceReader.GetPropertiesByMekelaar(propertiesByMekelaar, serviceUrl, 1, new System.Net.WebClient());

            return propertiesByMekelaar.OrderByDescending(e => e.Value).Take(top).ToDictionary(e => e.Key, e => e.Value);
        }
    }
}
