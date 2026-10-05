using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Features.Products.Commands.CreateProduct;
using Catalog.Application.Interfaces;
using Catalog.Application.Mappings;
using Catalog.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Catalog.Tests.Features.Products;

[TestFixture]
public class CreateProductCommandHandlerTests
{
    private Mock<IProductRepository> _productRepoMock = null!;
    private Mock<ICategoryRepository> _categoryRepoMock = null!;
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private IMapper _mapper = null!;
    private CreateProductCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _categoryRepoMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _unitOfWorkMock.Setup(u => u.Categories).Returns(_categoryRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Products).Returns(_productRepoMock.Object);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>(), NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();

        _handler = new CreateProductCommandHandler(_unitOfWorkMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_ValidCommand_ReturnsProductDto()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var category = Category.Create("Electronics", null!);
        var command = new CreateProductCommand("Laptop", "A great laptop", 999.99m, 3, categoryId);

        _categoryRepoMock
            .Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync(category);
        _productRepoMock
            .Setup(r => r.GetByNameAsync("Laptop"))
            .ReturnsAsync((Product?)null);
        _productRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Product>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Laptop"));
        Assert.That(result.Price, Is.EqualTo(999.99m));
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_CategoryNotFound_ThrowsInvalidOperationException()
    {
        var command = new CreateProductCommand("Laptop", null!, 999m, 1, Guid.NewGuid());

        _categoryRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Category?)null);

        Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public void Handle_DuplicateName_ThrowsInvalidOperationException()
    {
        var categoryId = Guid.NewGuid();
        var category = Category.Create("Electronics", null!);
        var command = new CreateProductCommand("Laptop", null!, 999m, 1, categoryId);

        _categoryRepoMock
            .Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync(category);
        _productRepoMock
            .Setup(r => r.GetByNameAsync("Laptop"))
            .ReturnsAsync(Product.Create("Laptop", "A great laptop", 999.99m, 10, categoryId));

        Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
