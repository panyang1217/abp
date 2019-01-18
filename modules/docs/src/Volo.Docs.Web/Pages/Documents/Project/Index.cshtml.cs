<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Docs.Documents;
using Volo.Docs.Formatting;
using Volo.Docs.Projects;

namespace Volo.Docs.Pages.Documents.Project
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ProjectName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Version { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string DocumentName { get; set; }

        public string ProjectFormat { get; private set; }

        public string DocumentNameWithExtension { get; private set; }

        public DocumentWithDetailsDto Document { get; private set; }

        public List<VersionInfo> Versions { get; private set; }

        public List<SelectListItem> VersionSelectItems { get; private set; }

        public NavigationWithDetailsDto Navigation { get; private set; }

        public VersionInfo LatestVersionInfo { get; private set; }

        private readonly IDocumentAppService _documentAppService;
        private readonly IDocumentConverterFactory _documentConverterFactory;
        private readonly IProjectAppService _projectAppService;

        public IndexModel(IDocumentAppService documentAppService, IDocumentConverterFactory documentConverterFactory, IProjectAppService projectAppService)
        {
            _documentAppService = documentAppService;
            _documentConverterFactory = documentConverterFactory;
            _projectAppService = projectAppService;
        }

        public async Task OnGet()
        {
            var project = await _projectAppService.FindByShortNameAsync(ProjectName);

            SetPageParams(project);

            await SetVersionAsync(project);

            await SetDocumentAsync();

            await SetNavigationAsync();
        }

        private async Task SetNavigationAsync()
        {
            Navigation = await _documentAppService.GetNavigationDocumentAsync(ProjectName, Version, false);
            Navigation.ConvertItems();
        }

        private void SetPageParams(ProjectDto project)
        {
            ProjectFormat = project.Format;

            if (DocumentName.IsNullOrWhiteSpace())
            {
                DocumentName = project.DefaultDocumentName;
            }

            DocumentNameWithExtension = DocumentName + "." + project.Format;
        }

        private async Task SetVersionAsync(ProjectDto project)
        {
            Versions = (await _documentAppService
                .GetVersions(project.ShortName, project.DefaultDocumentName, project.ExtraProperties,
                    project.DocumentStoreType, DocumentNameWithExtension))
                    .Select(v => new VersionInfo(v, v)).ToList();

            LatestVersionInfo = GetLatestVersion();

            if (string.Equals(Version, DocsAppConsts.LatestVersion, StringComparison.OrdinalIgnoreCase))
            {
                LatestVersionInfo.IsSelected = true;
                Version = LatestVersionInfo.Version;
            }
            else
            {
                var versionFromUrl = Versions.FirstOrDefault(v => v.Version == Version);
                if (versionFromUrl != null)
                {
                    versionFromUrl.IsSelected = true;
                    Version = versionFromUrl.Version;
                }
                else
                {
                    Versions.First().IsSelected = true;
                    Version = Versions.First().Version;
                }
            }

            VersionSelectItems = Versions.Select(v => new SelectListItem
            {
                Text = v.DisplayText,
                Value = CreateLink(v.Version, DocumentName),
                Selected = v.IsSelected
            }).ToList();
        }

        public string CreateLink(string version, string documentName = null)
        {
            var link = "/" + DocsAppConsts.WebsiteLinkFirstSegment + "/" + ProjectName + "/" + version;

            if (documentName != null)
            {
                link += "/" + DocumentName;
            }

            return link;
        }

        private VersionInfo GetLatestVersion()
        {
            var latestVersion = Versions.First();

            latestVersion.DisplayText = $"{latestVersion.Version} - " + DocsAppConsts.LatestVersion;
            latestVersion.Version = latestVersion.Version;

            return latestVersion;
        }

        public string GetSpecificVersionOrLatest()
        {
            return Document.Version == LatestVersionInfo.Version ?
                DocsAppConsts.LatestVersion :
                Document.Version;
        }

        private async Task SetDocumentAsync()
        {
            Document = await _documentAppService.GetByNameAsync(ProjectName, DocumentNameWithExtension, Version, true);
            var converter = _documentConverterFactory.Create(Document.Format ?? ProjectFormat);

            var content = converter.NormalizeLinks(Document.Content, Document.Project.ShortName, GetSpecificVersionOrLatest(), Document.LocalDirectory);
            content = converter.Convert(content);

            content = HtmlNormalizer.ReplaceImageSources(content, Document.RawRootUrl, Document.LocalDirectory);
            content = HtmlNormalizer.ReplaceCodeBlocksLanguage(content, "language-C#", "language-csharp"); //todo find a way to make it on client in prismJS configuration (eg: map C# => csharp)

            Document.Content = content;
        }
    }
=======
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Docs.Documents;
using Volo.Docs.HtmlConverting;
using Volo.Docs.Models;
using Volo.Docs.Projects;

namespace Volo.Docs.Pages.Documents.Project
{
    public class IndexModel : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ProjectName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Version { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string DocumentName { get; set; }

        public ProjectDto Project { get; set; }

        public string DocumentNameWithExtension { get; private set; }

        public DocumentWithDetailsDto Document { get; private set; }

        public List<SelectListItem> VersionSelectItems { get; private set; }

        public NavigationWithDetailsDto Navigation { get; private set; }

        public VersionInfoViewModel LatestVersionInfo { get; private set; }

        private readonly IDocumentAppService _documentAppService;
        private readonly IDocumentToHtmlConverterFactory _documentToHtmlConverterFactory;
        private readonly IProjectAppService _projectAppService;

        public IndexModel(
            IDocumentAppService documentAppService, 
            IDocumentToHtmlConverterFactory documentToHtmlConverterFactory, 
            IProjectAppService projectAppService)
        {
            _documentAppService = documentAppService;
            _documentToHtmlConverterFactory = documentToHtmlConverterFactory;
            _projectAppService = projectAppService;
        }

        public async Task OnGetAsync()
        {
            await SetProjectAsync();
            await SetVersionAsync();
            await SetDocumentAsync();
            await SetNavigationAsync();
        }

        private async Task SetProjectAsync()
        {
            Project = await _projectAppService.GetAsync(ProjectName);
        }

        private async Task SetVersionAsync()
        {
            //TODO: Needs refactoring

            var output = await _projectAppService.GetVersionsAsync(Project.ShortName);
            var versions = output.Items
                .Select(v => new VersionInfoViewModel(v.DisplayName, v.Name))
                .ToList();

            if (versions.Any())
            {
                LatestVersionInfo = versions.First();
                LatestVersionInfo.DisplayText = $"{LatestVersionInfo.DisplayText} ({DocsAppConsts.Latest})";
                LatestVersionInfo.Version = LatestVersionInfo.Version;

                if (string.Equals(Version, DocsAppConsts.Latest, StringComparison.OrdinalIgnoreCase))
                {
                    LatestVersionInfo.IsSelected = true;
                    Version = LatestVersionInfo.Version;
                }
                else
                {
                    var versionFromUrl = versions.FirstOrDefault(v => v.Version == Version);
                    if (versionFromUrl != null)
                    {
                        versionFromUrl.IsSelected = true;
                        Version = versionFromUrl.Version;
                    }
                    else
                    {
                        versions.First().IsSelected = true;
                        Version = versions.First().Version;
                    }
                }
            }

            VersionSelectItems = versions.Select(v => new SelectListItem
            {
                Text = v.DisplayText,
                Value = CreateLink(LatestVersionInfo, v.Version, DocumentName),
                Selected = v.IsSelected
            }).ToList();
        }

        private async Task SetNavigationAsync()
        {
            try
            {
                var document = await _documentAppService.GetNavigationAsync(
                    new GetNavigationDocumentInput
                    {
                        ProjectId = Project.Id,
                        Version = Version
                    }
                );

                Navigation = ObjectMapper.Map<DocumentWithDetailsDto, NavigationWithDetailsDto>(document);
            }
            catch (DocumentNotFoundException) //TODO: What if called on a remote service which may return 404
            {
                return;
            }

            Navigation.ConvertItems();
        }
        
        public string CreateLink(VersionInfoViewModel latestVersion, string version, string documentName = null)
        {
            if (latestVersion == null || latestVersion.Version == version)
            {
                version = DocsAppConsts.Latest;
            }

            var link = "/documents/" + ProjectName + "/" + version;

            if (documentName != null)
            {
                link += "/" + DocumentName;
            }

            return link;
        }

        public string GetSpecificVersionOrLatest()
        {
            if (Document?.Version == null)
            {
                return DocsAppConsts.Latest;
            }

            return Document.Version == LatestVersionInfo.Version ?
                DocsAppConsts.Latest :
                Document.Version;
        }

        private async Task SetDocumentAsync()
        {
            if (DocumentName.IsNullOrWhiteSpace())
            {
                DocumentName = Project.DefaultDocumentName;
            }

            DocumentNameWithExtension = DocumentName + "." + Project.Format;

            try
            {
                if (DocumentNameWithExtension.IsNullOrWhiteSpace())
                {
                    Document = await _documentAppService.GetDefaultAsync(
                        new GetDefaultDocumentInput
                        {
                            ProjectId = Project.Id,
                            Version = Version
                        }
                    );
                }
                else
                {
                    Document = await _documentAppService.GetAsync(
                        new GetDocumentInput
                        {
                            ProjectId = Project.Id,
                            Name = DocumentNameWithExtension,
                            Version = Version
                        }
                    );
                }
            }
            catch (DocumentNotFoundException)
            {
                //TODO: Handle it!
                throw;
            }

            ConvertDocumentContentToHtml();
        }

        private void ConvertDocumentContentToHtml()
        {
            var converter = _documentToHtmlConverterFactory.Create(Document.Format ?? Project.Format);
            var content = converter.Convert(Project, Document, GetSpecificVersionOrLatest());

            content = HtmlNormalizer.ReplaceImageSources(
                content,
                Document.RawRootUrl,
                Document.LocalDirectory
            );

            //todo find a way to make it on client in prismJS configuration (eg: map C# => csharp)
            content = HtmlNormalizer.ReplaceCodeBlocksLanguage(
                content,
                "language-C#",
                "language-csharp"
            );

            Document.Content = content;
        }
    }
>>>>>>> upstream/master
}