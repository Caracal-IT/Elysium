namespace Caracal.Text.Tests.Unit;

[Trait("Category","Unit")]
public class StringZipExtensionsTests
{
    [Fact]
    public void AString_ShouldBeCompressedAndDecompressed()
    {
        const string mockString = "Test Compress And Decompress";
        var compressedBuffer = mockString.Compress();
        var decompressedString = compressedBuffer.Decompress();

        compressedBuffer.Should().NotBeEmpty();
        decompressedString.Should().Be(mockString);
    }
    
    [Fact]
    public void AStringShouldBeCompressedToBase64AndDecompressedFromBase64()
    {
        const string mockString = "Test Compress And Decompress";
        var compressedBuffer = mockString.CompressToBase64();
        var decompressedString = compressedBuffer.DecompressFromBase64();

        compressedBuffer.Should().NotBeEmpty();
        decompressedString.Should().Be(mockString);
    }
}