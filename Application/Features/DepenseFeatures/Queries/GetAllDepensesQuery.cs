using Application.Extensions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DepenseFeatures.Queries
{
    public class GetAllDepensesQuery : IRequest<IEnumerable<Depense>>
    {
        public string OrderByDateOrMontant { get; set; }
        public class GetAllDepensesQueryHandler : IRequestHandler<GetAllDepensesQuery, IEnumerable<Depense>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllDepensesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Depense>> Handle(GetAllDepensesQuery query, CancellationToken cancellationToken)
            {
                if (!(query.OrderByDateOrMontant == "Date" || query.OrderByDateOrMontant == "Montant" || string.IsNullOrEmpty(query.OrderByDateOrMontant)))
                {
                    throw new ArgumentException("OrderBy must be Date , Montant or null");
                }

                List<Depense> DepenseList = null;

                if (!string.IsNullOrEmpty(query.OrderByDateOrMontant))
                {
                    DepenseList = await _context.Depenses
                   .OrderByPropertyName(query.OrderByDateOrMontant)
                   .ToListAsync();
                }
                else
                {
                    DepenseList = await _context.Depenses
                   .ToListAsync();
                }

                return DepenseList?.AsReadOnly();
            }
        }
    }
}
