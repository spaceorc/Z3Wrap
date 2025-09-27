using System.Runtime.InteropServices;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap;

/// <summary>
/// Represents a Z3 satisfiability solver that can check the satisfiability of logical formulas.
/// Supports constraint assertion, backtracking with push/pop operations, and model extraction.
/// </summary>
public sealed class Z3Solver : IDisposable
{
    private readonly Z3Context context;
    private IntPtr solverHandle;
    private bool disposed;
    private bool isBeingDisposedByContext;
    private Z3Model? cachedModel;
    private Z3Status? lastCheckResult;

    internal Z3Solver(Z3Context context, bool useSimpleSolver)
    {
        this.context = context;

        solverHandle = useSimpleSolver
            ? SafeNativeMethods.Z3MkSimpleSolver(context.Handle)
            : SafeNativeMethods.Z3MkSolver(context.Handle);

        if (solverHandle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to create Z3 solver");

        SafeNativeMethods.Z3SolverIncRef(context.Handle, solverHandle);
    }

    internal IntPtr Handle
    {
        get
        {
            ThrowIfDisposed();
            return solverHandle;
        }
    }

    internal IntPtr InternalHandle => solverHandle;

    /// <summary>
    /// Asserts a Boolean constraint to the solver. The solver will check satisfiability
    /// of all asserted constraints when Check() is called.
    /// </summary>
    /// <param name="constraint">The Boolean expression constraint to assert.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the solver has been disposed.</exception>
    public void Assert(BoolExpr constraint)
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after assertion

        SafeNativeMethods.Z3SolverAssert(context.Handle, solverHandle, constraint.Handle);
    }

    /// <summary>
    /// Removes all asserted constraints from the solver, returning it to an empty state.
    /// Any cached model becomes invalid.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown when the solver has been disposed.</exception>
    public void Reset()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after reset

        SafeNativeMethods.Z3SolverReset(context.Handle, solverHandle);
        lastCheckResult = null;
    }

    /// <summary>
    /// Checks the satisfiability of all currently asserted constraints.
    /// </summary>
    /// <returns>
    /// Satisfiable if the constraints can be satisfied,
    /// Unsatisfiable if they cannot be satisfied,
    /// Unknown if the solver cannot determine satisfiability.
    /// </returns>
    /// <exception cref="ObjectDisposedException">Thrown when the solver has been disposed.</exception>
    public Z3Status Check()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Clear any previous model

        var result = SafeNativeMethods.Z3SolverCheck(context.Handle, solverHandle);
        lastCheckResult = (Z3BoolValue)result switch
        {
            Z3BoolValue.False => Z3Status.Unsatisfiable,
            Z3BoolValue.True => Z3Status.Satisfiable,
            Z3BoolValue.Undefined => Z3Status.Unknown,
            _ => throw new InvalidOperationException(
                $"Unexpected boolean value result {result} from Z3_solver_check"
            ),
        };
        return lastCheckResult.Value;
    }

    /// <summary>
    /// Gets a string description of why the solver returned Unknown status.
    /// Only meaningful after Check() returns Z3Status.Unknown.
    /// </summary>
    /// <returns>A human-readable explanation of the unknown result.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the solver has been disposed.</exception>
    public string GetReasonUnknown()
    {
        ThrowIfDisposed();

        var reasonPtr = SafeNativeMethods.Z3SolverGetReasonUnknown(context.Handle, solverHandle);
        return Marshal.PtrToStringAnsi(reasonPtr) ?? "Unknown reason";
    }

    /// <summary>
    /// Creates a backtracking point by pushing the current solver state onto a stack.
    /// Constraints asserted after Push() can be removed with Pop().
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown when the solver has been disposed.</exception>
    public void Push()
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after push

        SafeNativeMethods.Z3SolverPush(context.Handle, solverHandle);
    }

    /// <summary>
    /// Removes constraints by popping solver states from the backtracking stack.
    /// Restores the solver to the state it was in before the corresponding Push() calls.
    /// </summary>
    /// <param name="numScopes">The number of scopes to pop (defaults to 1).</param>
    /// <exception cref="ObjectDisposedException">Thrown when the solver has been disposed.</exception>
    public void Pop(uint numScopes = 1)
    {
        ThrowIfDisposed();
        InvalidateModel(); // Model no longer valid after pop

        SafeNativeMethods.Z3SolverPop(context.Handle, solverHandle, numScopes);
    }

    /// <summary>
    /// Retrieves a model (satisfying assignment) for the constraints.
    /// Can only be called after Check() returns Satisfiable.
    /// </summary>
    /// <returns>A model containing variable assignments that satisfy all constraints.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the solver has been disposed.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when Check() has not been called, or when the last Check() result was not Satisfiable.
    /// </exception>
    public Z3Model GetModel()
    {
        ThrowIfDisposed();

        // Check if we have called Check() before
        if (lastCheckResult == null)
            throw new InvalidOperationException("Must call Check() before GetModel()");

        // Check if the result was satisfiable
        if (lastCheckResult != Z3Status.Satisfiable)
            throw new InvalidOperationException(
                $"Cannot get model when solver status is {lastCheckResult}"
            );

        // Return cached model if we have one
        if (cachedModel == null)
        {
            var modelHandle = SafeNativeMethods.Z3SolverGetModel(context.Handle, solverHandle);
            if (modelHandle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to get model from solver");

            cachedModel = new Z3Model(context, modelHandle);
        }

        return cachedModel;
    }

    private void InvalidateModel()
    {
        cachedModel?.Invalidate();
        cachedModel = null;
    }

    /// <summary>
    /// Releases all resources used by the solver.
    /// </summary>
    public void Dispose()
    {
        if (disposed)
            return;

        if (!isBeingDisposedByContext)
        {
            // Delegate disposal to context - it will call back to InternalDispose
            context.DisposeSolver(this);
        }

        disposed = true;
    }

    internal void InternalDispose()
    {
        if (disposed)
            return;

        // Clean up model before solver disposal
        InvalidateModel();

        isBeingDisposedByContext = true;
        solverHandle = IntPtr.Zero;
        disposed = true;
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, nameof(Z3Solver));
}
