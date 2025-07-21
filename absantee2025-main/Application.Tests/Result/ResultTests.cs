using Application;
using Xunit;

namespace Application.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Success_Should_Set_IsSuccess_True_And_Error_Null()
        {
            var result = Result.Success();

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Null(result.Error);
        }

        [Fact]
        public void Failure_Should_Set_IsSuccess_False_And_Return_Error()
        {
            var error = Error.NotFound("Resource not found");
            var result = Result.Failure(error);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(error, result.Error);
        }
    }

    public class ResultGenericTests
    {
        [Fact]
        public void SuccessT_Should_Set_IsSuccess_True_Value_Set_And_Error_Null()
        {
            var result = Result<string>.Success("Test");

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal("Test", result.Value);
            Assert.Null(result.Error);
        }

        [Fact]
        public void FailureT_Should_Set_IsSuccess_False_Error_Set_And_Default_Value_For_Reference_Type()
        {
            var error = Error.BadRequest("Invalid input");
            var result = Result<string>.Failure(error);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(error, result.Error);
            Assert.Null(result.Value); // default for reference type
        }

        [Fact]
        public void FailureT_Should_Set_Default_Value_For_Value_Types()
        {
            var error = Error.InternalServerError("Something went wrong");
            var result = Result<int>.Failure(error);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(0, result.Value); // default(int)
            Assert.Equal(error, result.Error);
        }
    }
}
