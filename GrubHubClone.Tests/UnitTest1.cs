namespace GrubHubClone.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var isTrue = true;
            // Act
            var checkIsTrue = isTrue ? true : false;
            // Assert
            Assert.True(checkIsTrue);
        }
    }
}