using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using EventStore.Store.EventStore.Infrastructure;
using EventStore.StreamListener.Projection.Marten;
using EventStore.StreamListener.Projection.Marten.Infrastructure;
using System;
using System.Threading.Tasks;

namespace EventStore.StreamListener
{
    public class App
    {
        #region Private Fields

        private IEventStore _EventStore;

        #endregion Private Fields

        #region Public Constructors

        public App(IEventStore eventStore)
        {
            _EventStore = eventStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task Run()
        {
            var userProjection = new BaseMartenProjection<User, Guid>();

            userProjection.AddEvent<UserCreated>(@event => @event.EntityId, (user, e) =>
            {
                user.Id = e.EntityId;
                user.UserName = e.Data?.UserName;
                user.Active = true;
            });

            userProjection.AddEvent<UserDeleted>(@event => @event.EntityId, (user, _) =>
            {
                user.Active = false;
            });

            var contentProjection = new ContentProjection();

            contentProjection.AddEvent<ContentCreated>(@event => @event.EntityId, (content, e) =>
            {
                content.Id = e.EntityId;
                content.ContentMetadata = e.Data?.ContentMetadata;
                content.ContentCdnLink = e.Data?.ContentCdnLink;
                content.PlayCount = e.Data?.PlayCount ?? 0;
                content.Active = true;
            });

            contentProjection.AddEvent<ContentDeleted>(@event => @event.EntityId, (content, _) =>
            {
                content.Active = false;
            });

            contentProjection.AddEvent<ContentPlayed>(@event => @event.Data.ViewedContent.Id, (content, _) =>
            {
                content.PlayCount++;
            });

            var playedContentProjection = new PlayedContentProjection();

            playedContentProjection.AddEvent<ContentPlayed>(@event => @event.EntityId, (playedContent, e) =>
           {
               playedContent.Id = e.EntityId;
               playedContent.AssignUser(e.Data?.ViewedUser);
               playedContent.AssignContent(e.Data?.ViewedContent);
               playedContent.Active = true;
           });

            playedContentProjection.AddEvent<ContentPlayDeleted>(@event => @event.EntityId, (playedContent, _) =>
            {
                playedContent.Active = false;
            });

            await _EventStore.AddProjection(playedContentProjection);
            await _EventStore.AddProjection(contentProjection);
            await _EventStore.AddProjection(userProjection);
            await _EventStore.StartProjectionDaemon();

            var collection = await _EventStore.GetCollection();

            var contents = await collection.Query<Content>();

            Console.WriteLine("Contents");
            foreach (var content in contents) Console.WriteLine(content.Id);

            Console.WriteLine("Users");

            var users = await collection.Query<User>();
            foreach (var user in users) Console.WriteLine(user.Id);



            Console.WriteLine("Played Contents");
            var playedContents = await collection.Query<PlayedContent>();
            foreach (var playedContent in playedContents) Console.WriteLine(playedContent.Id);
        }

        #endregion Public Methods
    }
}