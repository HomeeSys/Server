using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Raports.Application.Handlers.Read;

public record DownloadRaportCommand(int RaportID) : IRequest<DownloadRaportResponse>;
public record DownloadRaportResponse(Stream FileStream, string FileName, string ContentType);

public class DownloadRaportCommandValidator : AbstractValidator<DownloadRaportCommand>
{
    public DownloadRaportCommandValidator()
    {
        RuleFor(x => x.RaportID).GreaterThan(0);
    }
}
