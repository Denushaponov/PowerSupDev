using DevExpress.Mvvm;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WpfPaging.Events;

namespace WpfPaging.Services
{

    /// <summary>
    /// Этот класс позволяет обеспечить взаимодействие с базой данных
    /// </summary>
    public class Repository
    {
        private readonly LiteDatabase _database;
        private readonly EventBus _eventBus;
        public Repository(LiteDatabase database, EventBus eventBus)
        {
            _database = database;
            _eventBus = eventBus;
        }

        public async Task Save<T>(T item, Guid id)
        {
            await Task.Run(()=> GetCollection<T>().Upsert(item));
           _ = _eventBus.Publish(new OnSave<T>(item, id));
           
        }

        public async Task Remove<T>(Guid id)
        {
            await Task.Run(() => GetCollection<T>().Delete(id));
            _=_eventBus.Publish(new OnDelete<T>(id));

        
        }


        public Task <IEnumerable<T>> FindAll<T>()
        {
            return Task.Run(() => GetCollection<T>().FindAll());
        }


        private ILiteCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>();
        }

       
    }

    public class OnSave<T> : IEvent
    {
        public T Entity { get; set; }
        public Guid Id { get; set; }

        public OnSave(T entity, Guid id)
        {
            Entity = entity;
            Id = id;
        }
    }

    public class OnDelete<T> : IEvent
    {
        public Guid Id { get; set; }

        public OnDelete(Guid id)
        {
           Id = id;
        }
    }
}
