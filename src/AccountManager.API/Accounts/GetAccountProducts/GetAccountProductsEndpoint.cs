// <copyright file="GetAccountProductsEndpoint.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Accounts.GetAccountProducts
{
    using AccountManager.API.Authorization;
    using AccountManager.Application.Queries.GetAccountProductsQuery;
    using AccountManager.Domain.Exceptions;
    using FastEndpoints;
    using MediatR;

    /// <summary>
    /// Endpoint for retrieving products associated with an account.
    /// </summary>
    public class GetAccountProductsEndpoint(IMediator mediator, AutoMapper.IMapper mapper)
        : Endpoint<GetAccountProductsAPIRequest, GetAccountProductsAPIResponse>
    {
        /// <summary>
        /// Configures endpoint metadata.
        /// </summary>
        public override void Configure()
        {
            Version(1);
            Get("/accounts/{accountId}/products");
            AllowAnonymous(); // Validated by preprocessor.
            Validator<GetAccountProductsValidator>();
            /////PreProcessor<PermissionPreProcessor<GetAccountProductsAPIRequest>>();

            Summary(s =>
            {
                s.Summary = "Get Account Products";
                s.Description = "Retrieves paginated products associated with a specific account.";
                s.Response<GetAccountProductsAPIResponse>(StatusCodes.Status200OK, "Successfully retrieved account products.");
                s.Response<AccountNotFoundException>(StatusCodes.Status404NotFound, "Account not found.");
            });
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="req">The request object.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task HandleAsync(GetAccountProductsAPIRequest req, CancellationToken ct)
        {
            req.AccountId = Route<Guid>("accountId");
            var request = mapper.Map<GetAccountProductsQueryRequest>(req);
            var result = await mediator.Send(request, ct).ConfigureAwait(false);
            var response = mapper.Map<GetAccountProductsAPIResponse>(result);
            await Send.OkAsync(response, ct).ConfigureAwait(false);
        }
    }
}
