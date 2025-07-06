// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public interface IHost : IDisposable
    {
        IServiceProvider Services { get; }
        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}

namespace Microsoft.Extensions.Hosting
{
    public static class HostingAbstractionsHostExtensions
    {
        public static void Start(this IHost host)
        {
            host.StartAsync().GetAwaiter().GetResult();
        }

        public static async Task StopAsync(this IHost host, TimeSpan timeout)
        {
            using CancellationTokenSource cts = new CancellationTokenSource(timeout);
            await host.StopAsync(cts.Token).ConfigureAwait(false);
        }

        public static void WaitForShutdown(this IHost host)
        {
            host.WaitForShutdownAsync().GetAwaiter().GetResult();
        }

        public static void Run(this IHost host)
        {
            host.RunAsync().GetAwaiter().GetResult();
        }

        public static async Task RunAsync(this IHost host, CancellationToken token = default)
        {
            try
            {
                await host.StartAsync(token).ConfigureAwait(false);

                await host.WaitForShutdownAsync(token).ConfigureAwait(false);
            }
            finally
            {
                if (host is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                }
                else
                {
                    host.Dispose();
                }
            }
        }

        public static async Task WaitForShutdownAsync(this IHost host, CancellationToken token = default)
        {
            IHostApplicationLifetime applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

            token.Register(state =>
            {
                ((IHostApplicationLifetime)state!).StopApplication();
            },
            applicationLifetime);

#if NET8_0_OR_GREATER
            await Task.Delay(Timeout.Infinite, applicationLifetime.ApplicationStopping).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
#else
            var waitForStop = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);
            applicationLifetime.ApplicationStopping.Register(obj =>
            {
                var tcs = (TaskCompletionSource<object?>)obj!;
                tcs.TrySetResult(null);
            }, waitForStop);

            await waitForStop.Task.ConfigureAwait(false);
#endif

            // Host will use its default ShutdownTimeout if none is specified.
            // The cancellation token may have been triggered to unblock waitForStop. Don't pass it here because that would trigger an abortive shutdown.
            await host.StopAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
