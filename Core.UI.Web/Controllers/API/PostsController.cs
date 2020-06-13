using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Data.Models;
using Core.Data.Types;

namespace Core.UI.Web.Controllers.API
{
    public class PostsController : _Controller
    {
        // GET: Posts
        public async Task<ActionResult> Posts(string search)
        {
            return PartialView(string.IsNullOrEmpty(search) ? await _sdk.PostsManager.Posts() : await _sdk.PostsManager.Posts(search));
        }

        public ActionResult NewPost()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> Save(string nameAr, string nameEng, PostType type, string post)
        {
            bool state = await _sdk.PostsManager.AddPost(new Post
            {
                CreateDate = DateTime.Now,
                CreatedBy = GetUserId(),
                MedicineNameAr = nameAr,
                MedicineNameEng = nameEng,
                State = State.Active,
                Text = post, PostType = type
            });
            return Json(state, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Like(long id)
        {
            bool state = await _sdk.PostsManager.Like(postId:id,userId: GetUserId());
            return Json(state, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetPosts(string search)
        {
            return Json(string.IsNullOrEmpty(search) ? await _sdk.PostsManager.PostsList() : await _sdk.PostsManager.PostsList(search), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetPost(long id)
        {
            var userId = GetUserId();
            var post = await _sdk.PostsManager.Post(id,userId);
            return PartialView(post);
        }

        [HttpPost]
        public async Task<ActionResult> GetComments(long id)
        {
            var comments = await _sdk.PostsManager.Comments(id);
            return PartialView(comments);
        }

        [HttpPost]
        public async Task<ActionResult> Comment(long id, string text)
        {
            bool state = await _sdk.PostsManager.Comment(postId: id, userId: GetUserId(),comment:text);
            return Json(state, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteComment(long id)
        {
            bool state = await _sdk.PostsManager.DeleteComment(id);
            return Json(state, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sponsors()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> DeletePost(long id)
        {
            bool state = await _sdk.PostsManager.DeletePost(id);
            return Json(state, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> TimeLine()
        {
            return PartialView(await _sdk.PostsManager.TimeLine(GetUserId())); 
        }
    }
}