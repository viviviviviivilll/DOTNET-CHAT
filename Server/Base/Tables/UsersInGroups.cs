using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Server.Base.Tables
{
    public class UserInGroup
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("User")]
        public int UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Group")]
        public int GroupID { get; set; }

        [Required]
        public int FriendID { get; set; } = 0;

        [Required]
        [ForeignKey("Role")]
        public int RoleID { get; set; }

        public bool Muted { get; set; }

        public virtual Group Group { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }

    [DataContract]
    public class RUserInGroup
    {
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public int RoleID { get; set; }
        [DataMember]
        public bool Muted { get; set; }
        [DataMember]
        public int FriendID { get; set; }

        public RUserInGroup(UserInGroup usrInGrp)
        {
            UserID = usrInGrp.UserID;
            GroupID = usrInGrp.GroupID;
            RoleID = usrInGrp.RoleID;
            Muted = usrInGrp.Muted;
            FriendID = usrInGrp.FriendID;
        }
    }

    [DataContract]
    public class RMUserInGroup
    {
        [DataMember]
        public RUser User { get; set; }
        [DataMember]
        public RGroup Group { get; set; }
        [DataMember]
        public RRole Role { get; set; }
        [DataMember]
        public bool Muted { get; set; }
        [DataMember]
        public int FriendID { get; set; }

        public RMUserInGroup() { }

        public RMUserInGroup(UserInGroup usrInGrp) {
            User = new RUser(usrInGrp.User, false);
            Group = new RGroup(usrInGrp.Group);
            Role = new RRole(usrInGrp.Role);
            Muted = usrInGrp.Muted;
            FriendID = usrInGrp.FriendID;
        }
    }
}
