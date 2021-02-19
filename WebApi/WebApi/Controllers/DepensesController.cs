using Application.Features.DepenseFeatures.Commands;
using Application.Features.DepenseFeatures.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class DepensesController : BaseApiController
    {
        public DepensesController(IMediator mediator) : base(mediator)
        {

        }

        /// <summary>
        /// Creates a New Depense.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepenseCommand command)
        {
            var item = await Mediator.Send(command);

            if (item == 0)
            {
                return BadRequest();
            }

            return Ok(item);
        }


        /// <summary>
        /// Gets all Depenses.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(string orderByDateOrMontant = null)
        {

            return Ok(await Mediator.Send(new GetAllDepensesQuery { OrderByDateOrMontant = orderByDateOrMontant }));
        }


        /// <summary>
        /// Gets Depense Entity by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var item = await Mediator.Send(new GetDepenseByIdQuery { Id = id });

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);

        }


        /// <summary>
        /// Deletes Depense Entity based on Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await Mediator.Send(new DeleteDepenseByIdCommand { Id = id });

            if (item == 0)
            {
                return NotFound();
            }
            return Ok(item);
        }


        /// <summary>
        /// Updates the Depense Entity based on Id.   
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateDepenseCommand command)
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