<<<<<<< HEAD
﻿using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Blogging.Tagging;
using Volo.Blogging.Tagging.Dtos;
using Xunit;

namespace Volo.Blogging
{
    public class TagAppService_Tests : BloggingApplicationTestBase
    {
        private readonly ITagAppService _tagAppService;
        private readonly ITagRepository _tagRepository;

        public TagAppService_Tests()
        {
            _tagAppService = GetRequiredService<ITagAppService>();
            _tagRepository = GetRequiredService<ITagRepository>();
        }

        [Fact]
        public async Task Should_Get_List_Of_Tags()
        {
            var tags = await _tagAppService.GetListAsync();

            tags.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Get_Popular_Tags()
        {
            var tags = await _tagAppService.GetPopularTags(new GetPopularTagsInput() { ResultCount = 5, MinimumPostCount = 0 });

            tags.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Create_A_Tag()
        {
            var name = "test name";
            var description = "test description";

            var tagDto = await _tagAppService.CreateAsync(new CreateTagDto() { Name = name, Description = description });

            UsingDbContext(context =>
            {
                var tag = context.Tags.FirstOrDefault(q => q.Id == tagDto.Id);
                tag.ShouldNotBeNull();
                tag.Name.ShouldBe(tagDto.Name);
                tag.Description.ShouldBe(tagDto.Description);
            });
        }

        [Fact]
        public async Task Should_Update_A_Tag()
        {
            var newDescription = "new description";

            var oldTag = (await _tagRepository.GetListAsync()).FirstOrDefault(); ;

            await _tagAppService.UpdateAsync(oldTag.Id, new UpdateTagDto()
            { Description = newDescription, Name = oldTag.Name});

            UsingDbContext(context =>
            {
                var tag = context.Tags.FirstOrDefault(q => q.Id == oldTag.Id);
                tag.Description.ShouldBe(newDescription);
            });
        }

        [Fact]
        public async Task Should_Delete_A_Tag()
        {
            var tag = (await _tagRepository.GetListAsync()).First();

            await _tagAppService.DeleteAsync(tag.Id);
        }
    }
}
=======
﻿using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Blogging.Tagging;
using Volo.Blogging.Tagging.Dtos;
using Xunit;

namespace Volo.Blogging
{
    public class TagAppService_Tests : BloggingApplicationTestBase
    {
        private readonly ITagAppService _tagAppService;
        private readonly ITagRepository _tagRepository;
        private readonly BloggingTestData _bloggingTestData;

        public TagAppService_Tests()
        {
            _tagAppService = GetRequiredService<ITagAppService>();
            _tagRepository = GetRequiredService<ITagRepository>();
            _bloggingTestData = GetRequiredService<BloggingTestData>();
        }

        [Fact]
        public async Task Should_Get_Popular_Tags()
        {
            var tags = await _tagAppService.GetPopularTags(new GetPopularTagsInput() { BlogId = _bloggingTestData.Blog1Id, ResultCount = 5, MinimumPostCount = 0 });

            tags.Count.ShouldBeGreaterThan(0);
        }
    }
}
>>>>>>> upstream/master
