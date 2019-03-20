using Interview.Service;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Interview
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string filters = "amsterdam";
                IDictionary<string, int> propertiesByMekelaarInAmsterdam = FundaService.GetTopMekelaarWithFilters(filters, 10);

                PrintDictionaryOutput(filters, propertiesByMekelaarInAmsterdam);


                filters = "amsterdam/tuin";
                IDictionary<string, int> propertiesByMekelaarInAmsterdamWithTuin = FundaService.GetTopMekelaarWithFilters(filters, 10);

                PrintDictionaryOutput(filters, propertiesByMekelaarInAmsterdamWithTuin);
            }
            catch (Exception e)
            {
                Console.WriteLine("");
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void PrintDictionaryOutput(string filters, IDictionary<string, int> dictionary)
        {
            Console.WriteLine("");
            Console.WriteLine(string.Format("Printing resutls for: {0}", filters));
            Console.WriteLine("-------------");
            foreach (KeyValuePair<string, int> kvp in dictionary)
            {
                Console.WriteLine("{0}\t{1}", kvp.Value, kvp.Key);
            }
            Console.WriteLine("");
        }
    }
}
