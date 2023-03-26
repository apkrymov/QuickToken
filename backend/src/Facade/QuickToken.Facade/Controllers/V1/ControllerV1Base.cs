using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuickToken.Facade.Controllers.V1;

[ApiController]
[Authorize]
public class ControllerV1Base : ControllerBase
{
    protected const string BaseRoute = "/api/v1/";
}