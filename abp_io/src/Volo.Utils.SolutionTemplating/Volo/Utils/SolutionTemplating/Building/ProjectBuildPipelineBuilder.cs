﻿using Volo.Utils.SolutionTemplating.Building.Steps;

namespace Volo.Utils.SolutionTemplating.Building
{
    public static class ProjectBuildPipelineBuilder
    {
        public static ProjectBuildPipeline Build(ProjectBuildContext context)
        {
            var pipeline = new ProjectBuildPipeline();

            pipeline.Steps.Add(new GithubDownloadStep());
            pipeline.Steps.Add(new FileEntryListReadStep());
            pipeline.Steps.AddRange(context.Template.GetCustomSteps(context));
            pipeline.Steps.Add(new NugetReferenceReplaceStep());
            pipeline.Steps.Add(new TemplateCodeDeleteStep());
            pipeline.Steps.Add(new SolutionRenameStep());
            pipeline.Steps.Add(new CreateProjectResultZipStep());

            return pipeline;
        }
    }
}
