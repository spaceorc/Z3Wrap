using Spaceorc.Z3Wrap.Core.Interop;

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
