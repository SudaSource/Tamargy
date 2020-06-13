using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.EF.DataCtx;

namespace Core.Lib.Core
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DataCtx _context;

        public Repository(DataCtx context)
        {
            //var db = new DataCtx();
            _context = context;
        }

        public async Task<TEntity> GetSingle(long id)
        {
            try
            {
                return await _context.Set<TEntity>().FindAsync(id);
            }
            catch (Exception ec)
            {
                return null;
            }
        }


        public TEntity Get(long id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _context.Set<TEntity>().SingleOrDefaultAsync(predicate);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public bool Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public bool UpdateValues(TEntity entity, TEntity entity2)
        {
            _context.Entry(entity).CurrentValues.SetValues(entity2);
            return true;
        }

        public async Task<IQueryable<TEntity>> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return  _context.Set<TEntity>().Where(predicate);
        }
    }
}