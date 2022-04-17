using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.Application.Features.Queries.Cities.GetAllCitiesQuery;
using Identity.Application.Features.Queries.Cities.GetDistrictsOfCityQuery;
using Identity.Application.Features.Queries.Cities.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/v1/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CityViewModel>))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cities = await _mediator.Send(new GetAllCitiesQuery());
            return Ok(cities.Data);
        }
        
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CityViewModel>))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("{id:guid}/districts")]
        public async Task<IActionResult> GetDistrictsOfCity(Guid id)
        {
            var districts = await _mediator.Send(new GetDistrictsOfCityQuery(id));
            return Ok(districts.Data);
        }
    }
}