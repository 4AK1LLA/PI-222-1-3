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
    public class ManagersControllerTests
    {
        private readonly ManagersController _controller;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IDataRepository<Manager>> _mockRepo;

        public ManagersControllerTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IDataRepository<Manager>>();
            _mockUow.SetupGet(uow => uow.ManagerRepository).Returns(_mockRepo.Object);

            _controller = new ManagersController(_mockUow.Object);
        }

        [Fact]
        public async Task Details_ShouldReturnView_WhenIdIsValid()
        {
            int id = 10;

            _mockRepo?
                .Setup(a => a.GetAsync(id))
                .ReturnsAsync(new Manager { ManagerId = id });

            var item = await _controller.Details(id);

            var result = item.Result as ViewResult;
            var model = result.Model as Manager;

            model.Should().BeOfType(typeof(Manager));
            model.ManagerId.Should().Be(id);
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
                .ReturnsAsync(null as Manager);

            var result = await _controller.Edit(wrongId, GetValidItem());

            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenDeletedSuccessfully()
        {
            int id = 10;

            _mockRepo?
                .Setup(a => a.GetAsync(id))
                .ReturnsAsync(new Manager { ManagerId = id });

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            result.ActionName.Should().Be("Index");
        }

        private Manager GetValidItem() => new Manager
        {
            ManagerId = 100,
            FullName = "name",
            Telephone = "123"
        };
    }
}