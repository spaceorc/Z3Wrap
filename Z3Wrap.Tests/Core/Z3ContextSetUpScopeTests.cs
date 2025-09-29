using Spaceorc.Z3Wrap.Core;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3ContextSetUpScopeTests
{
    [Test]
    public void IsCurrentContextSet_Initially_False()
    {
        Assert.That(Z3Context.IsCurrentContextSet, Is.False);
    }

    [Test]
    public void SetUp_SetsCurrentContext()
    {
        using var context = new Z3Context();

        using var scope = context.SetUp();

        Assert.That(Z3Context.IsCurrentContextSet, Is.True);
        Assert.That(Z3Context.Current, Is.SameAs(context));
    }

    [Test]
    public void SetUp_AfterDispose_RestoresState()
    {
        using var context = new Z3Context();

        using (var scope = context.SetUp())
        {
            Assert.That(Z3Context.IsCurrentContextSet, Is.True);
        }

        Assert.That(Z3Context.IsCurrentContextSet, Is.False);
    }

    [Test]
    public void SetUp_Nested_RestoresPreviousContext()
    {
        using var context1 = new Z3Context();
        using var context2 = new Z3Context();

        using (var scope1 = context1.SetUp())
        {
            Assert.That(Z3Context.Current, Is.SameAs(context1));

            using (var scope2 = context2.SetUp())
            {
                Assert.That(Z3Context.Current, Is.SameAs(context2));
            }

            Assert.That(Z3Context.Current, Is.SameAs(context1));
        }
    }

    [Test]
    public void Current_WithoutSetUp_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => _ = Z3Context.Current);
    }
}
