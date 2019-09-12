using System;
using System.IO;
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
            var platform = Environment.OSVersion.Platform;
            string path = " **<empty>** ";
            if (platform.ToString().Contains("Win"))
            {
                path = Path.GetFullPath(Directory.GetCurrentDirectory() + @"\Images\csharpguitar-aci.png");
                var image = System.IO.File.OpenRead(path);
                return File(image, "image/png");
            }
            else
            {
                path = Path.GetFullPath(Directory.GetCurrentDirectory() + @"/Images/csharpguitar-aci.png");
		        var image = System.IO.File.OpenRead(path);
                return File(image, "image/png");
            }
        }
    }
}