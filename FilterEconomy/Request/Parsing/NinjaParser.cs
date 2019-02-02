﻿using FilterEconomy.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterEconomy.Request.Parsing
{
    public class NinjaParser
    {
        public static IEnumerable<NinjaItem> ParseNinjaString(string input)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(input, new JsonSerializerSettings() { CheckAdditionalContent = true });
            foreach (JObject job in jsonObj.lines)
            {
                NinjaItem nItem = new NinjaItem();
                job["explicitModifiers"] = job["explicitModifiers"].ToString();
                JsonConvert.PopulateObject(job.ToString(), nItem);
                yield return nItem;
            }
        }

        public static Dictionary<string, List<NinjaItem>> CreateOverviewDictionary(IEnumerable<NinjaItem> input)
        {
            var result = new Dictionary<string, List<NinjaItem>>();

            foreach (var item in input)
            {
                if (item.Links >= 5)
                {
                    continue;
                }

                var key = item.Name ?? item.BaseType;

                if (!result.ContainsKey(key))
                {
                    result.Add(key, new List<NinjaItem>());
                }

                result[key].Add(item);
            }

            return result;
        }

        // public static TOverviewType CreateOverview<TOverviewType>( IEnumerable<NinjaItem> input ) where TOverviewType : class
    }
}
