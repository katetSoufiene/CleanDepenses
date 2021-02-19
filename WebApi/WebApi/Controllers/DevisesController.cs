using Application.Features.DeviseFeatures.Commands;
using Application.Features.DeviseFeatures.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class DevisesController : BaseApiController
    {
        public DevisesController(IMediator mediator) : base(mediator)
        {

        }

        /// <summary>
        /// Creates a New Devise.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateDeviseCommand command)
        {

            var itemId = await Mediator.Send(command);

            if (itemId == 0)
            {
                return BadRequest();
            }

            return Ok(itemId);

        }


        /// <summary>
        /// Gets all Devises.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await Mediator.Send(new GetAllDevisesQuery());
            if (items == null)
            {
                return BadRequest();
            }

            return Ok(items);
        }


        /// <summary>
        /// Gets Devise Entity by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var item = await Mediator.Send(new GetDeviseByIdQuery { Id = id });

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }


        /// <summary>
        /// Deletes Devise Entity based on Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await Mediator.Send(new DeleteDeviseByIdCommand { Id = id });

            if (item == 0)
            {
                return NotFound();
            }
            return Ok(item);
        }


        /// <summary>
        /// Updates the Devise Entity based on Id.   
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<IActionResult> Update(int id, UpdateDeviseCommand command)
        {

            if (id != command.Id)
            {
                return BadRequest();
            }

            var item = await Mediator.Send(command);

            if (item == 0)
            {
                return NotFound();
            }
            return Ok(item);
        }
    }

}