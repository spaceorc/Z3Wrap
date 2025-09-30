using Spaceorc.Z3Wrap.Core;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3ParamsTests
{
    [Test]
    public void Set_Bool_SetsParameter()
    {
        var parameters = new Z3Params();

        parameters.Set("test_bool", true);

        Assert.That(parameters, Contains.Item(new KeyValuePair<string, object>("test_bool", true)));
    }

    [Test]
    public void Set_UInt_SetsParameter()
    {
        var parameters = new Z3Params();

        parameters.Set("test_uint", 42u);

        Assert.That(parameters, Contains.Item(new KeyValuePair<string, object>("test_uint", 42u)));
    }

    [Test]
    public void Set_Double_SetsParameter()
    {
        var parameters = new Z3Params();

        parameters.Set("test_double", 3.14);

        Assert.That(parameters, Contains.Item(new KeyValuePair<string, object>("test_double", 3.14)));
    }

    [Test]
    public void Set_String_SetsParameter()
    {
        var parameters = new Z3Params();

        parameters.Set("test_string", "value");

        Assert.That(parameters, Contains.Item(new KeyValuePair<string, object>("test_string", "value")));
    }

    [Test]
    public void Set_FluentChaining_WorksCorrectly()
    {
        var parameters = new Z3Params();

        var result = parameters.Set("bool_param", true).Set("uint_param", 10u).Set("double_param", 2.5);

        Assert.That(result, Is.SameAs(parameters));
        Assert.That(parameters, Contains.Item(new KeyValuePair<string, object>("bool_param", true)));
        Assert.That(parameters, Contains.Item(new KeyValuePair<string, object>("uint_param", 10u)));
        Assert.That(parameters, Contains.Item(new KeyValuePair<string, object>("double_param", 2.5)));
    }

    [Test]
    public void ToString_ReturnsNonEmpty()
    {
        var parameters = new Z3Params();
        parameters.Set("test", true);

        var str = parameters.ToString();

        Assert.That(str, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void GetEnumerator_CanIterateParameters()
    {
        var parameters = new Z3Params();
        parameters.Set("param1", true);
        parameters.Set("param2", 42u);
        parameters.Set("param3", "test_value");
        parameters.Set("param4", 45.6);

        Assert.That(
            parameters.ToArray(),
            Is.EqualTo(
                new[]
                {
                    new KeyValuePair<string, object>("param1", true),
                    new KeyValuePair<string, object>("param2", 42u),
                    new KeyValuePair<string, object>("param3", "test_value"),
                    new KeyValuePair<string, object>("param4", 45.6),
                }
            )
        );
    }
}
