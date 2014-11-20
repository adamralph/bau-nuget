// <copyright file="Plugin.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using BauCore;

    public static class Plugin
    {
        public static ITaskBuilder<NuGetPack> NuGetPack(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<NuGetPack>(builder, name);
        }

        public static ITaskBuilder<NuGetPush> NuGetPush(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<NuGetPush>(builder, name);
        }

        public static ITaskBuilder<NuGetRestore> NuGetRestore(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<NuGetRestore>(builder, name);
        }
    }
}
