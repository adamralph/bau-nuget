// <copyright file="Plugin.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using BauCore;

    public static class Plugin
    {
        public static ITaskBuilder<Restore> NuGetRestore(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<Restore>(builder, name);
        }

        public static ITaskBuilder<Pack> NuGetPack(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<Pack>(builder, name);
        }

        public static ITaskBuilder<Push> NuGetPush(this ITaskBuilder builder, string name = null)
        {
            return new TaskBuilder<Push>(builder, name);
        }
    }
}
