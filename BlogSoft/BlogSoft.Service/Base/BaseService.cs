﻿using BlogSoft.Core.Entity;
using BlogSoft.Core.Entity.Enums;
using BlogSoft.Core.Service;
using BlogSoft.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Transactions;

namespace BlogSoft.Service.Base
{

    public class BaseService<T> : ICoreService<T> where T : CoreEntity
    {
        private readonly BlogSoftContext _context;

        public BaseService(BlogSoftContext context)
        {
            _context = context;
        }

        public bool Activate(Guid id)
        {
            T activated = GetById(id);
            activated.Status = Status.Active;
            return Update(activated);
        }

        public bool Add(T item)
        {
            try
            {
                
                _context.Set<T>().Add(item);
                return Save() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                
                using (TransactionScope ts = new TransactionScope())
                {
                    _context.Set<T>().AddRange(items);
                    ts.Complete(); 
                    return Save() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool Any(Expression<Func<T, bool>> expression) => _context.Set<T>().Any(expression);


        public List<T> GetActive() => _context.Set<T>().Where(x => x.Status != Status.Deleted || x.Status != Status.None).ToList();


        public List<T> GetAll() => _context.Set<T>().ToList();


        public T GetByDefault(Expression<Func<T, bool>> expression) => _context.Set<T>().FirstOrDefault(expression);


        public T GetById(Guid id) => _context.Set<T>().Find(id);


        public List<T> GetDefault(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).ToList();
        }

        public bool Remove(T item)
        {
            try
            {
                _context.Set<T>().Remove(item);
                return Save() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Remove(Guid id)
        {
            try
            {
                T item = GetById(id);
                _context.Set<T>().Remove(item);
                return Save() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> expression)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var collection = GetDefault(expression);
                    int count = 0;
                    foreach (var item in collection)
                    {
                        bool operationResult = Remove(item);

                        if (operationResult)
                            count++;
                    }

                    if (collection.Count == count)
                        ts.Complete();
                    else
                        return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public bool Update(T item)
        {
            try
            {
                _context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
