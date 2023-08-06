using Microsoft.Extensions.Logging;

namespace Caracal.ErrorCodes;

public static class GatewayCodes
{
    public static readonly EventId GatewaySuccess = new(10_001, "Success - Gateway");
    public static readonly EventId GatewayFailed = new(10_002, "Failed - Gateway");
    public static readonly EventId GatewayStarted = new(10_003, "Started - Gateway");
    public static readonly EventId GatewayStopped = new(10_004, "Stopped - Gateway");
}