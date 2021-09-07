using SimpleAPI.Common;
using SimpleAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DTO = SimpleAPI.Public.DTO;

namespace SimpleAPI.Tests.MappingTests
{
    public class SimplePOCOTests
    {
        // The constructor for SimplePOCO assigns values to the Id, MyEnumField and MyChildren properties

        [Fact]
        public void MapToDTO()
        {
            //Arrange
            var parentId = Guid.NewGuid();
            var source = new SimplePOCO()
            {
                Id = parentId,
                MyField = "Something",
                MyEnumField = SimpleEnum.GoodValue,
                MyChildren = new List<SimpleChildPOCO>()
                {
                    new SimpleChildPOCO() { Id = Guid.NewGuid(), MyField = "First Child", MyEnumField = SimpleEnum.Unknown, MyParentId = parentId },
                    new SimpleChildPOCO() { Id = Guid.NewGuid(), MyField = "Second Child", MyEnumField = SimpleEnum.GoodValue, MyParentId = parentId }
                }
            };

            //TODO: Act
            //Use AutoMapper to copy to DTO object
            DTO.SimplePOCO target = new DTO.SimplePOCO();

            //Assert
            Assert.Equal(source.Id, target.Id);
            Assert.Equal(source.MyField, target.MyField);
            Assert.Equal(source.MyEnumField, target.MyEnumField);
            Assert.Equal(source.MyChildren.Count, target.MyChildren.Count);

            //TODO: Examine Children to make sure they match
        }

        [Fact]
        public void MapToEntity()
        {
            //Arrange
            var parentId = Guid.NewGuid();
            var source = new DTO.SimplePOCO()
            {
                Id = parentId,
                MyField = "Something",
                MyEnumField = SimpleEnum.GoodValue,
                MyChildren = new List<DTO.SimpleChildPOCO>()
                {
                    new DTO.SimpleChildPOCO() { Id = Guid.NewGuid(), MyField = "First Child", MyEnumField = SimpleEnum.Unknown, MyParentId = parentId },
                    new DTO.SimpleChildPOCO() { Id = Guid.NewGuid(), MyField = "Second Child", MyEnumField = SimpleEnum.GoodValue, MyParentId = parentId }
                }
            };

            //TODO: Act
            //Use AutoMapper to copy to Entity object
            SimplePOCO target = new SimplePOCO();

            //Assert
            Assert.Equal(source.Id, target.Id);
            Assert.Equal(source.MyField, target.MyField);
            Assert.Equal(source.MyEnumField, target.MyEnumField);
            Assert.Equal(source.MyChildren.Count, target.MyChildren.Count);

            //TODO: Examine Children to make sure they match
        }
    }
}
