using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Infrastructure.AssetManager.Data
{
    public interface IItemContainer : IDisposable
    {
        int TotalCount { get; }
        UniTask Load(Action OnLoaded, CancellationToken ct);
    }
}