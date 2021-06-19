using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RadzenTemplate.Server.Repositories
{
    public class DbContextRepository<TContext> where TContext : DbContext
    {
        protected readonly ILogger _logger;
        protected readonly TContext _context;
        protected readonly IMapper _mapper;

        public DbContextRepository(ILogger logger, TContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }



        #region Generic CRUD Methods
        public async Task<bool> CreateAsync<TDto, TEntity>(object key, TDto input) where TEntity : class
                                                                              where TDto : class
        {
            if (input == null || key == null)
                throw new ArgumentException("Input was null");

            var alreadyExists = await _context.FindAsync<TEntity>(key);

            if (alreadyExists != null)
                return false;

            var entity = _mapper.Map<TEntity>(input);

            _context.Add(entity);

            return await SafeSaveChanges();
        }
        public IQueryable<TDto> GetList<TDto, TEntity>() where TEntity : class
                                                               where TDto : class
        {
            var getAllExpression = _context.Set<TEntity>().AsQueryable().Expression;
            var getAllLambda = Expression.Lambda<Func<IQueryable<TEntity>>>(getAllExpression);

            var entities = _context.FromExpression(getAllLambda);

            return _mapper.ProjectTo<TDto>(entities);
        }

        public async Task<TDto> GetAsync<TDto, TEntity>(object key) where TEntity : class
                                                               where TDto : class
        {
            if (key == null)
                return null;

            var entity = await _context.FindAsync<TEntity>(key);

            if (entity == null)
                return null;

            return _mapper.Map<TDto>(entity);
        }

        public async Task<bool> UpdateAsync<TDto, TEntity>(object key, TDto input, bool upsert = false) where TEntity : class
                                                                                                        where TDto : class
        {
            if (input == null || key == null)
                return false;

            var entity = await _context.FindAsync<TEntity>(key);

            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(input);
            }
            else if (upsert)
            {
                _context.Add(entity);
            }
            else
            {
                return false;
            }

            return await SafeSaveChanges();
        }

        public async Task<bool> DeleteAsync<TDto, TEntity>(object key) where TEntity : class
                                                                  where TDto : class
        {
            if (key == null)
                return false;

            var entity = await _context.FindAsync<TEntity>(key);

            if (entity == null)
                return false;

            _context.Remove(entity);

            return await SafeSaveChanges();
        }
        #endregion
        
        protected async Task<bool> SafeSaveChanges()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to save to database");
                return false;
            }
        }
    }
}
