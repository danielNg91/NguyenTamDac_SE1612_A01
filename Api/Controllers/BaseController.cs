using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System.Linq.Expressions;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    private IMapper _mapper;
    
    protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
}
