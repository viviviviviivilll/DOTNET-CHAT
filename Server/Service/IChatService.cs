using System.Collections.Generic;
using System.ServiceModel;
using Server.Base.Tables;

namespace Server.Service
{
    public enum TypeGetUsers {
        All = 0, 
        OnlyInGroup, 
        OnlyOutInGroup,
    }

    [ServiceContract (CallbackContract = typeof(IChatServiceCallBack))]
    public interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void Register(string Login, string Email, string Password, string repeatPassword); // Регістрація
        [OperationContract(IsOneWay = true)]
        void Login(string Login, string Password);
        [OperationContract(IsOneWay = true)]
        void Leave(); 

        [OperationContract(IsOneWay = true)]
        void SendMessage(int groupID, string message);

        [OperationContract(IsOneWay = true)]
        void GetMyGroups();
        [OperationContract(IsOneWay = true)]
        void CreateGroup(string Name, int[] IDs);
        [OperationContract(IsOneWay = true)]
        void LeaveGroup(int ID); 
        [OperationContract(IsOneWay = true)]
        void AddUsersToGroup(int ID, int[] IDs); 
        [OperationContract(IsOneWay = true)]
        void RemoveUserFromGroup(int groupID, int userID);
        [OperationContract(IsOneWay = true)]
        void EditRoleUserInGroup(int groupID, int userID, int roleID);
        [OperationContract(IsOneWay = true)]
        void MuteUserInGroup(int groupID, int userID);


        [OperationContract]
        RUser[] GetUsers(int offset, int count); 
        [OperationContract]
        Dictionary<RUser, RGroupMessage> GetMessages(int groupID, bool reverced, int count, int offset);
        [OperationContract]
        RMUserInGroup[] GetUsersInGroup(int groupID, int offset, int count); 
        [OperationContract]
        RUser[] GetUsersOutGroup(int groupID, int offset, int count); 

        [OperationContract]
        int GetCountUsers(); 
        [OperationContract]
        int GetCountUsersInGroup(int groupID);
        [OperationContract]
        int GetCountMessages(int groupID);
    }
   

}
