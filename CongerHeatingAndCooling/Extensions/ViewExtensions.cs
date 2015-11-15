using Newtonsoft.Json;

namespace CongerHeatingAndCooling.Extensions
{
    public static class ViewExtensions
    {
        public static string ToJson(this object value, bool ignoreSelfReferencing = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (ignoreSelfReferencing)
            {
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }
            return JsonConvert.SerializeObject(value, settings);
        }
    }
}