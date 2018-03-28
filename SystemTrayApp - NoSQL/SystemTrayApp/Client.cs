using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace SystemTrayApp
{
    public class Client
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="baseLink">Base URL</param>
        /// <param name="path">Sub path of URL</param>
        /// <param name="handler">handler used only for testing purposes</param>
        /// <returns></returns>
        public string GetClient(string baseLink, string path)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(baseLink);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

                    HttpResponseMessage response = client.GetAsync(path).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return "Success";
                    }
                    else
                    {
                        return "Error";
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }
    }
}
