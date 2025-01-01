using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChyaTusha.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ChyaTusha.Extensions.Tests
{
    [TestClass()]
    public class StringFileHandlerTests
    {
        private readonly StringFileHandler _handler;

        public StringFileHandlerTests()
        {
            _handler = new StringFileHandler();
        }

        [Theory]
        [InlineData(@"C:\Projects\MyApp\bin\Debug\net7.0\app.dll", "MyApp", @"C:\Projects\MyApp")]
        [InlineData(@"C:\Projects\MyApp\bin\Debug\net7.0\app.dll", "bin", @"C:\Projects\MyApp\bin")]
        [InlineData(@"C:\Projects\MyApp\bin\Debug\net7.0\app.dll", "Projects", @"C:\Projects")]
        [InlineData(@"C:\MyApp\app.dll", "MyApp", @"C:\MyApp")]
        [InlineData(@"C:\Projects\app.dll", "app", @"C:\Projects")]
        public void TrimPath_ShouldReturnCorrectDirectory(string path, string trimAfter, string expected)
        {
            // Act
            string result = _handler.TrimPath(path, trimAfter);

            // Assert
            Assert.Equals(expected, result);
        }

        [Fact]
        public void TrimPath_ShouldReturnOriginalPath_IfTargetDirectoryNotFound()
        {
            // Arrange
            string path = @"C:\Projects\MyApp\bin\Debug\net7.0\app.dll";
            string trimAfter = "NotExist";

            // Act
            string result = _handler.TrimPath(path, trimAfter);

            // Assert
            Assert.Equals(path, result);
        }

        [Fact]
        public void TrimPath_ShouldHandleRootDirectory()
        {
            // Arrange
            string path = @"C:\MyApp\app.dll";
            string trimAfter = "MyApp";

            // Act
            string result = _handler.TrimPath(path, trimAfter);

            // Assert
            Assert.Equals(@"C:\MyApp", result);
        }

        [Theory]
        [InlineData("", "MyApp", "")]
        [InlineData(@"C:\Projects\MyApp\app.dll", "", @"C:\Projects\MyApp\app.dll")]
        public void TrimPath_ShouldReturnOriginalPath_IfInvalidInput(string path, string trimAfter, string expected)
        {
            // Act
            string result = _handler.TrimPath(path, trimAfter);

            // Assert
            Assert.Equals(expected, result);
        }

        [Theory]
        [InlineData(@"C:\Projects\MyApp\bin\Debug\net7.0\app.dll", @"C:\Projects\MyApp\bin\Debug\net7.0")]
        [InlineData(@"C:\MyApp\app.dll", @"C:\MyApp")]
        [InlineData(@"C:\Projects\", @"C:\Projects")]
        [InlineData(@"C:\", @"C:\")]
        public void GetDirectory_ShouldReturnCorrectDirectory(string filePath, string expected)
        {
            // Act
            string result = _handler.GetDirectory(filePath);

            // Assert
            Assert.Equals(expected, result);
        }

        [Fact]
        public void GetDirectory_ShouldReturnEmpty_IfPathIsEmpty()
        {
            // Act
            string result = _handler.GetDirectory("");

            // Assert
            Assert.Equals(string.Empty, result);
        }
    }
}