using E_TicaretAPI.Application.Abstraction.Storage;
using E_TicaretAPI.Application.Features.Commands.Product.CreateProduct;
using E_TicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using E_TicaretAPI.Application.Features.Commands.Product.UpdateProduct;
using E_TicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using E_TicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using E_TicaretAPI.Application.Features.Queries.Product.GetAllProduct;
using E_TicaretAPI.Application.Features.Queries.Product.GetByIdProduct;
using E_TicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using E_TicaretAPI.Application.Repositories;
using E_TicaretAPI.Application.RequestParameters;

using E_TicaretAPI.Application.ViewModels.Products;
using E_TicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_TicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {


        readonly IMediator _mediator;
        public ProductsController( IMediator mediator)
        {
            
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
          GetAllProductQueryResponse response =  await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);

          
            //Tracking Test
            //Product p = await _productReadRepository.GetByIdAsync("00c2f8e5-56df-4b01-bcd5-2852764896e8", false);
            //p.Name = "Product 2";
            //await _productWriteRepository.SaveAsync();
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);

            return  Ok(response);

        }

            [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest); 
          
            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            //Ef de bir veritabanın elde edilen veriyi güncellemek için ilgili verinin context sınıfından  gelmesi yeterlidir.
            //Güncelleme işlemi yapmak için ilgili veri üzerinde propertylerde değişiklik yapmamız tracking mekanızmasi tarafından bunun update sorgusu olduğu anlaşılır ve savachanges yapıldığında update sorgusu execute edilir.
            // eğerki ilgili veri tracking mekanizması tarafından takip edilmiyorsa ve bu veri üzerinde güncelleme yapılmak isteniyorsa ef deki update fonksiyonu kullanılabilir.

            UpdateProductCommandResponse response =  await _mediator.Send(updateProductCommandRequest);
    
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
        {

            RemoveProductCommandResponse response =  await  _mediator.Send(removeProductCommandRequest);
            return Ok();
        }



        [HttpPost("[action]")] // bu sekilde actionda id  parametresi query stringden gelir. www.asd.com/api/products?id=bla-bla-bla
        public async Task<IActionResult> UploadProductFile([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {

            uploadProductImageCommandRequest.Files = Request.Form.Files;
           UploadProductImageCommandResponse response =  await _mediator.Send(uploadProductImageCommandRequest);
            return Ok();
        }

        [HttpGet("[action]/{Id}")] // route parametrelerinden gelecek id
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
            List< GetProductImagesQueryResponse>  response =  await _mediator.Send(getProductImagesQueryRequest);
            return Ok(response);
        }



        [HttpDelete("[action]/{Id}/")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest, [FromQuery] string ImageId)
        {
            removeProductImageCommandRequest.ImageId = ImageId;
            RemoveProductImageCommandResponse response = await _mediator.Send(removeProductImageCommandRequest);
          
            return Ok();
        }
    }
}
