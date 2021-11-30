using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Catalog.Application.Features.Commands.Products.CreateProductCommand;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public void asdas(IFormFile file)
        {
            _mediator.Send(new CreateProductCommand()
            {
                FileList = new List<IFormFile>()
                {
                    file
                }
            });
        }
    }
}