using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Core.Data.Models;
using Core.EF.DataCtx;

namespace Core.Lib.Core
{
    public class Worker : IWorker
    {
        public DataCtx _context;
        public DataCtx DataCtx { get; set; }
        public Worker(DataCtx context)
        {
            _context = context;

            Comments = new Repository<Comment>(_context);
            ErrorLogs = new Repository<ErrorLog>(_context);
            Likes = new Repository<Like>(_context);
            Posts = new Repository<Post>(_context);
            Users = new Repository<User>(_context);

        }

        public DataCtx GetCtx()
        {
            return DataCtx ?? new DataCtx();
        }
        public Repository<Comment> Comments { get; private set; }
        public Repository<ErrorLog> ErrorLogs { get; private set; }
        public Repository<Like> Likes { get; private set; }
        public Repository<Post> Posts { get; private set; }
        public Repository<Reply> Replies { get; private set; }
        public Repository<User> Users { get; private set; }


        public async Task<bool> Complete()
        {
            try
            {
                return (await _context.SaveChangesAsync() >= 1);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}