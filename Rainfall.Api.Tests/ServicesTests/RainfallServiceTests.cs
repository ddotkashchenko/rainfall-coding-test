
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Rainfall.Api.Domain;
using Rainfall.Api.Services;

namespace Rainfall.Api.Tests.Services;

public class RainfallServiceTests
{
    [Fact]
    public async Task RainfallServiceReturnsReadings()
    {
        // Arrange
        var httpHandlerMock = new Mock<HttpMessageHandler>();

        httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(GetItems());

                var httpClientMock = new HttpClient(httpHandlerMock.Object) {BaseAddress = new Uri("https://test")};

        var configurationMock = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"RainfallFloodMonitorBaseUrl", "http://testurl/"}
            }).Build();
        var service = new RainfallService(httpClientMock, configurationMock);

        // Act
        var result = await service.GetRainfallReadings("test_station", 10);

        // Assert
        Assert.Equal(true, result.Success);
        Assert.Equal(3, result.Readings.Count());
    }

    [Fact]
    public async Task RainfallServiceReturnStationNotFound()
    {
        // Arrange
        var httpHandlerMock = new Mock<HttpMessageHandler>();

        httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(GetEmpty());

        var httpClientMock = new HttpClient(httpHandlerMock.Object) {BaseAddress = new Uri("https://test")};

        var configurationMock = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"RainfallFloodMonitorBaseUrl", "http://testurl/"}
            }).Build();

        var service = new RainfallService(httpClientMock, configurationMock);

        // Act
        var result = await service.GetRainfallReadings("test_station", 10);

        // Assert
        Assert.Equal(false, result.Success);
        Assert.Equal(1, result.Errors.Count());
        Assert.Equal(RainfallReadingError.StationNotFound, result.Errors.First());
    }

    [Fact]
    public async Task RainfallServiceReturnRequestError()
    {
        // Arrange
        var httpHandlerMock = new Mock<HttpMessageHandler>();

        httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(GetError());

                var httpClientMock = new HttpClient(httpHandlerMock.Object) {BaseAddress = new Uri("https://test")};

        var configurationMock = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"RainfallFloodMonitorBaseUrl", "http://testurl/"}
            }).Build();
        var service = new RainfallService(httpClientMock, configurationMock);

        // Act
        var result = await service.GetRainfallReadings("test_station", 10);

        // Assert
        Assert.Equal(false, result.Success);
        Assert.Equal(1, result.Errors.Count());
        Assert.Equal(RainfallReadingError.RequestError, result.Errors.First());
    }

    [Fact]
    public async Task RainfallServiceSendsCorrectRequest()
    {
        // Arrange
        var httpHandlerMock = new Mock<HttpMessageHandler>();

        httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(GetEmpty());

        var httpClientMock = new HttpClient(httpHandlerMock.Object) {BaseAddress = new Uri("https://test")};

        var configurationMock = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"RainfallFloodMonitoringBaseUrl", "http://testurl/"}
            }).Build();

        var service = new RainfallService(httpClientMock, configurationMock);

        // Act
        var result = await service.GetRainfallReadings("test_station", 10);

        //Assert
        httpHandlerMock.Protected().Verify(
            "SendAsync", 
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req => req.RequestUri == new Uri("http://testurl/id/stations/test_station/readings?_sorted&_limit=10")),
            ItExpr.IsAny<CancellationToken>());
    }

    private HttpResponseMessage GetError()
    {
        return new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.InternalServerError
        };
    }

    private HttpResponseMessage GetEmpty()
    {
        return new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = JsonContent.Create
            (
                new
                {
                    items = new List<dynamic>()
                }
            )
        };
    }

    private HttpResponseMessage GetItems()
    {
        return new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = JsonContent.Create
            (
                new
                {
                    items = new List<dynamic>
                    {
                        new
                        {
                            @id = "1",
                            dateTime = "2023-12-05T09:00:00Z",
                            measure = "testMeasure",
                            value = 0.0
                        },
                        new
                        {
                            @id = "1",
                            dateTime = "2023-12-05T08:45:00Z",
                            measure = "testMeasure",
                            value = 1.0
                        },
                        new
                        {
                            @id = "1",
                            dateTime = "2023-12-05T08:30:00Z",
                            measure = "testMeasure",
                            value = 2.0
                        },
                    },
                }
            )
        };
    }
}