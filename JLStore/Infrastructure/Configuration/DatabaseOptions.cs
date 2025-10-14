namespace JLStore.Infrastructure.Configuration;

public sealed class DatabaseOptions
{
    // "sqlserver" (default) o "postgres" se/quando lo abiliterai
    public string Provider { get; set; } = "sqlserver";

    // Chiave "ConnectionStrings:Default" (fallback)
    public string? Default { get; set; }

    // Eventuali chiavi aggiuntive (per estensioni future)
    public string? SqlServer { get; set; }
    public string? Postgres { get; set; }
}
