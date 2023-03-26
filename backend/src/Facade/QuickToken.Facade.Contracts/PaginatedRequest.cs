using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace QuickToken.Facade.Contracts;

public class PaginatedRequest
{
    [FromQuery(Name = "count")]
    [Range(1,int.MaxValue)]
    public int Count { get; set; } = 100;

    [FromQuery(Name = "shift")]
    [Range(0 ,int.MaxValue)]
    public int Shift { get; set; } = 0;
}