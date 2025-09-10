using System.Runtime.InteropServices;

namespace z3lib;

public static class NativeMethods
{
    private static IntPtr libraryHandle;

    private static readonly Dictionary<string, IntPtr> FunctionPointers = new();

    public static void LoadLibrary(string libraryPath)
    {
        if (libraryHandle != IntPtr.Zero)
        {
            NativeLibrary.Free(libraryHandle);
            FunctionPointers.Clear();
        }

        libraryHandle = NativeLibrary.Load(libraryPath);

        // Load function pointers
        LoadFunction("Z3_mk_config");
        LoadFunction("Z3_del_config");
        LoadFunction("Z3_mk_context");
        LoadFunction("Z3_mk_context_rc");
        LoadFunction("Z3_del_context");
        LoadFunction("Z3_update_param_value");
    }

    private static void LoadFunction(string functionName)
    {
        if (libraryHandle == IntPtr.Zero)
            throw new InvalidOperationException("Library not loaded. Call LoadLibrary first.");

        var functionPtr = NativeLibrary.GetExport(libraryHandle, functionName);
        FunctionPointers[functionName] = functionPtr;
    }

    private static IntPtr GetFunctionPointer(string functionName)
    {
        if (!FunctionPointers.TryGetValue(functionName, out var ptr))
            throw new InvalidOperationException($"Function {functionName} not loaded.");
        return ptr;
    }

    public static IntPtr Z3MkConfig()
    {
        var funcPtr = GetFunctionPointer("Z3_mk_config");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkConfigDelegate>(funcPtr);
        return func();
    }

    public static void Z3DelConfig(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_del_config");
        var func = Marshal.GetDelegateForFunctionPointer<Z3DelConfigDelegate>(funcPtr);
        func(cfg);
    }

    public static IntPtr Z3MkContext(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_context");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkContextDelegate>(funcPtr);
        return func(cfg);
    }

    public static IntPtr Z3MkContextRc(IntPtr cfg)
    {
        var funcPtr = GetFunctionPointer("Z3_mk_context_rc");
        var func = Marshal.GetDelegateForFunctionPointer<Z3MkContextRcDelegate>(funcPtr);
        return func(cfg);
    }

    public static void Z3DelContext(IntPtr ctx)
    {
        var funcPtr = GetFunctionPointer("Z3_del_context");
        var func = Marshal.GetDelegateForFunctionPointer<Z3DelContextDelegate>(funcPtr);
        func(ctx);
    }

    public static void Z3UpdateParamValue(IntPtr ctx, IntPtr paramId, IntPtr paramValue)
    {
        var funcPtr = GetFunctionPointer("Z3_update_param_value");
        var func = Marshal.GetDelegateForFunctionPointer<Z3UpdateParamValueDelegate>(funcPtr);
        func(ctx, paramId, paramValue);
    }

    private delegate IntPtr Z3MkConfigDelegate();
    private delegate void Z3DelConfigDelegate(IntPtr cfg);
    private delegate IntPtr Z3MkContextDelegate(IntPtr cfg);
    private delegate IntPtr Z3MkContextRcDelegate(IntPtr cfg);
    private delegate void Z3DelContextDelegate(IntPtr ctx);
    private delegate void Z3UpdateParamValueDelegate(IntPtr ctx, IntPtr paramId, IntPtr paramValue);
}