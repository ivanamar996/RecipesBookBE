using FluentNHibernate.Conventions.Inspections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RecepiesBook.Controllers;
using RecepiesBook.Models;
using RecepiesBook.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecepiesBook.Tests
{
    [TestFixture]
    public class RecepieControllerTests
    {
        private Mock<IRecepieService> _service;
        private RecepiesController _controller;
        private Recepie _recepie;


        [SetUp]
        public void SetUp()
        {
            _service = new Mock<IRecepieService>();
            _controller = new RecepiesController(_service.Object);
            _recepie = new Recepie();
        }

        [Test]
        public void Get_WhenCalled_ShouldReturnListOfRecepies()
        {
            var result = _controller.Get();

            _service.Verify(s => s.GetAllRecepies(),Times.Once);
            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void GetRecepieById_IdDoesNotExistsInDb_ShouldReturnBadRequest()
        {
            _service.Setup(s => s.GetRecepieById(1)).Returns((Recepie)null);

            var result = _controller.GetRecepieById(1);

            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

        }

        [Test]
        public void GetRecepieById_IdExistsInDb_ShouldReturnOk()
        {
            _recepie.Id = 1;
            _service.Setup(s => s.GetRecepieById(1)).Returns(_recepie);

            var result = _controller.GetRecepieById(1) as OkObjectResult;

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(result.Value, Is.EqualTo(_recepie));

        }

        [Test]
        public void Post_ModelStateIsNotValid_ShouldReturnBadRequest()
        {
            _controller.ModelState.AddModelError("test", "test");

            var result = _controller.Post(_recepie);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

        }

        [Test]
        public void Post_ModelStateIsValid_ShouldReturnCreatedAtAction()
        {
            var result = _controller.Post(_recepie) as CreatedAtActionResult;

            _service.Verify(s => s.CreateNewRecepie(_recepie));
            Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
            Assert.That(result.Value, Is.EqualTo(_recepie));
        }

        [Test]
        public void Update_ModelStateIsNotValid_ShouldReturnBadRequest()
        {
            _controller.ModelState.AddModelError("test", "test");

            var result = _controller.Update(1,_recepie);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Update_SuccessfullyUpdated_ShouldReturnNoContent()
        {
            _service.Setup(s => s.UpdateRecepie(1, _recepie)).Returns(true);

            var result = _controller.Update(1, _recepie);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public void Update_IdDoesNotExistsInDb_ShouldReturnNoFound()
        {
            _service.Setup(s => s.UpdateRecepie(1, _recepie)).Returns(false);

            var result = _controller.Update(1, _recepie);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Delete_SuccessfullyDeleted_ShouldReturnOk()
        {
            _service.Setup(s => s.DeleteRecepie(1)).Returns(true);

            var result = _controller.Delete(1);

            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public void Delete_IdDoesNotExistsInDb_ShouldReturnNotFound()
        {
            _service.Setup(s => s.DeleteRecepie(1)).Returns(false);

            var result = _controller.Delete(1);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}
