using Bogus;
using DrivingSchoolApi.Domain.ValueObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace DrivingSchoolApi.Fakers.ValueObject;

public class SignatureFaker : Faker<Signature>
{
    private SignatureFaker() {}

    public static SignatureFaker Create(int seed)
    {
        var faker = new SignatureFaker();

        faker.UseSeed(seed);
        faker.CustomInstantiator(f =>
        {
            using var image = new Image<A8>(120, 80);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var value = f.PickRandom(new List<byte>([0, 255]));
                    image[x, y] = new A8(value);
                }
            }

            using var ms = new MemoryStream();
            image.Save(ms, new PngEncoder());

            return Signature.Create(ms.ToArray());
        });

        return faker;
    }
}