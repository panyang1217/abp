﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class SoftDelete_Filter_Tests<TStartupModule> : TestAppTestBase<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected readonly IRepository<Person, Guid> PersonRepository;
        protected readonly IDataFilter DataFilter;

        protected SoftDelete_Filter_Tests()
        {
            PersonRepository = GetRequiredService<IRepository<Person, Guid>>();
            DataFilter = GetRequiredService<IDataFilter>();
        }

        [Fact]
        public void Should_Not_Get_Deleted_Entities_Linq()
        {
            WithUnitOfWork(() =>
            {
                var person = PersonRepository.FirstOrDefault(p => p.Name == "John-Deleted");
                person.ShouldBeNull();
            });
        }

        [Fact]
        public async Task Should_Not_Get_Deleted_Entities_By_Id()
        {
            await WithUnitOfWorkAsync(async () =>
            {
                var person = await PersonRepository.FindAsync(TestDataBuilder.UserJohnDeletedId);
                person.ShouldBeNull();
            });
        }

        [Fact]
        public void Should_Not_Get_Deleted_Entities_By_Default_ToList()
        {
            WithUnitOfWork(() =>
            {
                var people = PersonRepository.ToList();
                people.Count.ShouldBe(1);
                people.Any(p => p.Name == "Douglas").ShouldBeTrue();
            });
        }

        [Fact]
        public void Should_Get_Deleted_Entities_When_Filter_Is_Disabled()
        {
            WithUnitOfWork(() =>
            {
                //Soft delete is enabled by default
                var people = PersonRepository.ToList();
                people.Any(p => !p.IsDeleted).ShouldBeTrue();
                people.Any(p => p.IsDeleted).ShouldBeFalse();

                using (DataFilter.Disable<ISoftDelete>())
                {
                    //Soft delete is disabled
                    people = PersonRepository.ToList();
                    people.Any(p => !p.IsDeleted).ShouldBeTrue();
                    people.Any(p => p.IsDeleted).ShouldBeTrue();

                    using (DataFilter.Enable<ISoftDelete>())
                    {
                        //Soft delete is enabled again
                        people = PersonRepository.ToList();
                        people.Any(p => !p.IsDeleted).ShouldBeTrue();
                        people.Any(p => p.IsDeleted).ShouldBeFalse();
                    }

                    //Soft delete is disabled (restored previous state)
                    people = PersonRepository.ToList();
                    people.Any(p => !p.IsDeleted).ShouldBeTrue();
                    people.Any(p => p.IsDeleted).ShouldBeTrue();
                }

                //Soft delete is enabled (restored previous state)
                people = PersonRepository.ToList();
                people.Any(p => !p.IsDeleted).ShouldBeTrue();
                people.Any(p => p.IsDeleted).ShouldBeFalse();
            });
        }
    }
}
