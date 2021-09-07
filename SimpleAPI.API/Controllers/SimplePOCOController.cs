using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SimpleAPI.Data.Entities;
using SimpleAPI.Data.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DTO = SimpleAPI.Public.DTO;

namespace SimpleAPI.API.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class SimplePOCOController : ControllerBase
	{
		private readonly IUoW UoW;
		private readonly IMapper Mapper;
		private readonly ILogger<SimplePOCOController> _logger;

		public SimplePOCOController(IUoW uow, IMapper mapper, ILogger<SimplePOCOController> logger)
		{
			UoW = uow;
			Mapper = mapper;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var predicate = PredicateBuilder.New<SimplePOCO>(true);

			var recordList = await UoW.SimpleRepository.GetMatching(predicate, "MyChildren", "TravelerName");
			var result = recordList.Select(e => Mapper.Map<DTO.SimplePOCO>(e)).ToList();
			return Ok(result);
		}

		[HttpGet(Name="GetSimplePOCO")]
		public async Task<IActionResult> GetOne(Guid id)
		{
			var predicate = PredicateBuilder.New<SimplePOCO>(e => e.Id == id);

			var recordList = await UoW.SimpleRepository.GetMatching(predicate, "MyChildren", "");
			if ((recordList?.Count ?? 0) > 0)
			{
				var result = recordList.Select(e => Mapper.Map<DTO.SimplePOCO>(e)).FirstOrDefault();
				return Ok(result);
			}
			else
				return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> CreateOne(DTO.SimplePOCO newRecord)
		{
			if (newRecord == null)
			{
				//TODO: Build Error Message(s)
				return BadRequest();
			}

			var recordToSave = Mapper.Map<SimplePOCO>(newRecord);
			if (!recordToSave.IsValid)
            {
				//TODO: Build Error Message(s)
				return BadRequest();
            }

			var result = await UoW.SimpleRepository.Create(recordToSave);
			var changesSaved = await UoW.SaveChangesAsync();
			if (result != null && changesSaved)
			{
				return CreatedAtAction("GetSimplePOCO", result.Id, Mapper.Map<DTO.SimplePOCO>(result));
			}
			else
			{
				//TODO: Probable database error, build error messages
				return BadRequest();
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateOne(DTO.SimplePOCO changes)
		{
			if (changes == null)
			{
				//TODO: Build Error Message(s)
				return BadRequest();
			}

			var recordToSave = Mapper.Map<SimplePOCO>(changes);
			if (!recordToSave.IsValid)
			{
				//TODO: Build Error Message(s)
				return BadRequest();
			}

			var changesSaved = await UoW.SimpleRepository.Update(recordToSave);
			changesSaved = changesSaved && await UoW.SaveChangesAsync();
			if (changesSaved)
			{
				return Ok();
			}
			else
			{
				//TODO: Probable database error, build error messages
				return BadRequest();
			}
		}
	}
}
