using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Application.Services.Commands.Categories
{
     public record CreateCategoryCommand(Category Category, IFormFile ImageFile)
            : IRequest<(bool Success, string? Error, Category? Data)>;
     
}
