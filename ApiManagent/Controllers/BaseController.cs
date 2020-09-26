using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApiManagent.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiManagent.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        
    }
}
