namespace Ip2Location.Domain.Models;
public class IpLocation
{
    public int Id { get; set; }
    public string RequestId { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreateTime { get; set; }
    public string? Ip { get; set; }

    //Potentially could be saved for historical data
    public string? Type { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public string? Zip { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
