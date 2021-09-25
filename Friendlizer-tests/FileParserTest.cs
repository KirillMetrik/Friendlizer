using Friendlizer.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Friendlizer_tests
{
    public class FileParserTest
    {
        [Fact(DisplayName = "Parser should fail when file is not provided.")]
        public async Task Parse_Fails_When_No_File()
        {
            var fileParser = new FileParser();
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await fileParser.Parse(null, 1));
        }

        [Fact(DisplayName = "Parser should work correctly when provided with valid file.")]
        public async Task Parse_Is_Correct_For_Correct_File()
        {
            var content = new StringBuilder();
            content.AppendLine("0 128");
            content.AppendLine("2 35");
            content.AppendLine("15 2");

            var formFile = GetFormFile(content.ToString());
            var fileParser = new FileParser();
            var result = await fileParser.Parse(formFile, 1);

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.All(result, r => Assert.Equal(1, r.FriendsSetId));

            var resArr = result.ToArray();
            Assert.Equal(0, resArr[0].FirstPersonId);
            Assert.Equal(128, resArr[0].SecondPersonId);
            Assert.Equal(2, resArr[1].FirstPersonId);
            Assert.Equal(35, resArr[1].SecondPersonId);
            Assert.Equal(15, resArr[2].FirstPersonId);
            Assert.Equal(2, resArr[2].SecondPersonId);
        }

        [Fact(DisplayName = "Parser should fail when file has non-numeric entries.")]
        public async Task Parse_Fails_When_File_Has_NonNumber_Entries()
        {
            var content = new StringBuilder();
            content.AppendLine("0 128");
            content.AppendLine("2 35aa");
            content.AppendLine("15 2");

            var formFile = GetFormFile(content.ToString());
            var fileParser = new FileParser();
            await Assert.ThrowsAsync<FormatException>(async () => await fileParser.Parse(formFile, 1));
        }

        [Fact(DisplayName = "Parser should fail when file has invalid number of entries on each line.")]
        public async Task Parse_Fails_When_Number_Of_Entries_Invalid()
        {
            var content = new StringBuilder();
            content.AppendLine("0 128");
            content.AppendLine("2 35 280");
            content.AppendLine("15 2");

            var formFile = GetFormFile(content.ToString());
            var fileParser = new FileParser();
            await Assert.ThrowsAsync<ArgumentException>(async () => await fileParser.Parse(formFile, 1));
        }

        [Fact(DisplayName = "Parser should fail when file has invalid separators for entries.")]
        public async Task Parse_Fails_When_Separators_Incorrect()
        {
            var content = new StringBuilder();
            content.AppendLine("0 128");
            content.AppendLine("2       35");
            content.AppendLine("15 2");

            var formFile = GetFormFile(content.ToString());
            var fileParser = new FileParser();
            await Assert.ThrowsAsync<ArgumentException>(async () => await fileParser.Parse(formFile, 1));
        }

        private IFormFile GetFormFile(string content)
        {
            var fileName = "aaa.txt";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "aaa_file", fileName);
        }
    }
}
