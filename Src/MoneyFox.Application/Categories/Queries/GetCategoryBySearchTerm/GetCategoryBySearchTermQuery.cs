﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Categories.Queries.GetCategoryBySearchTerm
{
    public class GetCategoryBySearchTermQuery : IRequest<List<Category>>
    {
        public GetCategoryBySearchTermQuery(string searchTerm = "")
        {
            SearchTerm = searchTerm;
        }

        public string SearchTerm { get; }

        public class Handler : IRequestHandler<GetCategoryBySearchTermQuery, List<Category>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<List<Category>> Handle(GetCategoryBySearchTermQuery request, CancellationToken cancellationToken)
            {
                IOrderedQueryable<Category> categoriesQuery = contextAdapter.Context
                                                                            .Categories
                                                                            .OrderBy(x => x.Name);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                    return await categoriesQuery.WhereNameContains(request.SearchTerm).ToListAsync(cancellationToken);

                return await categoriesQuery.ToListAsync(cancellationToken);
            }
        }
    }
}
