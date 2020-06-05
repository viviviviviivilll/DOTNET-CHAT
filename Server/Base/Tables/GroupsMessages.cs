using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Server.Base.Tables
{

    public class GroupMessage
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        [ForeignKey("Group")]
        public int GroupID { get; set; }

        [Required]
        [StringLength(1000)]
        public string MessageSource { get; set; }

        [Required]
        public bool Deleted { get; set; }

        public virtual Group Group { get; set; }

        public virtual User User { get; set; }
    }

    [DataContract]
    public class RGroupMessage
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
        [DataMember]
        public string MessageSource { get; set; }
        [DataMember]
        public bool Deleted { get; set; }

        public RGroupMessage(GroupMessage grpMsg)
        {
            ID = grpMsg.ID;
            UserID = grpMsg.UserID;
            GroupID = grpMsg.GroupID;
            MessageSource = grpMsg.MessageSource;
            Deleted = grpMsg.Deleted;
        }
    }

    [DataContract]
    public class RMGroupMessage
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public RUser User { get; set; }
        [DataMember]
        public RGroup Group { get; set; }
        [DataMember]
        public string MessageSource { get; set; }
        [DataMember]
        public bool Deleted { get; set; }

        public RMGroupMessage(GroupMessage grpMsg) {
            ID = grpMsg.ID;
            User = new RUser(grpMsg.User, false);
            Group = new RGroup(grpMsg.Group);
            MessageSource = grpMsg.MessageSource;
            Deleted = grpMsg.Deleted;
        }
    }
}
