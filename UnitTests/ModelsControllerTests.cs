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
    public class ModelsControllerTests
    {
        private readonly ModelsController _controller;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IDataRepository<Model>> _mockRepo;

        public ModelsControllerTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IDataRepository<Model>>();
            _mockUow.SetupGet(uow => uow.ModelRepository).Returns(_mockRepo.Object);

            _controller = new ModelsController(_mockUow.Object);
        }

        [Fact]
        public async Task Details_ShouldReturnView_WhenIdIsValid()
        {
            int id = 10;

            _mockRepo?
                .Setup(a => a.GetAsync(id))
                .ReturnsAsync(new Model { ModelId = id });

            var item = await _controller.Details(id);

            var result = item.Result as ViewResult;
            var model = result.Model as Model;

            model.Should().BeOfType(typeof(Model));
            model.ModelId.Should().Be(id);
        }

        [Fact]
        public async Task Create_ShouldRedirectToIndex_WithValidColor()
        {
            var item = await _controller.Create(GetValidItem());

            var result = item as RedirectToActionResult;

            result.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WithWrongId()
        {
            int wrongId = -1;

            _mockRepo?
                .Setup(a => a.GetAsync(wrongId))
                .ReturnsAsync(null as Model);

            var result = await _controller.Edit(wrongId, GetValidItem());

            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenDeletedSuccessfully()
        {
            int id = 10;

            _mockRepo?
                .Setup(a => a.GetAsync(id))
                .ReturnsAsync(new Model { ModelId = id });

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            result.ActionName.Should().Be("Index");
        }

        private Model GetValidItem() => new Model
        {
            ModelId = 100,
            ModelName = "name",
            Cars = null
        };
    }
}