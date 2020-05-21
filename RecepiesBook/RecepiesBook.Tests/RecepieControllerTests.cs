using FluentNHibernate.Conventions.Inspections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RecepiesBook.Controllers;
using RecipesBook.Models;
using RecipesBook.Services;
using System;
using System.Collections.Generic;
using System.Text;
using RecipesBook.Controllers;

namespace RecipesBook.Tests
{
    [TestFixture]
    public class RecipeControllerTests
    {
        private Mock<IRecipeService> _service;
        private RecipesController _controller;
        private Recipe _recipe;


        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IRecipeService>();
            _controller = new RecipesController(_service.Object);
            _recipe = new Recipe();
        }

        [Test]
        public void Get_WhenCalled_ShouldReturnListOfRecipes()
        {
            var result = _controller.Get();

            _service.Verify(s => s.GetAllRecipes(),Times.Once);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void GetRecipeById_IdDoesNotExistsInDb_ShouldReturnBadRequest()
        {
            _service.Setup(s => s.GetRecipeById(1)).Returns((Recipe)null);

            var result = _controller.GetRecipeById(1);

            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

        }

        [Test]
        public void GetRecipeById_IdExistsInDb_ShouldReturnOk()
        {
            _recipe.Id = 1;
            _service.Setup(s => s.GetRecipeById(1)).Returns(_recipe);

            var result = _controller.GetRecipeById(1) as OkObjectResult;

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(result.Value, Is.EqualTo(_recipe));

        }

        [Test]
        public void Post_ModelStateIsNotValid_ShouldReturnBadRequest()
        {
            _controller.ModelState.AddModelError("test", "test");

            var result = _controller.Post(_recipe);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

        }

        [Test]
        public void Post_ModelStateIsValid_ShouldReturnCreatedAtAction()
        {
            var result = _controller.Post(_recipe) as CreatedAtActionResult;

            _service.Verify(s => s.CreateNewRecipe(_recipe));
            Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
            Assert.That(result.Value, Is.EqualTo(_recipe));
        }

        [Test]
        public void Update_ModelStateIsNotValid_ShouldReturnBadRequest()
        {
            _controller.ModelState.AddModelError("test", "test");

            var result = _controller.Update(1,_recipe);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Update_SuccessfullyUpdated_ShouldReturnNoContent()
        {
            _service.Setup(s => s.UpdateRecipe(1, _recipe)).Returns(true);

            var result = _controller.Update(1, _recipe);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public void Update_IdDoesNotExistsInDb_ShouldReturnNoFound()
        {
            _service.Setup(s => s.UpdateRecipe(1, _recipe)).Returns(false);

            var result = _controller.Update(1, _recipe);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Delete_SuccessfullyDeleted_ShouldReturnOk()
        {
            _service.Setup(s => s.DeleteRecipe(1)).Returns(true);

            var result = _controller.Delete(1);

            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public void Delete_IdDoesNotExistsInDb_ShouldReturnNotFound()
        {
            _service.Setup(s => s.DeleteRecipe(1)).Returns(false);

            var result = _controller.Delete(1);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}
