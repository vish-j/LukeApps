namespace LukeApps.GenericRepository
{
    using Interfaces;
    using LukeApps.TrackingExtended;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using TrackingExtended.Interfaces;

    public class GenericRepository<TContext, TEntity> : IDisposable,
       IGenericRepository<TEntity> where TEntity : class, IEntity, IAuditDetail where TContext : ExtendedEntities, IExtendedContext, new()
    {
        public GenericRepository(string username)
        {
            this.Context = new TContext();
            this.Context.ConfigureUsername(username);
        }

        public TContext Context { get; }

        public string Username
        {
            get => this.Context.GetUsername();
            set => this.Context.ConfigureUsername(value);
        }

        public virtual IQueryable<TEntity> GetAll(string includeProperties = "") =>
            getAll(cleanPropRefs(includeProperties));

        public virtual IQueryable<TEntity> GetAll(params string[] includeProperties) =>
            getAll(cleanPropRefs(includeProperties));

        public TEntity FindBy(Expression<Func<TEntity, bool>> predicate, string includeProperties = "") =>
            getAll(cleanPropRefs(includeProperties)).FirstOrDefault(predicate);

        public TEntity FindBy(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties) =>
            getAll(cleanPropRefs(includeProperties)).FirstOrDefault(predicate);

        public Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, string includeProperties = "") =>
            getAll(cleanPropRefs(includeProperties)).FirstOrDefaultAsync(predicate);

        public Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, params string[] includeProperties) =>
            getAll(cleanPropRefs(includeProperties)).FirstOrDefaultAsync(predicate);

        public virtual void Add(TEntity entity) => this.Context.Set<TEntity>().Add(entity);

        public virtual void Delete(TEntity entity) => this.Context.Set<TEntity>().Remove(entity);

        /// <summary>
        /// Edits the the entity (Excludes Collections)
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Edit(TEntity entity) => this.Context.Entry(entity).State = EntityState.Modified;

        public virtual TChild FindChildBy<TChild>(Func<TEntity, IEnumerable<TChild>> selector, Expression<Func<TChild, bool>> predicate) where TChild : class, IEntity, IAuditDetail =>
            this.Context.Set<TChild>().AsQueryable().FirstOrDefault(predicate);

        public virtual Task<TChild> FindChildByAsync<TChild>(Func<TEntity, IEnumerable<TChild>> selector, Expression<Func<TChild, bool>> predicate) where TChild : class, IEntity, IAuditDetail =>
            this.Context.Set<TChild>().AsQueryable().FirstOrDefaultAsync(predicate);

        public virtual void AddChild<TChild>(Func<TEntity, IEnumerable<TChild>> selector, TChild entity) where TChild : class, IEntity, IAuditDetail =>
            this.Context.Set<TChild>().Add(entity);

        public virtual void DeleteChild<TChild>(Func<TEntity, IEnumerable<TChild>> selector, TChild entity) where TChild : class, IEntity, IAuditDetail =>
            this.Context.Set<TChild>().Remove(entity);

        public virtual void DeleteChildCollection<TParent, TChild>(Func<TParent, IEnumerable<TChild>> selector, TParent entity) where TChild : class, IEntity, IAuditDetail =>
            this.Context.Set<TChild>().RemoveRange(selector(entity));

        public virtual void EditChild<TChild>(Func<TEntity, IEnumerable<TChild>> selector, TChild entity) where TChild : class, IEntity, IAuditDetail =>
            this.Context.Entry(entity).State = EntityState.Modified;

        public virtual void UpdateChildCollection<TParent, TChild>(Func<TParent, IEnumerable<TChild>> selector, TParent oldItem, TParent newItem) where TChild : class, IEntity, IAuditDetail where TParent : class, IEntity, IAuditDetail
        {
            var oldChildItems = selector(oldItem).ToList();
            var newChildItems = selector(newItem).ToList();

            if (oldChildItems == null && newChildItems == null)
                return;

            var original = oldChildItems?.Select(o => new KeyValuePair<object, TChild>(o.GetID(), o)).ToList() ?? new List<KeyValuePair<object, TChild>>();
            var updated = newChildItems?.Select(o => new KeyValuePair<object, TChild>(o.GetID(), o)).ToList() ?? new List<KeyValuePair<object, TChild>>();

            var toRemove = original.Where(i => !updated.Any(u => u.Key.Equals(i.Key))).ToArray();
            var removed = toRemove.Select(i => this.Context.Entry(i.Value).State = EntityState.Deleted).ToArray();

            var toUpdate = original.Where(i => updated.Any(u => u.Key.Equals(i.Key))).ToList();
            toUpdate.ForEach(i =>
            {
                updated.FirstOrDefault(u => u.Key.Equals(i.Key)).Value.AuditDetail = i.Value.AuditDetail;
                this.Context.Entry(i.Value).CurrentValues.SetValues(updated.FirstOrDefault(u => u.Key.Equals(i.Key)).Value);
            });

            var toAdd = updated.Where(i => !original.Any(u => u.Key.Equals(i.Key))).ToList();
            toAdd.ForEach(i => (selector(oldItem) as ICollection<TChild>).Add(i.Value));
        }

        public virtual void SaveChanges() => this.Context.SaveChanges();

        public virtual Task SaveChangesAsync() => this.Context.SaveChangesAsync();

        private IQueryable<TEntity> getAll(string[] includeProperties)
        {
            IQueryable<TEntity> query = this.Context.Set<TEntity>();

            query = includePropertiesToQuery(query, includeProperties);

            return query;
        }

        private IQueryable<TEntity> includePropertiesToQuery(IQueryable<TEntity> query, string[] includeProperties)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        /// <summary>
        /// Clean Property References from input (string)
        /// </summary>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        private string[] cleanPropRefs(string includeProperties)
        {
            return includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Clean Property References from input (string array)
        /// </summary>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        private string[] cleanPropRefs(string[] includeProperties)
        {
            return includeProperties.Where(p => !string.IsNullOrEmpty(p)).ToArray();
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}