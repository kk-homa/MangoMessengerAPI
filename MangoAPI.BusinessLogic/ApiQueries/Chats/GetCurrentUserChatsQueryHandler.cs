﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MangoAPI.BusinessLogic.BusinessExceptions;
using MangoAPI.DataAccess.Database;
using MangoAPI.Domain.Constants;
using MangoAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangoAPI.BusinessLogic.ApiQueries.Chats
{
    public class
        GetCurrentUserChatsQueryHandler : IRequestHandler<GetCurrentUserChatsQuery, GetCurrentUserChatsResponse>
    {
        private readonly MangoPostgresDbContext _postgresDbContext;

        public GetCurrentUserChatsQueryHandler(MangoPostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<GetCurrentUserChatsResponse> Handle(GetCurrentUserChatsQuery request,
            CancellationToken cancellationToken)
        {
            var currentUser =
                await _postgresDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (currentUser == null) throw new BusinessException(ResponseMessageCodes.UserNotFound);

            var chats = await _postgresDbContext.UserChats
                .AsNoTracking()
                .Include(x => x.Chat)
                .ThenInclude(x => x.Messages)
                .ThenInclude<UserChatEntity, MessageEntity, UserEntity>(x => x.User)
                .Where(x => x.UserId == currentUser.Id)
                .ToListAsync(cancellationToken);

            return GetCurrentUserChatsResponse.FromSuccess(chats);
        }
    }
}