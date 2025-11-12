using Microsoft.AspNetCore.Http;
using Pedz.AspNetCore.Fop.MinimalApi.Generators.UnitTests.Entities;
using System.Reflection;

namespace Pedz.AspNetCore.Fop.MinimalApi.Generators.UnitTests;

internal class OffsetTests
{
    [Test]
    public async Task TestDataObjectInstanciation()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act
        var dataObject =
            await OffsetEntityOffsetPagingData.BindAsync(context, default!);

        // Assert
        await Assert.That(dataObject).IsNotNull();
    }

    [Test]
    public async Task TestDataObjectOffset()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString("?offset[250]=500");

        // Act
        var dataObject =
            await OffsetEntityOffsetPagingData.BindAsync(context, default!);

        // Assert
        await Assert.That(dataObject).IsNotNull();
        await Assert.That(dataObject.Offset).IsEqualTo(500);
        await Assert.That(dataObject.Size).IsEqualTo(250);
    }

    [Test]
    public async Task TestDataObjectSortable()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString("?sort-by=asc:Id");

        // Act
        var dataObject =
            await OffsetEntityOffsetPagingData.BindAsync(context, default!);

        // Assert
        await Assert.That(dataObject).IsNotNull();
        await Assert.That(dataObject.SortBy).IsEqualTo("Id");
        await Assert.That(dataObject.SortDirection).IsEqualTo(Enums.SortDirectionEnum.Ascending);
    }
}
