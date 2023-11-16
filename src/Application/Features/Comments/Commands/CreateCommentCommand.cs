using Application.Features.Comments.Commands;
using Application.Responses;
using FluentValidation;
using MediatR;

namespace Application.Features.Comments.Commands
{
    public class CreateCommentCommand : IRequest<Result<Guid>>
    {
        public Guid BlogId { get; set; }
        public string Description { get; set; }
    }
}


public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(p => p.BlogId).NotNull().NotEmpty();
        RuleFor(p => p.Description).NotEmpty();
    }
}
