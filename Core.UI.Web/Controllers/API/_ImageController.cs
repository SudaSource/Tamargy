using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Core.UI.Web.Controllers.API
{
    public class _ImageController : _Controller
    {
        public async Task<ActionResult> GetProfileImage(long id)
        {
            var dir = "";

            var path = Path.Combine(dir, id + ".jpg"); //validate the path for security or use other means to generate the path.
            byte[] cover;
            var obj = await _sdk.Worker._context.Users.FindAsync(id);
            if (obj != null)
            {
                cover = obj.Img;
                return (cover != null) ? (ActionResult)File(cover, "image/pneg") : File(dir, "image/png");
            }
            return File(dir, "image/png");
        }
    }
}