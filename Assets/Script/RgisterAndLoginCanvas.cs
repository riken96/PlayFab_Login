using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;

namespace SuperStar.Register.Script
{
    public class RgisterAndLoginCanvas : MonoBehaviour
    {
        public GameObject mainMenu;
        public GameObject registerMenu;
        public GameObject loginMenu;
        public GameObject accountRecoveryMenu;
        
        public TextMeshProUGUI errorTextRed;

        private string loginEmailAddress, loginPassword;


        private void Start()
        {
            loginMenu.SetActive(false);
            registerMenu.SetActive(false);

            //Setting PlayFab Titel ID
            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
            {
                PlayFabSettings.TitleId = "3C1CF";
            }
        }

        public void OnLoginButtonClick()
        {
            loginMenu.SetActive(true);
            mainMenu.SetActive(false);
            accountRecoveryMenu.SetActive(false);
        }

       public void OnRegisterButtonClick()
        {
            registerMenu.SetActive(true);
            mainMenu.SetActive(false);
            accountRecoveryMenu.SetActive(false);
        }

        public void OnQuitButtonClick()
        {
            registerMenu.SetActive(false);
            loginMenu.SetActive(false);
            accountRecoveryMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        public void OnClickForgotPasswordBtn()
        {
            accountRecoveryMenu.SetActive(true);
            loginMenu.SetActive(false);
            registerMenu.SetActive(false);
            mainMenu.SetActive(false);
        }


        // LoginMenu Email And Password Setup
        public void OnLoginScreenLoginButton()
        {

            //Checking Email Address`
            if (string.IsNullOrEmpty(loginEmailAddress))
            {
                StartCoroutine(DisplayError("Please Enter A Email"));
                return;
            }

            //Checking Password
            if (string.IsNullOrEmpty(loginPassword))
            {
                StartCoroutine(DisplayError("Please Eneter A Password"));
                return;
            }

            if (loginEmailAddress.Contains("@") == false)
            {
                StartCoroutine(DisplayError("Please Eneter A valid Email Address!"));
                return;
            }

            var loginRequest = new LoginWithEmailAddressRequest
            {
                Email = loginEmailAddress,
                Password = loginPassword,
            };

            PlayFabClientAPI.LoginWithEmailAddress(
            
                loginRequest,
                request =>
                {
                    Debug.Log("Login Was Successful");
                    StartCoroutine(DisplayError("Login Was Successful"));
                    StartCoroutine(RegisterationSuccessful());
                },
                 errorCallback =>
                 {
                     StartCoroutine(DisplayError(errorCallback.ErrorMessage));
                 }
            );

        }

        public void OnLoginScreenBackButton()
        {
            loginMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        public void OnLoginEmialInputFiledValueChange(string Value)
        {
            loginEmailAddress = Value;
        }

        public void OnLoginPasswordInputFiledValueChange(string value)
        {
            loginPassword = value;
        }

        //Register Email And Password Setup
        private string registerUserName, registerEmailAddress, registerPassword;
        public void OnRegisterViewRegisterButtonClick()
        {
            if (string.IsNullOrEmpty(registerUserName))
            {
                StartCoroutine(DisplayError("Please Eneter A User Name"));
                return;
            }

            if (string.IsNullOrEmpty(registerEmailAddress))
            {
                StartCoroutine(DisplayError("Please Eneter A Email Address"));
                return;
            }

            if (string.IsNullOrEmpty(registerPassword))
            {
                StartCoroutine(DisplayError("Please Eneter A Password"));
                return;
            }

            if (registerPassword.Length < 6)
            {
                StartCoroutine(DisplayError("Please Eneter A Password With More than 6 Characters"));
                return;
            }

            if (registerUserName.Length < 6)
            {
                StartCoroutine(DisplayError("Please Eneter A UserName With More than 6 Characters"));
                return;
            }

            if (registerEmailAddress.Contains("@") == false)
            {
                StartCoroutine(DisplayError("Please Eneter A valid Email Address!"));
                return;
            }

            //Request For Player Registration
            var registerRequest = new RegisterPlayFabUserRequest
            {
                Username = registerUserName,
                DisplayName = registerUserName,
                Email = registerEmailAddress,
                Password = registerPassword,
            };

            // Sending Registration Request 
            PlayFabClientAPI.RegisterPlayFabUser(
                registerRequest,
                    resultCallback =>
                    {
                      Debug.Log("Regitration Was Successful");
                      SetupWinLossDateOnRegistration();
                      UpdateContactEmail();
                    },
                    errorCallback =>
                    {
                      Debug.Log("Failed to register the player");
                      StartCoroutine(DisplayError(errorCallback.ErrorMessage));
                    }
                );
        }


        private void SetupWinLossDateOnRegistration()
        {
            var executCloudScriptRequest = new ExecuteCloudScriptRequest
            {
                FunctionName = "setupWinLossData",
            };

            PlayFabClientAPI.ExecuteCloudScript(

                executCloudScriptRequest,

                resultCallback =>
                {
                    Debug.Log("DOne");
                    string resultWinLossData = Newtonsoft.Json.JsonConvert.SerializeObject(resultCallback.FunctionResult);
                    PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).
                    SerializeObject(resultCallback.FunctionResult);
                        
                    Debug.Log(resultWinLossData);
                    StartCoroutine(DisplayError("Regitration Was Successful"));
                    StartCoroutine(RegisterationSuccessful());
                },
                errorCallback => { Debug.Log(errorCallback.ErrorMessage); }
            );
        }
        public void OnRegisterViewBackButtonClick()
        {
            registerMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        public void OnRegisterUserNameInputFiledValueChange(string Value)
        {
            registerUserName = Value;
        }

        public void OnRegisterEmialInputFiledValueChange(string Value)
        {
            registerEmailAddress = Value;
        }

        public void OnRegisterPasswordInputFiledValueChange(string value)
        {
            registerPassword = value;
        }

        public  IEnumerator DisplayError(string Message)
        {
            errorTextRed.gameObject.SetActive(true);
            errorTextRed.text = Message;
            yield return new WaitForSeconds(3f);
            errorTextRed.gameObject.SetActive(false);
        }

        public IEnumerator RegisterationSuccessful()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        // Update Contact Email Address Functions
        public void UpdateContactEmail()
        {
            var updateContactEmailAddRequest = new AddOrUpdateContactEmailRequest
            {
                EmailAddress = registerEmailAddress,
            };

            PlayFabClientAPI.AddOrUpdateContactEmail
            (
                updateContactEmailAddRequest,
                resultCallback =>
                {
                    Debug.Log("Contact email update successfully");
                },
                errorCallback =>
                {
                    Debug.Log(errorCallback.ErrorMessage);
                }
            );
        }

        //Account Recovery Email And Password

        private string accountRecoveryEmail;

        public void OnAccountRecoveryEmailAddressInputFieldValueChange(string value)
        {
            accountRecoveryEmail = value;
        }

        public void SetupAccountRecoverySendEmail()
        {
            if (string.IsNullOrEmpty(accountRecoveryEmail))
            {
                StartCoroutine(DisplayError("Please Enter A Email"));
                return;
            }

            if (accountRecoveryEmail.Contains("@") == false)
            {
                StartCoroutine(DisplayError("Please Eneter A valid Email Address!"));
                return;
            }


            var accountRecoveryRequest = new SendAccountRecoveryEmailRequest
            {
                Email = accountRecoveryEmail,
                TitleId = "3C1CF"
            };

            PlayFabClientAPI.SendAccountRecoveryEmail(

                accountRecoveryRequest,
                request =>
                {
                    StartCoroutine(DisplaySuccessMessage());

                },
                errorCallback =>
                {
                    Debug.Log(errorCallback.ErrorMessage);
                    StartCoroutine(DisplayError(errorCallback.ErrorMessage));
                }
            );
        }

        private IEnumerator DisplaySuccessMessage()
        {
            errorTextRed.gameObject.SetActive(true);
            errorTextRed.text = "Email to Reset Password Has Been Sent! please check your email for resetting your password";
            yield return new WaitForSeconds(4f);
            errorTextRed.gameObject.SetActive(false);
        }
    }

    
}

