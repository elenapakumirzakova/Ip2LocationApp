namespace Ip2LocationApp.WebApi.Models;
public class ResponseModel
{
    public string? Ip { get; set; }
    public string? Type { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public string? Zip { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public ResponseModel(
        string? ip, 
        string? type, 
        string? country, 
        string? region, 
        string? city, 
        string? zip, 
        double? latitude,
        double? longitude)
    {
        Ip = ip;
        Type = type;
        Country = country;
        Region = region;
        City = city;
        Zip = zip;
        Latitude = latitude;
        Longitude = longitude;
    }
}

