﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MangoAPI.BusinessLogic.ApiCommands.KeyExchange;
using MangoAPI.BusinessLogic.ApiQueries.KeyExchange;
using MangoAPI.BusinessLogic.Responses;
using MangoAPI.DiffieHellmanConsole.Consts;
using Newtonsoft.Json;

namespace MangoAPI.DiffieHellmanConsole.Services
{
    public class KeyExchangeService
    {
        private const string Route = "key-exchange";
        private readonly HttpClient _httpClient;

        public KeyExchangeService(string accessToken)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<GetKeyExchangeResponse> GetKeyExchangesAsync()
        {
            var route = Urls.ApiUrl + Route;
            var result = await HttpRequest.GetAsync(_httpClient, route);
            var response = JsonConvert.DeserializeObject<GetKeyExchangeResponse>(result);
            return response;
        }

        public async Task<CreateKeyExchangeResponse> CreateKeyExchangeRequestAsync(Guid requestUserId,
            string publicKey)
        {
            var command = new CreateKeyExchangeRequest
            {
                PublicKey = publicKey,
                RequestedUserId = requestUserId
            };

            var route = Urls.ApiUrl + Route;
            var result = await HttpRequest.PostWithBodyAsync(_httpClient, route, command);
            var response = JsonConvert.DeserializeObject<CreateKeyExchangeResponse>(result);
            return response;
        }

        public async Task<ResponseBase> ConfirmOrDeclineKeyExchange(Guid requestId, string publicKeyBase64)
        {
            var request = new ConfirmOrDeclineKeyExchangeRequest
            {
                Confirmed = true,
                PublicKey = publicKeyBase64,
                RequestId = requestId
            };

            var route = Urls.ApiUrl + Route;
            var result = await HttpRequest.DeleteWithBodyAsync(_httpClient, route, request);
            var response = JsonConvert.DeserializeObject<ResponseBase>(result);
            return response;
        }
    }
}