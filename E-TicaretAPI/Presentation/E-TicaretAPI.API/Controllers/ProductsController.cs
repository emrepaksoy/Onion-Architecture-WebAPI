using E_TicaretAPI.Application.Features.Commands.Product.CreateProduct;
using E_TicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using E_TicaretAPI.Application.Features.Commands.Product.UpdateProduct;
using E_TicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using E_TicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using E_TicaretAPI.Application.Features.Queries.Product.GetAllProduct;
using E_TicaretAPI.Application.Features.Queries.Product.GetByIdProduct;
using E_TicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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


        [HttpGet("[Action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetProducts([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest) => Ok(await _mediator.Send(getAllProductQueryRequest));

        #region tracking test
        //Tracking Test
        //Product p = await _productReadRepository.GetByIdAsync("00c2f8e5-56df-4b01-bcd5-2852764896e8", false);
        //p.Name = "Product 2";
        //await _productWriteRepository.SaveAsync();
        #endregion

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)=> Ok(await _mediator.Send(getByIdProductQueryRequest));

        [HttpPost("[Action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> CreateProduct(CreateProductCommandRequest createProductCommandRequest)
          => Ok( await _mediator.Send(createProductCommandRequest)); 

        [HttpPut("[Action]")]
        [Authorize(AuthenticationSchemes = "Admin")]

        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommandRequest updateProductCommandRequest) => Ok(await _mediator.Send(updateProductCommandRequest));

        #region Update fonks not
        //Ef de bir veritabanın elde edilen veriyi güncellemek için ilgili verinin context sınıfından  gelmesi yeterlidir.
        //Güncelleme işlemi yapmak için ilgili veri üzerinde propertylerde değişiklik yapmamız tracking mekanızmasi tarafından bunun update sorgusu olduğu anlaşılır ve savachanges yapıldığında update sorgusu execute edilir.
        // eğerki ilgili veri tracking mekanizması tarafından takip edilmiyorsa ve bu veri üzerinde güncelleme yapılmak isteniyorsa ef deki update fonksiyonu kullanılabilir.
        #endregion

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest) =>  Ok(await _mediator.Send(removeProductCommandRequest));



        [HttpPost("[action]")] 
        // bu sekilde actionda id  parametresi query stringden gelir. www.asd.com/api/products?id=bla-bla-bla
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> UploadProductFile([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            return Ok(await _mediator.Send(uploadProductImageCommandRequest));
        }


        [HttpGet("[action]/{Id}")] 
        // route parametrelerinden gelecek id
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest) => Ok( await _mediator.Send(getProductImagesQueryRequest));
            

        [HttpDelete("[action]/{Id}/")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest, [FromQuery] string ImageId)
        {
            removeProductImageCommandRequest.ImageId = ImageId;
            return Ok(await _mediator.Send(removeProductImageCommandRequest));
        }
    }
}
