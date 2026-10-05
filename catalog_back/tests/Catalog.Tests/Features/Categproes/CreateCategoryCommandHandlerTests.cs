using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Features.Categories.Commands.CreateCategory;
using Catalog.Application.Interfaces;
using Catalog.Application.Mappings;
using Catalog.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Catalog.Tests.Features.Categories;

[TestFixture]
public class CreateCategoryCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<ICategoryRepository> _categoryRepoMock = null!;
    private IMapper _mapper = null!;
    private CreateCategoryCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _categoryRepoMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(u => u.Categories).Returns(_categoryRepoMock.Object);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<CategoryProfile>(), NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();

        _handler = new CreateCategoryCommandHandler(_unitOfWorkMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_ValidCommand_ReturnsCategoryDto()
    {
        // Arrange
        var command = new CreateCategoryCommand("Electronics", "Electronic devices", true);

        _categoryRepoMock
            .Setup(r => r.GetByNameAsync("Electronics", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);
        _categoryRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Category>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Electronics"));
        Assert.That(result.IsActive, Is.True);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_DuplicateName_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new CreateCategoryCommand("Electronics", null!, true);

        _categoryRepoMock
            .Setup(r => r.GetByNameAsync("Electronics", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Category.Create("Electronics", null!));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
