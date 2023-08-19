namespace Ip2LocationApp.WebApi.Models;
public class ErrorResponseModel
{
    public string? Type { get; set; }
    public string? Info { get; set; }

    public ErrorResponseModel(string? type, string? info)
    {
        Type = type;
        Info = info;
    }
}

