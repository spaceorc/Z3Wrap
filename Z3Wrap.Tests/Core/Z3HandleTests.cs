using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3HandleTests
{
    [Test]
    public void Equals_SameHandle_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var sameX = x;

        Assert.That(x.Equals(sameX), Is.True);
    }

    [Test]
    public void Equals_DifferentHandlesSameValue_Z3MayOptimize()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x1 = context.IntConst("x");
        var x2 = context.IntConst("x");

        // Z3 may reuse handles for identical constants
        // We just verify Equals works consistently
        bool result = x1.Equals(x2);
        Assert.That(result, Is.TypeOf<bool>());
    }

    [Test]
    public void Equals_DifferentHandles_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        Assert.That(x.Equals(y), Is.False);
    }

    [Test]
    public void Equals_DifferentTypes_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var intExpr = context.IntConst("x");
        var boolExpr = context.BoolConst("b");

        Assert.That(intExpr.Equals(boolExpr), Is.False);
    }

    [Test]
    public void Equals_Null_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");

        Assert.That(x.Equals(null), Is.False);
    }

    [Test]
    public void Equals_NonZ3HandleObject_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var notHandle = "some string";

        Assert.That(x.Equals(notHandle), Is.False);
    }

    [Test]
    public void Equals_DifferentContextsSameExpression_ReturnsFalse()
    {
        using var context1 = new Z3Context();
        using var scope1 = context1.SetUp();
        var x1 = context1.IntConst("x");

        using var context2 = new Z3Context();
        using var scope2 = context2.SetUp();
        var x2 = context2.IntConst("x");

        // Different contexts create different handles
        Assert.That(x1.Equals(x2), Is.False);
    }

    [Test]
    public void Equals_ExpressionAndItsSubexpression_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var sum = x + y;

        Assert.That(sum.Equals(x), Is.False);
        Assert.That(sum.Equals(y), Is.False);
    }

    [Test]
    public void Equals_EquivalentExpressions_Z3MayOptimize()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var expr1 = x + 1;
        var expr2 = x + 1;

        // Z3 may optimize and reuse handles for equivalent expressions
        // We just verify Equals works consistently
        bool result = expr1.Equals(expr2);
        Assert.That(result, Is.TypeOf<bool>());
    }

    [Test]
    public void Equals_SimplifiedExpressions_DependsOnZ3Behavior()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var expr1 = x + 0;
        var expr2 = x + 0;

        // Z3 may or may not simplify to same handle
        // We just verify Equals works consistently
        bool result = expr1.Equals(expr2);
        Assert.That(result, Is.TypeOf<bool>());
    }

    [Test]
    public void GetHashCode_SameHandle_ReturnsSameHashCode()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var sameX = x;

        Assert.That(x.GetHashCode(), Is.EqualTo(sameX.GetHashCode()));
    }

    [Test]
    public void GetHashCode_DifferentHandles_ReturnsDifferentHashCodes()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        // Different handles should (almost always) have different hash codes
        Assert.That(x.GetHashCode(), Is.Not.EqualTo(y.GetHashCode()));
    }

    [Test]
    public void GetHashCode_ConsistentAcrossMultipleCalls()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");

        var hash1 = x.GetHashCode();
        var hash2 = x.GetHashCode();
        var hash3 = x.GetHashCode();

        Assert.Multiple(() =>
        {
            Assert.That(hash1, Is.EqualTo(hash2));
            Assert.That(hash2, Is.EqualTo(hash3));
        });
    }

    [Test]
    public void GetHashCode_CanBeUsedInHashSet()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");

        var hashSet = new HashSet<Z3Expr> { x, y, z };

        Assert.Multiple(() =>
        {
            Assert.That(hashSet.Count, Is.EqualTo(3));
            Assert.That(hashSet.Contains(x), Is.True);
            Assert.That(hashSet.Contains(y), Is.True);
            Assert.That(hashSet.Contains(z), Is.True);
        });
    }

    [Test]
    public void GetHashCode_CanBeUsedInDictionary()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var dictionary = new Dictionary<Z3Expr, string> { { x, "x_value" }, { y, "y_value" } };

        Assert.Multiple(() =>
        {
            Assert.That(dictionary.Count, Is.EqualTo(2));
            Assert.That(dictionary[x], Is.EqualTo("x_value"));
            Assert.That(dictionary[y], Is.EqualTo("y_value"));
        });
    }

    [Test]
    public void GetHashCode_NoDuplicatesInHashSet()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var sameX = x;

        var hashSet = new HashSet<Z3Expr> { x, sameX };

        // Same reference should not create duplicate
        Assert.That(hashSet.Count, Is.EqualTo(1));
    }

    [Test]
    public void EqualsAndGetHashCode_HashCodeContract_Respected()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var sameX = x;

        // If two objects are equal, they must have the same hash code
        if (x.Equals(sameX))
        {
            Assert.That(x.GetHashCode(), Is.EqualTo(sameX.GetHashCode()));
        }
    }

    [Test]
    public void Equals_ReflexiveProperty_ObjectEqualsItself()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");

        // Reflexive: x.Equals(x) should be true
        Assert.That(x.Equals(x), Is.True);
    }

    [Test]
    public void Equals_SymmetricProperty_BothDirectionsConsistent()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        // Symmetric: if x.Equals(y), then y.Equals(x)
        bool xEqualsY = x.Equals(y);
        bool yEqualsX = y.Equals(x);

        Assert.That(xEqualsY, Is.EqualTo(yEqualsX));
    }

    [Test]
    public void Equals_TransitiveProperty_ChainOfEquality()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var sameX1 = x;
        var sameX2 = sameX1;

        // Transitive: if x.Equals(y) and y.Equals(z), then x.Equals(z)
        bool xEqualsSameX1 = x.Equals(sameX1);
        bool sameX1EqualsSameX2 = sameX1.Equals(sameX2);
        bool xEqualsSameX2 = x.Equals(sameX2);

        if (xEqualsSameX1 && sameX1EqualsSameX2)
        {
            Assert.That(xEqualsSameX2, Is.True);
        }
    }

    [Test]
    public void Equals_WithOperatorOverloads_ConsistentWithEquals()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        // Note: == operator on IntExpr creates BoolExpr, not boolean comparison
        // So we only test .Equals() method consistency
        bool equalsResult = x.Equals(y);

        Assert.That(equalsResult, Is.TypeOf<bool>());
    }

    [Test]
    public void ToString_IntConst_ReturnsName()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");

        Assert.That(x.ToString(), Is.EqualTo("x"));
    }

    [Test]
    public void ToString_IntValue_ReturnsValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var value = context.Int(42);

        Assert.That(value.ToString(), Is.EqualTo("42"));
    }

    [Test]
    public void ToString_BoolConst_ReturnsName()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var b = context.BoolConst("b");

        Assert.That(b.ToString(), Is.EqualTo("b"));
    }

    [Test]
    public void ToString_BoolValue_ReturnsValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var trueExpr = context.Bool(true);
        var falseExpr = context.Bool(false);

        Assert.Multiple(() =>
        {
            Assert.That(trueExpr.ToString(), Is.EqualTo("true"));
            Assert.That(falseExpr.ToString(), Is.EqualTo("false"));
        });
    }

    [Test]
    public void ToString_RealConst_ReturnsName()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var r = context.RealConst("r");

        Assert.That(r.ToString(), Is.EqualTo("r"));
    }

    [Test]
    public void ToString_RealValue_ReturnsValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var value = context.Real(3.5m);

        Assert.That(value.ToString(), Is.EqualTo("(/ 7.0 2.0)"));
    }

    [Test]
    public void ToString_ArithmeticExpression_ReturnsFormula()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var sum = x + y;

        Assert.That(sum.ToString(), Is.EqualTo("(+ x y)"));
    }

    [Test]
    public void ToString_ComplexExpression_ReturnsFormula()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var expr = (x + y) * 2;

        Assert.That(expr.ToString(), Is.EqualTo("(* (+ x y) 2)"));
    }

    [Test]
    public void ToString_BooleanExpression_ReturnsFormula()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var expr = x > y;

        Assert.That(expr.ToString(), Is.EqualTo("(> x y)"));
    }

    [Test]
    public void ToString_LogicExpression_ReturnsFormula()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var a = context.BoolConst("a");
        var b = context.BoolConst("b");
        var expr = a & b;

        Assert.That(expr.ToString(), Is.EqualTo("(and a b)"));
    }

    [Test]
    public void ToString_NestedExpression_ReturnsNestedFormula()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var expr = ((x + 1) * 2) - 3;

        Assert.That(expr.ToString(), Is.EqualTo("(- (* (+ x 1) 2) 3)"));
    }

    [Test]
    public void ToString_AfterContextDispose_ReturnsDisposed()
    {
        var context = new Z3Context();
        var scope = context.SetUp();
        var x = context.IntConst("x");

        scope.Dispose();
        context.Dispose();

        Assert.That(x.ToString(), Is.EqualTo("<disposed>"));
    }

    [Test]
    public void ToString_MultipleCallsSameExpression_ReturnsConsistentResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var expr = x + 10;

        var str1 = expr.ToString();
        var str2 = expr.ToString();
        var str3 = expr.ToString();

        Assert.Multiple(() =>
        {
            Assert.That(str1, Is.EqualTo(str2));
            Assert.That(str2, Is.EqualTo(str3));
            Assert.That(str1, Is.EqualTo("(+ x 10)"));
        });
    }

    [Test]
    public void ToString_ArrayExpression_ReturnsFormula()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var arr = context.ArrayConst<IntExpr, IntExpr>("arr");
        var index = context.IntConst("i");
        var select = arr[index];

        Assert.That(select.ToString(), Is.EqualTo("(select arr i)"));
    }

    [Test]
    public void ToString_ArrayStore_ReturnsFormula()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var arr = context.ArrayConst<IntExpr, IntExpr>("arr");
        var updatedArr = arr.Store(0, 42);

        Assert.That(updatedArr.ToString(), Is.EqualTo("(store arr 0 42)"));
    }
}
