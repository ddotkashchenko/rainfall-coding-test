
using System.Net.Http.Json;
using Moq;
using Moq.Protected;
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

        var httpClientMock = new HttpClient(httpHandlerMock.Object);
        var service = new RainfallService(httpClientMock);

        // Act
        var result = await service.GetRainfallReadings("test_station", 10);

        // Assert
        Assert.Equal(true, result.Success);
        Assert.Equal(3, result.Readings.Count());
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