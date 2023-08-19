namespace Ip2Location.Business.Models;
public class ErrorResponse
{
    public Error? Error { get; set; }
}

public class Error
{
    public string? Code { get; set; }
    public string? Type { get; set; }
    public string? Info { get; set; }
}
