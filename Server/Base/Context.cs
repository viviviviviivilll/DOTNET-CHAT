using System.Data.Entity;
using Server.Base.Tables;

namespace Server.Base
{
    public class Context : DbContext
    {
        public Context() : base("ChatBase") { }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupMessage> GroupsMessages { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserInGroup> UsersInGroups { get; set; }
    }
}
