using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiBanter.Shared.Models;

    public class AppConfig
    {
        public string AllowedHosts { get; set; }
        public bool ApplyMigrations { get; set; } = false;
        public string DefaultMigrationPassword { get; set; }
        public string AssetsPath { get; set; }
        public string BaseStoragePath { get; set; }
        public string DataStoragePath { get; set; }
        public EndpointOption Endpoints { get; set; }
        public JwtSettings TenantJwtSettings { get; set; }
        public JwtSettings MasterJwtSettings { get; set; }
        public Logging Logging { get; set; }
        public DatabaseOptions DatabaseOptions { get; set; }
        public HangfireOptions HangfireOptions { get; set; }
        public BasicCredential BasicCredentials { get; set; }
        public RedisOptions RedisOptions { get; set; }
        public HealthCheckOptions HealthCheckOptions { get; set; }
        public DataProtectionKey DataProtectionKey { get; set; } = new();

    }

    public class AppConfiguration
    {
        public EndpointOption Endpoints { get; set; }
    }
    public class HealthCheckOptions
    {
        public int MaxMemoryUsage { get; set; }
        public int MaxCpuUsage { get; set; }
        public int DiskThresholdMB { get; set; }
        public string DiskDrivePath { get; set; }

    }

    public class DataProtectionKey
    {
        public string? Key { get; set; }
        public string? Certificate { get; set; }
        public string? ApplicationName { get; set; }

    }

    public class RedisOptions
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }

    }


    public class DatabaseOptions
    {
        public string ConnectionString { get; set; }
        public int MaxRetryCount { get; set; }
        public int CommandTimeout { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
        public bool EnableDetailedErrors { get; set; }
        public bool EnableThreadSafetyChecks { get; set; }

    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
    }


    public class EndpointOption
    {
        public string Api { get; set; }
        public string Portal { get; set; }
    }

    public class HangfireOptions
    {
        public int MaximumJobsFailed { get; set; }
        public string ConnectionString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class BasicCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class JwtSettings
    {
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenExpiryInMinutes { get; set; }
        public int RefreshTokenExpiryInDay { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
    }

