using E_TicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_TicaretAPI.Application.Features.Queries.Product.GetByIdProduct
{
    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly ILogger<GetByIdProductQueryHandler> _logger;
        public GetByIdProductQueryHandler(IProductReadRepository productReadRepository, ILogger<GetByIdProductQueryHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
        {
           E_TicaretAPI.Domain.Entities.Product product =  await _productReadRepository.GetByIdAsync(request.Id, false);
            _logger.LogInformation($"Log lanan ürün Id -> {product.Id} , ürün adı:{product.Name}");
            return new()
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
            };
            
        }
    }
}
