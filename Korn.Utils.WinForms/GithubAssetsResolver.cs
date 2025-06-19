using Korn.Logger;
using Korn.Utils.GithubExplorer;

namespace Korn.Utils.WinForms;
public class GithubAssetsResolver
{
    public GithubAssetsResolver(RepositoryID repository) : this(new GithubClient(), repository) { }

    public GithubAssetsResolver(GithubClient client, RepositoryID repository)
    {
        Client = client;
        Repository = repository;
        tempFolder = Path.Combine(Path.GetTempPath(), $"Cache-{repository.Name}");
    }

    public readonly GithubClient Client;
    public readonly RepositoryID Repository;
    readonly string tempFolder;

    Dictionary<string, List<RepositoryEntryJson>> cachedEntries = [];
    public byte[] Resolve(string path)
    {
        path = path.Replace("/", "\\");

        var cachedEntry = ResolveCachedEntry(path);
        if (cachedEntry is not null)
            return cachedEntry;

        path = path.Replace('\\', '/');

        var pathParts = path.Split('/');

        var assetDirectry = pathParts.Length > 1 ? string.Join('/', pathParts[0..^1]) : "";
        var assetName = pathParts[^1];

        var entries = ResolveEntries(assetDirectry);
        var entry = entries.FirstOrDefault(e => e.Name == assetName);

        if (entry is null)
            throw new KornException($"GithubAssetsResolver->Resolve: Cannot resolve asset with path {path} for repository {Repository}");

        var bytes = Client.DownloadAsset(entry);
        CacheEntry(path, bytes);

        return bytes;
    }

    List<RepositoryEntryJson> ResolveEntries(string path)
    {
        if (cachedEntries.ContainsKey(path))
            return cachedEntries[path];

        var entries = Client.GetRepositoryEntries(Repository, path);
        cachedEntries[path] = entries;

        return entries;
    }

    byte[]? ResolveCachedEntry(string path)
    {
        path = Path.Combine(tempFolder, path);

        if (!File.Exists(path))
            return null;

        var bytes = File.ReadAllBytes(path);
        return bytes;
    } 

    void CacheEntry(string path, byte[] bytes)
    {
        path = Path.Combine(tempFolder, path);

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllBytes(path, bytes);
    }
}