﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MangoAPI.BusinessLogic.ApiCommands.PasswordRestoreRequests;
using MangoAPI.BusinessLogic.Responses;
using MangoAPI.Domain.Constants;
using MangoAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MangoAPI.Tests.ApiCommandsTests.PasswordRestoreCommandHandlerTests;

public class PasswordRestoreShouldThrowWeakPassword : ITestable<PasswordRestoreCommand, ResponseBase>
{
    private readonly MangoDbFixture _mangoDbFixture = new();
    private readonly Assert<ResponseBase> _assert = new();

    [Fact]
    public async Task PasswordRestoreTest_Success()
    {
        Seed();
        var handler = CreateHandler();
        const string expectedMessage = ResponseMessageCodes.WeakPassword;
        string expectedDetails = ResponseMessageCodes.ErrorDictionary[expectedMessage];
        var command = new PasswordRestoreCommand
        {
            RequestId = "9c4ddced-5de5-4388-84fd-39f92a77a977".AsGuid(),
            NewPassword = "1234"
        };

        var result = await handler.Handle(command, CancellationToken.None);
            
        _assert.Fail(result, expectedMessage, expectedDetails);
    }
        
    public bool Seed()
    {
        _mangoDbFixture.Context.Users.Add(_user);
        _mangoDbFixture.Context.PasswordRestoreRequests.Add(new PasswordRestoreRequestEntity
        {
            Id = "9c4ddced-5de5-4388-84fd-39f92a77a977".AsGuid(),
            UserId = SeedDataConstants.RazumovskyId,
            Email = _user.Email,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(3)
        });

        _mangoDbFixture.Context.SaveChanges();

        _mangoDbFixture.Context.Entry(_user).State = EntityState.Detached;
            
        return true;
    }

    public IRequestHandler<PasswordRestoreCommand, Result<ResponseBase>> CreateHandler()
    {
        var userManagerMock = MockedObjects.GetUserServiceMock("1234");
        var responseFactory = new ResponseFactory<ResponseBase>();
        var handler = new PasswordRestoreCommandHandler(_mangoDbFixture.Context, userManagerMock, responseFactory);
        return handler;
    }
        
    private readonly UserEntity _user = new()
    {
        DisplayName = "razumovsky r",
        Bio = "11011 y.o Dotnet Developer from $\"{cityName}\"",
        Id = SeedDataConstants.RazumovskyId,
        UserName = "razumovsky_r",
        Email = "kolosovp95@gmail.com",
        NormalizedEmail = "KOLOSOVP94@GMAIL.COM",
        EmailConfirmed = true,
        PhoneNumberConfirmed = true,
        Image = "razumovsky_picture.jpg"
    };
}