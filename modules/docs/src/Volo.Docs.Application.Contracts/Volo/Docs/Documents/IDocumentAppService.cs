<<<<<<< HEAD
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public interface IDocumentAppService : IApplicationService
    {
        Task<DocumentWithDetailsDto> GetByNameAsync(string projectShortName, string documentName, string version,
            bool normalize);

        Task<NavigationWithDetailsDto> GetNavigationDocumentAsync(string projectShortName, string version,
            bool normalize);

        Task<List<string>> GetVersions(string projectShortName, string defaultDocumentName,
            Dictionary<string, object> projectExtraProperties,
            string documentStoreType, string documentName);

        Task<DocumentWithDetailsDto> GetDocument(ProjectDto project, string documentName, string version, bool normalize);
    }
=======
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Documents
{
    public interface IDocumentAppService : IApplicationService
    {
        Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input);

        Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input);

        Task<DocumentWithDetailsDto> GetNavigationAsync(GetNavigationDocumentInput input);

        Task<DocumentResourceDto> GetResourceAsync(GetDocumentResourceInput input);
    }
>>>>>>> upstream/master
}