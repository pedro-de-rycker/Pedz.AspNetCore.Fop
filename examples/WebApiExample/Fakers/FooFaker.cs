using Bogus;
using WebApiExample.Entities;

namespace WebApiExample.Fakers;

public static class FooFaker
{
    public static Faker<FooEntity> GetFaker()
    {
        return new Faker<FooEntity>()
            .RuleFor(f => f.FooProperty, f => f.Random.String2(10))
            .RuleFor(f => f.MyProperty, f => f.Random.String2(10))
            .RuleFor(f => f.TestProperty, f => f.Random.Int())
            .RuleFor(f => f.DateTimeOffsetProperty, f => f.Date.PastOffset());

        throw new NotImplementedException();
    }
}
