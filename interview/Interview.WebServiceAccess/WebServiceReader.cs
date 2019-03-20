using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading;

namespace Interview.WebServiceAccess
{
    public static class WebServiceReader
    {
        public static void GetPropertiesByMekelaar(IDictionary<string, int> propertiesByMekelaar, string serviceUrl, int pageNumberParam, WebClient webClient)
        {
            int sleepTime = Int32.Parse(ConfigurationManager.AppSettings["sleepTime"]);

            string serviceResult = string.Empty;

            try
            {
                string parametrizedServiceUrl = serviceUrl.Replace("pageNumberParam", pageNumberParam.ToString());
                serviceResult = webClient.DownloadString(parametrizedServiceUrl);
            }
            catch(Exception ex)
            {
                throw new ApplicationException("There was an error fetching data from json webservice", ex);
            }

            JObject parsedResult = JObject.Parse(serviceResult);

            int aantalPaginas = Int32.Parse(parsedResult["Paging"]["AantalPaginas"].ToString());
            int huidigePagina = Int32.Parse(parsedResult["Paging"]["HuidigePagina"].ToString());

            foreach (JObject parsedPropertyObject in parsedResult["Objects"])
            {
                string makelaarNaam = parsedPropertyObject["MakelaarNaam"].ToString();

                if (!propertiesByMekelaar.ContainsKey(makelaarNaam))
                {
                    propertiesByMekelaar.Add(makelaarNaam, 0);
                }

                propertiesByMekelaar[makelaarNaam] = propertiesByMekelaar[makelaarNaam] + 1;
            }

            if (huidigePagina < aantalPaginas)
            {
                Thread.Sleep(sleepTime);
                pageNumberParam = pageNumberParam + 1;
                GetPropertiesByMekelaar(propertiesByMekelaar, serviceUrl, pageNumberParam, webClient);
            }
        }
    }
}
