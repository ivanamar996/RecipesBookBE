using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RecipesBook.Controllers;
using RecipesBook.Models;
using RecipesBook.Services;

namespace RecipesBook.Tests
{
    [TestFixture]
    public class ShoppingListsControllerTests
    {
        private Mock<IShoppingListService> _service;
        private ShoppingListsController _controller;
        private ShoppingList _shoppingList;


        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IShoppingListService>();
            _controller = new ShoppingListsController(_service.Object);
            _shoppingList = new ShoppingList();
        }

        [Test]
        public void Get_WhenCalled_ShouldReturnListOfShoppingLists()
        {
            var result = _controller.Get();

            _service.Verify(s => s.GetAllSoppingLists(), Times.Once);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void GetShoppingListById_IdDoesNotExistsInDb_ShouldReturnBadRequest()
        {
            _service.Setup(s => s.GetShoppingListById(1)).Returns((ShoppingList)null);

            var result = _controller.GetShoppingListById(1);

            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

        }

        [Test]
        public void GetShoppingListById_IdExistsInDb_ShouldReturnOk()
        {
            _shoppingList.Id = 1;
            _service.Setup(s => s.GetShoppingListById(1)).Returns(_shoppingList);

            var result = _controller.GetShoppingListById(1) as OkObjectResult;

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(result.Value, Is.EqualTo(_shoppingList));

        }

        [Test]
        public void Post_ModelStateIsNotValid_ShouldReturnBadRequest()
        {
            _controller.ModelState.AddModelError("test", "test");

            var result = _controller.Post(_shoppingList);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

        }

        [Test]
        public void Post_ModelStateIsValid_ShouldReturnCreatedAtAction()
        {
            var result = _controller.Post(_shoppingList) as CreatedAtActionResult;

            _service.Verify(s => s.CreateNewShoppingList(_shoppingList));
            Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
            Assert.That(result.Value, Is.EqualTo(_shoppingList));
        }

        [Test]
        public void Update_ModelStateIsNotValid_ShouldReturnBadRequest()
        {
            _controller.ModelState.AddModelError("test", "test");

            var result = _controller.Update(1, _shoppingList);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Update_SuccessfullyUpdated_ShouldReturnNoContent()
        {
            _service.Setup(s => s.UpdateShoppingList(1, _shoppingList)).Returns(true);

            var result = _controller.Update(1, _shoppingList);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public void Update_IdDoesNotExistsInDb_ShouldReturnNoFound()
        {
            _service.Setup(s => s.UpdateShoppingList(1, _shoppingList)).Returns(false);

            var result = _controller.Update(1, _shoppingList);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void AddIngredientsFromRecipeToSl_ModelStateIsNotValid_ShouldReturnBadRequest()
        {
            _controller.ModelState.AddModelError("test", "test");

            var result = _controller.AddIngredientsFromRecepieToSl(1, 2);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void AddIngredientsFromRecipeToSl_SuccessfullyUpdated_ShouldReturnNoContent()
        {
            _service.Setup(s => s.AddIngredientsFromRecipeToSl(1, 2)).Returns(true);

            var result = _controller.AddIngredientsFromRecepieToSl(1, 2);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public void AddIngredientsFromRecipeToSl_IdDoesNotExistsInDb_ShouldReturnNoFound()
        {
            _service.Setup(s => s.AddIngredientsFromRecipeToSl(1, 2)).Returns(false);

            var result = _controller.AddIngredientsFromRecepieToSl(1, 2);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Delete_SuccessfullyDeleted_ShouldReturnOk()
        {
            _service.Setup(s => s.DeleteShoppingList(1)).Returns(true);

            var result = _controller.Delete(1);

            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public void Delete_IdDoesNotExistsInDb_ShouldReturnNotFound()
        {
            _service.Setup(s => s.DeleteShoppingList(1)).Returns(false);

            var result = _controller.Delete(1);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}
