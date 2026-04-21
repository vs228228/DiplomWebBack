using DiplomWebBack.Application.Services.Interfaces;
using DiplomWebBack.Application.Usecases.Command.User;
using DiplomWebBack.Domain.CustomExceptions;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Domain.Repos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DiplomWebBack.Application.Usecases.CommandHandlers.User
{
    public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, Unit>
    {
        private readonly IUserVerificationService _userVerification;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;

        public UploadAvatarCommandHandler(
            IUserVerificationService userVerification,
            IUserRepository userRepository,
            IFileService fileService)
        {
            _userVerification = userVerification;
            _userRepository = userRepository;
            _fileService = fileService;
        }

        public async Task<Unit> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
        {
            var user = await _userVerification.CheckIfUserValidAndGetAsync(request.UserId, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            if (request.File is null || request.File.Length == 0)
            {
                throw new BadRequestException("File is empty");
            }

            var fileName = $"{user.Id.ToString()}_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.jpg";

            var userAvatarPath = await _fileService.SaveFilesAsync(new List<IFormFile> { request.File }, new List<string> { fileName });

            user.Avatar = userAvatarPath[0];

            await _userRepository.UpdateAsync(user, cancellationToken);

            return Unit.Value;
        }
    }
}
