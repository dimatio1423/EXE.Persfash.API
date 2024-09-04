
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.GenericRepos
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly PersfashApplicationDbContext context;
        private readonly DbSet<T> _entities;

        public GenericRepository(PersfashApplicationDbContext context)
        {
            this.context = context;
            _entities = context.Set<T>();

        }
        public async Task AddRange(List<T> entities)
        {
            try
            {
               _entities.AddRangeAsync(entities);
               await context.SaveChangesAsync();

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteRange(List<T> entities)
        {
            try
            {
                _entities.RemoveRange(entities);
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T?> Get(int id)
        {
            try
            {
                return await _entities.FindAsync(id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<T>> GetAll(int? page, int? size)
        {
            try
            {
                
                if (page.HasValue && size.HasValue)
                {
                    int validPage = page.Value > 0 ? page.Value : 1;
                    int validSize = size.Value > 0 ? size.Value : 10;

                    return await _entities.Skip((validPage - 1) * validSize).Take(validSize).ToListAsync();
                }

                return await _entities.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Add(T entity)
        {
            try
            {
                await _entities.AddAsync(entity);
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Remove(T entity)
        {
            try
            {
                _entities.Remove(entity);
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(T entity)
        {
            try
            {
                _entities.Update(entity);
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateRange(List<T> entities)
        {
            try
            {
                _entities.UpdateRange(entities);
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
