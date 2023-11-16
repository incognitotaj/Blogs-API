using Application.Responses;
using FluentValidation;
using MediatR;

namespace Application.Features.Blogs.Commands;

public class DeleteBlogCommand : IRequest<Result<Unit>>
{
    public Guid BlogId { get; set; }
}


public class DeleteBlogCommandValidator : AbstractValidator<DeleteBlogCommand>
{
    public DeleteBlogCommandValidator()
    {
        RuleFor(b => b.BlogId).NotNull().NotEmpty();
    }
}