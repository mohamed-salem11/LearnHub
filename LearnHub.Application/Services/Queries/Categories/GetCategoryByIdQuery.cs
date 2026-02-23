using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Application.Services.Queries.Categories
{ 
    public record GetCategoryByIdQuery(int Id) : IRequest<Category?>;
    
}
