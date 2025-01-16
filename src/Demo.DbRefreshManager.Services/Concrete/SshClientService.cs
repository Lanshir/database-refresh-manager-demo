using Demo.DbRefreshManager.Services.Abstract;
using Demo.DbRefreshManager.Services.Models.SshServiceModels;
using Renci.SshNet;

namespace Demo.DbRefreshManager.Services.Concrete;

/// <inheritdoc cref="ISshClientService" />
public class SshClientService : ISshClientService
{
    public bool IsConnected { get; private set; }

    /// <inheritdoc cref="SshClient" />
    private SshClient? _client;

    public void Connect(string host, string user, string pasword)
    {
        // Проверка существующего подключения.
        if (_client?.IsConnected ?? false)
        {
            return;
        }

        _client = new SshClient(host, user, pasword);
        _client.Connect();

        IsConnected = true;
    }

    public async Task ConnectAsync(string host, string user, string pasword)
    {
        // Проверка существующего подключения.
        if (_client?.IsConnected ?? false)
        {
            return;
        }

        _client = new SshClient(host, user, pasword);
        await _client.ConnectAsync(CancellationToken.None);

        IsConnected = true;
    }

    public void Disconnect() => _client?.Disconnect();

    public SshCommandResult RunCommand(string commandText)
    {
        if (_client == null || !_client.IsConnected)
        {
            throw new Exception("Ssh client not connected.");
        }

        var cmd = _client.RunCommand(commandText);

        var result = new SshCommandResult
        {
            IsSuccess = cmd.ExitStatus == 0,
            Code = cmd.ExitStatus ?? 0,
            Error = cmd.Error,
            Result = cmd.Result
        };

        return result;
    }

    public void Dispose()
    {
        if (_client != null)
        {
            if (_client.IsConnected)
            {
                _client.Disconnect();
            }

            _client.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}
