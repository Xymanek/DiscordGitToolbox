using System;

namespace DiscordGitToolbox.GitHub
{
    public class GitHubRepositoryReference : IEquatable<GitHubRepositoryReference>
    {
        public readonly string Owner;
        public readonly string Name;

        public GitHubRepositoryReference(string owner, string name)
        {
            Owner = owner;
            Name = name;
        }

        #region Equality

        public override string ToString()
        {
            return $"{Owner}/{Name}";
        }

        public static bool operator== (GitHubRepositoryReference left, GitHubRepositoryReference right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return true;
            if (ReferenceEquals(left, null)) return false;
            
            return left.Equals(right);
        }

        public static bool operator !=(GitHubRepositoryReference left, GitHubRepositoryReference right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            
            return Equals((GitHubRepositoryReference) obj);
        }

        public bool Equals(GitHubRepositoryReference? other)
        {
            if (ReferenceEquals(other, null)) return false;
            
            return Owner == other.Owner && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Owner, Name);
        }

        #endregion
    }
}