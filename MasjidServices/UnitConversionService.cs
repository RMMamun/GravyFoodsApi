using GogetPOS.Shared.Models;
using GogetPOS.Shared.Models.DTOs;
using GravyFoodsApi.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace GravyFoodsApi.MasjidServices
{

    public class UnitConversionService
    {
        private readonly HttpClient _http;
        public UnitConversionService(HttpClient http)
        {
            _http = http;
        }

        /// <summary>Given rows (top->bottom) compute cumulative factors (top-based) e.g. 1,10,60,600</summary>
        //public List<long> CalculateCumulative(IEnumerable<dynamic> rows)
        //{
        //    var list = new List<long>();
        //    long cumulative = 1;
        //    bool first = true;
        //    foreach (var r in rows)
        //    {
        //        if (first)
        //        {
        //            cumulative = 1;
        //            list.Add(cumulative);
        //            first = false;
        //            continue;
        //        }

        //        // r.Value is immediate (how many of this unit in 1 upper unit)
        //        long immediate = 1;
        //        try { immediate = Convert.ToInt64(r.Value); } catch { immediate = 1; }
        //        checked { cumulative = cumulative * Math.Max(1, immediate); }
        //        list.Add(cumulative);
        //    }
        //    return list;
        //}

        /// <summary>Convert value from one unit to another using cumulative factors and names lists</summary>
        //public decimal Convert(List<string> names, List<long> cumulativeFactors, decimal value, string fromUnit, string toUnit)
        //{
        //    var dict = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        //    for (int i = 0; i < names.Count && i < cumulativeFactors.Count; i++) dict[names[i]] = cumulativeFactors[i];

        //    if (!dict.ContainsKey(fromUnit) || !dict.ContainsKey(toUnit)) throw new ArgumentException("Unknown unit names");

        //    decimal fromFactor = dict[fromUnit];
        //    decimal toFactor = dict[toUnit];

        //    // Convert: value (from) -> base -> to
        //    // valueInBase = value * fromFactor
        //    // result = valueInBase / toFactor
        //    var valueInBase = value * fromFactor;
        //    return valueInBase / toFactor;
        //}

        /// <summary>Convert any quantity into the smallest unit (last)</summary>
        //public decimal ToSmallest(List<long> cumulativeFactors, decimal qty, string fromUnit, List<string> names)
        //{
        //    var dict = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        //    for (int i = 0; i < names.Count && i < cumulativeFactors.Count; i++) dict[names[i]] = cumulativeFactors[i];
        //    if (!dict.ContainsKey(fromUnit)) throw new ArgumentException("Unknown unit");
        //    return qty * dict[fromUnit];
        //}



        


        public async Task<double> ToBase(double value, int fromIndex, double[] segments)
        {
            double result = value;

            // multiply downward
            for (int i = fromIndex + 1; i < segments.Length; i++)
                result *= segments[i];

            return result;
        }



        public async Task<double> FromBase(double baseValue, int toIndex, double[] segments)
        {
            double result = baseValue;

            // divide upward
            for (int i = toIndex + 1; i < segments.Length; i++)
                result /= segments[i];

            return result;
        }



        public async Task<double> Convert(double value, string fromUnit, string toUnit, string[] units, double[] segments)
        {
            int from = Array.IndexOf(units, fromUnit);
            int to = Array.IndexOf(units, toUnit);

            // 1. Convert from any unit → base unit (smallest)
            double baseValue = await ToBase(value, from, segments);

            // 2. Convert from base unit → desired unit
            
            return await FromBase(baseValue, to, segments);

        }

        public async Task<double> UnitConvert(UnitConversionDto dto)
        {

            int from = Array.IndexOf(units, fromUnit);
            int to = Array.IndexOf(units, toUnit);

            // 1. Convert from any unit → base unit (smallest)
            double baseValue = await ToBase(value, from, segments);

            // 2. Convert from base unit → desired unit

            return await FromBase(baseValue, to, segments);

        }




    }


}
