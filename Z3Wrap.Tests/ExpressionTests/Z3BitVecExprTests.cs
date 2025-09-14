namespace Z3Wrap.Tests.ExpressionTests;

[TestFixture]
public class Z3BitVecExprTests
{
    [Test]
    public void BitVecConst_CreatesVariableWithCorrectSize()
    {
        using var context = new Z3Context();
        var bv = context.BitVecConst("x", 32);

        Assert.That(bv, Is.Not.Null);
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(bv.Context, Is.EqualTo(context));
        Assert.That(bv.Size, Is.EqualTo(32));
    }
}