using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Linq;
using Server.Service;
using System;

namespace Server.Base.Tables
{
    public class Group
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        public bool Deleted { get; set; }

        public virtual ICollection<GroupMessage> GroupsMessages { get; set; }
        public virtual ICollection<UserInGroup> UsersInGroups { get; set; }
    }

    [DataContract]
    public class RGroup
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool Deleted { get; set; }

        public RGroup(Group grp) {
            ID = grp.ID;
            Name = grp.Name;
            Deleted = grp.Deleted;
        }
    }
}
