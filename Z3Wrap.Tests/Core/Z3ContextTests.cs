using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Expressions.Quantifiers;

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

        using var context = new Z3Context(parameters);

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

    [Test]
    public void SetUp_MultipleContexts_IndependentScopes()
    {
        using var context1 = new Z3Context();
        using var context2 = new Z3Context();

        // First context setup
        using (context1.SetUp())
        {
            Assert.That(Z3Context.Current, Is.EqualTo(context1));
        }

        Assert.That(Z3Context.IsCurrentContextSet, Is.False);

        // Second context setup (independent)
        using (context2.SetUp())
        {
            Assert.That(Z3Context.Current, Is.EqualTo(context2));
        }

        Assert.That(Z3Context.IsCurrentContextSet, Is.False);
    }

    [Test]
    public void SetUp_ExceptionDuringScope_RestoresCorrectly()
    {
        using var context = new Z3Context();

        Assert.That(Z3Context.IsCurrentContextSet, Is.False);

        try
        {
            using var scope = context.SetUp();
            Assert.That(Z3Context.Current, Is.EqualTo(context));

            // Simulate exception
            throw new InvalidOperationException("Test exception");
        }
        catch (InvalidOperationException)
        {
            // Expected exception
        }

        // Context should be restored to null even after exception
        Assert.That(Z3Context.IsCurrentContextSet, Is.False);
    }

    [Test]
    public void SetUp_NestedWithException_RestoresCorrectly()
    {
        using var context1 = new Z3Context();
        using var context2 = new Z3Context();

        using var scope1 = context1.SetUp();
        Assert.That(Z3Context.Current, Is.EqualTo(context1));

        try
        {
            using var scope2 = context2.SetUp();
            Assert.That(Z3Context.Current, Is.EqualTo(context2));

            throw new InvalidOperationException("Test exception");
        }
        catch (InvalidOperationException)
        {
            // Expected exception
        }

        // Should restore to context1, not null
        Assert.That(Z3Context.Current, Is.EqualTo(context1));
    }

    [Test]
    public void SetUp_DisposedContext_ThrowsException()
    {
        var context = new Z3Context();
        context.Dispose();

        Assert.Throws<ObjectDisposedException>(() => context.SetUp());
    }

    [Test]
    public void SetUp_ThreadLocal_IsolationBetweenThreads()
    {
        var thread1Ready = new ManualResetEventSlim(false);
        var thread2Ready = new ManualResetEventSlim(false);
        var continueSignal = new ManualResetEventSlim(false);

        Z3Context? thread1Context = null;
        Z3Context? thread2Context = null;

        var task1 = Task.Run(() =>
        {
            // Each thread creates its own context - Z3 contexts are not thread-safe
            using var context = new Z3Context();
            using var scope = context.SetUp();

            thread1Context = Z3Context.Current;
            thread1Ready.Set();

            // Wait for both threads to be ready
            continueSignal.Wait();

            // Verify context is still correct after other thread operations
            Assert.That(Z3Context.Current, Is.EqualTo(context));
        });

        var task2 = Task.Run(() =>
        {
            // Each thread creates its own context - Z3 contexts are not thread-safe
            using var context = new Z3Context();
            using var scope = context.SetUp();

            thread2Context = Z3Context.Current;
            thread2Ready.Set();

            // Wait for both threads to be ready
            continueSignal.Wait();

            // Verify context is still correct after other thread operations
            Assert.That(Z3Context.Current, Is.EqualTo(context));
        });

        // Wait for both threads to set up their contexts
        thread1Ready.Wait();
        thread2Ready.Wait();

        // Verify each thread has its own different context
        Assert.That(thread1Context, Is.Not.Null);
        Assert.That(thread2Context, Is.Not.Null);
        Assert.That(thread1Context, Is.Not.EqualTo(thread2Context));

        // Signal threads to continue and complete
        continueSignal.Set();

        // Wait for completion
        Task.WaitAll(task1, task2);

        // Main thread should still have no current context
        Assert.That(Z3Context.IsCurrentContextSet, Is.False);
    }

    [Test]
    public void SetUp_ConcurrentAccess_ThreadSafe()
    {
        const int threadCount = 10;
        const int accessesPerThread = 100;

        var tasks = new Task[threadCount];
        var exceptions = new List<Exception>();

        for (var i = 0; i < threadCount; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                try
                {
                    // Each thread creates its own context - Z3 contexts are not thread-safe
                    using var context = new Z3Context();

                    for (var j = 0; j < accessesPerThread; j++)
                    {
                        // Rapid setup/teardown and access
                        using var scope = context.SetUp();
                        var current = Z3Context.Current;
                        Assert.That(current, Is.EqualTo(context));

                        // Access multiple times within scope
                        Assert.That(Z3Context.Current, Is.EqualTo(context));
                        Assert.That(Z3Context.Current, Is.EqualTo(context));
                    }
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            });
        }

        Task.WaitAll(tasks);

        // Should be no exceptions from concurrent access
        Assert.That(
            exceptions,
            Is.Empty,
            $"Concurrent access caused {exceptions.Count} exceptions: {string.Join(", ", exceptions.Select(e => e.Message))}"
        );

        // Main thread should have no current context
        Assert.That(Z3Context.IsCurrentContextSet, Is.False);
    }

    [Test]
    public void ErrorHandling_MultipleErrors_DoesNotCrashProcess()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var body = x > 0;

        // Multiple invalid quantifier calls should not crash the process
        // (passing constant instead of bound variable triggers Z3_INVALID_ARG)
        for (int i = 0; i < 3; i++)
        {
            var e = Assert.Throws<Z3Exception>(() => context.ForAll(context.Int(i), body));
            Assert.Multiple(() =>
            {
                Assert.That(e.ErrorCode, Is.EqualTo(Z3ErrorCode.InvalidArgument));
                Assert.That(e.Message, Does.Contain("InvalidArgument"));
            });
        }

        // Process should still be alive and context should still work
        var validQuantifier = context.ForAll(x, body);
        Assert.That(validQuantifier, Is.Not.Null);
    }
}
