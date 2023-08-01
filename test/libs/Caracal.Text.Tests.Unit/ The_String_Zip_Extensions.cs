// ReSharper disable InconsistentNaming
namespace Caracal.Text.Tests.Unit;

[Trait("Category","Unit")]
public sealed class The_String_Zip_Extensions
{
    [Fact]
    public void Should_Compress_A_String_To_Bytes_And_Then_Decompress_The_Bytes_To_The_Original_String()
    {
        const string mockString = "Test Compress And Decompress";
        var compressedBuffer = mockString.Compress();
        var decompressedString = compressedBuffer.Decompress();

        compressedBuffer.Should().NotBeEmpty();
        decompressedString.Should().Be(mockString);
    }
    
    [Fact]
    public void  Should_Compress_A_String_To_A_Base64_String_And_Then_Decompress_The_Base64_String_To_The_Original_String()
    {
        const string mockString = "Test Compress And Decompress";
        var compressedBuffer = mockString.CompressToBase64();
        var decompressedString = compressedBuffer.DecompressFromBase64();

        compressedBuffer.Should().NotBeEmpty();
        decompressedString.Should().Be(mockString);
    }
}