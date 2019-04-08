﻿using AutoMapper;
using Cafe.Core;
using Cafe.Core.TabContext.Commands;
using Cafe.Domain;
using Cafe.Domain.Events;
using Cafe.Persistance.EntityFramework;
using FluentValidation;
using Marten;
using MediatR;
using Optional;
using Optional.Async;
using System.Threading.Tasks;

namespace Cafe.Business.TabContext.CommandHandlers
{
    public class CloseTabHandler : BaseTabHandler<CloseTab>
    {
        public CloseTabHandler(
            IValidator<CloseTab> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            IMenuItemsService menuItemsService)
            : base(validator, dbContext, documentSession, eventBus, mapper, menuItemsService)
        {
        }

        public override Task<Option<Unit, Error>> Handle(CloseTab command) =>
            TabShouldNotBeClosed(command.TabId).
            FilterAsync(async tab => command.AmountPaid >= tab.ServedItemsValue, Error.Validation("You cannot pay less than the bill amount.")).MapAsync(tab =>
            PublishEvents(tab.Id, tab.CloseTab(command.AmountPaid)));
    }
}
