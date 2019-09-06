using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace csharpguitar_api.Controllers
{
    [Route("")]
    [ApiController]
    public class CsharpGuitarController : ControllerBase
    {
        private IHostingEnvironment _env;
        public CsharpGuitarController(IHostingEnvironment env)
        {
            _env = env;
        }
        [Route("")]
        [HttpGet]
        public ContentResult Get()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = 200,
                Content = "<html><body>Click <a href=api/CsharpGuitar>here</a></body></html>"
            };
        }
        [Route("api/[controller]")]
        [HttpGet("{id}")]
        public ActionResult<string> GetImage()
        {
            var image = System.IO.File.OpenRead($"{_env.ContentRootPath}\\Images\\csharpguitar-aci.png");
            return File(image, "image/png");
        }
    }
}
