using Core.EF.DataCtx;
using Core.Lib.Core;
using Core.Lib.SDK.Managers;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core.Lib.SDK
{
    public class SDK
    {
        public Worker Worker { get; set; }
        public UsersManager UsersManager { get; set; }
        public PostsManager PostsManager { get; set; }

        public SDK()
        {
            Worker = new Worker(context: DataCtx.Create());
            UsersManager = new UsersManager(Worker);
            PostsManager = new PostsManager(Worker);

        }
    }
}