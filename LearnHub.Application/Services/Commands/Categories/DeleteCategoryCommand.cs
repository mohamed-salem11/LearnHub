using LearnHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Application.Services.Commands.Categories
{
    public record DeleteCategoryCommand(int Id) : IRequest<Unit>;

}
