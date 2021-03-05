﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using EventStore.PoC.Domain.Entity.Infrastructure;
using EventStore.PoC.Store.Attributes;
using EventStore.PoC.Store.DocumentStore.Infrastructure;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace EventStore.PoC.Store.DocumentStore.Impl.Mongo
{
    public class MongoStore : IDocumentStore
    {
        #region Public Properties

        public bool Open => _Client != null;

        #endregion Public Properties

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        #region Private Destructors

        ~MongoStore()
        {
            ReleaseUnmanagedResources();
        }

        #endregion Private Destructors

        private void ReleaseUnmanagedResources()
        {
            _Client = null;
            _Database = null;
        }

        #region Private Fields

        private readonly string _ConnectionString;
        private readonly string _DatabaseName;
        private MongoClient _Client;

        private static readonly string s_DefaultConnectionString = @"mongodb://localhost:27017/mongo";
        private IMongoDatabase _Database;

        #endregion Private Fields

        #region Public Constructors

        public MongoStore() : this(s_DefaultConnectionString)
        {
        }

        public MongoStore(string connectionString)
        {
            var url = new MongoUrl(connectionString);
            _ConnectionString = url.Url;
            _DatabaseName = url.DatabaseName;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task Connect()
        {
            _Client = new MongoClient(_ConnectionString);
            _Database = _Client.GetDatabase(_DatabaseName);
            await Task.CompletedTask;
        }

        public async Task<IDocumentCollection<T, TKey>> GetCollection<T, TKey>() where T : BaseEntity<TKey>
        {
            if (!Open) await Connect().ConfigureAwait(false);

            RegisterUniqueKey<T, TKey>();

            var collection = _Database.GetCollection<T>(GetCollectionName<T>());

            return new MongoCollection<T, TKey>(collection);
        }

        #endregion Public Methods

        #region Private Methods

        private string GetCollectionName<T>()
        {
            var type = typeof(T).GetTypeInfo();

            var attr = type.GetCustomAttribute<CollectionName>();

            return attr == null ? type.Name.ToLowerInvariant() : attr.Name;
        }

        private void RegisterUniqueKey<T, TKey>() where T : BaseEntity<TKey>
        {
            var type = typeof(T);

            if (BsonClassMap.IsClassMapRegistered(type) == false)
            {
                BsonClassMap.RegisterClassMap<T>(cm =>
                {
                    cm.AutoMap();
                    if (cm.IdMemberMap == null) cm.MapIdProperty("Id");
                });
                return;
            }

            BsonClassMap.LookupClassMap(type);
        }

        #endregion Private Methods
    }
}