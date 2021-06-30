﻿using MangoAPI.DTO.Responses.Auth;
using MediatR;

namespace MangoAPI.DTO.Commands.Auth
{
    public class VerifyEmailCommand : IRequest<VerifyEmailResponse>
    {
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}