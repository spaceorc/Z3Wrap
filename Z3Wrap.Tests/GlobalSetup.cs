using Spaceorc.Z3Wrap.Interop;

namespace Z3Wrap.Tests;

[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public void Setup()
    {
        NativeMethods.LoadLibraryAuto();
    }
}