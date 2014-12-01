// <copyright file="Plugin.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using BauCore;

    public static class Plugin
    {
        public static ITaskBuilder<RestoreTask> NuGetRestore(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<RestoreTask>(builder, name);
        }

        public static ITaskBuilder<PackTask> NuGetPack(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<PackTask>(builder, name);
        }

        public static ITaskBuilder<PushTask> NuGetPush(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<PushTask>(builder, name);
        }
    }
}
