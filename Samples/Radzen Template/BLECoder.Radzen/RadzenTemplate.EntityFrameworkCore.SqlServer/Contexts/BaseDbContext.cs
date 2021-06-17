using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RadzenTemplate.EntityFrameworkCore.SqlServer.Contexts
{
    public abstract class BaseDbContext : DbContext
    {
        protected IMapper _mapper;
        private readonly ILogger _logger;

        public BaseDbContext(DbContextOptions options, IMapper mapper, ILogger logger) : base(options)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Generic CRUD Methods
        public async Task<bool> CreateAsync<TDto, TEntity>(object key, TDto input) where TEntity : class
                                                                              where TDto : class
        {
            if (input == null || key == null)
                throw new ArgumentException("Input was null");

            var alreadyExists = await FindAsync<TEntity>(key);

            if (alreadyExists != null)
                return false;

            var entity = _mapper.Map<TEntity>(input);

            Add(entity);

            return await SafeSaveChanges();
        }
        public IQueryable<TDto> GetList<TDto, TEntity>() where TEntity : class
                                                               where TDto : class
        {
            var getAllExpression = Set<TEntity>().AsQueryable().Expression;
            var getAllLambda = Expression.Lambda<Func<IQueryable<TEntity>>>(getAllExpression);

            var entities = FromExpression(getAllLambda);

            return _mapper.ProjectTo<TDto>(entities);
        }

        public async Task<TDto> GetAsync<TDto, TEntity>(object key) where TEntity : class
                                                               where TDto : class
        {
            if (key == null)
                return null;

            var entity = await FindAsync<TEntity>(key);

            if (entity == null)
                return null;

            return _mapper.Map<TDto>(entity);
        }

        public async Task<bool> UpdateAsync<TDto, TEntity>(object key, TDto input, bool upsert = false) where TEntity : class
                                                                                                        where TDto : class
        {
            if (input == null || key == null)
                return false;

            var entity = await FindAsync<TEntity>(key);

            if (entity != null)
            {
                Entry(entity).CurrentValues.SetValues(input);
            }
            else if (upsert)
            {
                Add(entity);
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

            var entity = await FindAsync<TEntity>(key);

            if (entity == null)
                return false;

            Remove(entity);

            return await SafeSaveChanges();
        }
        #endregion

        public async Task<bool> SafeSaveChanges()
        {
            try
            {
                await SaveChangesAsync();
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
