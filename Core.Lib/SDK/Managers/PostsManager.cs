using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Core.Data.Models;
using Core.Data.Types;
using Core.Lib.Core;
using Core.Lib.SDK.ViewModels;

namespace Core.Lib.SDK.Managers
{
    public class PostsManager
    {
        private Worker _manager;
        public PostsManager(Worker _worker)
        {
            _manager = _worker;
        }

        public async Task<List<PostVM>> Posts()
        {
            List<PostVM> postVms = new List<PostVM>();
            try
            {
                var posts = await _manager._context.Posts.ToListAsync();
                foreach (var cc in posts)
                {
                    var p = await _manager._context.Posts.FindAsync(cc.Id);
                    var comments = await _manager._context.Comments.Where(x => x.Post == p.Id).ToListAsync();
                    var likes = _manager._context.Likes.Count(x => x.Post == p.Id);
                    var createdBy = await _manager._context.Users.FindAsync(p.CreatedBy);
                    postVms.Add(new PostVM
                    {
                        Id = p.Id,
                        Comments = comments,
                        CreateDate = p.CreateDate,
                        CreatedBy = createdBy,
                        MedicineNameAr = p.MedicineNameAr,
                        MedicineNameEng = p.MedicineNameEng,
                        State = p.State,
                        Text = p.Text,
                        Likes = likes
                    });
                }

            }
            catch (Exception e)
            {

            }

            return postVms;

        }

        public async Task<List<PostVM>> Posts(string search)
        {
            List<PostVM> postVms = new List<PostVM>();
            try
            {
                var posts = await _manager._context.Posts
                    .Where(x =>
                        x.MedicineNameAr.Contains(search) ||
                        x.MedicineNameEng.Contains(search) ||
                        x.Text.Contains(search)
                    ).ToListAsync();
                foreach (var cc in posts)
                {
                    var p = await _manager._context.Posts.FindAsync(cc.Id);
                    var comments = await _manager._context.Comments.Where(x => x.Post == p.Id).ToListAsync();
                    var likes = _manager._context.Likes.Count(x => x.Post == p.Id);
                    var createdBy = await _manager._context.Users.FindAsync(p.CreatedBy);
                    var postLikes = await _manager._context.Likes.Where(x => x.Post == p.Id).ToListAsync();
                    postVms.Add(new PostVM
                    {
                        Id = p.Id,
                        Comments = comments,
                        CreateDate = p.CreateDate,
                        CreatedBy = createdBy,
                        MedicineNameAr = p.MedicineNameAr,
                        MedicineNameEng = p.MedicineNameEng,
                        State = p.State,
                        Text = p.Text,
                        Likes = likes,
                        PostLikes = postLikes
                    });
                }

            }
            catch (Exception e)
            {

            }

            return postVms;

        }

        public async Task<bool> AddPost(Post post)
        {
            try
            {
                post.CreateDate = DateTime.Now;
                _manager._context.Posts.Add(post);
                return await _manager.Complete();
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Like(long postId, long userId)
        {
            try
            {
                var likes = await _manager._context.Likes.SingleOrDefaultAsync(x =>
                    x.Post == postId && x.User == userId);
                if (likes == null)
                {
                    _manager._context.Likes.Add(new Like
                    {
                        User = userId,
                        Post = postId
                    });
                }
                else
                {
                    _manager._context.Likes.Remove(likes);
                }
            }
            catch { }
            return await _manager.Complete();
        }

        public async Task<List<long>> PostsList(string search)
        {
            List<long> ids = new List<long>();
            var posts = await _manager._context.Posts
                .Where(x =>
                    x.MedicineNameAr.Contains(search) ||
                    x.MedicineNameEng.Contains(search) ||
                    x.Text.Contains(search)
                ).ToListAsync();
            foreach (var p in posts)
            {
                ids.Add(p.Id);
            }

            return ids;
        }

        public async Task<List<long>> PostsList()
        {
            List<long> ids = new List<long>();
            List<Post> posts = await _manager._context.Posts.Take(1000).ToListAsync();
            posts = new List<Post>(posts.OrderByDescending(x => x.CreateDate));
            
            foreach (var p in posts)
            {
                ids.Add(p.Id);
            }

            return ids;
        }

        public async Task<PostVM> Post(long id, long userId)
        {
            //var post=  await _manager._context.Posts.FindAsync(id);
            var p = await _manager._context.Posts.FindAsync(id);
            var comments = await _manager._context.Comments.Where(x => x.Post == p.Id).ToListAsync();
            var likes = _manager._context.Likes.Count(x => x.Post == p.Id);
            var createdBy = await _manager._context.Users.FindAsync(p.CreatedBy);
            var postLikes = await _manager._context.Likes.Where(x => x.Post == p.Id).ToListAsync();
            var postVms = new PostVM
            {
                Id = p.Id,
                Comments = comments,
                CreateDate = p.CreateDate,
                CreatedBy = createdBy,
                MedicineNameAr = p.MedicineNameAr,
                MedicineNameEng = p.MedicineNameEng,
                State = p.State,
                Text = p.Text,
                Likes = likes,
                PostLikes = postLikes,
                AllowComment = (userId >= 1),PostType = p.PostType
            };
            return postVms;
        }

        public async Task<List<Comment>> Comments(long id) => await _manager._context.Comments.Where(x => x.Post == id).ToListAsync();

        public async Task<bool> Comment(long postId, long userId, string comment)
        {
            try
            {
                var user = await _manager._context.Users.FindAsync(userId);
                _manager._context.Comments.Add(new Comment
                {
                    CreateDate = DateTime.Now, Post = postId, Text = comment, State = State.Active, CreatedBy = user
                });
            }
            catch (Exception e)
            {
                
            }
            return await _manager.Complete();
        }

        public async Task<bool> DeleteComment(long id)
        {
            try
            {
                var comment = await _manager._context.Comments.FindAsync(id);
                if (comment != null) _manager._context.Comments.Remove(comment);
            }
            catch
            {
            }

            return await _manager.Complete();
        }

        public async Task<bool> DeletePost(long id)
        {

            try
            {
                var post = await _manager._context.Posts.FindAsync(id);
                if (post != null) _manager._context.Posts.Remove(post);
            }
            catch
            {
            }

            return await _manager.Complete();
        }


        public async Task<List<PostVM>> TimeLine(long id)
        {
            List<PostVM> timeline = new List<PostVM>();
            try
            {
                List<Post> posts = new List<Post>();
                posts = await _manager.Posts.Find(x => x.CreatedBy == id) as List<Post>;
                var likedPosts = await _manager.Likes.Find(x => x.User == id);
                var myComments = await _manager.Comments.Find(x => x.CreatedBy.Id == id);
                foreach (var comment in myComments)
                {
                    if (posts != null && posts.SingleOrDefault(x => x.Id == comment.Post) == null)
                    {
                        posts.Add(await _manager.Posts.SingleOrDefault(x => x.Id == comment.Post));
                    }
                }

                foreach (var likedPost in likedPosts)
                {
                    if (posts != null && posts.SingleOrDefault(x=>x.Id == likedPost.Post) == null)
                    {
                        posts.Add(await _manager.Posts.SingleOrDefault(x => x.Id == likedPost.Post));
                    }
                }

                if (posts != null) posts = new List<Post>(posts.OrderByDescending(x => x.CreateDate));
                foreach (var p in posts)
                {
                    var comments = await _manager._context.Comments.Where(x => x.Post == p.Id).ToListAsync();
                    var likes = _manager._context.Likes.Count(x => x.Post == p.Id);
                    var createdBy = await _manager._context.Users.FindAsync(p.CreatedBy);
                    var postLikes = await _manager._context.Likes.Where(x => x.Post == p.Id).ToListAsync();
                    timeline.Add(new PostVM
                    {
                        Id = p.Id,
                        Comments = comments,
                        CreateDate = p.CreateDate,
                        CreatedBy = createdBy,
                        MedicineNameAr = p.MedicineNameAr,
                        MedicineNameEng = p.MedicineNameEng,
                        State = p.State,
                        Text = p.Text,
                        Likes = likes,
                        PostLikes = postLikes,
                        AllowComment =true,
                        PostType = p.PostType
                    });
                }
            }
            catch (Exception e)
            {
                

            }

            return timeline;
        }
    }
}