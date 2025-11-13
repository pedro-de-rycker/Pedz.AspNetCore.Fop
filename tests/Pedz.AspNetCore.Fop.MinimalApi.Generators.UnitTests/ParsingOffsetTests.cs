using Microsoft.AspNetCore.Http;
using Pedz.AspNetCore.Fop.MinimalApi.Enums;
using Pedz.AspNetCore.Fop.MinimalApi.Generators.UnitTests.Entities;
using System.Reflection;

namespace Pedz.AspNetCore.Fop.MinimalApi.Generators.UnitTests;

internal class ParsingOffsetTests
{
    [Test]
    public async Task TestDataObjectInstanciation__WithSuccess()
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
    [MatrixDataSource]
    public async Task TestDataObjectOffset__WithSuccess(
        [Matrix("offset", "OFFSET")] string propertyName)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString($"?{propertyName}[250]=500");

        // Act
        var dataObject =
            await OffsetEntityOffsetPagingData.BindAsync(context, default!);

        // Assert
        await Assert.That(dataObject).IsNotNull();
        await Assert.That(dataObject.Offset).IsEqualTo(500);
        await Assert.That(dataObject.Size).IsEqualTo(250);
    }

    [Test]
    [MatrixDataSource]
    public async Task TestDataObjectSortable__WithSuccess(
        [Matrix("sort-by", "SORT-BY")] string propertyName)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString($"?{propertyName}=asc:Id");

        // Act
        var dataObject =
            await OffsetEntityOffsetPagingData.BindAsync(context, default!);

        // Assert
        await Assert.That(dataObject).IsNotNull();
        await Assert.That(dataObject.SortBy).IsEqualTo("Id");
        await Assert.That(dataObject.SortDirection).IsEqualTo(Enums.SortDirectionEnum.Ascending);
    }

    [Test]
    [MatrixDataSource]
    public async Task TestDataObjectFiltering__WithSuccess(
        [Matrix(["properties", "PROPERTIES"])] string prefix,
        [Matrix(nameof(OffsetEntity.NullableStringProperty), "nullablestringproperty", "NULLABLESTRINGPROPERTY")] string propertyName)
    {
        // Arrange
        string value = "test";
        var context = new DefaultHttpContext();
        context.Request.QueryString = new QueryString($"?{prefix}.{propertyName}=eq:{value}");

        // Act
        var dataObject =
            await OffsetEntityOffsetPagingData.BindAsync(context, default!);

        // Assert
        await Assert.That(dataObject).IsNotNull();
        await Assert.That(dataObject.FilterBy.Count).IsEqualTo(1);
        await Assert.That(dataObject.FilterBy.First().name).IsEqualTo(propertyName);
        await Assert.That(dataObject.FilterBy.First().value).IsEqualTo(value);
        await Assert.That(dataObject.FilterBy.First().filteringType).IsEqualTo(FilteringTypeEnum.Equal);
    }
}
