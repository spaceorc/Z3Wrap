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

        var str = parameters.ToString();
        Assert.That(str, Does.Contain("test_bool"));
    }

    [Test]
    public void Set_UInt_SetsParameter()
    {
        var parameters = new Z3Params();

        parameters.Set("test_uint", 42u);

        var str = parameters.ToString();
        Assert.That(str, Does.Contain("test_uint"));
    }

    [Test]
    public void Set_Double_SetsParameter()
    {
        var parameters = new Z3Params();

        parameters.Set("test_double", 3.14);

        var str = parameters.ToString();
        Assert.That(str, Does.Contain("test_double"));
    }

    [Test]
    public void Set_String_SetsParameter()
    {
        var parameters = new Z3Params();

        parameters.Set("test_string", "value");

        var str = parameters.ToString();
        Assert.That(str, Does.Contain("test_string"));
    }

    [Test]
    public void Set_FluentChaining_WorksCorrectly()
    {
        var parameters = new Z3Params();

        var result = parameters.Set("bool_param", true).Set("uint_param", 10u).Set("double_param", 2.5);

        Assert.That(result, Is.SameAs(parameters));
        var str = parameters.ToString();
        Assert.That(str, Does.Contain("bool_param"));
        Assert.That(str, Does.Contain("uint_param"));
        Assert.That(str, Does.Contain("double_param"));
    }

    [Test]
    public void ToString_ReturnsNonEmpty()
    {
        var parameters = new Z3Params();
        parameters.Set("test", true);

        var str = parameters.ToString();

        Assert.That(str, Is.Not.Null.And.Not.Empty);
    }
}
