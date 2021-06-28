﻿namespace MangoAPI.Domain.Constants
{
    public static class ResponseMessageCodes
    {
        public static string Success => "SUCCESS";
        public static string LogoutTokenNotFound => "REFRESH_TOKEN_NOT_FOUND";
        public static string LogoutTokenInvalid => "REFRESH_TOKEN_NOT_VALID";
        public static string LogoutSuspiciousLogout => "SUSPICIOUS_LOGOUT";
        public static string RegisterUserAlreadyRegistered => "USER_ALREADY_REGISTERED";
        public static string RegisterTermsNotAccepted => "TERMS_NOT_ACCEPTED";
        public static string RefreshTokenInvalidRefreshTokenProvided => "INVALID_REFRESH_TOKEN_PROVIDED";
        public static string RefreshTokenUserNotFound => "USER_NOT_FOUND_FOR_TOKEN";
        public static string LoginInvalidEmail => "INVALID_EMAIL";
        public static string LoginInvalidPassword => "INVALID_PASSWORD";
        public static string ConfirmRegisterInvalidIdentifier => "INVALID_CONFIRMATION_IDENTIFIER";
        public static string Unverified => "UNVERIFIED_USER";
    }
}