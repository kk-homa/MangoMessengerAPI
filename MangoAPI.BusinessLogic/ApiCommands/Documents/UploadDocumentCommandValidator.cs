﻿using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace MangoAPI.BusinessLogic.ApiCommands.Documents
{
    public class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
    {
        public UploadDocumentCommandValidator()
        {
            var allowedExtensions = new List<string>
            {
                ".jpg", ".JPG", ".txt", ".TXT", ".pdf", ".PDF", ".gif", ".GIF"
            };

            RuleFor(x => x.FormFile).NotEmpty();
            RuleFor(x => x.FormFile.Length).LessThanOrEqualTo(10 * 1024 * 1024);
            RuleFor(x => x.FormFile.FileName).Must(t => allowedExtensions.Any(t.Contains));
        }
    }
}