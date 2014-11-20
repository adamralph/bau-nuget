// <copyright file="NuGetCliBauTaskBase{TRequest}.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BauCore;

    public abstract class NuGetCliBauTaskBase<TRequest> : NuGetCliBauTaskBase where TRequest : NuGetCliCommandRequestBase, new()
    {
        protected NuGetCliBauTaskBase()
        {
            this.Request = new TRequest();
        }

        public TRequest Request { get; private set; }

        protected override void OnActionsExecuted()
        {
            if (this.UseCommandLine)
            {
                this.ExecuteUsingCommandLine();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        protected virtual NuGetBasicCommandLineRunner ExecuteUsingCommandLine()
        {
            var runner = new NuGetBasicCommandLineRunner()
                .WithWorkingDirectory(this.WorkingDirectory);
            runner
                .CreateProcessStartInfo(this.Request)
                .Run();
            return runner;
        }
    }
}
