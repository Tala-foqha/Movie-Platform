using MoviePlatform1.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Repository
{
    public class GenericRepository<T> :IGenericRepository<T> where T:class
    {
            protected readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<T>CreateAsync(T entity)
        {
           //set<Product>,product.Addنفس المعنى
          await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Remove(entity);
            var affected = await _context.SaveChangesAsync();
            return affected > 0;
        }

        public Task<bool> DeleteRangAsync(List<T> entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, string[]? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<T?> Getone(Expression<Func<T, bool>> filter, string[]? includes = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetQureable(Expression<Func<T, bool>> filter, string[]? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool>UpdateAsync(T entity)
        {
           _context.Update(entity);
            var affected= await _context.SaveChangesAsync();//number of row that will updatedxs
            return affected > 0;
        }
        public async Task<bool> UpdateRangAsync(List<T> entity)
        {
            _context.UpdateRange(entity);
            var affected=await _context.SaveChangesAsync();
            return affected > 0;
        }
    }
}
