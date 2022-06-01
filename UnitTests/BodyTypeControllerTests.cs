using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using BLL.Controllers;
using DAL.Models;

#nullable disable

namespace UnitTests
{
    public class BodyTypeControllerTests
    {
        private readonly BodyTypesController _btController;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IDataRepository<BodyType>> _mockRepo;

        public BodyTypeControllerTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IDataRepository<BodyType>>();
            _mockUow.SetupGet(uow => uow.BodyTypeRepository).Returns(_mockRepo.Object);

            _btController = new BodyTypesController(_mockUow.Object);
        }

        [Fact]
        public async Task Details_ShouldReturnView_WhenIdIsValid()
        {
            int id = 10;

            _mockRepo?
                .Setup(bt => bt.GetAsync(id))
                .ReturnsAsync(new BodyType { BodyTypeId = id });

            var bodyType = await _btController.Details(id);

            var result = bodyType.Result as ViewResult;
            var model = result.Model as BodyType;

            model.Should().BeOfType(typeof(BodyType));
            model.BodyTypeId.Should().Be(id);
        }

        [Fact]
        public async Task Create_ShouldRedirectToIndex_WithValidBodyType()
        {
            var bodyType = await _btController.Create(GetValidBodyType());

            var result = bodyType as RedirectToActionResult;

            result.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WithWrongId()
        {
            int wrongId = -1;

            _mockRepo?
                .Setup(bt => bt.GetAsync(wrongId))
                .ReturnsAsync(null as BodyType);

            var result = await _btController.Edit(wrongId, GetValidBodyType());

            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenBodyTypeDeletedSuccessfully()
        {
            int id = 10;

            _mockRepo?
                .Setup(bt => bt.GetAsync(id))
                .ReturnsAsync(new BodyType { BodyTypeId = id });

            var result = await _btController.DeleteConfirmed(id) as RedirectToActionResult;

            result.ActionName.Should().Be("Index");
        }

        private BodyType GetValidBodyType() => new BodyType
        {
            BodyTypeId = 100,
            BodyTypeNames = "body type name"
        };
    }
}