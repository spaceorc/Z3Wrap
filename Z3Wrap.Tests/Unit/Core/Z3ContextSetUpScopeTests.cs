using Spaceorc.Z3Wrap;

namespace Z3Wrap.Tests.Unit.Core;

[TestFixture]
public class Z3ContextSetUpScopeTests
{
    [Test]
    public void Current_InitiallyNotSet()
    {
        Assert.That(Z3Context.IsCurrentContextSet, Is.False);

        // Verify that accessing Current throws
        Assert.Throws<InvalidOperationException>(() => _ = Z3Context.Current);
    }

    [Test]
    public void SetUp_SetsCurrentContext()
    {
        using var context = new Z3Context();

        Assert.That(Z3Context.IsCurrentContextSet, Is.False);

        using var scope = context.SetUp();

        Assert.That(Z3Context.Current, Is.EqualTo(context));
    }

    [Test]
    public void SetUp_DisposalRestoresNull()
    {
        using var context = new Z3Context();

        Assert.That(Z3Context.IsCurrentContextSet, Is.False);

        {
            using var scope = context.SetUp();
            Assert.That(Z3Context.Current, Is.EqualTo(context));
        }

        Assert.That(Z3Context.IsCurrentContextSet, Is.False);
    }

    [Test]
    public void SetUp_NestedScopes_WorkCorrectly()
    {
        using var context1 = new Z3Context();
        using var context2 = new Z3Context();

        Assert.That(Z3Context.IsCurrentContextSet, Is.False);

        using (context1.SetUp())
        {
            Assert.That(Z3Context.Current, Is.EqualTo(context1));

            using (context2.SetUp())
            {
                Assert.That(Z3Context.Current, Is.EqualTo(context2));
            }

            // After inner scope disposal, should restore to context1
            Assert.That(Z3Context.Current, Is.EqualTo(context1));
        }

        // After outer scope disposal, should restore to null
        Assert.That(Z3Context.IsCurrentContextSet, Is.False);
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
    public void SetUp_SameContext_MultipleScopes()
    {
        using var context = new Z3Context();

        // First scope
        using (context.SetUp())
        {
            Assert.That(Z3Context.Current, Is.EqualTo(context));
        }

        Assert.That(Z3Context.IsCurrentContextSet, Is.False);

        // Second scope with same context
        using (context.SetUp())
        {
            Assert.That(Z3Context.Current, Is.EqualTo(context));
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
    public void Current_PropertyAccess_ThreadSafe()
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
}
