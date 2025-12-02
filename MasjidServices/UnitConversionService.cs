using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GravyFoodsApi.MasjidServices
{

    public class UnitConversionService : IUnitConversionRepository
    {
        private readonly MasjidDBContext _context;
        public UnitConversionService(MasjidDBContext context)
        {
            _context = context;
            
        }

              


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





        //public async Task<double> UnitConvert(UnitConversionDto dto)
        //{

        //    int from = Array.IndexOf(units, fromUnit);
        //    int to = Array.IndexOf(units, toUnit);

        //    // 1. Convert from any unit → base unit (smallest)
        //    double baseValue = await ToBase(value, from, segments);

        //    // 2. Convert from base unit → desired unit

        //    return await FromBase(baseValue, to, segments);

        //}




    }


}
