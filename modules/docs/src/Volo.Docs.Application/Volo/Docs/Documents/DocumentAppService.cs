using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class DocumentAppService : ApplicationService, IDocumentAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDistributedCache<List<string>> _distributedCache;
        private readonly IDocumentStoreFactory _documentStoreFactory;

        public DocumentAppService(
            IProjectRepository projectRepository,
            IDistributedCache<List<string>> distributedCache,
            IDocumentStoreFactory documentStoreFactory)
        {
            _projectRepository = projectRepository;
            _distributedCache = distributedCache;
            _documentStoreFactory = documentStoreFactory;
        }

        public async Task<DocumentWithDetailsDto> GetByNameAsync(string projectShortName, string documentName, string version, bool normalize)
        {
            var project = await _projectRepository.FindByShortNameAsync(projectShortName);

            return await GetDocument(ObjectMapper.Map<Project, ProjectDto>(project), documentName, version, normalize);
        }

        public async Task<NavigationWithDetailsDto> GetNavigationDocumentAsync(string projectShortName, string version, bool normalize)
        {
            var project = await _projectRepository.FindByShortNameAsync(projectShortName);

            return ObjectMapper.Map<DocumentWithDetailsDto, NavigationWithDetailsDto>(
                await GetDocument(ObjectMapper.Map<Project, ProjectDto>(project), project.NavigationDocumentName,
                    version, normalize));
        }

        public async Task<DocumentWithDetailsDto> GetDocument(ProjectDto project, string documentName, string version, bool normalize)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (string.IsNullOrWhiteSpace(documentName))
            {
                documentName = project.DefaultDocumentName;
            }

            IDocumentStore documentStore = _documentStoreFactory.Create(project.DocumentStoreType);

            Document document = await documentStore.FindDocumentByNameAsync(project.ExtraProperties, project.Format, documentName, version);

            var dto = ObjectMapper.Map<Document, DocumentWithDetailsDto>(document);

            dto.Project = project;

            return dto;
        }

        public async Task<List<string>> GetVersions(string projectShortName, string defaultDocumentName, Dictionary<string, object> projectExtraProperties,
            string documentStoreType, string documentName)
        {
            if (string.IsNullOrWhiteSpace(documentName))
            {
                documentName = defaultDocumentName;
            }

            var documentStore = _documentStoreFactory.Create(documentStoreType);

            var versions = await GetVersionsFromCache(projectShortName);

            if (versions == null)
            {
                versions = await documentStore.GetVersions(projectExtraProperties, documentName);
                await SetVersionsToCache(projectShortName, versions);
            }

            return versions;
        }

        private async Task<List<string>> GetVersionsFromCache(string projectShortName)
        {
            return await _distributedCache.GetAsync(projectShortName);
        }

        private async Task SetVersionsToCache(string projectShortName, List<string> versions)
        {
            var options = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromDays(1) };
            await _distributedCache.SetAsync(projectShortName, versions, options);
        }
    }
}