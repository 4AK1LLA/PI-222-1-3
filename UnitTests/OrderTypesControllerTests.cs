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
    public class OrderTypesControllerTests
    {
        private readonly OrderTypesController _controller;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IDataRepository<OrderType>> _mockRepo;

        public OrderTypesControllerTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IDataRepository<OrderType>>();
            _mockUow.SetupGet(uow => uow.OrderTypeRepository).Returns(_mockRepo.Object);

            _controller = new OrderTypesController(_mockUow.Object);
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
                .ReturnsAsync(null as OrderType);

            var result = await _controller.Edit(wrongId, GetValidItem());

            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenDeletedSuccessfully()
        {
            int id = 10;

            _mockRepo?
                .Setup(a => a.GetAsync(id))
                .ReturnsAsync(new OrderType { OrderTypeId = id });

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            result.ActionName.Should().Be("Index");
        }

        private OrderType GetValidItem() => new OrderType
        {
            OrderTypeId = 100,
            OrderName = "name"
        };
    }
}