<<<<<<< HEAD
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Projects
{
    public interface IProjectAppService : IApplicationService
    {
        Task<ListResultDto<ProjectDto>> GetListAsync();
     
        Task<ProjectDto> FindByShortNameAsync(string shortName);
    }
=======
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Docs.Projects
{
    public interface IProjectAppService : IApplicationService
    {
        Task<ListResultDto<ProjectDto>> GetListAsync();
     
        Task<ProjectDto> GetAsync(string shortName);
        
        Task<ListResultDto<VersionInfoDto>> GetVersionsAsync(string shortName);
    }
>>>>>>> upstream/master
}