﻿using System.Threading;
using System.Threading.Tasks;
using MangoAPI.BusinessLogic.BusinessExceptions;
using MangoAPI.DataAccess.Database;
using MangoAPI.Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangoAPI.BusinessLogic.ApiCommands.Users
{
    public class UpdateUserInformationCommandHandler : IRequestHandler<UpdateUserInformationCommand,
        UpdateUserInformationResponse>
    {
        private readonly MangoPostgresDbContext _postgresDbContext;

        public UpdateUserInformationCommandHandler(MangoPostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<UpdateUserInformationResponse> Handle(UpdateUserInformationCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _postgresDbContext.Users
                .Include(x => x.UserInformation)
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is null) throw new BusinessException(ResponseMessageCodes.UserNotFound);

            user.UserInformation.FirstName = request.FirstName;
            user.UserInformation.LastName = request.LastName;
            user.UserInformation.BirthDay = request.BirthDay;

            user.UserInformation.Website = request.Website;
            user.UserInformation.Address = request.Address;

            user.UserInformation.Facebook = request.Facebook;
            user.UserInformation.Twitter = request.Twitter;
            user.UserInformation.Instagram = request.Instagram;
            user.UserInformation.LinkedIn = request.LinkedIn;

            user.UserInformation.ProfilePicture = request.ProfilePicture;

            _postgresDbContext.UserInformation.Update(user.UserInformation);
            await _postgresDbContext.SaveChangesAsync(cancellationToken);

            return UpdateUserInformationResponse.SuccessResponse;
        }
    }
}