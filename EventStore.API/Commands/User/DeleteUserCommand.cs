﻿using MediatR;
using System;

namespace EventStore.API.Commands.User
{
    public class DeleteUserCommand : IRequest<bool>
    {
        #region Public Properties

        public Guid UserId { get; set; }

        #endregion Public Properties
    }
}