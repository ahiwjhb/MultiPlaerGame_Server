using Google.Protobuf.WellKnownTypes;
using MultiPlayerGame.Dao;
using Network.MessageChannel;
using Network.Protocol;


namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        private void HandleClientQueryUserInfoRequest(MessageChannel sender, UserInfoRequest request) {
            var requestResult = new RequestResult() {
                ActionCode = ActionCode.QueryUserInfo,
                IsSuccessful = false,
                Information = string.Empty
            };

            int queryID = request.UserID;
            if(UserTableDao.QueryUserIsExist(queryID) == false) {
                requestResult.Information = "用户不存在";
            }
            else {
                var (name, profilePhotoBase64) = UserTableDao.QueryUserInfo(queryID);
                requestResult.Args = Any.Pack(new UserInfo() {
                    Name = name,
                    ProfilePhotoBase64 = profilePhotoBase64
                });
                requestResult.IsSuccessful = true;
            }

            sender.SendMessageAsync(requestResult);
        }
    }
}
