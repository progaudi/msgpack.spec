using Shouldly;
using Xunit;

namespace ProGaudi.MsgPack.Tests.Reader
{
    public sealed class String
    {
        [Theory]
        [InlineData("asdf", new byte[] { 164, 97, 115, 100, 102 })]
        [InlineData("a", new byte[] { 161, 97 })]
        [InlineData("12345678901234567890", new byte[] { 180, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48 })]
        [InlineData("1234567890123456789012345678901234567890", new byte[] { 217, 40, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48 })]
        [InlineData("", new byte[] { 160 })]
        [InlineData("08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg", new byte[] {
                    218, 1, 80, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114,
                    49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49,
                    104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49,
                    104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103
                })]
        [InlineData("Мама мыла раму", new byte[] { 186, 208, 156, 208, 176, 208, 188, 208, 176, 32, 208, 188, 209, 139, 208, 187, 208, 176, 32, 209, 128, 208, 176, 208, 188, 209, 131 })]
        [InlineData("Шла Саша по шоссе и сосала сушку", new byte[] { 217, 58, 208, 168, 208, 187, 208, 176, 32, 208, 161, 208, 176, 209, 136, 208, 176, 32, 208, 191, 208, 190, 32, 209, 136, 208, 190, 209, 129, 209, 129, 208, 181, 32, 208, 184, 32, 209, 129, 208, 190, 209, 129, 208, 176, 208, 187, 208, 176, 32, 209, 129, 209, 131, 209, 136, 208, 186, 209, 131 })]
        [InlineData("❤", new byte [] { 0xa3, 0xe2, 0x9d, 0xa4 })]
        [InlineData("❤", new byte [] { 0xd9, 0x03, 0xe2, 0x9d, 0xa4 })]
        [InlineData("🍺", new byte [] { 0xa4, 0xf0, 0x9f, 0x8d, 0xba })]
        [InlineData("🍺", new byte [] { 0xd9, 0x04, 0xf0, 0x9f, 0x8d, 0xba })]
        [InlineData("Кириллица", new byte[] { 0xb2, 0xd0, 0x9a, 0xd0, 0xb8, 0xd1, 0x80, 0xd0, 0xb8, 0xd0, 0xbb, 0xd0, 0xbb, 0xd0, 0xb8, 0xd1, 0x86, 0xd0, 0xb0})]
        [InlineData("Кириллица", new byte[] { 0xd9, 0x12, 0xd0, 0x9a, 0xd0, 0xb8, 0xd1, 0x80, 0xd0, 0xb8, 0xd0, 0xbb, 0xd0, 0xbb, 0xd0, 0xb8, 0xd1, 0x86, 0xd0, 0xb0})]
        [InlineData("ひらがな", new byte[] { 0xac, 0xe3, 0x81, 0xb2, 0xe3, 0x82, 0x89, 0xe3, 0x81, 0x8c, 0xe3, 0x81, 0xaa})]
        [InlineData("ひらがな", new byte[] { 0xd9, 0x0c, 0xe3, 0x81, 0xb2, 0xe3, 0x82, 0x89, 0xe3, 0x81, 0x8c, 0xe3, 0x81, 0xaa})]
        [InlineData("한글", new byte[] { 0xa6, 0xed, 0x95, 0x9c, 0xea, 0xb8, 0x80})]
        [InlineData("한글", new byte[] { 0xd9, 0x06, 0xed, 0x95, 0x9c, 0xea, 0xb8, 0x80})]
        [InlineData("汉字", new byte[] { 0xa6, 0xe6, 0xb1, 0x89, 0xe5, 0xad, 0x97})]
        [InlineData("汉字", new byte[] { 0xd9, 0x06, 0xe6, 0xb1, 0x89, 0xe5, 0xad, 0x97})]
        [InlineData("漢字", new byte[] { 0xa6, 0xe6, 0xbc, 0xa2, 0xe5, 0xad, 0x97 })]
        [InlineData("漢字", new byte[] { 0xd9, 0x06, 0xe6, 0xbc, 0xa2, 0xe5, 0xad, 0x97 })]
        public void TryRead(string s, byte[] data)
        {
            MsgPackSpec.TryReadString(data, out var actual, out var readSize).ShouldBeTrue();
            actual.ShouldBe(s);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData("asdf", new byte[] { 164, 97, 115, 100, 102 })]
        [InlineData("a", new byte[] { 161, 97 })]
        [InlineData("12345678901234567890", new byte[] { 180, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48 })]
        [InlineData("1234567890123456789012345678901234567890", new byte[] { 217, 40, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48 })]
        [InlineData("", new byte[] { 160 })]
        [InlineData("08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg", new byte[] {
                    218, 1, 80, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114,
                    49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49,
                    104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49,
                    104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103
                })]
        [InlineData("Мама мыла раму", new byte[] { 186, 208, 156, 208, 176, 208, 188, 208, 176, 32, 208, 188, 209, 139, 208, 187, 208, 176, 32, 209, 128, 208, 176, 208, 188, 209, 131 })]
        [InlineData("Шла Саша по шоссе и сосала сушку", new byte[] { 217, 58, 208, 168, 208, 187, 208, 176, 32, 208, 161, 208, 176, 209, 136, 208, 176, 32, 208, 191, 208, 190, 32, 209, 136, 208, 190, 209, 129, 209, 129, 208, 181, 32, 208, 184, 32, 209, 129, 208, 190, 209, 129, 208, 176, 208, 187, 208, 176, 32, 209, 129, 209, 131, 209, 136, 208, 186, 209, 131 })]
        [InlineData("❤", new byte [] { 0xa3, 0xe2, 0x9d, 0xa4 })]
        [InlineData("❤", new byte [] { 0xd9, 0x03, 0xe2, 0x9d, 0xa4 })]
        [InlineData("🍺", new byte [] { 0xa4, 0xf0, 0x9f, 0x8d, 0xba })]
        [InlineData("🍺", new byte [] { 0xd9, 0x04, 0xf0, 0x9f, 0x8d, 0xba })]
        [InlineData("Кириллица", new byte[] { 0xb2, 0xd0, 0x9a, 0xd0, 0xb8, 0xd1, 0x80, 0xd0, 0xb8, 0xd0, 0xbb, 0xd0, 0xbb, 0xd0, 0xb8, 0xd1, 0x86, 0xd0, 0xb0})]
        [InlineData("Кириллица", new byte[] { 0xd9, 0x12, 0xd0, 0x9a, 0xd0, 0xb8, 0xd1, 0x80, 0xd0, 0xb8, 0xd0, 0xbb, 0xd0, 0xbb, 0xd0, 0xb8, 0xd1, 0x86, 0xd0, 0xb0})]
        [InlineData("ひらがな", new byte[] { 0xac, 0xe3, 0x81, 0xb2, 0xe3, 0x82, 0x89, 0xe3, 0x81, 0x8c, 0xe3, 0x81, 0xaa})]
        [InlineData("ひらがな", new byte[] { 0xd9, 0x0c, 0xe3, 0x81, 0xb2, 0xe3, 0x82, 0x89, 0xe3, 0x81, 0x8c, 0xe3, 0x81, 0xaa})]
        [InlineData("한글", new byte[] { 0xa6, 0xed, 0x95, 0x9c, 0xea, 0xb8, 0x80})]
        [InlineData("한글", new byte[] { 0xd9, 0x06, 0xed, 0x95, 0x9c, 0xea, 0xb8, 0x80})]
        [InlineData("汉字", new byte[] { 0xa6, 0xe6, 0xb1, 0x89, 0xe5, 0xad, 0x97})]
        [InlineData("汉字", new byte[] { 0xd9, 0x06, 0xe6, 0xb1, 0x89, 0xe5, 0xad, 0x97})]
        [InlineData("漢字", new byte[] { 0xa6, 0xe6, 0xbc, 0xa2, 0xe5, 0xad, 0x97 })]
        [InlineData("漢字", new byte[] { 0xd9, 0x06, 0xe6, 0xbc, 0xa2, 0xe5, 0xad, 0x97 })]
        public void Read(string s, byte[] data)
        {
            MsgPackSpec.ReadString(data, out var readSize).ShouldBe(s);
            readSize.ShouldBe(data.Length);
        }
    }
}
