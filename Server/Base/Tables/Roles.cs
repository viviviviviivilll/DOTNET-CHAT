using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Server.Base.Tables
{
    public class Role
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public virtual ICollection<UserInGroup> UsersInGroups { get; set; }
    }

    [DataContract]
    public class RRole
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }

        public RRole(Role rl)
        {
            ID = rl.ID;
            Name = rl.Name;
        }
    }
}
