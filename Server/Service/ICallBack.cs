using System.ServiceModel;
using Server.Base.Tables;

namespace Server.Service
{
    public interface IChatServiceCallBack 
    {
        [OperationContract(IsOneWay = true)]
        void Error(string message); 
        [OperationContract(IsOneWay = true)]
        void Message(string message); 

        [OperationContract(IsOneWay = true)]
        void RLogin(RUser usr); 
        [OperationContract(IsOneWay = true)]
        void RLeave(); 

        [OperationContract(IsOneWay = true)]
        void RChangeOnline(RUser usr);
        [OperationContract(IsOneWay = true)]
        void RNewUser(RUser usr);

        [OperationContract(IsOneWay = true)]
        void RGroups(RMUserInGroup[] groups); 
        [OperationContract(IsOneWay = true)]
        void RLeaveGroup(RGroup group);
        [OperationContract(IsOneWay = true)]
        void RNewGroup(RMUserInGroup usrInGrp);
        [OperationContract(IsOneWay = true)]
        void RNewUsersInGroup(RMUserInGroup[] users); 
        [OperationContract(IsOneWay = true)]
        void RLeaveUserInGroup(RGroup group, RUser usr);

        [OperationContract(IsOneWay = true)]
        void RNewMessage(RUser user, RGroupMessage msg); 
    }
}
