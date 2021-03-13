using System;
using System.Collections.Generic;
using System.Text;
using BCnEncoder.Encoder;
using BCnEncTests.Support;
using Xunit;
using BCnEncoder.ImageSharp;

namespace BCnEncTests
{
	public class EncoderOptionsTests
	{

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(5)]
		[InlineData(10)]
		public void MaxMipMaps(int requestedMipMaps)
		{
			var testImage = ImageLoader.TestBlur1;
			var encoder = new BcEncoder()
			{
				OutputOptions =
				{
					GenerateMipMaps = true,
					MaxMipMapLevel = requestedMipMaps
				}
			};

			Assert.Equal(requestedMipMaps, encoder.CalculateNumberOfMipLevels(testImage));

			var ktx = encoder.EncodeToKtx(testImage);

			Assert.Equal(requestedMipMaps, (int)ktx.header.NumberOfMipmapLevels);
			Assert.Equal(requestedMipMaps, ktx.MipMaps.Count);

			var dds = encoder.EncodeToDds(testImage);

			Assert.Equal(requestedMipMaps, (int)dds.header.dwMipMapCount);
			Assert.Equal(requestedMipMaps, dds.Faces[0].MipMaps.Length);
		}

		[Fact]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<Pending>")]
		public void GenerateMipMaps()
		{
			var testImage = ImageLoader.TestBlur1;
			const int requestedMipMaps = 1;
			var encoder = new BcEncoder()
			{
				OutputOptions =
				{
					GenerateMipMaps = false
				}
			};

			Assert.Equal(requestedMipMaps, encoder.CalculateNumberOfMipLevels(testImage));

			var ktx = encoder.EncodeToKtx(testImage);

			Assert.Equal(requestedMipMaps, (int)ktx.header.NumberOfMipmapLevels);
			Assert.Equal(requestedMipMaps, (int)ktx.MipMaps.Count);

			var dds = encoder.EncodeToDds(testImage);

			Assert.Equal(requestedMipMaps, (int)dds.header.dwMipMapCount);
			Assert.Equal(requestedMipMaps, dds.Faces[0].MipMaps.Length);
		}
	}
}
