using E_TicaretAPI.Application.ViewModels.Products;
using FluentValidation;


namespace E_TicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().NotNull()
                    .WithMessage("Lütfen ürün adını giriniz... ")
                .MaximumLength(150).MinimumLength(5)
                    .WithMessage("5 ile 150 arasında bir karakter giriniz...");

            RuleFor(p => p.Stock)
                .NotEmpty().NotNull()
                    .WithMessage("Lütfen stok bilgisi giriniz...!")
                .Must(s => s > 0)
                    .WithMessage("Stok bilgisi 0 dan küçük olamaz... ");


            RuleFor(p => p.Price)
              .NotEmpty().NotNull()
                  .WithMessage("Lütfen fiyat bilgisi giriniz...!")
              .Must(s => s > 0)
                  .WithMessage("Fiyat bilgisi 0 dan küçük olamaz... ");

        }

    }
}
