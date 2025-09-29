using Spaceorc.Z3Wrap.Core;

namespace Z3Wrap.Tests;

[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public void Setup()
    {
        Z3.LoadLibraryAuto();
    }
}
