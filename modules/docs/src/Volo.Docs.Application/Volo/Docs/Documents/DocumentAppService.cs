<<<<<<< HEAD
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
=======
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
        private readonly IDocumentStoreFactory _documentStoreFactory;
        protected IDistributedCache<DocumentWithDetailsDto> DocumentCache { get; }
        protected IDistributedCache<DocumentResourceDto> ResourceCache { get; }

        public DocumentAppService(
            IProjectRepository projectRepository,
            IDocumentStoreFactory documentStoreFactory, 
            IDistributedCache<DocumentWithDetailsDto> documentCache, 
            IDistributedCache<DocumentResourceDto> resourceCache)
        {
            _projectRepository = projectRepository;
            _documentStoreFactory = documentStoreFactory;
            DocumentCache = documentCache;
            ResourceCache = resourceCache;
        }

        public virtual async Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDto(
                project,
                input.Name,
                input.Version
            );
        }

        public virtual async Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDto(
                project,
                project.DefaultDocumentName,
                input.Version
            );
        }

        public virtual async Task<DocumentWithDetailsDto> GetNavigationAsync(GetNavigationDocumentInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);

            return await GetDocumentWithDetailsDto(
                project,
                project.NavigationDocumentName,
                input.Version
            );
        }

        public async Task<DocumentResourceDto> GetResourceAsync(GetDocumentResourceInput input)
        {
            var project = await _projectRepository.GetAsync(input.ProjectId);
            var cacheKey = $"Resource@{project.ShortName}#{input.Name}#{input.Version}";

            return await ResourceCache.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var store = _documentStoreFactory.Create(project.DocumentStoreType);
                    var documentResource = await store.GetResource(project, input.Name, input.Version);

                    return ObjectMapper.Map<DocumentResource, DocumentResourceDto>(documentResource);
                },
                () => new DistributedCacheEntryOptions
                {
                    //TODO: Configurable?
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                }
            );
        }

        protected virtual async Task<DocumentWithDetailsDto> GetDocumentWithDetailsDto(
            Project project, 
            string documentName, 
            string version)
        {
            var cacheKey = $"Document@{project.ShortName}#{documentName}#{version}";

            return await DocumentCache.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var store = _documentStoreFactory.Create(project.DocumentStoreType);
                    var document = await store.GetDocument(project, documentName, version);

                    return CreateDocumentWithDetailsDto(project, document);
                },
                () => new DistributedCacheEntryOptions
                {
                    //TODO: Configurable?
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6),
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                }
            );
        }

        protected virtual DocumentWithDetailsDto CreateDocumentWithDetailsDto(Project project, Document document)
        {
            var documentDto = ObjectMapper.Map<Document, DocumentWithDetailsDto>(document);
            documentDto.Project = ObjectMapper.Map<Project, ProjectDto>(project);
            documentDto.Contributors = ObjectMapper.Map<List<DocumentContributor>, List<DocumentContributorDto>>(document.Contributors);
            return documentDto;
        }
    }
>>>>>>> upstream/master
}