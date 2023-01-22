using Microsoft.EntityFrameworkCore;
using PrintShop.core;
using PrintShop.models.Base;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrintShop.Repository
{
    internal class DbRepository<T> : IRepository<T> where T : Entity, new()
    {
        private readonly ApplicationContext _db;
        private readonly DbSet<T> _set;
        public bool AutoSaveChanges { get; set; } = true;
        public DbRepository(ApplicationContext db)
        {
            _db = db;
            _set = db.Set<T>();
        }

        public virtual IQueryable<T> Items => _set;

        public T Add(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges)
                _db.SaveChanges();
            return item;
        }
        public async Task<T> AddAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges)
                _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
            return item;
        }
        public T Delete(int id)
        {
            var item = new T() { Id = id };
            _db.Remove(item);
            if (AutoSaveChanges)
                _db.SaveChanges();
            return item;
        }
        public async Task<T> DeleteAsync(int id, CancellationToken Cancel = default)
        {
            var item = new T() { Id = id };
            _db.Remove(item);
            if (AutoSaveChanges)
                _db.SaveChangesAsync(Cancel);
            return item;
        }
        public T Get(int id) => Items.SingleOrDefault(item => item.Id == id);
        public int GetAll() {
            return Items.Count();
        }
        
        public async Task<T> GetAsync(int id, CancellationToken Cancel = default) => await Items
            .SingleOrDefaultAsync(item => item.Id == id, Cancel)
            .ConfigureAwait(false);
        public T Update(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges)
                _db.SaveChanges();
            return item;
        }
        public async Task<T> UpdateAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges)
                _db.SaveChangesAsync(Cancel);
            return item;
        }
    }
}
