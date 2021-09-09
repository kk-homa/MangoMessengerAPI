﻿using System;
using MangoAPI.BusinessLogic.Responses;
using MangoAPI.Domain.Constants;

namespace MangoAPI.BusinessLogic.ApiCommands.UserChats
{
    public record LeaveGroupResponse : ResponseBase<LeaveGroupResponse>
    {
        public Guid ChatId { get; init; }

        public static LeaveGroupResponse FromSuccess(Guid chatId)
        {
            return new()
            {
                Success = true,
                Message = ResponseMessageCodes.Success,
                ChatId = chatId
            };
        }
    }
}