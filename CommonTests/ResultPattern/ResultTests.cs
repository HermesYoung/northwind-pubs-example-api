using Common.ResultPattern;

namespace CommonTests.ResultPattern;

public class ResultTests
{
    [Test]
    public void CreateSuccessResultWithNoValue_WhenGetError_ShouldThrowInvalidOperationException()
    {
        var result = Result.Success();

        Assert.That(result.IsSuccess, Is.True);
        Assert.Throws<InvalidOperationException>(() => { _ = result.Error; });
    }

    [Test]
    public void CreateFailureResultWithNoValue_WhenGetError_ShouldReturnError()
    {
        var error = new DefaultErrorMessage(1, "test error message");
        var result = Result.Failure(error);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(error));
        });
    }

    [Test]
    public void CreateSuccessResultWithValue_WhenGetError_ShouldThrowInvalidOperationException()
    {
        var value = new object();
        var result = Result.Success(value);

        Assert.That(result.IsSuccess, Is.True);
        Assert.Throws<InvalidOperationException>(() => _ = result.Error);
    }

    [Test]
    public void CreateSuccessResultWithValue_WhenGetValue_ShouldReturnValue()
    {
        const int value = 100;
        var result = Result.Success(value);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(value));
        });
    }

    [Test]
    public void CreateFailureResultWithValue_WhenGetError_ShouldReturnError()
    {
        var error = new DefaultErrorMessage(1, "test error message");
        var result = Result.Failure(error);
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(error));
        });
    }

    [Test]
    public void CreateFailureResultWithValue_WhenGetValue_ShouldThrowInvalidOperationException()
    {
        var error = new DefaultErrorMessage(1, "test error message");
        var result = Result.Failure<object>(error);

        Assert.That(result.IsSuccess, Is.False);
        Assert.Throws<InvalidOperationException>(() => _ = result.Value);
    }
}