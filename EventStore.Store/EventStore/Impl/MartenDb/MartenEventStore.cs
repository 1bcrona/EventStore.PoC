﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Store.EventStore.Infrastructure;
using Marten;
using Marten.Events.Projections;
using Marten.Events.Projections.Async;
using Marten.Storage;

namespace EventStore.Store.EventStore.Impl.MartenDb
{
    public class MartenEventStore : IEventStore
    {
        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public MartenEventStore(string connectionString)
        {
            _DocumentStore = Marten.DocumentStore.For(f =>
            {
                f.Connection(connectionString);
                f.AutoCreateSchemaObjects = AutoCreate.All;
                f.DefaultTenantUsageEnabled = true;
                f.Events.TenancyStyle = TenancyStyle.Separate;
            });
        }

        private readonly SemaphoreSlim _DaemonLock = new(1, 1);

        private IDaemon _Daemon;

        #endregion Public Constructors

        #region Public Properties

        private Marten.DocumentStore _DocumentStore { get; }

        #endregion Public Properties

        #region Public Methods

        public async Task<IEventCollection> GetCollection()
        {
            return await Task.FromResult(new MartenEventCollection(_DocumentStore));
        }

        public async Task<bool> Open()
        {
            return await Task.FromResult(true);
        }

        public async Task AddProjection(IEventProjection eventProjection)
        {
            if (!(eventProjection is IProjection martenProjection)) throw new Exception("NOT_A_VALID_PROJECTION");

            _DocumentStore.Events.InlineProjections.Add(martenProjection);
            await Task.CompletedTask;
        }

        public async Task StartProjectionDaemon()
        {

            await _DaemonLock.WaitAsync();

            try
            {
                if (_Daemon != null)
                {
                    var current = Interlocked.Exchange(ref _Daemon, null);
                    await current.StopAll();
                    current.Dispose();
                }

                _Daemon = _DocumentStore.BuildProjectionDaemon(
                    projections: _DocumentStore.Events.InlineProjections.ToArray());
                _Daemon.StartAll();
                await _Daemon.WaitForNonStaleResults();
            }
            finally
            {

                _DaemonLock.Release();
            }
        }




        #endregion Public Methods

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (!disposing) return;
            _DaemonLock?.Dispose();
            _Daemon?.Dispose();
            _DocumentStore?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MartenEventStore()
        {
            Dispose(false);
        }
    }
}