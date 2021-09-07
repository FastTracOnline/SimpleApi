using SimpleAPI.Common;
using SimpleAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleAPI.Tests.EntityValidationTests
{
    public class SimplePOCOTests
    {
        // The constructor for SimplePOCO assigns values to the Id, MyEnumField and MyChildren properties

        [Fact]
        public void ValidRecord()
        {
            //Arrange
            var record = new SimplePOCO()
            {
                MyField = "Something"
            };

            //Act
            var valid = record.IsValid;
            var messages = record.ValidationErrors;

            //Assert
            Assert.True(valid);
            Assert.IsType<List<ValidationResult>>(messages);
            Assert.Empty(messages);
        }

        [Fact]
        public void MissingId()
        {
            //Arrange
            var record = new SimplePOCO()
            {
                Id = Guid.Empty,
                MyField = "Something"
            };

            //Act
            var valid = record.IsValid;
            var messages = record.ValidationErrors;

            //Assert
            Assert.False(valid);
            Assert.IsType<List<ValidationResult>>(messages);
            Assert.NotEmpty(messages);
            Assert.Single(messages);
            Assert.Single(messages.First().MemberNames);
            var memberName = messages.First().MemberNames.First();
            Assert.Equal(nameof(SimplePOCO.Id), memberName);
        }

        [Fact]
        public void MissingMyField()
        {
            //Arrange
            var record = new SimplePOCO()
            {
            };

            //Act
            var valid = record.IsValid;
            var messages = record.ValidationErrors;

            //Assert
            Assert.False(valid);
            Assert.IsType<List<ValidationResult>>(messages);
            Assert.NotEmpty(messages);
            Assert.Single(messages);
            Assert.Single(messages.First().MemberNames);
            var memberName = messages.First().MemberNames.First();
            Assert.Equal(nameof(SimplePOCO.MyField), memberName);
        }

        [Fact]
        public void MissingMyEnumField()
        {
            //Arrange
            var record = new SimplePOCO()
            {
                MyField = "Something",
                MyEnumField = (SimpleEnum)(-1)
            };

            //Act
            var valid = record.IsValid;
            var messages = record.ValidationErrors;

            //Assert
            Assert.False(valid);
            Assert.IsType<List<ValidationResult>>(messages);
            Assert.NotEmpty(messages);
            Assert.Single(messages);
            Assert.Single(messages.First().MemberNames);
            var memberName = messages.First().MemberNames.First();
            Assert.Equal(nameof(SimplePOCO.MyEnumField), memberName);
        }

        [Fact]
        public void MissingMyChildren()         // If not initialized
        {
            //Arrange
            var record = new SimplePOCO()
            {
                MyField = "Something",
                MyChildren = null
            };

            //Act
            var valid = record.IsValid;
            var messages = record.ValidationErrors;

            //Assert
            Assert.False(valid);
            Assert.IsType<List<ValidationResult>>(messages);
            Assert.NotEmpty(messages);
            Assert.Single(messages);
            Assert.Single(messages.First().MemberNames);
            var memberName = messages.First().MemberNames.First();
            Assert.Equal(nameof(SimplePOCO.MyChildren), memberName);
        }
    }
}
