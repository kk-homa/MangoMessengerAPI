﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MangoAPI.BusinessLogic.Models;
using MangoAPI.DataAccess.Database;
using MangoAPI.DataAccess.Database.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangoAPI.BusinessLogic.ApiQueries.Chats
{
    public class SearchChatsQueryHandler : IRequestHandler<SearchChatsQuery, SearchChatsResponse>
    {
        private readonly MangoPostgresDbContext _postgresDbContext;

        public SearchChatsQueryHandler(MangoPostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<SearchChatsResponse> Handle(SearchChatsQuery request, CancellationToken cancellationToken)
        {
            var chats = await _postgresDbContext.Chats.GetPublicChatsIncludeMessagesUsersAsync(cancellationToken);

            if (!string.IsNullOrEmpty(request.DisplayName) || !string.IsNullOrWhiteSpace(request.DisplayName))
            {
                chats = chats
                    .Where(x => x.Title.ToUpper().Contains(request.DisplayName.ToUpper()))
                    .ToList();
            }

            var resultList = new List<Chat>();

            foreach (var chat in chats)
            {
                var isMember = await _postgresDbContext.UserChats
                    .AnyAsync(x => x.ChatId == chat.Id && x.UserId == request.UserId, cancellationToken);

                resultList.Add(new Chat
                {
                    ChatId = chat.Id,
                    Title = chat.Title,
                    Image = chat.Image,
                    Description = chat.Description,
                    LastMessage = chat.Messages.Any()
                        ? chat.Messages.OrderBy(x => x.CreatedAt).Last().Content
                        : null,
                    LastMessageAuthor = chat.Messages.Any()
                        ? chat.Messages.OrderBy(x => x.CreatedAt).Last().User.DisplayName
                        : null,
                    LastMessageAt = chat.UpdatedAt?.ToShortTimeString() ?? (chat.Messages.Any()
                        ? chat.Messages.OrderBy(messageEntity => messageEntity.CreatedAt)
                            .Last().CreatedAt
                            .ToShortTimeString()
                        : null),
                    MembersCount = chat.MembersCount,
                    ChatType = chat.ChatType,
                    IsMember = isMember,
                });
            }

            return SearchChatsResponse.FromSuccess(resultList);
        }
    }
}