﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventStore.PoC.Domain.Entity.Infrastructure;

namespace EventStore.PoC.Store.DocumentStore.Infrastructure
{
    public interface IDocumentCollection<T, in TKey> : IDisposable where T : BaseEntity<TKey>
    {
        #region Public Methods

        Task<long> Count();

        Task<long> Count(Expression<Func<T, bool>> filter);

        Task<bool> Delete(T document);

        Task<bool> Delete(TKey id);

        Task<long> DeleteAll(Expression<Func<T, bool>> filter);

        Task<long> DeleteAll();

        Task<T> Get(TKey id);

        Task<T> Get(Expression<Func<T, bool>> filter);

        Task<IEnumerable<T>> GetAll();

        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter);

        Task Insert(T document);

        Task Insert(IEnumerable<T> documents);

        Task<bool> Update(T document);

        #endregion Public Methods
    }
}