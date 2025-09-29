using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3ContextTests
{
    [Test]
    public void Constructor_Default_CreatesContext()
    {
        using var context = new Z3Context();

        Assert.That(context.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Constructor_WithParameters_CreatesContext()
    {
        var parameters = new Dictionary<string, string> { { "model", "true" } };

        using var context = new Z3Context(parameters: parameters);

        Assert.That(context.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Constructor_WithLibrary_UsesProvidedLibrary()
    {
        var library = Z3Library.LoadAuto();

        using var context = new Z3Context(library: library);

        Assert.That(context.Library, Is.SameAs(library));
    }

    [Test]
    public void SetParameter_AffectsBehavior()
    {
        using var context = new Z3Context(new Dictionary<string, string> { { "model", "true" } });
        using var scope = context.SetUp();
        
        using var solverWithParam = context.CreateSolver();
        var x = context.IntConst("x");
        solverWithParam.Assert(x == 42);
        Assert.That(solverWithParam.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var modelWithParam = solverWithParam.GetModel();
        Assert.That(modelWithParam.GetIntValue(x), Is.EqualTo(new BigInteger(42)));

        context.SetParameter("model", "false");
        using var solverWithoutParam = context.CreateSolver();
        var y = context.IntConst("y");
        solverWithoutParam.Assert(y == 42);
        Assert.That(solverWithoutParam.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var modelWithoutParam = solverWithoutParam.GetModel();
        Assert.That(modelWithoutParam.GetIntValue(y), Is.EqualTo(new BigInteger(0)));
    }

    [Test]
    public void CreateSolver_ReturnsValidSolver()
    {
        using var context = new Z3Context();

        using var solver = context.CreateSolver();

        Assert.That(solver.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CreateSimpleSolver_ReturnsValidSolver()
    {
        using var context = new Z3Context();

        using var solver = context.CreateSimpleSolver();

        Assert.That(solver.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Dispose_MultipleCalls_DoesNotThrow()
    {
        var context = new Z3Context();
        context.Dispose();

        Assert.DoesNotThrow(() => context.Dispose());
    }

    [Test]
    public void Handle_AfterDispose_ThrowsObjectDisposedException()
    {
        var context = new Z3Context();
        context.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = context.Handle);
    }

    [Test]
    public void Library_ReturnsNonNull()
    {
        using var context = new Z3Context();

        var library = context.Library;

        Assert.That(library, Is.Not.Null);
    }
}
