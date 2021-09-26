using Friendlizer;
using Friendlizer.Controllers;
using Friendlizer.Models;
using Friendlizer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Friendlizer_tests
{
    // We test only file import because other methods of FriendSetController were auto-generated.
    public class FriendSetsControllerTest : IDisposable
    {
        private FriendsSetsController controller;
        private FriendsDbContext context = new FriendsDbContext(new DbContextOptionsBuilder<FriendsDbContext>().UseInMemoryDatabase("FriendsDb").Options);

        [Fact(DisplayName = "File import should fail when file is not provided")]
        public async Task POST_Fails_With_Empty_File()
        {
            var fileParser = new Mock<IFileParser>();
            fileParser.Setup(parser => parser.Parse(It.IsAny<IFormFile>(), It.IsAny<long>())).ReturnsAsync(() => new List<Relation>());

            controller = new FriendsSetsController(context, fileParser.Object);

            var res = await controller.PostFriendsSet(null);
            Assert.NotNull(res);
            Assert.NotNull(res.Result);
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact(DisplayName = "New Friends Set record should be created when a valid file is provided")]
        public async Task POST_Creates_Friends_Set()
        {
            var fileParser = new Mock<IFileParser>();
            fileParser.Setup(parser => parser.Parse(It.IsAny<IFormFile>(), It.IsAny<long>())).ReturnsAsync(() => new Relation[]{
            new Relation{ FriendsSetId=1, FirstPersonId=0, SecondPersonId=1 },
            new Relation{ FriendsSetId=1, FirstPersonId=0, SecondPersonId=2 },
            new Relation{ FriendsSetId=1, FirstPersonId=0, SecondPersonId=3 }
            });

            var formFile = new Mock<IFormFile>();
            formFile.SetupGet(file => file.FileName).Returns("aaa.txt");

            controller = new FriendsSetsController(context, fileParser.Object);

            var res = await controller.PostFriendsSet(formFile.Object);
            Assert.NotNull(res);
            Assert.NotNull(res.Result);
            Assert.IsType<CreatedAtActionResult>(res.Result);

            var createdAtRes = (FriendsSetImportResult)(((CreatedAtActionResult)res.Result).Value);
            Assert.NotNull(createdAtRes);
            Assert.Equal("aaa.txt", createdAtRes.filename);
            Assert.Equal(3, createdAtRes.imported);

            fileParser.Verify(mock => mock.Parse(It.Is<IFormFile>(f => f == formFile.Object), It.Is<long>(id => id == 1)), Times.Once);
            Assert.Equal(context.Relations.Count(), createdAtRes.imported);
        }

        [Fact(DisplayName = "File import should fail if a file with the same name has already been imported.")]
        public async Task POST_Import_Fails_When_Same_Name_File_Imported()
        {
            var fileParser = new Mock<IFileParser>();
            fileParser.Setup(parser => parser.Parse(It.IsAny<IFormFile>(), It.IsAny<long>())).ReturnsAsync(() => new Relation[]{
            new Relation{ FriendsSetId=1, FirstPersonId=0, SecondPersonId=1 },
            new Relation{ FriendsSetId=1, FirstPersonId=0, SecondPersonId=2 },
            new Relation{ FriendsSetId=1, FirstPersonId=0, SecondPersonId=3 }
            });

            var formFile = new Mock<IFormFile>();
            formFile.SetupGet(file => file.FileName).Returns("aaa.txt");

            controller = new FriendsSetsController(context, fileParser.Object);

            var res = await controller.PostFriendsSet(formFile.Object);
            Assert.NotNull(res);
            Assert.NotNull(res.Result);
            Assert.IsType<CreatedAtActionResult>(res.Result);

            res = await controller.PostFriendsSet(formFile.Object);
            Assert.NotNull(res);
            Assert.NotNull(res.Result);
            Assert.IsType<BadRequestObjectResult>(res.Result);
        }

        [Fact(DisplayName ="Statistics should be counted correctly for a given dataset of friends")]
        public async Task GetStats_Counts_Correctly()
        {
            controller = new FriendsSetsController(context, null);
            context.FriendsSetItems.Add(new FriendsSet { ID = 1, Filename = "aaa.txt" });
            context.Relations.AddRange(
                new Relation { FriendsSetId = 1, FirstPersonId = 0, SecondPersonId = 1 },
                new Relation { FriendsSetId = 1, FirstPersonId = 0, SecondPersonId = 2 },
                new Relation { FriendsSetId = 1, FirstPersonId = 0, SecondPersonId = 3 },
                new Relation { FriendsSetId = 1, FirstPersonId = 1, SecondPersonId = 0 },
                new Relation { FriendsSetId = 1, FirstPersonId = 2, SecondPersonId = 1 }
                );
            context.SaveChanges();

            var stats = await controller.GetStats(1);
            Assert.NotNull(stats);
            Assert.NotNull(stats.Value);
            Assert.Equal(4, stats.Value.TotalUsers);
            Assert.Equal(1, stats.Value.AvgFriendsCount);
        }

        [Fact(DisplayName = "Getting statistics should fail when dataset with specified doesn't exist")]
        public async Task GetStats_Fails_For_Unknown_Dataset()
        {
            controller = new FriendsSetsController(context, null);

            var stats = await controller.GetStats(1);
            Assert.NotNull(stats);
            Assert.Null(stats.Value);
            Assert.IsType<NotFoundResult>(stats.Result);
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
