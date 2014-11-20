// <copyright file="Plugin.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using BauCore;

    public static class Plugin
    {
        public static ITaskBuilder<NuGetTask> NuGet(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<NuGetTask>(builder, name);
        }
    }
}
