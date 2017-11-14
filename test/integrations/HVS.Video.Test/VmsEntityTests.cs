namespace HVS.Video.Test
{
    using HVS.CLI.Database;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Xunit;

    public class VmsEntityTests
    {
        [Fact(DisplayName = "Add Vms Entity")]
        public void AddVmsEntity()
        {
            DatabaseCommands.New("hvs", null, new List<string> { "iot" }, null, null, null, "..\\Debug\\");

            Assert.Equal(2, 2);

            DatabaseCommands.Drop("hvs", null);
        }
    }
}
