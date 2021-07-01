using BlogSoft.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BlogSoft.Core.Service
{
    // Ortak olarak yazdığımız sorguların bütün tablolar için kullanılmasını sağlamak amacıyla Generic yapısını kullanıyor olacağız
    public interface ICoreService<T> where T : CoreEntity
    {
        bool Add(T item);
        bool Add(List<T> items);
        bool Update(T item);
        bool Remove(T item);
        bool Remove(Guid id);
        bool RemoveAll(Expression<Func<T, bool>> expression);
        T GetById(Guid id);
        T GetByDefault(Expression<Func<T, bool>> expression);
        List<T> GetActive();
        List<T> GetDefault(Expression<Func<T, bool>> expression);
        List<T> GetAll();
        bool Activate(Guid id);
        bool Any(Expression<Func<T, bool>> expression);
        int Save();
    }
}
