﻿namespace ZetaLongPaths.Native.FileOperations;

using Interop;

internal class FileOperation : IDisposable
{
    private bool _disposed;
    private readonly IFileOperation _fileOperation;
    private readonly FileOperationProgressSink _callbackSink;
    private readonly uint _sinkCookie;

    [UsedImplicitly]
    public FileOperation() : this(null)
    {
    }

    public FileOperation(FileOperationProgressSink callbackSink) : this(callbackSink, IntPtr.Zero)
    {
    }

    public FileOperation(FileOperationProgressSink callbackSink, IntPtr ownerHandle)
    {
        _callbackSink = callbackSink;
        _fileOperation = (IFileOperation)Activator.CreateInstance(FileOperationType);

        _fileOperation.SetOperationFlags(FileOperationFlags.FOF_NOCONFIRMMKDIR);
        if (_callbackSink != null) _sinkCookie = _fileOperation.Advise(_callbackSink);
        if (ownerHandle != IntPtr.Zero) _fileOperation.SetOwnerWindow((uint)ownerHandle);
    }

    public void SetOperationFlags(FileOperationFlags operationFlags)
    {
        _fileOperation.SetOperationFlags(operationFlags);
    }

    [UsedImplicitly]
    public void CopyItem(string source, string destination, string newName)
    {
        ThrowIfDisposed();
        using var sourceItem = CreateShellItem(source);
        using var destinationItem = CreateShellItem(destination);
        _fileOperation.CopyItem(sourceItem.Item, destinationItem.Item, newName, null);
    }

    [UsedImplicitly]
    public void MoveItem(string source, string destination, string newName)
    {
        ThrowIfDisposed();
        using var sourceItem = CreateShellItem(source);
        using var destinationItem = CreateShellItem(destination);
        _fileOperation.MoveItem(sourceItem.Item, destinationItem.Item, newName, null);
    }

    [UsedImplicitly]
    public void RenameItem(string source, string newName)
    {
        ThrowIfDisposed();
        using var sourceItem = CreateShellItem(source);
        _fileOperation.RenameItem(sourceItem.Item, newName, null);
    }

    public void DeleteItem(string source)
    {
        ThrowIfDisposed();
        using var sourceItem = CreateShellItem(source);
        _fileOperation.DeleteItem(sourceItem.Item, null);
    }

    [UsedImplicitly]
    public void NewItem(string folderName, string name, FileAttributes attrs)
    {
        ThrowIfDisposed();
        using var folderItem = CreateShellItem(folderName);
        _fileOperation.NewItem(folderItem.Item, attrs, name, string.Empty, _callbackSink);
    }

    public void PerformOperations()
    {
        ThrowIfDisposed();
        _fileOperation.PerformOperations();
    }

    private void ThrowIfDisposed()
    {
        if (_disposed) throw new ObjectDisposedException(GetType().Name);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
            if (_callbackSink != null) _fileOperation.Unadvise(_sinkCookie);
            Marshal.FinalReleaseComObject(_fileOperation);
        }
    }

    private static ComReleaser<IShellItem> CreateShellItem(string path)
    {
        return new(
            (IShellItem)SHCreateItemFromParsingName(path, null, ref _shellItemGuid));
    }

    [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode, PreserveSig = false)]
    [return: MarshalAs(UnmanagedType.Interface)]
    private static extern object SHCreateItemFromParsingName(
        [MarshalAs(UnmanagedType.LPWStr)] string pszPath,
        IBindCtx pbc,
        ref Guid riid);

    private static readonly Guid ClsidFileOperation = new("3ad05575-8857-4850-9277-11b85bdb8e09");
    private static readonly Type FileOperationType = Type.GetTypeFromCLSID(ClsidFileOperation);
    private static Guid _shellItemGuid = typeof(IShellItem).GUID;
}