using StickedWords.API.Models;
using StickedWords.Application.Commands.Images;

namespace StickedWords.API.Mappers;

internal static class ImageMapper
{
    public static CreateImageCommand ToCommand(this CreateImageRequestDto source) =>
        new(source.Base64Data);
}
