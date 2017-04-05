namespace Byndyusoft.Dotnet.Core.Infrastructure.Web.Authentication.JwtBearer
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using UserClaimsProvider;
    using UserClaimsProvider.Exceptions;

    public class TokensIssuingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserClaimsProvider _userClaimsProvider;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        private readonly SigningCredentials _signingCredentials;
        private readonly PathString _getEndpointPath;
        private readonly TimeSpan? _lifetime;

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public TokensIssuingMiddleware(RequestDelegate next, IUserClaimsProvider userClaimsProvider, IOptions<TokensIssuingOptions> options)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            if (userClaimsProvider == null)
                throw new ArgumentNullException(nameof(userClaimsProvider));
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(options.Value.GetEndpotint))
                throw new ArgumentNullException(nameof(options.Value.GetEndpotint));
            if (string.IsNullOrWhiteSpace(options.Value.SigningAlgorithmName))
                throw new ArgumentNullException(nameof(options.Value.SigningAlgorithmName));
            if (options.Value.SigningKey == null)
                throw new ArgumentNullException(nameof(options.Value.SigningKey));

            _next = next;
            _userClaimsProvider = userClaimsProvider;
            _tokenHandler = new JwtSecurityTokenHandler();

            _getEndpointPath = new PathString(options.Value.GetEndpotint);
            _signingCredentials = new SigningCredentials(options.Value.SigningKey, options.Value.SigningAlgorithmName);
            _lifetime = options.Value.Lifetime;

            _jsonSerializerSettings = new JsonSerializerSettings
                                      {
                                          Formatting = Formatting.Indented,
                                          Converters = {new IsoDateTimeConverter()},
                                          ContractResolver = new CamelCasePropertyNamesContractResolver()
                                      };
        }

        private static async Task<TokenRequestModel> DeserializeModel(HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body))
                return JsonConvert.DeserializeObject<TokenRequestModel>(await reader.ReadToEndAsync());
        }

        private static bool IsRequestValid(string requestMethod, TokenRequestModel requestModel)
        {
            return requestMethod == HttpMethods.Post
                   && requestModel != null
                   && string.IsNullOrWhiteSpace(requestModel.Login) == false
                   && string.IsNullOrWhiteSpace(requestModel.Password) == false;
        }

        private async Task WriteResponse(HttpResponse response, HttpStatusCode statusCode, object payload)
        {
            response.StatusCode = (int) statusCode;
            response.ContentType = "application/json; charset=utf-8";
            await response.WriteAsync(JsonConvert.SerializeObject(payload, _jsonSerializerSettings));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Equals(_getEndpointPath) == false)
            {
                await _next(context);
                return;
            }

            var requestModel = await DeserializeModel(context.Request);
            if (IsRequestValid(context.Request.Method, requestModel) == false)
            {
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return;
            }

            Claim[] claims;
            try
            {
                claims = await _userClaimsProvider.GetClaimsAsync(requestModel.Login, requestModel.Password);
            }
            catch (LoginNotFoundException)
            {
                await WriteResponse(context.Response, HttpStatusCode.BadRequest, new {ErrorMessage = "Неверный логин или пароль"});
                return;
            }
            catch (IncorrectPasswordException)
            {
                await WriteResponse(context.Response, HttpStatusCode.BadRequest, new {ErrorMessage = "Неверный логин или пароль"});
                return;
            }
            var notBefore = DateTime.UtcNow;
            var expires = _lifetime.HasValue ? notBefore.Add(_lifetime.Value) : (DateTime?)null;

            var token = new JwtSecurityToken(claims: claims, notBefore: DateTime.UtcNow, expires: expires, signingCredentials: _signingCredentials);
            await WriteResponse(context.Response, HttpStatusCode.OK, new {Token = _tokenHandler.WriteToken(token), ExpirationDate = expires});
        }
    }
}