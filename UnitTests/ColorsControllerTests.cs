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
    public class ColorsControllerTests
    {
        private readonly ColorsController _clController;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IDataRepository<Color>> _mockRepo;

        public ColorsControllerTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IDataRepository<Color>>();
            _mockUow.SetupGet(uow => uow.ColorRepository).Returns(_mockRepo.Object);

            _clController = new ColorsController(_mockUow.Object);
        }

        [Fact]
        public async Task Details_ShouldReturnView_WhenIdIsValid()
        {
            int id = 10;

            _mockRepo?
                .Setup(cl => cl.GetAsync(id))
                .ReturnsAsync(new Color { ColorId = id });

            var color = await _clController.Details(id);

            var result = color.Result as ViewResult;
            var model = result.Model as Color;

            model.Should().BeOfType(typeof(Color));
            model.ColorId.Should().Be(id);
        }

        [Fact]
        public async Task Create_ShouldRedirectToIndex_WithValidColor()
        {
            var color = await _clController.Create(GetValidColor());

            var result = color as RedirectToActionResult;

            result.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WithWrongId()
        {
            int wrongId = -1;

            _mockRepo?
                .Setup(bt => bt.GetAsync(wrongId))
                .ReturnsAsync(null as Color);

            var result = await _clController.Edit(wrongId, GetValidColor());

            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenColorDeletedSuccessfully()
        {
            int id = 10;

            _mockRepo?
                .Setup(bt => bt.GetAsync(id))
                .ReturnsAsync(new Color { ColorId = id });

            var result = await _clController.DeleteConfirmed(id) as RedirectToActionResult;

            result.ActionName.Should().Be("Index");
        }

        private Color GetValidColor() => new Color
        {
            ColorId = 100,
            ColorName = "color name"
        };
    }
}