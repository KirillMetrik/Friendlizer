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
    public class FriendSetsControllerTest
    {
        private DbContextOptions<FriendsDbContext> dboptions = new DbContextOptionsBuilder<FriendsDbContext>().UseInMemoryDatabase("FriendsDb").Options;
        private FriendsSetsController controller;

        [Fact(DisplayName = "File import should fail when file is not provided")]
        public async Task POST_Fails_With_Empty_File()
        {
            var fileParser = new Mock<IFileParser>();
            fileParser.Setup(parser => parser.Parse(It.IsAny<IFormFile>(), It.IsAny<long>())).ReturnsAsync(() => new List<Relation>());

            using var context = new FriendsDbContext(dboptions);
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

            using var context = new FriendsDbContext(dboptions);
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
    }
}
