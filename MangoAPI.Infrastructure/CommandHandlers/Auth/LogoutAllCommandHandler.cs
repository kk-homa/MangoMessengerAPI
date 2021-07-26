﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MangoAPI.Domain.Constants;
using MangoAPI.Domain.Entities;
using MangoAPI.DTO.ApiCommands.Auth;
using MangoAPI.DTO.Responses.Auth;
using MangoAPI.Infrastructure.BusinessExceptions;
using MangoAPI.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MangoAPI.Infrastructure.CommandHandlers.Auth
{
    public class LogoutAllCommandHandler : IRequestHandler<LogoutAllCommand, LogoutResponse>
    {
        private readonly MangoPostgresDbContext _postgresDbContext;
        private readonly UserManager<UserEntity> _userManager;

        public LogoutAllCommandHandler(MangoPostgresDbContext postgresDbContext, UserManager<UserEntity> userManager)
        {
            _postgresDbContext = postgresDbContext;
            _userManager = userManager;
        }

        public async Task<LogoutResponse> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
        {
            var token = await _postgresDbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Id == request.RefreshTokenId, cancellationToken);

            if (token is null)
            {
                throw new BusinessException(ResponseMessageCodes.InvalidOrEmptyRefreshToken);
            }

            var user = await _userManager.FindByIdAsync(token.UserId);

            if (user is null || user.Id != token.UserId)
            {
                throw new BusinessException(ResponseMessageCodes.UserNotFound);
            }

            var userTokens = _postgresDbContext
                .RefreshTokens
                .Where(x => x.UserId == user.Id);

            _postgresDbContext.RefreshTokens.RemoveRange(userTokens);

            await _postgresDbContext.SaveChangesAsync(cancellationToken);

            return LogoutResponse.SuccessResponse;
        }
    }
}