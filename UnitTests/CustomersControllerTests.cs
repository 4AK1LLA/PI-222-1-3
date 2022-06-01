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
    public class CustomersControllerTests
    {
        private readonly CustomersController _controller;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IDataRepository<Customer>> _mockRepo;

        public CustomersControllerTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IDataRepository<Customer>>();
            _mockUow.SetupGet(uow => uow.CustomerRepository).Returns(_mockRepo.Object);

            _controller = new CustomersController(_mockUow.Object);
        }

        [Fact]
        public async Task Details_ShouldReturnView_WhenIdIsValid()
        {
            int id = 10;

            _mockRepo?
                .Setup(a => a.GetAsync(id))
                .ReturnsAsync(new Customer { CustomerId = id });

            var item = await _controller.Details(id);

            var result = item.Result as ViewResult;
            var model = result.Model as Customer;

            model.Should().BeOfType(typeof(Customer));
            model.CustomerId.Should().Be(id);
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
                .ReturnsAsync(null as Customer);

            var result = await _controller.Edit(wrongId, GetValidItem());

            result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenDeletedSuccessfully()
        {
            int id = 10;

            _mockRepo?
                .Setup(a => a.GetAsync(id))
                .ReturnsAsync(new Customer { CustomerId = id });

            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            result.ActionName.Should().Be("Index");
        }

        private Customer GetValidItem() => new Customer
        {
            CustomerId = 100,
            Email = "email",
            FullName = "name",
            Telephone = "123"
        };
    }
}