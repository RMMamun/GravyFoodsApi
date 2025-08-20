namespace GravyFoodsApi.MasjidServices
{
    public class GeoLocationService
    {

        public string CalculateDistanceStr(double FromLat, double FromLong, double ToLati, double ToLong)
        {
            try
            {


                double distance = CalculateDistance(FromLat, FromLong, ToLati, ToLong);
                if (distance < 1)
                {
                    distance = Math.Round(distance * 1000, 2);

                    return distance.ToString() + " meters";
                }
                else
                {
                    distance = Math.Round(distance, 2);

                    return distance.ToString() + " kilometers";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public double CalculateDistance(double FromLat, double FromLong, double ToLati, double ToLong)
        {
            const double R = 6371; // Radius of the Earth in kilometers

            // Convert latitude and longitude from degrees to radians
            double lat1Rad = ToRadians(FromLat);
            double lon1Rad = ToRadians(FromLong);
            double lat2Rad = ToRadians(ToLati);
            double lon2Rad = ToRadians(ToLong);

            // Haversine formula
            double dLat = lat2Rad - lat1Rad;
            double dLon = lon2Rad - lon1Rad;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;

            return distance;
        }

        private static double ToRadians(double angle)
        {
            return angle * Math.PI / 180;
        }
    }
}
