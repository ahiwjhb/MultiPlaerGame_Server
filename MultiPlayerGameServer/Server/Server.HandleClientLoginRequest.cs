using Google.Protobuf.WellKnownTypes;
using MultiPlayerGame.Dao;
using Network.MessageChannel;
using Network.Protocol;

namespace MultiPlayerGame.Server
{
    public partial class Server
    {
        private void HandleClientLoginRequest(MessageChannel sender, LoginRequest request) {
            string username = request.Username;
            string inputPassword = request.Password;

            var requestResult = new RequestResult() {
                ActionCode = ActionCode.Login,
                IsSuccessful = false,
                Information = string.Empty
            };

            try {
                int? userID = UserTableDao.QueryIDByUsername(username);
                if (userID == null) {
                    requestResult.Information = "用户名不存在";
                }
                else {
                    string password = UserTableDao.QueryUserPassword(userID.Value);
                    if (inputPassword != password) {
                        requestResult.Information = "密码错误";
                    }
                    else {
                        requestResult.IsSuccessful = true;
                        requestResult.Information = "登录成功";
                        requestResult.Args = Any.Pack(new Int32Value() { Value = userID.Value });
                    }
                }
            }
            catch (Exception e) {
                requestResult.Information = e.Message;
            }

            sender.SendMessageAsync(requestResult);
        }
    }
}
