using Z3Wrap.DataTypes;

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

    [Test]
    public void BitVec_FromInt_CreatesValueWithCorrectSize()
    {
        using var context = new Z3Context();
        var bv = context.BitVec(new BitVec(42, 32));

        Assert.That(bv, Is.Not.Null);
        Assert.That(bv.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(bv.Context, Is.EqualTo(context));
        Assert.That(bv.Size, Is.EqualTo(32));
    }

    [Test]
    public void BitVec_FromInt_HandlesZero()
    {
        using var context = new Z3Context();
        var bv = context.BitVec(new BitVec(0, 16));

        Assert.That(bv, Is.Not.Null);
        Assert.That(bv.Size, Is.EqualTo(16));
    }

    [Test]
    public void BitVec_FromInt_HandlesNegativeValues()
    {
        using var context = new Z3Context();
        var bv = context.BitVec(new BitVec(-1, 32));

        Assert.That(bv, Is.Not.Null);
        Assert.That(bv.Size, Is.EqualTo(32));
    }

    [Test]
    public void BitVec_FromInt_HandlesDifferentSizes()
    {
        using var context = new Z3Context();

        var bv8 = context.BitVec(new BitVec(255, 8));
        var bv16 = context.BitVec(new BitVec(65535, 16));
        var bv64 = context.BitVec(new BitVec(123456789, 64));

        Assert.That(bv8.Size, Is.EqualTo(8));
        Assert.That(bv16.Size, Is.EqualTo(16));
        Assert.That(bv64.Size, Is.EqualTo(64));
    }

    [Test]
    public void BitVec_FromInt_DoesNotCrash()
    {
        using var context = new Z3Context();

        Assert.DoesNotThrow(() =>
        {
            for (int i = 0; i < 100; i++)
            {
                var bv = context.BitVec(new BitVec(i, 32));
                Assert.That(bv, Is.Not.Null);
            }
        });
    }
}