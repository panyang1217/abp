﻿using JetBrains.Annotations;
using Volo.Abp;
using Volo.Utils.SolutionTemplating.Files;

namespace Volo.Utils.SolutionTemplating.Building
{
    public class ProjectBuildContext
    {
        public ProjectBuildRequest Request { get; }

        public string TemplatesFolder { get; }

        public TemplateInfo Template { get; }

        public FileEntryList Files { get; set; }

        public ProjectResult Result { get; set; }
        
        public ProjectBuildContext(
            [NotNull] TemplateInfo template,
            ProjectBuildRequest request,
            string templatesFolder)
        {
            Template = Check.NotNull(template, nameof(template));
            Request = request;
            TemplatesFolder = templatesFolder;
            Result = new ProjectResult();
        }
    }
}